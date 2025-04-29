using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.AES
{
    public class ExtendedEuclid 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="baseN"></param>
        /// <returns>Mul inverse, -1 if no inv</returns>
        public int GetMultiplicativeInverse(int number, int baseN)
        {
            int A3= baseN;
            int B3 = number;
            int A1 = 1;
            int A2 = 0;
            int B1 = 0;
            int B2 = 1;
            int Q;
            int temp;

            while (true)
            {
                if (B3 == 0)
                {
                    return -1;
                }
                else if (B3 == 1)
                {
                    if (B2 >= 0)
                    {
                        return B2;
                    }
                    else
                    {
                        B2 = (B2 + baseN) % baseN;
                        return B2;
                    }
                }

                Q = A3 / B3;

                temp = A3 % B3;
                A3 = B3;
                B3 = temp;

                temp = A1 - (B1 * Q);
                A1 = B1;
                B1 = temp;

                temp = A2 - (B2 * Q);
                A2 = B2;
                B2 = temp;
            }
            //throw new NotImplementedException();
        }
    }
}
