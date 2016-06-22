namespace SFA.Apprenticeships.Web.Common.UnitTests.Providers
{
    using System.Web;
    using SFA.Infrastructure.Interfaces;
    using Common.Providers;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;

    [TestFixture]
    public class RobotCrawlerProviderTests
    {
        private readonly RobotCrawlerProvider _robotCrawlerProvider;

        public RobotCrawlerProviderTests()
        {
            var logService = new Mock<ILogService>();
            _robotCrawlerProvider = new RobotCrawlerProvider(logService.Object);
        }

        [TestCase("Mozilla/5.0 (compatible; Googlebot/2.1; +http://www.google.com/bot.html)", true)]
        [TestCase("Mozilla/5.0 (compatible; googlebot/2.1; +http://www.google.com/bot.html)", true)]
        [TestCase("Googlebot/2.1 (+http://www.googlebot.com/bot.html)", true)]
        [TestCase("Googlebot/2.1 (+http://www.google.com/bot.html)", true)]
        [TestCase("Mozilla/5.0 (compatible; Findxbot/1.0; +http://www.findxbot.com)", true)]
        [TestCase("Mozilla/5.0 (compatible; SemrushBot/0.98~bl; +http://www.semrush.com/bot.html)", true)]
        [TestCase("Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/43.0.2357.132 Safari/537.36", false)]
        [TestCase("User-Agent	Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; Touch; rv:11.0) like Gecko", false)]
        public void IdentifyRobotAgentsTests(string userAgent, bool isRobot)
        {
            var httpContextBase = new Mock<HttpContextBase>();
            var httpRequestBase = new Mock<HttpRequestBase>();
            httpRequestBase.Setup(x => x.UserAgent).Returns(userAgent);
            httpContextBase.Setup(x => x.Request).Returns(httpRequestBase.Object);

            Assert.AreEqual(_robotCrawlerProvider.IsRobot(httpContextBase.Object), isRobot);
        }
    }
}
