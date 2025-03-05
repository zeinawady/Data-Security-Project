using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary
{
    public class AutokeyVigenere : ICryptographicTechnique<string, string>
    {
        public string Analyse(string plainText, string cipherText)
 {
     
     plainText = plainText.ToUpper();
     cipherText = cipherText.ToUpper();
     string key = "";
     string key_stream = "";


     char[,] matrix = new char[26, 26];

     for (int rows = 0; rows < 26; rows++)
     {
         for (int cols = 0; cols < 26; cols++)
         {
             matrix[rows, cols] = (char)(((rows + cols) % 26) + 'A');
         }
     }
     for (int j = 0; j < plainText.Length; j++)
     {
         int col_index = plainText[j] - 'A';
         int row_index = 0;
         for (int rows = 0; rows < 26; rows++)
         {
             if (matrix[rows, col_index] == cipherText[j])
             {
                 row_index = rows;
                 break;
             }
         }
         key_stream += (char)(row_index + 'A');

     }
     key = key_stream.Substring(0, key_stream.IndexOf(plainText[0]));
     return key;
 }

 public string Decrypt(string cipherText, string key)
 {
     char[,] matrix = new char[26, 26];

     for (int rows = 0; rows < 26; rows++)
     {
         for (int cols = 0; cols < 26; cols++)
         {
             matrix[rows, cols] = (char)(((rows + cols) % 26) + 'A');
         }
     }

     cipherText = cipherText.ToUpper();
     key = key.ToUpper();
     string plainText = "";
     string key_stream = key;
   
     
     for(int j=0; j< cipherText.Length; j++)
     {
         int row = key_stream[j] - 'A';
         int col_index = 0;
            
         for(int cols=0;cols<26;cols++)
         {
            
             if (matrix[row, cols] == cipherText[j])
             {
                 col_index = cols;
                 break;

             }

         }
         plainText += (char)(col_index + 'A');

         if(cipherText.Length > key_stream.Length)
         {
             key_stream += plainText[j];

         }
     }
     return plainText;
 }

 public string Encrypt(string plainText, string key)
 {
     char[,] matrix = new char[26, 26];

     for (int rows = 0; rows < 26; rows++)
     {
         for (int cols = 0; cols < 26; cols++)
         {
             matrix[rows, cols] = (char)(((rows + cols) % 26) + 'A');
         }
     }

     plainText = plainText.ToUpper();
     key = key.ToUpper();
     string key_stream = key;
     string cipherText = "";



     if (plainText.Length > key.Length)
     {
         key_stream += plainText.Substring(0, plainText.Length - key.Length);
         //int remaning_size = plainText.Length - key.Length;
         //for(int i=0 ; i < remaning_size ; i++)
         //{
         //    temp += plainText[i];
         //}
         //key_stream += temp;
     }
     for(int j = 0; j < plainText.Length; j++)
     {
         int row = key_stream[j] - 'A';  
         int col = plainText[j] - 'A';   
         cipherText += matrix[row, col];
     }
     return cipherText;
 }
    }
}
