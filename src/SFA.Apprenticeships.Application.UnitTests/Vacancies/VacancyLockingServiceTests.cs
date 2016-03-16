namespace SFA.Apprenticeships.Application.UnitTests.Vacancies
{
    using System;
    using Apprenticeships.Application.VacancyPosting;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using Vacancy;
    using Web.Raa.Common.Configuration;

    [TestFixture]
    public class VacancyLockingServiceTests
    {
        private const string UserName = "userName";
        private const string AnotherUserName = "anotherUserName";
        
        [Test]
        public void ShouldBeAbleToReserveForQAIfNobodyHasLockedTheVacancy()
        {

            var vacancySummary = new VacancySummary();

            var canBeReserved = new VacancyLockingServiceBuilder().Build()
                .CanBeReservedForQABy(UserName, vacancySummary);

            canBeReserved.Should().BeTrue();
        }

        [Test]
        public void ShouldntBeAbleToReserveForQAIfAnotherUserHasLockedTheVacancy()
        {
            var vacancySummary = new VacancySummary {QAUserName = AnotherUserName};

            var canBeReserved = new VacancyLockingServiceBuilder().Build()
                .CanBeReservedForQABy(UserName, vacancySummary);

            canBeReserved.Should().BeFalse();
        }

        [Test]
        public void ShouldBeAbleToReserveForQAIfAnotherUserHasLockedTheVacancyButHasLeftItUnattended()
        {
            // Arrange
            const int timeout = 5;
            const int greaterTimeout = timeout + 1;
            var utcNow = DateTime.UtcNow;

            var vacancySummary = new VacancySummary
            {
                QAUserName = AnotherUserName,
                DateStartedToQA = utcNow.AddMinutes(-greaterTimeout)
            };

            var vacancyLockingService = GetVacancyLockingServiceWith(timeout, utcNow);

            // Act
            var canBeReserved = vacancyLockingService.CanBeReservedForQABy(UserName, vacancySummary);

            //Assert
            canBeReserved.Should().BeTrue();
        }

        private VacancyLockingService GetVacancyLockingServiceWith(int timeout, DateTime utcNow)
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