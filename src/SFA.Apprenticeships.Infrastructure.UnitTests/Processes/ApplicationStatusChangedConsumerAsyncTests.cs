﻿namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes
{
    using System;
    using System.Collections.Generic;
    using Application.Applications.Entities;
    using Application.Interfaces.Logging;
    using Builders;
    using Domain.Entities.Applications;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Infrastructure.Processes.Applications;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class ApplicationStatusChangedConsumerAsyncTests
    {
        [Test]
        public void ApplicationNotFound()
        {
            const int legacyApplicationId = 3456789;
            var repository = new Mock<IApprenticeshipApplicationReadRepository>();
            repository.Setup(r => r.Get(legacyApplicationId, true)).Returns((ApprenticeshipApplicationDetail)null);
            var logService = new Mock<ILogService>();
            var consumer = new ApplicationStatusChangedConsumerAsyncBuilder().With(repository).With(logService).Build();
            var applicationStatusChanged = new Fixture().Build<ApplicationStatusChanged>().With(asc => asc.LegacyApplicationId, legacyApplicationId).Create();

            consumer.Consume(applicationStatusChanged).Wait();

            var expectedMessage = string.Format(ApplicationStatusChangedConsumerAsync.ApplicationNotFoundMessageFormat, legacyApplicationId);
            logService.Verify(ls => ls.Warn(expectedMessage));
        }

        [Test]
        public void Mapping()
        {
            const int legacyApplicationId = 3456789;
            const ApplicationStatuses applicationStatus = ApplicationStatuses.Unsuccessful;
            const string unsuccessfulReason = "You do not have the required grades";
            var applicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            var apprenticeshipApplicationDetail = new Fixture().Build<ApprenticeshipApplicationDetail>().With(aad => aad.LegacyApplicationId, legacyApplicationId).Create();
            applicationReadRepository.Setup(r => r.Get(legacyApplicationId, true)).Returns(apprenticeshipApplicationDetail);
            var statusAlertRepository = new Mock<IApplicationStatusAlertRepository>();
            ApplicationStatusAlert applicationStatusAlert = null;
            statusAlertRepository.Setup(r => r.Save(It.IsAny<ApplicationStatusAlert>())).Callback<ApplicationStatusAlert>(asa => { applicationStatusAlert = asa; });
            statusAlertRepository.Setup(r => r.GetForApplication(It.IsAny<Guid>())).Returns(new List<ApplicationStatusAlert>());
            var consumer = new ApplicationStatusChangedConsumerAsyncBuilder().With(applicationReadRepository).With(statusAlertRepository).Build();
            var applicationStatusChanged = new Fixture().Build<ApplicationStatusChanged>()
                .With(asc => asc.LegacyApplicationId, legacyApplicationId)
                .With(asc => asc.ApplicationStatus, applicationStatus)
                .With(asc => asc.UnsuccessfulReason, unsuccessfulReason)
                .Create();

            consumer.Consume(applicationStatusChanged).Wait();

            applicationStatusAlert.Should().NotBeNull();
            applicationStatusAlert.CandidateId.Should().Be(apprenticeshipApplicationDetail.CandidateId);
            applicationStatusAlert.ApplicationId.Should().Be(apprenticeshipApplicationDetail.EntityId);
            applicationStatusAlert.VacancyId.Should().Be(apprenticeshipApplicationDetail.Vacancy.Id);
            applicationStatusAlert.Title.Should().Be(apprenticeshipApplicationDetail.Vacancy.Title);
            applicationStatusAlert.EmployerName.Should().Be(apprenticeshipApplicationDetail.Vacancy.EmployerName);
            applicationStatusAlert.Status.Should().Be(applicationStatus);
            applicationStatusAlert.UnsuccessfulReason.Should().Be(unsuccessfulReason);
            applicationStatusAlert.DateApplied.Should().Be(apprenticeshipApplicationDetail.DateApplied.Value);
            applicationStatusAlert.BatchId.Should().Be(null);
            applicationStatusAlert.SentDateTime.Should().Be(null);
        }

        [Test]
        public void UpdateExistingAlert()
        {
            const int legacyApplicationId = 3456789;
            const ApplicationStatuses applicationStatus = ApplicationStatuses.Unsuccessful;
            const string unsuccessfulReason = "You do not have the required grades";
            var applicationId = Guid.NewGuid();
            var applicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            var apprenticeshipApplicationDetail = new Fixture().Build<ApprenticeshipApplicationDetail>()
                .With(aad => aad.EntityId, applicationId)
                .With(aad => aad.LegacyApplicationId, legacyApplicationId)
                .Create();
            applicationReadRepository.Setup(r => r.Get(legacyApplicationId, true)).Returns(apprenticeshipApplicationDetail);
            var existingApplicationStatusAlert = new Fixture().Build<ApplicationStatusAlert>()
                .With(asa => asa.ApplicationId, applicationId)
                .With(asa => asa.Status, ApplicationStatuses.Successful)
                .With(asa => asa.BatchId, null)
                .With(asa => asa.SentDateTime, null)
                .Create();
            var statusAlertRepository = new Mock<IApplicationStatusAlertRepository>();
            statusAlertRepository.Setup(r => r.GetForApplication(applicationId)).Returns(new List<ApplicationStatusAlert> {existingApplicationStatusAlert});
            ApplicationStatusAlert applicationStatusAlert = null;
            statusAlertRepository.Setup(r => r.Save(It.IsAny<ApplicationStatusAlert>())).Callback<ApplicationStatusAlert>(asa => { applicationStatusAlert = asa; });
            var consumer = new ApplicationStatusChangedConsumerAsyncBuilder().With(applicationReadRepository).With(statusAlertRepository).Build();
            var applicationStatusChanged = new Fixture().Build<ApplicationStatusChanged>()
                .With(asc => asc.LegacyApplicationId, legacyApplicationId)
                .With(asc => asc.ApplicationStatus, applicationStatus)
                .With(asc => asc.UnsuccessfulReason, unsuccessfulReason)
                .Create();

            consumer.Consume(applicationStatusChanged).Wait();

            applicationStatusAlert.EntityId.Should().Be(existingApplicationStatusAlert.EntityId);
            applicationStatusAlert.Status.Should().Be(applicationStatus);
            applicationStatusAlert.UnsuccessfulReason.Should().Be(unsuccessfulReason);
            applicationStatusAlert.BatchId.Should().Be(null);
            applicationStatusAlert.SentDateTime.Should().Be(null);
        }

        [Test]
        public void CreateNewAlertWhenExistingHasBeenSent()
        {
            const int legacyApplicationId = 3456789;
            const ApplicationStatuses applicationStatus = ApplicationStatuses.Unsuccessful;
            const string unsuccessfulReason = "You do not have the required grades";
            var applicationId = Guid.NewGuid();
            var applicationReadRepository = new Mock<IApprenticeshipApplicationReadRepository>();
            var apprenticeshipApplicationDetail = new Fixture().Build<ApprenticeshipApplicationDetail>()
                .With(aad => aad.EntityId, applicationId)
                .With(aad => aad.LegacyApplicationId, legacyApplicationId)
                .Create();
            applicationReadRepository.Setup(r => r.Get(legacyApplicationId, true)).Returns(apprenticeshipApplicationDetail);
            var existingApplicationStatusAlert = new Fixture().Build<ApplicationStatusAlert>()
                .With(asa => asa.ApplicationId, applicationId)
                .With(asa => asa.Status, ApplicationStatuses.Successful)
                .Create();
            var statusAlertRepository = new Mock<IApplicationStatusAlertRepository>();
            statusAlertRepository.Setup(r => r.GetForApplication(applicationId)).Returns(new List<ApplicationStatusAlert> {existingApplicationStatusAlert});
            ApplicationStatusAlert applicationStatusAlert = null;
            statusAlertRepository.Setup(r => r.Save(It.IsAny<ApplicationStatusAlert>())).Callback<ApplicationStatusAlert>(asa => { applicationStatusAlert = asa; });
            var consumer = new ApplicationStatusChangedConsumerAsyncBuilder().With(applicationReadRepository).With(statusAlertRepository).Build();
            var applicationStatusChanged = new Fixture().Build<ApplicationStatusChanged>()
                .With(asc => asc.LegacyApplicationId, legacyApplicationId)
                .With(asc => asc.ApplicationStatus, applicationStatus)
                .With(asc => asc.UnsuccessfulReason, unsuccessfulReason)
                .Create();

            consumer.Consume(applicationStatusChanged).Wait();

            applicationStatusAlert.EntityId.Should().NotBe(existingApplicationStatusAlert.EntityId);
            applicationStatusAlert.Status.Should().Be(applicationStatus);
            applicationStatusAlert.UnsuccessfulReason.Should().Be(unsuccessfulReason);
            applicationStatusAlert.BatchId.Should().Be(null);
            applicationStatusAlert.SentDateTime.Should().Be(null);
        }
    }
}