using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

class Controller{
    public SqliteConnection sql_conn;

    public SqliteConnection SqlConn => sql_conn;
    public Controller(string database_path){

        string input = $"Data Source={database_path};";
        sql_conn = new SqliteConnection(input);

        try
        {
            sql_conn.Open();
            Console.WriteLine("Connection worked!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection didn't work!");
        }
    }

    public List<FingerprintData> getFingerData(){
        List<FingerprintData> fingers = new List<FingerprintData>();

        string query = @"
            SELECT * FROM sidik_jari;
        ";

        using (var command = new SqliteCommand(query, sql_conn))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string name = reader["name"].ToString();
                    string path = reader["path"].ToString();
                    Console.WriteLine(path);
                    fingers.Add(new FingerprintData(name, path));
                }
            }
        }

        return fingers;
    }

    public void insertFingerPrint(FingerprintData finger){
        string query = $"INSERT INTO sidik_jari (name, path) VALUES (@name, @path);";

        using (var command = new SqliteCommand(query, sql_conn))
        {
            command.Parameters.AddWithValue("@name", finger.getName());
            command.Parameters.AddWithValue("@path", finger.getPath());
            command.ExecuteNonQuery();
        }
    }

    public void insertFingerList(List<FingerprintData> finger_list){
        foreach (var finger in finger_list)
        {
            insertFingerPrint(finger);
        }
    }

    public void insertKTP(KTPData ktp){
        string query = @"INSERT INTO biodata (NIK, name, birth_place, birth_date, gender, blood_type, address, religion, marriage_status, job, citizenhip)
                         VALUES (@NIK, @name, @birth_place, @birth_date, @gender, @blood_type, @address, @religion, @marriage_status, @job, @citizenhip);";

        using (var command = new SqliteCommand(query, sql_conn))
        {
            command.Parameters.AddWithValue("@NIK", ktp.NIK);
            command.Parameters.AddWithValue("@name", ktp.name);
            command.Parameters.AddWithValue("@birth_place", ktp.birth_place);
            command.Parameters.AddWithValue("@birth_date", ktp.birth_date);
            command.Parameters.AddWithValue("@gender", ktp.gender);
            command.Parameters.AddWithValue("@blood_type", ktp.blood_type);
            command.Parameters.AddWithValue("@address", ktp.address);
            command.Parameters.AddWithValue("@religion", ktp.religion);
            command.Parameters.AddWithValue("@marriage_status", ktp.marriage_status);
            command.Parameters.AddWithValue("@job", ktp.job);
            command.Parameters.AddWithValue("@citizenhip", ktp.citizenhip);
            command.ExecuteNonQuery();
        }
    }

    public void insertKTPList(List<KTPData> ktp_list){
        foreach (var ktp in ktp_list)
        {
            insertKTP(ktp);
        }
    }

    public List<FingerprintData> TraverseSidikJari()
    {
        List<FingerprintData> fingerDataList = new List<FingerprintData>();
        string query = "SELECT berkas_citra, nama FROM sidik_jari;";
        using (var command = new SqliteCommand(query, sql_conn))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string berkasCitra = reader["berkas_citra"].ToString();
                    berkasCitra = berkasCitra.Trim('"');
                    berkasCitra = "../" + berkasCitra;
                    string nama = reader["nama"].ToString();
                    fingerDataList.Add(new FingerprintData(nama, berkasCitra));
                }
            }
        }
        return fingerDataList;
    }

    public void TraverseBiodata()
    {
        string query = @"SELECT NIK, nama, tempat_lahir, tanggal_lahir, jenis_kelamin, golongan_darah, alamat, agama, status_perkawinan, pekerjaan, kewarganegaraan FROM biodata;";
        using (var command = new SqliteCommand(query, sql_conn))
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string nik = reader["NIK"].ToString();
                    string biodataNama = reader["nama"].ToString();
                    string tempatLahir = reader["tempat_lahir"].ToString();
                    Console.WriteLine($"NIK: {nik}, Nama: {biodataNama}, Tempat Lahir: {tempatLahir}");
                }
            }
        }
    }

    // public void setup(){
    //     // insertKTP(new KTPData("NIK9","Nama1", "Tempat_lahir1", "tanggal_lahir1", "Laki-Laki", "O", "alamat1", "islam", "Menikah", "Mahasiswa", "Indonesia"));
    //     // insertFingerPrint(new FingerprintData("Finger2", "Idk"));
    // }
}