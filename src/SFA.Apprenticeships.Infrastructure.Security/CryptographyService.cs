using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.Apprenticeships.Infrastructure.Security
{
    using Application.Interfaces.Security;

    public class CryptographyService<T> : IEncryptionService<T>, IDecryptionService<T> where T : class
    {
        public string Encrypt(T objectToEncrypt)
        {
            throw new NotImplementedException();
        }
    }
}
