using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;

class DecryptCsv
{
    static void Main()
    {
        // string key = "YourVeryStrongKey1234567";
        string key = "printuyulGgXXXXXXXXX6969";
        string inputFile = "encrypted_sidik_jari.csv";
        string outputFile = "decrypted_sidik_jari.csv";

        List<string[]> decryptedRows = new List<string[]>();

        using (StreamReader reader = new StreamReader(inputFile))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] encryptedRow = line.Split(',');
                string[] decryptedRow = Array.ConvertAll(encryptedRow, cell => Decrypt(cell, key));
                decryptedRows.Add(decryptedRow);
            }
        }

        using (StreamWriter writer = new StreamWriter(outputFile))
        {
            foreach (string[] row in decryptedRows)
            {
                writer.WriteLine(string.Join(",", row));
            }
        }

        Console.WriteLine("Decryption complete.'");
    }

    // fungsi decrypt sama spt di EncryptHelper.cs
    static string Decrypt(string cipherText, string key)
    {
        byte[] combined = Convert.FromBase64String(cipherText);
        byte[] iv = combined.Take(16).ToArray();
        byte[] cipherTextBytes = combined.Skip(16).ToArray();

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(cipherTextBytes))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
