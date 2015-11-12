using System.Web;
using SFA.Apprenticeships.Web.Manage;

namespace SFA.Apprenticeships.Web.Manage.UnitTests.Views
{
    using System.Web.Routing;
    using NUnit.Framework;

    public abstract class ViewUnitTest
    {
        [SetUp]
        public void SetUp()
        {
            RouteTable.Routes.Clear();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //Following is needed on views that make use of the anti forgery token
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));
        }
    }
}