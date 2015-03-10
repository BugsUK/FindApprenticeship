﻿namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using System.Web;
    using Application.Address;
    using Application.Applications;
    using Application.Applications.Strategies;
    using Application.Authentication;
    using Application.Candidate;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
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
    using Configuration;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Mapping;
    using Mappers;
    using Mediators.Account;
    using Mediators.Application;
    using Mediators.Home;
    using Mediators.Login;
    using Mediators.Register;
    using Mediators.Search;
    using Microsoft.WindowsAzure;
    using Providers;
    using StructureMap.Configuration.DSL;
    using ISendPasswordResetCodeStrategy = Application.UserAccount.Strategies.ISendPasswordResetCodeStrategy;

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
            For<IMapper>().Singleton().Use<HomeWebMappers>().Name = "HomeWebMappers";

            For<IFeatureToggle>().Use<FeatureToggle>();

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
            For<IApprenticeshipVacancyDetailProvider>().Use<ApprenticeshipVacancyDetailProvider>().Ctor<IMapper>().Named("ApprenticeshipCandidateWebMappers");
            For<IApprenticeshipApplicationProvider>().Use<ApprenticeshipApplicationProvider>().Ctor<IMapper>().Named("ApprenticeshipCandidateWebMappers");
            For<IAccountProvider>().Use<AccountProvider>().Ctor<IMapper>().Named("ApprenticeshipCandidateWebMappers");
            For<ICandidateServiceProvider>().Use<CandidateServiceProvider>().Ctor<IMapper>().Named("ApprenticeshipCandidateWebMappers");
            For<ITraineeshipVacancyDetailProvider>().Use<TraineeshipVacancyDetailProvider>().Ctor<IMapper>().Named("TraineeshipCandidateWebMappers");
            For<ITraineeshipApplicationProvider>().Use<TraineeshipApplicationProvider>().Ctor<IMapper>().Named("TraineeshipCandidateWebMappers");
            For<IHomeProvider>().Use<HomeProvider>().Ctor<IMapper>().Named("HomeWebMappers");
        }

        private void RegisterServices()
        {
            For<ILocationSearchService>().Use<LocationSearchService>();
            For<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>().Use<VacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>();
            For<IVacancySearchService<TraineeshipSearchResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters>>().Use<VacancySearchService<TraineeshipSearchResponse, TraineeshipVacancyDetail, TraineeshipSearchParameters>>();
            For<ICandidateService>().Use<CandidateService>();
            For<IUserAccountService>().Use<UserAccountService>();
            For<IAddressSearchService>().Use<AddressSearchService>();
            For<IAuthenticationService>().Use<AuthenticationService>();
            For<ICommunicationService>().Use<CommunicationService>();
            For<IReferenceDataService>().Use<ReferenceDataService>();
        }

        private void RegisterStrategies()
        {
            var codeGenerator = CloudConfigurationManager.GetSetting("CodeGenerator");

            For<IGetCandidateApprenticeshipApplicationsStrategy>().Use<LegacyGetCandidateApprenticeshipApplicationsStrategy>();
            For<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>().Use<LegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
            For<ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>().Use<LegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>();
            For<ISendCandidateCommunicationStrategy>().Use<QueueCandidateCommunicationStrategy>();
            For<IActivateCandidateStrategy>().Use<QueuedLegacyActivateCandidateStrategy>();
            For<IRegisterCandidateStrategy>().Use<RegisterCandidateStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);
            For<IRegisterUserStrategy>().Use<RegisterUserStrategy>();
            For<IActivateUserStrategy>().Use<ActivateUserStrategy>();
            For<IResetForgottenPasswordStrategy>().Use<ResetForgottenPasswordStrategy>().Name = "ResetForgottenPasswordStrategy";
            For<IResetForgottenPasswordStrategy>().Use<LegacyResetForgottenPasswordStrategy>().Ctor<IResetForgottenPasswordStrategy>().Named("ResetForgottenPasswordStrategy").Name = "LegacyResetForgottenPasswordStrategy";
            For<IUnlockAccountStrategy>().Use<UnlockAccountStrategy>().Name = "UnlockAccountStrategy";
            For<IUnlockAccountStrategy>().Use<LegacyUnlockAccountStrategy>().Ctor<IUnlockAccountStrategy>().Named("UnlockAccountStrategy").Name = "LegacyUnlockAccountStrategy";
            For<ISendPasswordResetCodeStrategy>().Use<SendPasswordResetCodeStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);
            For<ISubmitApprenticeshipApplicationStrategy>().Use<LegacySubmitApprenticeshipApplicationStrategy>();
            For<ISubmitTraineeshipApplicationStrategy>().Use<LegacySubmitTraineeshipApplicationStrategy>();
            For<ISendApplicationSubmittedStrategy>().Use<LegacyQueueApprenticeshipApplicationSubmittedStrategy>();
            For<ISendTraineeshipApplicationSubmittedStrategy>().Use<LegacyQueueTraineeshipApplicationSubmittedStrategy>();
            For<IResendActivationCodeStrategy>().Use<ResendActivationCodeStrategy>().Ctor<ICodeGenerator>().Named(codeGenerator);
            For<ISendAccountUnlockCodeStrategy>().Use<SendAccountUnlockCodeStrategy>();
            For<ISaveCandidateStrategy>().Use<SaveCandidateStrategy>();
            For<ISendMobileVerificationCodeStrategy>().Use<SendMobileVerificationCodeStrategy>();
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
        }
    }
}
