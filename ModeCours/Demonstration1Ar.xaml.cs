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
    /// Interaction logic for Demonstration1Ar.xaml
    /// </summary>
    public partial class Demonstration1Ar : Page
    {
        public Demonstration1Ar()
        {
            InitializeComponent();
        }
        private void Button_Click_Svt_Demonstration1(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration2Ar());
            ViexBox_Demonstration1Ar.Visibility = Visibility.Hidden;
        }

        private void Button_Click_Prcdt_Demonstration1(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new P3CourAxeAr());
            ViexBox_Demonstration1Ar.Visibility = Visibility.Hidden;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            etape1.Position = TimeSpan.Zero;
            etape1.LoadedBehavior = MediaState.Play;
        }
    }
}
