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
    public sealed partial class AutomobiliPage : Page
    {
        public AutomobiliPage()
        {
            this.InitializeComponent();
            pregledautomobila.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka2();
        }
        private void button_back_Click1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private async void button_prijavi_se_Click(object sender, RoutedEventArgs e)
        {
            String ID = textbox_ID.Text;
            String model = textbox_model.Text.ToUpper();
            String godina = textbox_godina.Text.ToUpper();
            String cijena_po_danu = textbox_cijena_po_danu.Text.ToUpper();
            String količina = textbox_količina.Text.ToUpper();

            if (textbox_ID.Text.Length == 5)
            {
                if (textbox_ID.Text != "" && model != "" && godina != "" && cijena_po_danu != "" && količina != "")
                {
                    Rent_a_car_DB.dodavanjeAutomobila(Convert.ToInt64(ID), model, Convert.ToInt64(godina), (decimal)Convert.ToDouble(cijena_po_danu), Convert.ToInt64(količina));
                    pregledautomobila.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka2();
                    textbox_ID.Text = "";
                    textbox_model.Text = "";
                    textbox_godina.Text = "";
                    textbox_cijena_po_danu.Text = "";
                    textbox_količina.Text = "";
                }
            }
            else
            {
                MessageDialog dialog = new MessageDialog("Niste unijeli sve podatke ili ste ih unijeli pogrešno.", "Pogreška");
                await dialog.ShowAsync();
            }
            pregledautomobila.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka2();
        }
        private void button_izbrisi_podatke_Click1(object sender, RoutedEventArgs e)
        {
            Rent_a_car_DB.izbrisi2();
            pregledautomobila.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka2();
        }
        private async void button_izbrisi_odreden_podatak_Click1(object sender, RoutedEventArgs e)
        {

            if (textbox_ID_delete.Text != "" && textbox_ID_delete.Text.Length == 5)
            {
                Rent_a_car_DB.brisanjeAutomobila(Convert.ToInt64(textbox_ID_delete.Text));
                pregledautomobila.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka2();
                textbox_ID_delete.Text = "";
            }
            else if (textbox_ID_delete.Text.Length < 11 || textbox_ID_delete.Text.Length > 11)
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
