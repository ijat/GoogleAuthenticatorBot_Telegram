using System;
using System.Collections.Generic;
using LiteDB;

namespace GAuthBotDevBot.Database.Model
{
    class User
    {
        [BsonId]
        public Int64 _id
        {
            get
            {
                return Int64.Parse(uid);
            }
            set
            {
                uid = value.ToString();
            }
        }

        public string uid { get; set; }
        public DateTime date_joined { get; set; }
        public Dictionary<string, GAuthAcc> accounts { get; set; }

        public User(string uid, DateTime date)
        {
            this.uid = uid;
            date_joined = date;
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
