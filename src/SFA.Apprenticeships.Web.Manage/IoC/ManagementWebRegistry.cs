namespace SFA.Apprenticeships.Web.Manage.IoC
{
    using System.Web;
    using Application.Application;
    using Application.Communication;
    using Application.Communication.Strategies;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Organisations;
    using Application.Interfaces.Users;
    using Application.Interfaces.VacancyPosting;
    using Application.Organisation;
    using Application.UserAccount;
    using Application.UserAccount.Strategies.ProviderUserAccount;
    using Application.VacancyPosting;
    using Common.Configuration;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Infrastructure.Common.IoC;
    using Infrastructure.Logging.IoC;
    using Infrastructure.TacticalDataServices;
    using Mediators.AgencyUser;
    using Mediators.Vacancy;
    using Providers;
    using Raa.Common.Mappers;
    using StructureMap;
    using StructureMap.Configuration.DSL;
    using Application.Interfaces.ReferenceData;
    using Application.ReferenceData;
    using Raa.Common.Providers;

    public class ManagementWebRegistry : Registry
    {
        public ManagementWebRegistry()
        {
            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));
            For<IMapper>().Singleton().Use<RaaCommonWebMappers>().Name = "RaaCommonWebMappers";

            RegisterCodeGenerators();
            RegisterServices();
            RegisterStrategies();
            RegisterProviders();
            RegisterMediators();
        }

        private void RegisterCodeGenerators()
        {
            For<ICodeGenerator>().Use<RandomCodeGenerator>().Name = "RandomCodeGenerator";
            For<ICodeGenerator>().Use<StaticCodeGenerator>().Name = "StaticCodeGenerator";
        }

        private void RegisterProviders()
        {
            For<ILegacyProviderProvider>().Use<LegacyProviderProvider>();
            For<ILegacyEmployerProvider>().Use<LegacyEmployerProvider>();
            For<IAgencyUserProvider>().Use<AgencyUserProvider>();
            For<IVacancyQAProvider>().Use<VacancyProvider>();
        }

        private void RegisterServices()
        {
            For<IOrganisationService>().Use<OrganisationService>();
            For<IReferenceDataService>().Use<ReferenceDataService>();
            For<IProviderCommunicationService>().Use<ProviderCommunicationService>();
            For<IVacancyPostingService>().Use<VacancyPostingService>();
            For<IApplicationService>().Use<ApplicationService>();
        }

        private void RegisterStrategies()
        {
            var settingsContainer = new Container(x =>
            {
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommonRegistry>();
            });

            var configurationService = settingsContainer.GetInstance<IConfigurationService>();
            var codeGenerator = configurationService.Get<CommonWebConfiguration>().CodeGenerator;

            For<ISendProviderUserCommunicationStrategy>().Use<QueueProviderUserCommunicationStrategy>();
            For<ISendEmailVerificationCodeStrategy>().Use<SendEmailVerificationCodeStrategy>()
                .Ctor<ICodeGenerator>().Named(codeGenerator);
            For<IResendEmailVerificationCodeStrategy>().Use<ResendEmailVerificationCodeStrategy>();
        }

        private void RegisterMediators()
        {
            For<IAgencyUserMediator>().Use<AgencyUserMediator>();
            For<IVacancyMediator>().Use<VacancyMediator>();
        }
    }
}
