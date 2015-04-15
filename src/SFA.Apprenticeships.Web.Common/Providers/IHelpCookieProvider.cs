namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System.Web;

    public interface IHelpCookieProvider
    {
        bool ShowSearchTour(HttpContextBase httpContext);
    }
}