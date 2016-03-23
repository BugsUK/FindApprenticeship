namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using ViewModels;

    public interface IAgencyUserProvider
    {
        AgencyUserViewModel GetOrCreateAgencyUser(string username);
        AgencyUserViewModel GetAgencyUser(string username);
        AgencyUserViewModel SaveAgencyUser(string username, AgencyUserViewModel viewModel);
    }
}