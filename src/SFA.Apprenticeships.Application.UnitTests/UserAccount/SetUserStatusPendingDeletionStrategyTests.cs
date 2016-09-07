namespace SFA.Apprenticeships.Application.UnitTests.UserAccount
{
    using Builders;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Interfaces;
    using Moq;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class SetUserStatusPendingDeletionStrategyTests
    {
        private readonly Mock<IUserWriteRepository> _userWriteRepository = new Mock<IUserWriteRepository>();
        private readonly Mock<IAuditRepository> _auditRepository = new Mock<IAuditRepository>();
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();
        [Test]
        public void SetUserStatusPendingDeletionSuccess()
        {
            var user = new User
            {
                Status = UserStatuses.Active,
                EntityId = new Guid()
            };

            _userWriteRepository.Setup(uw => uw.SoftDelete(user));
            var setUserStatusPendingDeletionStrategy = new SetUserStatusDeletionPendingStrategyBuilder()
                .With(_userWriteRepository).With(_auditRepository).With(_logService).Build();
            user.Status = UserStatuses.PendingDeletion;
            setUserStatusPendingDeletionStrategy.SetUserStatusPendingDeletion(user);
            _userWriteRepository.Verify(uwr => uwr.SoftDelete(It.Is<User>(u =>
               u.Status == UserStatuses.PendingDeletion
           )));

        }
    }
}
