using System;
using Serilog;
using GAuthBotDevBot.Database;
using GAuthBotDevBot.Database.Model;
using System.Threading.Tasks;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Types.Enums;
using GAuthBotDevBot.Keyboard;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;
using GAuthBotDevBot.Language;

namespace GAuthBotDevBot
{
    class Program
    {
        private static readonly TelegramBotClient bot = new TelegramBotClient("451009981:AAFnutTPEyzXUGgWQtbNYSW7rA8x99RFaBk");
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
                Console.WriteLine(e.CallbackQuery.Data);
                bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, "OK");
            });

        }

        static int lastm;
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
                    var x = db.GetUserByUID(e.Message.From.Id.ToString());
                    if (x == null)
                        db.Insert(e.Message.From.Id.ToString(), DateTime.Now);

                    SendFirstMessage(x);
                }
                


                if (e.Message.Text == "delete")
                {
                    var m = bot.SendTextMessageAsync(e.Message.Chat.Id, "Deleting message in 2s...").Result;
                    Console.WriteLine(m.ToString());
                    Task.Delay(2000);
                    var a = bot.DeleteMessageAsync(m.Chat.Id, m.MessageId).Result; // can delete self message
                    var b = bot.DeleteMessageAsync(m.Chat.Id, e.Message.MessageId).Result; // cant delete user message
                }
                else if (e.Message.Text == "k")
                {
                    //var x = bot.SendTextMessageAsync(e.Message.Chat.Id, "Choose", replyMarkup: keyboard).Result;
                    //lastm = x.MessageId;
                }
                else if (e.Message.Text == "e")
                {
                    var z = bot.EditMessageTextAsync(e.Message.Chat.Id, lastm, "edited").Result;
                }
                else
                    bot.SendTextMessageAsync(e.Message.Chat.Id, "Hello");
            });
        }

        private static void SendFirstMessage(User x)
        {
            //bot.EditMessageTextAsync()

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
