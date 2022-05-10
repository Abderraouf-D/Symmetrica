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
        public int suivantCounter ;
        public String fich1;
        public String fich2;
        public String fich3;
        public String a;

        public MainQuizWindowTeacher(int i,String fich1, String fich2, String fich3)
        {
            InitializeComponent();
            string path=null;
            switch (i)
            {
                case 1: path = fich1;
                    break;
                case 2:
                    path = fich2;
                    break;
                case 3:
                    path = fich3;
                    break;
            }
            suivantCounter = i;
            this.fich1 = fich1;
            this.fich2 = fich2;
            this.fich3 = fich3;
            affichQuiz(path);
            this.Resources.MergedDictionaries.Add(MainWindow.ResLibre);


            if (MainWindow.francais) stckA.FlowDirection = stckB.FlowDirection = stckC.FlowDirection = stckD.FlowDirection = FlowDirection.LeftToRight;
            else stckA.FlowDirection = stckB.FlowDirection = stckC.FlowDirection = stckD.FlowDirection = FlowDirection.RightToLeft;
        }
        public void affichQuiz(string path)
        {
            String[] questions = File.ReadAllLines(path);

            if (MainWindow.francais)
            {
                a = questions[0];
                Question.Text = questions[1];
                ChoixA.Text = questions[2];
                ChoixB.Text= questions[3];
                ChoixC.Text = questions[4];
                ChoixD.Text = questions[5];
            }
            else
            {
                a = questions[6];
                Question.Text = questions[7];
                ChoixA.Text = questions[8];
                ChoixB.Text = questions[9];
                ChoixC.Text = questions[10];
                ChoixD.Text = questions[11];
            }
        }
        private void ButtonSuivant_Click(object sender, RoutedEventArgs e)
        {

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
            if (suivantCounter > 1)
            {
                suivantCounter--;
            }
            if (suivantCounter >=1 && suivantCounter == 1)
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



        private void btn_enregistrer_Click(object sender, RoutedEventArgs e)
        {
            String num_suiv = suivantCounter.ToString();
           
            int reponse = 1;
            var checkedValue = choices.Children.OfType<RadioButton>()
                 .FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value);
            if (choA.IsChecked==true) reponse = 1; 
            if (choB.IsChecked==true) reponse = 2; 
            if (choC.IsChecked==true) reponse = 3; 
            if (choD.IsChecked==true) reponse = 4; 
            

            string path=null;
            switch (suivantCounter)
            {
                case 1: path = fich1;
                     break;
                 case 2: path = fich2;
                     break;
                  case 3: path = fich3;
                     break;
            }

            String[] questions = new String[12];

            File.ReadAllLines(path).CopyTo(questions, 0);

            if (MainWindow.francais)
            {
                questions[0] = reponse.ToString();
                questions[1] = Question.Text;
                questions[2] = ChoixA.Text;
                questions[3] = ChoixB.Text;
                questions[4] = ChoixC.Text;
                questions[5] = ChoixD.Text;
            }
            else
            {
                questions[6] = reponse.ToString();
                questions[7] = Question.Text;
                questions[8] = ChoixA.Text;
                questions[9] = ChoixB.Text;
                questions[10] = ChoixC.Text;
                questions[11] = ChoixD.Text;
            }
            File.WriteAllLines(path, questions);
            MainWindow.MainFrame.NavigationService.Navigate(new MainQuizWindow(fich1, fich2,fich3));
        }
    }
}
