namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.UserProfiles.IoC
{
    using Mappers;

    using SFA.Apprenticeships.Application.Interfaces;
    using StructureMap.Configuration.DSL;

    public class UserProfileRepositoryRegistry : Registry
    {
        public UserProfileRepositoryRegistry()
        {
            For<IMapper>().Use<UserProfileMappers>().Name = "UserProfileMapper";
            //For<IProviderUserReadRepository>().Use<UserProfileRepository>().Ctor<IMapper>().Named("UserProfileMapper");
            //For<IProviderUserWriteRepository>().Use<UserProfileRepository>().Ctor<IMapper>().Named("UserProfileMapper");
            //For<IAgencyUserReadRepository>().Use<AgencyUserRepository>().Ctor<IMapper>().Named("UserProfileMapper");
            //For<IAgencyUserWriteRepository>().Use<AgencyUserRepository>().Ctor<IMapper>().Named("UserProfileMapper");
        }
    }
}