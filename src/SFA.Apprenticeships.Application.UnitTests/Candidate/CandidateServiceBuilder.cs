namespace SFA.Apprenticeships.Application.UnitTests.Candidate
{
    using Apprenticeships.Application.Candidate;
    using Apprenticeships.Application.Candidate.Strategies;
    using Apprenticeships.Application.Candidate.Strategies.Apprenticeships;
    using Apprenticeships.Application.Candidate.Strategies.SavedSearches;
    using Apprenticeships.Application.Candidate.Strategies.SuggestedVacancies;
    using Apprenticeships.Application.Candidate.Strategies.Traineeships;
    using Apprenticeships.Application.UserAccount.Strategies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Repositories;
    using Interfaces.Candidates;
    using Moq;
    using SFA.Infrastructure.Interfaces;

    internal class CandidateServiceBuilder
    {
        private Mock<IConfigurationService> _configurationService;
        private Mock<ISubmitApprenticeshipApplicationStrategy> _submitLegacyApplicationStrategy;
        private Mock<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>> _candidateApprenticeshipVacancyDetailsStrategy;
        private Mock<ISubmitApprenticeshipApplicationStrategy> _submitRaaApplicationStrategy;
        private Mock<ISubmitTraineeshipApplicationStrategy> _submitLegacyTraineeshipApplicationStrategy;
        private Mock<ISaveTraineeshipApplicationStrategy> _saveTraineeshipApplicationStrategy;
        private Mock<ISubmitTraineeshipApplicationStrategy> _submitRaaTraineeshipApplicationStrategy;

        public CandidateServiceBuilder()
        {
            _configurationService = new Mock<IConfigurationService>();
            _submitLegacyApplicationStrategy = new Mock<ISubmitApprenticeshipApplicationStrategy>();
            _submitRaaApplicationStrategy = new Mock<ISubmitApprenticeshipApplicationStrategy>();
            _candidateApprenticeshipVacancyDetailsStrategy = new Mock<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
            _submitLegacyTraineeshipApplicationStrategy = new Mock<ISubmitTraineeshipApplicationStrategy>();
            _saveTraineeshipApplicationStrategy = new Mock<ISaveTraineeshipApplicationStrategy>();
            _submitRaaTraineeshipApplicationStrategy = new Mock<ISubmitTraineeshipApplicationStrategy>();
        }

        public CandidateServiceBuilder With(Mock<IConfigurationService> configurationService)
        {
            _configurationService = configurationService;
            return this;
        }


        public CandidateServiceBuilder WithSubmitLegacy(Mock<ISubmitApprenticeshipApplicationStrategy> submitLegacyApplicationStrategy)
        {
            _submitLegacyApplicationStrategy = submitLegacyApplicationStrategy;
            return this;
        }

        public CandidateServiceBuilder WithSubmitRaa(Mock<ISubmitApprenticeshipApplicationStrategy> submitLegacyApplicationStrategy)
        {
            _submitRaaApplicationStrategy = submitLegacyApplicationStrategy;
            return this;
        }

        public CandidateServiceBuilder With(Mock<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>> candidateApprenticeshipVacancyDetailStrategy)
        {
            _candidateApprenticeshipVacancyDetailsStrategy = candidateApprenticeshipVacancyDetailStrategy;
            return this;
        }

        public CandidateServiceBuilder WithSubmitTraineeshipLegacy(Mock<ISubmitTraineeshipApplicationStrategy> submitLegacyTraineeshipApplicationStrategy)
        {
            _submitLegacyTraineeshipApplicationStrategy = submitLegacyTraineeshipApplicationStrategy;
            return this;
        }

        public CandidateServiceBuilder With(Mock<ISaveTraineeshipApplicationStrategy> saveTraineeshipApplicationStrategy)
        {
            _saveTraineeshipApplicationStrategy = saveTraineeshipApplicationStrategy;
            return this;
        }

        public CandidateServiceBuilder WithSubmitTraineeshipRaa(Mock<ISubmitTraineeshipApplicationStrategy> submitRaaTraineeshipApplicationStrategy)
        {
            _submitRaaTraineeshipApplicationStrategy = submitRaaTraineeshipApplicationStrategy;
            return this;
        }

        public ICandidateService Build()
        {
            return new CandidateService(
                new Mock<IGetCandidateByIdStrategy>().Object,
                new Mock<IApprenticeshipApplicationReadRepository>().Object,
                new Mock<IActivateCandidateStrategy>().Object, 
                new Mock<IAuthenticateCandidateStrategy>().Object, 
                _submitRaaApplicationStrategy.Object, 
                _submitLegacyApplicationStrategy.Object, 
                new Mock<IRegisterCandidateStrategy>().Object, 
                new Mock<ISaveApprenticeshipVacancyStrategy>().Object, 
                new Mock<IDeleteSavedApprenticeshipVacancyStrategy>().Object, 
                new Mock<ICreateDraftApprenticeshipFromSavedVacancyStrategy>().Object, 
                new Mock<ICreateApprenticeshipApplicationStrategy>().Object, 
                new Mock<ICreateTraineeshipApplicationStrategy>().Object, 
                new Mock<IGetCandidateApprenticeshipApplicationsStrategy>().Object, 
                new Mock<IResetForgottenPasswordStrategy>().Object, 
                new Mock<IUnlockAccountStrategy>().Object, 
                new Mock<ISaveApprenticeshipApplicationStrategy>().Object, 
                new Mock<IArchiveApplicationStrategy>().Object, 
                new Mock<IDeleteApplicationStrategy>().Object, 
                new Mock<ISaveCandidateStrategy>().Object, 
                _submitLegacyTraineeshipApplicationStrategy.Object,
                _submitRaaTraineeshipApplicationStrategy.Object,
                _saveTraineeshipApplicationStrategy.Object, 
                new Mock<ITraineeshipApplicationReadRepository>().Object, 
                new Mock<IGetCandidateTraineeshipApplicationsStrategy>().Object, 
                _candidateApprenticeshipVacancyDetailsStrategy.Object, 
                new Mock<ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>().Object, 
                new Mock<ISendMobileVerificationCodeStrategy>().Object, 
                new Mock<ILogService>().Object, 
                new Mock<IVerifyMobileStrategy>().Object, 
                new Mock<ISubmitContactMessageStrategy>().Object, 
                new Mock<ICreateSavedSearchStrategy>().Object, 
                new Mock<IRetrieveSavedSearchesStrategy>().Object, 
                new Mock<IUpdateSavedSearchStrategy>().Object, 
                new Mock<IDeleteSavedSearchStrategy>().Object, 
                new Mock<Apprenticeships.Application.Candidate.Strategies.IUpdateUsernameStrategy>().Object, 
                new Mock<IRequestEmailReminderStrategy>().Object, 
                new Mock<IUnsubscribeStrategy>().Object, 
                new Mock<IApprenticeshipVacancySuggestionsStrategy>().Object, 
                new Mock<IGetCandidateByUsernameStrategy>().Object, 
                _configurationService.Object);
        }
    }
}