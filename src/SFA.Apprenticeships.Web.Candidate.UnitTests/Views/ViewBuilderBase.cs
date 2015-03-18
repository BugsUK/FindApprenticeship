namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views
{
    using System.Web;
    using System.Web.Routing;

    public abstract class ViewBuilderBase
    {
        protected ViewBuilderBase()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Following is needed on views that make use of the anti forgery token
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null)); 
        }
    }
}
