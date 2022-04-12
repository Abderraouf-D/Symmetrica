﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using static Projet2Cp.Utili;

namespace Projet2Cp
{


    public partial class ModeLibre : Page
    {



        private List<ShapePair> shapePairs; //represents the pairs of shapes
        private Point mousePosition; //mouse position in canvas
        private Point actualPoint;//actual point to draw in 
        private PointCollection drawingPoints;
        private double step=25; // the step of the grid 
        private PointCollection shapepc, shapeSym; // the current shape being drawn  , and a holder for his symetrix 

        private ShapePair currentShapePair;
        private Polygon polygone;
        private Polyline polyline;
        private Ellipse ellipse;// to represent a point in the canvas
        private Line line;

        //----------------------------------------------Rotate_Utils-------------------------------------//

        private Line rotateLine = new Line()
        {
            Stroke = Brushes.Black,
            StrokeThickness = 2,
        };



        //---------------------DELETE(GOMME)_UTILS----------------------------------------------//
        bool deleteSegment;
        int segmentToDelete;
        private Shape shapeToDelete;
        private Line deleteLine = new Line()
        {
            Stroke = Brushes.BlueViolet,
            StrokeThickness = 8,
            StrokeEndLineCap = PenLineCap.Round,
            StrokeStartLineCap = PenLineCap.Round,
        };
        //---------------------------------------------------------------------------------------//

        
        private Brush oldShapeFill;//stocks the old shape fill of the shape (used when previewing the delete
        private bool isGen = false; 
        private bool isDrawing = true;
        private bool isTransforming = false;
        private bool isRotating = false;
        private bool isOverShape = false;
        private bool isOrigin = true;
        private bool isAxial = true; // IMPORTANT : will be set to true or false depending on the symmetry type selected ....
        private bool firstTimeRotating = true; //je pense qu'on peut s'en debarasser ....
        private bool isGomme = false; //quelque retouche a faire sur les cas speciaux de la gomme (when two /three are left ...)

        Point clickPosition = new Point(); //always indicates where the mouse "left-clicked" on the canvas



        ///Ta3 younes melfou9




        public static Line axeSym { get; set; } // l'axe de symetrie selectionné
        public Nullable<Point> centresym = null ;
        public static Ellipse centreSym { get; set; } // le centre de symetrie 

        List<dessinExo> dessinsModeExo;
        public static Brush trace;
        public static Brush rempli;

        public ModeLibre()
        {
            InitializeComponent();

            

            //------------------Rotate_Utils----------------------//
            DoubleCollection dc = new DoubleCollection(2);
            dc.Add(3);
            dc.Add(3);
            rotateLine.StrokeDashArray = dc;
            //----------------------------------------------------//

            ///// tae younes mefou9
            toolBar.addPolygon.Click += addPolygon;
            toolBar.horiz.Click += updateAxe;
            toolBar.verti.Click += updateAxe;
            toolBar.diag1.Click += updateAxe;
            toolBar.diag2.Click += updateAxe;
            toolBar.centre.Click += updateAxe;
            toolBar.effacerTout.Click += effacerTout;
            toolBar.genSym.Click += GenSym_Click;
            toolBar.delShape.Click += delete_Click;
            toolBar.deplacer.Click += deplacer_Click;
            toolBar.rotate.Click += rotate_Click;



            canvas.MouseLeave += ellipseCleaner;
            canvas.MouseMove += currentMousePosition;
            canvas.MouseMove += translating;
            canvas.MouseMove += mouseCursor;
            canvas.MouseMove += rotating;
            deleteLine.MouseLeave += dlCleaner;
            Canvas.SetZIndex(deleteLine, 2);



            initCanvas();

            //initialiser le centre de symetrie 
            centreSym = new Ellipse()
            {
                Height = 10,
                Width = 10,
                Fill = Brushes.Red,
                Stroke = trace,
            };



           




        }




        private void GenSym_Click(object sender, RoutedEventArgs e)
        {
            isDrawing = false;
            isTransforming = false;
            isRotating = false;
            isGomme = false;
            isGen = true;

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            isDrawing = false;
            isTransforming = false;
            isRotating = false;
            isGomme = true;
            isGen = false;

        }

        private void deplacer_Click(object sender, RoutedEventArgs e)
        {
            isDrawing = true;
            isTransforming = false;
            isRotating = false;
            isGomme = false;
            isGen = false;

        }
        private void rotate_Click(object sender, RoutedEventArgs e)
        {
            isDrawing = false;
            isTransforming = false;
            isRotating = true;
            isGomme = false;
            isGen = false;

        }


        //=======================================================================================================//
        // CurrentMousePosition : continuously updates "mousePosition" in canvas while not clicking on anything  //
        //=======================================================================================================//

        void currentMousePosition(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                mousePosition = e.GetPosition(canvas);
        }

        void dlCleaner(object sender, MouseEventArgs e)
        {
            canvas.Children.Remove(deleteLine);
        }

        //=======================================================================================================//
        //                                          INIT_CANVAS
        //=======================================================================================================//

        private void initCanvas()
        {
            shapePairs = new List<ShapePair>();
            currentShapePair = null;
            shapepc = new PointCollection();

            polyline = new Polyline()
            {
                Stroke = trace,
                StrokeThickness = 1,
                Points = shapepc,
            };

            canvas.Children.Add(polyline);

            line = new Line()
            {
                Stroke = trace,
                StrokeThickness = 1,
            };

            canvas.Children.Add(line);
            uncheckAxes();

        }



        //====================================================================================================================//
        //  TRANSLATING: if we are drawing + leftButton pressed + we are on a shape, we translate the shape or its corner     //  
        //====================================================================================================================//



        private void translating(Object sender, MouseEventArgs e)
        {
            if (isTransforming && isDrawing && (e.LeftButton == MouseButtonState.Pressed))
            {
                Vector vec = e.GetPosition(canvas) - mousePosition;
                if (currentShapePair != null)
                    currentShapePair.translating(vec, isOrigin, mousePosition, isAxial, axeSym,centresym);
                mousePosition = e.GetPosition(canvas); //on reactulise clickPosition pour le prochain move
            }


        }

        //====================================================================================================================//
        //                          ELLIPSE_CLEANER : ENLEVE LA PREVIEW ELLIPSE UNE FOIS QU'ON QUITTE LE CANVAS               //  
        //====================================================================================================================//


        private void ellipseCleaner(Object sender, MouseEventArgs e)
        {
            canvas.Children.Remove(ellipse);
        }





        //====================================================================================================================//
        //                          CANVAS_MLBD : COORDINE LE TOUS !! (DETERMINE CE QU'IL FAUT FAIRE AU CLICK)                //  
        //====================================================================================================================//

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //On recupere la position du click a toute fin utiles ...
            clickPosition = e.GetPosition(canvas);

            //soit il click sur le vide, et a ce moment la il se mettra a dessiner dans le seul cas ou isDrawing est a vrai
            if (!isOverShape && isDrawing)
            {
                line.Stroke = trace;
                polyline.Stroke = trace;
                polyline.Points.Add(actualPoint);
            }

            //sinon c'est qu'il a clické sur un shape, et dans ce cas il faudra juste determiner de quel ShapePair il s'agit
            else if (e.Source is Shape)//nrmlm si il est pas dans la premiere c'est qu'il est dans la deuxieme mais bon ....
            {
                Shape source = (Shape)(e.Source);               //si le click s'est fait sur la deleteLine
                if (e.Source == deleteLine)
                {
                    source = shapeToDelete;
                    deleteSegment = true;
                }

                //on met a true isTranslating, meme si c'est pas forcement le cas ...
                isTransforming = true;

                //on prepare le terrain pour rotating Si il y a rotating ...

                if (isRotating)
                {
                    firstTimeRotating = true;
                    rotateLine.X1 = clickPosition.X;
                    rotateLine.Y1 = clickPosition.Y;
                    rotateLine.X2 = clickPosition.X;
                    rotateLine.Y2 = clickPosition.Y;
                    canvas.Children.Add(rotateLine);
                }

                bool stop = false;
                int i;

                for (i = 0; i < shapePairs.Count && !stop; i++)
                {
                    stop = (shapePairs[i].origin == source);
                    for (int j = 0; j < shapePairs[i].oEllipse.Count && !stop; j++)
                        stop = (shapePairs[i].oEllipse[j] == source);
                    if (stop) isOrigin = true;
                    else
                    {
                        stop = (shapePairs[i].sym == source);
                        for (int j = 0; j < shapePairs[i].sEllipse.Count && !stop; j++)
                            stop = (shapePairs[i].sEllipse[j] == source);
                        if (stop) isOrigin = false; //hiya normalement bla ma endir if mais ma3lih ....
                    }
                }

                if (stop) //hiya nrmlm bla ma nverifyi ...
                {
                    i--;
                    currentShapePair = shapePairs[i];
                }

                if (isGomme)
                {
                    if (!deleteSegment)
                    {
                        canvas.Children.Remove(currentShapePair.origin);
                        foreach (Ellipse el in currentShapePair.oEllipse)
                        {
                            canvas.Children.Remove(el);
                        }
                        canvas.Children.Remove(currentShapePair.sym);
                        foreach (Ellipse el in currentShapePair.sEllipse)
                        {
                            canvas.Children.Remove(el);
                        }
                        shapePairs.Remove(currentShapePair);


                    }
                    else
                    {
                        canvas.Children.Remove(deleteLine);
                        currentShapePair.RemoveSegment(segmentToDelete);
                        deleteSegment = false;
                    }


                }


            }

        }
        

        //====================================================================================================================//
        //                          CANVAS_MLBU : COORDINE LE TOUS !! (DETERMINE CE QU'IL FAUT FAIRE AU DE_CLICK)             //  
        //====================================================================================================================//

        private void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isTransforming = false;
            
            //we clean rotating mess ...
            canvas.Children.Remove(rotateLine); //we remove the last line from the canvas 
            canvas.Children.Remove(line);

            line = new Line()
            {
                Stroke = trace,
                StrokeThickness = 1,
            };

            canvas.Children.Add(line); //we add the new empty line...GarbageCollector use 100 !
        }

        //====================================================================================================================//
        //                CANVAS_MOUSE_MOVE : DESSINE LA PREVIEW ELLIPSE SI : DRAWING + NOT_OVER_SHAPE + NOT_TRANSOFRMING     //  
        //====================================================================================================================//



        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && !isOverShape && !isTransforming)
            {
                canvas.Children.Remove(ellipse);
                mousePosition = e.GetPosition(canvas);

                actualPoint.X = Math.Round(mousePosition.X / step) * step;
                actualPoint.Y = Math.Round(mousePosition.Y / step) * step;

                if (Math.Abs(actualPoint.X - mousePosition.X) < 12.5 && Math.Abs(actualPoint.Y - mousePosition.Y) < 12.5 && canvas.IsMouseOver)
                {// if the cursor is close enough to an intersection then we draw a point 
                    ellipse = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Black,
                    };

                    ellipse.MouseLeftButtonDown += canvas_MouseLeftButtonDown;
                    ellipse.MouseRightButtonDown += canvas_MouseRightButtonDown;// C'EST PROVISOIR !!
                    canvas.Children.Add(ellipse);
                    Canvas.SetLeft(ellipse, actualPoint.X - 5);
                    Canvas.SetTop(ellipse, actualPoint.Y - 5);
                }

                //---------------------------------------------------------------------------------------//

                if (polyline.Points.Count > 0 && canvas.IsMouseOver) // ie il a engagé un dessin
                {

                    line.X1 = polyline.Points[polyline.Points.Count - 1].X;
                    line.Y1 = polyline.Points[polyline.Points.Count - 1].Y;

                    line.X2 = actualPoint.X;
                    line.Y2 = actualPoint.Y;

                }
            }



            else if (isGomme && ((e.Source is Polygon) || (e.Source is Polyline)))
            {
                shapeToDelete = (Shape)(e.Source);

                if (!canvas.Children.Contains(deleteLine))
                    canvas.Children.Add(deleteLine);

                PointCollection shapepc;
                if (e.Source is Polygon)
                    shapepc = ((Polygon)(e.Source)).Points;
                else
                    shapepc = ((Polyline)(e.Source)).Points;

                bool stop = false;
                int i;
                Point p = new Point();
                double radius;
                double actualRadius;
                for (i = 0; (i < (shapepc.Count)) && !stop; i++)
                {
                    radius = 0.5 * distancePointPoint(shapepc[i], shapepc[(i + 1) % (shapepc.Count)]);

                    p.X = 0.5 * (shapepc[i].X + shapepc[(i + 1) % (shapepc.Count)].X);
                    p.Y = 0.5 * (shapepc[i].Y + shapepc[(i + 1) % (shapepc.Count)].Y);

                    actualRadius = distancePointPoint(p, e.GetPosition(canvas));

                    stop = (distancePointLine(shapepc[i], shapepc[(i + 1) % (shapepc.Count)], e.GetPosition(canvas)) < 4 && (actualRadius <= radius));
                }

                if (stop)
                {
                    if (oldShapeFill != null)
                        ((Shape)(e.Source)).Fill = oldShapeFill;
                    i--;
                    //On stock le segmentToDelete si jamais il y a click gauche
                    segmentToDelete = i;
                    deleteLine.X1 = shapepc[i].X;
                    deleteLine.Y1 = shapepc[i].Y;
                    deleteLine.X2 = shapepc[(i + 1) % (shapepc.Count)].X;
                    deleteLine.Y2 = shapepc[(i + 1) % (shapepc.Count)].Y;

                }
                else
                {
                    if (!(e.Source is Polyline))
                    {

                        canvas.Children.Remove(deleteLine);
                        ((Shape)(e.Source)).Fill = Brushes.BlueViolet;
                    }
                }

            }

        }

        //====================================================================================================================//
        // SHAPE_MOUSE_ENTER : NETTOIE LA PREVIEW ELLIPSE ET LA BLOCK (AVEC IS_OVER_SHAPE = TRUE) DES QUE SHAPE_ENTER         //  
        //====================================================================================================================//

        public void shapeMouseEnter(object sender, MouseEventArgs e) //maybe its bad practice ....
        {
            if (isOverShape == false)
            {
                isOverShape = true;
                canvas.Children.Remove(ellipse);
            }

            oldShapeFill = ((Shape)(e.Source)).Fill;
        }

        //====================================================================================================================//
        //                  SHAPE_MOUSE_LEAVE: DEBLOCK PREVIEW ELLIPSE (AVEC IS_OVER_SHAPE = FALSE)                              //
        //====================================================================================================================//
        public void shapeMouseLeave(object sender, MouseEventArgs e)
        {
            isOverShape = false;
            if (oldShapeFill != null)
                ((Shape)(e.Source)).Fill = oldShapeFill;

        }

        private void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            // creating the shape 
            if (shapepc.Count > 1) //if the PointCollection of the polyline contains at least two points
            {
                if (Point.Equals(polyline.Points[0], polyline.Points[polyline.Points.Count - 1])) //if the shape is closed
                {
                    shapepc.RemoveAt(shapepc.Count - 1);
                    polygone = new Polygon()
                    {
                        Points = shapepc,
                        Fill = rempli,
                        Stroke = trace,
                        StrokeThickness = 3,
                    };

                    polygone.MouseEnter += shapeMouseEnter;
                    polygone.MouseLeave += shapeMouseLeave;
                    canvas.Children.Remove(polyline); //we remove the preview polyline
                    shapePairs.Add(new ShapePair(polygone, canvas, shapeMouseEnter, shapeMouseLeave)); //we create a new shapePair with origin = polygon "linked to canvas"
                }

                else
                {
                    //we keep the polyline
                    polyline.StrokeThickness = 8;
                    polyline.MouseMove += shapeMouseEnter;
                    polyline.MouseLeave += shapeMouseLeave;
                    shapePairs.Add(new ShapePair(polyline, canvas, shapeMouseEnter, shapeMouseLeave));
                }
            }

            

        cleaningTheMess();

        }


        private void cleaningTheMess()
        {
            //We clean the mess

            shapepc = new PointCollection();

            polyline = new Polyline()
            {
                Stroke = trace,
                StrokeThickness = 1,
                Points = shapepc,
            };

            canvas.Children.Add(polyline);


            canvas.Children.Remove(line); //we remove the last line from the canvas

            line = new Line()
            {
                Stroke = trace,
                StrokeThickness = 1,
            };

            canvas.Children.Add(line); //we add the new empty line...GarbageCollector use 100 ! XDDD


        }


        //==================================================================================================================//
        //       MOUSE_CURSOR: UPDATES THE CURSOR STYLE (AT EACH MOUSE_MOVE) DEPENDING ON THE SITUATION)                     //
        //==================================================================================================================//
        private void mouseCursor(Object sender, MouseEventArgs e)
        {
            if (isRotating)
            {
                this.Cursor = Cursors.Cross;
            }
            if (isDrawing)
            {
                if (isOverShape)
                    this.Cursor = Cursors.Hand;
                else
                    this.Cursor = Cursors.Pen;
            }
            if (isGomme)
            {
                this.Cursor = Cursors.Arrow;
            }
        }



        //==================================================================================================================//
        //       ROTATING: rotation de l'object dont lequel on veut rotate                                     //
        //==================================================================================================================//
        private void rotating(Object sender, MouseEventArgs e)
        {
            if (isTransforming && isRotating && (e.LeftButton == MouseButtonState.Pressed))
            {
                rotateLine.X2 = e.GetPosition(canvas).X;
                rotateLine.Y2 = e.GetPosition(canvas).Y;

                Vector newVec = e.GetPosition(canvas) - clickPosition;
                Vector oldVec = mousePosition - clickPosition;

                double angle;

                if (newVec.Length == 0 || oldVec.Length == 0)
                    angle = 0;
                else
                    angle = -Math.Atan2((newVec.X * oldVec.Y) - (newVec.Y * oldVec.X), (newVec.X * oldVec.X) + (newVec.Y * oldVec.Y));


                mousePosition = e.GetPosition(canvas);

                currentShapePair.rotating(clickPosition, angle, isOrigin, isAxial, axeSym,centresym);


            }
        }




        private void effacerTout(object sender, RoutedEventArgs e)
        {
            clear();
            
        }
        private void clear()
        {
            canvas.Children.Clear();
            initCanvas();
        }

        private void uncheckAxes()
        {
            toolBar.horiz.IsChecked = toolBar.verti.IsChecked = toolBar.diag1.IsChecked = toolBar.diag2.IsChecked = toolBar.centre.IsChecked = false;

            canvas.Children.Remove(axeSym);
            axeSym = null;

        }




        private void updateAxe(Object sender, RoutedEventArgs e)
        {
            checkAxes();
        }
        private void checkAxes()
        {
            canvas.Children.Remove(centreSym);
            centresym = null;


            for (int i = 0; i < shapePairs.Count; i++)
            {
                if (shapePairs[i].sym != null) canvas.Children.Remove((Shape)shapePairs[i].sym);
                shapePairs[i].sym = null;
            }
            if (axeSym == null)
            {
                axeSym = new Line();
                axeSym.StrokeThickness = 1;
                axeSym.Stroke = trace;
                canvas.Children.Add(axeSym);
            }
            axeSym.Stroke = trace;
            switch (true)
            {
                case true when (bool)toolBar.horiz.IsChecked :
                    {
                        axeSym.X1 = canvas.ActualWidth / 2;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = canvas.ActualWidth / 2;
                        axeSym.Y2 = canvas.ActualHeight;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;


                        break;

                    }
                case true when (bool)toolBar.verti.IsChecked :
                    {
                        axeSym.X1 = 0.5;
                        axeSym.Y1 = canvas.ActualHeight / 2;
                        axeSym.X2 = canvas.ActualWidth;
                        axeSym.Y2 = canvas.ActualHeight / 2;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;


                        break;
                    }
                case true when (bool)toolBar.diag1.IsChecked :
                    {
                        axeSym.X1 = 0.5;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = canvas.ActualWidth;
                        axeSym.Y2 = canvas.ActualHeight;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;


                        break;
                    }
                case true when (bool)toolBar.diag2.IsChecked :
                    {
                        axeSym.X1 = canvas.ActualWidth;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = 0.5;
                        axeSym.Y2 = canvas.ActualHeight; ;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;

                        break;
                    }
                case true when (bool)toolBar.centre.IsChecked :
                    {
                        centresym = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);
                        canvas.Children.Remove(axeSym);
                        if (!(canvas.Children.Contains(centreSym)))
                        {
                            canvas.Children.Add(centreSym);
                            Canvas.SetLeft(centreSym, canvas.ActualWidth / 2 - 5);
                            Canvas.SetTop(centreSym, canvas.ActualHeight / 2 - 5);
                        }
                        isAxial = false;


                        break;
                    }

                default:
                    {

                        break;
                    }



            }
        }


        private void addPolygon(Object sender, RoutedEventArgs e)
        {

            Point centrePoly = new Point();
            int cote, rayon;
           
            cote = toolBar.NbCote;
            rayon = toolBar.Rayon;
            
            centrePoly.X = (double)canvas.ActualWidth / (double)2;
            centrePoly.Y = (double)canvas.ActualHeight / (double)2;

            double angle = 360 / cote;
            Polygon poly = new Polygon()
            {
                Fill = rempli,
                Stroke = trace,

            };
            Point fstPt = new Point(centrePoly.X + rayon * step, centrePoly.Y);
            poly.Points.Add(fstPt);
            RotateTransform trans;

            for (int i = 1; i < cote; i++)
            {
                trans = new RotateTransform()
                {
                    CenterX = centrePoly.X,
                    CenterY = centrePoly.Y,
                    Angle = -angle * i,

                };

                poly.Points.Add(trans.Transform(fstPt));

            }

            poly.MouseEnter += shapeMouseEnter;
            poly.MouseLeave += shapeMouseLeave;
            shapePairs.Add(new ShapePair(poly, canvas,  shapeMouseEnter, shapeMouseLeave));


        }

    }
}
