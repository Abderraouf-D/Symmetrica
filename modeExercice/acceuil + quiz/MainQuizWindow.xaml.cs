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
    /// <summary>
    /// Interaction logic for MainQuizWindow.xaml
    /// </summary>
    public partial class MainQuizWindow : Page
    {
        public bool isClickedChoixA = false;
        public bool isClickedChoixB = false;
        public bool isClickedChoixC = false;
        public bool isClickedChoixD = false;
        public bool verifierIsclicked = false;
        public int suivantCounter = 1;
        public String fich1;
        public String fich2;
        public String fich3;
        public String a;

        public MainQuizWindow(String fich1 , String fich2 , String fich3)
        {
            InitializeComponent();
            this.fich1 = fich1;
            this.fich2 = fich2;
            this.fich3 = fich3;
            affichQuiz(fich1);
            if (!MainWindow.modeEns) edtText.Visibility=teacherButton.Visibility=Visibility.Collapsed;
            this.Resources.MergedDictionaries.Add(MainWindow.ResLibre);
        }

        public void affichQuiz(string path)
        {
            String[] questions = File.ReadAllLines(path);

            if (MainWindow.francais)
            {
                a = questions[0];
                Question.Text = questions[1];
                ChoixA.Content = questions[2];
                ChoixB.Content = questions[3];
                ChoixC.Content = questions[4];
                ChoixD.Content = questions[5];
            }
            else
            {
                a = questions[6];
                Question.Text = questions[7];
                ChoixA.Content = questions[8];
                ChoixB.Content = questions[9];
                ChoixC.Content = questions[10];
                ChoixD.Content = questions[11];
            }
        }


        private void ChoixD_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isClickedChoixD)
            {
                isClickedChoixA = false;
                isClickedChoixB = false;
                isClickedChoixC = false;
                return;
            }
            object resource = Application.Current.FindResource("ButtonQuizStyleMouseEnter");
            ChoixD.Style = (Style)resource;    
        }

        private void ChoixD_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isClickedChoixD)
            { 
                isClickedChoixA = false;
                isClickedChoixB = false;
                isClickedChoixC = false;
                return; 
            }
                

            object resource = Application.Current.FindResource("ButtonQuizStyle");
            ChoixD.Style = (Style)resource;
        }

        private void ChoixB_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isClickedChoixB)
            {
                isClickedChoixA = false;
                isClickedChoixD = false;
                isClickedChoixC = false;
                return;
            }
            object resource = Application.Current.FindResource("ButtonQuizStyleMouseEnter");
            ChoixB.Style = (Style)resource;
        }

        private void ChoixB_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isClickedChoixB)
            {
                isClickedChoixA = false;
                isClickedChoixD = false;
                isClickedChoixC = false;
                return;
            }
            object resource = Application.Current.FindResource("ButtonQuizStyle");
            ChoixB.Style = (Style)resource;
        }

        private void ChoixC_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isClickedChoixC)
            {
                isClickedChoixA = false;
                isClickedChoixD = false;
                isClickedChoixB = false;
                return;
            }
            ChoixC.Style = (Style)Application.Current.FindResource("ButtonQuizStyleMouseEnter");
        }

        private void ChoixC_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isClickedChoixC)
            {
                isClickedChoixA = false;
                isClickedChoixD = false;
                isClickedChoixB = false;
                return;
            }
            ChoixC.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
        }

        private void ChoixA_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isClickedChoixA)
            {
                isClickedChoixC = false;
                isClickedChoixD = false;
                isClickedChoixB = false;
                return;
            }
            ChoixA.Style = (Style)Application.Current.FindResource("ButtonQuizStyleMouseEnter");
        }

        private void ChoixA_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isClickedChoixA)
            { 
                isClickedChoixC = false;
                isClickedChoixD = false;
                isClickedChoixB = false;
                return; 
            }

            ChoixA.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
        }

        private void ButtonSuivant_Click(object sender, RoutedEventArgs e)
        {
            bravoImage.Visibility = Visibility.Hidden;
            ressayerImage.Visibility = Visibility.Hidden;
            trueText.Visibility = Visibility.Hidden;    
            falseText.Visibility = Visibility.Hidden;
            isClickedChoixA = false;
            isClickedChoixD = false;
            isClickedChoixB = false;
            isClickedChoixC = false;
            ChoixA.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixB.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixC.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixD.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            if (suivantCounter < 3)
            {
                suivantCounter++;
            }
            if (suivantCounter <= 3 && suivantCounter == 2)
            {
                affichQuiz(fich2);
                btnSuivant.Visibility = Visibility.Visible;
                btnPrecedent.Visibility = Visibility.Visible;

            }
            if (suivantCounter <= 3 && suivantCounter == 3)
            {
                affichQuiz(fich3);
                btnSuivant.Visibility = Visibility.Hidden;
                btnPrecedent.Visibility = Visibility.Visible;
            }
        }

        private void ButtonPrecedent_Click(object sender, RoutedEventArgs e)
        {
            bravoImage.Visibility = Visibility.Hidden;
            ressayerImage.Visibility = Visibility.Hidden;
            trueText.Visibility = Visibility.Hidden;
            falseText.Visibility = Visibility.Hidden;
            isClickedChoixA = false;
            isClickedChoixD = false;
            isClickedChoixB = false;
            isClickedChoixC = false;
            ChoixA.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixB.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixC.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixD.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");


            if (suivantCounter > 1)
            {
                suivantCounter--;
            }
            if (suivantCounter >= 1 && suivantCounter == 1)
            {


                affichQuiz(fich1);
                btnPrecedent.Visibility = Visibility.Hidden;
                btnSuivant.Visibility = Visibility.Visible;
            }
            if (suivantCounter >= 1 && suivantCounter == 2)
            {
                affichQuiz(fich2);
                btnPrecedent.Visibility = Visibility.Visible;
                btnSuivant.Visibility = Visibility.Visible;
            }
        }

        private void ChoixA_Click(object sender, RoutedEventArgs e)
        {
            
            isClickedChoixA = true;
            ChoixA.Style = (Style)Application.Current.FindResource("ButtonQuizStyleMouseEnter");
            ChoixB.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixC.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixD.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");

            if(a.CompareTo("1")==0 )
            {
                ChoixA.Style = (Style)Application.Current.FindResource("ButtonQuizStyleTrue");
                bravoImage.Visibility = Visibility.Visible;
                trueText.Visibility = Visibility.Visible;
                ressayerImage.Visibility = Visibility.Hidden;
                falseText.Visibility = Visibility.Hidden;
                btnSuivant.Visibility= Visibility.Visible;
            }
            else
            {
                ChoixA.Style = (Style)Application.Current.FindResource("ButtonQuizStyleFalse");
                bravoImage.Visibility = Visibility.Hidden;
                trueText.Visibility = Visibility.Hidden;
                ressayerImage.Visibility = Visibility.Visible;
                falseText.Visibility = Visibility.Visible;
                btnSuivant.Visibility = Visibility.Hidden;
            }
            
        }

        private void ChoixB_Click(object sender, RoutedEventArgs e)
        {
            isClickedChoixB = true;
            ChoixB.Style = (Style)Application.Current.FindResource("ButtonQuizStyleMouseEnter");
            ChoixA.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixC.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixD.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            if (a.CompareTo("2") == 0)
            {
                ChoixB.Style = (Style)Application.Current.FindResource("ButtonQuizStyleTrue");
                bravoImage.Visibility = Visibility.Visible;
                trueText.Visibility = Visibility.Visible;
                ressayerImage.Visibility = Visibility.Hidden;
                falseText.Visibility = Visibility.Hidden;
                btnSuivant.Visibility = Visibility.Visible;
            }
            else
            {
                ChoixB.Style = (Style)Application.Current.FindResource("ButtonQuizStyleFalse");
                bravoImage.Visibility = Visibility.Hidden;
                trueText.Visibility = Visibility.Hidden;
                ressayerImage.Visibility = Visibility.Visible;
                falseText.Visibility = Visibility.Visible;
                btnSuivant.Visibility = Visibility.Hidden;

            }
        }

        private void ChoixC_Click(object sender, RoutedEventArgs e)
        {
            isClickedChoixC = true;
            ChoixC.Style = (Style)Application.Current.FindResource("ButtonQuizStyleMouseEnter");
            ChoixA.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixB.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixD.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            if (a.CompareTo("3") == 0)
            {
                ChoixC.Style = (Style)Application.Current.FindResource("ButtonQuizStyleTrue");
                bravoImage.Visibility = Visibility.Visible;
                trueText.Visibility = Visibility.Visible;
                ressayerImage.Visibility = Visibility.Hidden;
                falseText.Visibility = Visibility.Hidden;
                btnSuivant.Visibility = Visibility.Visible;
            }
            else
            {
                ChoixC.Style = (Style)Application.Current.FindResource("ButtonQuizStyleFalse");
                bravoImage.Visibility = Visibility.Hidden;
                trueText.Visibility = Visibility.Hidden;
                ressayerImage.Visibility = Visibility.Visible;
                falseText.Visibility = Visibility.Visible;
                btnSuivant.Visibility = Visibility.Hidden;

            }
        }

        private void ChoixD_Click(object sender, RoutedEventArgs e)
        {
            isClickedChoixD = true;
            ChoixD.Style = (Style)Application.Current.FindResource("ButtonQuizStyleMouseEnter");
            ChoixA.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixB.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            ChoixC.Style = (Style)Application.Current.FindResource("ButtonQuizStyle");
            if (a.CompareTo("4") == 0)
            {
                ChoixD.Style = (Style)Application.Current.FindResource("ButtonQuizStyleTrue");
                bravoImage.Visibility = Visibility.Visible;
                trueText.Visibility = Visibility.Visible;
                ressayerImage.Visibility = Visibility.Hidden;
                falseText.Visibility = Visibility.Hidden;
                btnSuivant.Visibility = Visibility.Visible;
            }
            else
            {
                ChoixD.Style = (Style)Application.Current.FindResource("ButtonQuizStyleFalse");
                bravoImage.Visibility = Visibility.Hidden;
                trueText.Visibility = Visibility.Hidden;
                ressayerImage.Visibility = Visibility.Visible;  
                falseText.Visibility = Visibility.Visible;
                btnSuivant.Visibility = Visibility.Hidden;
            }
        }


        private void teacherButton_Click(object sender, RoutedEventArgs e)
        {
            MainQuizWindowTeacher pageQuiz = new MainQuizWindowTeacher(suivantCounter,fich1,fich2, fich3);
            MainWindow.MainFrame.NavigationService.Navigate(pageQuiz);
        }
    }
}
