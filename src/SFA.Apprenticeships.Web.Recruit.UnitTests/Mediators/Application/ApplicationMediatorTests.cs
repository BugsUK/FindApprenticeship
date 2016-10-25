namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.Application
{
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.Security;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation.Results;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.Validators.ProviderUser;
    using Raa.Common.ViewModels.Application;
    using Recruit.Mediators.Application;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using System.Web.Routing;

    [TestFixture]
    public class ApplicationMediatorTests
    {
        [Test]
        public void TestUrlIsEncoded()
        {
            //Arrange
            Mock<IApplicationProvider> mockApplicationProvider = new Mock<IApplicationProvider>();
            Mock<ShareApplicationsViewModelValidator> mockShareApplicationsViewModelValidator = new Mock<ShareApplicationsViewModelValidator>();
            Mock<IEncryptionService<AnonymisedApplicationLink>> mockEncryptionService = new Mock<IEncryptionService<AnonymisedApplicationLink>>();
            Mock<IDateTimeService> mockDateTimeService = new Mock<IDateTimeService>();
            Mock<UrlHelper> mockUrlHelper = new Mock<UrlHelper>();
            Mock<ValidationResult> mockValidationResult = new Mock<ValidationResult>();
            var applicationGuid = Guid.NewGuid();
            string applicantId = "applicantId";
            var applicationSummary = new ApplicationSummaryViewModel() { ApplicationId = applicationGuid, ApplicantID = applicantId };

            mockValidationResult.Setup(m => m.IsValid).Returns(true);

            mockShareApplicationsViewModelValidator.Setup(m => m.Validate(It.IsAny<ShareApplicationsViewModel>()))
                .Returns(mockValidationResult.Object);

            mockApplicationProvider.Setup(m => m.GetShareApplicationsViewModel(It.IsAny<int>()))
                .Returns(new ShareApplicationsViewModel()
                {
                    VacancyType = VacancyType.Apprenticeship,
                    ApplicationSummaries = { applicationSummary }
                });

            //this is the unencrypted version, that we should not use
            mockEncryptionService.Setup(m => m.Encrypt(It.IsAny<AnonymisedApplicationLink>())).Returns("abcABC+123/123");
            //this is the actual url data value
            mockUrlHelper.Setup(m => m.RouteUrl(It.IsAny<string>(), It.IsAny<RouteValueDictionary>())).Returns("THE_URL");

            var parameters = new ShareApplicationsViewModel();
            parameters.SelectedApplicationIds = new List<Guid>() { applicationGuid };
            parameters.ApplicationSummaries = new List<ApplicationSummaryViewModel>() { applicationSummary };

            var mediator = new ApplicationMediator(mockApplicationProvider.Object, mockShareApplicationsViewModelValidator.Object, mockEncryptionService.Object, mockDateTimeService.Object);

            //Act
            mediator.ShareApplications(parameters, mockUrlHelper.Object);

            //Assert
            mockApplicationProvider.Verify(m => m.ShareApplications(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);
            mockApplicationProvider.Verify(m => m.ShareApplications(It.IsAny<int>(), It.IsAny<string>(), It.Is<Dictionary<string, string>>(d => d[applicantId].Equals("THE_URL")), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once);
            mockUrlHelper.Verify(m => m.RouteUrl(It.IsAny<string>(), It.Is<RouteValueDictionary>(rvd => rvd["application"].ToString().Contains("+"))), Times.Never);
            mockUrlHelper.Verify(m => m.RouteUrl(It.IsAny<string>(), It.Is<RouteValueDictionary>(rvd => rvd["application"].ToString().Contains("/"))), Times.Never);
        }
    }
}
