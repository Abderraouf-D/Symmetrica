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

        public void hideNiv(int k)
        {
            switch (k)
            {
                case 1: b1.Visibility = b2.Visibility = b3.Visibility = Visibility.Collapsed;
                    break;
                case 2: b4.Visibility = b5.Visibility = b6.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    b7.Visibility = b8.Visibility = b9.Visibility = Visibility.Collapsed;
                    break;
                default:   break; 
            }
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //grid.Width = nivStack.ActualWidth;
        }
    }
}
