namespace SFA.DAS.RAA.Api.Strategies
{
    using Models;

    public interface ILinkEmployerStrategy
    {
        EmployerProviderSiteLink LinkEmployer(EmployerProviderSiteLink employerProviderSiteLink, int? employerId, int? edsUrn, int providerId);
    }
}