using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Projet2Cp
{
    internal class Utili
    {
        public static String CanvasToString(PointCollection pts, Brush rempli, Brush trace, Line axe, Point centre) {
            return String.Format("{0}{1}{2}{3}{4}", rempli == null, axe == null, pts.ToString(), rempli.ToString(), trace.ToString(), axe == null ? axe.ToString() : centre.ToString()); ; 
        }
        public static void strTofile(string filename, string str ) {
        }
    }
}
