namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views.Shared.DisplayTemplates.Vacancy
{
    using Common.Views.Shared.DisplayTemplates.Vacancy;
    using Ploeh.AutoFixture;
    using ViewModels.Vacancy;

    public abstract class BasicVacancyDetailsTestsBase : ViewUnitTest
    {
        protected NewVacancyViewModel ViewModel = new Fixture().Build<NewVacancyViewModel>().Create();
        protected BasicVacancyDetails Details = new BasicVacancyDetails();
    }
}