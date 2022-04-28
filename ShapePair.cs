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
        public List<TextBlock> otb = new List<TextBlock>();
        public List<Line> jointLines { get; set; }
        public Shape sym { get; set; }
        public List<Ellipse> sEllipse = new List<Ellipse>();
        public List<TextBlock> stb = new List<TextBlock>();

        MouseEventHandler shapeMouseEnter;
        MouseEventHandler shapeMouseLeave;
        MouseButtonEventHandler edit;

        public bool IsTransformable { get; set; }
        public bool removable { get; set; }
        
        //============================================================================================================================//
        //                                              TESTING CONSTRUCOR                                                            //
        //============================================================================================================================//

        public ShapePair(Shape origin, Canvas canvas, MouseEventHandler shapeMouseEnter, MouseEventHandler shapeMouseLeave, MouseButtonEventHandler edit)
        {
            IsTransformable = true;
            removable = false;
            jointLines = new List<Line>();

            this.canvas = canvas;
            this.origin = origin;
            if (origin is Polygon)
                canvas.Children.Add(origin);
            else if (((Polyline)origin).Points.Count == 2)
                removable = true;
            this.shapeMouseEnter = shapeMouseEnter;
            this.shapeMouseLeave = shapeMouseLeave;
            this.edit = edit;

            drawEllipses(true);

        }

        //============================================================================================================================//
        //                                                        AdaptToGrid                                                         //
        //============================================================================================================================//
        public void adaptToGrid(Point oldeCenter, Point newCenter)
        {
            PointCollection shapepc;
            if (origin == null)
                return;
            if (origin is Polygon)
                shapepc = ((Polygon)origin).Points;
            else
                shapepc = ((Polyline)origin).Points;
            Vector opo = newCenter - oldeCenter;

            for (int i = 0; i < shapepc.Count; i++)
                shapepc[i] += opo;
            drawEllipses(true);
            if (sym == null)
                return;
            if (sym is Polygon)
                shapepc = ((Polygon)sym).Points;
            else
                shapepc = ((Polyline)sym).Points;
            for (int i = 0; i < shapepc.Count; i++)
                shapepc[i] += opo;
            drawEllipses(false);
            jointLinesGen();
        }
        //============================================================================================================================//
        //                                                  JOINT_LINES_GEN                                                           //
        //============================================================================================================================//
        public void jointLinesGen()
        {
            Line line;
            PointCollection opc;
            PointCollection spc;

            if (sym != null)
            {
                foreach (Line l in jointLines)
                    canvas.Children.Remove(l);
                if (origin is Polygon)
                {
                    opc = ((Polygon)origin).Points;
                    spc = ((Polygon)sym).Points;
                }
                else
                {
                    opc = ((Polyline)origin).Points;
                    spc = ((Polyline)sym).Points;
                }
                for (int i = 0; i < opc.Count; i++)
                {
                    line = new Line()
                    {
                        Stroke = Brushes.BlueViolet,
                        StrokeThickness = 1,
                        X1 = opc[i].X,
                        Y1 = opc[i].Y,
                        X2 = spc[i].X,
                        Y2 = spc[i].Y,
                    };
                    jointLines.Add(line);
                    canvas.Children.Add(line);
                }
            }
        }

        
                                                            //(HAS BEEN MODIFIED)//
        //============================================================================================================================//
        //                                                      RemoveSegment                                                         //
        //============================================================================================================================//
        public void RemoveSegment(int indexSegment)
        {
            PointCollection shapepc;
            Polyline poly;
            if (origin != null)
            {
                if (origin is Polygon)
                    shapepc = ((Polygon)origin).Points;
                else
                    shapepc = ((Polyline)origin).Points;

                if (shapepc.Count > 3 && (origin is Polygon) || (origin is Polyline))
                {
                    if (origin is Polyline)
                        if (((Polyline)origin).Points.Count - 2 == indexSegment)
                            indexSegment++;
                    
                    shapepc.RemoveAt(indexSegment);
                    
                    canvas.Children.Remove(oEllipse[indexSegment]);
                    oEllipse.RemoveAt(indexSegment);
                    
                    canvas.Children.Remove(otb[indexSegment]);
                    otb.RemoveAt(indexSegment);
                    
                    if (shapepc.Count == 2)
                        removable = true;
                }

                else if (origin is Polygon)
                {
                    canvas.Children.Remove(origin);
                    poly = new Polyline();
                    poly.MouseEnter += shapeMouseEnter;
                    poly.MouseLeave += shapeMouseLeave;
                    for (int i = 1; i <= 3; i++)
                        poly.Points.Add(shapepc[(indexSegment + i) % 3]);
                    poly.Stroke = Brushes.Black;
                    poly.StrokeThickness = 8;
                    Canvas.SetZIndex(poly, -1);
                    origin = poly;
                    drawEllipses(true);
                    canvas.Children.Add(origin);
                }


            }

            if (sym != null)
            {
                if (sym is Polygon)
                    shapepc = ((Polygon)sym).Points;
                else
                    shapepc = ((Polyline)sym).Points;

                if (shapepc.Count > 3 && (sym is Polygon) || (sym is Polyline))
                {
                    shapepc.RemoveAt(indexSegment);
                    canvas.Children.Remove(sEllipse[indexSegment]);
                    sEllipse.RemoveAt(indexSegment);
                    canvas.Children.Remove(stb[indexSegment]);
                    stb.RemoveAt(indexSegment);
                }

                else if (sym is Polygon)
                {
                    canvas.Children.Remove(sym);
                    poly = new Polyline();
                    poly.MouseEnter += shapeMouseEnter;
                    poly.MouseLeave += shapeMouseLeave;
                    for (int i = 1; i <= 3; i++)
                        poly.Points.Add(shapepc[(indexSegment + i) % 3]);
                    poly.Stroke = Brushes.Black;
                    poly.StrokeThickness = 8;
                    Canvas.SetZIndex(poly, -1);
                    sym = poly;
                    drawEllipses(false);
                    canvas.Children.Add(sym);

                }



            }

            jointLinesGen();


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
                {
                    canvas.Children.Remove(oEllipse[i]);
                    canvas.Children.Remove(otb[i]);
                }

                oEllipse = new List<Ellipse>();
                otb = new List<TextBlock>();

                if (origin is Polygon)
                    shapepc = ((Polygon)origin).Points;
                else
                    shapepc = ((Polyline)origin).Points;

            }

            else
            {
                for (int i = 0; i < sEllipse.Count; i++)
                {
                    canvas.Children.Remove(sEllipse[i]);
                    canvas.Children.Remove(stb[i]);
                }

                sEllipse = new List<Ellipse>();
                stb = new List<TextBlock>();

                if (sym is Polygon)
                    shapepc = ((Polygon)sym).Points;
                else
                    shapepc = ((Polyline)sym).Points;

            }



            Ellipse ellipse;
            TextBlock tb;
            for (int i = 0; i < shapepc.Count; i++)
            {
                ellipse = new Ellipse()
                {
                    Height = 20,
                    Width = 20,
                    Fill = Brushes.White,
                    Stroke = Brushes.Black,

                };
                
                ellipse.MouseEnter += shapeMouseEnter;
                ellipse.MouseLeave += shapeMouseLeave;
                ellipse.MouseLeftButtonUp += edit;

                tb = new TextBlock()
                {
                    Height = 15,
                    Width = 15,
                    FontWeight = FontWeights.Bold,
                    Text = char.ToString(Convert.ToChar('A' + i)),
                    TextAlignment = TextAlignment.Center,
                    IsHitTestVisible = false,
                };
                

                if (isOrigin)
                {
                    oEllipse.Add(ellipse);
                    otb.Add(tb);
                }
                else
                {
                    tb.Text += "'";
                    sEllipse.Add(ellipse);
                    stb.Add(tb);
                }

                canvas.Children.Add(ellipse);
                canvas.Children.Add(tb);

                Canvas.SetLeft(ellipse, shapepc[i].X - 10);
                Canvas.SetTop(ellipse, shapepc[i].Y - 10);

                Canvas.SetLeft(tb, shapepc[i].X - 7.5);
                Canvas.SetTop(tb, shapepc[i].Y - 7.5);
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

            jointLinesGen();
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

            jointLinesGen();
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

            jointLinesGen();
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

            jointLinesGen();

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