namespace SFA.Apprenticeships.Application.UnitTests.UserAccount.Builders
{
    using Apprenticeships.Application.UserAccount;
    using Apprenticeships.Application.UserAccount.Strategies;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;
    using Moq;
    using SFA.Infrastructure.Interfaces;

    public class UserAccountServiceBuilder
    {
        private readonly Mock<IActivateUserStrategy> _activateUserStrategy = new Mock<IActivateUserStrategy>();
        private readonly Mock<IRegisterUserStrategy> _registerUserStrategy = new Mock<IRegisterUserStrategy>();
        private readonly Mock<ISendAccountUnlockCodeStrategy> _resendAccountUnlockCodeStrategy = new Mock<ISendAccountUnlockCodeStrategy>();
        private readonly Mock<IResendActivationCodeStrategy> _resendActivationCodeStrategy = new Mock<IResendActivationCodeStrategy>();
        private readonly Mock<IResetForgottenPasswordStrategy> _resetForgottenPasswordStrategy = new Mock<IResetForgottenPasswordStrategy>();
        private readonly Mock<ISendPasswordResetCodeStrategy> _sendPasswordCodeStrategy = new Mock<ISendPasswordResetCodeStrategy>();
        private readonly Mock<IUnlockAccountStrategy> _unlockAccountStrategy = new Mock<IUnlockAccountStrategy>();
        private readonly Mock<IUpdateUsernameStrategy> _updateUsernameStrategy = new Mock<IUpdateUsernameStrategy>();
        private readonly Mock<ISendPendingUsernameCodeStrategy> _sendPendingUsernameCodeStrategy = new Mock<ISendPendingUsernameCodeStrategy>();
        private Mock<IUserReadRepository> _userReadRepository = new Mock<IUserReadRepository>();
        private readonly Mock<ILogService> _logger = new Mock<ILogService>();

        public IUserAccountService Build()
        {
            var service = new UserAccountService(_userReadRepository.Object, _registerUserStrategy.Object,
                _activateUserStrategy.Object, _resetForgottenPasswordStrategy.Object, _sendPasswordCodeStrategy.Object,
                _resendActivationCodeStrategy.Object, _resendAccountUnlockCodeStrategy.Object,
                _unlockAccountStrategy.Object, _updateUsernameStrategy.Object, _sendPendingUsernameCodeStrategy.Object,
                _logger.Object);
            return service;
        }

        public UserAccountServiceBuilder With(Mock<IUserReadRepository> userReadRepository)
        {
            _userReadRepository = userReadRepository;
            return this;
        }
    }
}