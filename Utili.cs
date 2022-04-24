﻿using System;
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
        }
        public static String CanvasToString(PointCollection pts, Brush rempli, Brush trace,string repere) {
            return String.Format("{0}-{1}-{2}-{3}-{4}", rempli == null, pts.ToString(), (rempli != null)? rempli.ToString():null, trace.ToString(),repere);  
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

        public static Shape StringToShape(string shape,out Boolean polyg,out string repere) // polyg== true if shape is polygnoe 
        {
            polyg = true;
            char[] delimiterChars = { '-' };
            string[] data = shape.Split(delimiterChars);
            repere = data[4];
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
            dessinExo dessin = new dessinExo();
            for (int i=1; i<=9; i++)
            {
                Shape poly = Utili.StringToShape(Utili.fileTostr(@".\shapesExo.txt", i), out dessin.type, out dessin.repere);
                dessin.shape = poly;
                dessins.Add(dessin);
            }
            return dessins;

        }

        //// le3fayes tae younes 
  
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

        public static bool isSubTable( PointCollection p1 , PointCollection p2, bool polygone)
        {
            int i, k;
            if (p1.Count > p2.Count) return false;
            else
            {
                i = p2.IndexOf(p1[0]);
                if (i!=-1)
                {
                    k = 0;
                    
                    while (k < p1.Count)
                    {
                        if (p1[k] != p2[i]) return false; 
                        else
                        {
                            i = (i + 1) % p2.Count; 
                        }
                    }
                    return true; 
                }
                else
                {
                    return false;
                }
            }
            
        }





    }
}
