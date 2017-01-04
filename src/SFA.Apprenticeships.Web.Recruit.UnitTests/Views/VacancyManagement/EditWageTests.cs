namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Views.VacancyManagement
{
    using Builders;
    using Domain.Entities.Vacancies;
    using NUnit.Framework;
    using RazorGenerator.Testing;
    using Recruit.Views.VacancyManagement;

    [TestFixture]
    public class EditWageTests
    {
        [Test]
        public void EditFixedWageChoices()
        {
            var viewModel = new EditWageViewModelBuilder(WageType.Custom).Build();

            var view = new EditWage().RenderAsHtml(viewModel);
        }
    }
}