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
    public class HillCipher :  ICryptographicTechnique<List<int>, List<int>>
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
            throw new NotImplementedException();
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            throw new NotImplementedException();
        }

    }
}
