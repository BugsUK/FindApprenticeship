namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using System;
    using Common.UnitTests.Mediators;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
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
            var vacancyProvider = new Mock<IVacancyQAProvider>();
            vacancyProvider.Setup(vp => vp.GetVacancySummaryViewModel(vacancyReferenceNumber)).Returns(new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.UtcNow),
                    PossibleStartDate = new DateViewModel(DateTime.UtcNow)
                }
            });

            var mediator = new VacancyMediatorBuilder().With(vacancyProvider).Build();

            mediator.GetVacancySummaryViewModel(vacancyReferenceNumber);

            vacancyProvider.Verify(vp => vp.GetVacancySummaryViewModel(vacancyReferenceNumber));
        }

        [Test]
        public void GetVacancySummaryViewModelShoudReturnValidationFailedIfValidationFailed()
        {
            const int vacancyReferenceNumber = 1;
            var vacancyPostingProvider = new Mock<IVacancyQAProvider>();
            vacancyPostingProvider.Setup(vp => vp.GetVacancySummaryViewModel(vacancyReferenceNumber)).Returns(new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.UtcNow),
                    PossibleStartDate = new DateViewModel(DateTime.UtcNow)
                }
            });

            var mediator = new VacancyMediatorBuilder().With(vacancyPostingProvider).Build();

            var response = mediator.GetVacancySummaryViewModel(vacancyReferenceNumber);

            response.AssertValidationResult(VacancyMediatorCodes.GetVacancySummaryViewModel.FailedValidation);
        }

        [Test]
        public void GetVacancySummaryViewModelShoudReturnOkIfValidationPassed()
        {
            const int vacancyReferenceNumber = 1;
            var vacancyProvider = new Mock<IVacancyQAProvider>();
            vacancyProvider.Setup(vp => vp.GetVacancySummaryViewModel(vacancyReferenceNumber)).Returns(GetValidVacancySummaryViewModel(vacancyReferenceNumber));

            var mediator = new VacancyMediatorBuilder().With(vacancyProvider).Build();

            var response = mediator.GetVacancySummaryViewModel(vacancyReferenceNumber);

            response.AssertCode(VacancyMediatorCodes.GetVacancySummaryViewModel.Ok);
        }


        [Test]
        public void UpdateVacancyWillNotUpdateVacancyIfViewModelIsNotValid()
        {
            var vacancyProvider = new Mock<IVacancyQAProvider>();

            var mediator = new VacancyMediatorBuilder().With(vacancyProvider).Build();
            var viewModel = new FurtherVacancyDetailsViewModel
            {
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.UtcNow),
                    PossibleStartDate = new DateViewModel(DateTime.UtcNow)
                }
            };

            var result = mediator.UpdateVacancy(viewModel);

            result.AssertValidationResult(VacancyMediatorCodes.UpdateVacancy.FailedValidation);
            vacancyProvider.Verify(vp => vp.UpdateVacancyWithComments(viewModel), Times.Never);
        }

        [Test]
        public void UpdateVacancyWithAVacancyThatAcceptsWarningsWithWarningsShouldUpdateVacancy()
        {
            var vacancyProvider = new Mock<IVacancyQAProvider>();

            var mediator = new VacancyMediatorBuilder().With(vacancyProvider).Build();
            var viewModel = GetValidVacancySummaryViewModel(1);
            viewModel.VacancyDatesViewModel.ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(10));
            viewModel.AcceptWarnings = true;

            vacancyProvider.Setup(vp => vp.UpdateVacancyWithComments(viewModel))
                .Returns(new QAActionResult<FurtherVacancyDetailsViewModel>(QAActionResultCode.Ok,
                    new FurtherVacancyDetailsViewModel()));

            var result = mediator.UpdateVacancy(viewModel);

            result.AssertCode(VacancyMediatorCodes.UpdateVacancy.Ok);
            vacancyProvider.Verify(vp => vp.UpdateVacancyWithComments(viewModel));
        }

        [Test]
        public void UpdateVacancyWithAVacancyThatDontAcceptsWarningsWithWarningsShouldNotUpdateVacancy()
        {
            var vacancyProvider = new Mock<IVacancyQAProvider>();

            var mediator = new VacancyMediatorBuilder().With(vacancyProvider).Build();
            var viewModel = GetValidVacancySummaryViewModel(1);
            viewModel.VacancyDatesViewModel.ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(10));
            viewModel.AcceptWarnings = false;

            var result = mediator.UpdateVacancy(viewModel);

            result.AssertValidationResult(VacancyMediatorCodes.UpdateVacancy.FailedValidation);
            vacancyProvider.Verify(vp => vp.UpdateVacancyWithComments(viewModel), Times.Never);
        }

        [Test]
        public void GetVacancyQuestionsViewModelShouldGetViewmodelFromProvider()
        {
            const int vacancyReferenceNumber = 1;
            var vacancyProvider = new Mock<IVacancyQAProvider>();
            var mediator = new VacancyMediatorBuilder().With(vacancyProvider).Build();
            var viewModel = new VacancyQuestionsViewModel();
            vacancyProvider.Setup(vp => vp.GetVacancyQuestionsViewModel(vacancyReferenceNumber)).Returns(viewModel);

            var result = mediator.GetVacancyQuestionsViewModel(vacancyReferenceNumber);

            result.AssertCode(VacancyMediatorCodes.GetVacancyQuestionsViewModel.Ok);
            vacancyProvider.Verify(vp => vp.GetVacancyQuestionsViewModel(vacancyReferenceNumber));
        }

        private static FurtherVacancyDetailsViewModel GetValidVacancySummaryViewModel(int vacancyReferenceNumber)
        {
            return new FurtherVacancyDetailsViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                VacancyDatesViewModel = new VacancyDatesViewModel
                {
                    ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(20)),
                    PossibleStartDate = new DateViewModel(DateTime.UtcNow.AddDays(30))
                },
                Duration = 3,
                DurationType = DurationType.Years,
                LongDescription = "A description",
                WageType = WageType.ApprenticeshipMinimum,
                HoursPerWeek = 30,
                WorkingWeek = "A working week"
            };
        }
    }
}