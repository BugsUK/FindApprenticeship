namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using DAS.RAA.Api.Client.V1;

    public interface IApiClientProvider
    {
        IApiClient GetApiClient();
    }
}