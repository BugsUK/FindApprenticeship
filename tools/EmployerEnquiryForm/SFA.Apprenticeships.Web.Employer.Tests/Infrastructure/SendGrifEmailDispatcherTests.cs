namespace SFA.Apprenticeships.Web.Employer.Tests.Infrastructure
{
    using System;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Apprenticeships.Infrastructure.Communication.Email;
    using Domain.Entities;
    using Moq;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class SendGridEmailDispatcherTests
    {
        private IEmailDispatcher _dispatcher;

        private IEmailDispatcher _voidEmailDispatcher;

        private readonly Mock<ILogService> _logServiceMock = new Mock<ILogService>();

        private const string TestToEmail = "helpdesk.sfa@gmail.com";

        [SetUp]
        public void SetUp()
        {
            var container = new Container(x =>
            {
                x.For<IEmailDispatcher>().Use<SendGridEmailDispatcher>().Name = "SendGridEmailDispatcher";
                x.For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";
            });

            container.Configure(c => c.For<ILogService>().Use(_logServiceMock.Object));

            _dispatcher = container.GetInstance<IEmailDispatcher>("SendGridEmailDispatcher");
            _voidEmailDispatcher = container.GetInstance<IEmailDispatcher>("VoidEmailDispatcher");
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShoudConstructSendGridEmailDispatcher()
        {
            Assert.IsNotNull(_dispatcher);
            Assert.IsInstanceOf<SendGridEmailDispatcher>(_dispatcher);
        }

        [Test, Category("Integration"), Category("SmokeTests")]
        public void ShouldConstructVoidEmailDispatcher()
        {
            Assert.IsNotNull(_voidEmailDispatcher);
            Assert.IsInstanceOf<VoidEmailDispatcher>(_voidEmailDispatcher);
        }
        
        [Test, Category("Integration")]
        public void ShoudSendEmail()
        {
            //Unable to test due to SendGrid username/pwd stored in azure so for time being just void email dispatcher
            var request = new EmailRequest
            {
                ToEmail = TestToEmail,
                EmailContent= EmailContentGenerator.CreateContactForm()
            };

            _voidEmailDispatcher.SendEmail(request);
            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }
    }
}