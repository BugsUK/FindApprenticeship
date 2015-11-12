namespace SFA.Apprenticeships.Web.Common.UnitTests.Providers
{
    using System;
    using Application.Interfaces.VacancyPosting;
    using Common.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class VacancyProviderTests
    {
        [Test]
        public void ShouldSaveCommentsWhenUpdatingVacancySummaryViewModel()
        {
            const int vacancyReferenceNumber = 1;
            const string closingDateComment = "Closing date comment";
            const string workingWeekComment = "Working week comment";
            const string wageComment = "Wage comment";
            const string durationComment = "Duration comment";
            const string longDescriptionComment = "Long description comment";
            const string possibleStartDateComment = "Possible start date comment";

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var provider = GetProvider(vacancyPostingService);
            var viewModel = GetValidVacancySummaryViewModel(vacancyReferenceNumber);
            vacancyPostingService.Setup(vp => vp.GetVacancy(vacancyReferenceNumber)).Returns(new ApprenticeshipVacancy());
            vacancyPostingService.Setup(vp => vp.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(new ApprenticeshipVacancy());
            viewModel.ClosingDateComment = closingDateComment;
            viewModel.DurationComment = durationComment;
            viewModel.LongDescriptionComment = longDescriptionComment;
            viewModel.PossibleStartDateComment = possibleStartDateComment;
            viewModel.WageComment = wageComment;
            viewModel.WorkingWeekComment = workingWeekComment;

            provider.UpdateVacancy(viewModel);

            vacancyPostingService.Verify(vp => vp.GetVacancy(vacancyReferenceNumber));
            vacancyPostingService.Verify(
                vp =>
                    vp.SaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(
                            v =>
                                v.ClosingDateComment == closingDateComment && 
                                v.DurationComment == durationComment &&
                                v.LongDescriptionComment == longDescriptionComment &&
                                v.PossibleStartDateComment == possibleStartDateComment &&
                                v.WageComment == wageComment && 
                                v.WorkingWeekComment == workingWeekComment)));
        }

        private IVacancyProvider GetProvider(Mock<IVacancyPostingService> vacancyPostingService)
        {
            return new VacancyProvider(null, null, null, null, null, null, vacancyPostingService.Object);
        }

        private static VacancySummaryViewModel GetValidVacancySummaryViewModel(int vacancyReferenceNumber)
        {
            return new VacancySummaryViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(20)),
                PossibleStartDate = new DateViewModel(DateTime.UtcNow.AddDays(30)),
                Duration = 3,
                DurationType = DurationType.Years,
                LongDescription = "A description",
                WageType = WageType.ApprenticeshipMinimumWage,
                HoursPerWeek = 30,
                WorkingWeek = "A working week"
            };
        }
    }
}