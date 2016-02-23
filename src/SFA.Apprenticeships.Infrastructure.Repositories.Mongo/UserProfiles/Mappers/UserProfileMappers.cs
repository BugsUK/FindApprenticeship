namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.UserProfiles.Mappers
{
    using Domain.Entities.Raa.Users;
    using Infrastructure.Common.Mappers;
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