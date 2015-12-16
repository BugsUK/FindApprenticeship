namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Collections.Generic;
    using ViewModels.VacancyPosting;

    public interface ILocationsProvider
    {
        List<VacancyLocationAddressViewModel> GetAddressesFor(string fullPostcode);
    }
}