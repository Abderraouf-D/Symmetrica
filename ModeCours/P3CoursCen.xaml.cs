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
    /// Interaction logic for P3CoursCen.xaml
    /// </summary>
    public partial class P3CoursCen : Page
    {
        public P3CoursCen()
        {
            InitializeComponent();
        }
        private void Button_Click_Prcdt_CC_P3(object sender, RoutedEventArgs e)
        {
            ViexBox_P3CoursCen.Visibility = Visibility.Hidden;
            MainWindow.MainFrame.NavigationService.Navigate(new P2CoursCen());
            if (!MainWindow.modeEns) MainWindow.eleve.decProgress();


        }

        private void Button_Click_Svt_CC_P3(object sender, RoutedEventArgs e)
        {
            ViexBox_P3CoursCen.Visibility = Visibility.Hidden;
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration4());
            if (!MainWindow.modeEns) MainWindow.eleve.incProgress();



        }
    }
}
