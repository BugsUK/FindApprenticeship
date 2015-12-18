namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.CandidateServiceProvider
{
    using System;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Candidates;
    using Candidate.Providers;
    using Candidate.ViewModels.Home;
    using Domain.Entities.Communication;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SendContactMessageTests
    {
        private Mock<ICandidateService> _candidateServiceMock;
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        [SetUp]
        public void SetUp()
        {
            _candidateServiceMock = new Mock<ICandidateService>();
        }

        [Test]
        public void SendMessageOk()
        {
            var candidateId = Guid.NewGuid();

            var candidateServiceProvider = new CandidateServiceProvider(_candidateServiceMock.Object, null, null, null, _mapperMock.Object, null, null);
            _mapperMock.Setup(m => m.Map<ContactMessageViewModel, ContactMessage>(It.IsAny<ContactMessageViewModel>()))
                .Returns(new ContactMessage());

            var result = candidateServiceProvider.SendContactMessage(candidateId, new ContactMessageViewModel());

            result.Should().BeTrue();
            _candidateServiceMock.Verify(cs => cs.SubmitContactMessage(It.Is<ContactMessage>(cm => cm.UserId == candidateId)));
        }

        [Test]
        public void SendMessageFail()
        {
            var candidateId = Guid.NewGuid();

            var candidateServiceProvider = new CandidateServiceProvider(_candidateServiceMock.Object, null, null, null, _mapperMock.Object, null, null);
            _mapperMock.Setup(m => m.Map<ContactMessageViewModel, ContactMessage>(It.IsAny<ContactMessageViewModel>()))
                .Returns(new ContactMessage());

            _candidateServiceMock.Setup(cs => cs.SubmitContactMessage(It.IsAny<ContactMessage>()))
                .Throws<ArgumentException>();

            var result = candidateServiceProvider.SendContactMessage(candidateId, new ContactMessageViewModel());

            result.Should().BeFalse();
        }
    }
}