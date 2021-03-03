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

        static void Agrandir(string entree, string sortie)
        {
            Console.WriteLine("Veuillez saisir un coefficient de zoom de minimum supérieur strict à 1");
            double coef = 0;
            while (coef < 1)
            {
                coef = Convert.ToDouble(Console.ReadLine());
            }
            MyImage image = new MyImage(entree);
            image.Agrandir(coef);
            image.From_Image_To_File(sortie);
        }

        static void Retrecir(string entree, string sortie)
        {
            Console.WriteLine("Veuillez saisir un coefficient de retrecissement compris entre 0 et 1 exclut");
            double coef = -1;
            while (coef <= 0 || coef >= 1)
            {
                coef = Convert.ToDouble(Console.ReadLine());
            }
            MyImage image = new MyImage(entree);
            image.Retrecir(coef);
            image.From_Image_To_File(sortie);
        }

        static void RotateRemarquable(string entree, string sortie)
        {
            Console.Write("Veuillez saisir un angle de rotation remarquable : ");
            int angle = Convert.ToInt32(Console.ReadLine());
            MyImage image = new MyImage(entree);
            image.RotateRemarquable(angle);
            image.From_Image_To_File(sortie);
        }

        static void Rotate(string entree, string sortie)
        {
            Console.Write("Veuillez saisir un angle de rotation : ");
            double angle = Convert.ToDouble(Console.ReadLine());
            angle = Math.PI * angle/180;
            MyImage image = new MyImage(entree);
            image.Rotate(angle);
            image.From_Image_To_File(sortie);
        }

        static void Main(string[] args)
        {
            //NuancesDeGris("./coco.bmp", "./Gris.bmp");
            //NoirEtBlanc("./coco.bmp", "./Binaire.bmp");
            //Agrandir("./coco.bmp","./zoom.bmp");
            //Retrecir("./coco.bmp", "./zoom.bmp");
            //RotateRemarquable("./coco.bmp", "./GROS270.bmp");
            Rotate("./coco.bmp", "./rotateRandom.bmp");
            Retrecir("./coco.bmp", "./zoom.bmp");
            //Rotate("./coco.bmp", "./GROS270.bmp");

            Console.WriteLine("\nTerminé !");
            Console.ReadKey(true);
        }
    }
}