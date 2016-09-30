namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using Domain.Entities.Raa.Api;
    using Infrastructure.Common.Mappers;
    using ViewModels.Api;

    public class ApiUserMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApiUser, ApiUserViewModel>();
            Mapper.CreateMap<ApiUserViewModel, ApiUser>();
        }
    }
}