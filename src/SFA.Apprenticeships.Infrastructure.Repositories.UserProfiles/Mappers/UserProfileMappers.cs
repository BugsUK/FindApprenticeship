namespace SFA.Apprenticeships.Infrastructure.Repositories.UserProfiles.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Users;
    using Entities;

    public class UserProfileMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ProviderUser, MongoProviderUser>();
            Mapper.CreateMap<MongoProviderUser, ProviderUser>();

            Mapper.CreateMap<AgencyUser, MongoAgencyUser>();
            Mapper.CreateMap<MongoAgencyUser, AgencyUser>();
        }
    }
}