namespace SFA.DAS.RAA.Api.Strategies
{
    using Models;

    public interface ILinkEmployerStrategy
    {
        EmployerProviderSiteLinkResponse LinkEmployer(EmployerProviderSiteLinkRequest employerProviderSiteLink, int edsUrn, string ukprn);
    }
}