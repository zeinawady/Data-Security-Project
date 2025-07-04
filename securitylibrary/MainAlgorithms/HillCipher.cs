﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {

        public int getDeterminant(List<int> matrix)
        {
            int determinant;
            int size = matrix.Count();
            switch (size)
            {
                case 4:
                    //Determinant of 2*2 matrix
                    determinant = (matrix[0] * matrix[3] -
                                   matrix[1] * matrix[2]) % 26;
                    break;
                case 9:


                    determinant = (matrix[0] * (matrix[4] * matrix[8] - matrix[5] * matrix[7]) -
                                   matrix[1] * (matrix[3] * matrix[8] - matrix[5] * matrix[6]) +
                                   matrix[2] * (matrix[3] * matrix[7] - matrix[4] * matrix[6])) % 26;
                    break;
                default:
                    //Other matrices aren't supported
                    throw new Exception("Finding determinants of matrices of any sizes rather than 2*2 or 3*3 is NOT supported");
            }
            // Ensure the determinant is non-negative
            while (determinant < 0)
            {
                determinant += 26;
            }
            determinant %= 26;
            return determinant;
        }


        public int GCD(int determinant, int modulo)
        {
            while (modulo != 0)
            {
                int temp = modulo;
                modulo = determinant % modulo;
                determinant = temp;
            }
            return determinant;
        }


        public int GetbValue(int determinant)
        {
            while (determinant < 0) { determinant += 26; }
            determinant %= 26;

            // If the determinant is 0, it has no inverse
            if (determinant == 0)
            {
                return -1;
            }

            for (int bvalue = 1; bvalue < 26; bvalue++)
            {
                if ((bvalue * determinant) % 26 == 1)
                {
                    return bvalue;
                }
            }

            return -1; // Indicates that there is no bValue found
        }


        public List<int> Inverse2by2(List<int> matrix, int bvalue)
        {
            if (matrix.Count() != 4)
            {
                throw new Exception("Wrong matrix size");
            }
            /* matrix = {A, B, C, D} => [A  B]          inverse = (bvalue * [D   -B]
             *                          [C  D]                              [-C   A]) % 26
             *                          
             */
            List<int> inverse2 = new List<int> { };
            int A, B, C, D;

            D = matrix[3] * bvalue;
            inverse2.Add(D);

            B = matrix[1] * -1 * bvalue;
            inverse2.Add(B);

            C = matrix[2] * -1 * bvalue;
            inverse2.Add(C);

            A = matrix[0] * bvalue;
            inverse2.Add(A);

            for (int i = 0; i < 4; i++)
            {
                while (inverse2[i] < 0) { inverse2[i] += 26; }
                inverse2[i] %= 26;
            }
            return inverse2;
        }


        public List<int> Inverse3by3(List<int> matrix, int bvalue)
        {
            /* k = [A   B   C]      =>      cofactor(K)= [+(EI−FH)  −(DI−FG)    +(DH−EG)]       => Transpose the cofactor matrix
             *     [D   E   F]                           [−(BI−CH)  +(AI−CG)    −(AH−BG)]
             *     [G   H   I]                           [+(BF−CE)  −(AF−CD)    +(AE−BD)]
             *     
             * Loop on the matrix and handle the negative values
             * Get the modulo 26 of the matrix
             * Multiply the matrix by the bValue then get the modulo 26 again
             * det(K)=A(EI−FH)−B(DI−FG)+C(DH−EG)
             */

            if (matrix.Count != 9)
            {
                throw new Exception("Wrong matrix size. Must be 3*3");
            }

            int adjA, adjB, adjC, adjD, adjE, adjF, adjG, adjH, adjI;

            adjA = ((matrix[4] * matrix[8]) - (matrix[5] * matrix[7])) % 26;         //+(EI−FH)

            adjB = (-((matrix[3] * matrix[8]) - (matrix[5] * matrix[6]))) % 26;      //−(DI−FG)

            adjC = ((matrix[3] * matrix[7]) - (matrix[4] * matrix[6])) % 26;         //+(DH−EG)

            adjD = (-((matrix[1] * matrix[8]) - (matrix[2] * matrix[7]))) % 26;      //−(BI−CH)

            adjE = ((matrix[0] * matrix[8]) - (matrix[2] * matrix[6])) % 26;         //+(AI−CG)

            adjF = (-((matrix[0] * matrix[7]) - (matrix[1] * matrix[6]))) % 26;      //−(AH−BG)

            adjG = ((matrix[1] * matrix[5]) - (matrix[2] * matrix[4])) % 26;         //+(BF−CE)

            adjH = (-((matrix[0] * matrix[5]) - (matrix[2] * matrix[3]))) % 26;      //−(AF−CD)

            adjI = ((matrix[0] * matrix[4]) - (matrix[1] * matrix[3])) % 26;         //+(AE−BD)

            //Adding the ajoints in a transposed manner to the list
            List<int> inverse3 = new List<int> { adjA, adjD, adjG,
                                                 adjB, adjE, adjH,
                                                 adjC, adjF, adjI };

            //Handling negative values
            for (int i = 0; i < 9; i++)
            {
                while (inverse3[i] < 0)
                {
                    inverse3[i] += 26;
                }
                inverse3[i] %= 26;
                inverse3[i] = (inverse3[i] * bvalue) % 26;
            }
            return inverse3;
        }


        public List<int> Multiply2by2(List<int> matrix1, List<int> matrix2)
        {
            if (matrix1.Count() != 4 || matrix2.Count() != 4)
            {
                throw new Exception("Wrong matrices size to be multiplied. Size must be 2*2");
            }

            int A, B, C, D;
            A = (matrix1[0] * matrix2[0]) + (matrix1[1] * matrix2[2]);
            B = (matrix1[0] * matrix2[1]) + (matrix1[1] * matrix2[3]);

            C = (matrix1[2] * matrix2[0]) + (matrix1[3] * matrix2[2]);
            D = (matrix1[2] * matrix2[1]) + (matrix1[3] * matrix2[3]);

            List<int> multiply2by2 = new List<int> { A, B, C, D };

            for (int i = 0; i < 4; i++)
            {
                if (multiply2by2[i] < 0)
                {
                    multiply2by2[i] += 26;
                }
                multiply2by2[i] %= 26;
            }

            return multiply2by2;
        }


        public List<int> Multiply3by3(List<int> matrix1, List<int> matrix2)
        {
            /*                k0   k1  k2                           j0   j1  j2
             * matrix1 =   i0 [A   B   C]      =>      matrix2=  k0 [A   B   C]       
             *             i1 [D   E   F]                        k1 [D   E   F]
             *             i2 [G   H   I]                        k2 [G   H   I]
             *     
             * First layer = (A[i0][K0]*B[K0][j0]) + (A[i0][K1]*B[K0][j1]) + (A[i0][K2]*B[K0][j2]) 
             */

            if (matrix1.Count != 9 || matrix2.Count != 9)
            {
                throw new Exception("Wrong matrices size to be multiplied. Size must be 3*3");
            }

            int[,] mat1 = new int[3, 3];
            int[,] mat2 = new int[3, 3];
            int element = 0;
            int[,] multiply3by3 = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    mat1[i, j] = matrix1[element];
                    mat2[i, j] = matrix2[element];
                    multiply3by3[i, j] = 0;
                    element++;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        multiply3by3[i, j] += mat1[i, k] * mat2[k, j];
                    }
                }
            }
            List<int> resultList = new List<int> { };
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    resultList.Add(multiply3by3[i, j]);
                }
            }
            //Handle negative values and get mod of the list
            for (int i = 0; i < 9; i++)
            {
                while (resultList[i] < 0)
                {
                    resultList[i] += 26;
                }
                resultList[i] %= 26;
            }

            return resultList;
        }


        List<int> listMultiplication(List<int> text, List<int> key)
        {
            List<int> multiplicationResult = new List<int>();
            int size = key.Count();
            size = (int)Math.Sqrt(size);
            for (int i = 0; i < text.Count(); i += size)
            {
                for (int j = 0; j < key.Count(); j += size)
                {
                    int indexSum = 0;
                    for (int k = 0; k < size; k++)
                    {
                        indexSum += key[j + k] * text[i + k];
                    }
                    multiplicationResult.Add(indexSum % 26);
                }
            }
            return multiplicationResult;
        }


        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            for (int a = 0; a < 26; a++)
            {
                for (int b = 0; b < 26; b++)
                {
                    for (int c = 0; c < 26; c++)
                    {
                        for (int d = 0; d < 26; d++)
                        {
                            List<int> possibleKey = new List<int> { };
                            possibleKey.Add(a);
                            possibleKey.Add(b);
                            possibleKey.Add(c);
                            possibleKey.Add(d);
                            int determinant = getDeterminant(possibleKey);
                            int det_Mod_gcd = GCD(determinant, 26);
                            if (det_Mod_gcd != 1 || determinant == 0) { continue; }

                            List<int> possibleCipher = Encrypt(plainText, possibleKey);
                            if (possibleCipher.SequenceEqual(cipherText))
                            {
                                return possibleKey;
                            }
                        }
                    }
                }
            }
            throw new InvalidAnlysisException();
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {

            int determinant = getDeterminant(key);
            int gcd = GCD(determinant, 26);
            if (determinant == 0 || gcd != 1)
            {
                throw new NotImplementedException();
            }

            int bvalue = GetbValue(determinant);

            List<int> keyInverse = new List<int> { };

            int size = key.Count;

            switch (size)
            {
                case 4:
                    keyInverse = Inverse2by2(key, bvalue);
                    break;
                case 9:
                    keyInverse = Inverse3by3(key, bvalue);
                    break;
                default:
                    throw new Exception("Can't decrypt the passed matrix");
            }
            List<int> plain = listMultiplication(cipherText, keyInverse);
            return plain;

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

            int determinant = getDeterminant(key);
            int gcd = GCD(determinant, 26); //Insuring that the determinant and 26 have no common divisors but 1


            if (gcd == 1 && determinant != 0)
            {
                //2*2 Encryption 
                if (key.Count() == 4)
                {
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

                //3*3 Encryption
                else
                {
                    List<int> Cipher3by3 = new List<int> { };

                    for (int i = 0; i < plainText.Count(); i += 3)
                    {
                        int firstElement = (key[0] * plainText[i] + key[1] * plainText[i + 1] + key[2] * plainText[i + 2]) % 26;
                        int secondElement = (key[3] * plainText[i] + key[4] * plainText[i + 1] + key[5] * plainText[i + 2]) % 26;
                        int thirdElement = (key[6] * plainText[i] + key[7] * plainText[i + 1] + key[8] * plainText[i + 2]) % 26;

                        Cipher3by3.Add(firstElement);
                        Cipher3by3.Add(secondElement);
                        Cipher3by3.Add(thirdElement);

                    }
                    return Cipher3by3;
                }
            }
            else
                throw new InvalidAnlysisException();
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {

            //Validating the passed matrices size
            if (plainText.Count != 9 || cipherText.Count != 9)
            {
                throw new Exception("Can't get the key. Wrong size matrices");
            }

            //Validating the passed letter values
            for (int i = 0; i < cipherText.Count; i++)
            {
                if (cipherText.ElementAt(i) < 0 || cipherText.ElementAt(i) >= 26)
                {
                    throw new Exception("Invalid Cipher text");
                }
            }

            for (int i = 0; i < plainText.Count; i++)
            {
                if (plainText.ElementAt(i) < 0 || plainText.ElementAt(i) >= 26)
                {
                    throw new Exception("Invalid Plain text");
                }
            }

            int determinant3 = getDeterminant(plainText);

            int gcd = GCD(determinant3, 26); //Insuring that the determinant and 26 have no common divisors but 1


            if (gcd != 1 || determinant3 == 0)
            {
                throw new InvalidAnlysisException();
            }

            int bvalue = GetbValue(determinant3);

            if (bvalue == -1)
            {
                throw new InvalidAnlysisException();
            }



            List<int> plainInverse = Inverse3by3(plainText, bvalue);
            //Transposing the cipher matrix
            int A = cipherText[0], B = cipherText[1], C = cipherText[2],
                D = cipherText[3], E = cipherText[4], F = cipherText[5],
                G = cipherText[6], H = cipherText[7], I = cipherText[8];
            List<int> cipherTranspose = new List<int> { A, D, G,
                                                        B, E, H,
                                                        C, F, I };
            List<int> key3by3 = listMultiplication(cipherTranspose, plainInverse);
            return key3by3;
            //throw new NotImplementedException();
        }


    }
}