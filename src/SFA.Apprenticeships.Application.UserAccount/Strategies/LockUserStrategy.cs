namespace SFA.Apprenticeships.Application.UserAccount.Strategies
{
    using System;
    using Configuration;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces.Users;

    using SFA.Apprenticeships.Application.Interfaces;

    public class LockUserStrategy : ILockUserStrategy
    {
        private readonly UserAccountConfiguration _userAccountConfiguration;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IUserWriteRepository _userWriteRepository;

        public LockUserStrategy(
            IConfigurationService configurationService,
            ICodeGenerator codeGenerator,
            IUserWriteRepository userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
            _codeGenerator = codeGenerator;
            _userAccountConfiguration = configurationService.Get<UserAccountConfiguration>();
        }

        public void LockUser(User user)
        {
            // Create and set an unlock code, set code expiry, save user, send email containing unlock code.
            var unlockCodeExpiryDays = _userAccountConfiguration.UnlockCodeExpiryDays;

            var accountUnlockCode = _codeGenerator.GenerateAlphaNumeric();
            var expiry = DateTime.UtcNow.AddDays(unlockCodeExpiryDays);

            user.SetStateLocked(accountUnlockCode, expiry);
            _userWriteRepository.Save(user);
        }
    }
}
