using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int Encrypt(int p, int q, int M, int e)
        {
            int n = p * q;
            int cipherText = 1;

            for (int i = 0; i < e; i++)
            {
                cipherText =(cipherText * M) % n;
            }
            
            return cipherText;
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            throw new NotImplementedException();
        }
    }
}
