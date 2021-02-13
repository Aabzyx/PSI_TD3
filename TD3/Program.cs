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
            Console.WriteLine(image.toString());
            image.NuancesDeGris();
            /*for (int x = 0; x < image.Pixels.GetLength(0); x++)
            {
                for (int y = 0; y < image.Pixels.GetLength(1); y++)
                {
                    Console.Write(image.Pixels[x, y].toString());
                }
                Console.WriteLine();
            }*/

            Console.WriteLine(image.toString());

            image.From_Image_To_File(sortie);
        }

        static void Main(string[] args)
        {
            string fileName = "./coco.bmp";
            byte[]tab = File.ReadAllBytes(fileName);

            MyImage image = new MyImage(fileName);
            byte[] endian = image.Convertir_Int_To_Endian(320, 4);
            for(int x = 0; x < 4; x++)
            {
                Console.Write(endian[x] + " ");
            }

            //NuancesDeGris(fileName, "./Gris1.bmp");

            Console.WriteLine("\nTerminé !");
            Console.ReadKey(true);
        }
    }
}
