namespace SFA.Apprenticeships.Infrastructure.UnitTests.Processes
{
    using System;
    using Application.Applications;
    using Application.Applications.Entities;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Caching;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Processes.Configuration;
    using Infrastructure.Processes.Extensions;
    using Infrastructure.Processes.Vacancies;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyStatusSummaryConsumerAsyncTests
    {
        private VacancyStatusSummaryConsumerAsync _vacancyStatusSummaryConsumerAsync;
        private Mock<ICacheService> _cacheServiceMock;
        private Mock<IApplicationStatusProcessor> _applicationStatusProcessor;
        private Mock<IConfigurationService> _configurationServiceMock;

        [SetUp]
        public void SetUp()
        {
            _cacheServiceMock = new Mock<ICacheService>();
            _applicationStatusProcessor = new Mock<IApplicationStatusProcessor>();
            _configurationServiceMock = new Mock<IConfigurationService>();
            _configurationServiceMock.Setup(x => x.Get<ProcessConfiguration>())
                .Returns(new ProcessConfiguration() {EnableVacancyStatusPropagation = true});
            _vacancyStatusSummaryConsumerAsync = new VacancyStatusSummaryConsumerAsync(_cacheServiceMock.Object, _applicationStatusProcessor.Object, _configurationServiceMock.Object);
        }

        [Test]
        public void ShouldNotQueueWhenInCache()
        {
            _applicationStatusProcessor.Setup(x => x.ProcessApplicationStatuses(It.IsAny<VacancyStatusSummary>()));
            _cacheServiceMock.Setup(x => x.Get<VacancyStatusSummary>(It.IsAny<string>())).Returns(new VacancyStatusSummary());

            var task = _vacancyStatusSummaryConsumerAsync.Consume(new VacancyStatusSummary());
            task.Wait();

            _cacheServiceMock.Verify(x => x.PutObject(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheDuration>()), Times.Never);
            _applicationStatusProcessor.Verify(x => x.ProcessApplicationStatuses(It.IsAny<VacancyStatusSummary>()), Times.Never);
        }

        [Test]
        public void ShouldPutInCacheWhenNotInCache()
        {
            var vacancyStatusSummary = new VacancyStatusSummary { LegacyVacancyId = 123, ClosingDate = DateTime.Now.AddMonths(-3), VacancyStatus = VacancyStatuses.Expired };
            _applicationStatusProcessor.Setup(x => x.ProcessApplicationStatuses(It.IsAny<VacancyStatusSummary>()));
            _cacheServiceMock.Setup(x => x.PutObject(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CacheDuration>()));
            _configurationServiceMock.Setup(x => x.Get<ProcessConfiguration>())
                .Returns(new ProcessConfiguration {EnableVacancyStatusPropagation = true});
            _vacancyStatusSummaryConsumerAsync = new VacancyStatusSummaryConsumerAsync(_cacheServiceMock.Object, _applicationStatusProcessor.Object, _configurationServiceMock.Object);

            var task = _vacancyStatusSummaryConsumerAsync.Consume(vacancyStatusSummary);
            task.Wait();

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
