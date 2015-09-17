namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.ProviderUser
{
    using Moq;
    using NUnit.Framework;
    using Providers;
    using Recruit.Mediators.ProviderUser;
    using Recruit.Validators.ProviderUser;

    public class TestBase
    {
        protected Mock<IProviderUserProvider> MockProviderUserProvider;

        [SetUp]
        public void SetUp()
        {
            MockProviderUserProvider = new Mock<IProviderUserProvider>();
        }

        protected IProviderUserMediator GetMediator()
        {
            var providerUserViewModelValidator = new ProviderUserViewModelValidator();
            var verifyEmailViewModelValidator = new VerifyEmailViewModelValidator();
            return new ProviderUserMediator(MockProviderUserProvider.Object, providerUserViewModelValidator, verifyEmailViewModelValidator);
        }
    }
}
