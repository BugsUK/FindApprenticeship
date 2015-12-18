namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using System.Web;
    using Application.Applications;
    using Application.Applications.Strategies;
    using Application.Authentication;
    using Application.Candidate;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Application.Candidate.Strategies.SavedSearches;
    using Application.Candidate.Strategies.SuggestedVacancies;
    using Application.Candidate.Strategies.Traineeships;
    using Application.Communication;
    using Application.Communication.Strategies;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Communications;
    using Application.Interfaces.Locations;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Application.Interfaces.Vacancies;
    using Application.Location;
    using Application.ReferenceData;
    using Application.UserAccount;
    using Application.UserAccount.Strategies;
    using Application.Vacancy;
    using Application.Vacancy.SiteMap;
    using Common.Configuration;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Common.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.Logging.IoC;
    using Mappers;
    using Mediators.Account;
    using Mediators.Application;
    using Mediators.Home;
    using Mediators.Login;
    using Mediators.Register;
    using Mediators.Search;
    using Mediators.Unsubscribe;
    using Providers;
    using StructureMap;
    using StructureMap.Configuration.DSL;

    public class CandidateWebRegistry : Registry
    {
        public CandidateWebRegistry()
        {
            For<ICodeGenerator>().Use<RandomCodeGenerator>().Name = "RandomCodeGenerator";
            For<ICodeGenerator>().Use<StaticCodeGenerator>().Name = "StaticCodeGenerator";

            For<IApplicationStatusUpdater>().Use<ApplicationStatusUpdater>();
            For<IApplicationVacancyUpdater>().Use<ApplicationVacancyUpdater>();

            For<IMapper>().Singleton().Use<ApprenticeshipCandidateWebMappers>().Name = "ApprenticeshipCandidateWebMappers";
            For<IMapper>().Singleton().Use<TraineeshipCandidateWebMappers>().Name = "TraineeshipCandidateWebMappers";

            For<HttpContextBase>().Use(ctx => new HttpContextWrapper(HttpContext.Current));

            RegisterServices();

            RegisterStrategies();

            RegisterProviders();

            RegisterMediators();
        }

        private void RegisterProviders()
        {
            For<ISearchProvider>().Use<SearchProvider>()
                .Ctor<IMapper>("apprenticeshipSearchMapper").Named("ApprenticeshipCandidateWebMappers")
                .Ctor<IMapper>("traineeshipSearchMapper").Named("TraineeshipCandidateWebMappers");

            For<IApprenticeshipVacancyProvider>().Use<ApprenticeshipVacancyProvider>()
                .Ctor<IMapper>("apprenticeshipSearchMapper").Named("ApprenticeshipCandidateWebMappers");

            For<ITraineeshipVacancyProvider>().Use<TraineeshipVacancyProvider>()
                .Ctor<IMapper>("traineeshipSearchMapper").Named("TraineeshipCandidateWebMappers");

            For<IApprenticeshipApplicationProvider>().Use<ApprenticeshipApplicationProvider>()
                .Ctor<IMapper>().Named("ApprenticeshipCandidateWebMappers");

            For<IAccountProvider>().Use<AccountProvider>()
                .Ctor<IMapper>().Named("ApprenticeshipCandidateWebMappers");

            For<ICandidateServiceProvider>().Use<CandidateServiceProvider>()
                .Ctor<IMapper>().Named("ApprenticeshipCandidateWebMappers");

            For<ITraineeshipApplicationProvider>().Use<TraineeshipApplicationProvider>()
                .Ctor<IMapper>().Named("TraineeshipCandidateWebMappers");

            For<ISiteMapVacancyProvider>().Use<SiteMapVacancyProvider>();
        }

        private void RegisterServices()
        {
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>().Use<VacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>();
            For<IVacancySearchService<TraineeshipSearchResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters>>().Use<VacancySearchService<TraineeshipSearchResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters>>();
            For<ICandidateService>().Use<CandidateService>();
            For<IUserAccountService>().Use<UserAccountService>();
            For<IAuthenticationService>().Use<AuthenticationService>();
            For<ICommunicationService>().Use<CommunicationService>();
            For<IReferenceDataService>().Use<ReferenceDataService>();
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

            For<IGetCandidateApprenticeshipApplicationsStrategy>().Use<LegacyGetCandidateApprenticeshipApplicationsStrategy>();
            For<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>().Use<LegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
            For<ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>().Use<LegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>();
            For<ISendCandidateCommunicationStrategy>().Use<QueueCandidateCommunicationStrategy>();
            For<ISendUsernameUpdateCommunicationStrategy>().Use<QueueUsernameUpdateCommunicationStrategy>();
            For<IActivateCandidateStrategy>().Use<QueuedLegacyActivateCandidateStrategy>();
            For<IRegisterCandidateStrategy>().Use<RegisterCandidateStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);
            For<IRegisterUserStrategy>().Use<RegisterUserStrategy>();
            For<IActivateUserStrategy>().Use<ActivateUserStrategy>();
            For<IResetForgottenPasswordStrategy>().Use<ResetForgottenPasswordStrategy>().Name = "ResetForgottenPasswordStrategy";
            For<IResetForgottenPasswordStrategy>().Use<LegacyResetForgottenPasswordStrategy>().Ctor<IResetForgottenPasswordStrategy>().Named("ResetForgottenPasswordStrategy").Name = "LegacyResetForgottenPasswordStrategy";
            For<IUnlockAccountStrategy>().Use<UnlockAccountStrategy>().Name = "UnlockAccountStrategy";
            For<IUnlockAccountStrategy>().Use<LegacyUnlockAccountStrategy>().Ctor<IUnlockAccountStrategy>().Named("UnlockAccountStrategy").Name = "LegacyUnlockAccountStrategy";
            For<ISendPasswordResetCodeStrategy>().Use<SendPasswordResetCodeStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);
            For<ISendPendingUsernameCodeStrategy>().Use<SendPendingUsernameCodeStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);

            var servicesConfiguration = configurationService.Get<ServicesConfiguration>();
            if (servicesConfiguration.ServiceImplementation == "Legacy")
            {
                For<ISubmitApprenticeshipApplicationStrategy>().Use<LegacySubmitApprenticeshipApplicationStrategy>();
                For<ISubmitTraineeshipApplicationStrategy>().Use<LegacySubmitTraineeshipApplicationStrategy>();
            }
            else if(servicesConfiguration.ServiceImplementation == "Raa")
            {
                For<ISubmitApprenticeshipApplicationStrategy>().Use<SubmitApprenticeshipApplicationStrategy>();
                For<ISubmitTraineeshipApplicationStrategy>().Use<SubmitTraineeshipApplicationStrategy>();
            }

            For<ISendApplicationSubmittedStrategy>().Use<LegacyQueueApprenticeshipApplicationSubmittedStrategy>();
            For<ISendTraineeshipApplicationSubmittedStrategy>().Use<LegacyQueueTraineeshipApplicationSubmittedStrategy>();
            For<IResendActivationCodeStrategy>().Use<ResendActivationCodeStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);
            For<ISendAccountUnlockCodeStrategy>().Use<SendAccountUnlockCodeStrategy>();
            For<ISaveCandidateStrategy>().Use<SaveCandidateStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator).Name = "SaveCandidateStrategy";
            For<ISaveCandidateStrategy>().Use<QueuedLegacySaveCandidateStrategy>().Ctor<ISaveCandidateStrategy>().Named("SaveCandidateStrategy").Name = "QueuedLegacySaveCandidateStrategy";
            For<ISendMobileVerificationCodeStrategy>().Use<SendMobileVerificationCodeStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);
            For<IVerifyMobileStrategy>().Use<VerifyMobileStrategy>();
            For<ILockAccountStrategy>().Use<LockAccountStrategy>();
            For<ILockUserStrategy>().Use<LockUserStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);
            For<ICreateApprenticeshipApplicationStrategy>().Use<CreateApprenticeshipApplicationStrategy>();
            For<ICreateDraftApprenticeshipFromSavedVacancyStrategy>().Use<CreateDraftApprenticeshipFromSavedVacancyStrategy>();
            For<ISaveApprenticeshipVacancyStrategy>().Use<CreateApprenticeshipApplicationStrategy>();
            For<ICreateTraineeshipApplicationStrategy>().Use<CreateTraineeshipApplicationStrategy>();
            For<ISaveApprenticeshipApplicationStrategy>().Use<SaveApprenticeshipApplicationStrategy>();
            For<ISaveTraineeshipApplicationStrategy>().Use<SaveTraineeshipApplicationStrategy>();
            For<IArchiveApplicationStrategy>().Use<ArchiveApprenticeshipApplicationStrategy>();
            For<IDeleteApplicationStrategy>().Use<DeleteApprenticeshipApplicationStrategy>();
            For<IDeleteSavedApprenticeshipVacancyStrategy>().Use<DeleteSavedApprenticeshipApprenticeshipVacancyStrategy>();
            For<IAuthenticateCandidateStrategy>().Use<AuthenticateCandidateStrategy>();
            For<IGetCandidateTraineeshipApplicationsStrategy>().Use<GetCandidateTraineeshipApplicationsStrategy>();
            For<ISubmitContactMessageStrategy>().Use<SubmitContactMessageStrategy>();
            For<ISendContactMessageStrategy>().Use<QueueContactMessageStrategy>();
            For<IApplicationStatusAlertStrategy>().Use<ApplicationStatusAlertStrategy>();
            For<ICreateSavedSearchStrategy>().Use<CreateSavedSearchStrategy>();
            For<IRetrieveSavedSearchesStrategy>().Use<RetrieveSavedSearchesStrategy>();
            For<IUpdateSavedSearchStrategy>().Use<UpdateSavedSearchStrategy>();
            For<IDeleteSavedSearchStrategy>().Use<DeleteSavedSearchStrategy>();
            For<IRequestEmailReminderStrategy>().Use<RequestEmailReminderStrategy>();
            For<IApplicationStatusUpdateStrategy>().Use<ApplicationStatusUpdateStrategy>();
            For<IUnsubscribeStrategy>().Use<UnsubscribeStrategy>();
            For<IApprenticeshipVacancySuggestionsStrategy>().Use<ApprenticeshipVacancySuggestionsStrategy>();
            For<IGetCandidateByUsernameStrategy>().Use<GetCandidateByUsernameStrategy>();

            For<Application.Candidate.Strategies.IUpdateUsernameStrategy>().Use<Application.Candidate.Strategies.UpdateUsernameStrategy>().Ctor<ISaveCandidateStrategy>().Named("QueuedLegacySaveCandidateStrategy").Ctor<ICodeGenerator>().Named(codeGenerator);
            For<Application.UserAccount.Strategies.IUpdateUsernameStrategy>().Use<Application.UserAccount.Strategies.UpdateUsernameStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);
        }

        private void RegisterMediators()
        {
            For<IApprenticeshipApplicationMediator>().Use<ApprenticeshipApplicationMediator>();
            For<IApprenticeshipSearchMediator>().Use<ApprenticeshipSearchMediator>();
            For<ITraineeshipApplicationMediator>().Use<TraineeshipApplicationMediator>();
            For<ITraineeshipSearchMediator>().Use<TraineeshipSearchMediator>();
            For<IAccountMediator>().Use<AccountMediator>();
            For<IRegisterMediator>().Use<RegisterMediator>();
            For<ILoginMediator>().Use<LoginMediator>();
            For<IHomeMediator>().Use<HomeMediator>();
            For<IUnsubscribeMediator>().Use<UnsubscribeMediator>();
        }
    }
}
