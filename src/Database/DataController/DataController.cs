using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Runtime.InteropServices;

namespace DataController{
    class Controller{
        SQLiteConnection sql_conn;
        public Controller(string database_path){
 
            string input = $"Data Source={database_path};Version=3;";
            sql_conn = new SQLiteConnection(input);

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



            return fingers;
        }

        public void insertFingerPrint(FingerprintData finger){
            SQLiteCommand command;
            command = sql_conn.CreateCommand();

            string query = $" INSERT INTO sidik_jari VALUES('{finger.getName()}', '{finger.getPath()}');";

            command.CommandText = query;
            command.ExecuteNonQuery();
        }

        public void insertFingerList(List<FingerprintData> finger_list){
            for(int i = 0 ; i < finger_list.Count ; i++){
                insertFingerPrint(finger_list[i]);
            }
        }

        public void insertKTP(KTPData ktp){
            SQLiteCommand command;
            command = sql_conn.CreateCommand();

            string query = $" INSERT INTO biodata VALUES('{ktp.NIK}', '{ktp.name}', '{ktp.birth_place}', '{ktp.birth_date}', '{ktp.gender}', '{ktp.blood_type}', '{ktp.address}', '{ktp.religion}', '{ktp.marriage_status}', '{ktp.job}', '{ktp.citizenhip}');";

            command.CommandText = query;
            command.ExecuteNonQuery();
        }

        public void insertKTPList(List<KTPData> ktp_list){
            for(int i = 0 ; i < ktp_list.Count ; i++){
                insertKTP(ktp_list[i]);
            }
        }

        public void TraverseSidikJari()
        {
            string query = "SELECT berkas_citra, nama FROM sidik_jari;";
            using (SQLiteCommand command = new SQLiteCommand(query, sql_conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string berkasCitra = reader["berkas_citra"].ToString();
                        string nama = reader["nama"].ToString();

                        Console.WriteLine($"Berkas Citra: {berkasCitra}, Nama: {nama}");
                    }
                }
            }
        }

        public void TraverseBiodata()
        {
            string query = "SELECT NIK, nama, tempat_lahir, tanggal_lahir, jenis_kelamin, golongan_darah, alamat, agama, status_perkawinan, pekerjaan, kewarganegaraan FROM biodata;";
            using (SQLiteCommand command = new SQLiteCommand(query, sql_conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
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
    

        public void setup(){
            insertKTP(new KTPData("NIK9","Nama1", "Tempat_lahir1", "tanggal_lahir1", "Laki-Laki", "O", "alamat1", "islam", "Menikah", "Mahasiswa", "Indonesia"));
            insertFingerPrint(new FingerprintData("Finger2", "Idk"));
        }
    }
}