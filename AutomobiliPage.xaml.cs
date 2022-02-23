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
    public sealed partial class AutomobiliPage : Page
    {
        public AutomobiliPage()
        {
            this.InitializeComponent();
        }
        private void button_back_Click1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void button_prijavi_se_Click(object sender, RoutedEventArgs e)
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
                    textbox_provjera_ID.Text = "";
                    Rent_a_car_DB.dodavanjeAutomobila(Convert.ToInt64(ID), model, Convert.ToInt64(godina), (decimal)Convert.ToDouble(cijena_po_danu), Convert.ToInt64(količina));
                    pregledautomobila.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka2();
                }
            }
            else
            {
                textbox_provjera_ID.Text = "Niste unijeli sve podatke ili ste ih unijeli pogrešno.";
            }
            pregledautomobila.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka2();
        }
        private void button_izbrisi_podatke_Click1(object sender, RoutedEventArgs e)
        {
            Rent_a_car_DB.izbrisi2();
            pregledautomobila.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka2();
        }
        private void button_izbrisi_odreden_podatak_Click1(object sender, RoutedEventArgs e)
        {
            //Int64 oib = Convert.ToInt64(textbox_oib_delete.Text);
            if (textbox_ID_delete.Text != "")
            {
                Rent_a_car_DB.brisanjeAutomobila(Convert.ToInt64(textbox_ID_delete.Text));
                pregledautomobila.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka2();
            }
            else
            {
                textbox_provjera_ID_delete.Text = "Ovaj ID ne postoji!";
            }
        }
    }
}
