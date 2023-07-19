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
            //konstante per numrin e bytes qe do te kete salt
            const int keySize = 64;
            //konstante per numrin e iterimeve te hashimit te password
            const int iterations = 350000;
            // tregon se do te hashohet me menyren SHA512
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            //gjeneron nje salt random 
            salt = RandomNumberGenerator.GetBytes(keySize);
            //llogaritja e hashit
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            // kthen hashin nga bytes ne HexString
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
            //konstante per numrin e bytes qe do te kete salt
            const int keySize = 64;
            //konstante per numrin e iterimeve te hashimit te password
            const int iterations = 350000;
            // tregon se do te hashohet me menyren SHA512
            HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
            // hashim ne ane te kundert 
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            // kthen hashin e sapo kalkuluar me hashin e marre me pare
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}

