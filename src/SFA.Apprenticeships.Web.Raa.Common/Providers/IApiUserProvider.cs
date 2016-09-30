namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using ViewModels.Api;

    public interface IApiUserProvider
    {
        ApiUserSearchResultsViewModel SearchApiUsers(ApiUserSearchViewModel searchViewModel);
        ApiUserViewModel GetApiUserViewModel(Guid externalSystemId);
    }
}