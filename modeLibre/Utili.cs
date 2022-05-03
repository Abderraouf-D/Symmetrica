using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.IO;

namespace Projet2Cp
{
    internal class Utili
    {
        public struct dessinExo
        {
            public Shape shape;
            public Boolean type; // true for polygon 
            public string repere;
            public Point oldCenter;
            public double step; 
        }
        public static String CanvasToString(PointCollection pts, Brush rempli, Brush trace,string repere,Point oldCenter,double step) {

            if (pts == null) return null;

            return String.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}", rempli == null, pts.ToString(), (rempli != null)? rempli.ToString():null, trace.ToString(),repere,oldCenter.ToString(),step.ToString());  
        } 
        public static void strTofile(string filename, string str, int ind)
        {
            try
            {
                string[] arrLine = File.ReadAllLines(filename);
                arrLine[ind - 1] = str;
                File.WriteAllLines(filename, arrLine);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static String fileTostr(string filename, int ind)
        {
            try
            {
                string[] arrLine = File.ReadAllLines(filename);

                return arrLine[ind - 1];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static Shape StringToShape(string shape,out Boolean polyg,out string repere,out Point oldCenter,out double step)  // polyg== true if shape is polygnoe 
        {
            polyg = true;
            char[] delimiterChars = { '-' };
            string[] data = shape.Split(delimiterChars);
            repere = data[4];
            step = double.Parse(data[6]);
            string [] oldCent = data[5].Split(';');
            
           
            oldCenter = new Point(double.Parse(oldCent[0]), double.Parse(oldCent[1]));


            
            PointCollection pts = new PointCollection();
            string[] strpointarray = data[1].Split(' ');
            foreach (string item in strpointarray)
            {
                string[] newitem = item.Split(';');    
                pts.Add(new Point(double.Parse(newitem[0]), double.Parse(newitem[1])));
            }
            if (Boolean.Parse(data[0]))
            {     // false if polygon true if polyline 
                polyg = false;
                Polyline poly = new Polyline()
                {
                    Points = pts,
                    Stroke= (Brush)(new BrushConverter().ConvertFrom(data[3])),
                    StrokeThickness = 8 ,

                };
                return poly; 
            }
            else
            {
                Polygon poly = new Polygon()
                {
                    Points = pts,
                    Fill = (Brush)(new BrushConverter().ConvertFrom(data[2])),
                    Stroke = (Brush)(new BrushConverter().ConvertFrom(data[3])),
                    StrokeThickness = 3,
                };
                return poly; 
            }
            
        }


        public static  List<dessinExo> chargerDessins(string filename)
        {
            List<dessinExo> dessins = new List<dessinExo>();
            try
            {

                dessinExo dessin = new dessinExo();
                for (int i = 1; i <= 9; i++)
                {
                    Shape poly = Utili.StringToShape(Utili.fileTostr(filename, i), out dessin.type, out dessin.repere, out dessin.oldCenter, out dessin.step);
                    dessin.shape = poly;
                    dessins.Add(dessin);
                }
            } 
            finally {};
            return dessins;

        }

      
  
        public static double distancePointPoint(Point firstPoint, Point secondPoint)
        {
            return Math.Sqrt(Math.Pow(secondPoint.X - firstPoint.X, 2) + Math.Pow(secondPoint.Y - firstPoint.Y, 2));
        }


        public static double distancePointLine(Point firstPoint, Point secondPoint, Point point)
        {
            double num = (secondPoint.X - firstPoint.X) * (firstPoint.Y - point.Y) - (firstPoint.X - point.X) * (secondPoint.Y - firstPoint.Y);
            num = Math.Abs(num);
            num = num / Math.Sqrt(Math.Pow(secondPoint.X - firstPoint.X, 2) + Math.Pow(secondPoint.Y - firstPoint.Y, 2));
            return num;
        }

        public static PointCollection reverse(PointCollection pc)
        {
            PointCollection reversed = new PointCollection();
            int k = pc.Count - 1;
            while (k >= 0)
            {
                reversed.Add(pc[k]);
                k--;
            }
            return reversed; 
        }
        public static bool isSubTab(PointCollection p1, PointCollection tmp)
        {
            int k = 0;

            while (k < p1.Count && k < tmp.Count)
            {
                double x1 = Math.Round(tmp[k].X, 6);
                double y1 = Math.Round(tmp[k].Y, 6);
                double x2 = Math.Round(p1[k].X, 6);
                double y2 = Math.Round(p1[k].Y, 6);

                if (!(x1 == x2) || !(y1 == y2))
                {
                    return  false;
                }

                k++;
            }
            return true;

        }
        public static bool isSym(PointCollection p4, PointCollection p3, bool polygon,bool move  , Point  next)
        {
            PointCollection p2 = new PointCollection(p3);
            PointCollection p1 = new PointCollection(p4);

            if ((p1[p1.Count-1].Equals(  p1[0])) && polygon) p1.RemoveAt(p1.Count-1);
            if (move )  p2.Add(next);
            bool equal = true ;
            
            
            if (p1.Count > p2.Count) return false;
            if ((p1.Count == 1)&& p2.Contains(p1[0])) return true;
            if (p1.Count > 0)
            {
                PointCollection tmp = new PointCollection();
                int k = 0;
                int indEq = 0 ;
                bool contin = true;
                if (polygon)
                {
                    while (k < p2.Count && contin)
                    {
                        double x1 = Math.Round(p2[k].X, 6);
                        double y1 = Math.Round(p2[k].Y, 6);
                        double x2 = Math.Round(p1[0].X, 6);
                        double y2 = Math.Round(p1[0].Y, 6);
                        
                        if ((x1 == x2) && (y1 == y2))
                        {
                            contin = false;
                        }
                        else
                            k++;
                    }
                    if (k == p2.Count) return false;
                    indEq = k; 
                    while (tmp.Count != p2.Count)
                    {
                        tmp.Add(p2[k]);
                        k = (k + 1) % p2.Count;
                    }
                }
                else
                {
                    if (p1[0].Equals(p2[0])) tmp = p2;
                    else if (p1[0].Equals(p2[p2.Count - 1])) tmp = reverse(p2);
                    else return false;

                }
                equal = isSubTab(p1, tmp);
                if (polygon && !equal){
                    k = indEq;
                    tmp.Clear();
                    while (tmp.Count != p2.Count)
                    {
                        tmp.Add(p2[k]);
                        k --;
                        if (k < 0) k = p2.Count - 1;
                    }
                    equal = isSubTab(p1, tmp);

                }
                
            }
            else equal = false;
           
            
            return equal; 
            


        }





    }
}
