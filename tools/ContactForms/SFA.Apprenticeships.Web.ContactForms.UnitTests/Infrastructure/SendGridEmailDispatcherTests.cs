using SFA.Apprenticeships.Common.Configuration;

namespace SFA.Apprenticeships.Web.ContactForms.Tests.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Apprenticeships.Infrastructure.Communication.Email;
    using Moq;
    using NUnit.Framework;
    using StructureMap;

    [TestFixture]
    public class SendGridEmailDispatcherTests
    {
        private IEmailDispatcher _dispatcher;

        private IEmailDispatcher _voidEmailDispatcher;

        private readonly Mock<ILogService> _logServiceMock = new Mock<ILogService>();
        private readonly Mock<IXmlGenerator> _xmlGeneratorMock = new Mock<IXmlGenerator>();

        private const string TestToEmail = "helpdesk.sfa@gmail.com";

        [SetUp]
        public void SetUp()
        {
            IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>> emailMessageFormatters = new[]
            {
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.EmployerEnquiry, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.EmployerEnquiryConfirmation, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.GlaEmployerEnquiry, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.GlaEmployerEnquiryConfirmation, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.WebAccessRequest, new EmailSimpleMessageFormatter()),
                new KeyValuePair<MessageTypes, EmailMessageFormatter>(MessageTypes.WebAccessRequestConfirmation, new EmailSimpleMessageFormatter())
            };

            var container = new Container(x =>
            {
                x.For<IConfigurationManager>().Singleton().Use<ConfigurationManager>();
                
                x.For<IEmailDispatcher>().Use<SendGridEmailDispatcher>().Named("SendGridEmailDispatcher").Ctor<IEnumerable<KeyValuePair<MessageTypes, EmailMessageFormatter>>>()
                                                                                .Is(emailMessageFormatters);
                x.For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";
                x.For<SendGridConfiguration>().Singleton().Use(SendGridConfiguration.Instance); 
                x.For<IEmailDispatcher>().Use<VoidEmailDispatcher>().Name = "VoidEmailDispatcher";
            });

            container.Configure(c => c.For<ILogService>().Use(_logServiceMock.Object));
            container.Configure(c => c.For<IXmlGenerator>().Use(_xmlGeneratorMock.Object));

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
                //EmailContent= EmailContentGenerator.CreateContactForm()
            };

            _voidEmailDispatcher.SendEmail(request);
            _logServiceMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
        }
    }
}