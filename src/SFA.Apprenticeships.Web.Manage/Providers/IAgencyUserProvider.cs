namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using ViewModels;

    public interface IAgencyUserProvider
    {
        AgencyUserViewModel GetOrCreateAgencyUser(string username);
    }
}