namespace SFA.Apprenticeships.Infrastructure.FAAUnitTests.Processes
{
    using System;
    using Application.Interfaces.Communications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Messaging;
    using Builders;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class EmailRequestSubscriberTests
    {
        [Test]
        public void NoExceptionShouldReturnComplete()
        {
            //Arrange
            var dispatcher = new Mock<IEmailDispatcher>();
            var subscriber = new EmailRequestSubscriberBuilder().With(dispatcher).Build();

            //Act
            var result = subscriber.Consume(new EmailRequest());

            //Assert
            result.Should().Be(ServiceBusMessageStates.Complete);
        }

        [Test]
        public void EmailApiErrorShouldReturnComplete()
        {
            //Arrange
            var dispatcher = new Mock<IEmailDispatcher>();
            dispatcher.Setup(d => d.SendEmail(It.IsAny<EmailRequest>()))
                .Throws(new CustomException(ErrorCodes.EmailApiError));
            var subscriber = new EmailRequestSubscriberBuilder().With(dispatcher).Build();

            //Act
            var result = subscriber.Consume(new EmailRequest());

            //Assert
            result.Should().Be(ServiceBusMessageStates.Complete);
        }

        [Test]
        public void EmailErrorShouldReturnRequeue()
        {
            //Arrange
            var dispatcher = new Mock<IEmailDispatcher>();
            dispatcher.Setup(d => d.SendEmail(It.IsAny<EmailRequest>()))
                .Throws(new CustomException(ErrorCodes.EmailError));
            var subscriber = new EmailRequestSubscriberBuilder().With(dispatcher).Build();

            //Act
            var result = subscriber.Consume(new EmailRequest());

            //Assert
            result.Should().Be(ServiceBusMessageStates.Requeue);
        }

        [Test]
        public void ExceptionShouldReturnComplete()
        {
            //Arrange
            var dispatcher = new Mock<IEmailDispatcher>();
            dispatcher.Setup(d => d.SendEmail(It.IsAny<EmailRequest>()))
                .Throws<Exception>();
            var subscriber = new EmailRequestSubscriberBuilder().With(dispatcher).Build();

            //Act
            var result = subscriber.Consume(new EmailRequest());

            //Assert
            result.Should().Be(ServiceBusMessageStates.Complete);
        }
    }
}