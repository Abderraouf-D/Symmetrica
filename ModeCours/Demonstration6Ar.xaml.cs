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
    /// Interaction logic for Demonstration6Ar.xaml
    /// </summary>
    public partial class Demonstration6Ar : Page
    {
        public Demonstration6Ar()
        {
            InitializeComponent();
        }
        private void Button_Click_Svt_Demonstration6(object sender, RoutedEventArgs e)
        {
            ViexBox_Demonstration6Ar.Visibility = Visibility.Hidden;
            MainWindow.MainFrame.NavigationService.Navigate(new P4CoursCenAr());
        }

        private void Button_Click_Prcdt_Demonstration6(object sender, RoutedEventArgs e)
        {
            ViexBox_Demonstration6Ar.Visibility = Visibility.Hidden;
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration5Ar());
        }
        private void etape6_MediaEnded(object sender, RoutedEventArgs e)
        {
            etape6Cen.Position = TimeSpan.Zero;
            etape6Cen.LoadedBehavior = MediaState.Play;
        }
    }
}
