
namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.Home
{
    using System;
    using Candidate.Mediators.Home;
    using Candidate.Providers;
    using Candidate.Validators;
    using Candidate.ViewModels.Home;
    using Common.Constants;
    using Common.Mediators;
    using Common.UnitTests.Mediators;
    using Constants.Pages;
    using Domain.Entities.Candidates;
    using Domain.Entities.Users;
    using FluentAssertions;
    using FluentValidation.Results;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    [Parallelizable]
    public class FeedbackTests : MediatorBase
    {

        [SetUp]
        public void SetUp()
        {
            _candidateServiceProviderMock = new Mock<ICandidateServiceProvider>();
            _logServiceMock = new Mock<ILogService>();
            _contactMessageServerViewModelValidator = new Mock<ContactMessageServerViewModelValidator>();
            _feedbackServerViewModelValidatorMock = new Mock<FeedbackServerViewModelValidator>();

            _homeMediator = new HomeMediator(
                _logServiceMock.Object,
                _candidateServiceProviderMock.Object,
                _contactMessageServerViewModelValidator.Object,
                _feedbackServerViewModelValidatorMock.Object);
        }
        private const string ValidName = "Jane Doe";
        private const string ValidEmail = "jane.doe@example.com";
        private const string ValidDetails = "Some feedback";

        private Mock<ICandidateServiceProvider> _candidateServiceProviderMock;
        private Mock<ILogService> _logServiceMock;
        private Mock<ContactMessageServerViewModelValidator> _contactMessageServerViewModelValidator;
        private Mock<FeedbackServerViewModelValidator> _feedbackServerViewModelValidatorMock;

        private HomeMediator _homeMediator;

        [Test]
        public void GetFeedbackViewModelWithCandidateId()
        {
            var candidateId = Guid.NewGuid();

            const string candidateFirstName = "Jane";
            const string candidateLastName = "Doe";
            const string emailAddress = ValidEmail;
            
            _candidateServiceProviderMock.Setup(mock => mock.GetCandidate(candidateId)).Returns(new Candidate
            {
                RegistrationDetails = new RegistrationDetails
                {
                    FirstName = candidateFirstName,
                    LastName = candidateLastName,
                    EmailAddress = emailAddress
                }
            });

            var response = _homeMediator.GetFeedbackViewModel(candidateId);

            response.AssertCodeAndMessage(HomeMediatorCodes.GetFeedbackViewModel.Successful);
            response.ViewModel.Name.Should().Be(string.Format("{0} {1}", candidateFirstName, candidateLastName));
            response.ViewModel.Email.Should().Be(emailAddress);
        }

        [Test]
        public void GetFeedbackViewModelWithError()
        {
            var candidateId = Guid.NewGuid();

            _candidateServiceProviderMock.Setup(mock => mock.GetCandidate(candidateId)).Throws<ArgumentException>();

            var response = _homeMediator.GetFeedbackViewModel(candidateId);

            response.AssertCodeAndMessage(HomeMediatorCodes.GetFeedbackViewModel.Successful);

            response.ViewModel.Name.Should().BeNull();
            response.ViewModel.Email.Should().BeNull();
            response.ViewModel.Details.Should().BeNull();
        }
    
        [Test]
        public void GetFeedbackViewModelWithoutCandidateId()
        {
            var response = _homeMediator.GetFeedbackViewModel(null);

            response.AssertCodeAndMessage(HomeMediatorCodes.GetFeedbackViewModel.Successful);
            response.ViewModel.Name.Should().BeNull();
            response.ViewModel.Email.Should().BeNull();
            response.ViewModel.Details.Should().BeNull();
        }

        [Test]
        public void SendFeedback()
        {
            var viewModel = new FeedbackViewModel
            {
                Email = ValidEmail,
                Name = ValidName,
                Details = ValidDetails
            };

            _feedbackServerViewModelValidatorMock.Setup(mock => mock
                .Validate(It.IsAny<FeedbackViewModel>()))
                .Returns(new ValidationResult());

            _candidateServiceProviderMock.Setup(mock => mock.SendFeedback(null, viewModel)).Returns(true);

            var response = _homeMediator.SendFeedback(null, viewModel);

            response.AssertMessage(HomeMediatorCodes.SendFeedback.SuccessfullySent,
                ApplicationPageMessages.SendFeedbackSucceeded, UserMessageLevel.Success, true);
        }

        [Test]
        public void SendFeedbackFail()
        {
            var viewModel = new FeedbackViewModel
            {
                Email = ValidEmail,
                Name = ValidName,
                Details = ValidDetails
            };

            _feedbackServerViewModelValidatorMock.Setup(mock => mock.Validate(It.IsAny<FeedbackViewModel>()))
                .Returns(new ValidationResult());

            _candidateServiceProviderMock.Setup(mock => mock.SendFeedback(null, viewModel)).Returns(false);

            var response = _homeMediator.SendFeedback(null, viewModel);

            response.AssertMessage(HomeMediatorCodes.SendFeedback.Error,
                ApplicationPageMessages.SendFeedbackFailed, UserMessageLevel.Warning, true);
        }

        [Test]
        public void SendFeedbackWithValidationErrors()
        {
            var viewModel = new FeedbackViewModel
            {
                Email = ValidEmail,
                Name = ValidName,
                Details = ValidDetails
            };

            _feedbackServerViewModelValidatorMock.Setup(mock => mock.Validate(It.IsAny<FeedbackViewModel>()))
                .Returns(new ValidationResult(new []{new ValidationFailure("Name", "Error") }));

            _candidateServiceProviderMock.Setup(mock => mock.SendFeedback(null, viewModel)).Returns(false);

            var response = _homeMediator.SendFeedback(null, viewModel);

            response.AssertValidationResult(HomeMediatorCodes.SendFeedback.ValidationError, true);
        }
    }
}