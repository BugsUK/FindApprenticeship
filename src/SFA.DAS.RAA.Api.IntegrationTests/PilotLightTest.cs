namespace SFA.DAS.RAA.Api.UnitTests
{
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class PilotLightTest
    {
        [Test]
        public void HelloWorld()
        {
            const string text = "Hello World";
            text.Should().Be("Hello World");
        }
    }
}