namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IVacancyPartyReadRepository
    {
        VacancyParty Get(int vacancyPartyId);

        VacancyParty Get(int providerSiteId, int employerId);

        IEnumerable<VacancyParty> GetByIds(IEnumerable<int> vacancyPartyIds);

        IEnumerable<VacancyParty> GetForProviderSite(int providerSiteId);
    }

    public interface IVacancyPartyWriteRepository
    {
        VacancyParty Save(VacancyParty entity);
    }
}
