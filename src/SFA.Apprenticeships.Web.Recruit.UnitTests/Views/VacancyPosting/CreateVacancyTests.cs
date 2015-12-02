using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using SFA.Apprenticeships.Web.Common.ViewModels.Locations;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;
using SFA.Apprenticeships.Web.Recruit.Views.VacancyPosting;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    using System.Web.Mvc;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class CreateVacancyTests : ViewUnitTest
    {
        [Test]
        public void ShouldShowSaveAndExitButton()
        {
            var details = new CreateVacancy();

            var viewModel = new NewVacancyViewModel
            {
                SectorsAndFrameworks = new List<SelectListItem>(),
                Standards = new List<StandardViewModel>(),
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
                .With(v => v.Status, ProviderVacancyStatuses.RejectedByQA)
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
                .With(v => v.Status, ProviderVacancyStatuses.Draft)
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("createVacancyButton").Should().NotBeNull("Should exists a save button");
            view.GetElementbyId("createVacancyButton").InnerHtml.Should().Be("Save and continue");
            view.GetElementbyId("createVacancyButton").Attributes["value"].Value.Should().Be("CreateVacancy");
        }
    }
}
