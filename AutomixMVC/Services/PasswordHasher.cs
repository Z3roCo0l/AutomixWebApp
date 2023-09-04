using System;
using System.Security.Cryptography;
using System.Text;

namespace AutomixMVC.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private static readonly int SaltSize = 16; // 128 bits
        private static readonly int KeySize = 32; // 256 bits
        private static readonly int Iterations = 10000;

        public string HashPassword(string password)
        {
            using var algorithm = new Rfc2898DeriveBytes(
                password,
                SaltSize,
                Iterations,
                HashAlgorithmName.SHA256);

            var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
            var salt = Convert.ToBase64String(algorithm.Salt);

            return $"{Iterations}.{salt}.{key}";
        }

        public bool VerifyPassword(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format.");
            }

            if (!int.TryParse(parts[0], out int iterations))
            {
                throw new FormatException("Unexpected iteration format.");
            }

            byte[] salt, key;
            try
            {
                salt = Convert.FromBase64String(parts[1]);
                key = Convert.FromBase64String(parts[2]);
            }
            catch
            {
                throw new FormatException("Invalid Base64 encoding.");
            }

/*            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);*/

            using var algorithm = new Rfc2898DeriveBytes(
                password,
                salt,
                iterations,
                HashAlgorithmName.SHA256);

            var keyToCheck = algorithm.GetBytes(KeySize);

            return KeySize == keyToCheck.Length && ConstantTimeComparison(key, keyToCheck);

        }
        private static bool ConstantTimeComparison(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }

            return diff == 0;
        }
    }

}
