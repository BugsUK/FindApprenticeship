namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes
{
    using System;
    using Application.Applications;
    using Application.Applications.Entities;
    using Domain.Entities.Vacancies;
    using SFA.Infrastructure.Interfaces.Caching;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Messaging;
    using FluentAssertions;
    using Infrastructure.Processes.Configuration;
    using Infrastructure.Processes.Extensions;
    using Infrastructure.Processes.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class VacancyStatusSummarySubscriberTests
    {
        private Mock<IConfigurationService> _configurationServiceMock;
        private Mock<ICacheService> _cacheServiceMock;
        private Mock<IApplicationStatusProcessor> _applicationStatusProcessor;

        private VacancyStatusSummarySubscriber _subscriber;

        [SetUp]
        public void SetUp()
        {
            _configurationServiceMock = new Mock<IConfigurationService>();

            _configurationServiceMock.Setup(x => x.Get<ProcessConfiguration>())
                .Returns(new ProcessConfiguration {EnableVacancyStatusPropagation = true});

            _cacheServiceMock = new Mock<ICacheService>();
            _applicationStatusProcessor = new Mock<IApplicationStatusProcessor>();

            _subscriber = new VacancyStatusSummarySubscriber(
                _configurationServiceMock.Object,
                _cacheServiceMock.Object,
                _applicationStatusProcessor.Object);
        }

        [Test]
        public void ShouldNotQueueWhenInCache()
        {
            // Arrange.
            _applicationStatusProcessor.Setup(x => x.ProcessApplicationStatuses(It.IsAny<VacancyStatusSummary>()));
            _cacheServiceMock.Setup(x => x.Get<VacancyStatusSummary>(It.IsAny<string>())).Returns(new VacancyStatusSummary());

            // Act.
            var state = _subscriber.Consume(new VacancyStatusSummary());

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            _cacheServiceMock.Verify(x => x.PutObject(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheDuration>()), Times.Never);
            _applicationStatusProcessor.Verify(x => x.ProcessApplicationStatuses(It.IsAny<VacancyStatusSummary>()), Times.Never);
        }

        [Test]
        public void ShouldPutInCacheWhenNotInCache()
        {
            // Arrange.
            var vacancyStatusSummary = new VacancyStatusSummary
            {
                LegacyVacancyId = 123,
                ClosingDate = DateTime.UtcNow.AddMonths(-3),
                VacancyStatus = VacancyStatuses.Expired
            };

            _applicationStatusProcessor.Setup(x => x.ProcessApplicationStatuses(It.IsAny<VacancyStatusSummary>()));
            _cacheServiceMock.Setup(x => x.PutObject(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheDuration>()));

            _configurationServiceMock.Setup(x => x.Get<ProcessConfiguration>())
                .Returns(new ProcessConfiguration
                {
                    EnableVacancyStatusPropagation = true
                });

            _subscriber = new VacancyStatusSummarySubscriber(
                _configurationServiceMock.Object,
                _cacheServiceMock.Object,
                _applicationStatusProcessor.Object);

            // Act.
            var state = _subscriber.Consume(vacancyStatusSummary);

            // Assert.
            state.Should().NotBeNull();
            state.Should().Be(ServiceBusMessageStates.Complete);

            _cacheServiceMock.Verify(
                x =>
                    x.PutObject(
                        It.Is<string>(c => c == vacancyStatusSummary.CacheKey()),
                        It.Is<object>(vss => vss == vacancyStatusSummary),
                        It.Is<CacheDuration>(c => c == vacancyStatusSummary.CacheDuration())), Times.Once);

            _applicationStatusProcessor.Verify(
                x =>
                    x.ProcessApplicationStatuses(
                        It.Is<VacancyStatusSummary>(i => i == vacancyStatusSummary)), Times.Once);
        }
    }
}
