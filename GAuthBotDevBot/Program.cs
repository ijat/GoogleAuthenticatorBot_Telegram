using System;
using Serilog;
using GAuthBotDevBot.Database;
using GAuthBotDevBot.Database.Model;
using System.Threading.Tasks;
using Telegram.Bot;

namespace GAuthBotDevBot
{
    class Program
    {
        private static readonly TelegramBotClient bot = new TelegramBotClient("451009981:AAFnutTPEyzXUGgWQtbNYSW7rA8x99RFaBk");
        public static Serilog.Core.Logger log;
        public static AppDb db;

        static void Main(string[] args)
        {
            Init();
            InitLog();
            InitDb();

            bot.OnMessage += Bot_OnMessage;

            var me = bot.GetMeAsync().Result;
            Console.WriteLine(me.Username);

            bot.StartReceiving();
            Console.ReadLine();
            bot.StopReceiving();
        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Task.Run(() => {
                bot.SendTextMessageAsync(e.Message.Chat.Id, "Hello");
            });
        }

        private static void Init()
        {
            Console.Title = "GAuthBot";
             
        }

        private static void InitLog()
        {
            log = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Async(a => a.RollingFile("log/log-{Date}.txt", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Verbose))
                .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug)
                .CreateLogger();
            log.Verbose($"▉▉▉ Log Initialized ({DateTime.Now.ToString()}) ▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉▉");
            log.Information("Starting application...");
        }

        private static void InitDb()
        {
            db = new AppDb();
        }
    }
}
