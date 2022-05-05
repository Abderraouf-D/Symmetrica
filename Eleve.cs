using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet2Cp
{
    public class Eleve
    {

       private  string nom { get; }
        private int progressCours { get; set; }

        public Eleve (string nom , int progressCours)
        {
            this .nom = nom;
            this .progressCours = progressCours;
        }
    }
}
