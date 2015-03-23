namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Home
{
    using System;
    using Application.Interfaces.Logging;
    using Candidate.Mediators;
    using Candidate.Mediators.Home;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.Home;
    using Common.Constants;
    using Constants.Pages;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using FluentAssertions;
    using FluentValidation.Results;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class HomeMediatorTests : MediatorBase
    {
        private readonly Mock<ICandidateServiceProvider> _candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
        private readonly Mock<ILogService> _logServiceMock = new Mock<ILogService>(); 

        private readonly Mock<ContactMessageServerViewModelValidator> _contactMessageServerViewModelValidatorMock =
            new Mock<ContactMessageServerViewModelValidator>();
        private const string AString = "A string";
        private const string AnEmail = "valtechnas@gmail.com";
            
        [Test]
        public void GetContactMessageViewModelWithoutCandidateId()
        {
            var homeMediator = new HomeMediator(_candidateServiceProviderMock.Object, _contactMessageServerViewModelValidatorMock.Object, _logServiceMock.Object);

            var response = homeMediator.GetContactMessageViewModel(null);

            response.AssertCode(HomeMediatorCodes.GetContactMessageViewModel.Successful);
            response.ViewModel.Name.Should().BeNull();
            response.ViewModel.Email.Should().BeNull();
        }

        [Test]
        public void GetContactMessageViewModelWithCandidateId()
        {
            var homeMediator = new HomeMediator(_candidateServiceProviderMock.Object, _contactMessageServerViewModelValidatorMock.Object, _logServiceMock.Object);
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

            var response = homeMediator.GetContactMessageViewModel(candidateId);

            response.AssertCode(HomeMediatorCodes.GetContactMessageViewModel.Successful);
            response.ViewModel.Name.Should().Be(string.Format("{0} {1}", candidateFirstName, candidateLastName));
            response.ViewModel.Email.Should().Be(emailAddress);
        }

        [Test]
        public void GetContactMessageViewModelWithoError()
        {
            var homeMediator = new HomeMediator(_candidateServiceProviderMock.Object, _contactMessageServerViewModelValidatorMock.Object, _logServiceMock.Object);
            var candidateId = Guid.NewGuid();
            _candidateServiceProviderMock.Setup(csp => csp.GetCandidate(candidateId)).Throws<ArgumentException>();

            var response = homeMediator.GetContactMessageViewModel(candidateId);

            response.AssertCode(HomeMediatorCodes.GetContactMessageViewModel.Successful);
            response.ViewModel.Name.Should().BeNull();
            response.ViewModel.Email.Should().BeNull();
        }

        [Test]
        public void SendContactMessage()
        {
            var homeMediator = new HomeMediator(_candidateServiceProviderMock.Object, _contactMessageServerViewModelValidatorMock.Object, _logServiceMock.Object);
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
            var response = homeMediator.SendContactMessage(null, viewModel);

            response.AssertMessage(HomeMediatorCodes.SendContactMessage.SuccessfullySent,
                ApplicationPageMessages.SendContactMessageSucceeded, UserMessageLevel.Success, true);
        }

        [Test]
        public void SendContactMessageFail()
        {
            var homeMediator = new HomeMediator(_candidateServiceProviderMock.Object, _contactMessageServerViewModelValidatorMock.Object, _logServiceMock.Object);
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
            var response = homeMediator.SendContactMessage(null, viewModel);

            response.AssertMessage(HomeMediatorCodes.SendContactMessage.Error,
                ApplicationPageMessages.SendContactMessageFailed, UserMessageLevel.Warning, true);
        }

        [Test]
        public void SendContactMessageWithValidationErrors()
        {
            var homeMediator = new HomeMediator(_candidateServiceProviderMock.Object, _contactMessageServerViewModelValidatorMock.Object, _logServiceMock.Object);
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
            var response = homeMediator.SendContactMessage(null, viewModel);

            response.AssertValidationResult(HomeMediatorCodes.SendContactMessage.ValidationError, true);
        }
    }
}