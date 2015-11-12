namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using System;
    using Common.UnitTests.Mediators;
    using Common.ViewModels;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Manage.Mediators.Vacancy;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class EditCommentTests
    {
        [Test]
        public void GetVacancySummaryViewModelShouldgetViewModelFromProvider()
        {
            const int vacancyReferenceNumber = 1;
            var vacancyPostingProvider = new Mock<IVacancyPostingProvider>();
            vacancyPostingProvider.Setup(vp => vp.GetVacancySummaryViewModel(vacancyReferenceNumber)).Returns(new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(DateTime.UtcNow),
                PossibleStartDate = new DateViewModel(DateTime.UtcNow)
            });

            var mediator = new VacancyMediatorBuilder().With(vacancyPostingProvider).Build();

            mediator.GetVacancySummaryViewModel(vacancyReferenceNumber);

            vacancyPostingProvider.Verify(vp => vp.GetVacancySummaryViewModel(vacancyReferenceNumber));
        }

        [Test]
        public void GetVacancySummaryViewModelShoudReturnValidationFailedIfValidationFailed()
        {
            const int vacancyReferenceNumber = 1;
            var vacancyPostingProvider = new Mock<IVacancyPostingProvider>();
            vacancyPostingProvider.Setup(vp => vp.GetVacancySummaryViewModel(vacancyReferenceNumber)).Returns(new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(DateTime.UtcNow),
                PossibleStartDate = new DateViewModel(DateTime.UtcNow)
            });

            var mediator = new VacancyMediatorBuilder().With(vacancyPostingProvider).Build();

            var response = mediator.GetVacancySummaryViewModel(vacancyReferenceNumber);

            response.AssertValidationResult(VacancyMediatorCodes.GetVacancySummaryViewModel.FailedValidation);
        }

        [Test]
        public void GetVacancySummaryViewModelShoudReturnOkIfValidationPassed()
        {
            const int vacancyReferenceNumber = 1;
            var vacancyPostingProvider = new Mock<IVacancyPostingProvider>();
            vacancyPostingProvider.Setup(vp => vp.GetVacancySummaryViewModel(vacancyReferenceNumber)).Returns(GetValidVacancySummaryViewModel(vacancyReferenceNumber));

            var mediator = new VacancyMediatorBuilder().With(vacancyPostingProvider).Build();

            var response = mediator.GetVacancySummaryViewModel(vacancyReferenceNumber);

            response.AssertCode(VacancyMediatorCodes.GetVacancySummaryViewModel.Ok);
        }

        [Test]
        public void UpdateVacancyWillUpdateVacancyIfViewModelIsValid()
        {
            const int vacancyReferenceNumber = 1;
            var vacancyProvider = new Mock<IVacancyProvider>();

            var mediator = new VacancyMediatorBuilder().With(vacancyProvider).Build();
            var viewModel = GetValidVacancySummaryViewModel(vacancyReferenceNumber);

            var result = mediator.UpdateVacancy(viewModel);

            result.AssertCode(VacancyMediatorCodes.UpdateVacancy.Ok);
            vacancyProvider.Verify(vp => vp.UpdateVacancy(viewModel));
        }

        [Test]
        public void UpdateVacancyWillNotUpdateVacancyIfViewModelIsNotValid()
        {
            var vacancyProvider = new Mock<IVacancyProvider>();

            var mediator = new VacancyMediatorBuilder().With(vacancyProvider).Build();
            var viewModel = new VacancySummaryViewModel
            {
                ClosingDate = new DateViewModel(DateTime.UtcNow),
                PossibleStartDate = new DateViewModel(DateTime.UtcNow)
            };

            var result = mediator.UpdateVacancy(viewModel);

            result.AssertValidationResult(VacancyMediatorCodes.UpdateVacancy.FailedValidation);
            vacancyProvider.Verify(vp => vp.UpdateVacancy(viewModel), Times.Never);
        }

        [Test]
        public void UpdateVacancyWithAVacancyThatAcceptsWarningsWithWarningsShouldUpdateVacancy()
        {
            var vacancyProvider = new Mock<IVacancyProvider>();

            var mediator = new VacancyMediatorBuilder().With(vacancyProvider).Build();
            var viewModel = GetValidVacancySummaryViewModel(1);
            viewModel.ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(10));
            viewModel.AcceptWarnings = true;

            var result = mediator.UpdateVacancy(viewModel);

            result.AssertCode(VacancyMediatorCodes.UpdateVacancy.Ok);
            vacancyProvider.Verify(vp => vp.UpdateVacancy(viewModel));
        }

        [Test]
        public void UpdateVacancyWithAVacancyThatDontAcceptsWarningsWithWarningsShouldNotUpdateVacancy()
        {
            var vacancyProvider = new Mock<IVacancyProvider>();

            var mediator = new VacancyMediatorBuilder().With(vacancyProvider).Build();
            var viewModel = GetValidVacancySummaryViewModel(1);
            viewModel.ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(10));
            viewModel.AcceptWarnings = false;

            var result = mediator.UpdateVacancy(viewModel);

            result.AssertValidationResult(VacancyMediatorCodes.UpdateVacancy.FailedValidation);
            vacancyProvider.Verify(vp => vp.UpdateVacancy(viewModel), Times.Never);
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