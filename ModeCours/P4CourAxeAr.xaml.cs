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
    /// Interaction logic for P4CourAxeAr.xaml
    /// </summary>
    public partial class P4CourAxeAr : Page
    {
        public P4CourAxeAr()
        {
            InitializeComponent();
        }
        private void Button_Click_Svt_CA_P4(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new P5CourAxeAr());
            ViexBox_P4CourAxeAr.Visibility = Visibility.Hidden;
        }

        private void Button_Click_Prcdt_CA_P4(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration3Ar());
            ViexBox_P4CourAxeAr.Visibility = Visibility.Hidden;
        }
    }
}
