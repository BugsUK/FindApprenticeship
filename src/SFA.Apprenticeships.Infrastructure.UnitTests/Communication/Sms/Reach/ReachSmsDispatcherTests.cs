namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Sms.Reach
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Application.Interfaces.Communications;
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Exceptions;
    using FluentAssertions;
    using Infrastructure.Communication.Configuration;
    using Infrastructure.Communication.Sms;
    using Infrastructure.Communication.Sms.SmsMessageFormatters;
    using Moq;
    using NUnit.Framework;
    using RestSharp;

    using SFA.Apprenticeships.Application.Interfaces;

    using SmsMessageFormatters;
    using ErrorCodes = Application.Interfaces.Communications.ErrorCodes;

    [TestFixture]
    [Parallelizable]
    public class ReachSmsDispatcherTests
    {
        private static class RestResponses
        {
            public const string Valid = "<?xml version=\"1.0\" encoding=\"utf-8\"?><HttpAPI xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MessageId>8b70347c-dfa8-4943-814a-34db43a1a4b9</MessageId><MessageResult>Success</MessageResult></HttpAPI>";
            public const string MissingMessageId = "<?xml version=\"1.0\" encoding=\"utf-8\"?><HttpAPI xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MessageResult>Success</MessageResult></HttpAPI>";
            public const string InvalidMessageId = "<?xml version=\"1.0\" encoding=\"utf-8\"?><HttpAPI xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MessageId>X</MessageId><MessageResult>Success</MessageResult></HttpAPI>";
            public const string MissingMessageResult = "<?xml version=\"1.0\" encoding=\"utf-8\"?><HttpAPI xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MessageId>8b70347c-dfa8-4943-814a-34db43a1a4b9</MessageId></HttpAPI>";
            public const string UnsuccessfulMessageResult = "<?xml version=\"1.0\" encoding=\"utf-8\"?><HttpAPI xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"><MessageId>8b70347c-dfa8-4943-814a-34db43a1a4b9</MessageId><MessageResult>Failed</MessageResult></HttpAPI>";
            public const string InvalidXml = "X";
            public const string Empty = "";
        }

        private Mock<ILogService> _logService;
        private Mock<IConfigurationService> _configurationService;
        private ISmsNumberFormatter _smsNumberFormatter;
        private SmsTemplate[] _smsTemplateConfigurations;
        private IEnumerable<KeyValuePair<MessageTypes, SmsMessageFormatter>> _smsMessageFormatters;
        private IEnumerable<CommunicationToken> _communicationTokens;
        private SmsRequest _smsRequest;

        [SetUp]
        public void SetUp()
        {
            _logService = new Mock<ILogService>();

            // Templates.
            var smsTemplateConfiguration = new SmsTemplate { Name = "MessageTypes.SendAccountUnlockCode", Message = "Your account has been locked due to suspicious activity. To unlock it please enter the following code: {0}." };


            _smsTemplateConfigurations = new[] { smsTemplateConfiguration };

            var configService = new Mock<IConfigurationService>();
            configService.Setup(x => x.Get<SmsConfiguration>())
                .Returns(new SmsConfiguration() { Templates = _smsTemplateConfigurations });

            _smsMessageFormatters = new[]
            {
                new KeyValuePair<MessageTypes, SmsMessageFormatter>(MessageTypes.SendAccountUnlockCode,
                    new SmsAccountUnlockCodeMessageFormatter(configService.Object))
            };

            var smsConfig = new SmsConfiguration()
            {
                Templates = _smsTemplateConfigurations,
                Url = "https://www.example.com"
            };

            _configurationService = new Mock<IConfigurationService>();
            _configurationService.Setup(x => x.Get<SmsConfiguration>()).Returns(smsConfig);

            _smsNumberFormatter = new ReachSmsNumberFormatter();

            // Default request.
            _communicationTokens = new[]
            {
                new CommunicationToken(CommunicationTokens.AccountUnlockCode, "XYZ789")
            };

            _smsRequest = new SmsRequestBuilder()
                .WithMessageType(MessageTypes.SendAccountUnlockCode)
                .WithTokens(_communicationTokens)
                .Build();
        }

        [Test]
        public void ShouldSendSms()
        {
            // Arrange.
            var restClient = new Mock<IRestClient>();
            
            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = RestResponses.Valid
            };

            restClient.Setup(mock => mock.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            var dispatcher = new ReachSmsDispatcher(
                _logService.Object, restClient.Object, _configurationService.Object, _smsNumberFormatter, _smsMessageFormatters);

            // Act.
            dispatcher.SendSms(_smsRequest);

            // Assert.
            restClient.Verify(mock => mock.Execute(It.IsAny<IRestRequest>()), Times.Once);
            _logService.Verify(mock => mock.Info(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        [Test]
        public void ShouldThrowWhenRestResponseIsNot200Ok()
        {
            // Arrange.
            var restClient = new Mock<IRestClient>();

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = RestResponses.Valid
            };

            restClient.Setup(mock => mock.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            var dispatcher = new ReachSmsDispatcher(
                _logService.Object, restClient.Object, _configurationService.Object, _smsNumberFormatter, _smsMessageFormatters);

            // Act.
            Action action = () => dispatcher.SendSms(_smsRequest);

            // Assert.
            action.ShouldThrowExactly<DomainException>();
        }

        [Test]
        public void ShouldThrowWhenRestResponseContentIsEmpty()
        {
            // Arrange.
            var restClient = new Mock<IRestClient>();

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = RestResponses.Empty
            };

            restClient.Setup(mock => mock.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            var dispatcher = new ReachSmsDispatcher(
                _logService.Object, restClient.Object, _configurationService.Object, _smsNumberFormatter, _smsMessageFormatters);

            // Act.
            Action action = () => dispatcher.SendSms(_smsRequest);

            // Assert.
            action.ShouldThrowExactly<DomainException>();
        }

        [Test]
        public void ShouldThrowWhenRestResponseContentIsInvalidXml()
        {
            // Arrange.
            var restClient = new Mock<IRestClient>();

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = RestResponses.InvalidXml
            };

            restClient.Setup(mock => mock.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            var dispatcher = new ReachSmsDispatcher(
                _logService.Object, restClient.Object, _configurationService.Object, _smsNumberFormatter, _smsMessageFormatters);

            // Act.
            Action action = () => dispatcher.SendSms(_smsRequest);

            // Assert.
            action.ShouldThrowExactly<DomainException>();
        }

        [Test]
        public void ShouldThrowWhenRestResponseMessageIdIsNotAGuid()
        {
            // Arrange.
            var restClient = new Mock<IRestClient>();

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = RestResponses.InvalidMessageId
            };

            restClient.Setup(mock => mock.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            var dispatcher = new ReachSmsDispatcher(
                _logService.Object, restClient.Object, _configurationService.Object, _smsNumberFormatter, _smsMessageFormatters);

            // Act.
            Action action = () => dispatcher.SendSms(_smsRequest);

            // Assert.
            action.ShouldThrowExactly<DomainException>();
        }

        [Test]
        public void ShouldThrowWhenRestResponseMessageIdIsMissing()
        {
            // Arrange.
            var restClient = new Mock<IRestClient>();

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = RestResponses.MissingMessageId
            };

            restClient.Setup(mock => mock.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            var dispatcher = new ReachSmsDispatcher(
                _logService.Object, restClient.Object, _configurationService.Object, _smsNumberFormatter, _smsMessageFormatters);

            // Act.
            Action action = () => dispatcher.SendSms(_smsRequest);

            // Assert.
            action.ShouldThrowExactly<DomainException>();
        }

        [Test]
        public void ShouldThrowWhenRestResponseMessageResultIsMissing()
        {
            // Arrange.
            var restClient = new Mock<IRestClient>();

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = RestResponses.MissingMessageResult
            };

            restClient.Setup(mock => mock.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            var dispatcher = new ReachSmsDispatcher(
                _logService.Object, restClient.Object, _configurationService.Object, _smsNumberFormatter, _smsMessageFormatters);

            // Act.
            Action action = () => dispatcher.SendSms(_smsRequest);

            // Assert.
            var exception = action.ShouldThrowExactly<DomainException>();

            exception.WithMessage(ErrorCodes.SmsError);
            exception.WithInnerException<BoundaryException>();

            exception.Where(e => e.InnerException.Data.Contains("content"));
            exception.Where(e => e.Data.Contains("toNumber"));
            exception.Where(e => e.Data.Contains("messageType"));
        }

        [Test]
        public void ShouldThrowWhenRestResponseMessageResultIsUnsuccessful()
        {
            // Arrange.
            var restClient = new Mock<IRestClient>();

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = RestResponses.UnsuccessfulMessageResult
            };

            restClient.Setup(mock => mock.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            var dispatcher = new ReachSmsDispatcher(
                _logService.Object, restClient.Object, _configurationService.Object, _smsNumberFormatter, _smsMessageFormatters);

            // Act.
            Action action = () => dispatcher.SendSms(_smsRequest);

            // Assert.
            action.ShouldThrowExactly<DomainException>();
        }

        [Test]
        public void ShouldThrowWhenToNumberIsMissingOrInvalid()
        {
            // Arrange.
            var restClient = new Mock<IRestClient>();

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = RestResponses.Valid
            };

            restClient.Setup(mock => mock.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            var dispatcher = new ReachSmsDispatcher(
                _logService.Object, restClient.Object, _configurationService.Object, _smsNumberFormatter, _smsMessageFormatters);

            _smsRequest = new SmsRequestBuilder()
                .WithMessageType(MessageTypes.SendAccountUnlockCode)
                .WithTokens(_communicationTokens)
                .WithToNumber(null)
                .Build();

            // Act.
            Action action = () => dispatcher.SendSms(_smsRequest);

            // Assert.
            var exception = action.ShouldThrowExactly<DomainException>();

            exception.WithMessage(ErrorCodes.SmsError);
            exception.WithInnerException<ArgumentNullException>();
        }
    }
}