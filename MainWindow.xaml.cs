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
using System.Windows.Threading;
using System.Threading;
using System.Windows.Media.Animation;
using DrWPF.Windows.Controls;
namespace Projet2Cp
{


    public partial class MainWindow : Page
    {
        static public Boolean modeLibre , modeEns , francais=true;
        public static FaderFrame MainFrame;

        public static  Eleve eleve;



        public MainWindow(Boolean modeEns, Boolean francais , Eleve eleve )
        {
            InitializeComponent();

            MainWindow.eleve = eleve;
            MainWindow.modeEns = modeEns;
            MainWindow.francais = francais; 

            MainFrame = new FaderFrame();
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
            PagesNiveaux page = new PagesNiveaux();
            MainFrame.NavigationService.Navigate(page);

            page.niveau = 1;
            PagesNiveaux.btn_niveau1_is_clicked = true;
            PagesNiveaux.btn_niveau2_is_clicked = false;
            PagesNiveaux.btn_niveau3_is_clicked = false;
            page.BtnNiveau1.Margin = new System.Windows.Thickness(0, 0, 80, 0);
            page.BtnNiveau2.Margin = new System.Windows.Thickness(0, 20, 80, 0);
            page.BtnNiveau3.Margin = new System.Windows.Thickness(0, 20, 0, 0);
            page.BorderContainer.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#A2DBA1");
            page.BtnQuiz.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#82AF81");
            page.BtnOuiNon.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#82AF81");
            page.BtnTrouverLesAxes.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#82AF81");
            page.BtnDessinerLeSymetr.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#82AF81");




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

        private void logo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            symmetrica.symmetricaFrm.NavigationService.Navigate(new PageChoixMode());

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
