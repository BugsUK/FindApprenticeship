namespace SFA.Apprenticeships.Application.UnitTests.UserAccount.Builders
{
    using Apprenticeships.Application.UserAccount.Configuration;
    using Apprenticeships.Application.UserAccount.Strategies;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using Moq;

    public class RegisterUserStrategyBuilder
    {
        private readonly Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private Mock<IUserReadRepository> _userReadRepository = new Mock<IUserReadRepository>();
        private Mock<IUserWriteRepository> _userWriteRepository = new Mock<IUserWriteRepository>();

        public RegisterUserStrategyBuilder()
        {
            _configurationService.Setup(s => s.Get<UserAccountConfiguration>())
                .Returns(new UserAccountConfiguration {ActivationCodeExpiryDays = 30});
        }

        public IRegisterUserStrategy Build()
        {
            var strategy = new RegisterUserStrategy(_userWriteRepository.Object, _configurationService.Object, _userReadRepository.Object);
            return strategy;
        }

        public RegisterUserStrategyBuilder With(Mock<IUserReadRepository> userReadRepository)
        {
            _userReadRepository = userReadRepository;
            return this;
        }

        public RegisterUserStrategyBuilder With(Mock<IUserWriteRepository> userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
            return this;
        }
    }
}