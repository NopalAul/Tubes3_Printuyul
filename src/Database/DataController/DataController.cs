using System;
using System.Data.SQLite;

namespace DataController{
    // public class DataController{
    //     public string databaseDir;

    //     public 
    //     public DataController(string input) {
    //         databaseDir = input;
    //     }
    // }
    class Controller
    {
        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection sqlite_conn;
            // Create a new database connection:
            sqlite_conn = new SQLiteConnection("Data Source=MainData.db;Version=3;New=True;Compress=True;");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
                Console.WriteLine("Connection worked!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection didn't work!");
            }
            return sqlite_conn;
        }
    }
}