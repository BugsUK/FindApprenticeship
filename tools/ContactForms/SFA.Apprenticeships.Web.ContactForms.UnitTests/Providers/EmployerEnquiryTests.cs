using SFA.Apprenticeships.Domain.Entities;
using SFA.Apprenticeships.Web.ContactForms.Mappers.Interfaces;

namespace SFA.Apprenticeships.Web.ContactForms.Tests.Providers
{
    using Application.Interfaces.Communications;
    using Builders;
    using ViewModels;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using System.Collections;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class EmployerEnquiryTests
    {
        [Test]
        public void GivenError_ThenErrorStatusIsReturned()
        {
            //Arrange
            var viewModel = new EmployerEnquiryViewModelBuilder().Build();

            Mock<ICommunciationService> serviceMock = new Mock<ICommunciationService>();
            //todo: fix this : 
            serviceMock.Setup(cs => cs.SendMessage(It.IsAny<MessageTypes>(), It.IsAny<IEnumerable<CommunicationToken>>())).Throws(new Exception());
            var provider = new EmployerEnquiryProviderBuilder().With(serviceMock).Build();

            //Act
            var result = provider.SubmitEnquiry(viewModel);

            //Assert
            result.Should().Be(SubmitQueryStatus.Error);
        }

        [Test]
        public void GivenSuccess_ThenSuccessStatusIsReturned()
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

            var viewModel = new EmployerEnquiryViewModelBuilder().Firstname(firstname).Lastname(lastname).MobileNumber(mobile)
                .PhoneNumber(phoneNumber).Position(position).PrevExp(prevExp).Title(title)
                .WorkSector(workSector)
                .EnquirySource(enquirySource).EnquiryDescription(enquiryDescription).EmployeeCount(employeeCount)
                .Email(email).Companyname(companyname)
                .Address(addressViewModelBuilder)
                .Build();

            Mock<ICommunciationService> serviceMock = new Mock<ICommunciationService>();
            serviceMock.Setup(cs => cs.SendMessage(It.IsAny<MessageTypes>(), It.IsAny<IEnumerable<CommunicationToken>>()));
            Mock<IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry>> employerEnquiryVtoDMapper = new Mock<IViewModelToDomainMapper<EmployerEnquiryViewModel, EmployerEnquiry>>();

            employerEnquiryVtoDMapper.Setup(cs => cs.ConvertToDomain(It.IsAny<EmployerEnquiryViewModel>()))
                .Returns(new EmployerEnquiry() { Address = new Address() });

            var provider = new EmployerEnquiryProviderBuilder().With(serviceMock).With(employerEnquiryVtoDMapper).Build();

            //Act
            var result = provider.SubmitEnquiry(viewModel);

            //Assert
            result.Should().Be(SubmitQueryStatus.Success);
        }
    }
}