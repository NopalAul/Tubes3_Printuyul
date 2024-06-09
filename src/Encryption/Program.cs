using System;
using System.IO;


string biodataCsvFilePath = Path.Combine("..", "..", "test", "CsvFiles");
string dbPath = Path.Combine("..", "FingerprintApi", "EncryptedData.db");

try
{
    Console.WriteLine("Enter your 'sidik_jari' CSV file");
    string file_sidik_jari = Console.ReadLine();
    Console.WriteLine("Enter your 'biodata' CSV file");
    string file_biodata = Console.ReadLine();
    
    file_sidik_jari = Path.Combine(biodataCsvFilePath, file_sidik_jari);
    file_biodata = Path.Combine(biodataCsvFilePath, file_biodata);

    InsertBiodataToCSV.InsertBiodata(file_biodata, dbPath);
    InsertSidikJariToCSV.InsertSidikJari(file_sidik_jari, dbPath);
}
catch (Exception e)
{
    Console.WriteLine("An error occurred: " + e.Message);
}