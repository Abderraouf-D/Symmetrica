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
        private int id;
        private String nom;
        private int progressCoursCentrale  ;
        private int progressCoursAxiale  ;

        public Eleve (int Id,string nom , int progressCoursAxe,int progressCoursCentrale)
        {
            this.id = Id;
            this.nom = String.Copy(nom);
            this .progressCoursCentrale = progressCoursCentrale;
            this.progressCoursAxiale = progressCoursAxe;

        }
        public String getNom() { return nom; }
        public int getId() { return id; }


        public void incProgress()
        {
            if (MainWindow.francais)
            {
                if (PagePrincCours.axiale) progressCoursAxiale = (progressCoursAxiale + 1 )%9;
                else progressCoursCentrale = (progressCoursCentrale + 1) % 9;
            }
            else
            {
                if (PagePrincCoursAr.axiale) progressCoursAxiale = (progressCoursAxiale + 1) % 9;
                else progressCoursCentrale = (progressCoursCentrale + 1) % 9;
            }
        }
        public void decProgress()
        {
           
            if (MainWindow.francais)
            {
                if (progressCoursAxiale != 0)
                {
                    if (PagePrincCours.axiale) progressCoursAxiale = (progressCoursAxiale - 1) % 9;
                    else progressCoursCentrale = (progressCoursCentrale - 1) % 9;
                }
            }
            else
            {
                if (progressCoursCentrale != 0)
                {
                    if (PagePrincCoursAr.axiale) progressCoursAxiale = (progressCoursAxiale - 1) % 9;
                    else progressCoursCentrale = (progressCoursCentrale - 1) % 9;
                }
            }
        }


        public int getProgressAxe() { return progressCoursAxiale; }
        public int getProgressCen() { return progressCoursCentrale; }
    }
}
