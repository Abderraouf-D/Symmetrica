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
    public partial class PagePointsPrimesNiveau1 : Page
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        public int niveau;
        public string path;
        public BitmapImage[] image;
        public string[,] reponse;
        public string[,] point;
        public int cpt = 0;
        public bool[] done;
        public PagePointsPrimesNiveau1(string path)
        {
            InitializeComponent();
            this.path = path;

            

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

            precedent.Visibility = Visibility.Collapsed;
            image = new BitmapImage[3];
            reponse = new string[3, 3];
            point = new string[3, 3];
            done = new bool[3];
            string fileline;
            StreamReader sr = new StreamReader(path + "/images.txt");
            for (int i = 0; i < 3; i++)
            {
                if ((fileline = sr.ReadLine()) != null)
                {
                    image[i] = convertImg(System.IO.Path.GetFullPath(fileline));
                }
            }
            sr.Close();
            sr = new StreamReader(path + "/reponses.txt");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
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
                for (int j = 0; j < 3; j++)
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


        }

        private void loaded(object sender, RoutedEventArgs e)
        {
            this.Resources.MergedDictionaries.Clear();

            this.Resources.MergedDictionaries.Add(MainWindow.ResLibre);
            if (!MainWindow.modeEns) Save.Visibility = modify.Visibility = Visibility.Collapsed;
            else Save.Visibility = modify.Visibility = Visibility.Visible;
            if (!MainWindow.francais)
            {
                ensAr.Visibility = Visibility.Visible;
                ensFr.Visibility = Visibility.Collapsed;
                p1ens.Margin = new Thickness(867, 456, 642, 403);
                p2ens.Margin = new Thickness(958, 456, 554, 403);
                p3ens.Margin = new Thickness(1044, 456, 461, 403);
                pr1ens.Margin = new Thickness(867, 541, 642, 331);
                pr2ens.Margin = new Thickness(958, 541, 554, 331);
                pr3ens.Margin = new Thickness(1049, 541, 466, 331);
            }
            else
            {
                ensAr.Visibility = Visibility.Collapsed;
                ensFr.Visibility = Visibility.Visible;
                p1ens.Margin = new Thickness(1123, 456, 386, 403);
                p2ens.Margin = new Thickness(1215, 456, 297, 403);
                p3ens.Margin = new Thickness(1301, 456, 204, 403);
                pr1ens.Margin = new Thickness(1123, 541, 386, 331);
                pr2ens.Margin = new Thickness(1215, 541, 297, 331);
                pr3ens.Margin = new Thickness(1306, 541, 209, 331);
            }
        }


        




        private void Button_Click_Modify(object sender, RoutedEventArgs e)
        {
            GRID_ENS.Visibility = Visibility.Visible;
            GRID_ETUD.Visibility = Visibility.Collapsed;
            pr1ens.Text = reponse[cpt, 0];
            pr2ens.Text = reponse[cpt, 1];
            pr3ens.Text = reponse[cpt, 2];
            
            p1ens.Text = point[cpt, 0];
            p2ens.Text = point[cpt, 1];
            p3ens.Text = point[cpt, 2];
            

        }

        private void Suivant_Click(object sender, RoutedEventArgs e)
        {
            cpt++;
            precedent.Visibility = Visibility.Visible;
            if (cpt >= 2)
            {
                cpt = 2;
                suivant.Visibility = Visibility.Collapsed;
            }
            imageEns.Source = image[cpt];
            imageEtud.Source = image[cpt];
            if (done[cpt])
            {
                SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#1CB81C");
                bigCenterRectangle.Stroke = color;
                smallBelowRectangle.Stroke = color;
                verify.Background = color;
                verify.Content = MainWindow.ResLibre["bravoButton"];
                l1.Stroke = color;
                l2.Stroke = color;
                l3.Stroke = color;
                pr1etud.Text = reponse[cpt, 0];
                pr2etud.Text = reponse[cpt, 1];
                pr3etud.Text = reponse[cpt, 2];
                pr1etud.IsReadOnly = true;
                pr2etud.IsReadOnly = true;
                pr3etud.IsReadOnly = true;
            }
            else
            {
                SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#A2DBA1");
                bigCenterRectangle.Stroke = color;
                smallBelowRectangle.Stroke = color;
                verify.Background = color;
                verify.Content = MainWindow.ResLibre["verify"];
                l1.Stroke = color;
                l2.Stroke = color;
                l3.Stroke = color;
                pr1etud.Text = "";
                pr2etud.Text = "";
                pr3etud.Text = "";
                pr1etud.IsReadOnly = false;
                pr2etud.IsReadOnly = false;
                pr3etud.IsReadOnly = false;
            }                        
            p1etud.Text = point[cpt, 0];
            p2etud.Text = point[cpt, 1];
            p3etud.Text = point[cpt, 2];
        }

        private void Precedent_Click(object sender, RoutedEventArgs e)
        {
            cpt--;
            suivant.Visibility = Visibility.Visible;
            if (cpt <= 0)
            {
                cpt = 0;
                precedent.Visibility = Visibility.Collapsed;
            }
            imageEns.Source = image[cpt];
            imageEtud.Source = image[cpt];
            if (done[cpt])
            {
                SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#1CB81C");
                bigCenterRectangle.Stroke = color;
                smallBelowRectangle.Stroke = color;
                verify.Background = color;
                verify.Content = MainWindow.ResLibre["bravoButton"];
                l1.Stroke = color;
                l2.Stroke = color;
                l3.Stroke = color;
                pr1etud.Text = reponse[cpt, 0];
                pr2etud.Text = reponse[cpt, 1];
                pr3etud.Text = reponse[cpt, 2];
                pr1etud.IsReadOnly = true;
                pr2etud.IsReadOnly = true;
                pr3etud.IsReadOnly = true;
            }
            else
            {
                SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#A2DBA1");
                bigCenterRectangle.Stroke = color;
                smallBelowRectangle.Stroke = color;
                verify.Background = color;
                verify.Content = MainWindow.ResLibre["verify"];
                l1.Stroke = color;
                l2.Stroke = color;
                l3.Stroke = color;
                pr1etud.Text = "";
                pr2etud.Text = "";
                pr3etud.Text = "";
                pr1etud.IsReadOnly = false;
                pr2etud.IsReadOnly = false;
                pr3etud.IsReadOnly = false;
            }
            p1etud.Text = point[cpt, 0];
            p2etud.Text = point[cpt, 1];
            p3etud.Text = point[cpt, 2];
        }

       
        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            if ((pr1etud.Text.Length != 0) && (pr2etud.Text.Length != 0) && (pr3etud.Text.Length != 0))
            {
                string real1, real2, real3;
                real1 = reponse[cpt, 0];
                real2 = reponse[cpt, 1];
                real3 = reponse[cpt, 2];
                string maybe1, maybe2, maybe3;
                maybe1 = pr1etud.Text;
                maybe2 = pr2etud.Text;
                maybe3 = pr3etud.Text;
               
                if ((maybe1.CompareTo(real1) == 0) && (maybe2.CompareTo(real2) == 0) && (maybe3.CompareTo(real3) == 0))
                { 
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#1CB81C");
                    bigCenterRectangle.Stroke = color;
                    smallBelowRectangle.Stroke = color;
                    verify.Background = color;
                    l1.Stroke = color;
                    l2.Stroke = color;
                    l3.Stroke = color;
                    verify.Content = MainWindow.ResLibre["bravoButton"];
                    done[cpt] = true;
                    pr1etud.IsReadOnly = true;
                    pr2etud.IsReadOnly = true;
                    pr3etud.IsReadOnly = true;
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
                    verify.Content = MainWindow.ResLibre["oopsButton"];
                    dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
                    dispatcherTimer.Start();
                }
            }

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (!done[cpt])
            {
                SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#A2DBA1");
                bigCenterRectangle.Stroke = color;
                smallBelowRectangle.Stroke = color;
                verify.Background = color;
                verify.Content = MainWindow.ResLibre["verify"];
                l1.Stroke = color;
                l2.Stroke = color;
                l3.Stroke = color;                
            }
        }

        private BitmapImage convertImg(string path)
        {
            BitmapImage image = new BitmapImage();
            if (!string.IsNullOrEmpty(path))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.UriSource = new Uri(path);
                image.EndInit();

            }

            return image;
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

                try
                {
                    String filepath = op.FileName;
                    String name = System.IO.Path.GetFileName(filepath);
                    File.Copy(filepath, System.IO.Path.GetFullPath(image[cpt].UriSource.AbsolutePath), true);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                imageEns.Source = convertImg(op.FileName);
                imageEtud.Source = convertImg(op.FileName);
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
            if ((pr1ens.Text.Length != 0) && (pr2ens.Text.Length != 0) && (pr3ens.Text.Length != 0) && (p1ens.Text.Length != 0) && (p2ens.Text.Length != 0) && (p3ens.Text.Length != 0))
            {
                reponse[cpt, 0] = pr1ens.Text;
                reponse[cpt, 1] = pr2ens.Text;
                reponse[cpt, 2] = pr3ens.Text;
                
                point[cpt, 0] = p1ens.Text;
                point[cpt, 1] = p2ens.Text;
                point[cpt, 2] = p3ens.Text;
                
                p1etud.Text = point[cpt, 0];
                p2etud.Text = point[cpt, 1];
                p3etud.Text = point[cpt, 2];
                
                image[cpt] = (BitmapImage)imageEns.Source;
                if (done[cpt])
                {
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#1CB81C");
                    bigCenterRectangle.Stroke = color;
                    smallBelowRectangle.Stroke = color;
                    verify.Background = color;
                    verify.Content = MainWindow.ResLibre["bravoButton"];
                    l1.Stroke = color;
                    l2.Stroke = color;
                    l3.Stroke = color;
                    pr1etud.Text = reponse[cpt, 0];
                    pr2etud.Text = reponse[cpt, 1];
                    pr3etud.Text = reponse[cpt, 2];
                    pr1etud.IsReadOnly = true;
                    pr2etud.IsReadOnly = true;
                    pr3etud.IsReadOnly = true;
                }
                else
                {
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#A2DBA1");
                    bigCenterRectangle.Stroke = color;
                    smallBelowRectangle.Stroke = color;
                    verify.Background = color;
                    verify.Content = MainWindow.ResLibre["verify"];
                    l1.Stroke = color;
                    l2.Stroke = color;
                    l3.Stroke = color;
                    pr1etud.IsReadOnly = false;
                    pr2etud.IsReadOnly = false;
                    pr3etud.IsReadOnly = false;
                    pr1etud.Text = "";
                    pr2etud.Text = "";
                    pr3etud.Text = "";
                }
                
                
                GRID_ENS.Visibility = Visibility.Collapsed;
                GRID_ETUD.Visibility = Visibility.Visible;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sw = new StreamWriter(path + "/reponses.txt");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sw.WriteLine(reponse[i, j]);
                }
            }
            sw.Close();
            sw = new StreamWriter(path + "/points.txt");
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    sw.WriteLine(point[i, j]);
                }
            }
            sw.Close();
        }


    }


}
