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
using ModeCours;
using Project;

namespace Projet2Cp
{


    public partial class MainWindow : Window
    {
        static public Boolean modeLibre , modeEns , francais=true;
        public static Frame MainFrame;
        


        public MainWindow()
        {
            InitializeComponent();
            
         
            modeEns = false;
            MainFrame = new Frame();
            myDock.Children.Add(MainFrame);
            MainFrame.NavigationUIVisibility = NavigationUIVisibility.Hidden;






            SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            ExoImg.Source = new BitmapImage(new Uri("./Acceuil/Exercices Jaune.png", UriKind.Relative));
            TBExo.Foreground = color;
            LibreImg.Source = new BitmapImage(new Uri("./Acceuil/Libre.png", UriKind.Relative));
            CoursImg.Source = new BitmapImage(new Uri("./Acceuil/Cours.png", UriKind.Relative));
            TBCours.Foreground = Brushes.Snow;
            TBLibre.Foreground = Brushes.Snow;
            MainFrame.NavigationService.Navigate(new PagesNiveaux());


            
                 
        }
        void ButtonClickExo(object sender, RoutedEventArgs e)
        {
            SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
           
            ExoImg.Source = new BitmapImage(new Uri("./Acceuil/Exercices Jaune.png", UriKind.Relative));
            TBExo.Foreground = color;
            LibreImg.Source = new BitmapImage(new Uri("./Acceuil/Libre.png", UriKind.Relative));
            CoursImg.Source = new BitmapImage(new Uri("./Acceuil/Cours.png", UriKind.Relative));
            TBCours.Foreground = Brushes.Snow;
            TBLibre.Foreground = Brushes.Snow;
            MainFrame.NavigationService.Navigate(new PagesNiveaux());
        }

        private void ButtonClickLibre(object sender, RoutedEventArgs e)
        {
            SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            
            LibreImg.Source = new BitmapImage(new Uri("./Acceuil/Libre Jaune.png", UriKind.Relative));
            TBLibre.Foreground = color;
            ExoImg.Source = new BitmapImage(new Uri("./Acceuil/Exercices.png", UriKind.Relative));
            CoursImg.Source = new BitmapImage(new Uri("./Acceuil/Cours.png", UriKind.Relative));
            TBCours.Foreground = Brushes.Snow;
            TBExo.Foreground = Brushes.Snow;
            modeLibre = true;
            MainFrame.NavigationService.Navigate(new ModeLibre());
         



        }

        private void ButtonClickCours(object sender, RoutedEventArgs e)
        {
            SolidColorBrush color = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFCC00"));
            
            CoursImg.Source = new BitmapImage(new Uri("./Acceuil/Cours Jaune.png", UriKind.Relative));
            TBCours.Foreground = color;
            LibreImg.Source = new BitmapImage(new Uri("./Acceuil/Libre.png", UriKind.Relative));
            ExoImg.Source = new BitmapImage(new Uri("./Acceuil/Exercices.png", UriKind.Relative));
            TBLibre.Foreground = Brushes.Snow;
            TBExo.Foreground = Brushes.Snow;

            if (francais) MainFrame.NavigationService.Navigate(new PagePrincCours());
            else MainFrame.NavigationService.Navigate(new PagePrincCoursAr());


        }

        private void ButtonClickLogout(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


    }
}
