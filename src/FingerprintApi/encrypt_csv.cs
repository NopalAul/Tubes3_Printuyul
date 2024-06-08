using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;

class EncryptCsv
{
    static void Main()
    {
        // string key = "YourVeryStrongKey1234567";
        string key = "printuyulGgXXXXXXXXX6969";
        string inputFile = "biodata.csv";
        string outputFile = "encrypted_biodata.csv";

        List<string[]> encryptedRows = new List<string[]>();

        using (StreamReader reader = new StreamReader(inputFile))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] row = line.Split(',');
                string[] encryptedRow = Array.ConvertAll(row, cell => Encrypt(cell, key));
                encryptedRows.Add(encryptedRow);
            }
        }

        using (StreamWriter writer = new StreamWriter(outputFile))
        {
            foreach (string[] row in encryptedRows)
            {
                writer.WriteLine(string.Join(",", row));
            }
        }

        Console.WriteLine("Encryption complete.'");
    }

    // fungsi encrypt sama spt di EncryptHelper.cs
    static string Encrypt(string plainText, string key)
    {
        byte[] iv = new byte[16];
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = iv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                }
                byte[] encryptedBytes = msEncrypt.ToArray();
                byte[] combined = iv.Concat(encryptedBytes).ToArray();
                return Convert.ToBase64String(combined);
            }
        }
    }
}
