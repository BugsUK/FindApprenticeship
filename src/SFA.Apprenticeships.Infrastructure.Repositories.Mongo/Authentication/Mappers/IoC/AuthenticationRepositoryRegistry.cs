namespace SFA.Apprenticeships.Infrastructure.Repositories.Authentication.IoC
{
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class AuthenticationRepositoryRegistry : Registry
    {
        public AuthenticationRepositoryRegistry()
        {
            For<IMapper>().Use<AuthenticationMappers>().Name = "AuthenticationMappers";

            For<IAuthenticationRepository>()
                .Use<AuthenticationRepository>()
                .Ctor<IMapper>()
                .Named("AuthenticationMappers");
        }
    }
}