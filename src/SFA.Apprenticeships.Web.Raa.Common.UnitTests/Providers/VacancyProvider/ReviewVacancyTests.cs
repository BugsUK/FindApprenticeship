namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class ReviewVacancyTests
    {
        [Test]
        public void ShouldCallIsAvailableForQA()
        {
            const int vacancyReferenceNumber = 1;
            const string userName = "userName";
            
            var vacancyLockingService = new Mock<IVacancyLockingService>();
            vacancyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(userName, It.IsAny<VacancySummary>()))
                .Returns(false);
            
            var vacancyProviderBuilder = VacancyProviderTestHelper.GetBasicVacancyProviderBuilder(userName, vacancyReferenceNumber);
            var provider = vacancyProviderBuilder.With(vacancyLockingService).Build();

            provider.ReviewVacancy(vacancyReferenceNumber);

            vacancyProviderBuilder.VacancyPostingService.Verify(ps => ps.GetVacancyByReferenceNumber(vacancyReferenceNumber), Times.Once);
            vacancyLockingService.Verify(vls => vls.IsVacancyAvailableToQABy(userName, It.IsAny<VacancySummary>()), Times.Once);
        }

        [Test]
        public void ShouldReturnTheViewModelIfTheUserCanQAIt()
        {
            const int vacancyReferenceNumber = 1;
            const string userName = "userName";

            var vacancyLockingService = new Mock<IVacancyLockingService>();
            vacancyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(userName, It.IsAny<VacancySummary>()))
                .Returns(true);

            var vacancyProviderBuilder = VacancyProviderTestHelper.GetBasicVacancyProviderBuilder(userName, vacancyReferenceNumber);
            var provider = vacancyProviderBuilder.With(vacancyLockingService).Build();

            var viewModel = provider.ReviewVacancy(vacancyReferenceNumber);

            viewModel.VacancyReferenceNumber.Should().Be(vacancyReferenceNumber);
        }
    }
}