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
            //throw new NotImplementedException();
            SortedDictionary<int, int> sortDict = new SortedDictionary<int, int>();
            cipherText = cipherText.ToLower();
            int countPlainText = plainText.Length;

            for (int col = 1; col < 10; col++)
            {
                int index = 0;
                int row = (int)Math.Ceiling((double)countPlainText / col);

                // write plaintext in array
                string[,] arr = new string[row, col];
                for (int R = 0; R < row; R++)
                {
                    for (int C = 0; C < col; C++)
                    {
                        if (index < plainText.Length)
                        {
                            arr[R, C] = plainText[index].ToString();
                            index++;
                        }
                        else
                        {
                            arr[R, C] = "";
                        }
                    }
                }
                //make all colums in string and add to list
                List<string> newLst = new List<string>();
                for (int k = 0; k < col; k++)
                {
                    string cipher = "";

                    for (int j = 0; j < row; j++)
                    {
                        cipher += arr[j, k];
                    }

                    newLst.Add(cipher);
                }

                // check if column mawgod in cipherText
                bool mawgod = true;
                string cipherr = (string)cipherText.Clone();
                for (int c = 0; c < newLst.Count; c++)
                {
                    int position = cipherr.IndexOf(newLst[c]);
                    if (position != -1)
                    {
                        sortDict.Add(position, c + 1);
                        cipherr.Replace(newLst[c], "#");
                    }
                    else
                    {
                        mawgod = false;
                        break;
                    }
                }
                // el7amdula key tmam
                if (mawgod == true)
                    break;
            }

            // find order of key
            List<int> final = new List<int>(new int[sortDict.Count]);
            int sequentialIndex = 1;
            foreach (var pair in sortDict.OrderBy(kvp => kvp.Key))
            {
                final[pair.Value - 1] = sequentialIndex;
                sequentialIndex++;
            }

            return final;
        }



        public string Decrypt(string cipherText, List<int> key)
        {
            //throw new NotImplementedException();
            cipherText = cipherText.ToLower();
            int col = key.Count();
            int count_cipherText = cipherText.Length;
            int row;
            if (count_cipherText % col == 0)
            {
                row = (count_cipherText / col);
            }
            else
            {
                row = (count_cipherText / col) + 1;
            }
            string decrept = "";
            object[,] arr = new object[row + 1, col];
            int next = 1, cipher = 0;

            // make first row with key
            for (int i = 0; i < col; i++)
            {
                arr[0, i] = key[i];
            }

            // make cipherText in array  
            while (next <= col)
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

            for (int i = 1; i < row + 1; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (arr[i, j] != null)
                    {
                        decrept += (char)arr[i, j];
                    }
                }
            }
            return decrept;
        }

        public string Encrypt(string plainText, List<int> key)
        {
            // throw new NotImplementedException();
            int col = key.Count();
            int count = plainText.Length;
            int row, remender = 0;
            if (count % col == 0)
            {
                row = (count / col);
            }
            else
            {
                row = (count / col) + 1;
                // lw fi 7agat fadia n7othaa 'x'
                remender = (row * col) - count;
                for (int i = 0; i < remender; i++)
                {
                    plainText += "x";
                }
            }
            string enc = "";
            object[,] arr = new object[row + 1, col];
            int next = 0;
            for (int i = 0; i < row + 1; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    // awl row han7ot fih key 
                    if (i == 0)
                    {
                        arr[i, j] = key[j];
                    }
                    else
                    {
                        arr[i, j] = plainText[next];
                        next++;
                    }
                }
            }
            next = 1; // number of columns
            while (next <= col)
            {
                for (int i = 0; i < col; i++)
                {
                    // to print in order of columns 1,2,3,......
                    if ((int)arr[0, i] == next)
                    {
                        for (int j = 1; j < row + 1; j++)
                        {
                            enc += (char)arr[j, i];
                        }
                        next++;
                    }
                }
            }
            return enc.ToUpper();
        }
    }
}