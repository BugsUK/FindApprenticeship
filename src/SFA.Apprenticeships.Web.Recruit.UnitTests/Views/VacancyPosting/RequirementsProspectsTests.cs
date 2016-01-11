using FluentAssertions;
using NUnit.Framework;
using RazorGenerator.Testing;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;
using SFA.Apprenticeships.Web.Recruit.Views.VacancyPosting;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyPosting
{
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class RequirementsProspectsTests : ViewUnitTest
    {
        [Test]
        public void ShouldShowSaveAndExitButton()
        {
            var details = new VacancyRequirementsProspects();

            var viewModel = new VacancyRequirementsProspectsViewModel { };
           
            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("VacancyRequirementsProspectsAndExit").Should().NotBeNull("Should exists a save and exit button");
        }

        [Test]
        public void ShouldShowSaveAndContinueToPreviewButtonWhenEditingRejectedVacancy()
        {
            var details = new VacancyRequirementsProspects();

            var viewModel = new Fixture().Build<VacancyRequirementsProspectsViewModel>()
                .With(v => v.Status, ProviderVacancyStatuses.RejectedByQA)
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("VacancyRequirementsProspectsButton").Should().NotBeNull("Should exists a save button");
            view.GetElementbyId("VacancyRequirementsProspectsButton").InnerHtml.Should().Be("Save and return to Preview");
        }

        [Test]
        public void ShouldShowSaveButtonWhenEditingDraftVacancy()
        {
            var details = new VacancyRequirementsProspects();

            var viewModel = new Fixture().Build<VacancyRequirementsProspectsViewModel>()
                .With(v => v.Status, ProviderVacancyStatuses.Draft)
                .With(v => v.ComeFromPreview, false)
                .Create();

            var view = details.RenderAsHtml(viewModel);

            view.GetElementbyId("VacancyRequirementsProspectsButton").Should().NotBeNull("Should exists a save button");
            view.GetElementbyId("VacancyRequirementsProspectsButton").InnerHtml.Should().Be("Save and continue");
        }
    }
}
