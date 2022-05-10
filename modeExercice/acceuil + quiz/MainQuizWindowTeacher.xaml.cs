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



            



            
            StreamReader sr = new StreamReader(path);
            a = sr.ReadLine();
            Question.Text = sr.ReadLine();
            ChoixA.Text = sr.ReadLine();
            ChoixB.Text = sr.ReadLine();
            ChoixC.Text = sr.ReadLine();
            ChoixD.Text = sr.ReadLine();
            sr.Close();

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
                sr.Close();
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
                sr.Close();
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
               
                StreamReader sr = new StreamReader(fich1);
                a = sr.ReadLine();
                Question.Text = sr.ReadLine();
                ChoixA.Text = sr.ReadLine();
                ChoixB.Text = sr.ReadLine();
                ChoixC.Text = sr.ReadLine();
                ChoixD.Text = sr.ReadLine();
                sr.Close();
                btnPrecedent.Visibility = Visibility.Hidden;
                btnSuivant.Visibility = Visibility.Visible;
            }
            if (suivantCounter >= 1 && suivantCounter == 2)
            {
                
                StreamReader sr = new StreamReader(fich2);
                a = sr.ReadLine();
                Question.Text = sr.ReadLine();
                ChoixA.Text = sr.ReadLine();
                ChoixB.Text = sr.ReadLine();
                ChoixC.Text = sr.ReadLine();
                ChoixD.Text = sr.ReadLine();
                sr.Close();
                btnPrecedent.Visibility = Visibility.Visible;
                btnSuivant.Visibility = Visibility.Visible;
            }
        }



        private void btn_enregistrer_Click(object sender, RoutedEventArgs e)
        {
            String num_suiv = suivantCounter.ToString();
           
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


            }else
            {
                questions[6] = reponse.ToString();
                questions[7] = Question.Text;
                questions[8] = ChoixA.Text;
                questions[9] = ChoixB.Text;
                questions[10] = ChoixC.Text;
                questions[11] = ChoixD.Text;
            }
            File.WriteAllLines(path,questions);
            

            /*
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(reponse.ToString());
            sw.WriteLine(Question.Text);
            sw.WriteLine(ChoixA.Text);
            sw.WriteLine(ChoixB.Text);
            sw.WriteLine(ChoixC.Text);
            sw.WriteLine(ChoixD.Text);
            sw.Close();*/





            MainWindow.MainFrame.NavigationService.Navigate(new MainQuizWindow(fich1, fich2,fich3));


        }
    }
}
