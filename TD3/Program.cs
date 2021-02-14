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
        static void NuancesDeGris(string entree, string sortie)
        {
            MyImage image = new MyImage(entree);

            image.NuancesDeGris();

            image.From_Image_To_File(sortie);
        }

        static void Main(string[] args)
        {
            string fileName = "./coco.bmp";
            NuancesDeGris(fileName, "./Gris1.bmp");

            Console.WriteLine("\nTerminé !");
            Console.ReadKey(true);
        }
    }
}
