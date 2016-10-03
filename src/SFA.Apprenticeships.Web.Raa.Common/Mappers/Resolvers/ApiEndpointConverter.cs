namespace SFA.Apprenticeships.Web.Raa.Common.Mappers.Resolvers
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Domain.Entities.Raa.Api;
    using ViewModels.Api;

    public class ApiEndpointConverter : ITypeConverter<IList<ApiEndpointViewModel>, IList<ApiEndpoint>>
    {
        public IList<ApiEndpoint> Convert(ResolutionContext context)
        {
            var source = (IList<ApiEndpointViewModel>)context.SourceValue;
            var destination = source.Where(s => s.Authorised).Select(s => s.Endpoint).OrderBy(s => s).ToList();
            return destination;
        }
    }
}