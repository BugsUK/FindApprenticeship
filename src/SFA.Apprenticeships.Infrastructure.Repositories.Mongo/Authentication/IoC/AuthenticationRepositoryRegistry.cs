namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Authentication.IoC
{
    using Domain.Interfaces.Repositories;
    using Mappers;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class AuthenticationRepositoryRegistry : Registry
    {
        public AuthenticationRepositoryRegistry()
        {
            For<IMapper>().Singleton().Use<AuthenticationMappers>().Name = "AuthenticationMappers";

            For<IAuthenticationRepository>()
                .Use<AuthenticationRepository>()
                .Ctor<IMapper>()
                .Named("AuthenticationMappers");
        }
    }
}