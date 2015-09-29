namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.ProviderUser
{
    using Common.Providers.Azure.AccessControlService;
    using Moq;
    using NUnit.Framework;
    using Recruit.Mediators.ProviderUser;
    using Recruit.Providers;
    using Recruit.Validators.ProviderUser;

    public class TestBase
    {
        protected Mock<IProviderUserProvider> MockProviderUserProvider;
        protected Mock<IProviderProvider> MockProviderProvider;
        protected Mock<IAuthorizationErrorProvider> MockAuthorizationErrorProvider;

        [SetUp]
        public void SetUp()
        {
            MockProviderUserProvider = new Mock<IProviderUserProvider>();
            MockProviderProvider = new Mock<IProviderProvider>();
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
                providerUserViewModelValidator,
                verifyEmailViewModelValidator);
        }
    }
}
