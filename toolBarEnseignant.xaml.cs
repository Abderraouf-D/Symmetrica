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
    /// <summary>
    /// Interaction logic for toolBarEnseignant.xaml
    /// </summary>
    public partial class toolBarEnseignant : UserControl
    {

        [Browsable(true), EditorBrowsable(EditorBrowsableState.Always), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Rayon;
        public int NbCote;
        public int ray;
        public toolBarEnseignant()
        {
            InitializeComponent();




            Rayon = rayon.SelectedIndex + 3;
            NbCote = nbCote.SelectedIndex + 3;
        }

        private void nbCote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            NbCote = nbCote.SelectedIndex + 3;

        }

        private void rayon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            Rayon = rayon.SelectedIndex + 3;


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
