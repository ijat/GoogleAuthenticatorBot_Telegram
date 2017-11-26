using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace GAuthBotDevBot.Language
{
    class base_language
    {
        public string first_message { get; set; }
        public string log_init { get; set; }
        public string app_starting { get; set; }
        public string callback_query_user_not_found { get; set; }
    }

    class Translator
    {
        private static string current_language = "en_us";
        public static base_language get { get; set; }

        public static void Init()
        {
            using (StreamReader sr = new StreamReader($@"Language\{current_language}.txt"))
            {
                string lang_str = sr.ReadToEnd();
                get = JsonConvert.DeserializeObject<base_language>(lang_str);
            }
        }

        public static void set_language(string lang)
        {
            current_language = lang;
        }

    }
}
