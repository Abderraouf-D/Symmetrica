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
    /// Interaction logic for Demonstration5.xaml
    /// </summary>
    public partial class Demonstration5 : Page
    {
        public Demonstration5()
        {
            InitializeComponent();
        }

        private void Button_Click_Svt_Demonstration5(object sender, RoutedEventArgs e)
        {
            ViexBox_Demonstration5.Visibility = Visibility.Hidden;
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration6());
            if (!MainWindow.modeEns) MainWindow.eleve.incProgress();

        }

        private void Button_Click_Prcdt_Demonstration5(object sender, RoutedEventArgs e)
        {
            ViexBox_Demonstration5.Visibility = Visibility.Hidden;
            MainWindow.MainFrame.NavigationService.Navigate(new Demonstration4());
            if (!MainWindow.modeEns) MainWindow.eleve.decProgress();

        }
        private void etape2_MediaEnded(object sender, RoutedEventArgs e)
        {
            etape2Cen.Position = TimeSpan.Zero;
            etape2Cen.LoadedBehavior = MediaState.Play;
        }
    }
}
