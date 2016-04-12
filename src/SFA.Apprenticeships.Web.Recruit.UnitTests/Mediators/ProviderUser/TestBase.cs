namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.ProviderUser
{
    using Common.Providers.Azure.AccessControlService;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.Validators.ProviderUser;
    using Recruit.Mediators.ProviderUser;

    using SFA.Apprenticeships.Application.Interfaces.Providers;
    using SFA.Infrastructure.Interfaces;

    public class TestBase
    {
        protected Mock<IAuthorizationErrorProvider> MockAuthorizationErrorProvider;
        protected Mock<IProviderProvider> MockProviderProvider;
        protected Mock<IProviderUserProvider> MockProviderUserProvider;
        protected Mock<IVacancyPostingProvider> MockVacancyProvider;
        protected Mock<IMapper> Mapper;
        protected Mock<IProviderService> MockProviderService;
        protected Mock<ILogService> MockLogService;

        [SetUp]
        public void SetUp()
        {
            MockProviderUserProvider = new Mock<IProviderUserProvider>();
            MockProviderProvider = new Mock<IProviderProvider>();
            MockVacancyProvider = new Mock<IVacancyPostingProvider>();
            MockAuthorizationErrorProvider = new Mock<IAuthorizationErrorProvider>();
            Mapper=new Mock<IMapper>();
            MockProviderService=new Mock<IProviderService>();
            MockLogService=new Mock<ILogService>();
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
                verifyEmailViewModelValidator,MockProviderService.Object,Mapper.Object,
                MockLogService.Object);
        }
    }
}