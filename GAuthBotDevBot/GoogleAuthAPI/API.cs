using System.Diagnostics;
using System.IO;

namespace GAuthBotDevBot.GoogleAuthAPI
{
    class API
    {
        public static string PyPath = @"C:\Program Files\Python36\python.exe";
        public enum OTPType {
            TOTP,
            HOTP
        }

        private OTPType _t;
        private string _s;

        public API(OTPType type, string secret_code)
        {
            _t = type;
            _s = secret_code;
        }

        public string Now()
        {
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = PyPath;//cmd is full path to python.exe
            start.Arguments = $"GoogleAuthAPI/GoogleAuthPy/GoogleAuthPy.py {_t.ToString().ToLower()} {_s}";//args is path to .py file and any cmd line args
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    return reader.ReadToEnd().Trim();
                }
            }
        }
    }
}
