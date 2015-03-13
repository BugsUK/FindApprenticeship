namespace SFA.Apprenticeships.Web.ContactForms.Tests.Mediators
{
    using Builders;
    using Constants;
    using Constants.Pages;
    using ContactForms.Mediators.EmployerEnquiry;
    using ContactForms.Providers.Interfaces;
    using ViewModels;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    public class EmployerEnquiryTests
    {
        [TestCase(SubmitQueryStatus.Success, EmployerEnquiryMediatorCodes.SubmitEnquiry.Success, EmployerEnquiryPageMessages.QueryHasBeenSubmittedSuccessfully, UserMessageLevel.Success)]
        [TestCase(SubmitQueryStatus.Error, EmployerEnquiryMediatorCodes.SubmitEnquiry.Error, EmployerEnquiryPageMessages.ErrorWhileQuerySubmission, UserMessageLevel.Error)]
        public void SubmitEnquiryTests(SubmitQueryStatus submitQueryStatus, string mediatorCode, string pageMessage, UserMessageLevel userMessageLevel)
        {
            //Arrange
            string firstname = "First",
                lastname = "Last",
                companyname = "companyName",
                email = "valid@email.com",
                employeeCount = "100",
                enquiryDescription = "test query",
                enquirySource = "Telephone Call",
                title = "Mr",
                prevExp = "Yes",
                position = "Position",
                workSector = "retail",
                phoneNumber = "0987654321",
                mobile = "1234567890",
                address1 = "Line1",
                city = "City",
                postcode = "Postcode";
            var addressViewModelBuilder =
                new AddressViewModelBuilder().AddressLine1(address1).City(city).Postcode(postcode).Build();

            var employerEnquiryViewModel = new EmployerEnquiryViewModelBuilder().Firstname(firstname).Lastname(lastname).MobileNumber(mobile)
                .PhoneNumber(phoneNumber).Position(position).PrevExp(prevExp).Title(title)
                .WorkSector(workSector)
                .EnquirySource(enquirySource).EnquiryDescription(enquiryDescription).EmployeeCount(employeeCount)
                .Email(email).Companyname(companyname)
                .Address(addressViewModelBuilder)
                .Build();

            var employerProviderMock = new Mock<IEmployerEnquiryProvider>();
            employerProviderMock.Setup(x => x.SubmitEnquiry(It.IsAny<EmployerEnquiryViewModel>())).Returns(submitQueryStatus);

            var mediator = new EmployerEnquiryMediatorBuilder().With(employerProviderMock).Build();

            //Act
            var response = mediator.SubmitEnquiry(employerEnquiryViewModel);

            //Assert
            response.Code.Should().Be(mediatorCode);
            response.Message.Text.Should().Be(pageMessage);
            response.Message.Level.Should().Be(userMessageLevel);
        }

        [Test]
        public void SubmitEnquiryValidationTests()
        {
            //Arrange

            //setup Empty addressViewModel 
            var addressViewModelBuilder =
                new AddressViewModelBuilder().Build();
            //setup empty employerEnquiryViewModel
            var employerEnquiryViewModel = new EmployerEnquiryViewModelBuilder()
                                                .Address(addressViewModelBuilder)
                                                .Build();

            var mediator = new EmployerEnquiryMediatorBuilder().Build();

            //Act
            var response = mediator.SubmitEnquiry(employerEnquiryViewModel);

            //Assert
            response.Code.Should().Be(EmployerEnquiryMediatorCodes.SubmitEnquiry.ValidationError);
            response.ViewModel.Should().Be(employerEnquiryViewModel);
            response.ValidationResult.Should().NotBeNull();
        }
    }
}