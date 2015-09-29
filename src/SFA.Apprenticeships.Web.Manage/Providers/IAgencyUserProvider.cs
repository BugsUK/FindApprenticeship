namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using FluentValidation.Results;
    using ViewModels;

    public interface IAgencyUserProvider
    {
        AgencyUserViewModel GetOrCreateAgencyUser(string username, string roleList);
        AgencyUserViewModel GetAgencyUser(string username, string roleList);
        AgencyUserViewModel SaveAgencyUser(string username, string roleList, AgencyUserViewModel viewModel);
    }
}