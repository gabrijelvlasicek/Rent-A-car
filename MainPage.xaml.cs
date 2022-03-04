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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Rent_a_car
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            //Rent_a_car_DB.izbrisi();
            //Rent_a_car_DB.dodavanjeKlijenta(11111178910, "DEAN", "VIDOVIC", "A. NEMCICA 12", "30.10.2004.");
            //pogledkorisnika.ItemsSource = Rent_a_car_DB.DohvatSvihPodataka();
        }

        private void button_logout_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));
        }

        private void button_klijenti_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(KlijentiPage));
        }

        private void button_auti_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AutomobiliPage));
        }

        private void button_rezervacije_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RezervacijePage));
        }
    }
}
