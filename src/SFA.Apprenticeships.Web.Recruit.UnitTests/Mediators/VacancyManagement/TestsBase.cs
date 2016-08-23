namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyManagement
{
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Recruit.Mediators.VacancyManagement;

    public abstract class TestsBase
    {
        internal Mock<IVacancyManagementProvider> MockVacancyManagementProvider;

        [SetUp]
        public void Setup()
        {
            MockVacancyManagementProvider = new Mock<IVacancyManagementProvider>();
        }

        protected IVacancyManagementMediator GetMediator()
        {
            return new VacancyManagementMediator(MockVacancyManagementProvider.Object);
        }
    }
}
