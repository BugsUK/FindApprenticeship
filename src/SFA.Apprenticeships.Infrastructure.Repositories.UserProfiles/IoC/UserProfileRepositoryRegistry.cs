namespace SFA.Apprenticeships.Infrastructure.Repositories.UserProfiles.IoC
{
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class UserProfileRepositoryRegistry : Registry
    {
        public UserProfileRepositoryRegistry()
        {
            For<IMapper>().Use<UserProfileMappers>().Name = "UserProfileMapper";
            For<IProviderUserReadRepository>().Use<UserProfileRepository>().Ctor<IMapper>().Named("UserProfileMapper");
            For<IProviderUserWriteRepository>().Use<UserProfileRepository>().Ctor<IMapper>().Named("UserProfileMapper");
        }
    }
}