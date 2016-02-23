namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.ProviderUser
{
    using Common.Providers.Azure.AccessControlService;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.Validators.ProviderUser;
    using Recruit.Mediators.ProviderUser;

    public class TestBase
    {
        protected Mock<IAuthorizationErrorProvider> MockAuthorizationErrorProvider;
        protected Mock<IProviderProvider> MockProviderProvider;
        protected Mock<IProviderUserProvider> MockProviderUserProvider;
        protected Mock<IVacancyPostingProvider> MockVacancyProvider;

        [SetUp]
        public void SetUp()
        {
            MockProviderUserProvider = new Mock<IProviderUserProvider>();
            MockProviderProvider = new Mock<IProviderProvider>();
            MockVacancyProvider = new Mock<IVacancyPostingProvider>();
            MockAuthorizationErrorProvider = new Mock<IAuthorizationErrorProvider>();
        }

        protected IProviderUserMediator GetMediator()
        {
            var providerUserViewModelValidator = new ProviderUserViewModelValidator();
            var verifyEmailViewModelValidator = new VerifyEmailViewModelValidator();

            return new ProviderUserMediator(
                MockProviderUserProvider.Object,
                MockProviderProvider.Object,
                MockAuthorizationErrorProvider.Object,
                MockVacancyProvider.Object,
                providerUserViewModelValidator,
                verifyEmailViewModelValidator);
        }
    }
}