namespace SFA.Apprenticeship.Api.AvService.UnitTests.Providers
{
    using System;
    using System.Collections.Generic;
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


        public const string NoWebServiceCategories = "NoWebServiceCategories";
        public const string AllWebServiceCategories = "AllWebServiceCategories";
        public const string ReferenceWebServiceCategory = "ReferenceWebServiceCategory";
        public const string VacancyWebServiceCategories = "VacancyWebServiceCategories";

        private readonly Dictionary<string, List<WebServiceCategory>> _allowedWebCategories = new Dictionary<string, List<WebServiceCategory>>
        {
            {
                NoWebServiceCategories, new List<WebServiceCategory>()
            },
            {
                AllWebServiceCategories, new List<WebServiceCategory>
                {
                    WebServiceCategory.Reference,
                    WebServiceCategory.VacancyUpload,
                    WebServiceCategory.VacancySummary,
                    WebServiceCategory.VacancyDetail
                }
            },
            {
                ReferenceWebServiceCategory, new List<WebServiceCategory>
                {
                    WebServiceCategory.Reference
                }
            },
            {
                VacancyWebServiceCategories, new List<WebServiceCategory>
                {
                    WebServiceCategory.VacancyUpload,
                    WebServiceCategory.VacancySummary,
                    WebServiceCategory.VacancyDetail
                }
            }
        };

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

        [TestCase(WebServiceAuthenticationResult.NotAllowed, WebServiceCategory.Reference, NoWebServiceCategories)]
        [TestCase(WebServiceAuthenticationResult.NotAllowed, WebServiceCategory.Reference, VacancyWebServiceCategories)]
        [TestCase(WebServiceAuthenticationResult.NotAllowed, WebServiceCategory.VacancySummary, ReferenceWebServiceCategory)]
        [TestCase(WebServiceAuthenticationResult.Authenticated, WebServiceCategory.Reference, ReferenceWebServiceCategory)]
        [TestCase(WebServiceAuthenticationResult.Authenticated, WebServiceCategory.VacancyDetail, VacancyWebServiceCategories)]
        public void ShouldAllowWebServiceCategory(
            WebServiceAuthenticationResult expectedWebServiceAuthenticationResult,
            WebServiceCategory webServiceCategory,
            string allowedWebServiceCategoriesName)
        {
            // Arrange.
            var webServiceConsumer = new WebServiceConsumer
            {
                ExternalSystemId = new Guid(ValidExternalSystemId),
                ExternalSystemPassword = ValidExternalSystemPassword
            };

            var allowedWebServiceCategories = _allowedWebCategories[allowedWebServiceCategoriesName];

            webServiceConsumer.AllowReferenceDataService = allowedWebServiceCategories.Contains(WebServiceCategory.Reference);
            webServiceConsumer.AllowVacancyUploadService = allowedWebServiceCategories.Contains(WebServiceCategory.VacancyUpload);
            webServiceConsumer.AllowVacancySummaryService = allowedWebServiceCategories.Contains(WebServiceCategory.VacancySummary);
            webServiceConsumer.AllowVacancyDetailService = allowedWebServiceCategories.Contains(WebServiceCategory.VacancyDetail);

            _mockWebServiceConsumerService.Setup(mock => mock
                .Get(new Guid(ValidExternalSystemId)))
                .Returns(webServiceConsumer);

            // Act.
            var result = _webServiceAuthenticationProvider.Authenticate(
                new Guid(ValidExternalSystemId), ValidExternalSystemPassword, webServiceCategory);

            // Assert.
            result.Should().Be(expectedWebServiceAuthenticationResult);
        }
    }
}
