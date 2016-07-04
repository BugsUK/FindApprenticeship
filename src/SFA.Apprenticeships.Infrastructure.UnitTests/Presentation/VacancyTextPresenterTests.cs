namespace SFA.Apprenticeships.Infrastructure.UnitTests.Presentation
{
    using FluentAssertions;
    using Infrastructure.Presentation;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyTextPresenterTests
    {
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("Text", true)]
        [TestCase("Text\r\nMore text", true)]
        [TestCase("Text<br/>More text", false)]
        [TestCase("Text<br />More text", false)]
        [TestCase("Text<br>More text", false)]
        [TestCase("Text<br >More text", false)]
        public void GetPreserveFormattingCssClassTest(string text, bool expectPreserveFormattingCssClass)
        {
            var cssClass = text.GetPreserveFormattingCssClass();

            if (expectPreserveFormattingCssClass)
            {
                cssClass.Should().Be("preserve-formatting");
            }
            else
            {
                cssClass.Should().Be("");
            }
        }
    }
}