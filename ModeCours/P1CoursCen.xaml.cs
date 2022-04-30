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
    /// Interaction logic for P1CoursCen.xaml
    /// </summary>
    public partial class P1CoursCen : Page
    {
        public P1CoursCen()
        {
            InitializeComponent();
        }
        private void Button_Click_Prcdt_CC_P1(object sender, RoutedEventArgs e)
        {
            ViexBox_P1CoursCen.Visibility = Visibility.Hidden;
            MainWindow.MainFrame.NavigationService.Navigate(new PagePrincCours());

        }

        private void Button_Click_Svt_CC_P1(object sender, RoutedEventArgs e)
        {
            ViexBox_P1CoursCen.Visibility = Visibility.Hidden;
            MainWindow.MainFrame.NavigationService.Navigate(new P2CoursCen());

        }

        private void rotation_MediaEnded(object sender, RoutedEventArgs e)
        {
            rotation.Position = TimeSpan.Zero;
            rotation.LoadedBehavior = MediaState.Play;
        }
    }
}
