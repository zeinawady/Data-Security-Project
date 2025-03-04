using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
        {
            //throw new NotImplementedException();
            cipherText = cipherText.ToLower();
            string Alpha = "abcdefghijklmnopqrstuvwxyz";
            var map = new Dictionary<char, char>(); 
            // add letter to map without repeate
            for (int i = 0; i < plainText.Length; i++)
            {               
                    if (!map.ContainsKey(plainText[i]) && !map.ContainsValue(cipherText[i]))
                    {
                        map.Add(plainText[i], cipherText[i]);
                    }       
            }

            string missingalpha = "";
            string missingcipher = "";
            for (int i = 0; i < 26; i++)
            {
                if(!map.ContainsKey((char)('a' + i)))
                {
                    missingalpha += (char)('a' + i); 
                }
                if (!map.ContainsValue((char)('a' + i)))
                {
                    missingcipher += (char)('a' + i);
                }
            }
            
            for(int i = 0;i < missingalpha.Length; i++)
            {
                map.Add(missingalpha[i], missingcipher[i]);
            }

            var sort = map.OrderBy(x => x.Key);
            string key = "";
            foreach(var item in sort)
            {
                key += item.Value;
            }
            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            //throw new NotImplementedException();
            cipherText = cipherText.ToLower();
            string Alpha = "abcdefghijklmnopqrstuvwxyz";
            char[] dec = new char[cipherText.Length];
            for (int i = 0; i < cipherText.Length; i++)
            {
                dec[i] = Alpha[key.IndexOf(cipherText[i])];
            }
            return new string(dec);
        }

        public string Encrypt(string plainText, string key)
        {
            //throw new NotImplementedException();
            char[] enc = new char[plainText.Length];
            for (int i = 0; i < plainText.Length; i++)
            {
                int character = plainText[i];
                enc[i] = key[character - 97];
            }
            return new string(enc).ToUpper();
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            //throw new NotImplementedException();
            string Freq = "etaoinsrhldcumfpgwybvkxjqz";
            cipher = cipher.ToLower();
            var map = new Dictionary<char, int>(); // map to know number of each letter
            // calculate numbers of letters
            for (int i = 0; i < cipher.Length; i++)
            {               
                    if (map.ContainsKey(cipher[i]))
                    {
                        map[cipher[i]]++;
                    }
                    else
                    {
                        map.Add(cipher[i],1);
                    }
               
            }
            
            var sort = map.OrderByDescending(x => x.Value).ThenBy(x => x.Key);
            var ToPlan = new Dictionary<char, char>();
            int index = 0;
            // make each letter  with freq
            foreach (var item in sort)
            {
                if (!ToPlan.ContainsKey(item.Key))
                {
                    ToPlan.Add(item.Key, Freq[index]);
                    index++;
                }

            }
           
            /*string missingKey = "";
            string missingValue = "";
            for (int i = 0; i < 26; i++)
            {
                if (!ToPlan.ContainsKey((char)('a' + i)))
                {
                    missingKey += (char)('a' + i);
                }
                if (!ToPlan.ContainsValue((char)('a' + i)))
                {
                    missingValue += (char)('a' + i);
                }
            }
            
            for (int i = 0; i < missingKey.Length; i++)
            {
                ToPlan.Add(missingKey[i], missingValue[i]);
            }*/

            
            string plainText = "";
            for (int i = 0; i < cipher.Length; i++)
            {               
                plainText += ToPlan[cipher[i]];
            }

            return plainText;            
        }
    }
}
