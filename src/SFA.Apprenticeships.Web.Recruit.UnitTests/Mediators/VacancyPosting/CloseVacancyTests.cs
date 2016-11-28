namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Vacancy;
    using System;

    [TestFixture]
    [Parallelizable]
    public class CloseVacancyTests : TestsBase
    {
        [Test]
        public void ShouldReturnOkIfThereIsntAnyValidationError()
        {
            var vacancyReferenceNumber = 1;

            var closingDate = DateTime.Now;

            var apprenticeshipVacancy = new Vacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                Wage = new Wage(WageType.NationalMinimum, null, null, null, null, WageUnit.Weekly, 30, null)
            };

            var viewModel = new FurtherVacancyDetailsViewModel
            {
                Wage = new WageViewModel(),
                VacancyReferenceNumber = vacancyReferenceNumber,
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(closingDate)
                },
            };

            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<Vacancy, FurtherVacancyDetailsViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.CloseVacancy(viewModel);
            MockVacancyPostingService.Verify(s => s.UpdateVacancy(It.Is<Vacancy>(v => v.Status == VacancyStatus.Closed)));
        }
    }
}