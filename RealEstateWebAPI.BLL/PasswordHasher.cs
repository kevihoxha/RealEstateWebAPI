using System.Security.Cryptography;

namespace RealEstateWebAPI
{
        public class PasswordHasher
        {
            private const int SaltSize = 16; // 128 bits
            private const int HashSize = 32; // 256 bits
            private const int Iterations = 10000;

            public string HashPassword(string password)
            {
                byte[] salt = GenerateSalt();
                byte[] hash = GenerateHash(password, salt);

                string saltBase64 = Convert.ToBase64String(salt);
                string hashBase64 = Convert.ToBase64String(hash);

                return $"{Iterations}.{saltBase64}.{hashBase64}";
            }

            public bool VerifyPassword(string password, string hashedPassword)
            {
                try
                {
                    string[] parts = hashedPassword.Split('.');
                    int iterations = int.Parse(parts[0]);
                    byte[] salt = Convert.FromBase64String(parts[1]);
                    byte[] hash = Convert.FromBase64String(parts[2]);

                    byte[] computedHash = GenerateHash(password, salt, iterations);

                    return SlowEquals(hash, computedHash);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            private byte[] GenerateSalt()
            {
                byte[] salt = new byte[SaltSize];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(salt);
                }
                return salt;
            }

            private byte[] GenerateHash(string password, byte[] salt, int iterations = Iterations)
            {
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    return pbkdf2.GetBytes(HashSize);
                }
            }

            private bool SlowEquals(byte[] a, byte[] b)
            {
                uint diff = (uint)a.Length ^ (uint)b.Length;
                for (int i = 0; i < a.Length && i < b.Length; i++)
                    diff |= (uint)(a[i] ^ b[i]);
                return diff == 0;
            }
        }
    }

