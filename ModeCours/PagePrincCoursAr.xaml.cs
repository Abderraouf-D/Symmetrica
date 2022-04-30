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
    /// Interaction logic for PagePrincCoursAr.xaml
    /// </summary>
    public partial class PagePrincCoursAr : Page
    {
        public PagePrincCoursAr()
        {
            InitializeComponent();
        }
        private void ButtonClickAxe(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new P1CourAxeAr());
            ph.Visibility = Visibility.Hidden;
            BtnCourAxe.Visibility = Visibility.Hidden;
            BtnCourCen.Visibility = Visibility.Hidden;

        }

        private void BtnCourCen_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new P1CoursCenAr());
            ph.Visibility = Visibility.Hidden;
            BtnCourAxe.Visibility = Visibility.Hidden;
            BtnCourCen.Visibility = Visibility.Hidden;
        }
    }
}
