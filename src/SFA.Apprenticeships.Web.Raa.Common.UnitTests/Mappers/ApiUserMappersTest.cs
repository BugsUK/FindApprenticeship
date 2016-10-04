namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Mappers;
    using Domain.Entities.Raa.Api;
    using FluentAssertions;
    using NUnit.Framework;
    using ViewModels.Api;

    [TestFixture]
    [Parallelizable]
    public class ApiUserMappersTest
    {
        [Test]
        public void ShouldCreateMap()
        {
            new ApiUserMappers().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ConvertAuthorisedApiEndpoints()
        {
            var apiUser = new ApiUser
            {
                AuthorisedApiEndpoints = new List<ApiEndpoint>
                {
                    ApiEndpoint.VacancySummary,
                    ApiEndpoint.VacancyDetail,
                    ApiEndpoint.BulkVacancyUpload
                }
            };

            var apiUserViewModel = new ApiUserMappers().Map<ApiUser, ApiUserViewModel>(apiUser);

            apiUserViewModel.ApiEndpoints.Count.Should().Be(5);
            apiUserViewModel.ApiEndpoints.Single(e => e.Endpoint == ApiEndpoint.VacancySummary).Authorised.Should().BeTrue();
            apiUserViewModel.ApiEndpoints.Single(e => e.Endpoint == ApiEndpoint.VacancyDetail).Authorised.Should().BeTrue();
            apiUserViewModel.ApiEndpoints.Single(e => e.Endpoint == ApiEndpoint.ReferenceData).Authorised.Should().BeFalse();
            apiUserViewModel.ApiEndpoints.Single(e => e.Endpoint == ApiEndpoint.BulkVacancyUpload).Authorised.Should().BeTrue();
            apiUserViewModel.ApiEndpoints.Single(e => e.Endpoint == ApiEndpoint.ApplicationTracking).Authorised.Should().BeFalse();
        }

        [Test]
        public void ConvertApiEndpoints()
        {
            var apiUserViewModel = new ApiUserViewModel
            {
                ApiEndpoints = new List<ApiEndpointViewModel>
                {
                    new ApiEndpointViewModel { Endpoint = ApiEndpoint.BulkVacancyUpload, Authorised = true },
                    new ApiEndpointViewModel { Endpoint = ApiEndpoint.VacancyDetail, Authorised = true },
                    new ApiEndpointViewModel { Endpoint = ApiEndpoint.VacancySummary, Authorised = true },
                    new ApiEndpointViewModel { Endpoint = ApiEndpoint.ReferenceData, Authorised = false },
                    new ApiEndpointViewModel { Endpoint = ApiEndpoint.ApplicationTracking, Authorised = false }
                }
            };

            var apiUser = new ApiUserMappers().Map<ApiUserViewModel, ApiUser>(apiUserViewModel);

            apiUser.AuthorisedApiEndpoints.Count.Should().Be(3);
            apiUser.AuthorisedApiEndpoints.Contains(ApiEndpoint.VacancySummary).Should().BeTrue();
            apiUser.AuthorisedApiEndpoints.Contains(ApiEndpoint.VacancyDetail).Should().BeTrue();
            apiUser.AuthorisedApiEndpoints.Contains(ApiEndpoint.ReferenceData).Should().BeFalse();
            apiUser.AuthorisedApiEndpoints.Contains(ApiEndpoint.BulkVacancyUpload).Should().BeTrue();
            apiUser.AuthorisedApiEndpoints.Contains(ApiEndpoint.ApplicationTracking).Should().BeFalse();

            apiUser.AuthorisedApiEndpoints[0].Should().Be(ApiEndpoint.VacancySummary);
            apiUser.AuthorisedApiEndpoints[1].Should().Be(ApiEndpoint.VacancyDetail);
            apiUser.AuthorisedApiEndpoints[2].Should().Be(ApiEndpoint.BulkVacancyUpload);
        }
    }
}