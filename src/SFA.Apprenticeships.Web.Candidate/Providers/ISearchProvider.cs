namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using ViewModels.VacancySearch;

    public interface ISearchProvider
    {
        LocationsViewModel FindLocation(string placeNameOrPostcode);
    }
}
