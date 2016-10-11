namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using ViewModels.Api;

    public interface IApiUserProvider
    {
        ApiUserSearchResultsViewModel SearchApiUsers(ApiUserSearchViewModel searchViewModel);
        ApiUserViewModel GetApiUserViewModel(Guid externalSystemId);
        ApiUserViewModel GetApiUserViewModel(string companyId);
        ApiUserViewModel CreateApiUser(ApiUserViewModel viewModel);
        ApiUserViewModel SaveApiUser(ApiUserViewModel viewModel);
        ApiUserViewModel ResetApiUserPassword(ApiUserViewModel viewModel);
    }
}