namespace SFA.Apprenticeships.Application.UnitTests.Vacancies.SavedSearchProcessorTests
{
    using System.Linq;
    using Apprenticeships.Application.Vacancies.Entities;
    using Domain.Interfaces.Messaging;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using SFA.Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    using SFA.Apprenticeships.Application.Interfaces;

    [TestFixture]
    public class QueueCandidateSavedSearchesTests
    {
        [Test]
        public void RetrievesCandidateSavedSearches()
        {
            var savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();
            var processor = new SavedSearchProcessorBuilder().With(savedSearchReadRepository).Build();

            processor.QueueCandidateSavedSearches();

            savedSearchReadRepository.Verify(r => r.GetCandidateIds(), Times.Once);
        }

        [Test]
        public void QueuesMultipleMessages()
        {
            const int numCandidateIds = 50;

            var savedSearchReadRepository = new Mock<ISavedSearchReadRepository>();

            savedSearchReadRepository.Setup(r => r.GetCandidateIds()).Returns(new Fixture().Build<CandidateSavedSearches>().CreateMany(numCandidateIds).Select(s => s.CandidateId));

            var serviceBus = new Mock<IServiceBus>();
            var loggedMessage = string.Empty;
            var logService = new Mock<ILogService>();

            logService.Setup(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>())).Callback<string, object[]>((m, p) => { loggedMessage = m; });

            var processor =
                new SavedSearchProcessorBuilder().With(savedSearchReadRepository)
                    .With(serviceBus)
                    .With(logService)
                    .Build();

            processor.QueueCandidateSavedSearches();

            serviceBus.Verify(b => b.PublishMessage(It.IsAny<CandidateSavedSearches>()), Times.Exactly(numCandidateIds));
            logService.Verify(l => l.Info(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once());
            const string expectedMessage = "Querying candidate saved searches took ";
            loggedMessage.Should().StartWith(expectedMessage);
        }
    }
}