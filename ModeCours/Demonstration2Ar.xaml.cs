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
    /// Interaction logic for Demonstration2Ar.xaml
    /// </summary>
    public partial class Demonstration2Ar : Page
    {
        public Demonstration2Ar()
        {
            InitializeComponent();
        }
        private void Button_Click_Svt_Demonstration2(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration3Ar());
            ViexBox_Demonstration2Ar.Visibility = Visibility.Hidden;
        }

        private void Button_Click_Prcdt_Demonstration2(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration1Ar());
            ViexBox_Demonstration2Ar.Visibility = Visibility.Hidden;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            etape2.Position = TimeSpan.Zero;
            etape2.LoadedBehavior = MediaState.Play;
        }
    }
}
