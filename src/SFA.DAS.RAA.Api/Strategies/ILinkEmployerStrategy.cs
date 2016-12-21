namespace SFA.DAS.RAA.Api.Strategies
{
    using Models;

    public interface ILinkEmployerStrategy
    {
        EmployerProviderSiteLink LinkEmployer(EmployerProviderSiteLink employerProviderSiteLink, int edsUrn, string ukprn);
    }
}