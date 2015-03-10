namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Sms.Reach
{
    using System;
    using Communication.Sms;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SmsNumberFormatterTests
    {
        private const string ExpectedSmsNumber = "447999999999";

        [Test]
        public void ShouldFormatValidMobileNumber()
        {
            new SmsNumberFormatter().Format("07999999999").Should().Be(ExpectedSmsNumber);
        }

        [Test]
        public void ShouldFormatValidMobileNumberWithLeadingAndTrailingWhitespace()
        {
            new SmsNumberFormatter().Format("  07999999999  ").Should().Be(ExpectedSmsNumber);
        }

        [Test]
        public void ShouldFormatValidMobileNumberWithCountryCodePrefix()
        {
            new SmsNumberFormatter().Format("447999999999").Should().Be(ExpectedSmsNumber);
        }


        [Test]
        public void ShouldFormatValidMobileNumberWithPlusAndCountryCodePrefix()
        {
            new SmsNumberFormatter().Format("+447999999999").Should().Be(ExpectedSmsNumber);
        }

        [Test]
        public void ShouldThrowWhenMobileNumberIsEmpty()
        {
            Action action = () => new SmsNumberFormatter().Format(null);

            var exception = action.ShouldThrow<ArgumentNullException>();

            exception.Where(e => e.ParamName == "smsNumber");
        }
    }
}
