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
        public static Boolean axiale = true;

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
         axiale = false;

            MainWindow.MainFrame.NavigationService.Navigate(new P1CoursCenAr());
            ph.Visibility = Visibility.Hidden;
            BtnCourAxe.Visibility = Visibility.Hidden;
            BtnCourCen.Visibility = Visibility.Hidden;
        }

        private void navigateCourAxe()
        {
            if (!MainWindow.modeEns)
            {
                Page[] pagesCours = new Page[9];
                pagesCours[0] = new P1CourAxeAr();
                pagesCours[1] = new P2CourAxeAr();
                pagesCours[2] = new P3CourAxeAr();
                pagesCours[3] = new Demonstration1Ar();
                pagesCours[4] = new Demonstration2Ar();
                pagesCours[5] = new Demonstration3Ar();
                pagesCours[6] = new P4CourAxeAr();
                pagesCours[7] = new P5CourAxeAr();
                pagesCours[8] = new P5CourAxeAr();
                MainWindow.MainFrame.NavigationService.Navigate(pagesCours[MainWindow.eleve.getProgressAxe()]);
            }
            else
                MainWindow.MainFrame.NavigationService.Navigate(new P1CourAxeAr());

        }
        private void navigateCourCen()
        {
            if (!MainWindow.modeEns)
            {
                Page[] pagesCours = new Page[9];
                pagesCours[0] = new P1CoursCenAr();
                pagesCours[1] = new P2CoursCenAr();
                pagesCours[2] = new P3CoursCenAr();
                pagesCours[3] = new Demonstration4Ar();
                pagesCours[4] = new Demonstration5Ar();
                pagesCours[5] = new Demonstration6Ar();
                pagesCours[6] = new P4CoursCenAr();
                pagesCours[7] = new P5CoursCenAr();
                pagesCours[8] = new P5CoursCenAr();
                MainWindow.MainFrame.NavigationService.Navigate(pagesCours[MainWindow.eleve.getProgressCen()]);
            }
            else
                MainWindow.MainFrame.NavigationService.Navigate(new P1CoursCenAr());

        }
    }
}
