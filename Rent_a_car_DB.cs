using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Windows.Storage;
using Windows.UI.Popups;

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
            //----------------------------------------------------------------KLIJENTI----------------------------------------------------------------
            String nazivBaze = "RentAcar.db";
            await ApplicationData.Current.LocalFolder.CreateFileAsync(nazivBaze, CreationCollisionOption.OpenIfExists); //naziv baze, i funkcija koja ju otvara ako postoji
            String putDoBaze = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze); //funkcija koja trazi put do baze koja je drugi argument

            using (SqliteConnection con = new SqliteConnection($"Filename={putDoBaze}")) 
            {
                con.Open();
                String klijenti = "CREATE TABLE IF NOT EXISTS " +
                                         "Klijenti (" +
                                         "OIB INT(11) PRIMARY KEY NOT NULL," +
                                         "Ime VARCHAR(40) NOT NULL," +
                                         "Prezime VARCHAR(40) NOT NULL," +
                                         "Adresa VARCHAR(50) NOT NULL," +
                                         "Datum_rodenja VARCHAR(11) NOT NULL)";

                String automobili = "CREATE TABLE IF NOT EXISTS " +
                                         "Automobili(" +
                                         "ID INT(5) PRIMARY KEY NOT NULL," +
                                         "Model VARCHAR(40) NOT NULL," +
                                         "Godina INT(4) NOT NULL,"+
                                         "Cijena_po_danu DECIMAL(20) NOT NULL,"+
                                         "Količina INT(10) NOT NULL)";

                String rezervacije = "CREATE TABLE IF NOT EXISTS " +
                                     "Rezervacije(" +
                                     "OIB INT(11) NOT NULL," +
                                     "ID INT(5) NOT NULL," +
                                     "Broj_dana_najma INT(10) NOT NULL)";

                SqliteCommand naredbaZaKreiranjeKlijenata = new SqliteCommand(klijenti, con);
                SqliteCommand naredbaZaKreiranjeAutomobila = new SqliteCommand(automobili, con);
                SqliteCommand naredbaZaKreiranjeRezervacija = new SqliteCommand(rezervacije, con);

                naredbaZaKreiranjeKlijenata.ExecuteReader();
                naredbaZaKreiranjeAutomobila.ExecuteReader();
                naredbaZaKreiranjeRezervacija.ExecuteReader();
                con.Close();

            }
        }

        public static async void dodavanjeKlijenta(Int64 OIB, String Ime, String Prezime, String Adresa, String Datum_rodenja)
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


                    // provjera postoji li isti OIB
                    if (!Rent_a_car_DB.provjeraOIB().Contains(OIB))
                    {
                        naredba_insert.ExecuteReader();
                    }
                    else
                    {
                        MessageDialog dialog = new MessageDialog("Ovaj OIB već postoji.", "Pogreška");
                        await dialog.ShowAsync();
                    }
                    con.Close();
                }
            }
        }

        public static void brisanjeKlijenta(Int64 OIB)
        {
            String nazivBaze = "RentAcar.db";
            string putDoBaze = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);
            using (SqliteConnection con = new SqliteConnection($"Filename={putDoBaze}"))
            {
                con.Open();
                SqliteCommand naredba_insert = new SqliteCommand();
                naredba_insert.Connection = con; //konekcija naredbe se nalazi u varijabli con
                naredba_insert.CommandText = "DELETE FROM Klijenti WHERE OIB=@OIB";
                naredba_insert.Parameters.AddWithValue("@OIB", OIB);
                naredba_insert.ExecuteReader();
                con.Close();
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


        public class detaljiKlijenta
        {
            public Int64 OIB { get; set; }
            public String Ime { get; set; }
            public String Prezime { get; set; }
            public String Adresa { get; set; }
            public String Rođenje { get; set; }

            public detaljiKlijenta(Int64 OIB, String Ime, String Prezime, String Adresa, String Rođenje)
            {
                this.OIB = OIB;
                this.Ime = Ime;
                this.Prezime = Prezime;
                this.Adresa = Adresa;
                this.Rođenje = Rođenje;
            }

        }


        public static List<Int64> provjeraOIB()
        {
            String nazivBaze = "RentAcar.db";

            List<Int64> oibList = new List<Int64>();
            String pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);
            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                String naredba_select = "SELECT OIB FROM Klijenti";
                SqliteCommand cmd_getAllRec = new SqliteCommand(naredba_select, con);
                SqliteDataReader reader = cmd_getAllRec.ExecuteReader();

                while (reader.Read()) //dok je moguce citati iz baze cita
                {
                    oibList.Add(reader.GetInt64(0));
                }

                con.Close();
            }
            return oibList;
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

                while(reader.Read()) //dok je moguce citati iz baze cita
                {
                    klijentiList.Add(new detaljiKlijenta(reader.GetInt64(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetString(4)));
                }

                con.Close();
            }
            return klijentiList;
        }

        //----------------------------------------------------------------KLIJENTI----------------------------------------------------------------
        
        
        //----------------------------------------------------------------AUTOMOBILI----------------------------------------------------------------
        public class detaljiAutomobila
        {
            public Int64 ID { get; set; }
            public String Model { get; set; }
            public Int64 Godina { get; set; }
            public Decimal Cijena_po_danu { get; set; }
            public Int64 Količina { get; set; }

            public detaljiAutomobila(Int64 ID, String Model, Int64 Godina, Decimal Cijena_po_danu, Int64 Količina)
            {
                this.ID = ID;
                this.Model = Model;
                this.Godina = Godina;
                this.Cijena_po_danu = Cijena_po_danu;
                this.Količina = Količina;
            }

        }

        public static void dodavanjeAutomobila(Int64 ID, String Model, Int64 Godina, Decimal Cijena_po_danu, Int64 Količina)
        {
            String nazivBaze = "RentAcar.db";
            if (!ID.Equals("") && !Model.Equals("") && !Godina.Equals("") && !Cijena_po_danu.Equals("") && !Količina.Equals(""))
            {
                string putDoBaze = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);
                using (SqliteConnection con = new SqliteConnection($"Filename={putDoBaze}"))
                {
                    con.Open();
                    SqliteCommand naredba_insert = new SqliteCommand();
                    naredba_insert.Connection = con; //konekcija naredbe se nalazi u varijabli con
                    naredba_insert.CommandText = "INSERT INTO Automobili(ID, Model, Godina, Cijena_po_danu, Količina) VALUES(@ID, @Model, @Godina, @Cijena_po_danu, @Količina);";
                    naredba_insert.Parameters.AddWithValue("@ID", ID);
                    naredba_insert.Parameters.AddWithValue("@Model", Model);
                    naredba_insert.Parameters.AddWithValue("@Godina", Godina);
                    naredba_insert.Parameters.AddWithValue("@Cijena_po_danu", Cijena_po_danu);
                    naredba_insert.Parameters.AddWithValue("@Količina", Količina);

                    naredba_insert.ExecuteReader();
                    con.Close();
                }
            }
        }

        public static void brisanjeAutomobila(Int64 ID)
        {
            String nazivBaze = "RentAcar.db";
            string putDoBaze = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);
            using (SqliteConnection con = new SqliteConnection($"Filename={putDoBaze}"))
            {
                con.Open();
                SqliteCommand naredba_insert = new SqliteCommand();
                naredba_insert.Connection = con; //konekcija naredbe se nalazi u varijabli con
                naredba_insert.CommandText = "DELETE FROM Automobili WHERE ID=@ID";
                naredba_insert.Parameters.AddWithValue("@ID", ID);
                naredba_insert.ExecuteReader();
                con.Close();
            }
        }

        public static void izbrisi2()
        {
            String nazivBaze = "RentAcar.db";
            String pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                String naredba_delete = "DELETE FROM Automobili";
                SqliteCommand cmd_getAllRec = new SqliteCommand(naredba_delete, con);
                SqliteDataReader reader = cmd_getAllRec.ExecuteReader();
                con.Close();
            }
        }

        public static List<detaljiAutomobila> DohvatSvihPodataka2()
        {
            String nazivBaze = "RentAcar.db";

            List<detaljiAutomobila> automobiliList = new List<detaljiAutomobila>();
            String pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);
            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                String naredba_select = "SELECT * FROM Automobili";
                SqliteCommand cmd_getAllRec = new SqliteCommand(naredba_select, con);

                SqliteDataReader reader = cmd_getAllRec.ExecuteReader();

                while (reader.Read()) //dok je moguce citati iz baze cita
                {
                    automobiliList.Add(new detaljiAutomobila(reader.GetInt64(0), reader.GetString(1), reader.GetInt64(2), reader.GetDecimal(3), reader.GetInt64(4)));
                }
                con.Close();
            }
            return automobiliList;
        }
        //----------------------------------------------------------------AUTOMOBILI----------------------------------------------------------------

        //----------------------------------------------------------------REZERVACIJE----------------------------------------------------------------


        public class detaljiRezervacija
        {
            public Int64 OIB { get; set; }
            public Int64 ID { get; set; }
            public Int64 Broj_dana_najma { get; set; }

            public detaljiRezervacija(Int64 OIB, Int64 ID, Int64 Broj_dana_najma)
            {
                this.OIB = OIB;
                this.ID = ID;
                this.Broj_dana_najma = Broj_dana_najma;
            }

        }

        public static void dodavanjeRezervacija(Int64 OIB, Int64 ID, Int64 Broj_dana_najma)
        {
            String nazivBaze = "RentAcar.db";
            if (!OIB.Equals("") && !ID.Equals("") && !Broj_dana_najma.Equals(""))
            {
                string putDoBaze = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);
                using (SqliteConnection con = new SqliteConnection($"Filename={putDoBaze}"))
                {
                    con.Open();
                    SqliteCommand naredba_insert = new SqliteCommand();
                    naredba_insert.Connection = con; //konekcija naredbe se nalazi u varijabli con
                    naredba_insert.CommandText = "INSERT INTO Rezervacije(OIB, ID, Broj_dana_najma) VALUES(@OIB, @ID, @Broj_dana_najma);";
                    naredba_insert.Parameters.AddWithValue("@OIB", OIB);
                    naredba_insert.Parameters.AddWithValue("@ID", ID);
                    naredba_insert.Parameters.AddWithValue("@Broj_dana_najma", Broj_dana_najma);

                    naredba_insert.ExecuteReader();
                    con.Close();
                }
            }
        }

        public static void brisanjeRezervacija(Int64 OIB)
        {
            String nazivBaze = "RentAcar.db";
            string putDoBaze = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);
            using (SqliteConnection con = new SqliteConnection($"Filename={putDoBaze}"))
            {
                con.Open();
                SqliteCommand naredba_alter = new SqliteCommand();
                naredba_alter.Connection = con; //konekcija naredbe se nalazi u varijabli con
                //naredba_alter.CommandText = "SELECT Rezervacije.OIB, " +
                //                          "FROM employees" +
                //                           "INNER JOIN positions" +
                //                            "ON employees.position_id = positions.position_id;";

                naredba_alter.ExecuteReader();
                con.Close();
            }
        }

        public static void izbrisi3()
        {
            String nazivBaze = "RentAcar.db";
            String pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                String naredba_delete = "DELETE FROM Rezervacije";
                SqliteCommand cmd_getAllRec = new SqliteCommand(naredba_delete, con);
                SqliteDataReader reader = cmd_getAllRec.ExecuteReader();
                con.Close();
            }
        }

        public static List<detaljiRezervacija> DohvatSvihPodataka3()
        {
            String nazivBaze = "RentAcar.db";

            List<detaljiRezervacija> rezervacijeList = new List<detaljiRezervacija>();
            String pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, nazivBaze);
            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                String naredba_select = "SELECT * FROM Rezervacije";
                SqliteCommand cmd_getAllRec = new SqliteCommand(naredba_select, con);

                SqliteDataReader reader = cmd_getAllRec.ExecuteReader();

                while (reader.Read()) //dok je moguce citati iz baze cita
                {
                    rezervacijeList.Add(new detaljiRezervacija(reader.GetInt64(0), reader.GetInt64(1), reader.GetInt64(2)));
                }
                con.Close();
            }
            return rezervacijeList;
        }

    }
}
