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
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        private Brush pickedColorTrace { get; set; }
        private Brush pickedColorRempli { get; set; }

        Boolean choice=true;  // true for "tracé"

        private Border hitTrace , hitRempli; 
        public ColorPicker()
        {
            InitializeComponent();

            Border BORD;
            int k = 0; 
            String[] colors  =new string[20] {"#1abc9c", "#16a085", "#2ecc71", "#27ae60", "#3498db", "#2980b9", "#9b59b6", "#8e44ad", "#34495e", "#2c3e50", "#f1c40f", "#f39c12", "#e67e22", "#d35400", "#e74c3c", "#c0392b", "#ecf0f1", "#bdc3c7", "#95a5a6", "#7f8c8d"};
            for(int i = 0; i < grid.RowDefinitions.Count; i++)
            {
                for(int j =0;j< grid.ColumnDefinitions.Count; j++)
                {
                    BORD = new Border()
                    {
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(5, 5, 5, 5),
                        BorderBrush = Brushes.Black,
                        Height = 17.5,
                        Width = 17.5,

                    };
                    BORD.Background= (Brush)(new BrushConverter().ConvertFrom(colors[k]));
                    k++;
                    grid.Children.Add(BORD);
                    Grid.SetColumn(BORD, j);
                    Grid.SetRow(BORD, i);
                }
            }

            grid.MouseLeftButtonDown += pickColor;
            Trace.IsChecked = true;

          
        }

        private void Trace_Click(object sender, RoutedEventArgs e)
        {
            if (hitRempli != null) hitRempli.BorderBrush = Brushes.Black;
            if (hitTrace != null) hitTrace.BorderBrush = Brushes.White;
            
            choice = true;

        }

        private void Rempli_Click(object sender, RoutedEventArgs e)
        {
            if (hitTrace != null) hitTrace.BorderBrush = Brushes.Black;
            if (hitRempli != null) hitRempli.BorderBrush = Brushes.White;
           
            choice = false;

        }

        private void pickColor(Object sender , MouseButtonEventArgs e)
        {
            if(e.Source is Border)
            {
                if (choice)
                {
                    if (hitTrace != null) hitTrace.BorderBrush = Brushes.Black;
                    hitTrace = ((Border)e.Source);
                    pickedColorTrace = hitTrace.Background;
                    hitTrace.BorderBrush = Brushes.White;
                    //MessageBox.Show(String.Format("Picked color is{0}", pickedColor));
                }
                else
                {
                    if (hitRempli != null) hitRempli.BorderBrush = Brushes.Black;
                    hitRempli = ((Border)e.Source);
                    pickedColorRempli = hitRempli.Background;
                    hitRempli.BorderBrush = Brushes.White;

                }
            }
        }

        
    }
}
