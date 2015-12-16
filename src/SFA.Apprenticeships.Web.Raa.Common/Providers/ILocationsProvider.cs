namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using ViewModels.VacancyPosting;

    public interface ILocationsProvider
    {
        LocationSearchViewModel GetAddressesFor(LocationSearchViewModel viewModel);
    }
}