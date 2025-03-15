using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RailFence : ICryptographicTechnique<string, int>
    {
        public int Analyse(string plainText, string cipherText)
        {
            // throw new NotImplementedException();

            int key = 1;
            for (int i = 1; i <= plainText.Length; i++)
            {

                if (Encrypt(plainText, i).ToLower() == cipherText.ToLower())
                {
                    key = i;
                    break;
                }


            }

            return key;
        }

        public string Decrypt(string cipherText, int key)
        {
            //throw new NotImplementedException();
            string plainText = "";
            double col = Math.Ceiling((double)cipherText.Length / key);
            int row = key;

            char[,] matrix = new char[row, (int)col];


            int x = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (x < cipherText.Length)
                    {
                        matrix[i, j] = cipherText[x];
                        x++;

                    }
                    else
                    {
                        matrix[i, j] = '\0';
                    }
                }
            }


            for (int j = 0; j < col; j++)
            {
                for (int i = 0; i < row; i++)
                {
                    if (matrix[i, j] != '\0')
                    {
                        plainText += matrix[i, j];

                    }
                }
            }

            return plainText;
        }
        public string Encrypt(string plainText, int key)
        {
            // throw new NotImplementedException();
            string encryptedText = "";
            plainText = plainText.Replace(" ", "");
            int col = plainText.Length;
            int row = key;
            int lettersCount = 0;
            char[,] matrix = new char[row, col];



            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    if (lettersCount != col)
                    {
                        matrix[j, i] = plainText[lettersCount];
                        lettersCount++;
                    }
                }
            }


            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (matrix[i, j] != '\0')
                    {
                        encryptedText += matrix[i, j];

                    }
                }
            }


            return encryptedText;
        }
    }
}