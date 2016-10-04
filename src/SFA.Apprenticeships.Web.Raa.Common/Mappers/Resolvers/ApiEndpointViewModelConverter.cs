namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using System.Collections.Generic;
    using AutoMapper;
    using Domain.Entities.Raa.Api;
    using ViewModels.Api;

    public class ApiEndpointViewModelConverter : ITypeConverter<IList<ApiEndpoint>, IList<ApiEndpointViewModel>>
    {
        public IList<ApiEndpointViewModel> Convert(ResolutionContext context)
        {
            var source = (IList<ApiEndpoint>)context.SourceValue;
            var destination = new List<ApiEndpointViewModel>
            {
                new ApiEndpointViewModel { Endpoint = ApiEndpoint.VacancySummary, Authorised = source.Contains(ApiEndpoint.VacancySummary) },
                new ApiEndpointViewModel { Endpoint = ApiEndpoint.VacancyDetail, Authorised = source.Contains(ApiEndpoint.VacancyDetail) },
                new ApiEndpointViewModel { Endpoint = ApiEndpoint.ReferenceData, Authorised = source.Contains(ApiEndpoint.ReferenceData) },
                new ApiEndpointViewModel { Endpoint = ApiEndpoint.BulkVacancyUpload, Authorised = source.Contains(ApiEndpoint.BulkVacancyUpload) },
                new ApiEndpointViewModel { Endpoint = ApiEndpoint.ApplicationTracking, Authorised = source.Contains(ApiEndpoint.ApplicationTracking) }
            };

            return destination;
        }
    }
}