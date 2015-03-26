namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using Candidate.Mediators.Login;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Configuration;
    using Common.Providers;
    using Domain.Interfaces.Configuration;
    using Moq;

    public class LoginMediatorBuilder
    {
        private Mock<IUserDataProvider> _userDataProvider;
        private Mock<ICandidateServiceProvider> _candidateServiceProvider;
        private Mock<IConfigurationService> _configurationService;

        public LoginMediatorBuilder()
        {
            _userDataProvider = new Mock<IUserDataProvider>();
            _candidateServiceProvider = new Mock<ICandidateServiceProvider>();
        }

        public LoginMediatorBuilder With(Mock<IUserDataProvider> userDataProvider)
        {
            _userDataProvider = userDataProvider;
            return this;
        }

        public LoginMediatorBuilder With(Mock<ICandidateServiceProvider> candidateServiceProvider)
        {
            _candidateServiceProvider = candidateServiceProvider;
            return this;
        }

        public LoginMediatorBuilder With(Mock<IConfigurationService> configurationService)
        {
            _configurationService = configurationService;
            return this;
        }

        public LoginMediator Build()
        {
            if (_configurationService == null)
            {
                _configurationService = new Mock<IConfigurationService>();
                _configurationService.Setup(x => x.Get<WebConfiguration>(WebConfiguration.ConfigurationName))
                    .Returns(new WebConfiguration() {VacancyResultsPerPage = 5});
            }

            var mediator = new LoginMediator(_userDataProvider.Object, _candidateServiceProvider.Object, _configurationService.Object, new LoginViewModelServerValidator(), new AccountUnlockViewModelServerValidator(), new ResendAccountUnlockCodeViewModelServerValidator());
            return mediator;
        }
    }
}