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

        public GAuthAcc(string issuer, string secret, API.OTPType type = API.OTPType.TOTP, int count = 0, string email = "")
        {
            this.issuer = issuer;
            this.secret_code = secret;
            this.type = type;
            count = 0;
        }

        public GAuthAcc()
        {
            count = 0;
        }
    }
}
