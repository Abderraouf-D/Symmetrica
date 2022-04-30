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

namespace Project
{
    
    public partial class PagesNiveaux : Page
    {
        public static Boolean btn_centrale_is_clicked = false;
        public static Boolean btn_axiale_is_clicked = true;
        public static Boolean btn_niveau1_is_clicked = true;
        public static Boolean btn_niveau2_is_clicked = false;
        public static Boolean btn_niveau3_is_clicked = false;
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
            ButtonCentrale.Foreground = Brushes.Black;
            trouverHaja.Text = "Trouver les axes";
        }

        private void ButtonCentrale_Click(object sender, RoutedEventArgs e)
        {
            btn_centrale_is_clicked = true;
            btn_axiale_is_clicked = false;

            ButtonAxiale.Style = (Style)Application.Current.FindResource("ButtonCentral");
            ButtonAxiale.Foreground = Brushes.Black;
            ButtonCentrale.Style = (Style)Application.Current.FindResource("ButtonAxial");
            ButtonCentrale.Foreground = Brushes.White;
            trouverHaja.Text = "Points primes";

        }

        private void BtnNiveau1_Click(object sender, RoutedEventArgs e)
        {
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
            if (btn_axiale_is_clicked == true)
            {
                if (btn_niveau1_is_clicked == true)
                {
                    MainQuizWindow pageQuiz = new MainQuizWindow("Quiz1_niveau1_axiale.txt", "Quiz2_niveau1_axiale.txt", "Quiz3_niveau1_axiale.txt");
                    MainWindow.MainFrame.NavigationService.Navigate(pageQuiz);
                }
                if (btn_niveau2_is_clicked == true)
                {
                    MainQuizWindow pageQuiz = new MainQuizWindow("Quiz1_niveau2_axiale.txt", "Quiz2_niveau2_axiale.txt", "Quiz3_niveau2_axiale.txt");
                    MainWindow.MainFrame.NavigationService.Navigate(pageQuiz);
                }
                if (btn_niveau3_is_clicked == true)
                {
                    MainQuizWindow pageQuiz = new MainQuizWindow("Quiz1_niveau3_axiale.txt", "Quiz2_niveau3_axiale.txt", "Quiz3_niveau3_axiale.txt");
                    MainWindow.MainFrame.NavigationService.Navigate(pageQuiz);
                }
            }
            if (btn_centrale_is_clicked == true)
            {
                if (btn_niveau1_is_clicked == true)
                {
                    MainQuizWindow pageQuiz = new MainQuizWindow("Quiz1_niveau1_centrale.txt", "Quiz2_niveau1_centrale.txt", "Quiz3_niveau1_centrale.txt");
                    MainWindow.MainFrame.NavigationService.Navigate(pageQuiz);
                }
                if (btn_niveau2_is_clicked == true)
                {
                    MainQuizWindow pageQuiz = new MainQuizWindow("Quiz1_niveau2_centrale.txt", "Quiz2_niveau2_centrale.txt", "Quiz3_niveau2_centrale.txt");
                    MainWindow.MainFrame.NavigationService.Navigate(pageQuiz);
                }
                if (btn_niveau3_is_clicked == true)
                {
                    MainQuizWindow pageQuiz = new MainQuizWindow("Quiz1_niveau3_centrale.txt", "Quiz2_niveau3_centrale.txt", "Quiz3_niveau3_centrale.txt");
                    MainWindow.MainFrame.NavigationService.Navigate(pageQuiz);
                }
            }
        }

        private void BtnTrouverLesAxes_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOuiNon_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.MainFrame.NavigationService.Navigate(new Page_mode_eleve(0, "./Niveau1_assets/img1.txt", "./Niveau1_assets/ans1.txt"));
           
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
