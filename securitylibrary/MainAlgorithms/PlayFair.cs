using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SecurityLibrary
{
    public class PlayFair : ICryptographic_Technique<string, string>
    {
        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();

            char[,] keyMatrix = getMatrix(key);
            List<string> pairsOfCT = new List<string>();

            // split cipherText to pairs
            for (int i = 0; i < cipherText.Length; i += 2)
            {
                pairsOfCT.Add(cipherText.Substring(i, 2));
            }

            List<string> decryptedPairs = new List<string>();
            foreach (string item in pairsOfCT)
            {
                int firstRow = 0, firstCol = 0, secondRow = 0, secondCol = 0;
                // search in matrix
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (item[0] == keyMatrix[i, j])
                        {
                            firstRow = i;
                            firstCol = j;
                        }
                        if (item[1] == keyMatrix[i, j])
                        {
                            secondRow = i;
                            secondCol = j;
                        }
                    }
                }

                string dPair = "";
                if (firstRow == secondRow) // the same row
                {
                    if (firstCol == 0)
                        firstCol = 5;
                    if (secondCol == 0)
                        secondCol = 5;

                    dPair += keyMatrix[firstRow, (firstCol - 1) % 5];
                    dPair += keyMatrix[secondRow, (secondCol - 1) % 5];
                }
                else if (firstCol == secondCol) // the same column
                {
                    if (firstRow == 0)
                        firstRow = 5;
                    if (secondRow == 0)
                        secondRow = 5;

                    dPair += keyMatrix[(firstRow - 1) % 5, firstCol];
                    dPair += keyMatrix[(secondRow - 1) % 5, secondCol];
                }
                else // rectangle
                {
                    dPair += keyMatrix[firstRow, secondCol];
                    dPair += keyMatrix[secondRow, firstCol];
                }
                decryptedPairs.Add(dPair);
            }
            string decryptedText = string.Join("", decryptedPairs);
            decryptedText = checkDecryptedText(decryptedText);
            return decryptedText;
        }

        public string checkDecryptedText(string decryptedText)
        {
            decryptedText = decryptedText.ToUpper().Replace("J", "I");
            string ct = decryptedText[0].ToString();
            for (int i = 1; i < decryptedText.Length; i++)
            {
                // remove x between the duplicated chars
                if (decryptedText[i] == 'X' && decryptedText[i - 1] == decryptedText[i + 1] && i % 2 != 0)
                {
                    continue;
                }
                ct += decryptedText[i];
            }
            // remove x which in the last index
            if (ct.EndsWith("X") && ct.Length % 2 == 0)
            {
                ct = ct.Remove(ct.Length - 1);
            }

            return ct;
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();

            char[,] keyMatrix = getMatrix(key);

            plainText = checkPlainText(plainText);

            List<string> pairsOfPT = new List<string>();
            // split plainText to pairs
            for (int i = 0; i < plainText.Length; i += 2)
            {
                pairsOfPT.Add(plainText.Substring(i, 2));
            }

            List<string> encryptedPairs = new List<string>();
            foreach (string item in pairsOfPT)
            {
                int firstRow = 0, firstCol = 0, secondRow = 0, secondCol = 0;
                // search in matrix
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (item[0] == keyMatrix[i, j])
                        {
                            firstRow = i;
                            firstCol = j;
                        }
                        if (item[1] == keyMatrix[i, j])
                        {
                            secondRow = i;
                            secondCol = j;
                        }
                    }
                }
                string ePair = "";
                if (firstRow == secondRow) // the same row
                {
                    ePair += keyMatrix[firstRow, (firstCol + 1) % 5];
                    ePair += keyMatrix[secondRow, (secondCol + 1) % 5];
                }
                else if (firstCol == secondCol) // the same column
                {
                    ePair += keyMatrix[(firstRow + 1) % 5, firstCol];
                    ePair += keyMatrix[(secondRow + 1) % 5, secondCol];
                }
                else // rectangle
                {
                    ePair += keyMatrix[firstRow, secondCol];
                    ePair += keyMatrix[secondRow, firstCol];
                }
                encryptedPairs.Add(ePair);
            }
            return string.Join("", encryptedPairs);
        }

        private char[,] getMatrix(string key)
        {
            key = key.ToUpper().Replace('J', 'I');
            char[,] matKey = new char[5, 5];
            string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";
            string wordInMat = "";

            // adding key to the matrix
            foreach (char ch in key)
            {
                if (!(wordInMat.Contains(ch)))
                {
                    wordInMat += ch;
                }
            }
            // adding remaning alpha to the matrix
            foreach (char ch in alpha)
            {
                if (!(wordInMat.Contains(ch)))
                {
                    wordInMat += ch;
                }
            }

            // generating the matrix
            int index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    matKey[i, j] = wordInMat[index];
                    index++;
                }
            }
            return matKey;
        }

        private string checkPlainText(string plainText)
        {
            plainText = plainText.ToUpper().Replace("J", "I");
            string pt = plainText[0].ToString();
            for (int i = 1; i < plainText.Length; i++)
            {
                // adding x between duplicated chars
                if (plainText[i - 1] == plainText[i] && pt.Length % 2 != 0)
                {
                    pt += "X";
                }
                pt += plainText[i];
            }

            // adding x in the last position if the text is odd
            if (pt.Length % 2 != 0)
            {
                pt += "X";
            }
            return pt;
        }
    }
}
