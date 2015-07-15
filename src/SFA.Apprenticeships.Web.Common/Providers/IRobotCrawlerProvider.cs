namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System.Web;

    public interface IRobotCrawlerProvider
    {
        bool IsRobot(HttpContextBase httpContext);
    }
}