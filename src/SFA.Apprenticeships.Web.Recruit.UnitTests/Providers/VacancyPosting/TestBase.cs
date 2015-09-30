namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Moq;
    using NUnit.Framework;
    using Recruit.Providers;

    public abstract class TestBase
    {
        protected Mock<IUserProfileService> MockUserProfileService;
        protected Mock<IReferenceDataService> MockReferenceDataService;

        [SetUp]
        public void SetUpBase()
        {
            MockUserProfileService = new Mock<IUserProfileService>();
            MockReferenceDataService = new Mock<IReferenceDataService>();
        }

        protected IVacancyPostingProvider GetProvider()
        {
            return new VacancyPostingProvider(
                MockUserProfileService.Object,
                MockReferenceDataService.Object);
        }
    }
}
