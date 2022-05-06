using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


    public partial class LibreExoEns : Page
    {
        
        
        canvasUC canvas;

        public LibreExoEns()
        {
            InitializeComponent();
             canvas = new canvasUC(toolBarEns,niv);
            myDock.Children.Add(canvas);
            DockPanel.SetDock(canvas, Dock.Bottom);
            //----------------------------------------------------//


            
            this.Resources.MergedDictionaries.Add( MainWindow.ResLibre);


            //----------------------------------------------------//

            toolBarEns.effacerTout.Click += canvas.effacerTout;
            toolBarEns.delShape.Click += canvas.delete_Click;
            toolBarEns.deplacer.Click += canvas.deplacer_Click;
            toolBarEns.horiz.Click += canvas.updateAxe;
            toolBarEns.verti.Click += canvas.updateAxe;
            toolBarEns.diag1.Click += canvas.updateAxe;
            toolBarEns.diag2.Click += canvas.updateAxe;
            toolBarEns.centre.Click += canvas.updateAxe;
            toolBarEns.valider.Click += canvas.valider_Click;
            toolBarEns.annuler.Click += canvas.annuler_Click;
            toolBarEns.EditEns.Click += canvas.EditEns_Click;

            niv.b1.Click += canvas.Niv_Click;
            niv.b2.Click += canvas.Niv_Click;
            niv.b3.Click += canvas.Niv_Click;
            niv.b4.Click += canvas.Niv_Click;
            niv.b5.Click += canvas.Niv_Click;
            niv.b6.Click += canvas.Niv_Click;
            niv.b7.Click += canvas.Niv_Click;
            niv.b8.Click += canvas.Niv_Click;
            niv.b9.Click += canvas.Niv_Click;

            
         
        }
       
        
        
        
        
        
        

        
        
    }
}
