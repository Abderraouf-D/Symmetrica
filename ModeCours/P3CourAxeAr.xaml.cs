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
namespace ModeCours
{
    /// <summary>
    /// Interaction logic for P3CourAxeAr.xaml
    /// </summary>
    public partial class P3CourAxeAr : Page
    {
        public P3CourAxeAr()
        {
            InitializeComponent();
        }
        private void Button_Click_Svt_CA_P3(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration1Ar());
            ViexBox_P3CourAxeAr.Visibility = Visibility.Hidden;
        }

        private void Button_Click_Prcdt_CA_P2(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new P2CourAxeAr());
            ViexBox_P3CourAxeAr.Visibility = Visibility.Hidden;
        }
    }
}

