namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Account
{
    using Application.Interfaces.Candidates;
    using Candidate.Mediators.Account;
    using Candidate.Providers;
    using Candidate.Validators;
    using Moq;

    using SFA.Apprenticeships.Application.Interfaces;

    internal class AccountMediatorBuilder
    {

        private Mock<ICandidateService> _candidateServiceMock = new Mock<ICandidateService>();
        private Mock<IApprenticeshipApplicationProvider> _apprenticeshipApplicationProviderMock = new Mock<IApprenticeshipApplicationProvider>();
        private Mock<IApprenticeshipVacancyProvider> _apprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
        private Mock<ITraineeshipVacancyProvider> _traineeshipVacancyProvider = new Mock<ITraineeshipVacancyProvider>();
        private IAccountProvider _accountProvider = new Mock<IAccountProvider>().Object;
        private Mock<ICandidateServiceProvider> _candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
        private Mock<VerifyMobileViewModelServerValidator> _verifyMobileViewModelServerValidatorMock = new Mock<VerifyMobileViewModelServerValidator>();
        private readonly VerifyUpdatedEmailViewModelServerValidator _verifyUpdatedEmailViewModelServerValidatorMock = new VerifyUpdatedEmailViewModelServerValidator();
        private readonly EmailViewModelServerValidator _emailViewModelServerValidatorMock = new EmailViewModelServerValidator();
        private Mock<IConfigurationService> _configurationServiceMock = new Mock<IConfigurationService>();
        private readonly SettingsViewModelServerValidator _settingsViewModelServerValidator = new SettingsViewModelServerValidator();
        private readonly DeleteAccountSettingsViewModelServerValidator _deleteAccountSettingsViewModelServerValidatorMock = new DeleteAccountSettingsViewModelServerValidator();

        public AccountMediatorBuilder With(Mock<IApprenticeshipVacancyProvider> apprenticeshipVacancyProvider)
        {
            _apprenticeshipVacancyProvider = apprenticeshipVacancyProvider;
            return this;
        }

        public AccountMediatorBuilder With(Mock<IApprenticeshipApplicationProvider> apprenticeshipApplicationProvider)
        {
            _apprenticeshipApplicationProviderMock = apprenticeshipApplicationProvider;
            return this;
        }

        public AccountMediatorBuilder With(Mock<VerifyMobileViewModelServerValidator> verifyMobileViewModelServerValidator)
        {
            _verifyMobileViewModelServerValidatorMock = verifyMobileViewModelServerValidator;
            return this;
        }

        public AccountMediatorBuilder With(Mock<ITraineeshipVacancyProvider> traineeshipVacancyProvider)
        {
            _traineeshipVacancyProvider = traineeshipVacancyProvider;
            return this;
        }

        public AccountMediatorBuilder With(IAccountProvider accountProvider)
        {
            _accountProvider = accountProvider;
            return this;
        }

        public AccountMediatorBuilder With(Mock<ICandidateServiceProvider> candidateServiceProvider)
        {
            _candidateServiceProviderMock = candidateServiceProvider;
            return this;
        }

        public AccountMediatorBuilder With(Mock<IConfigurationService> configurationService)
        {
            _configurationServiceMock = configurationService;
            return this;
        }

        public IAccountMediator Build()
        {
            return new AccountMediator(_accountProvider,
                _candidateServiceProviderMock.Object,
                _settingsViewModelServerValidator,
                _apprenticeshipApplicationProviderMock.Object,
                _apprenticeshipVacancyProvider.Object,
                _traineeshipVacancyProvider.Object,
                _configurationServiceMock.Object,
                _verifyMobileViewModelServerValidatorMock.Object,
                _emailViewModelServerValidatorMock,
                _verifyUpdatedEmailViewModelServerValidatorMock,
                _candidateServiceMock.Object, _deleteAccountSettingsViewModelServerValidatorMock);
        }
    }
}
