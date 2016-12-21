namespace SFA.Apprenticeships.Application.Provider.Strategies
{
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa.Parties;

    public interface IGetVacancyOwnerRelationshipStrategy
    {
        VacancyOwnerRelationship GetVacancyOwnerRelationship(int providerSiteId, string edsUrn, bool liveOnly = true);

        VacancyOwnerRelationship GetVacancyOwnerRelationship(int providerSiteId, int employerId, bool liveOnly = true);

        bool IsADeletedVacancyOwnerRelationship(int providerSiteId, string edsUrn);

        bool IsADeletedVacancyOwnerRelationship(int providerSiteId, int employerId);

        void ResurrectVacancyOwnerRelationship(int providerSiteId, string edsUrn);

        void ResurrectVacancyOwnerRelationship(int providerSiteId, int employerId);
    }
}