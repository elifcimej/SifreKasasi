using System.Security.Cryptography;
using System.Text;

namespace SifreKasasi.API
{
    public static class EncryptionHelper
    {
        private static readonly string key = "buCokGucluBirKey123!";

        public static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(16).Substring(0, 16));
            aes.IV = new byte[16];
            var encryptor = aes.CreateEncryptor();
            var buffer = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(encryptor.TransformFinalBlock(buffer, 0, buffer.Length));
        }

        public static string Decrypt(string cipherText)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key.PadRight(16).Substring(0, 16));
            aes.IV = new byte[16];
            var decryptor = aes.CreateDecryptor();
            var buffer = Convert.FromBase64String(cipherText);
            return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(buffer, 0, buffer.Length));
        }
    }
}
