using GAuthBotDevBot.Database.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using System.Linq;
using Telegram.Bot.Types.InlineKeyboardButtons;

namespace GAuthBotDevBot.Keyboard
{
    class Generator
    {
        public static InlineKeyboardMarkup Create(User x, Page backPage = Page.Main)
        {
            var z = Layout.key1;
            var y = x.accounts.OrderBy(o => o.Key).ToList();
            InlineKeyboardButton[][] a = new InlineKeyboardButton[y.Count+1][];
            for (int i=0; i<y.Count; i++)
            {
                InlineKeyboardButton[] b = new InlineKeyboardButton[1];
                b[0] = new InlineKeyboardCallbackButton(y[i].Key, y[i].Key);
                a[i] = b;
                //k.InlineKeyboard = new[] { new InlineKeyboardCallbackButton("Get Code", "get") };
            }

            InlineKeyboardButton[] c = new InlineKeyboardButton[1];
            c[0] = new InlineKeyboardCallbackButton("< Back", backPage.ToString());
            a[y.Count] = c;

            return new InlineKeyboardMarkup(a);
        }
    }
}
