using System;
using System.Security.Cryptography;
using System.Text;
using log4net;
using Sparrow.Services.API;

namespace Sparrow.Services.Utils
{
    public static class Auth
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Auth));
        public static string GenerateHash(string value)
        {
            var hashedValue = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(value));

                foreach (var b in result)
                    hashedValue.Append(b.ToString("x2"));
            }

            return hashedValue.ToString();
        }

        public static int GenerateSalt()
        {
            var rngCsp = new RNGCryptoServiceProvider();
            var randomNumber = new byte[4];
            rngCsp.GetBytes(randomNumber);

            return BitConverter.ToInt32(randomNumber, 0);
        }

        public static string GenerateToken(int salt, string password, string email)
        {
            email = email.ToLower();

            log.Debug("Method:GenerateToken | " + "salt:" + salt + " | password:" + password + " | email:" + email);
            var rawToken = salt + password + email;
            log.Debug("Method:GenerateToken | rawToken:" + rawToken);
            var token = GenerateHash(rawToken);
            log.Debug("Method:GenerateToken | token:" + token);
            return token;
        }
    }
}
