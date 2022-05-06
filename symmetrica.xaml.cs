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
using DrWPF.Windows.Controls;
using Project;
using System.IO;
namespace Projet2Cp
   
{
    /// <summary>
    /// Interaction logic for symmetrica.xaml
    /// </summary>
    public partial class symmetrica : Window
    {
        public static Page pagechoix = new PageChoixMode();
        public static FaderFrame symmetricaFrm;
        public symmetrica()
        {
            InitializeComponent();
            symmetricaFrm = new FaderFrame();
            symGrid.Children.Add(symmetricaFrm);
            symmetricaFrm.NavigationUIVisibility = NavigationUIVisibility.Hidden;

            App.ArResLibre = this.Resources.MergedDictionaries[0];
            App.FrResLibre = this.Resources.MergedDictionaries[1];
            symmetricaFrm.NavigationService.Navigate(pagechoix);
            App.ArResLibre = this.Resources.MergedDictionaries[0];
            App.FrResLibre = this.Resources.MergedDictionaries[1];





        }
    }
}
