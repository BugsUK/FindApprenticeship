namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Common.Mediators;
    using FluentAssertions;
    using NUnit.Framework;
    using Recruit.Mediators.VacancyPosting;
    using ViewModels.Vacancy;

    [TestFixture]
    public class RequirementsProspectsTests : TestsBase
    {
        const string SomeString = "SomeString";

        readonly VacancyRequirementsProspectsViewModel _viewModel = new VacancyRequirementsProspectsViewModel
        {
            VacancyReferenceNumber = 1,
            DesiredQualifications = SomeString,
            DesiredSkills = SomeString,
            FutureProspects = SomeString,
            PersonalQualities = SomeString,
            ThingsToConsider = SomeString
        };

        [Test]
        public void ShouldReturnOkAndExitIfCalledFromTheSaveAndExitAction()
        {
            var mediator = GetMediator();

            var result = mediator.UpdateVacancyAndExit(_viewModel);

            result.Should()
                .Match(
                    (MediatorResponse<VacancyRequirementsProspectsViewModel> p) =>
                        p.Code == VacancyPostingMediatorCodes.UpdateVacancy.OkAndExit);
        }

        [Test]
        public void ShouldReturnOfflineVacancyOkIfCalledFromTheSaveAndContinueActionAndTheVacancyIsAnOfflineOne()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(_viewModel.VacancyReferenceNumber))
                .Returns(new VacancyViewModel
                {
                    OfflineVacancy = true
                });

            var mediator = GetMediator();

            var result = mediator.UpdateVacancy(_viewModel);

            result.Should()
                .Match(
                    (MediatorResponse<VacancyRequirementsProspectsViewModel> p) =>
                        p.Code == VacancyPostingMediatorCodes.UpdateVacancy.OfflineVacancyOk);
        }

        [Test]
        public void ShouldReturnOnlineVacancyOkIfCalledFromTheSaveAndContinueActionAndTheVacancyIsAnOnlineOne()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(_viewModel.VacancyReferenceNumber))
                .Returns(new VacancyViewModel
                {
                    OfflineVacancy = false
                });

            var mediator = GetMediator();

            var result = mediator.UpdateVacancy(_viewModel);

            result.Should()
                .Match(
                    (MediatorResponse<VacancyRequirementsProspectsViewModel> p) =>
                        p.Code == VacancyPostingMediatorCodes.UpdateVacancy.OnlineVacancyOk);
        }
    }
}