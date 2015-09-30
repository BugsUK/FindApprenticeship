namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Moq;
    using NUnit.Framework;
    using Recruit.Mediators.VacancyPosting;
    using Recruit.Providers;

    public class TestsBase
    {
        protected Mock<IVacancyPostingProvider> VacancyPostingProvider;

        [SetUp]
        public void SetUp()
        {
            VacancyPostingProvider = new Mock<IVacancyPostingProvider>();
        }

        protected IVacancyPostingMediator GetMediator()
        {
            return new VacancyPostingMediator(
                VacancyPostingProvider.Object);
        }
    }
}
