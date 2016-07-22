namespace SFA.Apprenticeships.Infrastructure.Security
{
    using System;
    using Configuration;
    using SFA.Infrastructure.Interfaces;

    public class AES256Provider
    {
        private readonly CryptographyConfiguration configuration;

        public AES256Provider(IConfigurationService configurationService)
        {
            configuration = configurationService.Get<CryptographyConfiguration>();
        }

        public string Encrypt(string input)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string input)
        {
            throw new NotImplementedException();
        }
    }
}
