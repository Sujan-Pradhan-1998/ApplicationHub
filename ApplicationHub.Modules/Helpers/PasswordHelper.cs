using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ApplicationHub.Modules.Helpers
{
    public static class PasswordHelper
    {
        private static class Salt
        {
            public static string Create(string email)
            {
                byte[] randomBytes = Encoding.UTF8.GetBytes(email.ToLower());
                return Convert.ToBase64String(randomBytes);
            }
        }

        public static string CreateHash(string password, string email)
        {
            var salt = Salt.Create(email);
            var valueBytes = KeyDerivation.Pbkdf2(
                password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);

            var hash = Convert.ToBase64String(valueBytes);
            return hash;
        }

        public static bool Validate(string user, string password, string hash)
            => CreateHash(password, user) == hash;
    }
}