using System.Security.Cryptography;
using System.Text;

namespace RealEstateWebAPI
{

    public static class PasswordHashing
    {
        /// <summary>
        /// Hashon passwordin e marre dhe krijon nje salt per te
        /// </summary>
        /// <param name="password">Passwordi per tu hashuar</param>
        /// <param name="salt">Salt i gjeneruar.</param>
        /// <returns>Password e hashuar si nje hexadecimal string.</returns>
        public static string HashPasword(string password, out byte[] salt)
        {
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            salt = RandomNumberGenerator.GetBytes(keySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return Convert.ToHexString(hash);
        }
        /// <summary>
        /// Verifikon nese password i marre perputhet me hash pass e regjistruar
        /// </summary>
        /// <param name="password">Password per tu verifikuar</param>
        /// <param name="hash">Passwordi i hashuar i ruajtur si  hexadecimal string.</param>
        /// <param name="salt">Salt i perfshire me password hash</param>
        /// <returns><see langword="true"/> nese password validohet , perndryshe kthen , <see langword="false"/>.</returns>
        public static bool VerifyPassword(string password, string hash, byte[] salt)
        {
            const int keySize = 64;
            const int iterations = 350000;
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}

