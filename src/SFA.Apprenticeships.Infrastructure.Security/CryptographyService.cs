namespace SFA.Apprenticeships.Infrastructure.Security
{
    using System;
    using Application.Interfaces.Security;

    public class CryptographyService<T> : IEncryptionService<T>, IDecryptionService<T> where T : class
    {
        public string Encrypt(T objectToEncrypt)
        {
            throw new NotImplementedException();
        }
    }
}
