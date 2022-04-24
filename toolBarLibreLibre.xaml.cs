using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Projet2Cp
{
    
    public partial class toolBarLibreLibre : UserControl
    {

        
        public toolBarLibreLibre()
        {
            InitializeComponent();

            canvasUC.rayon = rayon.SelectedIndex + 3;
            canvasUC.cote =nbCote.SelectedIndex + 3;
        }

        private void nbCote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            canvasUC.rayon = nbCote.SelectedIndex + 3; 

        }

        private void rayon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            canvasUC.cote = rayon.SelectedIndex+3;
            

        }

      
    }
}
