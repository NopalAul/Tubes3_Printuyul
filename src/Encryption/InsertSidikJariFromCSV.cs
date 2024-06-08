using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

class InsertSidikJariToCSV
{
    public static void InsertSidikJari(string csvFilePath, string dbPath)
    {

        List<string[]> sidikJariEntries = ReadCsv(csvFilePath);
        InsertIntoDatabase(sidikJariEntries, dbPath);

        Console.WriteLine("Data inserted successfully.");
    }

    static List<string[]> ReadCsv(string filePath)
    {
        List<string[]> rows = new List<string[]>();
        using (var reader = new StreamReader(filePath))
        {
            reader.ReadLine(); // Skip the header
            while (!reader.EndOfStream)
            {
                string[] row = reader.ReadLine().Split(',');
                rows.Add(row);
            }
        }
        return rows;
    }

    static void InsertIntoDatabase(List<string[]> sidikJariEntries, string dbPath)
    {
        using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DROP TABLE IF EXISTS sidik_jari";
                cmd.ExecuteNonQuery();

                // Create the sidik_jari table if it does not exist
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS sidik_jari (
                    berkas_citra TEXT NOT NULL,
                    nama TEXT NOT NULL)";
                cmd.ExecuteNonQuery();

                // Insert data into the sidik_jari table
                cmd.CommandText = @"INSERT INTO sidik_jari (berkas_citra, nama)
                    VALUES (@berkas_citra, @nama)";

                foreach (var entry in sidikJariEntries)
                {
                    // Call this function before the executemany statement
                    CheckForExtraValues(entry);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@berkas_citra", entry[0]);
                    cmd.Parameters.AddWithValue("@nama", entry[1]);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    static void CheckForExtraValues(string[] entry)
    {
        if (entry.Length > 2)
        {
            Console.WriteLine($"Row {Array.IndexOf(entry, entry) + 1}: {string.Join(",", entry)}");
        }
    }
}