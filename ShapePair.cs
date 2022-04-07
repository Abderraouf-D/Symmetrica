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
        public List<Ellipse> oEllipse = new List<Ellipse>();

        private Line sline; //will represent the current line of symmetry between the two shapes
        private Point spoint; //Will represent the current point of symmetry between the two shapes
        public Shape sym { get; set; }
        public List<Ellipse> sEllipse = new List<Ellipse>();

        MouseEventHandler shapeMouseEnter;
        MouseEventHandler shapeMouseLeave;

        //============================================================================================================================//
        //                                              TESTING CONSTRUCOR                                                            //
        //============================================================================================================================//

        public ShapePair(Shape origin, Canvas canvas,Line sline, MouseEventHandler shapeMouseEnter, MouseEventHandler shapeMouseLeave)
        {
            this.canvas = canvas;
            this.origin = origin;

            if (origin is Polygon)
                canvas.Children.Add(origin);

            this.shapeMouseEnter = shapeMouseEnter;
            this.shapeMouseLeave = shapeMouseLeave;

            drawEllipses(true);



            this.sline = sline;

           

            

        }

        //============================================================================================================================//
        //                                              drawingOrigin                                                                 //
        //============================================================================================================================//

        private void drawEllipses(bool isOrigin) //draws the ellipseCollection of either origin or sym on canvas
        {
            PointCollection shapepc;

            if (isOrigin)
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

            for (int i = 0; i < shapepc.Count; i++)
            {
                ellipse = new Ellipse()
                {
                    Height = 10,
                    Width = 10,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,

                };

                ellipse.MouseEnter += shapeMouseEnter;
                ellipse.MouseLeave += shapeMouseLeave;

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

        public void translating(Vector vec, bool isOrigin, Point mousePosition)
        {

            PointCollection shapepc;

            Point endPoint = new Point()
            {
                X = vec.X,
                Y = vec.Y,
            };

            Point p1 = new Point()
            {
                X = sline.X1,
                Y = sline.Y1,
            };

            Point p2 = new Point()
            {
                X = sline.X2,
                Y = sline.Y2,
            };

            endPoint = symPoint(p1, p2, endPoint);

            Point ori = new Point(0, 0);
            ori = symPoint(p1, p2, ori);

            Vector reverseVec = endPoint - ori;

            Shape trans;
            Shape revTrans;

            if (isOrigin)
            {
                trans = origin;
                revTrans = sym;

            }
            else
            {
                trans = sym;
                revTrans = origin;
            }

            Point revMousePosition = symPoint(p1, p2, mousePosition); // ou ma na3ref

            if (trans != null)
            {
                if (trans is Polygon)
                    shapepc = ((Polygon)trans).Points;
                else
                    shapepc = ((Polyline)trans).Points;

                bool stop = false;

                for (int i = 0; i < shapepc.Count && !stop; i++)
                {
                    if (distancePointPoint(mousePosition, shapepc[i]) < 15) // on verra pour la valeur exacte apres
                    {
                        shapepc[i] = shapepc[i] + vec;
                        stop = true;
                    }

                }
                if (!stop)
                    for (int i = 0; i < shapepc.Count; i++)
                        shapepc[i] = shapepc[i] + vec;

                drawEllipses(trans == origin);
            }

            if (revTrans != null)
            {
                if (revTrans is Polygon)
                    shapepc = ((Polygon)revTrans).Points;
                else
                    shapepc = ((Polyline)revTrans).Points;

                bool stop = false;
                for (int i = 0; i < shapepc.Count && !stop; i++)
                {
                    if (distancePointPoint(revMousePosition, shapepc[i]) < 15) // on verra pour la valeur exacte apres
                    {
                        shapepc[i] = shapepc[i] + reverseVec;
                        stop = true;
                    }

                }

                if (!stop)
                    for (int i = 0; i < shapepc.Count; i++)
                        shapepc[i] = shapepc[i] + reverseVec;
                drawEllipses((revTrans == origin));

            }
        }



        //============================================================================================================================//
        //                                                          SYM_GEN                                                           //
        //============================================================================================================================//



        public void symGen(MouseEventHandler shapeMouseEnter, MouseEventHandler shapeMouseLeave)
        {
            if (sline != null)
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
                        Stroke = old.Stroke, //((Polygon)old).Stroke, //j'ai de gros doute sur cette manoeuvr ....
                        StrokeThickness = old.StrokeThickness,//((Polygon)old).StrokeThickness,
                        Fill = old.Fill,//((Polygon)old).Fill,
                    };

                    ((Polygon)newer).Points = symmetricpc;
                }
                else
                {
                    shapepc = ((Polyline)old).Points;
                    newer = new Polyline()
                    {
                        Stroke = old.Stroke,//((Polyline)old).Stroke, //j'ai de gros doute sur cette manoeuvr ....
                        StrokeThickness = old.StrokeThickness,//((Polyline)old).StrokeThickness,
                                                              //((Polyline)old).Fill,
                    };

                    ((Polyline)newer).Points = symmetricpc;

                }

                for (int i = 0; i < shapepc.Count; i++)
                    symmetricpc.Add(symPoint(p1, p2, shapepc[i]));

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
            else
            {
                MessageBox.Show("Veuillez choisir un axe de symétrie!");
            }


        }

        private double distancePointPoint(Point firstPoint, Point secondPoint)
        {
            return Math.Sqrt(Math.Pow(secondPoint.X - firstPoint.X, 2) + Math.Pow(secondPoint.Y - firstPoint.Y, 2));
        }

        private double distancePointLine(Point firstPoint, Point secondPoint, Point point)
        {
            double num = (secondPoint.X - firstPoint.X) * (firstPoint.Y - point.Y) - (firstPoint.X - point.X) * (secondPoint.Y - firstPoint.Y);
            num = Math.Abs(num);
            num = num / Math.Sqrt(Math.Pow(secondPoint.X - firstPoint.X, 2) + Math.Pow(secondPoint.Y - firstPoint.Y, 2));

            return num;
        }


        //=================================================================================================================//
        //                                                           AxialSymGen                                           //
        //=================================================================================================================//

        private Point symPoint(Point p1, Point p2, Point p3)
        {
            double k = ((p2.Y - p1.Y) * (p3.X - p1.X) - (p2.X - p1.X) * (p3.Y - p1.Y)) / (Math.Pow(p2.X - p1.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            return new Point(2 * (p3.X - k * (p2.Y - p1.Y)) - p3.X, 2 * (p3.Y + k * (p2.X - p1.X)) - p3.Y);
        }



    }//END OF CLASS

}//END OF NAMESPACE

