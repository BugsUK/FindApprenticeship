namespace SFA.Apprenticeships.Infrastructure.UnitTests.Communication.Sms.Reach
{
    using System;
    using FluentAssertions;
    using Infrastructure.Communication.Sms;
    using NUnit.Framework;

    [TestFixture]
    public class ReachSmsNumberFormatterTests
    {
        private const string ExpectedSmsNumber = "447999999999";

        [Test]
        public void ShouldFormatValidMobileNumber()
        {
            new ReachSmsNumberFormatter().Format("07999999999").Should().Be(ExpectedSmsNumber);
        }

        [Test]
        public void ShouldFormatValidMobileNumberWithLeadingAndTrailingWhitespace()
        {
            new ReachSmsNumberFormatter().Format("  07999999999  ").Should().Be(ExpectedSmsNumber);
        }

        [Test]
        public void ShouldFormatValidMobileNumberWithCountryCodePrefix()
        {
            new ReachSmsNumberFormatter().Format("447999999999").Should().Be(ExpectedSmsNumber);
        }


        [Test]
        public void ShouldFormatValidMobileNumberWithPlusAndCountryCodePrefix()
        {
            new ReachSmsNumberFormatter().Format("+447999999999").Should().Be(ExpectedSmsNumber);
        }

        [Test]
        public void ShouldThrowWhenMobileNumberIsNull()
        {
            Action action = () => new ReachSmsNumberFormatter().Format(null);

            var exception = action.ShouldThrow<ArgumentNullException>();

            exception.Where(e => e.ParamName == "smsNumber");
        }

        [Test]
        public void ShouldThrowWhenMobileNumberIsWhitespace()
        {
            Action action = () => new ReachSmsNumberFormatter().Format(" ");

            var exception = action.ShouldThrow<ArgumentNullException>();

            exception.Where(e => e.ParamName == "smsNumber");
        }
    }
}
