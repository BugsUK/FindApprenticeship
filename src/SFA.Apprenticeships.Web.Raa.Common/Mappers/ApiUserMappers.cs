namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Api;
    using Infrastructure.Common.Mappers;
    using Resolvers;
    using ViewModels.Api;

    public class ApiUserMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<IList<ApiEndpoint>, IList<ApiEndpointViewModel>>().ConvertUsing<ApiEndpointViewModelConverter>();
            Mapper.CreateMap<IList<ApiEndpointViewModel>, IList<ApiEndpoint>>().ConvertUsing<ApiEndpointConverter>();

            Mapper.CreateMap<ApiUser, ApiUserViewModel>()
                .ForMember(dest => dest.ApiEndpoints, opt => opt.MapFrom(src => src.AuthorisedApiEndpoints));
            Mapper.CreateMap<ApiUserViewModel, ApiUser>()
                .ForMember(dest => dest.AuthorisedApiEndpoints, opt => opt.MapFrom(src => src.ApiEndpoints));
        }
    }
}