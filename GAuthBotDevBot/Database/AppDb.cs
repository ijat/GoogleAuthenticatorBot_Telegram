using LiteDB;
using System.IO;
using GAuthBotDevBot.Database.Model;
using System;
using System.Collections.Generic;

namespace GAuthBotDevBot.Database
{
    class AppDb
    {
        private LiteDatabase _db;
        private LiteCollection<User> _col;

        private static AppDb _instance;
        public static AppDb Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppDb();
                }
                return _instance;
            }
        }

        public AppDb()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

            _db = new LiteDatabase(@"Data\GAuthBot");
            using (_db)
                _col = _db.GetCollection<User>("data");

            if (_instance == null)
                _instance = this;
        }

        public User Insert(string uid, DateTime dt)
        {
            try
            {
                User u = new User(uid, dt, new Dictionary<string, GAuthAcc>());
                _col.Insert(u);
                return u;
            }
            catch (Exception ex)
            {
                Program.log.Error(ex.ToString());
                return null;
            }
        }

        public void Insert(string uid, DateTime dt, Dictionary<string, GAuthAcc> acc)
        {
            try
            {
                _col.Insert(new User(uid, dt, acc));
            }
            catch (Exception ex)
            {
                Program.log.Error(ex.ToString());
            }
        }

        public User GetUserbyUID(string uid)
        {
            try
            {
                User u = _col.FindOne(o => o.uid == uid);
                return u;
            }
            catch (Exception ex)
            {
                Program.log.Error(ex.ToString());
                return null;
            }
        }

        public Dictionary<string, GAuthAcc> GetAccbyUID(string uid)
        {
            try
            {
                Dictionary<string, GAuthAcc> a = _col.FindOne(x => x.uid == uid).accounts;
                return a;
            }
            catch (Exception ex)
            {
                Program.log.Error(ex.ToString());
                return null;
            }
        }
    }
}
