namespace SFA.Apprenticeships.Application.Provider.Strategies
{
    using Domain.Entities.Raa.Parties;

    public interface IGetVacancyOwnerRelationshipStrategy
    {
        VacancyOwnerRelationship GetVacancyOwnerRelationship(int providerSiteId, string edsUrn, bool liveOnly = true);

        VacancyOwnerRelationship GetVacancyOwnerRelationship(int providerSiteId, int employerId, bool liveOnly = true);
    }
}