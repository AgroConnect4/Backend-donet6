using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace agroApp.API.Utils
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password, byte[] salt)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public static bool VerifyPassword(string password, byte[] salt, string hashedPassword)
        {
            string hashed = HashPassword(password, salt);
            return hashed == hashedPassword;
        }
    }
}