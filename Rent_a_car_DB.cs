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
        public Rent_a_car_DB() //konstruktor u kojem se poziva metoda InitializeDB()
        {
            InitializeDB();
        }
        public async static void InitializeDB()
        {
            String nazivBaze = "RentAcar.db";
            await ApplicationData.Current.LocalFolder.CreateFileAsync(nazivBaze, CreationCollisionOption.OpenIfExists); //naziv baze, i funkcija koja ju otvara ako postoji
            string putDoBaze = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze); //funkcija koja trazi put do baze koja je drugi argument

            using (SqliteConnection con = new SqliteConnection($"Filename={putDoBaze}")) 
            {
                con.Open();
                string sintaksaNaredbe = "CREATE TABLE IF NOT EXISTS " +
                                         "Klijenti (" +
                                         "OIB INT(11) PRIMARY KEY NOT NULL," +
                                         "Ime VARCHAR(40) NOT NULL," +
                                         "Prezime VARCHAR(40) NOT NULL," +
                                         "Adresa VARCHAR(50) NOT NULL," +
                                         "Datum_rodenja VARCHAR(11) NOT NULL)";

                SqliteCommand naredbaZaKreiranje = new SqliteCommand(sintaksaNaredbe, con);
                naredbaZaKreiranje.ExecuteReader();
                con.Close();

            }
        }

        //internal static void addRecord(String Klijenti, int OIB, String Ime, String Prezime, String Adresa, DateTime DatumRodenja)
        //{
        //    throw new NotImplementedException();
        //}

        public static void addRecord(Int64 OIB, String Ime, String Prezime, String Adresa, String Datum_rodenja)
        {
            String nazivBaze = "RentAcar.db";
            if (!OIB.Equals("") && !Ime.Equals("") && !Prezime.Equals("") && !Adresa.Equals("") && !Datum_rodenja.Equals(""))
            {
                string putDoBaze = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);
                using (SqliteConnection con = new SqliteConnection($"Filename={putDoBaze}"))
                {
                    con.Open();
                    SqliteCommand naredba_insert = new SqliteCommand();
                    naredba_insert.Connection = con; //konekcija naredbe se nalazi u varijabli con
                    naredba_insert.CommandText = "INSERT INTO Klijenti(OIB, Ime, Prezime, Adresa, Datum_rodenja) VALUES(@OIB, @Ime, @Prezime, @Adresa, @Datum_rodenja);";
                    naredba_insert.Parameters.AddWithValue("@OIB", OIB);
                    naredba_insert.Parameters.AddWithValue("@Ime", Ime);
                    naredba_insert.Parameters.AddWithValue("@Prezime", Prezime);
                    naredba_insert.Parameters.AddWithValue("@Adresa", Adresa);
                    naredba_insert.Parameters.AddWithValue("@Datum_rodenja", Datum_rodenja);

                    naredba_insert.ExecuteReader();
                    con.Close();

                }
            }
        }

        public class detaljiKlijenta
        {
            public Int64 oib { get; set; }
            public String ime { get; set; }
            public String prezime { get; set; }
            public String adresa { get; set; }
            public String datum_rodenja { get; set; }

            public detaljiKlijenta(Int64 OIB, String Ime, String Prezime, String Adresa, String Datum_rodenja)
            {
                oib = OIB;
                ime = Ime;
                prezime = Prezime;
                adresa = Adresa;
                datum_rodenja = Datum_rodenja;
            }

        }

        public static void izbrisi()
        {
            String nazivBaze = "RentAcar.db";
            String pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                String naredba_delete = "DELETE FROM Klijenti";
                SqliteCommand cmd_getAllRec = new SqliteCommand(naredba_delete, con);
                SqliteDataReader reader = cmd_getAllRec.ExecuteReader();
                con.Close();
            }
        }

        public static List<detaljiKlijenta> DohvatSvihPodataka()
        {
            String nazivBaze = "RentAcar.db";

            List<detaljiKlijenta> klijentiList = new List<detaljiKlijenta>();
            String pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);
            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                String naredba_select = "SELECT * FROM Klijenti";
                SqliteCommand cmd_getAllRec = new SqliteCommand(naredba_select, con);

                SqliteDataReader reader = cmd_getAllRec.ExecuteReader();

                while(reader.Read())
                {
                    klijentiList.Add(new detaljiKlijenta(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)));
                }
                con.Close();
            }
            return klijentiList;
        }
    }
}
