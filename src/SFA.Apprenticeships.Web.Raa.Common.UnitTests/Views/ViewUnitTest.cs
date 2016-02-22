namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views
{
    using System.Web;
    using NUnit.Framework;

    public abstract class ViewUnitTest
    {
        [SetUp]
        public void SetUp()
        {
            //Following is needed on views that make use of the anti forgery token
            HttpContext.Current = new HttpContext(new HttpRequest(null, "http://localhost", null), new HttpResponse(null));
        }
    }
}