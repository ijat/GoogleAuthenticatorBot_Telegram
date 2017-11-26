using System;
using System.Collections.Generic;
using LiteDB;
using Telegram.Bot.Types;

namespace GAuthBotDevBot.Database.Model
{
    class User
    {
        [BsonId]
        public string _id
        {
            get
            {
                return uid;
            }
            set
            {
                uid = value.ToString();
            }
        }

        public ChatId chatid { get; set; }
        public string uid { get; set; }
        public DateTime date_joined { get; set; }
        public Dictionary<string, GAuthAcc> accounts { get; set; }
        public string messageid { get; set; }

        public User(string uid, DateTime date)
        {
            this.uid = uid;
            date_joined = date;
            accounts = new Dictionary<string, GAuthAcc>();
        }

        public User(string uid, DateTime date, ChatId cid)
        {
            this.uid = uid;
            date_joined = date;
            chatid = cid;
            accounts = new Dictionary<string, GAuthAcc>();
        }

        public User(string uid, DateTime date, Dictionary<string, GAuthAcc> lt)
        {
            this.uid = uid;
            date_joined = date;
            accounts = lt;
        }

        public User()
        { }
    }
}
