namespace SFA.DAS.RAA.Api.UnitTests.Controllers
{
    using Api.Controllers;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Providers;

    [TestFixture]
    [Parallelizable]
    public class VacancyManagementControllerTests
    {
        [Test]
        public void EditWageUpdatesVacancy()
        {
            var vacancy = new Fixture().Build<Vacancy>().Create();
            var vacancyProvider = new Mock<IVacancyProvider>();
            vacancyProvider.Setup(p => p.Get(1, null, null)).Returns(vacancy);

            var controller = new VacancyManagementController(vacancyProvider.Object);


        }
    }
}