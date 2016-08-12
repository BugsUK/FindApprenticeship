namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Users;
    using Builders;
    using Constants.Pages;
    using Domain.Entities.UnitTests.Builder;
    using Domain.Entities.Users;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class LoginTests
    {
        [TestCase(true)]
        [TestCase(false)]
        public void ShouldSetMobileVerificationRequired(bool verifiedMobile)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var userAccountService = new Mock<IUserAccountService>();

            userAccountService
                .Setup(s => s.GetUser(LoginViewModelBuilder.ValidEmailAddress, false))
                .Returns(new User
                {
                    Status = UserStatuses.Active
                });

            var candidateService = new Mock<ICandidateService>();

            candidateService
                .Setup(cs => cs.Authenticate(LoginViewModelBuilder.ValidEmailAddress, LoginViewModelBuilder.ValidPassword))
                .Returns(new CandidateBuilder(candidateId).EnableApplicationStatusChangeAlertsViaText(true).VerifiedMobile(verifiedMobile).Build);

            var provider = new CandidateServiceProviderBuilder().With(candidateService).With(userAccountService).Build();
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            // Act.
            var resultViewModel = provider.Login(viewModel);

            // Assert.
            resultViewModel.MobileVerificationRequired.Should().Be(!verifiedMobile);
        }

        [TestCase(null, false)]
        [TestCase("jane.doe@example.com", true)]
        public void ShouldSetPendingUsernameVerificationRequired(string pendingUsername, bool expectedResult)
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var userAccountService = new Mock<IUserAccountService>();

            userAccountService
                .Setup(s => s.GetUser(LoginViewModelBuilder.ValidEmailAddress, false))
                .Returns(new User
                {
                    Status = UserStatuses.Active,
                    PendingUsername = pendingUsername
                });

            var candidateService = new Mock<ICandidateService>();

            candidateService
                .Setup(cs => cs.Authenticate(LoginViewModelBuilder.ValidEmailAddress, LoginViewModelBuilder.ValidPassword))
                .Returns(new CandidateBuilder(candidateId).EnableApplicationStatusChangeAlertsViaText(true).Build);

            var provider = new CandidateServiceProviderBuilder().With(candidateService).With(userAccountService).Build();
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            // Act.
            var resultViewModel = provider.Login(viewModel);

            // Assert.
            resultViewModel.PendingUsernameVerificationRequired.Should().Be(expectedResult);
        }

        [Test]
        public void ShouldNotAllowLoginIfPendingDeletion()
        {
            // Arrange.
            var candidateId = Guid.NewGuid();
            var userAccountService = new Mock<IUserAccountService>();

            userAccountService
                .Setup(s => s.GetUser(LoginViewModelBuilder.ValidEmailAddress, false))
                .Returns(new User
                {
                    Status = UserStatuses.PendingDeletion
                });

            var provider = new CandidateServiceProviderBuilder().With(userAccountService).Build();
            var viewModel = new LoginViewModelBuilder().WithValidCredentials().Build();

            // Act.
            var resultViewModel = provider.Login(viewModel);

            /*return new LoginResultViewModel(LoginPageMessages.InvalidUsernameOrPasswordErrorText)
            {
                EmailAddress = model.EmailAddress,
                UserStatus = userStatus
            };*/

            // Assert.
            resultViewModel.ViewModelMessage.Should().Be(LoginPageMessages.InvalidUsernameAndOrPasswordErrorText);
        }
    }
}