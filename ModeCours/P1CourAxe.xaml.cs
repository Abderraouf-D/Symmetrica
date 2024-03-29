﻿using System;
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
    /// Interaction logic for P1CourAxe.xaml
    /// </summary>
    public partial class P1CourAxe : Page
    {

        public P1CourAxe()
        {
            InitializeComponent();
        }

        private void Button_Click_Prcdt_CA_P1 (object sender, RoutedEventArgs e)
        {
            ViexBox_P1CourAxe.Visibility = Visibility.Hidden;
            MainWindow.MainFrame.NavigationService.Navigate(new PagePrincCours());
            if (!MainWindow.modeEns) MainWindow.eleve.decProgress();

        }

        private void Button_Click_Svt_CA_P1(object sender, RoutedEventArgs e)
        {

            MainWindow.MainFrame.NavigationService.Navigate(new P2CourAxe());
            ViexBox_P1CourAxe.Visibility = Visibility.Hidden;
            if (!MainWindow.modeEns) MainWindow.eleve.incProgress();
        

        }

        private void etape1_MediaEnded(object sender, RoutedEventArgs e)
        {
            videoplier.Position = TimeSpan.Zero;
            videoplier.LoadedBehavior = MediaState.Play;
        }

       
    }
}
