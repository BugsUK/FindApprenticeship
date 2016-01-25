namespace SFA.Apprenticeship.Api.AvService.Providers
{
    public enum WebServiceAuthenticationResult
    {
        Unknown = 0,
        Authenticated = 1,
        InvalidPublicKey = 2,
        AuthenticationFailed = 3,
        InvalidExternalSystemId = 4,
        NotAllowed = 5
    }
}
