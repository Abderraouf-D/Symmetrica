using System;
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

        canvasUC canvas;
        public ModeLibre()
        {
            InitializeComponent();
            canvas   =  new canvasUC(toolBar,null);
            myDock.Children.Add(canvas);
            DockPanel.SetDock(canvas, Dock.Bottom);
            //----------------------------------------------------//
            this.Resources.MergedDictionaries.Add( MainWindow.ResLibre);


            //----------------------------------------------------//
            toolBar.addPolygon.Click += canvas.addPolygon;
            toolBar.effacerTout.Click += canvas.effacerTout;
            toolBar.genSym.Click += canvas.GenSym_Click;
            toolBar.delShape.Click += canvas.delete_Click;
            toolBar.deplacer.Click += canvas.deplacer_Click;
            toolBar.rotate.Click += canvas.rotate_Click;
            toolBar.colorier.Click += canvas.colorier_Click;
            toolBar.duplicate.Click += canvas.dupliquer;
            toolBar.horiz.Click += canvas.updateAxe;
            toolBar.verti.Click += canvas.updateAxe;
            toolBar.diag1.Click += canvas.updateAxe;
            toolBar.diag2.Click += canvas.updateAxe;
            toolBar.centre.Click += canvas.updateAxe;



        }



    }
}
