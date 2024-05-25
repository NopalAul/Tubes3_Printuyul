using System;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Runtime.InteropServices;

namespace DataController{
    class Controller{
        SQLiteConnection sql_conn;
        public Controller(string database_path)
        {
 
            string input = $"Data Source={database_path};Vesion=3;";
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

        public void setup(){
            SQLiteCommand command;
            command = sql_conn.CreateCommand();

            command.CommandText = @"
                INSERT INTO sidik_jari VALUES('zaki','nigger');
            ";

            command.ExecuteNonQuery();
        }
    }
}