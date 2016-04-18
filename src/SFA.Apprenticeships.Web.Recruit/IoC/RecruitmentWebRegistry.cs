namespace SFA.Apprenticeships.Web.Recruit.IoC
{
    using System.Web;
    using Application.Communication;
    using Application.Communication.Strategies;
    using Application.Employer;
    using Application.Employer.Strategies;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Organisations;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Application.Location;
    using Application.Organisation;
    using Application.Provider;
    using Application.ReferenceData;
    using Application.UserAccount;
    using Application.UserAccount.Strategies.ProviderUserAccount;
    using Common.Configuration;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Common.IoC;
    using Infrastructure.Logging.IoC;
    using Mappers;
    using Mediators.Application;
    using Mediators.Provider;
    using Mediators.ProviderUser;
    using Mediators.VacancyPosting;
    using Raa.Common.Mappers;
    using Raa.Common.Providers;

    using SFA.Apprenticeships.Web.Recruit.Mediators.Home;

    using StructureMap;
    using StructureMap.Configuration.DSL;

    public class RecruitmentWebRegistry : Registry
    {
        public RecruitmentWebRegistry()
        {
            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));
            For<IMapper>().Singleton().Use<RaaCommonWebMappers>().Name = "RaaCommonWebMappers";
            For<IMapper>().Singleton().Use<RecruitMappers>().Name = "RecruitMappers";

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
            For<IProviderProvider>().Use<ProviderProvider>();
            For<IEmployerProvider>().Use<EmployerProvider>();
            For<IVacancyPostingProvider>().Use<VacancyProvider>().Ctor<IMapper>().Named("RaaCommonWebMappers");
            For<IProviderUserProvider>().Use<ProviderUserProvider>();
            For<IProviderMediator>().Use<ProviderMediator>();
            For<IApplicationProvider>().Use<ApplicationProvider>().Ctor<IMapper>().Named("RecruitMappers");
            For<ILocationsProvider>().Use<LocationsProvider>();
        }

        private void RegisterServices()
        {
            For<IOrganisationService>().Use<OrganisationService>();
            For<IProviderCommunicationService>().Use<ProviderCommunicationService>();
            For<IReferenceDataService>().Use<ReferenceDataService>();
            For<IProviderService>().Use<ProviderService>();
            For<IEmployerService>().Use<EmployerService>();
            For<IAddressSearchService>().Use<AddressSearchService>();
            For<ICommunicationService>().Use<CommunicationService>();
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

            For<IGetByIdStrategy>().Use<GetByIdStrategy>();
            For<IGetByIdsStrategy>().Use<GetByIdsStrategy>();
            For<IGetByEdsUrnStrategy>().Use<GetByEdsUrnStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<IGetPagedEmployerSearchResultsStrategy>().Use<GetPagedEmployerSearchResultsStrategy>().Ctor<IMapper>().Named("EmployerMappers");
            For<ISaveEmployerStrategy>().Use<SaveEmployerStrategy>();
            For<ISubmitContactMessageStrategy>().Use<SubmitContactMessageStrategy>();                      
        }

        private void RegisterMediators()
        {
            For<IProviderMediator>().Use<ProviderMediator>();
            For<IProviderUserMediator>().Use<ProviderUserMediator>();
            For<IVacancyPostingMediator>().Use<VacancyPostingMediator>();
            For<IApplicationMediator>().Use<ApplicationMediator>();
            For<IApprenticeshipApplicationMediator>().Use<ApprenticeshipApplicationMediator>();
            For<ITraineeshipApplicationMediator>().Use<TraineeshipApplicationMediator>();
            For<IHomeMediator>().Use<HomeMediator>();            
        }
    }
}