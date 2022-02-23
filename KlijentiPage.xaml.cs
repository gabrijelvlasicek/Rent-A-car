using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Rent_a_car
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KlijentiPage : Page
    {

        public KlijentiPage()
        {
            this.InitializeComponent();
        }
        private void button_back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        //unos podataka u databazu (gumb)
        private void button_prijavi_se_Click(object sender, RoutedEventArgs e)
        {
            //Rent_a_car_DB.izbrisi();
            //Rent_a_car_DB.dodavanjeKlijenta(11111178910, "DEAN", "VIDOVIC", "A. NEMCICA 12", "30.10.2004.");
            //pogledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();

            pregledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();

            //Int64 oib = Convert.ToInt64(textbox_oib.Text);
            String oib = textbox_oib.Text;
            String ime = textbox_ime.Text.ToUpper();
            String prezime = textbox_prezime.Text.ToUpper();
            String adresa = textbox_adresa.Text.ToUpper();
            String rodenje = textbox_rodenje.Text.ToUpper();


            if (textbox_oib.Text.Length == 11)
            {
                if (textbox_oib.Text != "" && ime != "" && prezime != "" && adresa != "" && rodenje != "")
                {
                    textbox_provjera_oib.Text = "";
                    Rent_a_car_DB.dodavanjeKlijenta(Convert.ToInt64(oib), ime, prezime, adresa, rodenje);
                    pregledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();
                }
            }
            else
            {
                textbox_provjera_oib.Text = "Niste unjeli sve podatke ili ste ih unjeli pogrešno.";
            }

            pregledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();
        }

        private void button_izbrisi_podatke_Click(object sender, RoutedEventArgs e)
        {
            Rent_a_car_DB.izbrisi();
            pregledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();
        }

        private void button_izbrisi_odreden_podatak_Click(object sender, RoutedEventArgs e)
        {
            //Int64 oib = Convert.ToInt64(textbox_oib_delete.Text);
            if(textbox_oib_delete.Text != "")
            {
                Rent_a_car_DB.brisanjeKlijenta(Convert.ToInt64(textbox_oib_delete.Text));
                pregledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();
            }
            else
            {
                textbox_provjera_oib_delete.Text = "Ovaj OIB ne postoji!";
            }
            
        }
    }
}
