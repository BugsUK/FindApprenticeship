namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using Application.Interfaces.Candidates;
    using Candidate.ViewModels.Register;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class RequestEmailReminderTests
    {
        [Test]
        public void Sent()
        {
            const string phoneNumber = "0123456789";
            var viewModel = new ForgottenEmailViewModel
            {
                PhoneNumber = phoneNumber
            };

            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.RequestEmailReminder(phoneNumber));
            var provider = new CandidateServiceProviderBuilder().Build();

            var sent = provider.RequestEmailReminder(viewModel);

            sent.Should().BeTrue();
        }

        [Test]
        public void NotSent_NotFound()
        {
            var phoneNumber = "0123456789";
            var viewModel = new ForgottenEmailViewModel
            {
                PhoneNumber = phoneNumber
            };
            
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.RequestEmailReminder(phoneNumber)).Throws(new CustomException(ErrorCodes.CandidateNotFoundError));
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();

            var sent = provider.RequestEmailReminder(viewModel);

            sent.Should().BeFalse();
        }

        [Test]
        public void NotSent_NotVerified()
        {
            const string phoneNumber = "0123456789";
            var viewModel = new ForgottenEmailViewModel
            {
                PhoneNumber = phoneNumber
            };
            
            var candidateService = new Mock<ICandidateService>();
            candidateService.Setup(cs => cs.RequestEmailReminder(phoneNumber)).Throws(new CustomException(Domain.Entities.ErrorCodes.EntityStateError));
            var provider = new CandidateServiceProviderBuilder().With(candidateService).Build();

            var sent = provider.RequestEmailReminder(viewModel);

            sent.Should().BeFalse();
        }
    }
}