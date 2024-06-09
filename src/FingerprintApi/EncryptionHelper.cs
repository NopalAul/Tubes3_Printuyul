using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FingerprintApi.Controllers
{
    public class EncryptionHelper
    {
        // private static readonly byte[] key = Encoding.UTF8.GetBytes("YourVeryStrongKey1234567");
        private static readonly byte[] key = Encoding.UTF8.GetBytes("printuyulGgXXXXXXXXX6969");
        // private static readonly byte[] key = Encoding.UTF8.GetBytes("YourVeryStrongKey1234555");
        static EncryptionHelper()
        {
            if (key.Length != 16 && key.Length != 24 && key.Length != 32)
            {
                throw new ArgumentException("Invalid key size for AES algorithm. Key size must be 16, 24, or 32 bytes.");
            }
        }

        public static string EncryptString(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.GenerateIV(); // menghasilkan IV secara acak

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length); // simpan IV di awal ciphertext
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string DecryptString(string cipherText)
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                byte[] cipher = new byte[fullCipher.Length - iv.Length];

                Array.Copy(fullCipher, iv, iv.Length);
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipher))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        // static void Main(string[] args)
        // {
        //     // test encrypt
        //     string originalText = "\"../test/1__M_Right_little_finger.BMP\"";
        //     Console.WriteLine("Original Text: " + originalText);

        //     string encryptedText = EncryptionHelper.EncryptString(originalText);
        //     Console.WriteLine("Encrypted Text: " + encryptedText);

        //     // test decrypt
        //     string decryptedText = EncryptionHelper.DecryptString(encryptedText);
        //     Console.WriteLine("Decrypted Text: " + decryptedText);
        // }

    }
}