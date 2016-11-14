namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using Entities.Raa.Parties;
    using System.Collections.Generic;

    public interface IVacancyOwnerRelationshipReadRepository
    {
        VacancyOwnerRelationship GetByProviderSiteAndEmployerId(int providerSiteId, int employerId);

        IEnumerable<VacancyOwnerRelationship> GetByIds(IEnumerable<int> vacancyOwnerRelationshipIds, bool currentOnly = true); // TODO: Return IDictionary<int, VacancyOwnerRelationship>

        IEnumerable<VacancyOwnerRelationship> GetByProviderSiteId(int providerSiteId);

        bool IsADeletedVacancyOwnerRelationship(int providerSiteId, int employerId);
    }

    public interface IVacancyOwnerRelationshipWriteRepository
    {
        VacancyOwnerRelationship Save(VacancyOwnerRelationship vacancyOwnerRelationship);

        void ResurrectVacancyOwnerRelationship(int providerSiteId, int employerId);
    }
}
