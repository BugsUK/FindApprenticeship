using SFA.Apprenticeships.Web.Common.Mediators;
using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Home
{
    using System;
    using SFA.Infrastructure.Interfaces;
    using Candidate.Mediators;
    using Candidate.Mediators.Home;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.Home;
    using Common.Constants;
    using Common.Constants.Pages;
    using Constants.Pages;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using FluentAssertions;
    using FluentValidation.Results;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class ContactMessageTests : MediatorBase
    {
        private const string AString = "A string";
        private const string AnEmail = "valtechnas@gmail.com";

        private Mock<ICandidateServiceProvider> _candidateServiceProviderMock;
        private Mock<ILogService> _logServiceMock;

        private Mock<ContactMessageServerViewModelValidator> _contactMessageServerViewModelValidatorMock;
        private Mock<FeedbackServerViewModelValidator> _feedbackServerViewModelValidator;
        private HomeMediator _homeMediator;

        [SetUp]
        public void SetUp()
        {
            _logServiceMock = new Mock<ILogService>();
            _candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
            _contactMessageServerViewModelValidatorMock = new Mock<ContactMessageServerViewModelValidator>();
            _feedbackServerViewModelValidator = new Mock<FeedbackServerViewModelValidator>();

            _homeMediator = new HomeMediator(
                _logServiceMock.Object,
                _candidateServiceProviderMock.Object,
                _contactMessageServerViewModelValidatorMock.Object,
                _feedbackServerViewModelValidator.Object);
        }

        [Test]
        public void GetContactMessageViewModelWithoutCandidateId()
        {
            var response = _homeMediator.GetContactMessageViewModel(null);

            response.AssertCode(HomeMediatorCodes.GetContactMessageViewModel.Successful);
            response.ViewModel.Name.Should().BeNull();
            response.ViewModel.Email.Should().BeNull();
        }

        [Test]
        public void GetContactMessageViewModelWithCandidateId()
        {
            var candidateId = Guid.NewGuid();

            const string candidateFirstName = "John";
            const string candidateLastName = "Doe";
            const string emailAddress = "someemail@gmail.com";
            
            _candidateServiceProviderMock.Setup(csp => csp.GetCandidate(candidateId)).Returns(new Candidate
            {
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = candidateFirstName,
                    LastName = candidateLastName,
                    EmailAddress = emailAddress
                }
            });

            var response = _homeMediator.GetContactMessageViewModel(candidateId);

            response.AssertCode(HomeMediatorCodes.GetContactMessageViewModel.Successful);
            response.ViewModel.Name.Should().Be(string.Format("{0} {1}", candidateFirstName, candidateLastName));
            response.ViewModel.Email.Should().Be(emailAddress);
        }

        [Test]
        public void GetContactMessageViewModelWithoError()
        {
            var candidateId = Guid.NewGuid();

            _candidateServiceProviderMock.Setup(csp => csp.GetCandidate(candidateId)).Throws<ArgumentException>();

            var response = _homeMediator.GetContactMessageViewModel(candidateId);

            response.AssertCode(HomeMediatorCodes.GetContactMessageViewModel.Successful);
            response.ViewModel.Name.Should().BeNull();
            response.ViewModel.Email.Should().BeNull();
        }

        [Test]
        public void SendContactMessage()
        {
            var viewModel = new ContactMessageViewModel
            {
                Details = AString,
                Email = AnEmail,
                Enquiry = AString,
                Name = AString,
                SelectedEnquiry = AString
            };

            _contactMessageServerViewModelValidatorMock.Setup(v => v.Validate(It.IsAny<ContactMessageViewModel>()))
                .Returns(new ValidationResult());

            _candidateServiceProviderMock.Setup(hp => hp.SendContactMessage(null, viewModel)).Returns(true);
            
            var response = _homeMediator.SendContactMessage(null, viewModel);

            response.AssertMessage(HomeMediatorCodes.SendContactMessage.SuccessfullySent,
                ApplicationPageMessages.SendContactMessageSucceeded, UserMessageLevel.Success, true);
        }

        [Test]
        public void SendContactMessageFail()
        {
            var viewModel = new ContactMessageViewModel
            {
                Details = AString,
                Email = AnEmail,
                Enquiry = AString,
                Name = AString,
                SelectedEnquiry = AString
            };

            _contactMessageServerViewModelValidatorMock.Setup(v => v.Validate(It.IsAny<ContactMessageViewModel>()))
                .Returns(new ValidationResult());

            _candidateServiceProviderMock.Setup(hp => hp.SendContactMessage(null, viewModel)).Returns(false);
            
            var response = _homeMediator.SendContactMessage(null, viewModel);

            response.AssertMessage(HomeMediatorCodes.SendContactMessage.Error,
                ApplicationPageMessages.SendContactMessageFailed, UserMessageLevel.Warning, true);
        }

        [Test]
        public void SendContactMessageWithValidationErrors()
        {
            var viewModel = new ContactMessageViewModel
            {
                Details = AString,
                Email = AnEmail,
                Enquiry = AString,
                Name = AString,
                SelectedEnquiry = AString
            };

            _contactMessageServerViewModelValidatorMock.Setup(v => v.Validate(It.IsAny<ContactMessageViewModel>()))
                .Returns(new ValidationResult(new []{new ValidationFailure("Name", "Error") }));

            _candidateServiceProviderMock.Setup(hp => hp.SendContactMessage(null, viewModel)).Returns(false);
            var response = _homeMediator.SendContactMessage(null, viewModel);

            response.AssertValidationResult(HomeMediatorCodes.SendContactMessage.ValidationError, true);
        }
    }
}