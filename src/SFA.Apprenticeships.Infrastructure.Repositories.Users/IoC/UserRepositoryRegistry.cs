namespace SFA.Apprenticeships.Infrastructure.Repositories.Users.IoC
{
    using System;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using Mappers;
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
