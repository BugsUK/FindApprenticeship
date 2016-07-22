namespace SFA.Apprenticeships.Infrastructure.Security
{
    using System;
    using Application.Interfaces.Security;
    using Newtonsoft.Json;

    public class CryptographyService<T> : IEncryptionService<T>, IDecryptionService<T> where T : class
    {
        private readonly IEncryptionProvider _encryptionProvider;

        public CryptographyService(IEncryptionProvider encryptionProvider)
        {
            _encryptionProvider = encryptionProvider;
        }
             
        public string Encrypt(T objectToEncrypt)
        {
            if (objectToEncrypt == null)
                throw new ArgumentNullException(nameof(objectToEncrypt));

            var jsonObj = JsonConvert.SerializeObject(objectToEncrypt);
            return _encryptionProvider.Encrypt(jsonObj);
        }

        public T Decrypt(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
                throw new ArgumentNullException(nameof(cipherText));

            var jsonRepresentation = _encryptionProvider.Decrypt(cipherText);
            return JsonConvert.DeserializeObject<T>(jsonRepresentation);
        }
    }
}
