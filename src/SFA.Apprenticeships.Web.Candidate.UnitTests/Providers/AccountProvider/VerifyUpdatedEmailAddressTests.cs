namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using Application.Interfaces.Candidates;
    using Candidate.ViewModels.Account;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ErrorCodes = Application.Interfaces.Users.ErrorCodes;

    [TestFixture]
    [Parallelizable]
    public class VerifyUpdatedEmailAddressTests
    {
        [Test]
        public void VerifyUpdatedEmailAddressTestsOk()
        {
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(x => x.UpdateUsername(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()));
            var provider = new AccountProviderBuilder().With(candidateService).Build();
            var returnedModel = provider.VerifyUpdatedEmailAddress(Guid.NewGuid(), new VerifyUpdatedEmailViewModel());
            
            returnedModel.Should().NotBeNull();
            returnedModel.UpdateStatus.Should().Be(UpdateEmailStatus.Updated);
            returnedModel.ViewModelMessage.Should().BeNull();
        }

        [TestCase(ErrorCodes.UserDirectoryAccountExistsError, UpdateEmailStatus.AccountAlreadyExists)]
        [TestCase(ErrorCodes.InvalidUpdateUsernameCode, UpdateEmailStatus.InvalidUpdateUsernameCode)]
        [TestCase(ErrorCodes.UserPasswordError, UpdateEmailStatus.UserPasswordError)]
        public void VerifyUpdatedEmailAddressTestsCustomException(string exceptionCode, UpdateEmailStatus emailStatus)
        {
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(x => x.UpdateUsername(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new CustomException(exceptionCode));
            var provider = new AccountProviderBuilder().With(candidateService).Build();
            var returnedModel = provider.VerifyUpdatedEmailAddress(Guid.NewGuid(), new VerifyUpdatedEmailViewModel());

            returnedModel.Should().NotBeNull();
            returnedModel.UpdateStatus.Should().Be(emailStatus);
            returnedModel.ViewModelMessage.Should().Be(emailStatus.ToString());
        }

        public void VerifyUpdatedEmailAddressTestsException()
        {
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(x => x.UpdateUsername(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());
            var provider = new AccountProviderBuilder().With(candidateService).Build();
            var returnedModel = provider.VerifyUpdatedEmailAddress(Guid.NewGuid(), new VerifyUpdatedEmailViewModel());

            returnedModel.Should().NotBeNull();
            returnedModel.UpdateStatus.Should().Be(UpdateEmailStatus.Error);
            returnedModel.ViewModelMessage.Should().Be(UpdateEmailStatus.Error.ToString());
        }
    }
}
