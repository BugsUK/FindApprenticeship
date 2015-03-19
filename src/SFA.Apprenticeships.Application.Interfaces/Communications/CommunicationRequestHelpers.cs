namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System.Linq;

    // TODO: AG: no unit tests yet.
    public static class CommunicationRequestHelpers
    {
        public static string GetToken(this CommunicationRequest communicationRequest, CommunicationTokens communicationTokens)
        {
            return communicationRequest.Tokens.First(t => t.Key == communicationTokens).Value;
        }
    }
}
