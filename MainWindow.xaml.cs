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
using System.IO;

namespace Projet2Cp
{


    public partial class MainWindow : Page
    {
        static public Boolean modeLibre , modeEns , francais=true;
        public static FaderFrame MainFrame;
        public static  Eleve eleve;
        public static PagesNiveaux pageNiveaux = new PagesNiveaux();
        public static ResourceDictionary ResLibre;
         
        
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


            if (modeEns) UserName.Text = francais ? "Enseignant" : "أستاذ";
            else UserName.Text = eleve.getNom();
            MainFrame.NavigationService.Navigate(pageNiveaux);


            
                 
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
            
            MainFrame.NavigationService.Navigate(pageNiveaux);

            

           

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

        private void loaded (Object sender , RoutedEventArgs e)
        {
            if (francais) ResLibre = App.FrResLibre;
            else ResLibre = App.ArResLibre;
            this.Resources.MergedDictionaries.Add(ResLibre);
            francais = PageChoixMode.francais;
        }

        private void logo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            save(eleve);
            symmetrica.symmetricaFrm.NavigationService.Navigate(symmetrica.pagechoix);

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
            save(eleve);
            System.Windows.Application.Current.Shutdown();
        }


        public void save(Eleve elev)
        {
            if (!modeEns)
            {
                int i;
                for (i = 0; i < PageChoixMode.users.Length; i++)
                {
                    if (elev.getNom().Equals(PageChoixMode.users[i]))
                    {
                        break;
                    }
                }
                FileStream fs = new FileStream("Data/Users/" + elev.getNom() + eleve.getId().ToString() + "/data.bin", FileMode.Open);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Seek(0, SeekOrigin.Begin);
                bw.Write((int)elev.getProgressCen());
                bw.Write((int)elev.getProgressAxe());
                fs.Close();
                bw.Close();
            }
        }





    }
}
