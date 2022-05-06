using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project ;
using ModeCours;
namespace Projet2Cp
{
    public class Eleve
    {

        private String nom;
        private int progressCoursCentrale = 0 ;
        private int progressCoursAxiale = 0 ;

        public Eleve (string nom , int progressCours)
        {
            this .nom = nom;
            this .progressCoursCentrale = progressCours;
            this.progressCoursCentrale = progressCours;

        }
        public String getNom() { return nom; }
        

        public void incProgress()
        {
            if ( (MainWindow.francais && PagePrincCours.axiale) || ( !MainWindow.francais && PagePrincCoursAr.axiale))
            {
                if (progressCoursAxiale < 8) progressCoursAxiale++;
            }else
            {
                if (progressCoursCentrale < 8) progressCoursCentrale++;

            }
        }
        public void decProgress()
        {
            if ((MainWindow.francais && !PagePrincCours.axiale) || (!MainWindow.francais && !PagePrincCoursAr.axiale))
            {
                if (progressCoursAxiale > 0) progressCoursAxiale--;
            }
            else
            {
                if (progressCoursCentrale > 0) progressCoursCentrale--;

            }
        }


        public int getProgressAxe() { return progressCoursAxiale; }
        public int getProgressCen() { return progressCoursCentrale; }
    }
}
