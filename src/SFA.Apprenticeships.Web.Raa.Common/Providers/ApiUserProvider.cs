﻿namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Raa.Api;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Mappers;
    using ViewModels.Api;

    public class ApiUserProvider : IApiUserProvider
    {
        private readonly ApiUserMappers _apiUserMappers = new ApiUserMappers();

        //Referencing the repository directly in the provider. Supposed to go through a service to follow the usual pattern but as API users are considered legacy and this is for an admin screen, I feel it's OK
        private readonly IApiUserRepository _apiUserRepository;

        public ApiUserProvider(IApiUserRepository apiUserRepository)
        {
            _apiUserRepository = apiUserRepository;
        }

        public ApiUserSearchResultsViewModel SearchApiUsers(ApiUserSearchViewModel searchViewModel)
        {
            var viewModel = new ApiUserSearchResultsViewModel
            {
                SearchViewModel = searchViewModel
            };

            if (!searchViewModel.PerformSearch) return viewModel;

            var searchParameters = new ApiUserSearchParameters
            {
                ExternalSystemId = searchViewModel.ExternalSystemId,
                Id = searchViewModel.Id,
                Name = searchViewModel.Name
            };

            var apiUsers = _apiUserRepository.SearchApiUsers(searchParameters);

            viewModel.ApiUsers = apiUsers.Select(a => _apiUserMappers.Map<ApiUser, ApiUserViewModel>(a)).ToList();

            return viewModel;
        }

        public ApiUserViewModel GetApiUserViewModel(Guid externalSystemId)
        {
            return _apiUserMappers.Map<ApiUser, ApiUserViewModel>(_apiUserRepository.GetApiUser(externalSystemId));
        }

        public ApiUserViewModel GetApiUserViewModel(string companyId)
        {
            return _apiUserMappers.Map<ApiUser, ApiUserViewModel>(_apiUserRepository.GetApiUser(companyId));
        }

        public ApiUserViewModel CreateApiUser(ApiUserViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public ApiUserViewModel SaveApiUser(ApiUserViewModel viewModel)
        {
            var apiUser = _apiUserRepository.GetApiUser(viewModel.ExternalSystemId);

            //Copy over changes
            apiUser.AuthorisedApiEndpoints = _apiUserMappers.Map<IList<ApiEndpointViewModel>, IList<ApiEndpoint>>(viewModel.ApiEndpoints);

            var updatedApiUser = _apiUserRepository.Update(apiUser);

            return _apiUserMappers.Map<ApiUser, ApiUserViewModel>(updatedApiUser);
        }
    }
}