namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Users.IoC
{
    using Domain.Interfaces.Repositories;
    using Mappers;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class UserRepositoryRegistry : Registry
    {
        public UserRepositoryRegistry()
        {
            For<IMapper>().Use<UserMappers>().Name = "UserMapper";
            For<IUserReadRepository>().Use<UserRepository>().Ctor<IMapper>().Named("UserMapper");
            For<IUserWriteRepository>().Use<UserRepository>().Ctor<IMapper>().Named("UserMapper");
        }
    }
}
