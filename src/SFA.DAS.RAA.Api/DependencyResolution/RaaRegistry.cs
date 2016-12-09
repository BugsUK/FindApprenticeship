namespace SFA.DAS.RAA.Api.DependencyResolution
{
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Application.Provider;
    using Services;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class RaaRegistry : Registry
    {
        public RaaRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

            For<IProviderService>().Use<ProviderService>();
            For<IProviderVacancyAuthorisationService>().Use<ProviderVacancyAuthorisationService>();

            For<IAuthenticationService>().Use<ApiKeyAuthenticationService>();
        }
    }
}