using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;

namespace Rent_a_car
{
    class Rent_a_car_DB
    {
        public Rent_a_car_DB()
        {
            InitializeDB();
        }
        public async static void InitializeDB()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("Rent a car.db", CreationCollisionOption.OpenIfExists);
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Rent a car.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}")) 
            {
                con.Open();
                string initCMD = "CREATE TABLE IF NOT EXISTS" +
                    "Klijenti(OIB INT(11) PRIMARY KEY NOT NULL," +
                    "Ime VARCHAR(40) NOT NULL," +
                    "Prezime VARCHAR(40) NOT NULL," +
                    "Adresa VARCHAR(50) NOT NULL," +
                    "Datum_rođenja DATA NOT NULL)";

                SqliteCommand CMDcreateTable = new SqliteCommand(initCMD, con);
                CMDcreateTable.ExecuteReader();
                con.Close();

            }
        }

        internal static void addRecord(string v1, string v2, string v3, string v4, string v5)
        {
            throw new NotImplementedException();
        }

        public static void addRecord(int OIB, string Ime, string Prezime, string Adresa, DateTime Datum_rođenja)
        {
            if(!OIB.Equals("") && !Ime.Equals("") && !Prezime.Equals("") && !Adresa.Equals("") && !Datum_rođenja.Equals(""))
            {
                string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Rent a car.db");
                using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
                {
                    con.Open();
                    SqliteCommand CMD_Insert = new SqliteCommand();
                    CMD_Insert.Connection = con;
                    CMD_Insert.CommandText = "INSERT INTO Klijenti VALUES(@OIB,@Ime,@Prezime,@Adresa,@Datum_rođenja);";
                    CMD_Insert.Parameters.AddWithValue("OIB", OIB);
                    CMD_Insert.Parameters.AddWithValue("Ime", Ime);
                    CMD_Insert.Parameters.AddWithValue("Prezime", Prezime);
                    CMD_Insert.Parameters.AddWithValue("Adresa", Adresa);
                    CMD_Insert.Parameters.AddWithValue("Datum_rođenja", Datum_rođenja);

                    CMD_Insert.ExecuteReader();
                    con.Close();

                }
            }
        }

        public class  detaljiKlijenta
        {
            public int oib { get; set; }
            public String ime { get; set; }
            public String prezime { get; set; }
            public String adresa { get; set; }
            public DateTime datum_rođenja { get; set; }

            public detaljiKlijenta(int OIB, String Ime, String Prezime, String Adresa, DateTime Datum_rođenja)
            {
                oib = OIB;
                ime = Ime;
                prezime = Prezime;
                adresa = Adresa;
                datum_rođenja = Datum_rođenja;
            }

            public detaljiKlijenta(string v1, string v2)
            {
            }
        }

        public static List<detaljiKlijenta> GetAllRecords()
        {
            List<detaljiKlijenta> klijentiList = new List<detaljiKlijenta>();
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "Rent a car.db");
            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                string selectCmd = "SELECT OIB,Ime,Prezime,Adresa,Datum_rođenja FROM Klijenti";
                SqliteCommand cmd_getAllRec = new SqliteCommand(selectCmd,con);

                SqliteDataReader reader = cmd_getAllRec.ExecuteReader();

                while(reader.Read())
                {
                    klijentiList.Add(new detaljiKlijenta(reader.GetString(0), reader.GetString(1)));
                }
                con.Close();
            }
            return klijentiList;
        }
    }
}
