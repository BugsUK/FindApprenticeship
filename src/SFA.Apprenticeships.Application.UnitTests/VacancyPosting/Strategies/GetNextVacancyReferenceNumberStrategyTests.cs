namespace SFA.Apprenticeships.Application.UnitTests.VacancyPosting.Strategies
{
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Domain.Interfaces.Repositories;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class GetNextVacancyReferenceNumberStrategyTests
    {
        private readonly Mock<IReferenceNumberRepository> _mockReferenceNumberRepository = new Mock<IReferenceNumberRepository>();

        private IGetNextVacancyReferenceNumberStrategy _getNextVacancyReferenceNumberStrategy;

        [SetUp]
        public void SetUp()
        {
            _getNextVacancyReferenceNumberStrategy = new GetNextVacancyReferenceNumberStrategy(_mockReferenceNumberRepository.Object);
        }

        [Test]
        public void GetNextVacancyReferenceNumberShouldCallRepository()
        {
            // Arrange.
            _getNextVacancyReferenceNumberStrategy.GetNextVacancyReferenceNumber();

            _mockReferenceNumberRepository.Verify(r => r.GetNextVacancyReferenceNumber());
        }
    }
}