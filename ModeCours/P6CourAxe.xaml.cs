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
    /// Interaction logic for P6CourAxe.xaml
    /// </summary>
    public partial class P6CourAxe : Page
    {
        public P6CourAxe()
        {
            InitializeComponent();
        }

        private void Button_Click_Prcdt_CA_P6(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new P5CourAxe());
            ViexBox_P6CourAxe.Visibility = Visibility.Hidden;
        }
    }
}
