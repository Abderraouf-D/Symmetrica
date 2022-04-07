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
        public static String CanvasToString(PointCollection pts, Brush rempli, Brush trace, Line axe, Point centre) {
            return String.Format("{0}-{1}-{2}-{3}-{4}", rempli == null, axe == null, pts.ToString(), rempli.ToString(), trace.ToString(), axe == null ? axe.ToString() : centre.ToString()); ; 
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

        public static Shape StringToShape(string shape)
        {
            char[] delimiterChars = { '-' };
            string[] data = shape.Split(delimiterChars);
            PointCollection pts = new PointCollection();
            string[] strpointarray = data[2].Split(' ');
            foreach (string item in strpointarray)
            {
                string[] newitem = item.Split(';');    
                pts.Add(new Point(double.Parse(newitem[0]), double.Parse(newitem[1])));
            }
            if (Boolean.Parse(data[0]))
            {     // false if polygon true if polyline 
                Polyline poly = new Polyline()
                {
                    Points = pts,
                    Stroke= (Brush)(new BrushConverter().ConvertFrom(data[4])),
                    StrokeThickness = 8 ,

                };
                return poly; 
            }
            else
            {
                Polygon poly = new Polygon()
                {
                    Points = pts,
                    Fill = (Brush)(new BrushConverter().ConvertFrom(data[4])),
                    Stroke = (Brush)(new BrushConverter().ConvertFrom(data[3])),
                    StrokeThickness = 1,
                };
                return poly; 
            }
            Boolean.Parse(data[1]);// false if centrale true if axiale
        }
    }
}
