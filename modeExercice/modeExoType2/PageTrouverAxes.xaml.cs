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
            if (!MainWindow.modeEns) modify.Visibility = Visibility.Collapsed;

            precedent.Visibility = Visibility.Collapsed;
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
                while ( ( j <= linecpt[i] ) && ( ( fileline = srl.ReadLine() ) != null ) )
                {
                    Line a = new Line();
                    a.LayoutTransform = new RotateTransform();                    
                    a.X1 = 0;
                    a.Y1 = 0;
                    a.X2 = 0;
                    a.Y2 = int.Parse(fileline);
                    a.Width = 4;
                    a.Stroke = Brushes.Black;
                    a.StrokeThickness = 10;
                    a.Opacity = 0.8;
                    Canvas.SetLeft(a, int.Parse(srl.ReadLine()));
                    Canvas.SetTop(a, int.Parse(srl.ReadLine()));
                    (a.LayoutTransform as RotateTransform).Angle = int.Parse(srl.ReadLine());
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
            if(reponseBox.Text.Length == 0)
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
            if(int.TryParse(reponseBox.Text, out _))
            {
                int real = reponse[cpt];
                int maybe = int.Parse(reponseBox.Text);
                if (maybe == real)
                {
                    SolidColorBrush color = (SolidColorBrush)new BrushConverter().ConvertFrom("#1CB81C");
                    bigCenterRectangle.Stroke = color;
                    smallBelowRectangle.Stroke = color;
                    verifyButton.Background = color;
                    verifyButton.Content = "Bravo !";
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
                    verifyButton.Content = "Oops !";
                }
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
                for (int i = 0; i <= linecpt[cpt] ; i++)
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
                    verifyButton.Content = "Bravo !";
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
                    verifyButton.Content = "Vérifier";
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
                verifyButton.Content = "Bravo !";
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
                verifyButton.Content = "Vérifier";
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
                verifyButton.Content = "Bravo !";
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
                verifyButton.Content = "Vérifier";
                reponseBox.Text = "";
                reponseBox.IsReadOnly = false;
                canvasEtud.Visibility = Visibility.Collapsed;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
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
                    sw.WriteLine(line[j][i].Y2);
                    sw.WriteLine(Canvas.GetLeft(line[j][i]));
                    sw.WriteLine(Canvas.GetTop(line[j][i]));
                    sw.WriteLine((line[j][i].LayoutTransform as RotateTransform).Angle);
                }
                sw.Close();
            }            
        }

        private void sliderEns_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            sliderEns.Value = Math.Round(sliderEns.Value);
            if (linecpt[cpt] >= 0)
            {
                if (rtt)
                {
                    (line[cpt][linecpt[cpt]].LayoutTransform as RotateTransform).Angle = sliderEns.Value;
                }
                if (vrt)
                {
                    Canvas.SetTop(line[cpt][linecpt[cpt]], sliderEns.Value);
                }
                if (hrz)
                {
                    Canvas.SetLeft(line[cpt][linecpt[cpt]], sliderEns.Value);
                }
                if (sz)
                {
                    line[cpt][linecpt[cpt]].Y2 = sliderEns.Value;
                }
            }
            
            
        }

        private void btnRotation_Click(object sender, RoutedEventArgs e)
        {                        
            hrz = false;
            vrt = false;
            sz = false;
            sliderEns.Maximum = 360;
            rtt = true;
            if (linecpt[cpt] >= 0)
            {
                sliderEns.Value = (line[cpt][linecpt[cpt]].LayoutTransform as RotateTransform).Angle;
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
            a.X1 = 0;
            a.Y1 = 0;
            a.X2 = 0;
            a.Y2 = 150;
            a.Width = 4;
            a.Stroke = Brushes.Black;
            a.StrokeThickness = 10;
            a.Opacity = 0.8;
            Canvas.SetLeft(a, 300);
            Canvas.SetTop(a, 100);
            linecpt[cpt]++;
            canvasEns.Children.Add(a);
            line[cpt].Add(a);
            if (rtt)
            {
                sliderEns.Value = (line[cpt][linecpt[cpt]].LayoutTransform as RotateTransform).Angle;
            }
            if (vrt)
            {
                sliderEns.Value = Canvas.GetTop(line[cpt][linecpt[cpt]]);
            }
            if (hrz)
            {
                sliderEns.Value = Canvas.GetLeft(line[cpt][linecpt[cpt]]);
            }
            if (sz)
            {
                sliderEns.Value = line[cpt][linecpt[cpt]].Y2;
            }
        }

        private void btnSize_Click(object sender, RoutedEventArgs e)
        {            
            rtt = false;
            hrz = false;
            vrt = false;
            sliderEns.Maximum = 600;
            sz = true;                        
            if (linecpt[cpt] >= 0)
            {
                sliderEns.Value = line[cpt][linecpt[cpt]].Y2;
            }

        }

        private void btnVertical_Click(object sender, RoutedEventArgs e)
        {            
            rtt = false;
            hrz = false;            
            sz = false;
            sliderEns.Maximum = 400;
            vrt = true;
            if (linecpt[cpt] >= 0)
            {
                sliderEns.Value = Canvas.GetTop(line[cpt][linecpt[cpt]]);
            }                                    
        }

        private void btnHorizontal_Click(object sender, RoutedEventArgs e)
        {            
            rtt = false;            
            vrt = false;
            sz = false;
            sliderEns.Maximum = 600;
            hrz = true;
            if (linecpt[cpt] >= 0)
            {
                sliderEns.Value = Canvas.GetLeft(line[cpt][linecpt[cpt]]);
            }                        
        }
    }
}
