using System;
using System.Linq;
using System.Security.Cryptography;

namespace Tea.Core.Impl
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;    // 128 bit 
        private const int KeySize = 32;     // 256 bit
        private const int Iterations = 100;

        public bool Check(string hash, string password)
        {
            var parts = hash.Split('.');

            if (parts.Length != 2)
            {
                throw new FormatException("Unexpected hash format. " +
                  "Should be formatted as `{salt}.{hash}`");
            }

            var salt = Convert.FromBase64String(parts[0]);
            var key = Convert.FromBase64String(parts[1]);

            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                Iterations))
            {
                var keyToCheck = algorithm.GetBytes(KeySize);
                return keyToCheck.SequenceEqual(key);
            }
        }

        public string Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Iterations))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);
                return $"{Iterations}.{salt}.{key}";
            }
        }
    }
}
