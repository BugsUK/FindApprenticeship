namespace SFA.Apprenticeships.Application.UnitTests.UserAccount.Builders
{
    using Apprenticeships.Application.Candidate.Strategies.Candidates;
    using Domain.Interfaces.Repositories;
    using Interfaces;
    using Moq;

    public class SetUserStatusDeletionPendingStrategyBuilder
    {
        private Mock<IUserWriteRepository> _userWriteRepository = new Mock<IUserWriteRepository>();
        private Mock<IAuditRepository> _auditRepository = new Mock<IAuditRepository>();
        private Mock<ILogService> _logService = new Mock<ILogService>();

        public ISetUserStatusPendingDeletionStrategy Build()
        {
            var service = new SetUserStatusPendingDeletionStrategy(_userWriteRepository.Object, _auditRepository.Object, _logService.Object);
            return service;
        }

        public SetUserStatusDeletionPendingStrategyBuilder With(Mock<IUserWriteRepository> userWriteRepository)
        {
            _userWriteRepository = userWriteRepository;
            return this;
        }

        public SetUserStatusDeletionPendingStrategyBuilder With(Mock<IAuditRepository> auditRepository)
        {
            _auditRepository = auditRepository;
            return this;
        }

        public SetUserStatusDeletionPendingStrategyBuilder With(Mock<ILogService> logService)
        {
            _logService = logService;
            return this;
        }
    }
}