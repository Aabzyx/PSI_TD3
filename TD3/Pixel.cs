using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD3
{
    class Pixel
    {
        private int red; //Variables de classe
        private int green;
        private int blue;

        public Pixel(int red, int green, int blue) //Constructeur de la classe
        {
            this.blue = blue;
            this.green = green;
            this.red = red;
        }

        // Propriétés des variables, toutes en lecture seule
        public int Red
        {
            get { return red; }
        }

        public int Green
        {
            get { return green; }
        }

        public int Blue
        {
            get { return blue; }
        }

        // Fonction toString décrivant les couleurs d'un Pixel
        public string toString()
        {
            return red + " " + green + " " + blue + " ";
        }
    }
}
