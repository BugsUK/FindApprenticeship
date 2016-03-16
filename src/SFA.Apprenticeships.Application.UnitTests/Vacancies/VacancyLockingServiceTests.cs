namespace SFA.Apprenticeships.Application.UnitTests.Vacancies
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Application.VacancyPosting;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Interfaces.Vacancies;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Web.Raa.Common.Configuration;

    [TestFixture]
    public class VacancyLockingServiceTests
    {
        private const string UserName = "userName";
        private const string AnotherUserName = "anotherUserName";
        private const int Timeout = 5;
        private const int GreaterTimeout = Timeout + 1;
        private const int SmallerTimeout = Timeout - 1;
        private DateTime _utcNow = DateTime.UtcNow;

        [Test]
        public void ShouldBeAbleToReserveForQAIfNobodyHasLockedTheVacancy()
        {
            var vacancySummary = new VacancySummary {Status = VacancyStatus.Submitted};

            var canBeReserved = new VacancyLockingServiceBuilder().Build()
                .IsVacancyAvailableToQABy(UserName, vacancySummary);

            canBeReserved.Should().BeTrue();
        }

        [TestCase(VacancyStatus.Live)]
        [TestCase(VacancyStatus.Closed)]
        [TestCase(VacancyStatus.Completed)]
        [TestCase(VacancyStatus.Deleted)]
        [TestCase(VacancyStatus.Draft)]
        [TestCase(VacancyStatus.PostedInError)]
        [TestCase(VacancyStatus.Referred)]
        [TestCase(VacancyStatus.Unknown)]
        [TestCase(VacancyStatus.Withdrawn)]
        public void ShouldNotBeAbleToReserveForQAIfTheIfTheStatusIsNotReservedForQAOrSubmitted(VacancyStatus vacancyStatus)
        {
            var vacancySummary = new VacancySummary { Status = vacancyStatus};

            var canBeReserved = new VacancyLockingServiceBuilder().Build()
                .IsVacancyAvailableToQABy(UserName, vacancySummary);

            canBeReserved.Should().BeFalse();
        }

        [Test]
        public void ShouldntBeAbleToReserveForQAIfNobodyHasLockedTheVacancyButTheStateIsReserveForQA()
        {
            // TODO: this is a situation that should be impossible to reach (an invalid status of the vacancy summary) 
            // but we test it because is not controlled by the entity itself
            var vacancySummary = new VacancySummary {Status = VacancyStatus.ReservedForQA };

            var canBeReserved = new VacancyLockingServiceBuilder().Build()
                .IsVacancyAvailableToQABy(UserName, vacancySummary);

            canBeReserved.Should().BeFalse();
        }

        [Test]
        public void ShouldBeAbleToReserveForQAIfTheVacancyIsReservedByMe()
        {
            var vacancySummary = new VacancySummary {Status = VacancyStatus.ReservedForQA, QAUserName = UserName};

            var canBeReserved = new VacancyLockingServiceBuilder().Build()
                .IsVacancyAvailableToQABy(UserName, vacancySummary);

            canBeReserved.Should().BeTrue();
        }

        [Test]
        public void ShouldBeAbleToReserveForQAIfQAUserNameIsFilledButStatusIsSubmitted()
        {
            var vacancySummary = new VacancySummary {QAUserName = AnotherUserName, Status = VacancyStatus.Submitted};

            var canBeReserved = new VacancyLockingServiceBuilder().Build()
                .IsVacancyAvailableToQABy(UserName, vacancySummary);

            canBeReserved.Should().BeTrue();
        }

        [Test]
        public void ShouldNotBeAbleToReserveForQAIfAnotherUserHasLockedTheVacancy()
        {
            // Arrange
            var vacancySummary = new VacancySummary
            {
                QAUserName = AnotherUserName,
                Status = VacancyStatus.ReservedForQA,
                DateStartedToQA = _utcNow.AddMinutes(-SmallerTimeout)
            };

            var canBeReserved = new VacancyLockingServiceBuilder().Build()
                .IsVacancyAvailableToQABy(UserName, vacancySummary);

            canBeReserved.Should().BeFalse();
        }

        [Test]
        public void ShouldBeAbleToReserveForQAIfAnotherUserHasLockedTheVacancyButHasLeftItUnattended()
        {
            // Arrange
            
            var vacancySummary = new VacancySummary
            {
                QAUserName = AnotherUserName,
                DateStartedToQA = _utcNow.AddMinutes(-GreaterTimeout),
                Status = VacancyStatus.ReservedForQA
            };

            var vacancyLockingService = GetVacancyLockingServiceWith(Timeout, _utcNow);

            // Act
            var canBeReserved = vacancyLockingService.IsVacancyAvailableToQABy(UserName, vacancySummary);

            //Assert
            canBeReserved.Should().BeTrue();
        }

        [Test]
        public void GivenAnotherUserIsReviewingVacancyBNextVacancyShouldBeVacancyC()
        {
            const int vacancyBReferenceNumber = 1;
            const int vacancyCReferenceNumber = 2;

            var vacancies = new List<VacancySummary>
            {
                new VacancySummary
                {
                    VacancyReferenceNumber = vacancyBReferenceNumber,
                    Status = VacancyStatus.ReservedForQA,
                    QAUserName = AnotherUserName,
                    DateStartedToQA = _utcNow
                },
                new VacancySummary
                {
                    VacancyReferenceNumber = vacancyCReferenceNumber,
                    Status = VacancyStatus.Submitted
                }
            };

            var vacancyLockingService = GetVacancyLockingServiceWith(Timeout, _utcNow);

            var nextAvailableVacancy = vacancyLockingService.GetNextAvailableVacancy(UserName, vacancies);

            nextAvailableVacancy.VacancyReferenceNumber.Should().Be(vacancyCReferenceNumber);
        }

        [Test]
        public void ShouldReturnNullIfAllVacanciesAreReservedForQA()
        {
            var vacancies = new List<VacancySummary>
            {
                new VacancySummary
                {
                    Status = VacancyStatus.ReservedForQA,
                    QAUserName = AnotherUserName,
                    DateStartedToQA = _utcNow.AddMinutes(-SmallerTimeout)
                },
                new VacancySummary
                {
                    Status = VacancyStatus.ReservedForQA,
                    QAUserName = AnotherUserName,
                    DateStartedToQA = _utcNow.AddMinutes(-SmallerTimeout)
                }
            };

            var vacancyLockingService = GetVacancyLockingServiceWith(Timeout, _utcNow);

            var nextAvailableVacancy = vacancyLockingService.GetNextAvailableVacancy(UserName, vacancies);

            nextAvailableVacancy.Should().BeNull();
        }

        [Test]
        public void ShouldReturnNullIfThereAreNoVacanciesToQA()
        {
            var vacancyLockingService = GetVacancyLockingServiceWith(Timeout, _utcNow);

            var nextAvailableVacancy = vacancyLockingService.GetNextAvailableVacancy(UserName, new List<VacancySummary>());

            nextAvailableVacancy.Should().BeNull();
        }


        private IVacancyLockingService GetVacancyLockingServiceWith(int timeout, DateTime utcNow)
        {
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(cs => cs.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = timeout });

            var dateTimeService = new Mock<IDateTimeService>();
            dateTimeService.Setup(ds => ds.UtcNow).Returns(utcNow);

            return new VacancyLockingServiceBuilder()
                .With(dateTimeService)
                .With(configurationService)
                .Build();
        }
    }

    internal class VacancyLockingServiceBuilder
    {
        private Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private Mock<IDateTimeService> _dateTimeService = new Mock<IDateTimeService>();
        private const int DefaultTimeout = 1;

        public VacancyLockingServiceBuilder()
        {
            _configurationService.Setup(cs => cs.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration {QAVacancyTimeout = DefaultTimeout });

            _dateTimeService.Setup(ds => ds.UtcNow).Returns(DateTime.MinValue);
        }

        public VacancyLockingService Build()
        {
            return new VacancyLockingService(_dateTimeService.Object, _configurationService.Object);
        }

        public VacancyLockingServiceBuilder With(Mock<IConfigurationService> configurationService)
        {
            _configurationService = configurationService;
            return this;
        }

        public VacancyLockingServiceBuilder With(Mock<IDateTimeService> dateTimeService)
        {
            _dateTimeService = dateTimeService;
            return this;
        }
    }
}