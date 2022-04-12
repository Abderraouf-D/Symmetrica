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

       
        private Nullable<Point> spoint; //Will represent the current point of symmetry between the two shapes
        public Shape sym { get; set; }
        public List<Ellipse> sEllipse = new List<Ellipse>();

        MouseEventHandler shapeMouseEnter;
        MouseEventHandler shapeMouseLeave;

        public bool IsTransformable { get; set; }

        //============================================================================================================================//
        //                                              TESTING CONSTRUCOR                                                            //
        //============================================================================================================================//

        public ShapePair(Shape origin, Canvas canvas, MouseEventHandler shapeMouseEnter, MouseEventHandler shapeMouseLeave)
        {
            IsTransformable = true;

            this.canvas = canvas;
            this.origin = origin;
            if (origin is Polygon)
                canvas.Children.Add(origin);

            this.shapeMouseEnter = shapeMouseEnter;
            this.shapeMouseLeave = shapeMouseLeave;

            drawEllipses(true);

            


        }


        //============================================================================================================================//
        //                                                      RemoveSegment                                                         //
        //============================================================================================================================//
        public void RemoveSegment(int indexSegment)
        {
            PointCollection shapepc;
            if (origin != null)
            {
                if (origin is Polygon)
                    shapepc = ((Polygon)origin).Points;
                else
                    shapepc = ((Polyline)origin).Points;

                shapepc.RemoveAt(indexSegment);
                /*

                Polyline poly = new Polyline();
                if (shapepc.Count <= 3 && (origin is Polygon))
                {
                    poly = new Polyline();
                    poly.Points = ((Polygon)origin).Points;
                    poly.Stroke = ((Polygon)origin).Stroke;
                    poly.StrokeThickness = 8;
                    poly.Fill = Brushes.Yellow;
                    canvas.Children.Remove(origin);
                    origin = poly;
                    canvas.Children.Add(origin);
                    
                }
                else 
                */

                canvas.Children.Remove(oEllipse[indexSegment]);
                oEllipse.RemoveAt(indexSegment);// a voir le cas ou il ne rest plus que deux ...
            }

            if (sym != null)
            {
                if (sym is Polygon)
                    shapepc = ((Polygon)sym).Points;
                else
                    shapepc = ((Polyline)sym).Points;
                shapepc.RemoveAt(indexSegment);
                canvas.Children.Remove(sEllipse[indexSegment]);
                sEllipse.RemoveAt(indexSegment);// a voir le cas ou il ne rest plus que deux ...
            }

        }







        //============================================================================================================================//
        //                                              drawingEllipses (private method)                                              //
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

        public void translating(Vector vec, bool isOrigin, Point mousePosition, bool isAxial, Line sline, Nullable<Point> spoint)
        {

            PointCollection shapepc;

            
            Point endPoint = new Point()
            {
                X = vec.X,
                Y = vec.Y,
            };
            Point ori = new Point(0, 0);


            Point p1 = new Point();
            Point p2 = new Point();
            if (isAxial && sline != null)
            {

                p1.X = sline.X1;
                p1.Y = sline.Y1;

                p2.X = sline.X2;
                p2.Y = sline.Y2;

                endPoint = symPoint(p1, p2, endPoint);
                ori = symPoint(p1, p2, ori);

            }

            else if (!isAxial && spoint != null)
            {
                endPoint = cSymPoint(spoint, endPoint);
                ori = cSymPoint(spoint, ori);

            }

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
            Point revMousePosition;
            if (isAxial)
                revMousePosition = symPoint(p1, p2, mousePosition); // ou ma na3ref
            else
                revMousePosition = cSymPoint(spoint, mousePosition);
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

        //=================================================================================================================//
        //                                                           Rotating                                              //
        //=================================================================================================================//
        public void rotating(Point center, double angle, bool isOrigin, bool isAxial, Line sline, Nullable<Point> spoint)
        {

            PointCollection shapepc;

            RotateTransform rt = new RotateTransform()
            {
                CenterX = center.X,
                CenterY = center.Y,
                Angle = (angle / Math.PI) * 180,
            };


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

            if (trans != null)
            {
                if (trans is Polygon)
                    shapepc = ((Polygon)trans).Points;
                else
                    shapepc = ((Polyline)trans).Points;

                for (int i = 0; i < shapepc.Count; i++)
                    shapepc[i] = rt.Transform(shapepc[i]);

                drawEllipses(trans == origin);
            }

            if (isAxial && (revTrans != null))
            {
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
                rt.Angle *= -1;
                center = symPoint(p1, p2, center);

            }
            else
                center = cSymPoint(spoint, center);

            rt.CenterX = center.X;
            rt.CenterY = center.Y;

            if (revTrans != null)
            {
                if (revTrans is Polygon)
                    shapepc = ((Polygon)revTrans).Points;
                else
                    shapepc = ((Polyline)revTrans).Points;

                for (int i = 0; i < shapepc.Count; i++)
                    shapepc[i] = rt.Transform(shapepc[i]);

                drawEllipses((revTrans == origin));

            }
        }


        //============================================================================================================================//
        //                                                          A_SYM_GEN de l'axiale                                             //
        //============================================================================================================================//



        public void aSymGen(MouseEventHandler shapeMouseEnter, MouseEventHandler shapeMouseLeave, Line sline)
        {
            //MAYBE I SHOULD ADD HERE A CONDITION sline != null ...
            PointCollection shapepc;
            PointCollection symmetricpc = new PointCollection(); //symmetric point collection

            Point p1 = new Point(sline.X1, sline.Y1);
            Point p2 = new Point(sline.X2, sline.Y2);

            Shape old;
            Shape newer;
            old = origin;
            if (sym == null)
            {  //donc orgin != null c'est lui le old



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
                sym = newer;
                drawEllipses(false);
            }

        }



        //============================================================================================================================//
        //                                                          C_SYM_GEN de centrale                                               //
        //============================================================================================================================//

        public void cSymGen(MouseEventHandler shapeMouseEnter, MouseEventHandler shapeMouseLeave, Nullable<Point> spoint)
        {
            //Maybe i should add here a condition spoint != null
            PointCollection shapepc;
            PointCollection symmetricpc = new PointCollection(); //symmetric point collection

            Shape old;
            Shape newer;

  
            old = origin;
            if (sym == null)
            {
                //On recupere la PointCollection de old et on instantie newer selon la nature de old
                if (old is Polygon)
                {
                    shapepc = ((Polygon)old).Points;
                    newer = new Polygon()
                    {
                        Stroke = old.Stroke,
                        StrokeThickness = old.StrokeThickness,
                        Fill = old.Fill,
                    };

                    ((Polygon)newer).Points = symmetricpc;
                }
                else
                {
                    shapepc = ((Polyline)old).Points;
                    newer = new Polyline()
                    {
                        Stroke = old.Stroke,
                        StrokeThickness = old.StrokeThickness,

                    };

                    ((Polyline)newer).Points = symmetricpc;

                }

                for (int i = 0; i < shapepc.Count; i++)
                    symmetricpc.Add(cSymPoint(spoint, shapepc[i]));

                newer.MouseEnter += shapeMouseEnter;
                newer.MouseLeave += shapeMouseLeave;


                canvas.Children.Add(newer);
                sym = newer;
                drawEllipses(false);
            }


        }

        //=================================================================================================================//
        //                                                           cSymPoint                                             //
        //=================================================================================================================//



        private Point cSymPoint(Nullable<Point> sp, Point p)
        {
            if (sp != null)
                return new Point(2 * sp.Value.X - p.X, 2 * sp.Value.Y - p.Y);
            else return p; 
        }




        //=================================================================================================================//
        //                                                           aSymPoint                                             //
        //=================================================================================================================//

        private Point symPoint(Point p1, Point p2, Point p3)
        {
            double k = ((p2.Y - p1.Y) * (p3.X - p1.X) - (p2.X - p1.X) * (p3.Y - p1.Y)) / (Math.Pow(p2.X - p1.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
            return new Point(2 * (p3.X - k * (p2.Y - p1.Y)) - p3.X, 2 * (p3.Y + k * (p2.X - p1.X)) - p3.Y);
        }
        //=================================================================================================================//
        //                                                      distances                                                  //
        //=================================================================================================================//
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


    }//END OF CLASS

}//END OF NAMESPACE

