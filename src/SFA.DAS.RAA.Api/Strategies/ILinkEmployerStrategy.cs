namespace SFA.DAS.RAA.Api.Strategies
{
    using Models;

    public interface ILinkEmployerStrategy
    {
        EmployerProviderSiteLink LinkEmployer(EmployerProviderSiteLinkRequest employerProviderSiteLinkRequest, int edsUrn, string ukprn);
    }
}