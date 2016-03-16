namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories
{
    using System.Collections.Generic;
    using Entities.Raa.Parties;

    public interface IVacancyPartyReadRepository
    {
        VacancyParty GetById(int vacancyPartyId);

        VacancyParty GetByProviderSiteAndEmployerId(int providerSiteId, int employerId);

        IEnumerable<VacancyParty> GetByIds(IEnumerable<int> vacancyPartyIds);

        IEnumerable<VacancyParty> GetByProviderSiteId(int providerSiteId);
    }

    public interface IVacancyPartyWriteRepository
    {
        VacancyParty Save(VacancyParty vacancyParty);
    }
}
