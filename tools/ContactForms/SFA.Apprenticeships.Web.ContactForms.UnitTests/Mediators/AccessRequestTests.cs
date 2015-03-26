namespace SFA.Apprenticeships.Web.ContactForms.Tests.Mediators
{
    using Domain.Enums;
    using ContactForms.Mediators;
    using ContactForms.Mediators.Interfaces;
    using Builders;
    using Constants;
    using Constants.Pages;
    using ContactForms.Mediators.AccessRequest;
    using ContactForms.Providers.Interfaces;
    using ViewModels;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    public class AccessRequestTests
    {
        [TestCase(AccessRequestSubmitStatus.Success, AccessRequestMediatorCodes.SubmitAccessRequest.Success, AccessRequestPageMessages.RequestHasBeenSubmittedSuccessfully, UserMessageLevel.Success)]
        [TestCase(AccessRequestSubmitStatus.Error, AccessRequestMediatorCodes.SubmitAccessRequest.Error, AccessRequestPageMessages.ErrorWhileRequestSubmission, UserMessageLevel.Error)]
        public void SubmitAccessRequestTests(AccessRequestSubmitStatus submitQueryStatus, string mediatorCode, string pageMessage, UserMessageLevel userMessageLevel)
        {
            //Arrange
            string firstname = "First",
                lastname = "Last",
                companyname = "companyName",
                email = "valid@email.com",
                confirmEmail = "valid@email.com",
                userType = "Employer",
                title = "Mr",
                position = "Position",
                phoneNumber = "0987654321",
                mobile = "1234567890",
                address1 = "Line1",
                city = "City",
                postcode = "Postcode";
            var addressViewModelBuilder =
                new AddressViewModelBuilder().AddressLine1(address1).City(city).Postcode(postcode).Build();

            var accessRequestViewModel = new AccessRequestViewModelBuilder().Firstname(firstname).Lastname(lastname).MobileNumber(mobile)
                .PhoneNumber(phoneNumber).Position(position).UserType(userType).ConfirmEmail(confirmEmail).Title(title)
                .Email(email).Companyname(companyname)
                .Address(addressViewModelBuilder)
                .Build();

            var providerMock = new Mock<IAccessRequestProvider>();
            providerMock.Setup(x => x.SubmitAccessRequest(It.IsAny<AccessRequestViewModel>())).Returns(submitQueryStatus);

            Mock<IReferenceDataMediator> referenceDataMediator = new Mock<IReferenceDataMediator>();
            referenceDataMediator.Setup(c => c.GetReferenceData(It.IsAny<ReferenceDataTypes>()))
                .Returns(new MediatorResponse<ReferenceDataListViewModel>() { ViewModel = new ReferenceDataListViewModel() });

            var mediator = new AccessRequestMediatorBuilder().With(providerMock).With(referenceDataMediator).Build();

            //Act
            var response = mediator.SubmitAccessRequest(accessRequestViewModel);

            //Assert
            response.Code.Should().Be(mediatorCode);
            response.Message.Text.Should().Be(pageMessage);
            response.Message.Level.Should().Be(userMessageLevel);
        }

        [Test]
        public void SubmitAccessRequestValidationTests()
        {
            //Arrange

            //setup Empty addressViewModel 
            var addressViewModelBuilder =
                new AddressViewModelBuilder().Build();
            //setup empty AccessRequestViewModel
            var accessRequestViewModel = new AccessRequestViewModelBuilder()
                                                .Address(addressViewModelBuilder)
                                                .Build();

            Mock<IReferenceDataMediator> referenceDataMediator = new Mock<IReferenceDataMediator>();
            referenceDataMediator.Setup(c => c.GetReferenceData(It.IsAny<ReferenceDataTypes>()))
                .Returns(new MediatorResponse<ReferenceDataListViewModel>() { ViewModel = new ReferenceDataListViewModel() });
            
            var mediator = new AccessRequestMediatorBuilder().With(referenceDataMediator).Build();

            //Act
            var response = mediator.SubmitAccessRequest(accessRequestViewModel);

            //Assert
            response.Code.Should().Be(AccessRequestMediatorCodes.SubmitAccessRequest.ValidationError);
            response.ViewModel.Should().Be(accessRequestViewModel);
            response.ValidationResult.Should().NotBeNull();
        }
    }
}