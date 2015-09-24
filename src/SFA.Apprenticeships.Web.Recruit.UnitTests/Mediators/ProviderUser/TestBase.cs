namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.ProviderUser
{
    using Moq;
    using NUnit.Framework;
    using Recruit.Mediators.ProviderUser;
    using Recruit.Providers;
    using Recruit.Validators.ProviderUser;

    public class TestBase
    {
        protected Mock<IProviderUserProvider> MockProviderUserProvider;
        protected Mock<IProviderProvider> MockProviderProvider;

        [SetUp]
        public void SetUp()
        {
            MockProviderUserProvider = new Mock<IProviderUserProvider>();
            MockProviderProvider = new Mock<IProviderProvider>();
        }

        protected IProviderUserMediator GetMediator()
        {
            var providerUserViewModelValidator = new ProviderUserViewModelValidator();
            var verifyEmailViewModelValidator = new VerifyEmailViewModelValidator();
            return new ProviderUserMediator(MockProviderUserProvider.Object, MockProviderProvider.Object, providerUserViewModelValidator, verifyEmailViewModelValidator);
        }
    }
}
