namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using SFA.Infrastructure.Interfaces;
    using Candidate.Mediators.Login;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Configuration;
    using Common.Providers;
    using Common.Services;
    using Moq;

    using SFA.Apprenticeships.Application.Interfaces;

    public class LoginMediatorBuilder
    {
        private Mock<IUserDataProvider> _userDataProvider;
        private Mock<ICandidateServiceProvider> _candidateServiceProvider;
        private Mock<IConfigurationService> _configurationService;
        private readonly Mock<IAuthenticationTicketService> _authenticationTicketService;

        public LoginMediatorBuilder()
        {
            _userDataProvider = new Mock<IUserDataProvider>();
            _candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            _authenticationTicketService = new Mock<IAuthenticationTicketService>();
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

        public ILoginMediator Build()
        {
            if (_configurationService == null)
            {
                _configurationService = new Mock<IConfigurationService>();
                _configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                    .Returns(new CommonWebConfiguration {VacancyResultsPerPage = 5});
            }

            var mediator = new LoginMediator(_userDataProvider.Object, _candidateServiceProvider.Object, _configurationService.Object, new LoginViewModelServerValidator(), new AccountUnlockViewModelServerValidator(), new ResendAccountUnlockCodeViewModelServerValidator(), _authenticationTicketService.Object, new ForgottenPasswordViewModelServerValidator(), new PasswordResetViewModelServerValidator(), new ForgottenEmailViewModelServerValidator(), new Mock<ILogService>().Object);
            return mediator;
        }
    }
}