using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;

class InsertBiodataToCSV
{
    public static void InsertBiodata(string csvFilePath, string dbPath)
    {

        List<string[]> biodataEntries = ReadCsv(csvFilePath);
        InsertIntoDatabase(biodataEntries, dbPath);

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

    static void InsertIntoDatabase(List<string[]> biodataEntries, string dbPath)
    {
        using (var conn = new SQLiteConnection($"Data Source={dbPath};Version=3;"))
        {
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "DROP TABLE IF EXISTS biodata";
                cmd.ExecuteNonQuery();

                // Create the biodata table if it does not exist
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS biodata (
                    NIK TEXT PRIMARY KEY,
                    nama TEXT NOT NULL,
                    tempat_lahir TEXT NOT NULL,
                    tanggal_lahir TEXT NOT NULL,
                    jenis_kelamin TEXT NOT NULL,
                    golongan_darah TEXT NOT NULL,
                    alamat TEXT NOT NULL,
                    agama TEXT NOT NULL,
                    status_perkawinan TEXT NOT NULL,
                    pekerjaan TEXT NOT NULL,
                    kewarganegaraan TEXT NOT NULL)";
                cmd.ExecuteNonQuery();

                // Insert data into the biodata table
                cmd.CommandText = @"INSERT INTO biodata (NIK, nama, tempat_lahir, tanggal_lahir, jenis_kelamin, golongan_darah, alamat, agama, status_perkawinan, pekerjaan, kewarganegaraan)
                    VALUES (@NIK, @nama, @tempat_lahir, @tanggal_lahir, @jenis_kelamin, @golongan_darah, @alamat, @agama, @status_perkawinan, @pekerjaan, @kewarganegaraan)";

                foreach (var entry in biodataEntries)
                {
                    // Call this function before the executemany statement
                    CheckForExtraValues(entry);
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@NIK", entry[0]);
                    cmd.Parameters.AddWithValue("@nama", entry[1]);
                    cmd.Parameters.AddWithValue("@tempat_lahir", entry[2]);
                    cmd.Parameters.AddWithValue("@tanggal_lahir", entry[3]);
                    cmd.Parameters.AddWithValue("@jenis_kelamin", entry[4]);
                    cmd.Parameters.AddWithValue("@golongan_darah", entry[5]);
                    cmd.Parameters.AddWithValue("@alamat", entry[6]);
                    cmd.Parameters.AddWithValue("@agama", entry[7]);
                    cmd.Parameters.AddWithValue("@status_perkawinan", entry[8]);
                    cmd.Parameters.AddWithValue("@pekerjaan", entry[9]);
                    cmd.Parameters.AddWithValue("@kewarganegaraan", entry[10]);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

    static void CheckForExtraValues(string[] entry)
    {
        if (entry.Length > 11)
        {
            Console.WriteLine($"Row {Array.IndexOf(entry, entry) + 1}: {string.Join(",", entry)}");
        }
    }
}
