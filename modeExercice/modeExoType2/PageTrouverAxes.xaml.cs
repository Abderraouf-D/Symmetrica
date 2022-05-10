using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class PageTrouverAxes : Page
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        public double crnt_angle = 0;//*
        public double crnt_size = 150;
        public double crnt_vertical = 100;
        public double crnt_horizontal = 300;//*
        private bool firsttime = true;//*
        public int tempt = 0;
        public bool rtt, vrt, hrz, sz;
        readonly string path;
        public List<Line>[] line;
        public BitmapImage[] image;
        public int[] reponse;
        public int cpt = 0;
        public int[] linecpt;
        public bool[] done;

        public PageTrouverAxes(string path)
        {
            rtt = true;
            this.path = path;
            InitializeComponent();

            this.Resources.MergedDictionaries.Clear();

            this.Resources.MergedDictionaries.Add(MainWindow.ResLibre);
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            precedent.Visibility = Visibility.Collapsed;
            if (!MainWindow.modeEns) Save.Visibility = modify.Visibility = Visibility.Collapsed;

            StreamReader srp = new StreamReader(path + "/images.txt");
            StreamReader sr = new StreamReader(path + "/reponses.txt");
            StreamReader srl;
            string fileline;
            done = new bool[3];
            image = new BitmapImage[3];
            reponse = new int[3];
            line = new List<Line>[3];
            linecpt = new int[3];
            for (int i = 0; i < 3; i++)
            {
                linecpt[i] = -1;
                if ((fileline = sr.ReadLine()) != null)
                {
                    reponse[i] = int.Parse(fileline);
                }
                if ((fileline = srp.ReadLine()) != null)
                {
                    image[i] = new BitmapImage(new Uri(fileline, UriKind.RelativeOrAbsolute));
                }
                line[i] = new List<Line>();
                done[i] = false;
                srl = new StreamReader(path + "/line" + (i + 1).ToString() + ".txt");
                if ((fileline = srl.ReadLine()) != null)
                {
                    linecpt[i] = int.Parse(fileline);
                }
                int j = 0;
                while (j <= linecpt[i])
                {
                    Line a = new Line();
                    a.X1 = double.Parse(srl.ReadLine());
                    a.Y1 = double.Parse(srl.ReadLine());
                    a.X2 = double.Parse(srl.ReadLine());
                    a.Y2 = double.Parse(srl.ReadLine());
                    a.Stroke = Brushes.Black;
                    a.StrokeThickness = 5;
                    a.Opacity = 0.8;
                    line[i].Add(a);
                    if (i == 0)
                    {
                        canvasEtud.Children.Add(a);
                    }
                    j++;
                }
                srl.Close();
            }
            imageEtud.Source = image[cpt];
            imageEns.Source = image[cpt];
            sr.Close();
            srp.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (reponseBox.Text.Length == 0)
            {
                page2Hint.Visibility = Visibility.Visible;
            }
            else
            {
                page2Hint.Visibility = Visibility.Hidden;
            }
        }



        private void Button_Click_Verify(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(reponseBox.Text, out _))
            {
                int real = reponse[cpt];
                int maybe = int.Parse(reponseBox.Text);
                if (maybe == real)
                {
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#1CB81C");
                    bigCenterRectangle.Stroke = color;
                    smallBelowRectangle.Stroke = color;
                    verifyButton.Background = color;
                    verifyButton.Content = MainWindow.ResLibre["bravoButton"];
                    done[cpt] = true;
                    reponseBox.IsReadOnly = true;
                    for (int i = 0; i <= linecpt[cpt]; i++)
                    {
                        line[cpt][i].Stroke = color;
                    }
                    canvasEtud.Visibility = Visibility.Visible;
                }
                else
                {
                    if (tempt >= 2)
                    {
                        canvasEtud.Visibility = Visibility.Visible;
                    }
                    tempt++;
                    SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EC3D3D"));
                    bigCenterRectangle.Stroke = color;
                    smallBelowRectangle.Stroke = color;
                    verifyButton.Background = color;
                    verifyButton.Content = MainWindow.ResLibre["oopsButton"];
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
                verifyButton.Background = color;
                verifyButton.Content = MainWindow.ResLibre["verify"];
            }
        }

        private void PlaySoundYay()
        {
            var uri = new Uri(@"Icons/yay.mp3", UriKind.Relative);
            var player = new MediaPlayer();

            player.Open(uri);
            player.Play();
        }


        private void Button_Click_Modify(object sender, RoutedEventArgs e)
        {
            GRID_ENS.Visibility = Visibility.Visible;
            GRID_ETUD.Visibility = Visibility.Collapsed;
            textBoxEns.Text = reponse[cpt].ToString();
            for (int i = 0; i <= linecpt[cpt]; i++)
            {
                line[cpt][i].Stroke = Brushes.Black;
            }
            for (int i = 0; i <= linecpt[cpt]; i++)
            {
                canvasEtud.Children.Remove(line[cpt][i]);
                canvasEns.Children.Add(line[cpt][i]);
            }
        }

        private void OKEns_Click_OK(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(textBoxEns.Text, out _))
            {
                GRID_ENS.Visibility = Visibility.Collapsed;
                GRID_ETUD.Visibility = Visibility.Visible;
                image[cpt] = (BitmapImage)imageEns.Source;
                reponse[cpt] = int.Parse(textBoxEns.Text);
                for (int i = 0; i <= linecpt[cpt]; i++)
                {
                    canvasEns.Children.Remove(line[cpt][i]);
                    canvasEtud.Children.Add(line[cpt][i]);
                }
                if (done[cpt])
                {
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#1CB81C");
                    bigCenterRectangle.Stroke = color;
                    smallBelowRectangle.Stroke = color;
                    verifyButton.Background = color;
                    verifyButton.Content = MainWindow.ResLibre["bravoButton"];
                    reponseBox.IsReadOnly = true;
                    for (int i = 0; i <= linecpt[cpt]; i++)
                    {
                        line[cpt][i].Stroke = color;
                    }
                    canvasEtud.Visibility = Visibility.Visible;
                    reponseBox.Text = reponse[cpt].ToString();
                }
                else
                {
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#A2DBA1");
                    bigCenterRectangle.Stroke = color;
                    smallBelowRectangle.Stroke = color;
                    verifyButton.Background = color;
                    verifyButton.Content = MainWindow.ResLibre["verify"];
                    reponseBox.Text = "";
                    reponseBox.IsReadOnly = false;
                    canvasEtud.Visibility = Visibility.Collapsed;
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

        private void Suivant_Click(object sender, RoutedEventArgs e)
        {
            tempt = 0;
            cpt++;
            precedent.Visibility = Visibility.Visible;
            if (cpt >= 2)
            {
                cpt = 2;
                suivant.Visibility = Visibility.Collapsed;
            }
            imageEns.Source = image[cpt];
            imageEtud.Source = image[cpt];
            for (int i = 0; i <= linecpt[cpt - 1]; i++)
            {
                canvasEtud.Children.Remove(line[cpt - 1][i]);
            }
            for (int i = 0; i <= linecpt[cpt]; i++)
            {
                canvasEtud.Children.Add(line[cpt][i]);
            }
            if (done[cpt])
            {
                SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#1CB81C");
                bigCenterRectangle.Stroke = color;
                smallBelowRectangle.Stroke = color;
                verifyButton.Background = color;
                verifyButton.Content = MainWindow.ResLibre["bravoButton"];
                reponseBox.IsReadOnly = true;
                canvasEtud.Visibility = Visibility.Visible;
                reponseBox.Text = reponse[cpt].ToString();
            }
            else
            {
                SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#A2DBA1");
                bigCenterRectangle.Stroke = color;
                smallBelowRectangle.Stroke = color;
                verifyButton.Background = color;
                verifyButton.Content = MainWindow.ResLibre["verify"];
                reponseBox.Text = "";
                reponseBox.IsReadOnly = false;
                canvasEtud.Visibility = Visibility.Collapsed;
            }
        }

        private void Precedent_Click(object sender, RoutedEventArgs e)
        {
            tempt = 0;
            cpt--;
            suivant.Visibility = Visibility.Visible;
            if (cpt <= 0)
            {
                cpt = 0;
                precedent.Visibility = Visibility.Collapsed;
            }
            imageEns.Source = image[cpt];
            imageEtud.Source = image[cpt];
            for (int i = 0; i <= linecpt[cpt + 1]; i++)
            {
                canvasEtud.Children.Remove(line[cpt + 1][i]);
            }
            for (int i = 0; i <= linecpt[cpt]; i++)
            {
                canvasEtud.Children.Add(line[cpt][i]);
            }
            if (done[cpt])
            {
                SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#1CB81C");
                bigCenterRectangle.Stroke = color;
                smallBelowRectangle.Stroke = color;
                verifyButton.Background = color;
                verifyButton.Content = MainWindow.ResLibre["bravoButton"];
                reponseBox.IsReadOnly = true;
                canvasEtud.Visibility = Visibility.Visible;
                reponseBox.Text = reponse[cpt].ToString();
            }
            else
            {
                SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#A2DBA1");
                bigCenterRectangle.Stroke = color;
                smallBelowRectangle.Stroke = color;
                verifyButton.Background = color;
                verifyButton.Content = MainWindow.ResLibre["verify"];
                reponseBox.Text = "";
                reponseBox.IsReadOnly = false;
                canvasEtud.Visibility = Visibility.Collapsed;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)//**
        {
            StreamWriter sw = new StreamWriter(path + "/reponses.txt");
            for (int i = 0; i < 3; i++)
            {
                sw.WriteLine(reponse[i]);
            }
            sw.Close();
            sw = new StreamWriter(path + "/images.txt");
            for (int i = 0; i < 3; i++)
            {
                sw.WriteLine(image[i].UriSource);
            }
            sw.Close();
            for (int j = 0; j < 3; j++)
            {
                sw = new StreamWriter(path + "/line" + (j + 1).ToString() + ".txt");
                sw.WriteLine(linecpt[j]);
                for (int i = 0; i <= linecpt[j]; i++)
                {
                    sw.WriteLine(line[j][i].X1);
                    sw.WriteLine(line[j][i].Y1);
                    sw.WriteLine(line[j][i].X2);
                    sw.WriteLine(line[j][i].Y2);
                }
                sw.Close();
            }
        }

        private void sliderEns_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if (linecpt[cpt] >= 0)
            {
                if (rtt)
                {
                    if (firsttime)
                    {
                        firsttime = false;
                        return;
                    }

                    Point a = new Point(line[cpt][linecpt[cpt]].X1, line[cpt][linecpt[cpt]].Y1);
                    Point b = new Point(line[cpt][linecpt[cpt]].X2, line[cpt][linecpt[cpt]].Y2);
                  
                    RotateTransform rt = new RotateTransform()
                    {
                        CenterX = (line[cpt][linecpt[cpt]].X1 + line[cpt][linecpt[cpt]].X2) / 2,
                        CenterY = (line[cpt][linecpt[cpt]].Y1 + line[cpt][linecpt[cpt]].Y2) / 2, // OK
                        Angle = (e.OldValue - e.NewValue),
                    };

                    a = rt.Transform(a);
                    b = rt.Transform(b);

                    line[cpt][linecpt[cpt]].X1 = a.X;
                    line[cpt][linecpt[cpt]].Y1 = a.Y;
                    line[cpt][linecpt[cpt]].X2 = b.X;
                    line[cpt][linecpt[cpt]].Y2 = b.Y;
                    
                    crnt_angle = e.NewValue;
                    
                }
                if (vrt)
                {
                    if (firsttime)
                    {
                        firsttime = false;
                        return;
                    }
                    line[cpt][linecpt[cpt]].Y1 -= (e.OldValue - e.NewValue);
                    line[cpt][linecpt[cpt]].Y2 -= (e.OldValue - e.NewValue);

                    crnt_vertical = e.NewValue;
                }
                if (hrz)
                {
                    if (firsttime)
                    {
                        firsttime = false;
                        return;
                    }
                    line[cpt][linecpt[cpt]].X1 -= (e.OldValue - e.NewValue);
                    line[cpt][linecpt[cpt]].X2 -= (e.OldValue - e.NewValue);

                    crnt_horizontal = e.NewValue;
                }
                if (sz)
                {
                    if (firsttime)
                    {
                        firsttime = false;
                        return;
                    }

                    Point a = new Point(line[cpt][linecpt[cpt]].X1, line[cpt][linecpt[cpt]].Y1);
                    Point b = new Point(line[cpt][linecpt[cpt]].X2, line[cpt][linecpt[cpt]].Y2);
                    
                    Vector vec = a - b;
                  
                    vec.Normalize();
                    vec *= (e.NewValue - e.OldValue)*0.5;
                    
                    if(a+vec == b-vec)
                        return;

                    a += vec;
                    b -= vec;

                    line[cpt][linecpt[cpt]].X1 = a.X;
                    line[cpt][linecpt[cpt]].Y1 = a.Y;

                    line[cpt][linecpt[cpt]].X2 = b.X;
                    line[cpt][linecpt[cpt]].Y2 = b.Y;

                    crnt_size = sliderEns.Value;

                }
            }


        }

        private void btnRotation_Click(object sender, RoutedEventArgs e)
        {

            hrz = false;
            vrt = false;
            sz = false;

            sliderEns.Maximum = 180;
            sliderEns.Minimum = -180;

            rtt = true;
            if (linecpt[cpt] >= 0)
            {
                firsttime = true;
                sliderEns.Value = crnt_angle;
                firsttime = false;
            }
        }

        private void btnEffacer_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i <= linecpt[cpt]; i++)
            {
                canvasEns.Children.Remove(line[cpt][i]);
            }
            for (int i = 0; i <= linecpt[cpt]; i++)
            {
                line[cpt].RemoveAt(0);
            }
            linecpt[cpt] = -1;
        }

        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            Line a = new Line();
            a.LayoutTransform = new RotateTransform();
            a.X1 = 300;
            a.Y1 = 100;
            a.X2 = 300;
            a.Y2 = 250;
            a.Stroke = Brushes.Black;
            a.StrokeThickness = 5;
            a.Opacity = 0.8;
            linecpt[cpt]++;
            canvasEns.Children.Add(a);
            line[cpt].Add(a);
            crnt_angle = 0;
            crnt_size = 150;
            crnt_horizontal = 300;
            crnt_vertical = 100;
            
            if (rtt)
            {
                firsttime = true;
                sliderEns.Value = 0;
                firsttime = false;
            }
            if (vrt)
            {
                firsttime = true;
                sliderEns.Value = 100;
                firsttime = false;
            }
            if (hrz)
            {
                firsttime = true;
                sliderEns.Value = 300;
                firsttime = false;
            }
            if (sz)
            {
                firsttime = true;
                sliderEns.Value = 150;
                firsttime = false;
            }
        }

        private void btnSize_Click(object sender, RoutedEventArgs e)
        {
            rtt = false;
            hrz = false;
            vrt = false;
            sliderEns.Maximum = 600;
            sliderEns.Minimum = 0;
            sz = true;
            if (linecpt[cpt] >= 0)
            {
                firsttime = true;
                sliderEns.Value = crnt_size;
            }

        }

        private void btnVertical_Click(object sender, RoutedEventArgs e)
        {
            rtt = false;
            hrz = false;
            sz = false;
            sliderEns.Maximum = 400;
            sliderEns.Minimum = 0;
            vrt = true;
            if (linecpt[cpt] >= 0)
            {
                firsttime = true;
                sliderEns.Value = crnt_vertical;
                firsttime = false;
            }
        }

        private void btnHorizontal_Click(object sender, RoutedEventArgs e)
        {
            rtt = false;
            vrt = false;
            sz = false;
            sliderEns.Maximum = 600;
            sliderEns.Minimum = 0;
            hrz = true;
            if (linecpt[cpt] >= 0)
            {
                firsttime = true;
                sliderEns.Value = crnt_horizontal;
                firsttime = false;
            }
        }
    }
}
