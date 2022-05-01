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
using OUI_Non;
using Projet2Cp;
using MAINPAGE;
namespace Project
{
    
    public partial class PagesNiveaux : Page
    {
        public static Boolean btn_centrale_is_clicked = false;
        public static Boolean btn_axiale_is_clicked = true;
        public static Boolean btn_niveau1_is_clicked = true;
        public static Boolean btn_niveau2_is_clicked = false;
        public static Boolean btn_niveau3_is_clicked = false;
        public string path;
        public int niveau = 1; 


        public PagesNiveaux()
        {
            InitializeComponent();
        }
        private void ButtonAxiale_Click(object sender, RoutedEventArgs e)
        {
            btn_axiale_is_clicked = true;
            btn_centrale_is_clicked = false;
            ButtonAxiale.Style = (Style)Application.Current.FindResource("ButtonAxial");
            ButtonAxiale.Foreground = Brushes.White;
            ButtonCentrale.Style = (Style)Application.Current.FindResource("ButtonCentral");
            ButtonCentrale.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#009400");
            trouverHaja.Text = "Trouver les axes";
        }

        private void ButtonCentrale_Click(object sender, RoutedEventArgs e)
        {
            btn_centrale_is_clicked = true;
            btn_axiale_is_clicked = false;

            ButtonAxiale.Style = (Style)Application.Current.FindResource("ButtonCentral");
            ButtonAxiale.Foreground = (SolidColorBrush)new BrushConverter().ConvertFromString("#009400"); 
            ButtonCentrale.Style = (Style)Application.Current.FindResource("ButtonAxial");
            ButtonCentrale.Foreground = Brushes.White;
            trouverHaja.Text = "Points primes";

        }

        private void BtnNiveau1_Click(object sender, RoutedEventArgs e)
        {
            niveau = 1;
            btn_niveau1_is_clicked = true;
            btn_niveau2_is_clicked = false;
            btn_niveau3_is_clicked = false;
            BtnNiveau1.Margin = new System.Windows.Thickness(0, 0, 80, 0);
            BtnNiveau2.Margin = new System.Windows.Thickness(0, 20, 80, 0);
            BtnNiveau3.Margin = new System.Windows.Thickness(0, 20, 0, 0);
            BorderContainer.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#A2DBA1");
            BtnQuiz.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#82AF81");
            BtnOuiNon.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#82AF81");
            BtnTrouverLesAxes.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#82AF81");
            BtnDessinerLeSymetr.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#82AF81");
        }

        private void BtnNiveau2_Click(object sender, RoutedEventArgs e)
        {
            niveau = 2;
            btn_niveau2_is_clicked = true;
            btn_niveau1_is_clicked = false;
            btn_niveau3_is_clicked = false;
            BtnNiveau1.Margin = new System.Windows.Thickness(0, 20, 80, 0);
            BtnNiveau2.Margin = new System.Windows.Thickness(0, 0, 80, 0);
            BtnNiveau3.Margin = new System.Windows.Thickness(0, 20, 0, 0);
            BorderContainer.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#ffde59");
            BtnQuiz.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFCC00");
            BtnOuiNon.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFCC00");
            BtnTrouverLesAxes.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFCC00");
            BtnDessinerLeSymetr.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FFCC00");
        }

        private void BtnNiveau3_Click(object sender, RoutedEventArgs e)
        {
            niveau = 3;
            btn_niveau3_is_clicked = true;
            btn_niveau1_is_clicked = false;
            btn_niveau2_is_clicked = false;
            BtnNiveau1.Margin = new System.Windows.Thickness(0, 20, 80, 0);
            BtnNiveau2.Margin = new System.Windows.Thickness(0, 20, 80, 0);
            BtnNiveau3.Margin = new System.Windows.Thickness(0, 0, 0, 0);
            BorderContainer.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#f26666");
            BtnQuiz.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#EC3D3D");
            BtnOuiNon.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#EC3D3D");
            BtnTrouverLesAxes.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#EC3D3D");
            BtnDessinerLeSymetr.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#EC3D3D");
        }

        private void BtnQuiz_Click(object sender, RoutedEventArgs e)
        {
            string path1 ,path2, path3 ;
            path1 = "Exercices\\Quiz\\Quiz1_niveau";
            path2 = "Exercices\\Quiz\\Quiz2_niveau";
            path3 = "Exercices\\Quiz\\Quiz3_niveau";
            switch (niveau)
            {

                case 1:
                    {
                        path1 += "1";
                        path2 += "1";
                        path3 += "1";
                        break;
                    }
                case 2:
                    {
                        path1 += "2";
                        path2 += "2";
                        path3 += "2";
                        break;
                    }
                case 3:
                    {
                        path1 += "3";
                        path2 += "3";
                        path3 += "3";
                        break;
                    }
            }
            if (btn_axiale_is_clicked) { 
                path1 += "_axiale.txt"; 
                path2 += "_axiale.txt"; 
                path3 += "_axiale.txt"; 
            }
            else { 
                path1 += "_centrale.txt"; 
                path2 += "_centrale.txt"; 
                path3 += "_centrale.txt"; 
            }
            
            MainQuizWindow pageQuiz = new MainQuizWindow(path1, path2, path3);
            MainWindow.MainFrame.NavigationService.Navigate(pageQuiz);
        }

        private void BtnTrouverLesAxes_Click(object sender, RoutedEventArgs e)
        {
            path = "Exercices";
            if (btn_axiale_is_clicked) path += "\\TrouverAxes";
            else path += "\\PointsPrimes";

            switch (niveau)
            {

                case 1:
                    path += "\\niveau1";
                    break;
                case 2:
                    path += "\\niveau2";
                    break;
                case 3:
                    path += "\\niveau3";
                    break;
            }


            if (btn_axiale_is_clicked) MainWindow.MainFrame.NavigationService.Navigate(new PageTrouverAxes(path));
            else {

                switch (niveau)
                {

                    case 1:
                        MainWindow.MainFrame.NavigationService.Navigate(new PagePointsPrimesNiveau1(path));
                        break;
                    case 2:
                        MainWindow.MainFrame.NavigationService.Navigate(new PagePointsPrimesNiveau2(path));
                        break;
                    case 3:
                        MainWindow.MainFrame.NavigationService.Navigate(new PagePointsPrimesNiveau3(path));
                        break;
                }
                
                    };

        }

        private void BtnOuiNon_Click(object sender, RoutedEventArgs e)
        {
            path = "Exercices\\OuiNon";

            if (btn_axiale_is_clicked) path += "\\axiale";
            else path += "\\centrale";

            switch (niveau)
            {

                case 1: path += "\\Niveau1_assets";
                    break;
                case 2:
                    path += "\\Niveau2_assets";
                    break;
                case 3:
                    path += "\\Niveau3_assets";
                    break;
            }


            

           if (MainWindow.modeEns) MainWindow.MainFrame.NavigationService.Navigate(new Page1(0, path));
           else MainWindow.MainFrame.NavigationService.Navigate(new Page_mode_eleve(0, path));
        }

        private void BtnDessinerLeSymetr_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.modeLibre = false;
           
            MainWindow.MainFrame.NavigationService.Navigate(new LibreExoEns());
        }
























        private void BtnNiveau1_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnNiveau1.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#d1f2d0"));
        }
        private void BtnNiveau1_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnNiveau1.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#A2DBA1"));
        }

        private void BtnNiveau2_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnNiveau2.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffeda6"));
        }

        private void BtnNiveau2_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnNiveau2.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffde59"));
        }

        private void BtnNiveau3_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnNiveau3.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffabab"));
        }

        private void BtnNiveau3_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnNiveau3.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f26666"));
        }

        private void BtnQuiz_MouseLeave(object sender, MouseEventArgs e)
        {
            if (btn_niveau1_is_clicked)
            {
                BtnQuiz.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#82AF81"));
            }
            if (btn_niveau2_is_clicked)
            {
                BtnQuiz.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            }
            if (btn_niveau3_is_clicked)
            {
                BtnQuiz.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EC3D3D"));
            }
        }

        private void BtnQuiz_MouseEnter(object sender, MouseEventArgs e)
        {
            if (btn_niveau1_is_clicked)
            {
                BtnQuiz.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#d1f2d0"));
            }
            if (btn_niveau2_is_clicked)
            {
                BtnQuiz.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffeda6"));
            }
            if (btn_niveau3_is_clicked)
            {
                BtnQuiz.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffabab"));
            }
        }

        private void BtnTrouverLesAxes_MouseEnter(object sender, MouseEventArgs e)
        {
            if (btn_niveau1_is_clicked)
            {
                BtnTrouverLesAxes.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#d1f2d0"));
            }
            if (btn_niveau2_is_clicked)
            {
                BtnTrouverLesAxes.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffeda6"));
            }
            if (btn_niveau3_is_clicked)
            {
                BtnTrouverLesAxes.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffabab"));
            }
        }

        private void BtnTrouverLesAxes_MouseLeave(object sender, MouseEventArgs e)
        {
            if (btn_niveau1_is_clicked)
            {
                BtnTrouverLesAxes.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#82AF81"));
            }
            if (btn_niveau2_is_clicked)
            {
                BtnTrouverLesAxes.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            }
            if (btn_niveau3_is_clicked)
            {
                BtnTrouverLesAxes.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EC3D3D"));
            }
        }

        private void BtnOuiNon_MouseEnter(object sender, MouseEventArgs e)
        {
            if (btn_niveau1_is_clicked)
            {
                BtnOuiNon.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#d1f2d0"));
            }
            if (btn_niveau2_is_clicked)
            {
                BtnOuiNon.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffeda6"));
            }
            if (btn_niveau3_is_clicked)
            {
                BtnOuiNon.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffabab"));
            }
        }

        private void BtnOuiNon_MouseLeave(object sender, MouseEventArgs e)
        {
            if (btn_niveau1_is_clicked)
            {
                BtnOuiNon.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#82AF81"));
            }
            if (btn_niveau2_is_clicked)
            {
                BtnOuiNon.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            }
            if (btn_niveau3_is_clicked)
            {
                BtnOuiNon.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EC3D3D"));
            }
        }

        private void BtnDessinerLeSymetr_MouseEnter(object sender, MouseEventArgs e)
        {
            if (btn_niveau1_is_clicked)
            {
                BtnDessinerLeSymetr.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#d1f2d0"));
            }
            if (btn_niveau2_is_clicked)
            {
                BtnDessinerLeSymetr.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffeda6"));
            }
            if (btn_niveau3_is_clicked)
            {
                BtnDessinerLeSymetr.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffabab"));
            }
        }

        private void BtnDessinerLeSymetr_MouseLeave(object sender, MouseEventArgs e)
        {
            if (btn_niveau1_is_clicked)
            {
                BtnDessinerLeSymetr.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#82AF81"));
            }
            if (btn_niveau2_is_clicked)
            {
                BtnDessinerLeSymetr.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            }
            if (btn_niveau3_is_clicked)
            {
                BtnDessinerLeSymetr.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EC3D3D"));
            }
        }
    }
}
