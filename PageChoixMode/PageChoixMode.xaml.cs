using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Projet2Cp; 
namespace Project
{
    /// <summary>
    /// Interaction logic for PageChoixMode.xaml
    /// </summary>s
    public partial class PageChoixMode : Page
    {
        bool francais = true;
        bool modeEns;

        public PageChoixMode()
        {
            InitializeComponent();
            passwd.Clear();
            eleve.Clear();
            eleve.TextAlignment = TextAlignment.Center;
            passwd.HorizontalContentAlignment = HorizontalAlignment.Center;
            eleve.VerticalContentAlignment = VerticalAlignment.Center;
            
        }

      

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(eleve.Text) && String.IsNullOrEmpty( passwd.Password ))
            {
                MessageBox.Show("Veuillez remplir l'un des champs!");
            }
            else
            {
               

                if (modeEns) symmetrica.symmetricaFrm.NavigationService.Navigate(new MainWindow(modeEns, francais, null));
                else symmetrica.symmetricaFrm.NavigationService.Navigate(new MainWindow(modeEns, francais, new Eleve(eleve.Text, 0)));
            }
        }

        private void Fr_Click(object sender, RoutedEventArgs e)
        {
            francais = true;
            Fr.Style = (Style)Application.Current.FindResource("ButtonCentralJaune");
        
            Ar.Style = (Style)Application.Current.FindResource("ButtonCentral");
        }
        private void Ar_Click(object sender, RoutedEventArgs e)
        {
            francais = false;
            Ar.Style = (Style)Application.Current.FindResource("ButtonCentralJaune");

            Fr.Style = (Style)Application.Current.FindResource("ButtonCentral");
        }

       

        private void passwd_GotFocus(object sender, RoutedEventArgs e)
        {
            eleve.Clear();
            modeEns = true;

        }

        private void eleve_GotFocus(object sender, RoutedEventArgs e)
        {
            passwd.Clear();
            modeEns = false;
        }
    }
}
