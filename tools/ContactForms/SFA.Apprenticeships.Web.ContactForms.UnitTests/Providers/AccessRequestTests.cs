
namespace SFA.Apprenticeships.Web.ContactForms.Tests.Providers
{
    using Domain.Entities;
    using Mappers.Interfaces;
    using Application.Interfaces.Communications;
    using Builders;
    using ViewModels;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class AccessRequestTests
    {
        [Test]
        public void GivenError_ThenErrorStatusIsReturned()
        {
            //Arrange
            var viewModel = new AccessRequestViewModelBuilder().Build();

            Mock<ICommunciationService> serviceMock = new Mock<ICommunciationService>();
            //todo: fix this : 
            serviceMock.Setup(cs => cs.SendMessage(It.IsAny<MessageTypes>(), It.IsAny<IEnumerable<CommunicationToken>>())).Throws(new Exception());
            var provider = new AccessRequestProviderBuilder().With(serviceMock).Build();

            //Act
            var result = provider.SubmitAccessRequest(viewModel);

            //Assert
            result.Should().Be(AccessRequestSubmitStatus.Error);
        }

        [Test]
        public void GivenSuccess_ThenSuccessStatusIsReturned()
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

            Mock<ICommunciationService> serviceMock = new Mock<ICommunciationService>();
            serviceMock.Setup(cs => cs.SendMessage(It.IsAny<MessageTypes>(), It.IsAny<IEnumerable<CommunicationToken>>()));
            Mock<IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest>> vtoDMapper = new Mock<IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest>>();

            vtoDMapper.Setup(cs => cs.ConvertToDomain(It.IsAny<AccessRequestViewModel>()))
                .Returns(new AccessRequest() { Address = new Address() });

            var provider = new AccessRequestProviderBuilder().With(serviceMock).With(vtoDMapper).Build();

            //Act
            var result = provider.SubmitAccessRequest(accessRequestViewModel);

            //Assert
            result.Should().Be(AccessRequestSubmitStatus.Success);
        }
    }
}