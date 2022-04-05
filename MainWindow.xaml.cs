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
        double gridStep = 30;
        private List<ShapePair> shapes;
        private Point mousePosition; // to save the mouse position when needed 
        private double actualPoint;
        private PointCollection drawingPoints;
        Point centrePoly = new Point();
        




        public MainWindow()
        {
            InitializeComponent();
            toolBar.addPolygon.Click += addPolygon;

        }

        private void DrawPolygon(Object sender, MouseEventArgs e)
        {
            mousePosition= e.GetPosition(canvas);
        }
        private void addPolygon(Object sender, RoutedEventArgs e)
        {
            centrePoly.X=(double)canvas.ActualWidth/(double)2;
            centrePoly.Y=(double)canvas.ActualHeight/(double)2;
            canvas.Children.Add(new Ellipse()
            {
                Height = 10,
                Width = 10,
                Fill =toolBar.CP.pickedColorRempli,
                Margin = new Thickness(centrePoly.X+ (toolBar.Rayon*gridStep), centrePoly.Y+5,0,0)

            });;
            double angle = 360/toolBar.NbCote;
            Polygon poly = new Polygon() {
                Fill = toolBar.CP.pickedColorRempli,
                Stroke = toolBar.CP.pickedColorTrace,

            };
            Point fstPt = new Point(centrePoly.X + 50, centrePoly.Y);
            poly.Points.Add(fstPt);
            RotateTransform trans;

            for (int i = 1; i < toolBar.NbCote; i++)
            {
                trans = new RotateTransform()
                {
                    CenterX = centrePoly.X,
                    CenterY = centrePoly.Y,
                    Angle = -angle*i,

                };
             
                poly.Points.Add(trans.Transform(fstPt));
               
            }
            canvas.Children.Add(poly);



        }
    }
}
