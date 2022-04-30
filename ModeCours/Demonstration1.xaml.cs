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
    /// Interaction logic for Demonstration1.xaml
    /// </summary>
    public partial class Demonstration1 : Page
    {
        public Demonstration1()
        {
            InitializeComponent();
        }
        private void Button_Click_Svt_Demonstration1(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration2());
            ViexBox_Demonstration1.Visibility = Visibility.Hidden;
        }

        private void Button_Click_Prcdt_Demonstration1(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new P3CourAxe());
            ViexBox_Demonstration1.Visibility = Visibility.Hidden;
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            etape1.Position = TimeSpan.Zero;
            etape1.LoadedBehavior = MediaState.Play;
        }
    }
}
