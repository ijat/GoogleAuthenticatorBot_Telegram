using System;
using System.Collections.Generic;
using LiteDB;
using Telegram.Bot.Types;

namespace GAuthBotDevBot.Database.Model
{
    public enum Page
    {
        Main,
        Get,
        Add,
        Remove,
        Code
    }
    class User
    {
        [BsonId]
        public Int64 _id
        {
            get
            {
                return uid;
            }
            set
            {
                uid = value;
            }
        }

        public Int64 uid { get; set; }
        public DateTime date_joined { get; set; }
        public Dictionary<string, GAuthAcc> accounts { get; set; }
        public int messageid { get; set; }
        public Page page { get; set; } 

        public User(Int64 uid, DateTime date)
        {
            this.uid = uid;
            date_joined = date;
            accounts = new Dictionary<string, GAuthAcc>();
        }

        public User(Int64 uid, DateTime date, Dictionary<string, GAuthAcc> lt)
        {
            this.uid = uid;
            date_joined = date;
            accounts = lt;
        }

        public User()
        { }
    }
}
