namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;
    using ViewModels.Vacancy;
    using Web.Common.ViewModels;

    [TestFixture]
    public class ManageDatesTests : TestBase
    {
        [Test]
        public void ShouldUpdateClosingDate()
        {
            const int vacancyReferenceNumber = 1;
            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(closingDate),
                PossibleStartDate = new DateViewModel(possibleStartDate),
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancy = new Vacancy { VacancyReferenceNumber = vacancyReferenceNumber};
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.SaveVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<Vacancy, VacancyDatesViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.UpdateVacancy(viewModel);

            MockVacancyPostingService.Verify(s => s.SaveVacancy(It.Is<Vacancy>(v => v.ClosingDate == closingDate)));
        }

        [Test]
        public void ShouldUpdatePossibleStartDate()
        {
            const int vacancyReferenceNumber = 1;
            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(closingDate),
                PossibleStartDate = new DateViewModel(possibleStartDate),
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancy = new Vacancy { VacancyReferenceNumber = vacancyReferenceNumber };
            MockVacancyPostingService.Setup(s => s.GetVacancyByReferenceNumber(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.SaveVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<Vacancy, VacancyDatesViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.UpdateVacancy(viewModel);

            MockVacancyPostingService.Verify(s => s.SaveVacancy(It.Is<Vacancy>(v => v.PossibleStartDate == possibleStartDate)));
        }
    }
}