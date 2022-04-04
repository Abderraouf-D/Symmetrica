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

namespace Projet2Cp
{

    
    public partial class MainWindow : Window

    {
        private List<ShapePair> shapes; 
        private double mousePosition; // to save the mouse position when needed 
        private double actualPoint;
        private PointCollection drawingPoints; 




        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
