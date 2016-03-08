using SFA.Apprenticeships.Application.Interfaces.Employers;

namespace SFA.Apprenticeships.Application.Organisation
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Parties;

    public interface ILegacyProviderProvider
    {
        Provider GetProvider(string ukprn);

        ProviderSite GetProviderSite(string ukprn, string edsUrn);

        IEnumerable<ProviderSite> GetProviderSites(string ukprn);

        //TODO: VacancyParty provider
        VacancyParty GetVacancyParty(int providerSiteId, int employerId);
        
        IEnumerable<VacancyParty> GetVacancyParties(int providerSiteId);
    }
}