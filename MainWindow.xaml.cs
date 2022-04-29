using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Projet2Cp.Utili; 

namespace Projet2Cp
{


    public partial class MainWindow : Window
    {
        static public Boolean modeLibre , modeEns; 



        public MainWindow()
        {
            InitializeComponent();
            modeLibre = true ;
            modeEns = true;
            if (modeLibre)  MainFrame.NavigationService.Navigate(new ModeLibre());
            else MainFrame.NavigationService.Navigate(new LibreExoEns());
                 
        }
        void ButtonClickExo(object sender, RoutedEventArgs e)
        {
            SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            /*TriangleCours.Visibility = Visibility.Hidden;
            TriangleLibre.Visibility = Visibility.Hidden;
            TriangleExo.Visibility = Visibility.Visible;*/
            ExoImg.Source = new BitmapImage(new Uri("./icons/Acceuil/Exercices Jaune.png", UriKind.Relative));
            TBExo.Foreground = color;
            LibreImg.Source = new BitmapImage(new Uri("./icons/Acceuil/Libre.png", UriKind.Relative));
            CoursImg.Source = new BitmapImage(new Uri("./icons/Acceuil/Cours.png", UriKind.Relative));
            TBCours.Foreground = Brushes.Snow;
            TBLibre.Foreground = Brushes.Snow;
            
        }

        private void ButtonClickLibre(object sender, RoutedEventArgs e)
        {
            SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            /*TriangleCours.Visibility = Visibility.Hidden;
            TriangleLibre.Visibility = Visibility.Visible;
            TriangleExo.Visibility = Visibility.Hidden;*/
            LibreImg.Source = new BitmapImage(new Uri("./icons/Acceuil/Libre Jaune.png", UriKind.Relative));
            TBLibre.Foreground = color;
            ExoImg.Source = new BitmapImage(new Uri("./icons/Acceuil/Exercices.png", UriKind.Relative));
            CoursImg.Source = new BitmapImage(new Uri("./icons/Acceuil/Cours.png", UriKind.Relative));
            TBCours.Foreground = Brushes.Snow;
            TBExo.Foreground = Brushes.Snow;



        }

        private void ButtonClickCours(object sender, RoutedEventArgs e)
        {
            SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            /*TriangleCours.Visibility = Visibility.Visible;
            TriangleLibre.Visibility = Visibility.Hidden;
            TriangleExo.Visibility = Visibility.Hidden;*/
            CoursImg.Source = new BitmapImage(new Uri("./icons/Acceuil/Cours Jaune.png", UriKind.Relative));
            TBCours.Foreground = color;
            LibreImg.Source = new BitmapImage(new Uri("./icons/Acceuil/Libre.png", UriKind.Relative));
            ExoImg.Source = new BitmapImage(new Uri("./icons/Acceuil/Exercices.png", UriKind.Relative));
            TBLibre.Foreground = Brushes.Snow;
            TBExo.Foreground = Brushes.Snow;


        }

        private void ButtonClickLogout(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


    }
}
