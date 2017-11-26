using GAuthBotDevBot.GoogleAuthAPI;

namespace GAuthBotDevBot.Database.Model
{
    class GAuthAcc
    {
        public string secret_code { get; set; }
        public string email { get; set; }
        public string issuer { get; set; }
        public API.OTPType type { get; set; }
        public int count { get; set; }

        public GAuthAcc()
        {
            count = 0;
        }
    }
}
