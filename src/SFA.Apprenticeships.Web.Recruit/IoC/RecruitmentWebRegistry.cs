namespace SFA.Apprenticeships.Web.Recruit.IoC
{
    using System.Web;
    using Application.Interfaces.Organisations;
    using Application.Organisation;
    using Mediators.Provider;
    using Mediators.ProviderUser;
    using StructureMap.Configuration.DSL;

    public class RecruitmentWebRegistry : Registry
    {
        public RecruitmentWebRegistry()
        {
            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));

            RegisterServices();
            RegisterStrategies();
            RegisterProviders();
            RegisterMediators();
        }

        private void RegisterProviders()
        {
        }

        private void RegisterServices()
        {
            For<IOrganisationService>().Use<OrganisationService>();
        }

        private void RegisterStrategies()
        {
        }

        private void RegisterMediators()
        {
            For<IProviderMediator>().Use<ProviderMediator>();
            For<IProviderUserMediator>().Use<ProviderUserMediator>();
        }
    }
}
