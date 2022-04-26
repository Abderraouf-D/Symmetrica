﻿using Syncfusion.UI.Xaml.Diagram;
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
using static Projet2Cp.Utili;
namespace Projet2Cp
{

    public partial class canvasUC : UserControl
    {
        private List<ShapePair> shapePairs { get; set; } //represents the pairs of shapes
        private Point mousePosition; //mouse position in canvas
        private Point actualPoint;//actual point to draw in 
        private PointCollection drawingPoints;
        private double step = 25; // the step of the grid 
        public PointCollection shapepc; // the current shape being drawn  , and a holder for his symetrix 
        private ShapePair currentShapePair;
        public Polygon polygone;
        public Polyline polyline;
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
        private bool isGomme = false; //quelque retouche a faire sur les cas speciaux de la gomme (when two /three are left ...)
        private bool isColoring = false;
        private bool isDuplicating = false;
        private bool isEditing = false;

        Point clickPosition = new Point(); //always indicates where the mouse "left-clicked" on the canvas
        RadioButton lastRepere = null; 

        public static Line axeSym { get; set; } // l'axe de symetrie selectionné
        public Nullable<Point> centresym = null;
        public static Ellipse centreSym { get; set; } // le centre de symetrie 

        List<dessinExo> dessinsModeExo;
        public static Brush trace = Brushes.Black;
        public static Brush rempli = Brushes.White;

        RadioButton horiz, verti, diag1, diag2, centre;
        UserControl TB; 

        niveauxLibre niv;
        TextBlock tb; 
        static public int cote, rayon;
        //Cursor colorate = new Cursor(Application.GetResourceStream(new Uri("C://Users//raouf//Desktop//Projet2Cp//cursors//color.cur")).Stream);
        bool answer = false;
        public canvasUC(UserControl TB, niveauxLibre niv)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBaFt/QHFqVVhkW1pFdEBBXHxAd1p/VWJYdVt5flBPcDwsT3RfQF9jTXxXdkxhWHxWeXZQQw==;NjIzNzA1QDMyMzAyZTMxMmUzMGljYUdhRmY2OXNTZ0ZIdSsvN3NGTDMzMk1hRmtyejRMcndKTG5XOG42TUk9");

            InitializeComponent();

            dessinsModeExo = chargerDessins(@".\shapesExo.txt");
            this.TB = TB;
            if (MainWindow.modeLibre)
            {
                horiz = ((toolBarLibreLibre)TB).horiz;
                verti = ((toolBarLibreLibre)TB).verti;
                diag1 = ((toolBarLibreLibre)TB).diag1;
                diag2 = ((toolBarLibreLibre)TB).diag2;
                centre = ((toolBarLibreLibre)TB).centre;
                

            }
            else
            {
                this.niv = niv;
                if (MainWindow.modeEns)
                {
                    horiz = ((toolBarEnseignant)TB).horiz;
                    verti = ((toolBarEnseignant)TB).verti;
                    diag1 = ((toolBarEnseignant)TB).diag1;
                    diag2 = ((toolBarEnseignant)TB).diag2;
                    centre = ((toolBarEnseignant)TB).centre;
                }

            }

            //------------------Rotate_Utils----------------------//
            DoubleCollection dc = new DoubleCollection(2);
            dc.Add(3);
            dc.Add(3);
            rotateLine.StrokeDashArray = dc;

            canvas.MouseLeave += ellipseCleaner;
            canvas.MouseMove += currentMousePosition;
            canvas.MouseMove += translating;
            canvas.MouseMove += mouseCursor;
            canvas.MouseMove += rotating;
            canvas.MouseWheel += zoom;
            canvas.Loaded += load;
            canvas.SizeChanged += adaptGrid;
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

        //==========================================================================================================//
        //                                            adaptGrid                                                     //
        //==========================================================================================================//

        void adaptGrid(object sender, SizeChangedEventArgs e)
        {
            Point oldCenter = new Point(e.PreviousSize.Width * 0.5, e.PreviousSize.Height * 0.5);
            Point newCenter = new Point(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5);
            gridDrawing(step);
            bool sym = false; 
            for (int i = 0; i < shapePairs.Count; i++)
            {
                sym = !(shapePairs[i].sym == null);
                shapePairs[i].adaptToGrid(oldCenter, newCenter);
                if (sym)
                {
                    if (isAxial) shapePairs[i].aSymGen(shapeMouseEnter, shapeMouseLeave, axeSym);
                    else shapePairs[i].cSymGen(shapeMouseEnter, shapeMouseLeave, centresym);
                   if (!MainWindow.modeLibre && !answer ) hideSym(shapePairs[i]);

                    
                }  
            }
        }
        //==========================================================================================================//
        //                                            load                                                          //
        //==========================================================================================================//
        void load(object sender, RoutedEventArgs e)
        {
            gridDrawing(step);
            if (!MainWindow.modeLibre) dessinerDessinNum(niv.Selected());
        }
        //==========================================================================================================//
        //                                           OLD KINDS OF CLICKS                                            //
        //==========================================================================================================//

        public void dupliquer(object sender, RoutedEventArgs e)
        {
            isDrawing = false;
            isTransforming = false;
            isRotating = false;
            isGomme = false;
            isGen = false;
            isColoring = false;
            isDuplicating = true;
        }
        public void colorier_Click(object sender, RoutedEventArgs e)
        {
            isDrawing = false;
            isTransforming = false;
            isRotating = false;
            isGomme = false;
            isDuplicating = false;
            isGen = false;
            isColoring = true;
        }
        public void GenSym_Click(object sender, RoutedEventArgs e)
        {
            isDrawing = false;
            isTransforming = false;
            isRotating = false;
            isGomme = false;
            isGen = true;
            isDuplicating = false;
            isColoring = false;

        }
        public void delete_Click(object sender, RoutedEventArgs e)
        {
            isDrawing = false;
            isTransforming = false;
            isRotating = false;
            isGomme = true;
            isDuplicating = false;
            isGen = false;
            isColoring = false;

        }
        public void deplacer_Click(object sender, RoutedEventArgs e)
        {
            isDrawing = true;
            isTransforming = false;
            isRotating = false;
            isGomme = false;
            isDuplicating = false;
            isGen = false;
            isColoring = false;

        }
        public void rotate_Click(object sender, RoutedEventArgs e)
        {
            isDrawing = false;
            isDuplicating = false;
            isTransforming = false;
            isRotating = true;
            isGomme = false;
            isGen = false;
            isColoring = false;

        }
        //=======================================================================================================//
        //                                      ZOOM                                                             //
        //=======================================================================================================//
        void zoom(object sender, MouseWheelEventArgs e)
        {
            canvas.Children.Remove(ellipse);

            double alpha;
            if (e.Delta < 0)
                alpha = 0.8;
            else
                alpha = 1.25;

            if (step * alpha > 50 || step * alpha < 10)
                return;
            step *= alpha;
            gridDrawing(step);

            for (int i = 0; i < shapePairs.Count; i++)
            {
                PointCollection pc; //originPointCollection
                if (shapePairs[i].origin is Polygon)
                    pc = ((Polygon)shapePairs[i].origin).Points;
                else
                    pc = ((Polyline)shapePairs[i].origin).Points;

                Point paz;
                Point center = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);

                for (int j = 0; j < pc.Count; j++)
                {
                    paz = new Point();
                    paz.X = (pc[j].X - center.X) * alpha + center.X;
                    paz.Y = (pc[j].Y - center.Y) * alpha + center.Y;
                    pc[j] = paz;
                }
                for (int j = 0; j < shapePairs[i].oEllipse.Count; j++)
                {
                    Canvas.SetLeft(shapePairs[i].oEllipse[j], pc[j].X - 5);
                    Canvas.SetTop(shapePairs[i].oEllipse[j], pc[j].Y - 5);
                }

                if (shapePairs[i].sym != null)
                {
                    if (shapePairs[i].sym is Polygon)
                        pc = ((Polygon)shapePairs[i].sym).Points;
                    else
                        pc = ((Polyline)shapePairs[i].sym).Points;

                    for (int j = 0; j < pc.Count; j++)
                    {
                        paz = new Point();
                        paz.X = (pc[j].X - center.X) * alpha + center.X;
                        paz.Y = (pc[j].Y - center.Y) * alpha + center.Y;
                        pc[j] = paz;
                    }
                    for (int j = 0; j < shapePairs[i].sEllipse.Count; j++)
                    {
                        Canvas.SetLeft(shapePairs[i].sEllipse[j], pc[j].X - 5);
                        Canvas.SetTop(shapePairs[i].sEllipse[j], pc[j].Y - 5);
                    }
                }
                if (MainWindow.modeLibre) shapePairs[i].jointLinesGen();
                else
                {
                    if ( shapePairs[0].sym.Visibility == Visibility.Visible) shapePairs[0].jointLinesGen();
                }
            }
        }
        //=======================================================================================================//
        //                                      GRID_DRAWING                                                     //
        //=======================================================================================================//
        private void gridDrawing(double step)
        {
            DrawingBrush res = new DrawingBrush();
            GeometryDrawing GD = new GeometryDrawing();
            GeometryGroup GG = new GeometryGroup();

            GG.Children.Add(new LineGeometry(new Point(0, 0), new Point(0, step)));
            GG.Children.Add(new LineGeometry(new Point(0, 0), new Point(step, 0)));

            res.Viewbox = new Rect(0, 0, step, step);
            res.Viewport = new Rect(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5, step, step);


            res.TileMode = TileMode.Tile;
            res.ViewportUnits = BrushMappingMode.Absolute;
            res.ViewboxUnits = BrushMappingMode.Absolute;

            GD.Pen = new Pen()
            {
                DashStyle = new DashStyle()
                {
                    Dashes = { 5, 3 }
                },

                Brush = Brushes.Black,
                Thickness = 1,
                DashCap = PenLineCap.Flat,
            };

            GD.Geometry = GG;
            res.Drawing = GD;
            canvas.Background = res;
        }

        //=======================================================================================================//
        // CurrentMousePosition : continuously updates "mousePosition" in canvas while not clicking on anything  //
        //=======================================================================================================//

        public void currentMousePosition(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                mousePosition = e.GetPosition(canvas);
        }

        public void dlCleaner(object sender, MouseEventArgs e)
        {
            canvas.Children.Remove(deleteLine);
        }

        //=======================================================================================================//
        //                                          INIT_CANVAS
        //=======================================================================================================//


        public void initCanvas()
        {
            gridDrawing(step);
            
            
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
            if (isTransforming && isDrawing && currentShapePair.IsTransformable && (e.LeftButton == MouseButtonState.Pressed))
            {

                Point newPosition = e.GetPosition(canvas);
                newPosition.X = Math.Round((newPosition.X - canvas.ActualWidth * 0.5) / step) * step + (canvas.ActualWidth * 0.5);
                newPosition.Y = Math.Round((newPosition.Y - canvas.ActualHeight * 0.5) / step) * step + (canvas.ActualHeight * 0.5);

                mousePosition.X = Math.Round((mousePosition.X - canvas.ActualWidth * 0.5) / step) * step + (canvas.ActualWidth * 0.5);
                mousePosition.Y = Math.Round((mousePosition.Y - canvas.ActualHeight * 0.5) / step) * step + (canvas.ActualHeight * 0.5);

                Vector vec = newPosition - mousePosition;
                if (currentShapePair != null)
                    currentShapePair.translating(vec, isOrigin, mousePosition, isAxial, axeSym, centresym);
                mousePosition = newPosition; //on reactulise clickPosition pour le prochain move
            }


        }
     
        //====================================================================================================================//
        //                          ELLIPSE_CLEANER : ENLEVE LA PREVIEW ELLIPSE UNE FOIS QU'ON QUITTE LE CANVAS               //  
        //====================================================================================================================//


        public void ellipseCleaner(Object sender, MouseEventArgs e)
        {
            canvas.Children.Remove(ellipse);
        }

        //====================================================================================================================//
        //                          CANVAS_MLBD : COORDINE LE TOUS !! (DETERMINE CE QU'IL FAUT FAIRE AU CLICK)                //  
        //====================================================================================================================//

        public void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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

                    if (isRotating) //JE PENSE QU'ON PEUT DEGAGER QUELQUE TRUC ICI ....
                    {
                        
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
                if (currentShapePair.IsTransformable)
                {
                    if (isGomme)
                    {
                        if (!deleteSegment || currentShapePair.removable)
                        {
                            canvas.Children.Remove(deleteLine);
                            foreach (Line l in currentShapePair.jointLines)
                                canvas.Children.Remove(l);

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

                    if (isGen)
                    {
                        if (axeSym != null || centresym != null)
                            if (isAxial)
                            {
                                currentShapePair.aSymGen(shapeMouseEnter, shapeMouseLeave, axeSym);

                            }

                            else
                            {
                                currentShapePair.cSymGen(shapeMouseEnter, shapeMouseLeave, centresym);
                            }
                        else MessageBox.Show("Veuillez sélectionner un repère de symetrie !");

                    }

                    if (isColoring)
                    {
                        if (e.Source is Polygon)
                        {
                            ((Polygon)e.Source).Fill = rempli;
                            ((Polygon)e.Source).Stroke = trace;

                        }

                        else if (e.Source is Polyline)
                        {
                            ((Polyline)e.Source).Stroke = trace;
                        }
                    }

                    if (isDuplicating)
                    {
                        PointCollection pc = new PointCollection();
                        Polygon polygon;
                        Polyline polyline;
                        Shape shape;
                        if (isOrigin)
                            shape = currentShapePair.origin;
                        else
                            shape = currentShapePair.sym;

                        if (shape is Polygon)
                        {
                            foreach (Point p in ((Polygon)shape).Points)
                                pc.Add(new Point(p.X + 10, p.Y + 10));
                            polygon = new Polygon()
                            {
                                Stroke = ((Polygon)shape).Stroke,
                                StrokeThickness = 3,
                                Fill = ((Polygon)shape).Fill,
                                Points = pc,
                            };
                            polygon.MouseEnter += shapeMouseEnter;
                            polygon.MouseLeave += shapeMouseLeave;
                            shapePairs.Add(new ShapePair(polygon, canvas, shapeMouseEnter, shapeMouseLeave));
                        }

                        else
                        {
                            foreach (Point p in ((Polyline)shape).Points)
                                pc.Add(new Point(p.X + 10, p.Y + 10));
                            polyline = new Polyline()
                            {
                                Stroke = ((Polyline)shape).Stroke,
                                StrokeThickness = 8,
                                Points = pc,
                            };
                            canvas.Children.Add(polyline);
                            polyline.MouseEnter += shapeMouseEnter;
                            polyline.MouseLeave += shapeMouseLeave;
                            shapePairs.Add(new ShapePair(polyline, canvas, shapeMouseEnter, shapeMouseLeave));

                        }

                    }
                }

                
            }

        }//END OF FUNCTION


        //====================================================================================================================//
        //                          CANVAS_MLBU : COORDINE LE TOUS !! (DETERMINE CE QU'IL FAUT FAIRE AU DE_CLICK)             //  
        //====================================================================================================================//

        public void canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

                actualPoint.X = Math.Round((mousePosition.X - canvas.ActualWidth * 0.5) / step) * step + (canvas.ActualWidth * 0.5);
                actualPoint.Y = Math.Round((mousePosition.Y - canvas.ActualHeight * 0.5) / step) * step + (canvas.ActualHeight * 0.5);



                if (Math.Abs(actualPoint.X - mousePosition.X) < step * 0.5 && Math.Abs(actualPoint.Y - mousePosition.Y) < step * 0.5 && canvas.IsMouseOver)
                {// if the cursor is close enough to an intersection then we draw a point 
                    ellipse = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Black,
                    };

                    ellipse.MouseLeftButtonDown += canvas_MouseLeftButtonDown;
                    ellipse.MouseRightButtonDown += canvas_MouseRightButtonDown;// C'EST PROVISOIR !!
                    ellipse.MouseWheel += zoom;
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
        //                  SHAPE_MOUSE_LEAVE: DEBLOCK PREVIEW ELLIPSE (AVEC IS_OVER_SHAPE = FALSE)                           //
        //====================================================================================================================//
        public void shapeMouseLeave(object sender, MouseEventArgs e)
        {
            isOverShape = false;
            if (oldShapeFill != null && isGomme)
                ((Shape)(e.Source)).Fill = oldShapeFill;

        }

        //=====================================================================================================================//
        //                                                      MRBD                                                           //
        //=====================================================================================================================//

        public void canvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
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


        public void cleaningTheMess()
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
        public void mouseCursor(Object sender, MouseEventArgs e)
        {
            if (isRotating)
            {
                this.Cursor = Cursors.Cross;
            }
            if (isDrawing)
            {
                if (isOverShape)
                    this.Cursor = Cursors.SizeAll;
                else
                    this.Cursor = Cursors.Pen;
            }
            if (isGomme)
            {
                this.Cursor = Cursors.Arrow;
            }
            if (isGen)
            {
                this.Cursor = Cursors.Hand;
            }
            //if (isColoring) this.Cursor = colorate;
        }



        //==================================================================================================================//
        //       ROTATING: rotation de l'object dont lequel on veut rotate                                                  //
        //==================================================================================================================//
        public void rotating(Object sender, MouseEventArgs e)
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

                currentShapePair.rotating(clickPosition, angle, isOrigin, isAxial, axeSym, centresym);


            }
        }




        public void effacerTout(object sender, RoutedEventArgs e)
        {
            clear();
        }
        public void clear()
        {
            if (isEditing)
            {
                canvas.Children.Add(tb);
                Canvas.SetTop(tb, 0);
            }
            canvas.Children.Clear();
            initCanvas();
        }

        public void uncheckAxes()
        {
            horiz.IsChecked = verti.IsChecked = diag1.IsChecked = diag2.IsChecked = centre.IsChecked = false;
            canvas.Children.Remove(axeSym);
            axeSym = null;
            lastRepere = null; 
        }

        public void updateAxe(Object sender, RoutedEventArgs e)
        {
            if (lastRepere == sender) {
                lastRepere.IsChecked = false;
                canvas.Children.Remove(axeSym);
                isAxial = false;
                lastRepere = null;
            }
            else
            {
                lastRepere = (RadioButton)sender;
            }
            checkAxes();

        }
        public void checkAxes()
        {

            canvas.Children.Remove(centreSym);
            centresym = null;

            

            for (int i = 0; i < shapePairs.Count; i++)
            {
               canvas.Children.Remove(shapePairs[i].sym);
               shapePairs[i].sym = null;
                
                foreach (Ellipse el in shapePairs[i].sEllipse)
                {
                    canvas.Children.Remove(el);
                }
                shapePairs[i].sEllipse = new List<Ellipse>();
                foreach (Line el in shapePairs[i].jointLines)
                {
                    canvas.Children.Remove(el);
                }
                shapePairs[i].jointLines = new List<Line>();
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
                case true when (bool)horiz.IsChecked:
                    {
                        axeSym.X1 = canvas.ActualWidth / 2;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = canvas.ActualWidth / 2;
                        axeSym.Y2 = canvas.ActualHeight;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;


                        break;

                    }
                case true when (bool)verti.IsChecked:
                    {
                        axeSym.X1 = 0.5;
                        axeSym.Y1 = canvas.ActualHeight / 2;
                        axeSym.X2 = canvas.ActualWidth;
                        axeSym.Y2 = canvas.ActualHeight / 2;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;


                        break;
                    }
                case true when (bool)diag1.IsChecked:
                    {
                        axeSym.X1 = 0.5;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = canvas.ActualWidth;
                        axeSym.Y2 = canvas.ActualHeight;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;


                        break;
                    }
                case true when (bool)diag2.IsChecked:
                    {
                        axeSym.X1 = canvas.ActualWidth;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = 0.5;
                        axeSym.Y2 = canvas.ActualHeight; ;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;

                        break;
                    }
                case true when (bool)centre.IsChecked:
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



        public void addPolygon(Object sender, RoutedEventArgs e)
        {

            Point centrePoly = new Point();




            centrePoly.X = (double)canvas.ActualWidth / (double)2;
            centrePoly.Y = (double)canvas.ActualHeight / (double)2;

            double angle = 360 / cote;
            Polygon poly = new Polygon()
            {
                Fill = rempli,
                Stroke = trace,
                StrokeThickness = 3,

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
            shapePairs.Add(new ShapePair(poly, canvas, shapeMouseEnter, shapeMouseLeave));


        }



        //==================================================================================================//
        //                                   Methodes  du mode enseignant                                   //
        //==================================================================================================//


        public void EditEns_Click(Object sender, RoutedEventArgs e)
        {
            clear();
            isEditing = true;
            tb = new TextBlock();
            tb.Text = "Veuillez créer UNE forme, selectionner le repère de symetrie, Puis cliquer sur Confirmer.";
            tb.Foreground = Brushes.Red;
            tb.FontSize = 18;
            tb.TextAlignment = TextAlignment.Center;
            tb.Width = canvas.ActualWidth;
            tb.Height = canvas.ActualHeight;
            canvas.Children.Add(tb);
            Canvas.SetTop(tb, 0);
            


            ((toolBarEnseignant)TB).vld.Text = "Confirmer";
            ((toolBarEnseignant)TB).annuler.Visibility = Visibility.Visible;
            niv.IsEnabled = false;



        }

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            switch (true)
            {
                case true when (bool)horiz.IsChecked:
                    {
                        axeSym.X1 = canvas.ActualWidth / 2;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = canvas.ActualWidth / 2;
                        axeSym.Y2 = canvas.ActualHeight;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;


                        break;

                    }
                case true when (bool)verti.IsChecked:
                    {
                        axeSym.X1 = 0.5;
                        axeSym.Y1 = canvas.ActualHeight / 2;
                        axeSym.X2 = canvas.ActualWidth;
                        axeSym.Y2 = canvas.ActualHeight / 2;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;


                        break;
                    }
                case true when (bool)diag1.IsChecked:
                    {
                        axeSym.X1 = 0.5;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = canvas.ActualWidth;
                        axeSym.Y2 = canvas.ActualHeight;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;


                        break;
                    }
                case true when (bool)diag2.IsChecked:
                    {
                        axeSym.X1 = canvas.ActualWidth;
                        axeSym.Y1 = 0.5;
                        axeSym.X2 = 0.5;
                        axeSym.Y2 = canvas.ActualHeight; ;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;

                        break;
                    }
                case true when (bool)centre.IsChecked:
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

        public void valider_Click(Object sender, RoutedEventArgs e)
        {
            if (isEditing)
            {
                canvas.Children.Remove(tb);
                if (shapePairs.Count > 1)
                {
                    MessageBox.Show("Vous ne pouvez pas creer plusieurs  formes a la fois!");
                }
                else if (axeSym == null && centresym == null)
                {
                    MessageBox.Show("Veuillez choisir un repère de symetrie!");

                }
                else
                {
                    Shape shp = shapePairs[0].origin;
                    int ind = niv.Selected() + 1;

                    if (shp is Polygon)
                        Utili.strTofile(@".\shapesExo.txt", Utili.CanvasToString(((Polygon)shp).Points, ((Polygon)shp).Fill, ((Polygon)shp).Stroke, ((toolBarEnseignant)TB).selectedAxe(),new Point(canvas.ActualWidth*0.5 , canvas.ActualHeight*0.5)), ind);
                    else
                        Utili.strTofile(@".\shapesExo.txt", Utili.CanvasToString(((Polyline)shp).Points, null, ((Polyline)shp).Stroke, ((toolBarEnseignant)TB).selectedAxe(), new Point(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5)), ind);
                    dessinsModeExo = chargerDessins(@".\shapesExo.txt");
                    MessageBox.Show("Dessin modifié avec succès!");
                    canvas.Children.Remove(tb);
                    ((toolBarEnseignant)TB).annuler.Visibility = Visibility.Collapsed;
                    niv.IsEnabled = true;
                    ((toolBarEnseignant)TB).vld.Text = "Valider";
                    isEditing = false;



                }

            }
            else
            {
                answer = true; 
                if (shapePairs.Count==1)
                {
                    MessageBox.Show("Veuillez dessiner le symetrique");
                }
                else {

                    if (!shapePairs[0].origin.GetType().Equals(shapePairs[1].origin.GetType()))
                    {
                        shapePairs[0].sym.Stroke = Brushes.Red;
                        MessageBox.Show("Ooops");
                        showSym(shapePairs[0]);
                        shapePairs[0].jointLinesGen();
                    }
                    else
                    {
                        bool polygon = true;
                        PointCollection p1 = null, p2 = null;
                        if (shapePairs[0].origin is Polyline) { 
                            p2 = ((Polyline)shapePairs[0].sym).Points;
                            polygon = false; 
                        }
                        else p2 = ((Polygon)shapePairs[0].sym).Points;


                        
                        if (shapePairs[1].origin is Polygon) p1 = ((Polygon)shapePairs[1].origin).Points;
                        else p1 = ((Polyline)shapePairs[1].origin).Points;
                        if (isSubTable(p1, p2 ,polygon))
                        {
                            shapePairs[0].sym.Stroke = Brushes.Green;
                            MessageBox.Show("Bravo!");
                            showSym(shapePairs[0]);
                            shapePairs[0].jointLinesGen();


                            foreach (Ellipse el in shapePairs[1].oEllipse) canvas.Children.Remove(el);
                            canvas.Children.Remove(shapePairs[1].origin);
                            shapePairs.Remove(shapePairs[1]);

                        }
                        else
                        {
                            shapePairs[0].sym.Stroke = Brushes.Red;
                            MessageBox.Show("Ooops");
                            showSym(shapePairs[0]);
                            shapePairs[0].jointLinesGen();
                        }
                    }
                }



            }



        }
        public void annuler_Click(Object sender, RoutedEventArgs e)
        {
            canvas.Children.Remove(tb);
            ((toolBarEnseignant)TB).annuler.Visibility = Visibility.Collapsed;
            niv.IsEnabled = true;
            ((toolBarEnseignant)TB).vld.Text = "Valider";
            isEditing = false;
            dessinerDessinNum(niv.Selected());


        }

        public void Niv_Click(object sender, RoutedEventArgs e)
        {
            answer = false;
            dessinerDessinNum(niv.Selected());
        }
        public void dessinerDessinNum(int i)
        {
            step = 25;
            gridDrawing(step);
            clear();
            dessinExo poly = dessinsModeExo[i];
            Point newCenter = new Point(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5);
            selectAxe(poly.repere);
            checkAxes();
            ShapePair shep;
            if (poly.type)
            {
                polygone = (Polygon)poly.shape;
                polygone.MouseEnter += shapeMouseEnter;
                polygone.MouseEnter += shapeMouseEnter;
                polygone.MouseLeave += shapeMouseLeave;
                shep = new ShapePair(polygone, canvas, shapeMouseEnter, shapeMouseLeave) { IsTransformable = false };
                shep.adaptToGrid(poly.oldCenter, newCenter);
                shapePairs.Add(shep);
                cleaningTheMess();

            }
            else
            {
                polyline = (Polyline)poly.shape;
                polyline.MouseEnter += shapeMouseEnter;
                polyline.MouseLeave += shapeMouseLeave;
                canvas.Children.Add(polyline);
                shep = new ShapePair(polyline, canvas, shapeMouseEnter, shapeMouseLeave) { IsTransformable = false };
                shep.adaptToGrid(poly.oldCenter, newCenter);
                shapePairs.Add(shep);
                cleaningTheMess();
            }

            // resave the shape in the file
            Shape shp = shapePairs[0].origin;
            int ind = niv.Selected() + 1;
            if (shp is Polygon)
                Utili.strTofile(@".\shapesExo.txt", Utili.CanvasToString(((Polygon)shp).Points, ((Polygon)shp).Fill, ((Polygon)shp).Stroke, ((toolBarEnseignant)TB).selectedAxe(), new Point(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5)), ind);
            else
                Utili.strTofile(@".\shapesExo.txt", Utili.CanvasToString(((Polyline)shp).Points, null, ((Polyline)shp).Stroke, ((toolBarEnseignant)TB).selectedAxe(), new Point(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5)), ind);
            dessinsModeExo = chargerDessins(@".\shapesExo.txt");

            if (axeSym != null || centresym != null)
            {
                if (isAxial)
                {
                    shapePairs[0].aSymGen(shapeMouseEnter, shapeMouseLeave, axeSym);

                }

                else
                {
                    shapePairs[0].cSymGen(shapeMouseEnter, shapeMouseLeave, centresym);
                }
             hideSym(shapePairs[0]);

            }









        }

        private void hideSym(ShapePair shp)
        {
            shp.sym.Visibility = Visibility.Hidden;
            foreach (Ellipse el in shp.sEllipse)
            {
                el.Visibility = Visibility.Hidden;
            }
            foreach (Line el in shp.jointLines)
            {
                el.Visibility = Visibility.Hidden;
            }
        }

        private void showSym(ShapePair shp)
        {
            shp.sym.Visibility = Visibility.Visible;
            foreach (Ellipse el in shp.sEllipse)
            {
                el.Visibility = Visibility.Visible;
            }

            //shp.jointLinesGen();
            
        }


        public void selectAxe(String axe)
        {
            switch (axe)
            {
                case "horiz":
                    {
                        horiz.IsChecked = true;
                        break;
                    }
                case "verti":
                    {
                        verti.IsChecked = true;

                        break;
                    }
                case "diag1":
                    {
                        diag1.IsChecked = true;

                        break;
                    }
                case "diag2":
                    {
                        diag2.IsChecked = true;

                        break;
                    }
                case "centre":
                    {
                        centre.IsChecked = true;

                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }


    }
}
