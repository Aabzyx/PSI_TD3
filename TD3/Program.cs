using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TD3
{
    class Program
    {
        /// <summary>
        /// Fonction extrêmement simple, on crée une image de classe MyImage grâce au chemin du fichier entré,
        /// puis on utilise la fonction NuancesDeGris interne à la classe MyImage, pour finalement exporter la nouvelle
        /// image sous le nom et chemin de sortie donné en paramètre
        /// </summary>
        /// <param name="entree">Chemin du fichier bitmap à passer en gris</param>
        /// <param name="sortie">Chemin de sortie et nom du fichier sous lequel on va l'exporter</param>
        static void NuancesDeGris(string entree, string sortie)
        {
            MyImage image = new MyImage(entree);
            image.NuancesDeGris();
            image.From_Image_To_File(sortie);
        }

        //Identique à NuancesDeGris
        static void NoirEtBlanc(string entree, string sortie)
        {
            MyImage image = new MyImage(entree);
            image.NoirEtBlanc();
            image.From_Image_To_File(sortie);
        }

        static void Main(string[] args)
        {
            NuancesDeGris("./coco.bmp", "./Gris.bmp");
            NoirEtBlanc("./coco.bmp", "./Binaire.bmp");

            Console.WriteLine("\nTerminé !");
            Console.ReadKey(true);
        }
    }
}
