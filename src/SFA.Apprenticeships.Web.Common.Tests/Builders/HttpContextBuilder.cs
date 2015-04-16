namespace SFA.Apprenticeships.Web.Common.Tests.Builders
{
    using System.Collections;
    using System.Collections.Specialized;
    using System.Web;
    using Moq;

    public class HttpContextBuilder
    {
        private Mock<HttpRequestBase> _httpRequest;
        private Mock<HttpResponseBase> _httpResponse;

        public HttpContextBuilder()
        {
            _httpRequest = new Mock<HttpRequestBase>();

            _httpRequest.Setup(m => m.IsLocal).Returns(false);
            _httpRequest.Setup(m => m.ApplicationPath).Returns("/");
            _httpRequest.Setup(m => m.ServerVariables).Returns(new NameValueCollection());
            _httpRequest.Setup(m => m.RawUrl).Returns(string.Empty);
            _httpRequest.Setup(m => m.Cookies).Returns(new HttpCookieCollection());

            _httpResponse = new Mock<HttpResponseBase>();

            _httpResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(virtualPath => virtualPath);
            _httpResponse.Setup(m => m.Cookies).Returns(new HttpCookieCollection());
        }

        public HttpContextBase Build()
        {
            var mockHttpContext = new Mock<HttpContextBase>();

            mockHttpContext.Setup(m => m.Items).Returns(new Hashtable());
            mockHttpContext.Setup(m => m.Request).Returns(_httpRequest.Object);
            mockHttpContext.Setup(m => m.Response).Returns(_httpResponse.Object);

            return mockHttpContext.Object;
        }

        public HttpContextBuilder With(Mock<HttpRequestBase> httpRequest)
        {
            _httpRequest = httpRequest;
            return this;
        }

        public HttpContextBuilder With(Mock<HttpResponseBase> httpResponse)
        {
            _httpResponse = httpResponse;
            return this;
        }
    }
}