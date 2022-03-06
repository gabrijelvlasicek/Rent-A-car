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
using Windows.UI.Popups;

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
            pregledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();
        }
        private void button_back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        //unos podataka u databazu (gumb)
        private async void button_prijavi_se_Click(object sender, RoutedEventArgs e)
        { 
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
                    Rent_a_car_DB.dodavanjeKlijenta(Convert.ToInt64(oib), ime, prezime, adresa, rodenje);
                    textbox_oib.Text = "";
                    textbox_ime.Text = "";
                    textbox_prezime.Text = "";
                    textbox_adresa.Text = "";
                    textbox_rodenje.Text = "";
                    pregledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();
                }
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Niste unjeli sve podatke ili ste ih unjeli pogrešno.", "Pogreška");
                await dialog.ShowAsync();
                //textbox_provjera_oib.Text = "Niste unjeli sve podatke ili ste ih unjeli pogrešno.";
            }

            pregledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();
        }

        private void button_izbrisi_podatke_Click(object sender, RoutedEventArgs e)
        {
            Rent_a_car_DB.izbrisi();
            pregledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();
        }

        private async void button_izbrisi_odreden_podatak_Click(object sender, RoutedEventArgs e)
        {
            //Int64 oib = Convert.ToInt64(textbox_oib_delete.Text);
            if(textbox_oib_delete.Text != "" && textbox_oib_delete.Text.Length == 11)
            {
                Rent_a_car_DB.brisanjeKlijenta(Convert.ToInt64(textbox_oib_delete.Text));
                pregledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();
                textbox_oib_delete.Text = "";
            }
            else if(textbox_oib.Text.Length < 11 || textbox_oib.Text.Length > 11)
            {
                MessageDialog dialog = new MessageDialog("OIB mora sadržavati 11 brojeva!", "Pogreška");
                await dialog.ShowAsync();
                //textbox_provjera_oib_delete.Text = "Ovaj OIB ne postoji!";
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Ovaj OIB ne postoji!", "Pogreška");
                await dialog.ShowAsync();
            }
            
        }
    }
}
