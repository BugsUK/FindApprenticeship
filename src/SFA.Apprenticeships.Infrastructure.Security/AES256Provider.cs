namespace SFA.Apprenticeships.Infrastructure.Security
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using Configuration;
    using SFA.Infrastructure.Interfaces;
    
    public class AES256Provider : IEncryptionProvider
    {
        private readonly CryptographyConfiguration _configuration;
        private readonly ILogService _logService;
        private static int _iterations = 2;
        private static int _keySize = 256;
        private static string _hash = "SHA1";
        private byte[] _keyBytes;
        private byte[] _ivBytes;

        public AES256Provider(IConfigurationService configurationService, ILogService logService)
        {
            _configuration = configurationService.Get<CryptographyConfiguration>();
            _keyBytes = Convert.FromBase64String(_configuration.Key);
            _ivBytes = Convert.FromBase64String(_configuration.IV);
            _logService = logService;
        }

        public string Encrypt(string input)
        {
            // Check arguments.
            if (input == null || input.Length <= 0)
                throw new ArgumentNullException(nameof(input));

            return Convert.ToBase64String(EncryptStringToBytes(input, _keyBytes, _ivBytes));
        }

        public string Decrypt(string cipherText)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException(nameof(cipherText));

            var cipherTextBytes = Convert.FromBase64String(cipherText);

            return DecryptStringFromBytes(cipherTextBytes, _keyBytes, _ivBytes);
        }

        /// <summary>
        /// Used to generate keys
        /// </summary>
        //public void GenerateKeys()
        //{
        //    var myRijndael = new RijndaelManaged();

        //    myRijndael.GenerateKey();
        //    myRijndael.GenerateIV();

        //    var stringIV = Convert.ToBase64String(myRijndael.IV);
        //    var stringKey = Convert.ToBase64String(myRijndael.Key);
        //}


        private byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException(nameof(Key));
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException(nameof(IV));

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }
    }
}
