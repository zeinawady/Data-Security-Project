using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {

            throw new NotImplementedException();
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {


            //Validating the passed text
            for (int i = 0; i < plainText.Count(); i++)
            {
                if (plainText.ElementAt(i) > 25 || plainText.ElementAt(i) < 0)
                {
                    throw new Exception("Plain text includes invalid values");
                }
            }

            //2*2 Encryption 
            if (key.Count() == 4)
            {
                //validating the determinant
                int determinant;
                determinant = key.ElementAt(0) * key.ElementAt(3) - key.ElementAt(1) * key.ElementAt(2);
                if (determinant == 0 || determinant % 2 == 0 || determinant % 13 == 0)
                {
                    throw new Exception("Wrong key matrix");
                }
                else
                {
                    if (plainText.Count() % 2 != 0)
                    {
                        // Adding an x letter if the #letters is odd
                        plainText.Add(25);
                    }


                    List<int> Cipher2by2 = new List<int> { };

                    for (int i = 0; i < plainText.Count(); i += 2)
                    {
                        int firstElement = (key.ElementAt(0) * plainText.ElementAt(i) + key.ElementAt(1) * plainText.ElementAt(i + 1)) % 26;
                        int secondElement = (key.ElementAt(2) * plainText.ElementAt(i) + key.ElementAt(3) * plainText.ElementAt(i + 1)) % 26;
                        Cipher2by2.Add(firstElement);
                        Cipher2by2.Add(secondElement);
                    }
                    return Cipher2by2;

                }

            }

            //3*3 Encryption
            else if (key.Count() == 9)
            {
                //Getting the determinant
                int determinant3 = (key.ElementAt(0) * key.ElementAt(4) * key.ElementAt(8)) +
                                   (key.ElementAt(1) * key.ElementAt(5) * key.ElementAt(6)) +
                                   (key.ElementAt(2) * key.ElementAt(3) * key.ElementAt(7)) -
                                   (key.ElementAt(6) * key.ElementAt(4) * key.ElementAt(2)) -
                                   (key.ElementAt(7) * key.ElementAt(5) * key.ElementAt(0)) -
                                   (key.ElementAt(8) * key.ElementAt(3) * key.ElementAt(1));
                //validating the determinant
                if (determinant3 == 0 || determinant3 % 2 == 0 || determinant3 % 13 == 0)
                {
                    throw new Exception("Wrong key matrix");
                }
                while (plainText.Count() % 3 != 0)
                {
                    plainText.Add(25);
                }



                List<int> Cipher3by3 = new List<int> { };

                for (int i = 0; i < plainText.Count(); i += 3)
                {
                    int firstElement = (key.ElementAt(0) * plainText.ElementAt(i) + key.ElementAt(1) * plainText.ElementAt(i + 1) + key.ElementAt(2) * plainText.ElementAt(i + 2)) % 26;
                    int secondElement = (key.ElementAt(3) * plainText.ElementAt(i) + key.ElementAt(4) * plainText.ElementAt(i + 1) + key.ElementAt(5) * plainText.ElementAt(i + 2)) % 26;
                    int thirdElement = (key.ElementAt(6) * plainText.ElementAt(i) + key.ElementAt(7) * plainText.ElementAt(i + 1) + key.ElementAt(8) * plainText.ElementAt(i + 2)) % 26;

                    Cipher3by3.Add(firstElement);
                    Cipher3by3.Add(secondElement);
                    Cipher3by3.Add(thirdElement);

                }
                return Cipher3by3;
            }
            else
                throw new NotImplementedException();

        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

    }
}
