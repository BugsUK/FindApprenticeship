namespace SFA.Apprenticeship.Api.AvService.UnitTests.Mediators.Version51.ReferenceDataService
{
    using System;
    using AvService.Mediators.Version51;
    using AvService.Providers;
    using AvService.Providers.Version51;
    using Domain;
    using Moq;
    using NUnit.Framework;

    public class ReferenceDataServiceMediatorTestsBase
    {
        protected Mock<IWebServiceAuthenticationProvider> MockWebServiceAuthenticationProvider;
        protected Mock<IReferenceDataProvider> MockReferenceDataProvider;

        protected ReferenceDataServiceMediator ReferenceDataServiceMediator;

        [SetUp]
        public void SetUp()
        {
            // Providers.
            MockWebServiceAuthenticationProvider = new Mock<IWebServiceAuthenticationProvider>();
            MockReferenceDataProvider = new Mock<IReferenceDataProvider>();

            MockWebServiceAuthenticationProvider.Setup(mock => mock
                .Authenticate(It.IsAny<Guid>(), It.IsAny<string>(), WebServiceCategory.Reference))
                .Returns(WebServiceAuthenticationResult.Authenticated);

            // Mediator.
            ReferenceDataServiceMediator = new ReferenceDataServiceMediator(
                MockWebServiceAuthenticationProvider.Object, MockReferenceDataProvider.Object);
        }
    }
}