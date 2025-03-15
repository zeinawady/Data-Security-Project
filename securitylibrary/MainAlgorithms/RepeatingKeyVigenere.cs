using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class RepeatingkeyVigenere : ICryptographicTechnique<string, string>
    {
        public static char[,] CreateVigenereTable()
        {
            char[,] matrix = new char[26, 26];

            for (int row = 0; row < 26; row++)
            {
                for (int col = 0; col < 26; col++)
                {
                    matrix[row, col] = (char)(((row + col) % 26) + 'A');
                    //بيحسب الحرف المشفر بناء على كل صف وعمود، الصف هو حرف المفتاح، والعمود هو حرف النص الأصلي
                    //+'A' بيحول الرقم لحرف
                    // uses the algorithm equation to build the table: ci = (ki + pi)% 26
                }
            }

            return matrix;
            // جدول لكل الحروف، وحاطط لكل حرف كل الاحتمالات اللي ممكن يطلع بيها، بحيث أن تقاطع الحرفين هيطلع الحرف المشفر

        }

        private static string ExtendKey(string key, int length)
        {
            StringBuilder extendedKey = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                extendedKey.Append(key[i % key.Length]);
            }
            return extendedKey.ToString();
            // extends key to be equals the plain text
        }

        public string Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToUpper();
            cipherText = cipherText.ToUpper();
            StringBuilder keyStream = new StringBuilder();

            char[,] matrix = CreateVigenereTable();

            for (int i = 0; i < plainText.Length; i++)
            {
                int col = plainText[i] - 'A';
                int row = 0;


                for (int j = 0; j < 26; j++)
                {
                    if (matrix[j, col] == cipherText[i])
                    {
                        row = j;
                        break;
                    }
                }
                keyStream.Append((char)(row + 'A'));
            }
            // لحد هنا هو استخرج المفتاح المتكرر

            string fullKey = keyStream.ToString();

            for (int length = 1; length <= fullKey.Length / 2; length++)
            {
                string subKey = fullKey.Substring(0, length);
                string repeatedKey = "";

                while (repeatedKey.Length < fullKey.Length)
                    repeatedKey += subKey;

                if (repeatedKey.StartsWith(fullKey))
                    return subKey;
            }

            return fullKey;
        }


        public string Decrypt(string cipherText, string key)
        {
            cipherText = cipherText.ToUpper();
            key = key.ToUpper();
            StringBuilder plainText = new StringBuilder();
            char[,] matrix = CreateVigenereTable();

            string extendedKey = ExtendKey(key, cipherText.Length);

            for (int i = 0; i < cipherText.Length; i++)
            {
                int row = extendedKey[i] - 'A';
                int col = 0;


                for (int j = 0; j < 26; j++)
                {
                    if (matrix[row, j] == cipherText[i])
                    {
                        col = j;
                        break;
                    }
                }
                plainText.Append((char)(col + 'A'));
                //بيدور على الحرف الأصلي والشمفر وياخد الkey بتاعهم
            }

            return plainText.ToString();
        }

        public string Encrypt(string plainText, string key)
        {
            plainText = plainText.ToUpper();
            key = key.ToUpper();
            //All letters capital
            StringBuilder cipherText = new StringBuilder();

            char[,] matrix = CreateVigenereTable(); //object from table

            string extendedKey = ExtendKey(key, plainText.Length);

            for (int i = 0; i < plainText.Length; i++)
            {
                int row = extendedKey[i] - 'A'; //بيحدد هو في أنهي صف في الجدول على حسب الkey
                int col = plainText[i] - 'A';   //بيحدد هو في أنهي عمود على حسب النص الحقيقي
                cipherText.Append(matrix[row, col]); // بيجيب التقاطع بينهم ويحطه في السايفر تكست
            }

            return cipherText.ToString();

            // طريقة تانية باستخدام المعادلة مباشرة
            //        plainText = plainText.ToUpper(); 
            //        key = key.ToUpper(); 
            //        StringBuilder cipherText = new StringBuilder();

            //        string extendedKey = ExtendKey(key, plainText.Length); 

            //        for (int i = 0; i < plainText.Length; i++)
            //        {
            //             C_i = (P_i + K_i) mod 26
            //            char encryptedChar = (char)(((plainText[i] - 'A' + extendedKey[i] - 'A') % 26) + 'A');
            //            cipherText.Append(encryptedChar);
            //        }

            //        return cipherText.ToString();


        }
    }
}