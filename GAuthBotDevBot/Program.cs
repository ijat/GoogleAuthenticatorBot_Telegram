using System;
using Serilog;
using GAuthBotDevBot.Language;
using GAuthBotDevBot.Database;
using GAuthBotDevBot.Database.Model;
using GAuthBotDevBot.Keyboard;
using GAuthBotDevBot.GoogleAuthAPI;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using System.Text.RegularExpressions;

namespace GAuthBotDevBot
{
    class Program
    {
        private static readonly TelegramBotClient bot = new TelegramBotClient("451009981:AAGic9MJdnogIi5mOIxl4hcPCSoNICfS3Cw");
        public static Serilog.Core.Logger log;
        public static AppDb db;

        static void Main(string[] args)
        {
            InitApp();
            InitLanguage();
            InitLog();
            InitDb();

            bot.OnMessage += Bot_OnMessage;
            bot.OnCallbackQuery += Bot_OnCallbackQuery;

            string a = new API(API.OTPType.TOTP, "sfsdf").Now();

            var me = bot.GetMeAsync().Result;
            Console.WriteLine(me.Username);

            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        private static void Bot_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            Task.Run(() =>
            {
                var x = db.GetUserByUID(e.CallbackQuery.From.Id);
                if (x != null)
                {
                    switch (e.CallbackQuery.Data)
                    {
                        case "get":
                            x.page = Page.Get;
                            var k = GAuthBotDevBot.Keyboard.Generator.Create(x, backPage: Page.Main);
                            bot.EditMessageTextAsync(x.uid, x.messageid, Translator.get.first_message, replyMarkup: k);
                            break;
                        case "add":
                            x.page = Page.Add;
                            bot.EditMessageTextAsync(x.uid, x.messageid, "Send your Google Auth secret code using this format\n\n<code>[Issuer/Website] [CODE]\n\nEg: gugel.com 131512312SA</code>", parseMode: ParseMode.Html, replyMarkup: Layout.key2);
                            break;
                        case "rem":
                            x.page = Page.Remove;
                            var kk = GAuthBotDevBot.Keyboard.Generator.Create(x, backPage: Page.Main);
                            bot.EditMessageTextAsync(x.uid, x.messageid, Translator.get.first_message, replyMarkup: kk);
                            break;
                        case "cancel":
                            bot.EditMessageTextAsync(x.uid, x.messageid, Translator.get.first_message, replyMarkup: Keyboard.Layout.key1);
                            x.page = Page.Main;
                            break;
                        default:
                            switch (x.page)
                            {
                                case Page.Get:
                                    if (e.CallbackQuery.Data == Page.Main.ToString())
                                    {
                                        bot.EditMessageTextAsync(x.uid, x.messageid, Translator.get.first_message, replyMarkup: Keyboard.Layout.key1);
                                        x.page = Page.Main;
                                    }
                                    else if (x.accounts.ContainsKey(e.CallbackQuery.Data)) 
                                    {
                                        bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, new API(API.OTPType.TOTP, x.accounts[e.CallbackQuery.Data].secret_code).Now(), true);
                                    }
                                    break;
                                case Page.Remove:
                                    if (x.accounts.ContainsKey(e.CallbackQuery.Data))
                                    {
                                        x.accounts.Remove(e.CallbackQuery.Data);
                                        bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "Deleted!", true);
                                    }
                                    bot.EditMessageTextAsync(x.uid, x.messageid, Translator.get.first_message, replyMarkup: Keyboard.Layout.key1);
                                    x.page = Page.Main;
                                    break;

                            }
                            break;
                    }

                    db.Update(x);
                }
                else
                {
                    bot.SendTextMessageAsync(e.CallbackQuery.Message.From.Id, Translator.get.callback_query_user_not_found);
                }

            });
        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Task.Run(() =>
            {
                bot.SendChatActionAsync(e.Message.Chat.Id, ChatAction.Typing);
                if (e.Message == null || e.Message.Type != MessageType.TextMessage) return;

                // user first time start this bot, so lets register it in database
                if (e.Message.Text.Contains("/start"))
                {
                    // recheck in database if exists
                    var x = db.GetUserByUID(e.Message.From.Id);
                    if (x == null)
                    {
                        db.Insert(e.Message.From.Id, DateTime.Now);

                    }

                    SendFirstMessage(x);
                    Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(x));
                }
                else
                {
                    var x = db.GetUserByUID(e.Message.From.Id);
                    if (x != null)
                    {
                        switch (x.page)
                        {
                            case Page.Add:
                                string[] strs = e.Message.Text.Split(" ");
                                try
                                {
                                    bool a = Regex.IsMatch(strs[0], @"^[a-zA-Z0-9_]+$");
                                    bool b = Regex.IsMatch(strs[1], @"^[a-zA-Z0-9_]+$");

                                    if (a && b)
                                    {
                                        GAuthAcc newAcc = new GAuthAcc(issuer: strs[0], secret: strs[1]);
                                        x.accounts[newAcc.issuer] = newAcc;
                                        x.page = Page.Main;
                                        db.Update(x);
                                        bot.EditMessageTextAsync(x.uid, x.messageid, Translator.get.first_message, replyMarkup: Keyboard.Layout.key1);
                                        bot.SendTextMessageAsync(x.uid, "Successfully added. Please remove any message containing secret code, OTPs, and QRCode for security reasons.");
                                    }
                                    else
                                        bot.SendTextMessageAsync(x.uid, "Invalid input. Please try again. ;)");
                                }
                                catch
                                {
                                    bot.SendTextMessageAsync(x.uid, "Invalid input. Please try again. ;)");
                                }
                                break;
                            case Page.Get:
                                break;
                            case Page.Remove:
                                break;
                            case Page.Main:
                            default:
                                bot.SendTextMessageAsync(x.uid, "Please use this message. If it is not available anymore, please send /start.", replyToMessageId: x.messageid);
                                break;
                        }
                    }
                    else
                    {
                        bot.SendTextMessageAsync(x.uid, Translator.get.callback_query_user_not_found);
                    }
                }
            });
        }

        private static void SendFirstMessage(User x)
        {
            var m = bot.SendTextMessageAsync(x.uid, Translator.get.first_message, replyMarkup: Keyboard.Layout.key1).Result;
            x.messageid = m.MessageId;
            x.page = Page.Main;
            db.Update(x);
        }

        private static void InitApp()
        {
            Console.Title = "GAuthBot";
        }

        private static void InitLanguage()
        {
            // need some code to read other conf file later for language selection... for now just english language
            Translator.Init();
        }

        private static void InitLog()
        {
            log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Async(a => a.RollingFile("log/log-{Date}.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose))
                .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
                .CreateLogger();
            log.Verbose($"▉▉▉ {Translator.get.log_init} ({DateTime.Now.ToString()}) ▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉");
            log.Information(Translator.get.app_starting);
        }

        private static void InitDb()
        {
            db = new AppDb();
        }
    }
}
