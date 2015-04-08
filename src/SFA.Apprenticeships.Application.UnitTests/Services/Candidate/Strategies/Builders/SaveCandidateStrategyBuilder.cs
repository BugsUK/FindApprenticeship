﻿namespace SFA.Apprenticeships.Application.UnitTests.Services.Candidate.Strategies.Builders
{
    using System;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Domain.Entities.Applications;
    using Domain.Interfaces.Repositories;
    using Interfaces.Logging;
    using Interfaces.Users;
    using Moq;

    public class SaveCandidateStrategyBuilder
    {
        private readonly Mock<ICandidateWriteRepository> _candidateWriteRepository = new Mock<ICandidateWriteRepository>();
        private Mock<IGetCandidateApprenticeshipApplicationsStrategy> _getCandidateApplicationsStrategy = new Mock<IGetCandidateApprenticeshipApplicationsStrategy>();
        private Mock<ICandidateReadRepository> _candidateReadRepository = new Mock<ICandidateReadRepository>();
        private Mock<IApprenticeshipApplicationWriteRepository> _apprenticeshipApplicationWriteRepository = new Mock<IApprenticeshipApplicationWriteRepository>();
        private Mock<IApprenticeshipApplicationReadRepository> _apprenticeshipApplicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
        private ISendMobileVerificationCodeStrategy _sendMobileVerificationCodeStrategy = new Mock<ISendMobileVerificationCodeStrategy>().Object;
        private Mock<ICodeGenerator> _codeGenerator = new Mock<ICodeGenerator>();
        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        public SaveCandidateStrategyBuilder()
        {
            _getCandidateApplicationsStrategy.Setup(s => s.GetApplications(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new ApprenticeshipApplicationSummary[0]);
        }

        public ISaveCandidateStrategy Build()
        {
            var strategy = new SaveCandidateStrategy(_candidateWriteRepository.Object, _getCandidateApplicationsStrategy.Object, _candidateReadRepository.Object, _apprenticeshipApplicationWriteRepository.Object, _apprenticeshipApplicationReadRepository.Object, _codeGenerator.Object, _sendMobileVerificationCodeStrategy, _logService.Object);
            return strategy;
        }

        public SaveCandidateStrategyBuilder With(Mock<IGetCandidateApprenticeshipApplicationsStrategy> getCandidateApplicationsStrategy)
        {
            _getCandidateApplicationsStrategy = getCandidateApplicationsStrategy;
            return this;
        }

        public SaveCandidateStrategyBuilder With(Mock<ICandidateReadRepository> candidateReadRepository)
        {
            _candidateReadRepository = candidateReadRepository;
            return this;
        }

        public SaveCandidateStrategyBuilder With(Mock<IApprenticeshipApplicationWriteRepository> apprenticeshipApplicationWriteRepository)
        {
            _apprenticeshipApplicationWriteRepository = apprenticeshipApplicationWriteRepository;
            return this;
        }

        public SaveCandidateStrategyBuilder With(Mock<IApprenticeshipApplicationReadRepository> apprenticeshipApplicationReadRepository)
        {
            _apprenticeshipApplicationReadRepository = apprenticeshipApplicationReadRepository;
            return this;
        }

        public SaveCandidateStrategyBuilder With(SendMobileVerificationCodeStrategy sendMobileVerificationCodeStrategy)
        {
            _sendMobileVerificationCodeStrategy = sendMobileVerificationCodeStrategy;
            return this;
        }

        public SaveCandidateStrategyBuilder With(Mock<ICodeGenerator> codeGenerator)
        {
            _codeGenerator = codeGenerator;
            return this;
        }
    }
}
