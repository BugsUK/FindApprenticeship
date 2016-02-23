namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IVacancyPartyReadRepository
    {
        VacancyParty Get(int vacancyPartyId);

        VacancyParty Get(int providerSiteId, int employerId);

        IEnumerable<VacancyParty> GetForProviderSite(int providerSiteId);
    }

    public interface IVacancyPartyWriteRepository
    {
        void Delete(int vacancyPartyId);

        VacancyParty Save(VacancyParty entity);
    }
}
