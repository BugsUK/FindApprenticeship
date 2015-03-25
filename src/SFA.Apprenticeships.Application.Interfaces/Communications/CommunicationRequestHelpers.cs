namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    using System.Linq;

    public static class CommunicationRequestHelpers
    {
        public static string GetToken(this CommunicationRequest communicationRequest, CommunicationTokens communicationToken)
        {
            return communicationRequest.Tokens.First(t => t.Key == communicationToken).Value;
        }

        public static bool ContainsToken(this CommunicationRequest communicationRequest, CommunicationTokens communicationToken)
        {
            return communicationRequest.Tokens.Count(each => each.Key == communicationToken) > 0;
        }
    }
}
