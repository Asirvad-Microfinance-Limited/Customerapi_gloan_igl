using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace DataAccessLibrary
{
    public class PasswordSecurity 
    {


        private const string _securityKey = "raju";




        public string Decrypt(string EncryptedText)
        {
            byte[] toEncryptArray = Convert.FromBase64String(EncryptedText);



            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();



            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.

            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));



            //De-allocatinng the memory after doing the Job.

            objMD5CryptoService.Clear();



            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();



            //Assigning the Security key to the TripleDES Service Provider.

            objTripleDESCryptoService.Key = securityKeyArray;



            //Mode of the Crypto service is Electronic Code Book.

            objTripleDESCryptoService.Mode = CipherMode.ECB;



            //Padding Mode is PKCS7 if there is any extra byte is added.

            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;



            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();



            //Transform the bytes array to resultArray

            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);



            //Releasing the Memory Occupied by TripleDES Service Provider for Decryption.          

            objTripleDESCryptoService.Clear();



            //Convert and return the decrypted data/byte into string format.

            return UTF8Encoding.UTF8.GetString(resultArray);

        }

        public string Encrypt(string Text)
        {
            byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(Text);



            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();



            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.

            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(_securityKey));



            //De-allocatinng the memory after doing the Job.

            objMD5CryptoService.Clear();



            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();



            //Assigning the Security key to the TripleDES Service Provider.

            objTripleDESCryptoService.Key = securityKeyArray;



            //Mode of the Crypto service is Electronic Code Book.

            objTripleDESCryptoService.Mode = CipherMode.ECB;



            //Padding Mode is PKCS7 if there is any extra byte is added.

            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;



            var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();



            //Transform the bytes array to resultArray

            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);



            //Releasing the Memory Occupied by TripleDES Service Provider for Encryption.

            objTripleDESCryptoService.Clear();



            //Convert and return the encrypted data/byte into string format.

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        // added by vaishnav for AES encryption in doorstep VAPT

        public string EncryptString(string key , string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;
            //string key = "7x!A%D*G-KaPdSgV";

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }
    }
}
