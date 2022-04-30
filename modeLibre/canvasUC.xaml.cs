using Microsoft.Win32;
using Syncfusion.UI.Xaml.Diagram;
using System;
using System.Collections.Generic;
using System.IO;
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
using Ookii;
using Ookii.Dialogs.Wpf;

namespace Projet2Cp
{

    public partial class canvasUC : UserControl
    {

        private string exoPath = @".\Exercices\DessinerSym\shapesExo.txt";

        private List<ShapePair> shapePairs { get; set; } //represents the pairs of shapes
        private Point mousePosition; //mouse position in canvas
        private Point actualPoint;//actual point to draw in 
        
        public static double step = 25; // the step of the grid 
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
        bool answer = false;

        String fileDrawing;

        int attempts=0;
        private List<TextBlock> nums = new List<TextBlock>();
        public canvasUC(UserControl TB, niveauxLibre niv)
        {

            InitializeComponent();
            canvas.Children.Remove(message);
            dessinsModeExo = chargerDessins(exoPath);
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
                upload.Visibility=Visibility.Collapsed;
                save.Visibility=Visibility.Collapsed;
                
                ((toolBarEnseignant)TB).ensStack.Visibility=Visibility.Collapsed;
                tb = new TextBlock();
                tb.Text = "Dessine le symétrique du déssin par rapport au repère donné puis clique sur" + "\"" + ((toolBarEnseignant)TB).vld.Text + "\"";
                tb.Foreground = Brushes.Black;
                tb.FontSize = 18;
                tb.TextAlignment = TextAlignment.Center;
                niv.nivStack.Children.Add(tb);
                tb.Background=Brushes.White;
                Panel.SetZIndex(tb,122);
                this.niv = niv;
                    horiz = ((toolBarEnseignant)TB).horiz;
                    verti = ((toolBarEnseignant)TB).verti;
                    diag1 = ((toolBarEnseignant)TB).diag1;
                    diag2 = ((toolBarEnseignant)TB).diag2;
                    centre = ((toolBarEnseignant)TB).centre;
                
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
        //=============================================================================================================================//
        //                                                    EDIT MODE                                                                //
        //=============================================================================================================================//
        void edit(object sender, MouseButtonEventArgs e)
        {
            if (currentShapePair.IsTransformable && isDrawing && (clickPosition.Equals(e.GetPosition(canvas))))
            {


                if (currentShapePair.origin is Polygon)
                    foreach (Point p in ((Polygon)currentShapePair.origin).Points)
                        polyline.Points.Add(p);
                else
                    foreach (Point p in ((Polyline)currentShapePair.origin).Points)
                        polyline.Points.Add(p);

                foreach (Line l in currentShapePair.jointLines)
                    canvas.Children.Remove(l);
                foreach (TextBlock tb in currentShapePair.otb)
                    canvas.Children.Remove(tb);

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
                foreach (TextBlock tb in currentShapePair.stb)
                    canvas.Children.Remove(tb);
                shapePairs.Remove(currentShapePair);

            }
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

            if (step * alpha > 50 || step * alpha < 15)
                return;
            step *= alpha;
            gridDrawing(step);
            PointCollection pc = polyline.Points; //originPointCollection
            Point paz;
            Point center = new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2);
            for (int j = 0; j < pc.Count; j++)
            {
                paz = new Point();
                paz.X = (pc[j].X - center.X) * alpha + center.X;
                paz.Y = (pc[j].Y - center.Y) * alpha + center.Y;
                pc[j] = paz;
            }

            for (int i = 0; i < shapePairs.Count; i++)
            {
                if (shapePairs[i].origin is Polygon)
                    pc = ((Polygon)shapePairs[i].origin).Points;
                else
                    pc = ((Polyline)shapePairs[i].origin).Points;
                for (int j = 0; j < pc.Count; j++)
                {
                    paz = new Point();
                    paz.X = (pc[j].X - center.X) * alpha + center.X;
                    paz.Y = (pc[j].Y - center.Y) * alpha + center.Y;
                    pc[j] = paz;
                }
                for (int j = 0; j < shapePairs[i].oEllipse.Count; j++)
                {
                    Canvas.SetLeft(shapePairs[i].oEllipse[j], pc[j].X - 10);
                    Canvas.SetTop(shapePairs[i].oEllipse[j], pc[j].Y - 10);

                    Canvas.SetLeft(shapePairs[i].otb[j], pc[j].X - 7.5);
                    Canvas.SetTop(shapePairs[i].otb[j], pc[j].Y - 7.5);
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
                        Canvas.SetLeft(shapePairs[i].sEllipse[j], pc[j].X - 10);
                        Canvas.SetTop(shapePairs[i].sEllipse[j], pc[j].Y - 10);

                        Canvas.SetLeft(shapePairs[i].stb[j], pc[j].X - 7.5);
                        Canvas.SetTop(shapePairs[i].stb[j], pc[j].Y - 7.5);
                    }
                }


                if (MainWindow.modeLibre) shapePairs[i].jointLinesGen();
                else
                {
                    if (shapePairs[0].sym != null)
                    {
                        if ((shapePairs[0].sym.Visibility == Visibility.Visible)) shapePairs[0].jointLinesGen();
                    }
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
            GD.Brush = Brushes.White;
            

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



            //RULER
            foreach (TextBlock el in nums) canvas.Children.Remove(el);
            nums.Clear();
            double nbNumHoriz = Math.Truncate((canvas.ActualWidth / 2) / step);
            TextBlock num;

            ////Horizontal 0 + 
            double marche = canvas.ActualWidth / 2;
            int i = 0;
             while(marche < canvas.ActualWidth)
            {
                num = new TextBlock()
                {
                    Foreground = Brushes.Green,
                    IsHitTestVisible = false,

                };
                
                num.Text = i.ToString();
                canvas.Children.Add(num);
                Panel.SetZIndex(num, -10);

                Canvas.SetLeft(num, marche-10);
                Canvas.SetTop(num, canvas.ActualHeight / 2 );

                marche += step;
                i++;
                nums.Add(num);
            }
            ////Horizontal 0 - 
            marche = canvas.ActualWidth/ 2;
            i = -1;
            while (marche > 0)
            {
                num = new TextBlock()
                {
                    Foreground = Brushes.Green,
                    IsHitTestVisible = false,


                };
                marche -= step;
                num.Text = i.ToString();
                canvas.Children.Add(num);
                Panel.SetZIndex(num, -10);

                Canvas.SetLeft(num, marche - 14);
                Canvas.SetTop(num, canvas.ActualHeight / 2);
                i--;
                nums.Add(num);

            }
            ////vertical 0 - 
            marche = canvas.ActualHeight / 2;
            i = -1;
            while (marche < canvas.ActualHeight)
            {
                num = new TextBlock()
                {
                    IsHitTestVisible = false,
                    Foreground = Brushes.Green,
                    

                };
                marche += step;
                num.Text = i.ToString();
                canvas.Children.Add(num);
                Panel.SetZIndex(num, -10);
                Canvas.SetRight(num, canvas.ActualWidth / 2 + 3);
                Canvas.SetTop(num, marche );

                i--;
                nums.Add(num);

            }

            ////Vertical 0 + 
            marche = canvas.ActualHeight / 2;
             i = 1;
            while (marche > 0)
            {
                num = new TextBlock()
                {
                    
                    IsHitTestVisible = false,
                    Foreground = Brushes.Green,
                    
                };
                marche -= step;
                num.Text = i.ToString();
                canvas.Children.Add(num);
                Panel.SetZIndex(num, -10);

                Canvas.SetRight(num, canvas.ActualWidth / 2 +3);
                Canvas.SetTop(num, marche);
                
                i++;
                nums.Add(num);

            }
            
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

                bool polygon = true;
                if (!MainWindow.modeLibre && !isEditing)
                {
                    
                    PointCollection p1 = null, p2 = null;
                    if (shapePairs[0].origin is Polyline)
                    {
                        p2 = ((Polyline)shapePairs[0].sym).Points;
                        polygon = false;
                    }
                    else p2 = ((Polygon)shapePairs[0].sym).Points;

                    if (currentShapePair.origin is Polyline)
                    {
                        p1 = ((Polyline)currentShapePair.origin).Points;
                        polygon = false;
                    }
                    else p1 = ((Polygon)currentShapePair.origin).Points;


                    if (isSym(p1, p2, polygon, false, actualPoint))
                    {
                        trace = Brushes.Green;

                    }
                    else
                    {
                        trace = Brushes.Red;

                    }
                    line.Stroke = trace;
                    currentShapePair.origin.Stroke = trace;
                }
               
                
                
            }


        }

        //====================================================================================================================//
        //                          ELLIPSE_CLEANER : ENLEVE LA PREVIEW ELLIPSE UNE FOIS QU'ON QUITTE LE CANVAS               //  
        //====================================================================================================================//


        public void ellipseCleaner(Object sender, MouseEventArgs e)
        {
            canvas.Children.Remove(ellipse);
            canvas.Children.Remove(rotateLine);
            isTransforming = false;
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



                if (polyline.Points.Count > 0)
                {
                    if (!actualPoint.Equals(polyline.Points[polyline.Points.Count - 1])) polyline.Points.Add(actualPoint);
                }
                else polyline.Points.Add(actualPoint);



                if (!MainWindow.modeLibre && !isEditing)
                {
                    bool polygon = true;
                    PointCollection p1 = null, p2 = null;
                    if (shapePairs[0].origin is Polyline)
                    {
                        p2 = ((Polyline)shapePairs[0].sym).Points;
                        polygon = false;
                    }
                    else p2 = ((Polygon)shapePairs[0].sym).Points;



                    p1 = polyline.Points;
                    if (isSym(p1, p2, polygon, false, actualPoint))
                    {
                        trace = Brushes.Green;

                    }
                    else
                    {
                        trace = Brushes.Red;

                    }
                }
                line.Stroke = trace;
                polyline.Stroke = trace;
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
                            foreach (TextBlock tb in currentShapePair.otb)
                                canvas.Children.Remove(tb);

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
                            foreach (TextBlock tb in currentShapePair.stb)
                                canvas.Children.Remove(tb);
                            shapePairs.Remove(currentShapePair);

                            deleteSegment = false;


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
                                pc.Add(new Point(p.X + step, p.Y + step));
                            polygon = new Polygon()
                            {
                                Stroke = ((Polygon)shape).Stroke,
                                StrokeThickness = 3,
                                Fill = ((Polygon)shape).Fill,
                                Points = pc,
                            };
                            polygon.MouseEnter += shapeMouseEnter;
                            polygon.MouseLeave += shapeMouseLeave;
                            shapePairs.Add(new ShapePair(polygon, canvas, shapeMouseEnter, shapeMouseLeave,edit));
                        }

                        else
                        {
                            foreach (Point p in ((Polyline)shape).Points)
                                pc.Add(new Point(p.X + step, p.Y + step));
                            polyline = new Polyline()
                            {
                                Stroke = ((Polyline)shape).Stroke,
                                StrokeThickness = 8,
                                Points = pc,
                            };
                            canvas.Children.Add(polyline);
                            polyline.MouseEnter += shapeMouseEnter;
                            polyline.MouseLeave += shapeMouseLeave;
                            shapePairs.Add(new ShapePair(polyline, canvas, shapeMouseEnter, shapeMouseLeave,edit));

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
                    shapePairs.Add(new ShapePair(polygone, canvas, shapeMouseEnter, shapeMouseLeave,edit)); //we create a new shapePair with origin = polygon "linked to canvas"
                }

                else
                {
                    //we keep the polyline
                    polyline.StrokeThickness = 8;
                    polyline.MouseMove += shapeMouseEnter;
                    polyline.MouseLeave += shapeMouseLeave;
                    shapePairs.Add(new ShapePair(polyline, canvas, shapeMouseEnter, shapeMouseLeave,edit));
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
                this.Cursor = ((TextBlock)Resources["CursorRotate"]).Cursor;
            }
            if (isDrawing)
            {
                if (isOverShape)
                    this.Cursor = ((TextBlock)Resources["CursorMove"]).Cursor;
                else this.Cursor = ((TextBlock)Resources["CursorDraw"]).Cursor;
                // this.Cursor = Cursors.Pen;
            }
            if (isGomme)
            {
                this.Cursor = ((TextBlock)Resources["CursorErase"]).Cursor;
            }
            if (isGen)
            {
                this.Cursor = this.Cursor = ((TextBlock)Resources["CursorSym"]).Cursor; ;
            }
            if (isColoring) this.Cursor = ((TextBlock)Resources["CursorColor"]).Cursor;
            if (isDuplicating) this.Cursor = ((TextBlock)Resources["CursorDuplicate"]).Cursor;
          


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
            
            canvas.Children.Clear();
            initCanvas();
            if (!MainWindow.modeLibre & !isEditing)
            {
                dessinerDessinNum(niv.Selected());
            }
            answer = false;
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
                foreach (TextBlock el in shapePairs[i].stb)
                {
                    canvas.Children.Remove(el);
                }
                shapePairs[i].stb=new List<TextBlock>();
            }

            if (axeSym == null)
            {
                axeSym = new Line();
                axeSym.StrokeThickness = 1;
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
                case true when (bool)diag2.IsChecked:
                    {
                        axeSym.X1 = 0;
                        axeSym.Y1 = (canvas.ActualHeight+canvas.ActualWidth )*0.5;

                        axeSym.X2 = (canvas.ActualHeight + canvas.ActualWidth) * 0.5;
                        axeSym.Y2 = 0;

                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;


                        break;
                    }
                case true when (bool)diag1.IsChecked:
                    {
                        
                        axeSym.X2 = (canvas.ActualWidth - canvas.ActualHeight  ) * 0.5; 
                        axeSym.Y2 = 0;

                        axeSym.Y1 = canvas.ActualHeight; 
                        axeSym.X1 = (canvas.ActualWidth + canvas.ActualHeight) * 0.5;

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
            shapePairs.Add(new ShapePair(poly, canvas, shapeMouseEnter, shapeMouseLeave,edit));


        }



        //==================================================================================================//
        //                                   Methodes  du mode enseignant                                   //
        //==================================================================================================//


        public void EditEns_Click(Object sender, RoutedEventArgs e)
        {
            clear();
            isEditing = true;
            tb.Text = "Veuillez créer UNE forme, selectionner le repère de symetrie, Puis cliquer sur \"Confirmer\".";
            tb.Foreground = Brushes.Red;
            ((toolBarEnseignant)TB).vld.Text = "Confirmer";
            ((toolBarEnseignant)TB).annuler.Visibility = Visibility.Visible;
            niv.IsEnabled = false;
            ((toolBarEnseignant)TB).ensStack.Visibility = Visibility.Visible;



        }

        private void canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvas.Children.Remove(centreSym);
            centresym = null;
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
                case true when (bool)diag2.IsChecked:
                    {
                        axeSym.X1 = 0;
                        axeSym.Y1 = (canvas.ActualHeight + canvas.ActualWidth) * 0.5;

                        axeSym.X2 = (canvas.ActualHeight + canvas.ActualWidth) * 0.5;
                        axeSym.Y2 = 0;
                        if (!canvas.Children.Contains(axeSym)) canvas.Children.Add(axeSym);
                        isAxial = true;


                        break;
                    }
                case true when (bool)diag1.IsChecked:
                    {
                        axeSym.X2 = (canvas.ActualWidth - canvas.ActualHeight) * 0.5;
                        axeSym.Y2 = 0;

                        axeSym.Y1 = canvas.ActualHeight;
                        axeSym.X1 = (canvas.ActualWidth + canvas.ActualHeight) * 0.5;
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
                        Utili.strTofile(exoPath, Utili.CanvasToString(((Polygon)shp).Points, ((Polygon)shp).Fill, ((Polygon)shp).Stroke, ((toolBarEnseignant)TB).selectedAxe(), new Point(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5),step), ind);
                    else
                        Utili.strTofile(exoPath, Utili.CanvasToString(((Polyline)shp).Points, null, ((Polyline)shp).Stroke, ((toolBarEnseignant)TB).selectedAxe(), new Point(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5),step), ind); ;
                    dessinsModeExo = chargerDessins(exoPath);
                    MessageBox.Show("Dessin modifié avec succès!");
                    canvas.Children.Remove(tb);
                    ((toolBarEnseignant)TB).annuler.Visibility = Visibility.Collapsed;
                    niv.IsEnabled = true;
                    ((toolBarEnseignant)TB).vld.Text = "Valider";
                    isEditing = false;
                    tb.Text = "Dessine le symétrique du déssin par rapport au repère donné puis clique sur"+"\""+ ((toolBarEnseignant)TB).vld.Text + "\"";
                    tb.Foreground = Brushes.Black;
                    ((toolBarEnseignant)TB).ensStack.Visibility = Visibility.Collapsed;





                }
                clear();

            }
            else
            {
                if (!answer)
                {
                   

                    if (shapePairs.Count > 1)
                    {
                        attempts++;
                        answer = true;
                        verifSym();
                        ((toolBarEnseignant)TB).vld.Text = "Réessayer!";
                       
                    }
                   
                   

                }
                else
                {
                    clear();
                    ((toolBarEnseignant)TB).vld.Text = "Valider";
                    ((toolBarEnseignant)TB).vld.Style = ((Style)Resources["validSty"]);
                }



            }



        }

        public void verifSym()
        {
            
                if (!shapePairs[0].origin.GetType().Equals(shapePairs[1].origin.GetType()))
                {
                    shapePairs[0].sym.Stroke = Brushes.Red;
                    if (!canvas.Children.Contains(message)) canvas.Children.Add(message);
                    message.Text = "Oooops!";
                    message.Foreground = Brushes.Red;
                    if (attempts == 3)
                    {
                        foreach (Ellipse el in shapePairs[1].oEllipse) canvas.Children.Remove(el);
                        foreach (TextBlock el in shapePairs[1].otb) canvas.Children.Remove(el);
                        canvas.Children.Remove(shapePairs[1].origin);
                        shapePairs.Remove(shapePairs[1]);
                        showSym(shapePairs[0]);
                        shapePairs[0].jointLinesGen();
                        attempts = 0;
                    }
                }
                else
                {
                    bool polygon = true;
                    PointCollection p1 = null, p2 = null;
                    if (shapePairs[0].origin is Polyline)
                    {
                        p2 = ((Polyline)shapePairs[0].sym).Points;
                        polygon = false;
                    }
                    else p2 = ((Polygon)shapePairs[0].sym).Points;

                    if (shapePairs[1].origin is Polygon) p1 = ((Polygon)shapePairs[1].origin).Points;
                    else p1 = ((Polyline)shapePairs[1].origin).Points;



                    if (isSym(p1, p2, polygon, false, new Point()))
                    {
                        shapePairs[0].sym.Stroke = Brushes.Green;
                        if (!canvas.Children.Contains(message)) canvas.Children.Add(message);

                        message.Text = "Super!";
                        message.Foreground = Brushes.Green;
                        showSym(shapePairs[0]);
                        shapePairs[0].jointLinesGen();
                        foreach (Ellipse el in shapePairs[1].oEllipse) canvas.Children.Remove(el);
                        foreach (TextBlock el in shapePairs[1].otb) canvas.Children.Remove(el);
                        canvas.Children.Remove(shapePairs[1].origin);
                        shapePairs.Remove(shapePairs[1]);

                    }
                    else
                    {

                        shapePairs[0].sym.Stroke = Brushes.Red;
                        if (!canvas.Children.Contains(message)) canvas.Children.Add(message);
                        message.Text = "Oooops!";
                        message.Foreground = Brushes.Red;
                        if (attempts == 3)
                        {
                            foreach (Ellipse el in shapePairs[1].oEllipse) canvas.Children.Remove(el);
                            foreach (TextBlock el in shapePairs[1].otb) canvas.Children.Remove(el);
                            canvas.Children.Remove(shapePairs[1].origin);
                            shapePairs.Remove(shapePairs[1]);
                            showSym(shapePairs[0]);
                            shapePairs[0].jointLinesGen();
                            attempts = 0;

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
            clear();
            tb.Text = "Dessine le symétrique du déssin par rapport au repere donné puis clique sur " + "\""+((toolBarEnseignant)TB).vld.Text + "\"";
            tb.Foreground = Brushes.Black;
            ((toolBarEnseignant)TB).ensStack.Visibility = Visibility.Collapsed;


        }

   

        public void Niv_Click(object sender, RoutedEventArgs e)
        {
            answer = false;
            attempts = 0;
            clear();
        }
        public void dessinerDessinNum(int i)
        {
            try
            {
                dessinExo poly = dessinsModeExo[i];
                step = poly.step;
                gridDrawing(step);
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
                    shep = new ShapePair(polygone, canvas, shapeMouseEnter, shapeMouseLeave,edit) { IsTransformable = false };
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
                    shep = new ShapePair(polyline, canvas, shapeMouseEnter, shapeMouseLeave,edit) { IsTransformable = false };
                    shep.adaptToGrid(poly.oldCenter, newCenter);
                    shapePairs.Add(shep);
                    cleaningTheMess();
                }

                // resave the shape in the file
                Shape shp = shapePairs[0].origin;
                int ind = niv.Selected() + 1;
                if (shp is Polygon)
                    Utili.strTofile(exoPath, Utili.CanvasToString(((Polygon)shp).Points, ((Polygon)shp).Fill, ((Polygon)shp).Stroke, ((toolBarEnseignant)TB).selectedAxe(), new Point(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5), step), ind);
                else
                    Utili.strTofile(exoPath, Utili.CanvasToString(((Polyline)shp).Points, null, ((Polyline)shp).Stroke, ((toolBarEnseignant)TB).selectedAxe(), new Point(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5), step), ind);
                dessinsModeExo = chargerDessins(exoPath);

                if (axeSym != null || centresym != null)
                {
                    if (isAxial)
                    {
                        axeSym.Stroke = Brushes.Red;
                        axeSym.StrokeThickness = 3;

                        shapePairs[0].aSymGen(shapeMouseEnter, shapeMouseLeave, axeSym);
                    }

                    else
                    {
                        shapePairs[0].cSymGen(shapeMouseEnter, shapeMouseLeave, centresym);
                    }
                    hideSym(shapePairs[0]);

                }
            }catch (Exception ex) {
               
            }
            finally
            {
                
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
            foreach(TextBlock el in shp.stb)
            {
                el.Visibility  = Visibility.Hidden;
            }
        }

        private void showSym(ShapePair shp)
        {
            shp.sym.Visibility = Visibility.Visible;
            foreach (Ellipse el in shp.sEllipse)
            {
                el.Visibility = Visibility.Visible;
            }
            foreach (TextBlock el in shp.stb)
            {
                el.Visibility = Visibility.Visible;
            }



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



        //////////////////////////////////////////////// save & Upload ///////////////////

        public void ExportToPng(Uri path, Canvas surface)
        {
            if (path == null) return;
           
            Size size = new Size(surface.ActualWidth, surface.ActualHeight);
           
            surface.Measure(size);
            surface.Arrange(new Rect(size));
            surface.UpdateLayout();

            
            RenderTargetBitmap renderBitmap =
              new RenderTargetBitmap(
                (int)size.Width,
                (int)size.Height,
                96d,
                96d,
                PixelFormats.Pbgra32);
            renderBitmap.Render(cnvBorder);

            
            using (FileStream outStream = new FileStream(path.LocalPath, FileMode.Create))
            {
                // Use png encoder for our data
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                // push the rendered bitmap to it
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                // save the data to the stream
                encoder.Save(outStream);
            }
            
         
        }



        private void exportPng_Click(object sender, RoutedEventArgs e)
        {
            string path;
            string fileDrawing=""; 
            
                int i = 0;
                path = ShowFolderBrowserDialog();
            fileDrawing = path + "/MonDessinImage" + i.ToString() + ".png";

            while (File.Exists(fileDrawing))
                {
                    fileDrawing = path + "/MonDessinImage" + i.ToString() + ".png";
                    i++;
                }
            ExportToPng(new System.Uri(fileDrawing), canvas);

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveDrawing();
            }catch (Exception ex)
            {

            }
            finally { }

        }

        private void upload_Click(object sender, RoutedEventArgs e)
        {
            try { 
            OpenDrawing();
             }
            catch (Exception ex)
            {

            }
            finally { }
        }

        public bool OpenDrawing()
        {
            
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == true)
            {
                fileDrawing= dlg.FileName;
                string[] shapes = File.ReadAllLines(fileDrawing);
                string symIndex = shapes[shapes.Length - 1];
                dessinExo poly = new dessinExo();
                string[] newitem = symIndex.Split(';');
                HashSet<int> indexes = new HashSet<int>();
                foreach (string item in newitem)
                {
                   if (!string.IsNullOrEmpty(item)) indexes.Add(int.Parse(item));
                }

                for (int i = 0; i< shapes.Length-1; i++)
                {
                    poly.shape = StringToShape(shapes[i], out poly.type,out poly.repere,out poly.oldCenter,out poly.step) ;
                    step = poly.step;
                    gridDrawing(step);
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
                        shep = new ShapePair(polygone, canvas, shapeMouseEnter, shapeMouseLeave,edit) ;
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
                            shep = new ShapePair(polyline, canvas, shapeMouseEnter, shapeMouseLeave,edit) ;
                            shep.adaptToGrid(poly.oldCenter, newCenter);
                            shapePairs.Add(shep);
                            cleaningTheMess();
                     
                    }

                    if (indexes.Contains(i))
                    {
                        if (isAxial)
                        {
                            shep.aSymGen(shapeMouseEnter, shapeMouseLeave, axeSym);
                        }

                        else
                        {
                            shep.cSymGen(shapeMouseEnter, shapeMouseLeave, centresym);
                        }
                      
                    }


                }
                fileDrawing = null;
                return true;
            }
            fileDrawing = null;
            return false;
        }
        public void SaveDrawing()
        {
            string path; 
            if (string.IsNullOrEmpty(fileDrawing)) {
                int i = 0;
                path = ShowFolderBrowserDialog();
                fileDrawing = path + "/MonDessin" + i.ToString() + ".sym";
                while (File.Exists(fileDrawing)) {
                    fileDrawing = path + "/MonDessin" + i.ToString() + ".sym";
                    i++;
                }
            }
                List<string> list = new List<string>();
                string listSym="";
                string shapeStr;
                foreach(ShapePair shp in shapePairs)
                {
                    if (shp.origin is Polygon) {    
                       shapeStr=Utili.CanvasToString(((Polygon)shp.origin).Points, ((Polygon)shp.origin).Fill, ((Polygon)shp.origin).Stroke, ((toolBarLibreLibre)TB).selectedAxe(), new Point(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5), step)  ;
                     }
                    else
                    {
                        shapeStr = Utili.CanvasToString(((Polyline)shp.origin).Points, ((Polyline)shp.origin).Fill, ((Polyline)shp.origin).Stroke, ((toolBarLibreLibre)TB).selectedAxe(), new Point(canvas.ActualWidth * 0.5, canvas.ActualHeight * 0.5), step) ;

                    }
                    list.Add(shapeStr);
                    

                    if (shp.sym != null)
                    {
                        listSym+= shapePairs.IndexOf(shp).ToString()+ ";";
                    }
                    
                }
                list.Add(listSym);
                string[] data = list.ToArray();
                File.WriteAllLines(fileDrawing, data);
               
            
        }
       

        private string ShowFolderBrowserDialog()
        {
            var dialog = new VistaFolderBrowserDialog();
            dialog.Description = "Veuillez choisir un dossier:";
            dialog.UseDescriptionForTitle = true; // This applies to the Vista style dialog only, not the old dialog.

            

            if ((bool)dialog.ShowDialog())
            {
                return dialog.SelectedPath;
            }
            return null;
        }
    }
}


