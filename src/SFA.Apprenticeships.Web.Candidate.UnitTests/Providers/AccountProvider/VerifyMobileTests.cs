namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Builders;
    using Candidate.ViewModels.Account;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ErrorCodes = Domain.Entities.ErrorCodes;

    [TestFixture]
    [Parallelizable]
    public class VerifyMobileTests
    {
        private const string PhoneNumber = "123456789";
        private const string VerificationCode = "1234";

        [TestCase(ErrorCodes.EntityStateError, VerifyMobileState.MobileVerificationNotRequired)]
        [TestCase(Application.Interfaces.Users.ErrorCodes.MobileCodeVerificationFailed,
            VerifyMobileState.VerifyMobileCodeInvalid)]
        [TestCase(Application.Interfaces.Users.ErrorCodes.UnknownUserError, VerifyMobileState.Error)]
        public void GivenEntityStateError_ThenValidViewModelIsReturned(string errorCode,
            VerifyMobileState verifyMobileState)
        {
            //Arrange
            var candidateId = Guid.NewGuid();
            var candidateServiceMock = new Mock<ICandidateService>();
            candidateServiceMock.Setup(cs => cs.VerifyMobileCode(candidateId, VerificationCode))
                .Throws(new CustomException(errorCode));
            var viewModel =
                new VerifyMobileViewModelBuilder().PhoneNumber(PhoneNumber)
                    .MobileVerificationCode(VerificationCode)
                    .Build();
            var provider = new AccountProviderBuilder().With(candidateServiceMock).Build();

            //Act
            var result = provider.VerifyMobile(candidateId, viewModel);

            //Assert
            result.Status.Should().Be(verifyMobileState);
            result.HasError().Should().BeTrue();
            result.ViewModelMessage.Should().NotBeNull();
        }

        [Test]
        public void GivenValidCode_DefaultViewModelIsReturned()
        {
            //Arrange
            var candidateId = Guid.NewGuid();
            var candidateServiceMock = new Mock<ICandidateService>();
            candidateServiceMock.Setup(cs => cs.VerifyMobileCode(candidateId, VerificationCode));
            ;
            var viewModel =
                new VerifyMobileViewModelBuilder().PhoneNumber(PhoneNumber)
                    .MobileVerificationCode(VerificationCode)
                    .Build();
            var provider = new AccountProviderBuilder().With(candidateServiceMock).Build();

            //Act
            var result = provider.VerifyMobile(candidateId, viewModel);

            //Assert
            result.Status.Should().Be(VerifyMobileState.Ok);
            result.HasError().Should().BeFalse();
            result.ViewModelMessage.Should().BeNullOrEmpty();
        }
    }
}