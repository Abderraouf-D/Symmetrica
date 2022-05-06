using System;
using System.Collections;
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
  
    public partial class toolBarEnseignant : UserControl
    {

        
        public toolBarEnseignant()
        {
            InitializeComponent();

           if (!MainWindow.modeEns) EditEns.Visibility = Visibility.Collapsed;
            this.Resources.MergedDictionaries.Add(MainWindow.ResLibre);
            



        }

       

        

        public string selectedAxe()
        {
            foreach (RadioButton elem in ensStack.Children)
            {
                if (elem.IsChecked != null)
                {
                    if ((bool)(elem).IsChecked) return elem.Name;

                }

            }
            return null;
        }
    }
}
