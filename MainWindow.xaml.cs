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
        Line axeSym;
        Ellipse centreSym; 
        




        public MainWindow()
        {
            InitializeComponent();
            toolBar.addPolygon.Click += addPolygon;
            toolBar.horiz.Click += updateAxe;
            toolBar.verti.Click += updateAxe;
            toolBar.diag1.Click += updateAxe;
            toolBar.diag2.Click += updateAxe;
            toolBar.centre.Click += updateAxe;
            centreSym = new Ellipse()
            {
                Height = 10,
                Width = 10,
                Fill = toolBar.CP.pickedColorRempli,
                Stroke = toolBar.CP.pickedColorTrace,
                

            };
            axeSym = new Line();
            axeSym.StrokeThickness = 1;
            axeSym.Stroke = toolBar.CP.pickedColorTrace;
            axeSym.X1 = canvas.ActualWidth / 2;
            axeSym.Y1 = 0.5;
            axeSym.X2 = canvas.ActualWidth / 2;
            axeSym.Y2 = canvas.ActualHeight;
            canvas.Children.Add(axeSym);
            if (axeSym == null) MessageBox.Show("hh");
           



        }
        private void updateAxe(Object sender, RoutedEventArgs e)
        {
            axeSym.Stroke = toolBar.CP.pickedColorTrace;
            canvas.Children.Remove(centreSym);
            switch (true)
            {
                case true when (bool) toolBar.horiz.IsChecked:
                    {
                        axeSym.X1 = canvas.ActualWidth / 2;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = canvas.ActualWidth / 2;
                        axeSym.Y2 = canvas.ActualHeight ;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);



                        break;

                    }
               case true when (bool)toolBar.verti.IsChecked:
                    {
                        axeSym.X1 = 0.5;
                        axeSym.Y1 = canvas.ActualHeight/ 2;
                        axeSym.X2 = canvas.ActualWidth;
                        axeSym.Y2 = canvas.ActualHeight / 2;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);


                        break;
                    }
                case true when (bool)toolBar.diag1.IsChecked:
                    {
                        axeSym.X1 = 0.5;
                        axeSym.Y1 =  0.5;
                        axeSym.X2 = canvas.ActualWidth;
                        axeSym.Y2 = canvas.ActualHeight;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);


                        break;
                    }
                case true when (bool)toolBar.diag2.IsChecked:
                    {
                        axeSym.X1 = canvas.ActualWidth;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = 0.5;
                        axeSym.Y2 = canvas.ActualHeight; ;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);

                        break;
                    }
                case true when (bool)toolBar.centre.IsChecked:
                    {
                        canvas.Children.Remove(axeSym);
                        if (!(canvas.Children.Contains(centreSym))) { 
                            canvas.Children.Add(centreSym);
                            Canvas.SetLeft(centreSym, canvas.ActualWidth / 2-5);
                            Canvas.SetTop(centreSym, canvas.ActualHeight / 2-5);
                        }


                        break;
                    }

                default:
                    {

                        break;
                    }
              


            }
        }

            private void DrawPolygon(Object sender, MouseEventArgs e)
        {
            mousePosition= e.GetPosition(canvas);
        }
        private void addPolygon(Object sender, RoutedEventArgs e)
        {
            centrePoly.X=(double)canvas.ActualWidth/(double)2;
            centrePoly.Y=(double)canvas.ActualHeight/(double)2;
            
            double angle = 360/toolBar.NbCote;
            Polygon poly = new Polygon() {
                Fill = toolBar.CP.pickedColorRempli,
                Stroke = toolBar.CP.pickedColorTrace,

            };
            Point fstPt = new Point(centrePoly.X + toolBar.Rayon*gridStep, centrePoly.Y);
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
