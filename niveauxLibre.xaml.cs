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
    public partial class niveauxLibre : UserControl
    {




        public niveauxLibre()
        { 
            InitializeComponent();
           
        }


        public int Selected()
        {
            foreach(RadioButton elem in grid.Children)
            {
                if( elem.IsChecked != null)
                {
                    if ((bool)(elem).IsChecked) return Grid.GetColumn(elem) ; 
                    
                }
                
            }
            return -1;

            
        }
        
    }
}
