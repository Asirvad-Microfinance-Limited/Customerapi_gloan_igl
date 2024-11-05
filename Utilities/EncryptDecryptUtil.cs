using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public class EncryptDecryptUtil
    {
        #region summary
        /// <summary>       
        /// Created on : 16-Dec-2019
        /// Created By : 100101
        /// Description: EncryptDecryptUtil
        /// Modify Date:
        /// Modify By  : 
        /// Description:
        /// </summary>

        #endregion

        #region Methods
        public byte[] getEdata(string userName, string password)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            UTF8Encoding encoder = new UTF8Encoding();
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(userName + "raju" + password));
            return hashedDataBytes;
        }


        private string _securityKey = "raju";

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

            _securityKey = "SecurityKeyandPassing" + RandomString(25);


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

        private static Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXABCDEFGHIJKLMNOPQRSTUVWXYZYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion
    }
}
