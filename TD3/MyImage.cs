using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TD3
{
    class MyImage //test
    {
        //Variables de classe
        private string typeImage;
        private int tailleDuFichier;
        private int tailleOffset;
        private int largeurImage;
        private int hauteurImage;
        private int nombreBytesParCouleur;
        private Pixel[,] pixels;

        /// <summary>
        /// Constructeur de la classe, on récupère les informations essentielles de l'offset
        /// et on les convertit en entier pour les stocker dans les variables de classe
        /// Puis on créé une matrice de Pixels qu'on remplit à l'aide de la fonction du même nom
        /// </summary>
        /// <param name="myFile">Chemin du fichier à étudier</param>
        public MyImage(string myFile)
        {
            byte[] image = File.ReadAllBytes(myFile);
            typeImage = TypeDImage(image);
            tailleDuFichier = Convertir_Endian_To_Int(image, 4, 2);
            tailleOffset = Convertir_Endian_To_Int(image, 4, 10);
            largeurImage = Convertir_Endian_To_Int(image, 4, 18);
            hauteurImage = Convertir_Endian_To_Int(image, 4, 22);
            nombreBytesParCouleur = Convertir_Endian_To_Int(image, 2, 28);
            pixels = new Pixel[hauteurImage, largeurImage];
            Remplir_pixels(pixels, image);
        }

        /// <summary>
        /// On va remplir de manière automatique la matrice de pixels en commençant
        /// au bon indice (fin de l'offset), et en faisant attention à deux détails :
        ///     - En bitmap, les pixels sont dans l'ordre BVR (ou BGR), il nous faut donc
        /// les inverser pour rester dans l'ordre classique RVB lors de l'utilisation de la matrice
        ///     - Si l'image a une largeur n'étant pas un multiple de 4, il y aura des bytes
        /// non utilisés, qu'il faudra ne pas prendre en compte à l'import.
        /// </summary>
        /// <param name="pixels">Matrice à remplir</param>
        /// <param name="image">Tableau de bytes à partir duquel on va remplir la matrice</param>
        public void Remplir_pixels(Pixel[,] pixels, byte[] image)
        {
            int indice = Convert.ToInt32(tailleOffset);
            int valeursInutiles = largeurImage % 4;
            for (int l = 0; l < pixels.GetLength(0); l++)
            {
                for (int c = 0; c < pixels.GetLength(1); c++)
                {
                    Pixel nouveauPixel = new Pixel(image[indice + 2], image[indice + 1], image[indice]);
                    pixels[l, c] = nouveauPixel;
                    indice += 3;
                }
                //Pour ignorer les bytes inutiles, on incrémentera seulement l'indice parcourant le tableau
                //lorsque l'on atteindra la fin d'une ligne
                if (valeursInutiles != 0)
                {
                    if (valeursInutiles == 3)
                    {
                        indice++;
                    }
                    if (valeursInutiles == 2)
                    {
                        indice += 2;
                    }
                    if (valeursInutiles == 1)
                    {
                        indice += 3;
                    }
                }
            }
        }

        /// <summary>
        /// La fonction est longue mais très simple dans son fonctionnement :
        /// nous allons créer un tableau de bytes que nous remplirons avec les informations
        /// décrivant l'image exportée selon les paramètres bitmap.
        /// Puis nous ferons l'exact inverse de la fonction Remplir_pixels en remplissant
        /// le tableau à partir de la matrice de Pixels, en faisant attention à replacer les
        /// bytes inutiles.
        /// Puis on exporte le tout grâce à la fonction WriteAllBytes
        /// </summary>
        /// <param name="file">Chemin et nom du fichier utilisé pour l'export</param>
        public void From_Image_To_File(string file)
        {
            byte[] returned = new byte[tailleDuFichier];
            returned[0] = Convert.ToByte(66);
            returned[1] = Convert.ToByte(77);
            byte[] tailleDuFichierEndian = Convertir_Int_To_Endian(tailleDuFichier, 4);
            for (int i = 2; i < 6; i++)
            {
                returned[i] = tailleDuFichierEndian[i - 2];
            }
            byte[] tailleOffsetEndian = Convertir_Int_To_Endian(tailleOffset, 4);
            for (int i = 10; i < 14; i++)
            {
                returned[i] = tailleOffsetEndian[i - 10];
            }
            byte[] tailleHeader = Convertir_Int_To_Endian(tailleOffset - 14, 4);
            for (int i = 14; i < 18; i++)
            {
                returned[i] = tailleHeader[i - 14];
            }
            byte[] largeurImageEndian = Convertir_Int_To_Endian(largeurImage, 4);
            for (int i = 18; i < 22; i++)
            {
                returned[i] = largeurImageEndian[i - 18];
            }
            byte[] hauteurImageEndian = Convertir_Int_To_Endian(hauteurImage, 4);
            for (int i = 22; i < 26; i++)
            {
                returned[i] = hauteurImageEndian[i - 22];
            }
            returned[26] = Convert.ToByte(1);
            byte[] nombreBytesParCouleurEndian = Convertir_Int_To_Endian(nombreBytesParCouleur, 2);
            for (int i = 28; i < 30; i++)
            {
                returned[i] = nombreBytesParCouleurEndian[i - 28];
            }
            byte[] tailleImageEndian = Convertir_Int_To_Endian(tailleDuFichier - tailleOffset, 4);
            for (int i = 34; i < 38; i++)
            {
                returned[i] = tailleImageEndian[i - 34];
            }

            int cpt = tailleOffset;
            int valeursInutiles = largeurImage % 4;
            for (int i = 0; i < hauteurImage; i++)
            {
                for (int j = 0; j < largeurImage; j++)
                {
                    returned[cpt] = Convert.ToByte(pixels[i, j].Blue);
                    returned[cpt + 1] = Convert.ToByte(pixels[i, j].Green);
                    returned[cpt + 2] = Convert.ToByte(pixels[i, j].Red);
                    cpt += 3;
                }
                if (valeursInutiles != 0)
                {
                    if (valeursInutiles == 3)
                    {
                        cpt++;
                    }
                    if (valeursInutiles == 2)
                    {
                        cpt += 2;
                    }
                    if (valeursInutiles == 1)
                    {
                        cpt += 3;
                    }
                }
            }
            File.WriteAllBytes(file, returned);
        }

        //Fonction retournant sous forme de string les informations essentielles contenues dans l'offset
        public string toString()
        {
            string returned = "";
            returned += "Type de l'image : " + typeImage + "\n";
            returned += "Taille de l'image : " + Convert.ToString(tailleDuFichier) + "\n";
            returned += "Taille de l'offset : " + Convert.ToString(tailleOffset) + "\n";
            returned += "Largeur de l'image : " + Convert.ToString(largeurImage) + "\n";
            returned += "Hauteur de l'image : " + Convert.ToString(hauteurImage) + "\n";
            returned += "Nomvre de bytes par couleur : " + Convert.ToString(nombreBytesParCouleur) + "\n";
            return returned;
        }

        //Fonction extrêmement basique, identique à celle utilisée depuis un an
        public string AffichageMatricePixel()
        {
            string returned = "";
            for (int x = 0; x < pixels.GetLength(0); x++)
            {
                for (int y = 0; y < pixels.GetLength(1); y++)
                {
                    returned += pixels[x, y].toString();
                }
                returned += "\n";
            }
            return returned;
        }

        //Ici, on vérifie que l'image est bien de type BM, et on return "inconnu" dans tous les autres cas
        private string TypeDImage(byte[] image)
        {
            string returned = "";
            if (image[0] == 66 && image[1] == 77)
            {
                returned = "BM";
            }
            else returned = "Inconnu";
            return returned;
        }

        /// <summary>
        /// On va récupérer chaque byte sur lequel l'entier est codé, multiplier ledit byte
        /// à la puissance de 256 à laquelle il est associé, puis convertir cette valeur en int
        /// et l'ajouter au résultat final
        /// </summary>
        /// <param name="image">Tableau de bytes obtenu avec le ReadAllBytes</param>
        /// <param name="nombreOctets">Nombre d'octets sur lequel l'entier est codé</param>
        /// <param name="indiceDepart">Indice du byte du tableau Image à partir duquel le nombre est codé</param>
        /// <returns>Le même nombre mais entier</returns>
        public int Convertir_Endian_To_Int(byte[] image, int nombreOctets, int indiceDepart)
        {
            int returned = 0;
            for (int x = 0; x < nombreOctets; x++)
            {
                //NE PAS OUBLIER LA CONVERSION DE LA PUISSANCE EN INT, cela a été source de nombreux problèmes
                returned += Convert.ToInt32(image[indiceDepart + x] * (int)Math.Pow(256, x));
            }
            return returned;
        }

        /// <summary>
        /// Ici, on va diviser l'entier entré par chaque puissance de 256,
        /// de la puissance 'nombreOctets -1' à 0, attribuer le quotient au byte correspondant
        /// dans le tableau de bytes créé, et répéter l'opération avec le reste de la division
        /// </summary>
        /// <param name="val">Entier à convertir</param>
        /// <param name="nombreOctets">Nombre d'octets sur lequel on code la valeur</param>
        /// <returns>Le même nombre sous forme de tableau de bytes</returns>
        public byte[] Convertir_Int_To_Endian(int val, int nombreOctets)
        {
            byte[] returned = new byte[nombreOctets];
            for (int x = nombreOctets - 1; x >= 0; x--)
            {
                int puissance = (int)Math.Pow(256, x);
                returned[x] = Convert.ToByte(val / puissance);
                val = (int)(val % puissance);
            }
            return returned;
        }

        public void RotateRemarquable(int angle)
        {
            if (angle == 180)
            {
                Pixel[,] nouvelleMatrice = new Pixel[hauteurImage, largeurImage];
                for (int x = 0; x < hauteurImage; x++)
                {
                    for (int y = 0; y < largeurImage; y++)
                    {
                        nouvelleMatrice[x, y] = pixels[hauteurImage - 1 - x, largeurImage - 1 - y];
                    }
                }
                pixels = nouvelleMatrice;

            }
            else if (angle == 270 || angle == 90)
            {
                Pixel[,] nouvelleMatrice = new Pixel[largeurImage, hauteurImage];
                if (angle == 90)
                {
                    for (int x = 0; x < largeurImage; x++)
                    {
                        for (int y = 0; y < hauteurImage; y++)
                        {
                            nouvelleMatrice[x, y] = pixels[y, x];
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < largeurImage; x++)
                    {
                        for (int y = 0; y < hauteurImage; y++)
                        {
                            nouvelleMatrice[x, y] = pixels[hauteurImage - 1 - y, x];
                        }
                    }
                }
                int retenue = hauteurImage;
                hauteurImage = largeurImage;
                largeurImage = retenue;
                pixels = nouvelleMatrice;
            }
            else Console.WriteLine("pafétrodur");
        }

        public void Rotate(int angle)
        {

        }

        public void Miroir()
        {
            Pixel[,] newImage = new Pixel[largeurImage, hauteurImage];
            for (int l = 0; l < pixels.GetLength(0); l++)
            {
                for (int c = 0; c < pixels.GetLength(1); c++)
                {
                    newImage[l, c] = pixels[l, largeurImage - c - 1];
                }
            }
            pixels = newImage;
        }

        public void Agrandir(double coef)
        {
            int new_largeur = Convert.ToInt32(Math.Round(coef * (double)largeurImage));
            int new_hauteur = Convert.ToInt32(Math.Round(coef * (double)hauteurImage));
            int nbr_new_hauteur = new_hauteur - hauteurImage;
            int nbr_new_largeur = new_largeur - largeurImage;
            int[] largeur = new int[largeurImage];
            int[] hauteur = new int[hauteurImage];
            Random random = new Random();
            if (coef < 2)
            {
                int compteur = 0;
                while (compteur != nbr_new_hauteur)
                {
                    int index = random.Next(0, hauteur.Length - 1);
                    if (hauteur[index] == 0)
                    {
                        hauteur[index]++;
                        compteur++;
                    }
                }
                compteur = 0;
                while (compteur != nbr_new_largeur)
                {
                    int index = random.Next(0, largeur.Length - 1);
                    if (largeur[index] == 0)
                    {
                        largeur[index]++;
                        compteur++;
                    }
                }
            }
            else
            {
                int compteur = 0;
                for (int i = 0; i < hauteur.Length; i++)
                {
                    hauteur[i] = Convert.ToInt32(Math.Floor(coef)) - 1;
                }
                int case_en_plus = new_hauteur - Convert.ToInt32(Math.Floor(coef) * (double)hauteurImage);
                while (compteur != case_en_plus)
                {
                    int index = random.Next(0, hauteur.Length - 1);
                    if (hauteur[index] == Convert.ToInt32(Math.Floor(coef)) - 1)
                    {
                        hauteur[index]++;
                        compteur++;
                    }
                }
                compteur = 0;
                for (int i = 0; i < largeur.Length; i++)
                {
                    largeur[i] = Convert.ToInt32(Math.Floor(coef)) - 1;
                }
                case_en_plus = new_largeur - Convert.ToInt32(Math.Floor(coef) * (double)largeurImage);
                while (compteur != case_en_plus)
                {
                    int index = random.Next(0, largeur.Length - 1);
                    if (largeur[index] == Convert.ToInt32(Math.Floor(coef)) - 1)
                    {
                        largeur[index]++;
                        compteur++;
                    }
                }
            }
            Pixel[,] newimage = new Pixel[new_hauteur, new_largeur];
            int indice_hauteur = 0;
            int indice_largeur = 0;
            for (int l = 0; l < pixels.GetLength(0); l++)
            {
                for (int c = 0; c < pixels.GetLength(1); c++)
                {
                    if (hauteur[l] > 0 && largeur[c] > 0)
                    {
                        for (int i = 0; i <= largeur[c]; i++)
                        {
                            for (int j = 0; j <= hauteur[l]; j++)
                            {
                                newimage[indice_hauteur + j, indice_largeur] = pixels[l, c];
                            }
                            indice_largeur++;
                        }
                    }
                    else if (hauteur[l] > 0 && largeur[c] == 0)
                    {
                        for (int i = 0; i <= hauteur[l]; i++)
                        {
                            newimage[indice_hauteur + i, indice_largeur] = pixels[l, c];
                        }
                        indice_largeur++;
                    }
                    else if (hauteur[l] == 0 && largeur[c] > 0)
                    {
                        for (int i = 0; i <= largeur[c]; i++)
                        {
                            newimage[indice_hauteur, indice_largeur] = pixels[l, c];
                            indice_largeur++;
                        }
                    }
                    else if (hauteur[l] == 0 && largeur[c] == 0)
                    {
                        newimage[indice_hauteur, indice_largeur] = pixels[l, c];
                        indice_largeur++;
                    }
                }
                indice_hauteur += hauteur[l] + 1;
                indice_largeur = 0;
            }
            largeurImage = new_largeur;
            hauteurImage = new_hauteur;
            tailleDuFichier = new_largeur * new_hauteur * 3 + tailleOffset;
            pixels = newimage;
        }

        public void Retrecir(double coef)
        {
            int new_largeur = Convert.ToInt32(Math.Round(coef * (double)largeurImage));
            int new_hauteur = Convert.ToInt32(Math.Round(coef * (double)hauteurImage));
            bool[] largeur = new bool[largeurImage];
            bool[] hauteur = new bool[hauteurImage];
            Random random = new Random();
            int compteur = 0;
            while (compteur != new_hauteur)
            {
                int index = random.Next(0, hauteur.Length);
                if (hauteur[index] == false)
                {
                    hauteur[index] = true;
                    compteur++;
                }
            }
            compteur = 0;
            while (compteur != new_largeur)
            {
                int index = random.Next(0, largeur.Length);
                if (largeur[index] == false)
                {
                    largeur[index] = true;
                    compteur++;
                }
            }
            Pixel[,] newimage = new Pixel[new_hauteur, new_largeur];
            int indice_hauteur = 0;
            int indice_largeur = 0;
            for (int l = 0; l < pixels.GetLength(0); l++)
            {
                if (hauteur[l] == true)
                {
                    for (int c = 0; c < pixels.GetLength(1); c++)
                    {
                        if (largeur[c] == true)
                        {
                            newimage[indice_hauteur, indice_largeur] = pixels[l, c];
                            indice_largeur++;
                        }
                    }
                    indice_hauteur++;
                    indice_largeur = 0;
                }
            }
            largeurImage = new_largeur;
            hauteurImage = new_hauteur;
            tailleDuFichier = new_hauteur * new_largeur * 3 + tailleOffset;
            pixels = newimage;
        }

        /// <summary>
        /// Pour faire passer l'image en nuances de gris,
        /// on applique simplement sur chaque couleur de chaque Pixel
        /// la moyenne des valeurs des couleurs du Pixel en question
        /// </summary>
        public void NuancesDeGris()
        {
            for (int x = 0; x < pixels.GetLength(0); x++)
            {
                for (int y = 0; y < pixels.GetLength(1); y++)
                {
                    int rvbGRIS = (pixels[x, y].Red + pixels[x, y].Green + pixels[x, y].Blue) / 3;
                    Pixel newPixel = new Pixel(rvbGRIS, rvbGRIS, rvbGRIS);
                    pixels[x, y] = newPixel;
                }
            }
        }

        /// <summary>
        /// Très similaire à la fonction NuancesDeGris, mais ici on rendra le pixel noir uniquement
        /// si la moyenne des trois couleurs est inférieure strictement à 128 (valeur médiane de 0-255)
        /// et blanc si elle est supérieure à 128
        /// </summary>
        public void NoirEtBlanc()
        {
            for (int x = 0; x < pixels.GetLength(0); x++)
            {
                for (int y = 0; y < pixels.GetLength(1); y++)
                {
                    int moyenne = (pixels[x, y].Red + pixels[x, y].Green + pixels[x, y].Blue) / 3;
                    if (moyenne < 128)
                    {
                        Pixel newPixel = new Pixel(0, 0, 0);
                        pixels[x, y] = newPixel;
                    }
                    else
                    {
                        Pixel newPixel = new Pixel(255, 255, 255);
                        pixels[x, y] = newPixel;
                    }
                }
            }
        }
    }
}
