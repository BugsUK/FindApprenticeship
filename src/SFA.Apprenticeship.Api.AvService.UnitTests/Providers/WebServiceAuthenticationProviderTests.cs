namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers
{
    using System;
    using Apprenticeships.Domain.Entities.WebServices;
    using AvService.Providers;
    using AvService.Services;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class WebServiceAuthenticationProviderTests
    {
        private const string ValidExternalSystemPassword = "letmein";
        private const string ValidExternalSystemId = "bbd33d6d-fc94-4be3-9e52-282bf7293356";

        private const string InvalidExternalSystemId = "63c545db-d08f-4a9b-b05b-33727432e987";
        private const string InvalidExternalSystemPassword = "dontletmein";

        private Mock<IWebServiceConsumerService> _mockWebServiceConsumerService;
        private IWebServiceAuthenticationProvider _webServiceAuthenticationProvider;

        [SetUp]
        public void SetUp()
        {
            // Services.
            _mockWebServiceConsumerService = new Mock<IWebServiceConsumerService>();

            // Provider under test.
            _webServiceAuthenticationProvider = new WebServiceAuthenticationProvider(
                _mockWebServiceConsumerService.Object);
        }

        [TestCase(WebServiceAuthenticationResult.Authenticated, ValidExternalSystemId, ValidExternalSystemPassword)]
        [TestCase(WebServiceAuthenticationResult.AuthenticationFailed, ValidExternalSystemId, InvalidExternalSystemPassword)]
        [TestCase(WebServiceAuthenticationResult.InvalidExternalSystemPassword, ValidExternalSystemId, "")]
        [TestCase(WebServiceAuthenticationResult.InvalidExternalSystemPassword, ValidExternalSystemId, " ")]
        [TestCase(WebServiceAuthenticationResult.InvalidExternalSystemId, InvalidExternalSystemId, ValidExternalSystemPassword)]
        public void ShouldAuthenticateWebServiceConsumer(
            WebServiceAuthenticationResult expectedWebServiceAuthenticationResult,
            string externalSystemId,
            string externalSystemPassword)
        {
            // Arrange.
            var validWebServiceConsumer = new WebServiceConsumer
            {
                ExternalSystemId = new Guid(ValidExternalSystemId),
                ExternalSystemPassword = ValidExternalSystemPassword,
                AllowReferenceDataService = true
            };

            _mockWebServiceConsumerService.Setup(mock => mock
                .Get(new Guid(ValidExternalSystemId)))
                .Returns(validWebServiceConsumer);

            _mockWebServiceConsumerService.Setup(mock => mock
                .Get(new Guid(InvalidExternalSystemId)))
                .Returns(default(WebServiceConsumer));

            // Act.
            var result = _webServiceAuthenticationProvider.Authenticate(
                new Guid(externalSystemId), externalSystemPassword, WebServiceCategory.Reference);

            // Assert.
            result.Should().Be(expectedWebServiceAuthenticationResult);
        }

        [TestCase(WebServiceCategory.Reference, false, WebServiceAuthenticationResult.NotAllowed)]
        [TestCase(WebServiceCategory.Reference, true, WebServiceAuthenticationResult.Authenticated)]

        [TestCase(WebServiceCategory.VacancyUpload, false, WebServiceAuthenticationResult.NotAllowed)]
        [TestCase(WebServiceCategory.VacancyUpload, true, WebServiceAuthenticationResult.Authenticated)]

        [TestCase(WebServiceCategory.VacancyDetail, false, WebServiceAuthenticationResult.NotAllowed)]
        [TestCase(WebServiceCategory.VacancyDetail, true, WebServiceAuthenticationResult.Authenticated)]

        [TestCase(WebServiceCategory.VacancySummary, false, WebServiceAuthenticationResult.NotAllowed)]
        [TestCase(WebServiceCategory.VacancySummary, true, WebServiceAuthenticationResult.Authenticated)]
        public void ShouldAuthoriseWebServiceAccess(
            WebServiceCategory webServiceCategory, bool allow, WebServiceAuthenticationResult expectedWebServiceAuthenticationResult)
        {
            // Arrange.
            var webServiceConsumer = new WebServiceConsumer
            {
                ExternalSystemId = new Guid(ValidExternalSystemId),
                ExternalSystemPassword = ValidExternalSystemPassword,
                WebServiceConsumerType = WebServiceConsumerType.Provider,
                AllowReferenceDataService = webServiceCategory == WebServiceCategory.Reference && allow,
                AllowVacancyUploadService = webServiceCategory == WebServiceCategory.VacancyUpload && allow,
                AllowVacancySummaryService = webServiceCategory == WebServiceCategory.VacancySummary && allow,
                AllowVacancyDetailService = webServiceCategory == WebServiceCategory.VacancyDetail && allow
            };

            _mockWebServiceConsumerService.Setup(mock => mock
                .Get(new Guid(ValidExternalSystemId)))
                .Returns(webServiceConsumer);

            // Act.
            var result = _webServiceAuthenticationProvider.Authenticate(
                new Guid(ValidExternalSystemId), ValidExternalSystemPassword, webServiceCategory);

            // Assert.
            result.Should().Be(expectedWebServiceAuthenticationResult);
        }

        [TestCase(WebServiceConsumerType.Provider, WebServiceAuthenticationResult.Authenticated)]
        [TestCase(WebServiceConsumerType.Employer, WebServiceAuthenticationResult.Authenticated)]
        [TestCase(WebServiceConsumerType.ThirdParty, WebServiceAuthenticationResult.NotAllowed)]
        [TestCase(WebServiceConsumerType.Unknown, WebServiceAuthenticationResult.NotAllowed)]
        public void ShouldAuthoriseVacancyUploadWebServiceAccessByConsumerType(
            WebServiceConsumerType webServiceConsumerType, WebServiceAuthenticationResult expectedWebServiceAuthenticationResult)
        {
            // Arrange.
            var webServiceConsumer = new WebServiceConsumer
            {
                ExternalSystemId = new Guid(ValidExternalSystemId),
                ExternalSystemPassword = ValidExternalSystemPassword,
                WebServiceConsumerType = webServiceConsumerType,
                AllowVacancyUploadService = true
            };

            _mockWebServiceConsumerService.Setup(mock => mock
                .Get(new Guid(ValidExternalSystemId)))
                .Returns(webServiceConsumer);

            // Act.
            var result = _webServiceAuthenticationProvider.Authenticate(
                new Guid(ValidExternalSystemId), ValidExternalSystemPassword, WebServiceCategory.VacancyUpload);

            // Assert.
            result.Should().Be(expectedWebServiceAuthenticationResult);
        }
    }
}
