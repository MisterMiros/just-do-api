using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace API.Model.Utils
{
    public static class PasswordUtils
    {
        private const int ITERATIONS = 10000;
        private const int SALT_SIZE = 16;
        private const int KEY_SIZE = 20;

        public static string Hash(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SALT_SIZE]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            byte[] hash = pbkdf2.GetBytes(KEY_SIZE);

            byte[] hashBytes = new byte[SALT_SIZE + KEY_SIZE];
            Array.Copy(salt, 0, hashBytes, 0, SALT_SIZE);
            Array.Copy(hash, 0, hashBytes, SALT_SIZE, KEY_SIZE);

            string passwordHash = Convert.ToBase64String(hashBytes);
            return passwordHash;
        }

        public static bool Verify(string password, string passwordHash)
        {
            byte[] hashBytes = Convert.FromBase64String(passwordHash);
            byte[] salt = new byte[SALT_SIZE];
            Array.Copy(hashBytes, 0, salt, 0, SALT_SIZE);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, ITERATIONS);
            byte[] hash = pbkdf2.GetBytes(KEY_SIZE);

            for (int i = 0; i < KEY_SIZE; i++)
            {
                if (hashBytes[i + SALT_SIZE] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
