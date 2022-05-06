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
    /// Interaction logic for P5CourAxeAr.xaml
    /// </summary>
    public partial class P5CourAxeAr : Page
    {
        public P5CourAxeAr()
        {
            InitializeComponent();
        }
        private void Button_Click_Svt_CA_P5(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new P6CourAxeAr());
            ViexBox_P5CourAxeAr.Visibility = Visibility.Hidden;
            if (!MainWindow.modeEns) MainWindow.eleve.incProgress();

        }

        private void Button_Click_Prcdt_CA_P5(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new P4CourAxeAr());
            ViexBox_P5CourAxeAr.Visibility = Visibility.Hidden;
            if (!MainWindow.modeEns) MainWindow.eleve.decProgress();

        }
    }
}
