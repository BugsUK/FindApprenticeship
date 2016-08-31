namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.AccountProvider
{
    using System;
    using Application.Interfaces.Users;
    using Candidate.ViewModels.Account;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ErrorCodes = Application.Interfaces.Users.ErrorCodes;

    [TestFixture]
    [Parallelizable]
    public class UpdateEmailAddressTests
    {
        private EmailViewModel viewModel = new EmailViewModel() { EmailAddress = "email@email.com" };
        [Test]
        public void UpdateEmailAddressOk()
        {
            var provider = new AccountProviderBuilder().Build();
            var returnedModel = provider.UpdateEmailAddress(Guid.NewGuid(), viewModel);
            returnedModel.Should().NotBeNull();
            returnedModel.UpdateStatus.Should().Be(UpdateEmailStatus.Ok);
        }

        [Test]
        public void UpdateEmailAddressAccountAlreadyExixtsCustomError()
        {
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(x => x.UpdateUsername(It.IsAny<Guid>(), It.IsAny<string>())).Throws(new CustomException(ErrorCodes.UserDirectoryAccountExistsError));
            var provider = new AccountProviderBuilder().With(userAccountService).Build();
            var returnedModel = provider.UpdateEmailAddress(Guid.NewGuid(), viewModel);
            returnedModel.Should().NotBeNull();
            returnedModel.UpdateStatus.Should().Be(UpdateEmailStatus.AccountAlreadyExists);
        }

        [Test]
        public void UpdateEmailAddressUnknownCustomError()
        {
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(x => x.UpdateUsername(It.IsAny<Guid>(), It.IsAny<string>())).Throws(new CustomException("UNKNOWN"));
            var provider = new AccountProviderBuilder().With(userAccountService).Build();
            var returnedModel = provider.UpdateEmailAddress(Guid.NewGuid(), viewModel);
            returnedModel.Should().NotBeNull();
            returnedModel.UpdateStatus.Should().Be(UpdateEmailStatus.Error);
        }

        [Test]
        public void UpdateEmailAddressUnknownError()
        {
            var userAccountService = new Mock<IUserAccountService>();
            userAccountService.Setup(x => x.UpdateUsername(It.IsAny<Guid>(), It.IsAny<string>())).Throws(new Exception());
            var provider = new AccountProviderBuilder().With(userAccountService).Build();
            var returnedModel = provider.UpdateEmailAddress(Guid.NewGuid(), viewModel);
            returnedModel.Should().NotBeNull();
            returnedModel.UpdateStatus.Should().Be(UpdateEmailStatus.Error);
        }
    }
}
