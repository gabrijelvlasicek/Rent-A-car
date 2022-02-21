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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private void button_prijavi_se_Click(object sender, RoutedEventArgs e)
        {
            string kor_ime = textbox_korisnicko_ime.Text;
            string password = passwordbox_lozinka.Password;

            if(kor_ime == "admin" && password == "admin")
            {
                this.Frame.Navigate(typeof(MainPage));
            }
            else
            {
                textblock_kriva_sifra_ili_ime.Text = "Krivo korisničko ime ili loznika.";
            }
        }
    }
}
