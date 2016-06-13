namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.LegacyWebServices
{
    using System;
    using Application.Candidate;
    using Application.Interfaces.Applications;
    using Common.Configuration;
    using Common.IoC;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Helpers;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.Monitor.IoC;
    using Logging.IoC;
    using Moq;
    using NUnit.Framework;
    using StructureMap;

    public class LegacyApplicationProviderIntegrationTests
    {
        private ILegacyApplicationProvider _legacyApplicationProviderProvider;
        private ILegacyCandidateProvider _legacyCandidateProvider;
        private readonly Mock<ICandidateReadRepository> _candidateRepositoryMock = new Mock<ICandidateReadRepository>();

        private const int TestVacancyId = 445650;
        private const int CandidateId = int.MaxValue;

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry(new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Legacy, VacanciesSource = ServicesConfiguration.Legacy }, new CacheConfiguration()));
                x.AddRegistry(new VacancySourceRegistry(new CacheConfiguration(), new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Legacy }));
                x.For<ICandidateReadRepository>().Use(_candidateRepositoryMock.Object);
            });

            _legacyApplicationProviderProvider = container.GetInstance<ILegacyApplicationProvider>();
            _legacyCandidateProvider = container.GetInstance<ILegacyCandidateProvider>();
        }

        [Test, Category("Integration")]
        public void ShouldCreateApplicationForAValidApplication()
        {
            _candidateRepositoryMock.ResetCalls();
            _candidateRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new Candidate
            {
                LegacyCandidateId = CreateLegacyCandidateId()
            });
            var applicationDetail = new TestApplicationBuilder()
                .WithCandidateInformation()
                .WithVacancyId(TestVacancyId)
                .Build();

            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        [ExpectedException(Handler = "CheckForLegacyCandidateNotFoundErrorException")]
        public void ShouldThrowAnErrorIfTheCandidateDoesntExistInNasGateway()
        {
            _candidateRepositoryMock.ResetCalls();
            _candidateRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new Candidate
            {
                LegacyCandidateId = CandidateId
            });
            var applicationDetail = new TestApplicationBuilder()
                .WithCandidateInformation()
                .WithVacancyId(TestVacancyId)
                .Build();

            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
        }

        [Test, Category("Integration")]
        [ExpectedException(Handler = "CheckForApplicationGatewayCreationException")]
        public void ShouldGetAnErrorWhenCreatinganApplication()
        {
            _candidateRepositoryMock.ResetCalls();
            _candidateRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new Candidate
            {
                LegacyCandidateId = CreateLegacyCandidateId()
            });
            var applicationDetail = new TestApplicationBuilder().Build();

            applicationDetail.CandidateInformation.EducationHistory = new Education
            {
                Institution =  "GENERAL_ERROR",
                FromYear = 1999,
                ToYear = 2001
            };
          
            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
        }

        [Test, Category("Integration")]
        [ExpectedException(Handler = "CheckForDuplicatedApplicationException")]
        public void ShouldGetACustomExceptionWhenResubmittingAnApplication()
        {
           _candidateRepositoryMock.ResetCalls();
           _candidateRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new Candidate
            {
                LegacyCandidateId = CreateLegacyCandidateId()
            });

            var applicationDetail = new TestApplicationBuilder()
                .WithCandidateInformation()
                .WithVacancyId(TestVacancyId)
                .Build();

            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
            applicationDetail.CandidateInformation.EducationHistory = new Education()
            {
                Institution = "DUPLICATE_APPLICATION",
                FromYear = 1999,
                ToYear = 2001
            };
            _legacyApplicationProviderProvider.CreateApplication(applicationDetail);
        }

        public void CheckForDuplicatedApplicationException( Exception ex )
        {
            ex.Should().BeOfType<DomainException>();

            var cex = ex as DomainException;
// ReSharper disable once PossibleNullReferenceException
            cex.Code.Should().Be(Application.Interfaces.Applications.ErrorCodes.ApplicationDuplicatedError);
        }

        public void CheckForApplicationGatewayCreationException(Exception ex)
        {
            ex.Should().BeOfType<DomainException>();

            var cex = ex as DomainException;
// ReSharper disable once PossibleNullReferenceException
            cex.Code.Should().Be(ErrorCodes.ApplicationCreationFailed);
        }

        public void CheckForLegacyCandidateNotFoundErrorException(Exception ex)
        {
            ex.Should().BeOfType<DomainException>();

            var cex = ex as DomainException;
// ReSharper disable once PossibleNullReferenceException
            cex.Code.Should().Be(Application.Interfaces.Candidates.ErrorCodes.CandidateNotFoundError);
        }

        private int CreateLegacyCandidateId()
        {
            var candidate = TestCandidateHelper.CreateFakeCandidate();

            return _legacyCandidateProvider.CreateCandidate(candidate);
        }
    }
}