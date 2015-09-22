namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.Communication.ProviderUser
{
    using System.Collections.Generic;
    using Application.Interfaces.Communications;

    public static class ProviderUserEmailTokenGenerator
    {
        private const string TestProviderUserUsername = "jane.doe@example.com";
        private const string TestEmailVerificationCode = "XYZ456";
        private const string TestProviderUserFullName = "Jane Doe";

        public static IEnumerable<CommunicationToken> SendEmailVerificationCodeTokens()
        {
            return new[]
            {
                new CommunicationToken(CommunicationTokens.ProviderUserUsername, TestProviderUserUsername),
                new CommunicationToken(CommunicationTokens.ProviderUserFullName, TestProviderUserFullName),
                new CommunicationToken(CommunicationTokens.ProviderUserEmailVerificationCode, TestEmailVerificationCode)
            };
        }
    }
}
