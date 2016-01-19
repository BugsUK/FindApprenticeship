namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using System;
    using Common.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class ManageDatesTests : TestBase
    {
        [Test]
        public void ShouldUpdateClosingDate()
        {
            const long vacancyReferenceNumber = 1;
            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(closingDate),
                PossibleStartDate = new DateViewModel(possibleStartDate),
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancy = new ApprenticeshipVacancy {VacancyReferenceNumber = vacancyReferenceNumber};
            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.ShallowSaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, VacancyDatesViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.UpdateVacancy(viewModel);

            MockVacancyPostingService.Verify(s => s.ShallowSaveApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(v => v.ClosingDate == closingDate)));
        }

        [Test]
        public void ShouldUpdatePossibleStartDate()
        {
            const long vacancyReferenceNumber = 1;
            var closingDate = DateTime.Today.AddDays(20);
            var possibleStartDate = DateTime.Today.AddDays(30);

            var viewModel = new VacancyDatesViewModel
            {
                ClosingDate = new DateViewModel(closingDate),
                PossibleStartDate = new DateViewModel(possibleStartDate),
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancy = new ApprenticeshipVacancy { VacancyReferenceNumber = vacancyReferenceNumber };
            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyReferenceNumber))
                .Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(s => s.ShallowSaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(apprenticeshipVacancy);
            MockMapper.Setup(m => m.Map<ApprenticeshipVacancy, VacancyDatesViewModel>(apprenticeshipVacancy))
                .Returns(viewModel);

            var provider = GetVacancyPostingProvider();

            provider.UpdateVacancy(viewModel);

            MockVacancyPostingService.Verify(s => s.ShallowSaveApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(v => v.PossibleStartDate == possibleStartDate)));
        }
    }
}