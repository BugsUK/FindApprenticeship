namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System.Threading.Tasks;
    using Common.Mediators;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Recruit.Mediators.VacancyPosting;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    [Parallelizable]
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
        public async Task ShouldReturnOfflineVacancyOkIfCalledFromTheSaveAndContinueActionAndTheVacancyIsAnOfflineOne()
        {
            VacancyPostingProvider.Setup(p => p.GetVacancy(_viewModel.VacancyReferenceNumber))
                .Returns(Task.FromResult(new VacancyViewModel
                {
                    NewVacancyViewModel = new NewVacancyViewModel
                    {
                        OfflineVacancy = true
                    }
                }));

            var vacancyRequirementsAndProspects = new VacancyRequirementsProspectsViewModel();
            VacancyPostingProvider.Setup(p => p.UpdateVacancy(It.IsAny<VacancyRequirementsProspectsViewModel>()))
                .Returns(vacancyRequirementsAndProspects);

            var mediator = GetMediator();

            var result = await mediator.UpdateVacancy(_viewModel);

            result.Should()
                .Match(
                    (MediatorResponse<VacancyRequirementsProspectsViewModel> p) =>
                        p.Code == VacancyPostingMediatorCodes.UpdateVacancy.OfflineVacancyOk);
        }

        [Test]
        public async Task ShouldReturnOnlineVacancyOkIfCalledFromTheSaveAndContinueActionAndTheVacancyIsAnOnlineOne()
        {
            var newVacancyViewModel = new VacancyViewModel
            {
                NewVacancyViewModel = new NewVacancyViewModel
                { 
                    OfflineVacancy = false
                }
            };

            var vacancyRequirementsAndProspects = new VacancyRequirementsProspectsViewModel();

            VacancyPostingProvider.Setup(p => p.GetVacancy(_viewModel.VacancyReferenceNumber))
                .Returns(Task.FromResult(newVacancyViewModel));
            VacancyPostingProvider.Setup(p => p.UpdateVacancy(It.IsAny<VacancyRequirementsProspectsViewModel>()))
                .Returns(vacancyRequirementsAndProspects);

            var mediator = GetMediator();

            var result = await mediator.UpdateVacancy(_viewModel);

            result.Should()
                .Match(
                    (MediatorResponse<VacancyRequirementsProspectsViewModel> p) =>
                        p.Code == VacancyPostingMediatorCodes.UpdateVacancy.OnlineVacancyOk);
        }
    }
}