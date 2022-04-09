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


    public partial class MainWindow : Window
    {
        private Boolean modeLibre , modeEns; 
        public MainWindow()
        {
            InitializeComponent();
            modeLibre = false;
            modeEns = false;
            if (modeLibre)  MainFrame.NavigationService.Navigate(new ModeLibre());
            else if(modeEns) MainFrame.NavigationService.Navigate(new LibreExoEns());
                 else MainFrame.NavigationService.Navigate(new LibreExoEleve());
        }

    }
}
