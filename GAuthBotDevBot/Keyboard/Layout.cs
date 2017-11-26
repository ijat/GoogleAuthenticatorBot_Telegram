using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineKeyboardButtons;
using Telegram.Bot.Types.ReplyMarkups;

namespace GAuthBotDevBot.Keyboard
{
    class Layout
    {
        public static readonly InlineKeyboardMarkup key1 = new InlineKeyboardMarkup(new[]
                    {
                        new[] // first row
                        {
                            new InlineKeyboardCallbackButton("Get Code", "get")
                        },
                        new[] // first row
                        {
                            new InlineKeyboardCallbackButton("Add Account", "add")
                        },
                        new[] // second row
                        {
                            new InlineKeyboardCallbackButton("Remove Account", "rem")
                        }
                    });
    }
}
