namespace SFA.Apprenticeships.Infrastructure.UnitTests.LegacyWebServices.Candidate
{
    using System;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Infrastructure.LegacyWebServices.GatewayServiceProxy;
    using Moq;
    using Candidate = Domain.Entities.Candidates.Candidate;

    [TestFixture]
    [Parallelizable]
    public class CreateCandidateTests
    {
        [Test]
        public void SuccessTest()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            const int legacyCandidateId = 123456;
            gatewayServiceContract.Setup(c => c.CreateCandidate(It.IsAny<CreateCandidateRequest>())).Returns(new CreateCandidateResponse(legacyCandidateId, null));
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            var result = provider.CreateCandidate(candidate);
            result.Should().Be(legacyCandidateId);
        }

        [Test]
        public void DomainExceptionTest()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            gatewayServiceContract.Setup(c => c.CreateCandidate(It.IsAny<CreateCandidateRequest>())).Returns(new CreateCandidateResponse(0, new []{new ValidationError{ErrorCode = "000", KeyData = "Error", Message = "An error has occurred"}}));
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            Action createCandidateAction = () => provider.CreateCandidate(candidate);

            var exception = createCandidateAction.ShouldThrow<DomainException>().Which;
            exception.Code.Should().Be(Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.CreateCandidateFailed);
            exception.Data["message"].Should().Be("1 unexpected validation error(s): {\"CandidateId\":0,\"ValidationErrors\":[{\"ErrorCode\":\"000\",\"KeyData\":\"Error\",\"Message\":\"An error has occurred\"}]}");
            exception.Data["candidateId"].Should().Be(candidate.EntityId);
        }

        [Test]
        public void DomainExceptionTestNullResponse()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            gatewayServiceContract.Setup(c => c.CreateCandidate(It.IsAny<CreateCandidateRequest>())).Returns((CreateCandidateResponse) null);
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            Action createCandidateAction = () => provider.CreateCandidate(candidate);

            var exception = createCandidateAction.ShouldThrow<DomainException>().Which;
            exception.Code.Should().Be(Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.CreateCandidateFailed);
            exception.Data["message"].Should().Be("No response");
            exception.Data["candidateId"].Should().Be(candidate.EntityId);
        }

        [Test]
        public void BoundaryExceptionExceptionTest()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            gatewayServiceContract.Setup(c => c.CreateCandidate(It.IsAny<CreateCandidateRequest>())).Throws(new BoundaryException(Infrastructure.LegacyWebServices.ErrorCodes.WebServiceFailed));
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            Action createCandidateAction = () => provider.CreateCandidate(candidate);

            var exception = createCandidateAction.ShouldThrow<DomainException>().Which;
            exception.Code.Should().Be(Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.CreateCandidateFailed);
            exception.InnerException.Should().BeOfType<BoundaryException>();
        }

        [Test]
        public void ExceptionExceptionTest()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            gatewayServiceContract.Setup(c => c.CreateCandidate(It.IsAny<CreateCandidateRequest>())).Throws(new Exception());
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            Action createCandidateAction = () => provider.CreateCandidate(candidate);

            createCandidateAction.ShouldThrow<Exception>();
        }

        [Test]
        public void BasicMappingTest()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            CreateCandidateRequest request = null;
            gatewayServiceContract.Setup(c => c.CreateCandidate(It.IsAny<CreateCandidateRequest>())).Callback<CreateCandidateRequest>(r => { request = r; }).Returns(new CreateCandidateResponse(123456, null));
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            provider.CreateCandidate(candidate);

            request.Should().NotBeNull();
            request.Candidate.EmailAddress.Should().Be(candidate.RegistrationDetails.EmailAddress);
            request.Candidate.FirstName.Should().Be(candidate.RegistrationDetails.FirstName);
            request.Candidate.MiddleName.Should().Be(candidate.RegistrationDetails.MiddleNames);
            request.Candidate.Surname.Should().Be(candidate.RegistrationDetails.LastName);
            request.Candidate.DateOfBirth.Should().Be(candidate.RegistrationDetails.DateOfBirth.Date);
            request.Candidate.AddressLine1.Should().Be(candidate.RegistrationDetails.Address.AddressLine1);
            request.Candidate.AddressLine2.Should().Be(candidate.RegistrationDetails.Address.AddressLine2);
            request.Candidate.AddressLine3.Should().Be(candidate.RegistrationDetails.Address.AddressLine3);
            request.Candidate.AddressLine4.Should().Be(candidate.RegistrationDetails.Address.AddressLine4);
            request.Candidate.TownCity.Should().Be("N/A");
            request.Candidate.Postcode.Should().Be(candidate.RegistrationDetails.Address.Postcode);
            request.Candidate.LandlineTelephone.Should().Be(candidate.RegistrationDetails.PhoneNumber);
            request.Candidate.MobileTelephone.Should().Be(string.Empty);
        }

        [TestCase(null, null)]
        [TestCase(99, 20)]
        [TestCase(31, 16)]
        [TestCase(32, 17)]
        [TestCase(33, 20)]
        [TestCase(34, 18)]
        [TestCase(35, 13)]
        [TestCase(36, 12)]
        [TestCase(37, 11)]
        [TestCase(38, 14)]
        [TestCase(39, 3)]
        [TestCase(40, 4)]
        [TestCase(41, 2)]
        [TestCase(42, 19)]
        [TestCase(43, 5)]
        [TestCase(44, 7)]
        [TestCase(45, 8)]
        [TestCase(46, 9)]
        [TestCase(47, 20)]
        [TestCase(98, 20)]
        public void EthnicityMappingTest(int? sourceEthnicity, int? expectedEthnicity)
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            CreateCandidateRequest request = null;

            gatewayServiceContract.Setup(c => c.CreateCandidate(It.IsAny<CreateCandidateRequest>()))
                .Callback<CreateCandidateRequest>(r => { request = r; })
                .Returns(new CreateCandidateResponse(123456, null));

            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate =
                new Fixture().Build<Candidate>()
                    .With(c => c.MonitoringInformation, new MonitoringInformation
                    {
                        Ethnicity = sourceEthnicity
                    }).Create();

            provider.CreateCandidate(candidate);

            request.Should().NotBeNull();
            request.Candidate.Should().NotBeNull();

            request.Candidate.EthnicOrigin.Should().Be(expectedEthnicity);
            request.Candidate.EthnicOriginSpecified.Should().Be(expectedEthnicity.HasValue);
        }

        [TestCase(null, null)]
        [TestCase(31, null)]
        [TestCase(99, "Not provided")]
        [TestCase(33, "Gypsy or Irish Traveller")]
        [TestCase(47, "Arab")]
        public void EthnicityOtherMappingTest(int? sourceEthnicity, string expectedEthnicityOther)
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            CreateCandidateRequest request = null;

            gatewayServiceContract.Setup(c => c.CreateCandidate(It.IsAny<CreateCandidateRequest>()))
                .Callback<CreateCandidateRequest>(r => { request = r; })
                .Returns(new CreateCandidateResponse(123456, null));

            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate =
                new Fixture().Build<Candidate>()
                    .With(c => c.MonitoringInformation, new MonitoringInformation
                    {
                        Ethnicity = sourceEthnicity
                    }).Create();

            provider.CreateCandidate(candidate);

            request.Should().NotBeNull();
            request.Candidate.Should().NotBeNull();

            request.Candidate.EthnicOrginOther.Should().Be(expectedEthnicityOther);
        }

        [TestCase(null, null)]
        [TestCase(Gender.Male, 1)]
        [TestCase(Gender.Female, 2)]
        [TestCase(Gender.Other, 3)]
        [TestCase(Gender.PreferNotToSay, 4)]
        public void GenderMappingTest(Gender? sourceGender, int? expectedGender)
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            CreateCandidateRequest request = null;

            gatewayServiceContract.Setup(c => c.CreateCandidate(It.IsAny<CreateCandidateRequest>()))
                .Callback<CreateCandidateRequest>(r => { request = r; })
                .Returns(new CreateCandidateResponse(123456, null));

            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate =
                new Fixture().Build<Candidate>()
                    .With(c => c.MonitoringInformation, new MonitoringInformation
                    {
                        Gender = sourceGender
                    }).Create();

            provider.CreateCandidate(candidate);

            request.Should().NotBeNull();
            request.Candidate.Should().NotBeNull();

            request.Candidate.Gender.Should().Be(expectedGender);
            request.Candidate.GenderSpecified.Should().Be(expectedGender.HasValue);
        }

        [TestCase(null, null)]
        [TestCase(DisabilityStatus.No, 0)]
        [TestCase(DisabilityStatus.Yes, 13)]
        [TestCase(DisabilityStatus.PreferNotToSay, 14)]
        public void DisabilityMappingTest(DisabilityStatus? sourceDisabilityStatus, int? expectedDisabilityStatus)
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            CreateCandidateRequest request = null;

            gatewayServiceContract.Setup(c => c.CreateCandidate(It.IsAny<CreateCandidateRequest>()))
                .Callback<CreateCandidateRequest>(r => { request = r; })
                .Returns(new CreateCandidateResponse(123456, null));

            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate =
                new Fixture().Build<Candidate>()
                    .With(c => c.MonitoringInformation, new MonitoringInformation
                    {
                        DisabilityStatus = sourceDisabilityStatus
                    }).Create();

            provider.CreateCandidate(candidate);

            request.Should().NotBeNull();
            request.Candidate.Should().NotBeNull();

            request.Candidate.Disability.Should().Be(expectedDisabilityStatus);
            request.Candidate.DisabilitySpecified.Should().Be(expectedDisabilityStatus.HasValue);
        }
    }
}