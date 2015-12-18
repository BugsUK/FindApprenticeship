using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Login
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Builders;
    using Candidate.Mediators.Login;
    using Candidate.Providers;
    using Common.Configuration;
    using Common.Constants;
    using Common.Providers;
    using Constants;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using Domain.Entities.Vacancies;
    using SFA.Infrastructure.Interfaces;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class IndexTests
    {
        [Test]
        public void ValidationError()
        {
            var viewModel = new LoginViewModelBuilder().Build();

            var mediator = new LoginMediatorBuilder().Build();
            var response = mediator.Index(viewModel);

            response.AssertValidationResult(LoginMediatorCodes.Index.ValidationError);
        }

        [Test]
        public void AccountLocked()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder(UserStatuses.Locked).Build);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.AccountLocked);
        }

        [Test]
        public void PendingActivation()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            var loginResultViewModel = new LoginResultViewModelBuilder(UserStatuses.PendingActivation).Build();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(loginResultViewModel);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.PendingActivation);
        }

        [Test]
        public void SessionReturnUrl()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string returnUrl = "/localallowedurl";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.SessionReturnUrl)).Returns(returnUrl);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().Build);
            candidateServiceProvider.Setup(x => x.GetApprenticeshipApplications(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new List<ApprenticeshipApplicationSummary>());
            candidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<string>())).Returns(new Candidate { EntityId = Guid.Empty });

            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.ReturnUrl, true, true);
            response.Parameters.Should().Be(returnUrl);
        }

        [Test]
        public void SessionReturnUrlNotAllowed()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string returnUrl = "http://notallowedurl.com/";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.SessionReturnUrl)).Returns(returnUrl);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().Build);
            candidateServiceProvider.Setup(x => x.GetApprenticeshipApplications(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new List<ApprenticeshipApplicationSummary>());
            candidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<string>())).Returns(new Candidate { EntityId = Guid.Empty });

            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.Ok, true);
            response.Parameters.Should().BeNull();
        }

        [Test]
        public void ReturnUrl()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string returnUrl = "/localUrl/";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.ReturnUrl)).Returns(returnUrl);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().Build);
            candidateServiceProvider.Setup(x => x.GetApprenticeshipApplications(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new List<ApprenticeshipApplicationSummary>());
            candidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<string>())).Returns(new Candidate { EntityId = Guid.Empty });
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.ReturnUrl, true, true);
            response.Parameters.Should().Be(returnUrl);
        }

        [Test]
        public void ReturnUrlNotAllowed()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string returnUrl = "http://return.url.com";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.ReturnUrl)).Returns(returnUrl);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().Build);
            candidateServiceProvider.Setup(x => x.GetApprenticeshipApplications(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new List<ApprenticeshipApplicationSummary>());
            candidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<string>())).Returns(new Candidate { EntityId = Guid.Empty });

            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.Ok, true);
            response.Parameters.Should().BeNull();
        }

        [Test]
        public void ApprenticeshipApply()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string vacancyId = "1";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(CandidateDataItemNames.LastViewedVacancy)).Returns(VacancyType.Apprenticeship + "_" + vacancyId);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().WithEmailAddress(LoginViewModelBuilder.ValidEmailAddress).Build);
            var entityId = Guid.NewGuid();
            candidateServiceProvider.Setup(p => p.GetCandidate(LoginViewModelBuilder.ValidEmailAddress)).Returns(new Candidate { EntityId = entityId });
            candidateServiceProvider.Setup(p => p.GetApplicationStatus(entityId, 1)).Returns(ApplicationStatuses.Draft);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.ApprenticeshipApply, true, true);
            response.Parameters.Should().Be(int.Parse(vacancyId));
        }

        [Test]
        public void ApprenticeshipDetails()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string vacancyId = "1";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(CandidateDataItemNames.LastViewedVacancy)).Returns(VacancyType.Apprenticeship + "_" + vacancyId);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().WithEmailAddress(LoginViewModelBuilder.ValidEmailAddress).Build);
            var entityId = Guid.NewGuid();
            candidateServiceProvider.Setup(p => p.GetCandidate(LoginViewModelBuilder.ValidEmailAddress)).Returns(new Candidate { EntityId = entityId });
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.ApprenticeshipDetails, true, true);
            response.Parameters.Should().Be(int.Parse(vacancyId));
        }

        [Test]
        public void TraineeshipDetails()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string vacancyId = "1";
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(CandidateDataItemNames.LastViewedVacancy)).Returns(VacancyType.Traineeship + "_" + vacancyId);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().WithEmailAddress(LoginViewModelBuilder.ValidEmailAddress).Build);
            var entityId = Guid.NewGuid();
            candidateServiceProvider.Setup(p => p.GetCandidate(LoginViewModelBuilder.ValidEmailAddress)).Returns(new Candidate { EntityId = entityId });
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.TraineeshipDetails, true, true);
            response.Parameters.Should().Be(int.Parse(vacancyId));
        }

        [Test]
        public void LoginFailed()
        {
            var viewModel = new LoginViewModelBuilder().WithEmailAddress(LoginViewModelBuilder.ValidEmailAddress).WithPassword(LoginViewModelBuilder.InvalidPassword).Build();

            const string viewModelMessage = "Invalid Email Address or Password";
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder(UserStatuses.Unknown, false).WithViewModelMessage(viewModelMessage).Build);
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.LoginFailed, true, true);
            response.Parameters.Should().Be(viewModelMessage);
        }

        [Test]
        public void Ok()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().Build);
            candidateServiceProvider.Setup(x => x.GetApprenticeshipApplications(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new List<ApprenticeshipApplicationSummary>());
            candidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<string>())).Returns(new Candidate { EntityId = Guid.Empty });

            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.Ok);
        }

        [Test]
        public void SavedAndDraftCountIsSet()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            
            var applicationStatusSummaries = new Fixture().CreateMany<ApprenticeshipApplicationSummary>(25);
            candidateServiceProvider.Setup(x => x.GetApprenticeshipApplications(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(applicationStatusSummaries);
            candidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<string>())).Returns(new Candidate {EntityId = Guid.Empty});
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().Build);

            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(x => x.Push(UserDataItemNames.SavedAndDraftCount, It.IsAny<string>()));

            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.Ok);
            var count = applicationStatusSummaries.Count(a => a.Status == ApplicationStatuses.Draft || a.Status == ApplicationStatuses.Saved);
            userDataProvider.Verify(x => x.Push(UserDataItemNames.SavedAndDraftCount, count.ToString(CultureInfo.InvariantCulture)), Times.Once);
        }

        [Test]
        public void TermsAndConditionsVersion()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            const string returnUrl = "/allowedasolutoepath";
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration() {TermsAndConditionsVersion = "2", VacancyResultsPerPage = 5});
            var userDataProvider = new Mock<IUserDataProvider>();
            userDataProvider.Setup(p => p.Pop(UserDataItemNames.ReturnUrl)).Returns(returnUrl);
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(new LoginResultViewModelBuilder().WithAcceptedTermsAndConditionsVersion("1").Build);
            candidateServiceProvider.Setup(x => x.GetApprenticeshipApplications(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new List<ApprenticeshipApplicationSummary>());
            candidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<string>())).Returns(new Candidate { EntityId = Guid.Empty });

            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).With(userDataProvider).With(configurationService).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.TermsAndConditionsNeedAccepted, true, true);
            response.Parameters.Should().Be(returnUrl);
        }

        [Test]
        public void MobileVerificationRequired()
        {
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            var loginResultViewModel = new LoginResultViewModelBuilder().MobileVerificationRequired().Build();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();
            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(loginResultViewModel);
            candidateServiceProvider.Setup(x => x.GetApprenticeshipApplications(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new List<ApprenticeshipApplicationSummary>());
            candidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<string>())).Returns(new Candidate { EntityId = Guid.Empty });
            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            var response = mediator.Index(viewModel);

            response.AssertCode(LoginMediatorCodes.Index.Ok);
            response.ViewModel.MobileVerificationRequired.Should().BeTrue();
        }

        [TestCase(false)]
        [TestCase(true)]
        public void PendingUsernameVerificationRequired(bool expectedValue)
        {
            // Arrange.
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            var loginResultViewModel = new LoginResultViewModelBuilder().PendingUsernameVerificationRequired(expectedValue).Build();
            var candidateServiceProvider = new Mock<ICandidateServiceProvider>();

            candidateServiceProvider.Setup(p => p.Login(viewModel)).Returns(loginResultViewModel);
            candidateServiceProvider.Setup(x => x.GetApprenticeshipApplications(It.IsAny<Guid>(), It.IsAny<bool>())).Returns(new List<ApprenticeshipApplicationSummary>());
            candidateServiceProvider.Setup(x => x.GetCandidate(It.IsAny<string>())).Returns(new Candidate { EntityId = Guid.Empty });

            var mediator = new LoginMediatorBuilder().With(candidateServiceProvider).Build();

            // Act.
            var response = mediator.Index(viewModel);

            // Assert.
            response.AssertCode(LoginMediatorCodes.Index.Ok);
            response.ViewModel.PendingUsernameVerificationRequired.Should().Be(expectedValue);
        }
    }
}