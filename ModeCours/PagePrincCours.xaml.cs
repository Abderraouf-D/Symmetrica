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
  
    public partial class PagePrincCours : Page
    {
        public static Boolean axiale = true;
        public PagePrincCours()
        {
            InitializeComponent();
        }
        private void ButtonClickAxe(object sender, RoutedEventArgs e)
        {
            navigateCourAxe();
            ph.Visibility = Visibility.Hidden;
            BtnCourAxe.Visibility = Visibility.Hidden;
            BtnCourCen.Visibility = Visibility.Hidden;

        }

        private void BtnCourCen_Click(object sender, RoutedEventArgs e)
        {
            navigateCourCen();
            ph.Visibility = Visibility.Hidden;
            BtnCourAxe.Visibility = Visibility.Hidden;
            BtnCourCen.Visibility = Visibility.Hidden;
            axiale = false;
        }



        private void navigateCourAxe()
        {
            if (!MainWindow.modeEns)
            {
                Page[] pagesCours = new Page[9];
                pagesCours[0] = new P1CourAxe();
                pagesCours[1] = new P2CourAxe();
                pagesCours[2] = new P3CourAxe();
                pagesCours[3] = new Demonstration1();
                pagesCours[4] = new Demonstration2();
                pagesCours[5] = new Demonstration3();
                pagesCours[6] = new P4CourAxe();
                pagesCours[7] = new P5CourAxe();
                pagesCours[8] = new P5CourAxe();
                MainWindow.MainFrame.NavigationService.Navigate(pagesCours[MainWindow.eleve.getProgressAxe()]);
            }
            else
                MainWindow.MainFrame.NavigationService.Navigate(new P1CourAxe());

        }
        private void navigateCourCen()
        {
            if (!MainWindow.modeEns)
            {
                Page[] pagesCours = new Page[9];
                pagesCours[0] = new P1CoursCen();
                pagesCours[1] = new P2CoursCen();
                pagesCours[2] = new P3CoursCen();
                pagesCours[3] = new Demonstration4();
                pagesCours[4] = new Demonstration5();
                pagesCours[5] = new Demonstration6();
                pagesCours[6] = new P4CoursCen();
                pagesCours[7] = new P5CoursCen();
                pagesCours[8] = new P5CoursCen();
                MainWindow.MainFrame.NavigationService.Navigate(pagesCours[MainWindow.eleve.getProgressCen()]);
            }
            else
                MainWindow.MainFrame.NavigationService.Navigate(new P1CoursCen());

        }
    }
}
