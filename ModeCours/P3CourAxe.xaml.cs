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
    /// Interaction logic for P3CourAxe.xaml
    /// </summary>
    public partial class P3CourAxe : Page
    {
        public P3CourAxe()
        {
            InitializeComponent();
        }

        private void Button_Click_Svt_CA_P3(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration1());
            ViexBox_P3CourAxe.Visibility = Visibility.Hidden;
        }

        private void Button_Click_Prcdt_CA_P2(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new P2CourAxe());
            ViexBox_P3CourAxe.Visibility = Visibility.Hidden;
        }
    }
}
