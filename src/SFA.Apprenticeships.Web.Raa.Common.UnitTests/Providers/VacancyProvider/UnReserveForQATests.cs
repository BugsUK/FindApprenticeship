namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using Application.Interfaces.Vacancies;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;

    public class UnReserveForQATests
    {
        [Test]
        public void ShouldCallUpdateIfTheUserIsTheOwnerOfTheVacancy()
        {
            const int vacancyReferenceNumber = 1;
            const string userName = "userName";

            var vacancyLockingService = new Mock<IVacancyLockingService>();
            vacancyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(userName, It.IsAny<VacancySummary>()))
                .Returns(true);

            var vacancyProviderBuilder = VacancyProviderTestHelper.GetBasicVacancyProviderBuilder(userName, vacancyReferenceNumber);
            var provider = vacancyProviderBuilder.With(vacancyLockingService).Build();

            provider.UnReserveVacancyForQA(vacancyReferenceNumber);

            vacancyProviderBuilder.VacancyPostingService.Verify(
                ps =>
                    ps.UpdateVacancy(
                        It.Is<Vacancy>(v => v.VacancyReferenceNumber == vacancyReferenceNumber && v.QAUserName == null)),
                Times.Once);
        }

        [Test]
        public void ShouldNotCallUpdateIfTheUserIsTheOwnerOfTheVacancy()
        {
            const int vacancyReferenceNumber = 1;
            const string userName = "userName";

            var vacancyLockingService = new Mock<IVacancyLockingService>();
            vacancyLockingService.Setup(vls => vls.IsVacancyAvailableToQABy(userName, It.IsAny<VacancySummary>()))
                .Returns(false);

            var vacancyProviderBuilder = VacancyProviderTestHelper.GetBasicVacancyProviderBuilder(userName, vacancyReferenceNumber);
            var provider = vacancyProviderBuilder.With(vacancyLockingService).Build();

            provider.UnReserveVacancyForQA(vacancyReferenceNumber);

            vacancyProviderBuilder.VacancyPostingService.Verify(
                ps =>
                    ps.UpdateVacancy(
                        It.Is<Vacancy>(v => v.VacancyReferenceNumber == vacancyReferenceNumber && 
                            v.QAUserName == null && v.Status == VacancyStatus.Submitted)),
                Times.Never);
        }
    }
}