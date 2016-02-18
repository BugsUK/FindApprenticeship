namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    using Ploeh.AutoFixture;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using Common.ViewModels.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Views.VacancyPosting;

    [TestFixture]
    public class CreateVacancyTests : ViewUnitTest
    {
        [Test]
        public void ShouldShowSaveAndExitButton()
        {
            var details = new CreateVacancy();

            var viewModel = new NewVacancyViewModel
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel()
                {
                    Employer = new EmployerViewModel()
                    {
                        Address = new AddressViewModel()
                    }
                }
            };
           
            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("createVacancyAndExit").Should().NotBeNull("Should exists a save and exit button");
        }

        [Test]
        public void ShouldShowSaveAndContinueToPreviewButtonWhenEditingRejectedVacancy()
        {
            var details = new CreateVacancy();

            var viewModel = new Fixture().Build<NewVacancyViewModel>()
                .With(v => v.Status, VacancyStatus.RejectedByQA)
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("createVacancyButton").Should().NotBeNull("Should exists a save button");
            view.GetElementbyId("createVacancyButton").InnerHtml.Should().Be("Save and return to Preview");
            view.GetElementbyId("createVacancyButton").Attributes["value"].Value.Should().Be("CreateVacancyAndPreview");
        }

        [Test]
        public void ShouldShowSaveButtonWhenEditingDraftVacancy()
        {
            var details = new CreateVacancy();

            var viewModel = new Fixture().Build<NewVacancyViewModel>()
                .With(v => v.Status, VacancyStatus.Draft)
                .With(v => v.ComeFromPreview, false)
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("createVacancyButton").Should().NotBeNull("Should exists a save button");
            view.GetElementbyId("createVacancyButton").InnerHtml.Should().Be("Save and continue");
            view.GetElementbyId("createVacancyButton").Attributes["value"].Value.Should().Be("CreateVacancy");
        }
    }
}
