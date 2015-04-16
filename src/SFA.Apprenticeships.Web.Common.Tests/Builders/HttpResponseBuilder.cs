namespace SFA.Apprenticeships.Web.Common.Tests.Builders
{
    using System.Web;
    using Moq;

    public class HttpResponseBuilder
    {
        public Mock<HttpResponseBase> Build()
        {
            var httpResponse = new Mock<HttpResponseBase>();

            httpResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(virtualPath => virtualPath);
            httpResponse.Setup(m => m.Cookies).Returns(new HttpCookieCollection());

            return httpResponse;
        }
    }
}