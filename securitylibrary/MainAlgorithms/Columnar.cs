using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Columnar : ICryptographicTechnique<string, List<int>>
    {
        public List<int> Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText, List<int> key)
        {
            //throw new NotImplementedException();
            cipherText = cipherText.ToLower();
            int col = key.Count();
            int count = cipherText.Length;
            int row;
            if (count % col == 0)
            {
                row = (count / col);
            }
            else
            {
                row = (count / col) + 1;
            }
            char[] dec = new char[cipherText.Length];
            object[,] arr = new object[row + 1, col];
            int next = 1 , cipher =0;

            for (int i = 0; i < col; i++)
            {
                arr[0,i] = key[i];
            }

            while(next <= col)
            {
                for (int i = 0; i < col; i++)
                {
                    if ((int)arr[0, i] == next)
                    {
                        next++;
                        for (int j = 1; j < row + 1; j++)
                        {
                            if (cipher < cipherText.Length)
                            {
                                arr[j, i] = cipherText[cipher];
                                cipher++;
                            }
                            else
                            {
                                arr[j, i] = null; 
                            }

                        }                      
                    }
                    
                }
            }
            cipher = 0;
            for (int i = 1; i < row + 1; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (arr[i, j] != null) 
                    {
                        dec[cipher] = (char)arr[i, j];
                        cipher++;
                    }
                }
            }
            return new string(dec);
        }

        public string Encrypt(string plainText, List<int> key)
        {
           // throw new NotImplementedException();
            int col = key.Count();
            int count = plainText.Length;
            int row , remender = 0;
            if (count % col == 0)
            {
                row = (count / col) ;
            }
            else
            {
                row = (count / col) + 1;
                remender = (row * col) - count;
                for (int i = 0; i < remender; i++)
                {
                    plainText += "x";
                }
            }
            char[] enc = new char[plainText.Length];
            object[,] arr = new object[row+1, col];
            int next = 0;
            for (int i = 0; i < row + 1; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (i == 0)
                    {
                        arr[i, j] = key[j];
                    }
                    else
                    {
                        arr [i, j] = plainText[next];
                        next++;
                    }
                }
            }
            next = 1;
            int plan = 0;
            while (next <= col)
            {
                for (int i = 0; i < col; i++)
                {
                    if ((int)arr[0, i] == next)
                    {
                        for (int j = 1; j < row + 1; j++)
                        {
                            enc[plan] = (char)arr[j, i];
                            plan++;
                        }
                        next++;
                    }
                }
            }

            return new string(enc).ToUpper();
        }
    }
}
