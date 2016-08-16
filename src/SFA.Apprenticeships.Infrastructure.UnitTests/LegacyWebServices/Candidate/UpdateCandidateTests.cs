namespace SFA.Apprenticeships.Infrastructure.UnitTests.LegacyWebServices.Candidate
{
    using System;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Infrastructure.LegacyWebServices.GatewayServiceProxy;
    using Moq;
    using Candidate = Domain.Entities.Candidates.Candidate;

    [TestFixture]
    [Parallelizable]
    public class UpdateCandidateTests
    {
        [Test]
        public void SuccessTest()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            gatewayServiceContract.Setup(c => c.UpdateCandidate(It.IsAny<UpdateCandidateRequest>())).Returns(new UpdateCandidateResponse(null));
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            Action updateCandidateAction = () => provider.UpdateCandidate(candidate);

            updateCandidateAction.ShouldNotThrow();
        }

        [Test]
        public void DomainExceptionTest()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            gatewayServiceContract.Setup(c => c.UpdateCandidate(It.IsAny<UpdateCandidateRequest>())).Returns(new UpdateCandidateResponse(new[] { new ValidationError { ErrorCode = "000", KeyData = "Error", Message = "An error has occurred" } }));
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            Action updateCandidateAction = () => { provider.UpdateCandidate(candidate); };

            var exception = updateCandidateAction.ShouldThrow<DomainException>().Which;
            exception.Code.Should().Be(Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.UpdateCandidateFailed);
            exception.Data["message"].Should().Be("1 unexpected validation error(s): {\"ValidationErrors\":[{\"ErrorCode\":\"000\",\"KeyData\":\"Error\",\"Message\":\"An error has occurred\"}]}");
            exception.Data["candidateId"].Should().Be(candidate.EntityId);
        }

        [Test]
        public void DomainExceptionTestNullResponse()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            gatewayServiceContract.Setup(c => c.UpdateCandidate(It.IsAny<UpdateCandidateRequest>())).Returns((UpdateCandidateResponse) null);
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            Action updateCandidateAction = () => { provider.UpdateCandidate(candidate); };

            var exception = updateCandidateAction.ShouldThrow<DomainException>().Which;
            exception.Code.Should().Be(Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.UpdateCandidateFailed);
            exception.Data["message"].Should().Be("No response");
            exception.Data["candidateId"].Should().Be(candidate.EntityId);
        }

        [Test]
        public void BoundaryExceptionExceptionTest()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            gatewayServiceContract.Setup(c => c.UpdateCandidate(It.IsAny<UpdateCandidateRequest>())).Throws(new BoundaryException(Infrastructure.LegacyWebServices.ErrorCodes.WebServiceFailed));
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            Action updateCandidateAction = () => { provider.UpdateCandidate(candidate); };

            var exception = updateCandidateAction.ShouldThrow<DomainException>().Which;
            exception.Code.Should().Be(Apprenticeships.Application.Interfaces.Candidates.ErrorCodes.UpdateCandidateFailed);
            exception.InnerException.Should().BeOfType<BoundaryException>();
        }

        [Test]
        public void ExceptionExceptionTest()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            gatewayServiceContract.Setup(c => c.UpdateCandidate(It.IsAny<UpdateCandidateRequest>())).Throws(new Exception());
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            Action updateCandidateAction = () => { provider.UpdateCandidate(candidate); };

            updateCandidateAction.ShouldThrow<Exception>();
        }

        [Test]
        public void MappingTest()
        {
            var gatewayServiceContract = new Mock<GatewayServiceContract>();
            UpdateCandidateRequest request = null;
            gatewayServiceContract.Setup(c => c.UpdateCandidate(It.IsAny<UpdateCandidateRequest>())).Callback<UpdateCandidateRequest>(r => { request = r; }).Returns(new UpdateCandidateResponse(null));
            var service = new MockGatewayService(gatewayServiceContract.Object);
            var provider = new LegacyCandidateProviderBuilder().With(service).Build();

            var candidate = new Fixture().Build<Candidate>().Create();
            provider.UpdateCandidate(candidate);

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
    }
}