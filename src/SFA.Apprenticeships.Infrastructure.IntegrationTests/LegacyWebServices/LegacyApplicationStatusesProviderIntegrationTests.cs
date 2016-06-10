﻿namespace SFA.Apprenticeships.Infrastructure.IntegrationTests.LegacyWebServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Application.Applications;
    using Application.Applications.Entities;
    using Application.Candidate;
    using Common.Configuration;
    using Common.IoC;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Helpers;
    using Infrastructure.LegacyWebServices.IoC;
    using Infrastructure.Monitor.IoC;
    using Infrastructure.Repositories.Mongo.Applications.IoC;
    using Logging.IoC;
    using Moq;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class LegacyApplicationStatusesProviderIntegrationTests
    {
        private const int TestVacancyId = 445650;

        private ILegacyCandidateProvider _legacyCandidateProvider;
        private ILegacyApplicationProvider _legacyApplicationProvider;
        private ILegacyApplicationStatusesProvider _legacyApplicationStatusesProvider;

        private readonly Mock<ICandidateReadRepository> _candidateReadRepositoryMock = new Mock<ICandidateReadRepository>();

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry(new LegacyWebServicesRegistry());
                x.AddRegistry(new VacancySourceRegistry(new CacheConfiguration(), new ServicesConfiguration { ServiceImplementation = ServicesConfiguration.Legacy }));
                x.AddRegistry<ApplicationRepositoryRegistry>();
                x.For<ICandidateReadRepository>().Use(_candidateReadRepositoryMock.Object);
            });

            // Providers.
            _legacyCandidateProvider = container.GetInstance<ILegacyCandidateProvider>();
            _legacyApplicationProvider = container.GetInstance<ILegacyApplicationProvider>();
            _legacyApplicationStatusesProvider = container.GetInstance<ILegacyApplicationStatusesProvider>();
        }

        [Test, Category("Integration")]
        public void ShouldNotGetAnyApplicationStatusesForCandidateWithNoSubmittedApplications()
        {
            // Arrange.
            var candidate = CreateCandidate();
            _candidateReadRepositoryMock.ResetCalls();
            _candidateReadRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>())).Returns(candidate);

            // Act.
            var result = _legacyApplicationStatusesProvider
                .GetCandidateApplicationStatuses(candidate)
                .ToList();

            // Assert.
            result.Should().NotBeNull();
            result.Count().Should().Be(0);
        }

        [Test, Category("Integration"), Category("ConcreteGatewayOnly")]
        public void ShouldGetOneApplicationStatusForCandidateWithOneSubmittedApplication()
        {
            // Arrange.
            var candidate = CreateCandidate();
            _candidateReadRepositoryMock.ResetCalls();
            _candidateReadRepositoryMock.Setup(cr => cr.Get(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(candidate);

            var applicationDetail = CreateApplicationForCandidate(candidate);

            // Act: get application statuses (should only be one).
            var statuses = new List<ApplicationStatusSummary>(0);
            for (var i = 0; i < 10; i++)
            {
                //There appears to be a delay in the nas gateway between submitting the application and being able to query the state.
                //Loop until the state is found or we deem that this request has failed
                statuses = _legacyApplicationStatusesProvider.GetCandidateApplicationStatuses(candidate).ToList();
                if (statuses.Count > 0)
                    break;
                Thread.Sleep(1000);
            }
            
            // Assert.
            statuses.Should().NotBeNull();
            statuses.Count().Should().Be(1);

            var status = statuses.First();

            status.ApplicationStatus.Should().Be(ApplicationStatuses.Submitted);
            status.LegacyApplicationId.Should().Be(applicationDetail.LegacyApplicationId);
        }

        private Candidate CreateCandidate()
        {
            // Create a new candidate in legacy and repo.
            var candidate = TestCandidateHelper.CreateFakeCandidate();

            candidate.LegacyCandidateId = _legacyCandidateProvider.CreateCandidate(candidate);

            return candidate;
        }

        private ApprenticeshipApplicationDetail CreateApplicationForCandidate(Candidate candidate)
        {
            // Create a new application for candidate in legacy and repo.
            var applicationDetail = new TestApplicationBuilder()
                .WithCandidateInformation()
                .WithVacancyId(TestVacancyId)
                .Build();

            applicationDetail.CandidateId = candidate.EntityId;

            var legacyApplicationId = _legacyApplicationProvider.CreateApplication(applicationDetail);

            applicationDetail.LegacyApplicationId = legacyApplicationId;

            return applicationDetail;
        }
    }
}