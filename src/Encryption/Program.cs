string biodataCsvFilePath = "encrypted_biodata.csv";
string fingerprintCsvFilePath = "decrypted_sidik_jari.csv";
string dbPath = "../FingerprintApi/EncryptedData.db";
InsertBiodataToCSV.InsertBiodata(biodataCsvFilePath, dbPath);
InsertSidikJariToCSV.InsertSidikJari(fingerprintCsvFilePath, dbPath);