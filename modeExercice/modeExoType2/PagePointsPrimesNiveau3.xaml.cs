using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
namespace MAINPAGE
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class PagePointsPrimesNiveau3 : Page
    {
        public int niveau;
        public string path;
        public BitmapImage[] image;
        public string[,] reponse;
        public string[,] point;
        public int cpt = 0;
        public PagePointsPrimesNiveau3(string path)
        {
            InitializeComponent();
            if (!MainWindow.modeEns) modify.Visibility = Visibility.Collapsed;

            this.path = path;
            precedent.Visibility = Visibility.Collapsed;
            image = new BitmapImage[3];
            reponse = new string[3, 7];
            point = new string[3, 7];
            string fileline;
            StreamReader sr = new StreamReader(path + "/images.txt");
            for (int i = 0; i < 3; i++)
            {
                if ((fileline = sr.ReadLine()) != null)
                {
                    image[i] = new BitmapImage(new Uri(fileline, UriKind.RelativeOrAbsolute));
                }
            }
            sr.Close();
            sr = new StreamReader(path + "/reponses.txt");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if ((fileline = sr.ReadLine()) != null)
                    {
                        reponse[i, j] = fileline;
                    }
                }
            }
            sr.Close();
            sr = new StreamReader(path + "/points.txt");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    if ((fileline = sr.ReadLine()) != null)
                    {
                        point[i, j] = fileline;
                    }
                }
            }
            sr.Close();
            imageEtud.Source = image[cpt];
            imageEns.Source = image[cpt];
            p1etud.Text = point[0, 0];
            p2etud.Text = point[0, 1];
            p3etud.Text = point[0, 2];
            p4etud.Text = point[0, 3];
            p5etud.Text = point[0, 4];
            p6etud.Text = point[0, 5];
            p7etud.Text = point[0, 6];
        }

        private void Button_Click_Modify(object sender, RoutedEventArgs e)
        {
            GRID_ENS.Visibility = Visibility.Visible;
            GRID_ETUD.Visibility = Visibility.Collapsed;
            pr1ens.Text = reponse[cpt, 0];
            pr2ens.Text = reponse[cpt, 1];
            pr3ens.Text = reponse[cpt, 2];
            pr4ens.Text = reponse[cpt, 3];
            pr5ens.Text = reponse[cpt, 4];
            pr6ens.Text = reponse[cpt, 5];
            pr7ens.Text = reponse[cpt, 6];
            p1ens.Text = point[cpt, 0];
            p2ens.Text = point[cpt, 1];
            p3ens.Text = point[cpt, 2];
            p4ens.Text = point[cpt, 3];
            p5ens.Text = point[cpt, 4];
            p6ens.Text = point[cpt, 5];
            p7ens.Text = point[cpt, 6];

        }

        private void Suivant_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#A2DBA1");
            bigCenterRectangle.Stroke = color;
            smallBelowRectangle.Stroke = color;
            verify.Background = color;
            verify.Content = "Vérifier";
            l1.Stroke = color;
            l2.Stroke = color;
            l3.Stroke = color;
            l4.Stroke = color;
            l5.Stroke = color;
            l6.Stroke = color;
            l7.Stroke = color;
            pr1etud.Text = "";
            pr2etud.Text = "";
            pr3etud.Text = "";
            pr4etud.Text = "";
            pr5etud.Text = "";
            pr6etud.Text = "";
            pr7etud.Text = "";
            cpt++;
            precedent.Visibility = Visibility.Visible;
            if (cpt >= 2)
            {
                cpt = 2;
                suivant.Visibility = Visibility.Collapsed;
            }
            imageEns.Source = image[cpt];
            imageEtud.Source = image[cpt];
            p1etud.Text = point[cpt, 0];
            p2etud.Text = point[cpt, 1];
            p3etud.Text = point[cpt, 2];
            p4etud.Text = point[cpt, 3];
            p5etud.Text = point[cpt, 4];
            p6etud.Text = point[cpt, 5];
            p7etud.Text = point[cpt, 6];
        }

        private void Precedent_Click(object sender, RoutedEventArgs e)
        {
            SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#A2DBA1");
            bigCenterRectangle.Stroke = color;
            smallBelowRectangle.Stroke = color;
            verify.Background = color;
            verify.Content = "Vérifier";
            l1.Stroke = color;
            l2.Stroke = color;
            l3.Stroke = color;
            l4.Stroke = color;
            l5.Stroke = color;
            l6.Stroke = color;
            l7.Stroke = color;
            pr1etud.Text = "";
            pr2etud.Text = "";
            pr3etud.Text = "";
            pr4etud.Text = "";
            pr5etud.Text = "";
            pr6etud.Text = "";
            pr7etud.Text = "";
            cpt--;
            suivant.Visibility = Visibility.Visible;
            if (cpt <= 0)
            {
                cpt = 0;
                precedent.Visibility = Visibility.Collapsed;
            }
            imageEns.Source = image[cpt];
            imageEtud.Source = image[cpt];
            p1etud.Text = point[cpt, 0];
            p2etud.Text = point[cpt, 1];
            p3etud.Text = point[cpt, 2];
            p4etud.Text = point[cpt, 3];
            p5etud.Text = point[cpt, 4];
            p6etud.Text = point[cpt, 5];
            p7etud.Text = point[cpt, 6];
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            if ((pr1etud.Text.Length != 0) && (pr2etud.Text.Length != 0) && (pr3etud.Text.Length != 0) && (pr4etud.Text.Length != 0) && (pr5etud.Text.Length != 0) && (pr6etud.Text.Length != 0) && (pr7etud.Text.Length != 0))
            {
                string real1, real2, real3, real4, real5, real6, real7;
                real1 = reponse[cpt, 0];
                real2 = reponse[cpt, 1];
                real3 = reponse[cpt, 2];
                real4 = reponse[cpt, 3];
                real5 = reponse[cpt, 4];
                real6 = reponse[cpt, 5];
                real7 = reponse[cpt, 6];
                string maybe1, maybe2, maybe3, maybe4, maybe5, maybe6, maybe7;
                maybe1 = pr1etud.Text;
                maybe2 = pr2etud.Text;
                maybe3 = pr3etud.Text;
                maybe4 = pr4etud.Text;
                maybe5 = pr5etud.Text;
                maybe6 = pr6etud.Text;
                maybe7 = pr7etud.Text;
                if ((maybe1.CompareTo(real1) == 0) && (maybe2.CompareTo(real2) == 0) && (maybe3.CompareTo(real3) == 0) && (maybe4.CompareTo(real4) == 0) && (maybe5.CompareTo(real5) == 0) && (maybe6.CompareTo(real6) == 0) && (maybe7.CompareTo(real7) == 0))
                {
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#1CB81C");
                    bigCenterRectangle.Stroke = color;
                    smallBelowRectangle.Stroke = color;
                    verify.Background = color;
                    l1.Stroke = color;
                    l2.Stroke = color;
                    l3.Stroke = color;
                    l4.Stroke = color;
                    l5.Stroke = color;
                    l6.Stroke = color;
                    l7.Stroke = color;
                    verify.Content = "Bravo !";
                }
                else
                {
                    SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EC3D3D"));
                    bigCenterRectangle.Stroke = color;
                    smallBelowRectangle.Stroke = color;
                    verify.Background = color;
                    l1.Stroke = color;
                    l2.Stroke = color;
                    l3.Stroke = color;
                    l4.Stroke = color;
                    l5.Stroke = color;
                    l6.Stroke = color;
                    l7.Stroke = color;
                    verify.Content = "Oops !";
                }
            }

        }

        private void UploadButtonEns_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == true)
            {
                imageEns.Source = new BitmapImage(new Uri(op.FileName));
                imageEtud.Source = new BitmapImage(new Uri(op.FileName));
            }
        }
        private void PlaySoundYay()
        {
            var uri = new Uri(@"Icons/yay.mp3", UriKind.Relative);
            var player = new MediaPlayer();

            player.Open(uri);
            player.Play();
        }

        
        private void OKEns_Click_OK(object sender, RoutedEventArgs e)
        {
            if ((pr1ens.Text.Length != 0) && (pr2ens.Text.Length != 0) && (pr3ens.Text.Length != 0) && (pr4ens.Text.Length != 0) && (pr5ens.Text.Length != 0) && (p1ens.Text.Length != 0) && (p2ens.Text.Length != 0) && (p3ens.Text.Length != 0) && (p4ens.Text.Length != 0) && (p5ens.Text.Length != 0))
            {
                reponse[cpt, 0] = pr1ens.Text;
                reponse[cpt, 1] = pr2ens.Text;
                reponse[cpt, 2] = pr3ens.Text;
                reponse[cpt, 3] = pr4ens.Text;
                reponse[cpt, 4] = pr5ens.Text;
                reponse[cpt, 5] = pr6ens.Text;
                reponse[cpt, 6] = pr7ens.Text;
                point[cpt, 0] = p1ens.Text;
                point[cpt, 1] = p2ens.Text;
                point[cpt, 2] = p3ens.Text;
                point[cpt, 3] = p4ens.Text;
                point[cpt, 4] = p5ens.Text;
                point[cpt, 5] = p6ens.Text;
                point[cpt, 6] = p7ens.Text;
                p1etud.Text = point[cpt, 0];
                p2etud.Text = point[cpt, 1];
                p3etud.Text = point[cpt, 2];
                p4etud.Text = point[cpt, 3];
                p5etud.Text = point[cpt, 4];
                p6etud.Text = point[cpt, 5];
                p7etud.Text = point[cpt, 6];
                image[cpt] = (BitmapImage)imageEns.Source;
                SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#A2DBA1");
                bigCenterRectangle.Stroke = color;
                smallBelowRectangle.Stroke = color;
                verify.Background = color;
                verify.Content = "Vérifier";
                l1.Stroke = color;
                l2.Stroke = color;
                l3.Stroke = color;
                l4.Stroke = color;
                l5.Stroke = color;
                l6.Stroke = color;
                l7.Stroke = color;
                pr1etud.Text = "";
                pr2etud.Text = "";
                pr3etud.Text = "";
                pr4etud.Text = "";
                pr5etud.Text = "";
                pr6etud.Text = "";
                pr7etud.Text = "";
                GRID_ENS.Visibility = Visibility.Collapsed;
                GRID_ETUD.Visibility = Visibility.Visible;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sw = new StreamWriter(path + "/images.txt");
            for (int i = 0; i < 3; i++)
            {
                sw.WriteLine(image[i].UriSource);
            }
            sw.Close();
            sw = new StreamWriter(path + "/reponses.txt");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    sw.WriteLine(reponse[i, j]);
                }
            }
            sw.Close();
            sw = new StreamWriter(path + "/points.txt");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    sw.WriteLine(point[i, j]);
                }
            }
            sw.Close();
        }


    }


}
