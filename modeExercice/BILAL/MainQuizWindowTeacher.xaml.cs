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
using System.IO;
using Projet2Cp;

namespace Project
{

    public partial class MainQuizWindowTeacher : Page
    {
        public int suivantCounter = 1;
        public String fich1;
        public String fich2;
        public String fich3;
        public String a;

        public MainQuizWindowTeacher(String fich1 , String fich2 , String fich3)
        {
            InitializeComponent();
            this.fich1 = fich1;
            this.fich2 = fich2;
            this.fich3 = fich3;
            StreamReader sr = new StreamReader(fich1);
            a = sr.ReadLine();
            Question.Text = sr.ReadLine();
            ChoixA.Text = sr.ReadLine();
            ChoixB.Text = sr.ReadLine();
            ChoixC.Text = sr.ReadLine();
            ChoixD.Text = sr.ReadLine();
        }
        private void ButtonSuivant_Click(object sender, RoutedEventArgs e)
        {
            
            if (suivantCounter < 3)
            {
                suivantCounter++;
            }
            if (suivantCounter <= 3 && suivantCounter == 2)
            {
                StreamReader sr = new StreamReader(fich2);
                a = sr.ReadLine();
                Question.Text = sr.ReadLine();
                ChoixA.Text = sr.ReadLine();
                ChoixB.Text = sr.ReadLine();
                ChoixC.Text = sr.ReadLine();
                ChoixD.Text = sr.ReadLine();
                btnSuivant.Visibility = Visibility.Visible;
                btnPrecedent.Visibility = Visibility.Visible;

            }
            if (suivantCounter <= 3 && suivantCounter == 3)
            {
                StreamReader sr = new StreamReader(fich3);
                a = sr.ReadLine();
                Question.Text = sr.ReadLine();
                ChoixA.Text = sr.ReadLine();
                ChoixB.Text = sr.ReadLine();
                ChoixC.Text = sr.ReadLine();
                ChoixD.Text = sr.ReadLine();
                btnSuivant.Visibility = Visibility.Hidden;
                btnPrecedent.Visibility = Visibility.Visible;
            }
        }

        private void ButtonPrecedent_Click(object sender, RoutedEventArgs e)
        {
           
            if (suivantCounter <= 3 && suivantCounter == 2)
            {
                suivantCounter--;
                StreamReader sr = new StreamReader(fich1);
                a = sr.ReadLine();
                Question.Text = sr.ReadLine();
                ChoixA.Text = sr.ReadLine();
                ChoixB.Text = sr.ReadLine();
                ChoixC.Text = sr.ReadLine();
                ChoixD.Text = sr.ReadLine();
                btnPrecedent.Visibility = Visibility.Hidden;
                btnSuivant.Visibility = Visibility.Visible;
            }
            if (suivantCounter <= 3 && suivantCounter == 3)
            {
                suivantCounter--;
                StreamReader sr = new StreamReader(fich2);
                a = sr.ReadLine();
                Question.Text = sr.ReadLine();
                ChoixA.Text = sr.ReadLine();
                ChoixB.Text = sr.ReadLine();
                ChoixC.Text = sr.ReadLine();
                ChoixD.Text = sr.ReadLine();
                btnPrecedent.Visibility = Visibility.Visible;
                btnSuivant.Visibility = Visibility.Visible;
            }
        }

        private void btn_enregistrer_Click(object sender, RoutedEventArgs e)
        {
            String num_suiv = suivantCounter.ToString();
            String concatfichname;
            int reponse;
            if (reponse_de_user.Text.CompareTo("A") == 0)
            {
                reponse = 1;
            }
            else
            {
                if (reponse_de_user.Text.CompareTo("B") == 0)
                {
                    reponse = 2;
                }
                else
                {
                    if (reponse_de_user.Text.CompareTo("C") == 0)
                    {
                        reponse = 3;
                    }
                    else
                    {
                        if (reponse_de_user.Text.CompareTo("D") == 0)
                        {
                            reponse = 4;
                        }
                        else
                        {
                            reponse_de_user.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFromString("#EC3D3D");
                            return;
                        }
                    }
                }
            }
            if (PagesNiveaux.btn_axiale_is_clicked == true)
            {
                if (PagesNiveaux.btn_niveau1_is_clicked == true)
                {
                    concatfichname = "Quiz" +num_suiv+ "_niveau1_axiale.txt";
                    StreamWriter sw = new StreamWriter(concatfichname);
                    sw.WriteLine(reponse.ToString());
                    sw.WriteLine(Question.Text);
                    sw.WriteLine(ChoixA.Text);
                    sw.WriteLine(ChoixB.Text);
                    sw.WriteLine(ChoixC.Text);
                    sw.WriteLine(ChoixD.Text);
                    sw.Close();
                    MainWindow.MainFrame.NavigationService.Navigate(new MainQuizWindow("Quiz1_niveau1_axiale.txt", "Quiz2_niveau1_axiale.txt", "Quiz3_niveau1_axiale.txt"));
                }
                if (PagesNiveaux.btn_niveau2_is_clicked == true)
                {
                    concatfichname = "Quiz" + num_suiv + "_niveau2_axiale.txt";
                    StreamWriter sw = new StreamWriter(concatfichname);
                    sw.WriteLine(reponse.ToString());
                    sw.WriteLine(Question.Text);
                    sw.WriteLine(ChoixA.Text);
                    sw.WriteLine(ChoixB.Text);
                    sw.WriteLine(ChoixC.Text);
                    sw.WriteLine(ChoixD.Text);
                    sw.Close();
                    MainWindow.MainFrame.NavigationService.Navigate(new MainQuizWindow("Quiz1_niveau2_axiale.txt", "Quiz2_niveau2_axiale.txt", "Quiz3_niveau2_axiale.txt"));
                }
                if (PagesNiveaux.btn_niveau3_is_clicked == true)
                {
                    concatfichname = "Quiz" + num_suiv + "_niveau3_axiale.txt";
                    StreamWriter sw = new StreamWriter(concatfichname);
                    sw.WriteLine(reponse.ToString());
                    sw.WriteLine(Question.Text);
                    sw.WriteLine(ChoixA.Text);
                    sw.WriteLine(ChoixB.Text);
                    sw.WriteLine(ChoixC.Text);
                    sw.WriteLine(ChoixD.Text);
                    sw.Close();
                    MainWindow.MainFrame.NavigationService.Navigate(new MainQuizWindow("Quiz1_niveau3_axiale.txt", "Quiz2_niveau3_axiale.txt", "Quiz3_niveau3_axiale.txt"));
                }
            }
            if (PagesNiveaux.btn_centrale_is_clicked == true)
            {
                if (PagesNiveaux.btn_niveau1_is_clicked == true)
                {
                    concatfichname = "Quiz" + num_suiv + "_niveau1_centrale.txt";
                    StreamWriter sw = new StreamWriter(concatfichname);
                    sw.WriteLine(reponse.ToString());
                    sw.WriteLine(Question.Text);
                    sw.WriteLine(ChoixA.Text);
                    sw.WriteLine(ChoixB.Text);
                    sw.WriteLine(ChoixC.Text);
                    sw.WriteLine(ChoixD.Text);
                    sw.Close();
                    MainWindow.MainFrame.NavigationService.Navigate(new MainQuizWindow("Quiz1_niveau1_centrale.txt", "Quiz2_niveau1_centrale.txt", "Quiz3_niveau1_centrale.txt"));
                }
                if (PagesNiveaux.btn_niveau2_is_clicked == true)
                {
                    concatfichname = "Quiz" + num_suiv + "_niveau2_centrale.txt";
                    StreamWriter sw = new StreamWriter(concatfichname);
                    sw.WriteLine(reponse.ToString());
                    sw.WriteLine(Question.Text);
                    sw.WriteLine(ChoixA.Text);
                    sw.WriteLine(ChoixB.Text);
                    sw.WriteLine(ChoixC.Text);
                    sw.WriteLine(ChoixD.Text);
                    sw.Close();
                    MainWindow.MainFrame.NavigationService.Navigate(new MainQuizWindow("Quiz1_niveau2_centrale.txt", "Quiz2_niveau2_centrale.txt", "Quiz3_niveau2_centrale.txt"));
                }
                if (PagesNiveaux.btn_niveau3_is_clicked == true)
                {
                    concatfichname = "Quiz" + num_suiv + "_niveau3_centrale.txt";
                    StreamWriter sw = new StreamWriter(concatfichname);
                    sw.WriteLine(reponse.ToString());
                    sw.WriteLine(Question.Text);
                    sw.WriteLine(ChoixA.Text);
                    sw.WriteLine(ChoixB.Text);
                    sw.WriteLine(ChoixC.Text);
                    sw.WriteLine(ChoixD.Text);
                    sw.Close();
                    MainWindow.MainFrame.NavigationService.Navigate(new MainQuizWindow("Quiz1_niveau3_centrale.txt", "Quiz2_niveau3_centrale.txt", "Quiz3_niveau3_centrale.txt"));
                }
            }
        }
    }
}
