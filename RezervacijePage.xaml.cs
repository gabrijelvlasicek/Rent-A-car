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
    public sealed partial class RezervacijePage : Page
    {
        public RezervacijePage()
        {
            this.InitializeComponent();
            pregledrezervacija.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka3();

        }

        private void button_back_Click1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void button_prijavi_se_Click(object sender, RoutedEventArgs e)
        {
            pregledrezervacija.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka3();

            if (textbox_oib.Text.Length == 11)
            {
                if (textbox_oib.Text != "" && textbox_id_auta.Text != "" && textbox_broj_dana.Text != "")
                {
                    Rent_a_car_DB.dodavanjeRezervacija(Convert.ToInt64(textbox_oib.Text), Convert.ToInt64(textbox_id_auta), Convert.ToInt64(textbox_broj_dana.Text));
                    textbox_oib.Text = "";
                    textblock_id_auta.Text = "";
                    textbox_broj_dana.Text = "";
                    pregledrezervacija.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka3();
                }
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Niste unjeli sve podatke ili ste ih unjeli pogrešno.", "Pogreška");
                await dialog.ShowAsync();
                //textbox_provjera_oib.Text = "Niste unjeli sve podatke ili ste ih unjeli pogrešno.";
            }

            pregledrezervacija.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka3();
        }

        private void button_izbrisi_podatke_Click(object sender, RoutedEventArgs e)
        {
            Rent_a_car_DB.izbrisi3();
            pregledrezervacija.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka3();
        }

        private async void button_izbrisi_odreden_podatak_Click(object sender, RoutedEventArgs e)
        {
            //Int64 oib = Convert.ToInt64(textbox_oib_delete.Text);
            if (textbox_oib_delete.Text != "" && textbox_oib_delete.Text.Length == 11)
            {
                Rent_a_car_DB.brisanjeRezervacija(Convert.ToInt64(textbox_oib_delete.Text));
                pregledrezervacija.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka3();
                textbox_oib_delete.Text = "";
            }
            else if (textbox_oib.Text.Length < 11 || textbox_oib.Text.Length > 11)
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
