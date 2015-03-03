﻿namespace SFA.Apprenticeships.Web.Employer.Tests.Providers
{
    using System;
    using Application.Interfaces.Communications;
    using Builders;
    using Domain.Entities;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using ViewModels;

    [TestFixture]
    public class EmployerEnquiryTests
    {   
        [Test]
        public void GivenError_ThenErrorStatusIsReturned()
        {
            //Arrange
            var viewModel = new EmployerEnquiryViewModelBuilder().Build();

            Mock<ICommunciationService> serviceMock = new Mock<ICommunciationService>();
            serviceMock.Setup(cs => cs.SubmitEnquiry(It.IsAny<EmployerEnquiry>())).Throws(new Exception());
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
            serviceMock.Setup(cs => cs.SubmitEnquiry(It.IsAny<EmployerEnquiry>()));
            var provider = new EmployerEnquiryProviderBuilder().With(serviceMock).Build();

            //Act
            var result = provider.SubmitEnquiry(viewModel);

            //Assert
            result.Should().Be(SubmitQueryStatus.Success);
        }
    }
}