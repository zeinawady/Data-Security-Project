using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        public string Encrypt(string plainText, int key)
        {
            //throw new NotImplementedException();
            string encryptedText = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                if (plainText[i] >= 97 && plainText[i] <= 122)
                {
                    encryptedText += (char)((plainText[i] - 'a' + key + 26) % 26 + 'a');
                }
                else if (plainText[i] >= 65 && plainText[i] <= 90)
                {
                    encryptedText += (char)((plainText[i] - 'A' + key + 26) % 26 + 'A');
                }
                else
                {
                    encryptedText += plainText[i];
                }
            }

            return encryptedText;

        }

        public string Decrypt(string cipherText, int key)
        {

            //throw new NotImplementedException();
            string plainText = "";
            for (int i = 0; i < cipherText.Length; i++)
            {
                if (cipherText[i] >= 97 && cipherText[i] <= 122)
                {
                    plainText += (char)((cipherText[i] - 'a' - key + 26) % 26 + 'a');
                }
                else if (cipherText[i] >= 65 && cipherText[i] <= 90)
                {
                    plainText += (char)((cipherText[i] - 'A' - key + 26) % 26 + 'A');
                }
                else
                {
                    plainText += cipherText[i];
                }
            }

            return plainText;

        }

        public int Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            //find the key that is used in encryption
            plainText = plainText.ToLower();
            cipherText = cipherText.ToLower();

            int key = cipherText[0] - plainText[0];
            if (key < 0)
            {
                key += 26;
            }
            return key;

        }
    }
}