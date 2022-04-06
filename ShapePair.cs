using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace Projet2Cp
{
    internal class ShapePair
    {
        Canvas canvas;
        public Shape origin { get; set; }
        private List<Ellipse> oEllipse = new List<Ellipse>();

        private Line sline ; //will represent the current line of symmetry between the two shapes
        private Point spoint; //Will represent the current point of symmetry between the two shapes
        public Shape sym { get; set; }
        private List<Ellipse> sEllipse = new List<Ellipse>();

        //============================================================================================================================//
        //                                              TESTING CONSTRUCOR                                                            //
        //============================================================================================================================//

        public ShapePair(Shape origin,Canvas canvas,MouseEventHandler shapeMouseEnter, MouseEventHandler shapeMouseLeave)
        {
            this.canvas = canvas;
            this.origin = origin;
            if(origin is Polygon)
                canvas.Children.Add(origin);
            drawEllipses(true);
          
            //------------------------test
            /* sline = new Line()
             {
                 X1 = 400,
                 Y1 = 0,
                 X2 = 400,
                 Y2 = 300,
                 Stroke = Brushes.Blue,
                 StrokeThickness = 3,
             };

             canvas.Children.Add(sline);

             symGen(shapeMouseEnter, shapeMouseLeave);*/

        }

        //============================================================================================================================//
        //                                              drawingOrigin                                                                 //
        //============================================================================================================================//

        private void drawEllipses(bool isOrigin) //draws the ellipseCollection of either origin or sym on canvas
        {
                PointCollection shapepc;
                
                if(isOrigin)
                {
                    for (int i = 0; i < oEllipse.Count; i++)
                        canvas.Children.Remove(oEllipse[i]);
                
                    oEllipse = new List<Ellipse>();
                
                    if (origin is Polygon)
                        shapepc = ((Polygon)origin).Points;
                    else
                        shapepc = ((Polyline)origin).Points;

                }
                
                else
                {
                    for (int i = 0; i < sEllipse.Count; i++)
                        canvas.Children.Remove(sEllipse[i]);

                    sEllipse = new List<Ellipse>();

                    if (sym is Polygon)
                        shapepc = ((Polygon)sym).Points;
                    else
                        shapepc = ((Polyline)sym).Points;

                }

                //-------------------------------
                Ellipse ellipse;

                for(int i = 0; i<shapepc.Count; i++)
                {
                    ellipse = new Ellipse()
                    {
                        Height = 10,
                        Width = 10,
                        Fill = Brushes.White,
                        Stroke = Brushes.Black,

                    };

                    if (isOrigin)
                        oEllipse.Add(ellipse);
                    else
                        sEllipse.Add(ellipse);
                    
                    canvas.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, shapepc[i].X - 5);
                    Canvas.SetTop(ellipse, shapepc[i].Y - 5);
                }

        }
    



    //============================================================================================================================//
    //                                                          translating                                                       //
    //============================================================================================================================//

        public void translating(Vector vec, bool isOrigin)
        {
           
            PointCollection shapepc;
            
            if (origin is Polygon)
                    shapepc = ((Polygon)origin).Points;
            else
                    shapepc = ((Polyline)origin).Points;
            
            for (int i = 0; i < shapepc.Count; i++)
                shapepc[i] = shapepc[i] + vec;

            drawEllipses(isOrigin);
          
        }



        //============================================================================================================================//
        //                                                          SYM_GEN                                                           //
        //============================================================================================================================//



        public void symGen(MouseEventHandler shapeMouseEnter, MouseEventHandler shapeMouseLeave)
        {
            PointCollection shapepc;
            PointCollection symmetricpc = new PointCollection(); //symmetric point collection

            Point p1 = new Point(sline.X1, sline.Y1);
            Point p2 = new Point(sline.X2, sline.Y2);

            Shape old;
            Shape newer;

            if (sym == null) //donc orgin != null c'est lui le old
                old = origin;
            else
                old = sym;

                //On recupere la PointCollection de old et on instantie newer selon la nature de old
                if (old is Polygon)
                { 
                    shapepc = ((Polygon)old).Points;
                    newer = new Polygon()
                    {
                        Stroke = Brushes.Black, //((Polygon)old).Stroke, //j'ai de gros doute sur cette manoeuvr ....
                        StrokeThickness = 3,//((Polygon)old).StrokeThickness,
                        Fill = Brushes.Red,//((Polygon)old).Fill,
                    };
                    
                    ((Polygon)newer).Points = symmetricpc;
                }
                else
                {
                    shapepc = ((Polyline)old).Points;
                    newer = new Polyline()
                    {
                        Stroke = Brushes.Black,//((Polyline)old).Stroke, //j'ai de gros doute sur cette manoeuvr ....
                        StrokeThickness = 3,//((Polyline)old).StrokeThickness,
                       // Fill = Brushes.Red,//((Polyline)old).Fill,

                    };
                    
                    ((Polyline)newer).Points = symmetricpc;

                }

                for(int i = 0; i<shapepc.Count; i++)
                    symmetricpc.Add( symPoint( p1, p2, shapepc[i] ) );

            newer.MouseEnter += shapeMouseEnter;
            newer.MouseLeave += shapeMouseLeave;


            canvas.Children.Add(newer);
            
            if (sym == null)
            {
                sym = newer;
                drawEllipses(false);
            }
            else
            {
                origin = newer;
                drawEllipses(true);

            }


        }

        private double distancePointPoint(Point firstPoint, Point secondPoint)
        {
            return Math.Sqrt(Math.Pow(secondPoint.X - firstPoint.X, 2) + Math.Pow(secondPoint.Y - firstPoint.Y, 2));
        }


    //=================================================================================================================//
    //                                                  AxialSymGen                                                    //
    //=================================================================================================================//

    private Point symPoint(Point p1, Point p2, Point p3)
    {
        double k = ((p2.Y - p1.Y) * (p3.X - p1.X) - (p2.X - p1.X) * (p3.Y - p1.Y)) / (Math.Pow(p2.X - p1.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        return new Point( 2 * ( p3.X - k * (p2.Y-p1.Y) ) - p3.X , 2 * ( p3.Y + k*(p2.X - p1.X) ) - p3.Y );
    }


    }//END OF CLASS

}//END OF NAMESPACE

