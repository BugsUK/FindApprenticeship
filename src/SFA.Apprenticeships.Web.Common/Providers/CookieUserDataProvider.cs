namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Web;

    public class CookieUserDataProvider : IUserDataProvider
    {
        private const string UserDataCookieName = "User.Data";
        private const string UserContextCookieName = "User.Context";
        private const string UserNameCookieName = "User.UserName";
        private const string FullNameCookieName = "User.FullName";
        private const string AcceptedTermsAndConditionsVersion = "User.TermsConditionsVersion";

        private readonly HttpContextBase _httpContext;
        
        private HttpCookie _httpDataCookie;
        
        public CookieUserDataProvider(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        public UserContext GetUserContext()
        {
            lock (_httpContext)
            {
                var cookie = _httpContext.Request.Cookies.Get(UserContextCookieName);

                return cookie == null
                    ? null
                    : new UserContext
                    {
                        UserName = cookie.Values[UserNameCookieName],
                        FullName = cookie.Values[FullNameCookieName],
                        AcceptedTermsAndConditionsVersion = cookie.Values[AcceptedTermsAndConditionsVersion]
                    };
            }
        }

        public void SetUserContext(string userName, string fullName, string acceptedTermsAndConditionsVersion)
        {
            lock (_httpContext)
            {
                var cookie = new HttpCookie(UserContextCookieName);

                cookie.Values.Add(UserNameCookieName, userName);
                cookie.Values.Add(FullNameCookieName, fullName);
                cookie.Values.Add(AcceptedTermsAndConditionsVersion, acceptedTermsAndConditionsVersion);

                AddOrUpdateResponseCookie(_httpContext, cookie);
            }
        }

        public void Clear()
        {
            lock (_httpContext)
            {
                var cookie = CreateExpiredCookie(UserContextCookieName);

                AddOrUpdateResponseCookie(_httpContext, cookie);
            }
        }

        public void Push(string key, string value)
        {
            lock (_httpContext)
            {
                var urlEncode = _httpContext.Server.UrlEncode(value);
                if (urlEncode == null) return;
                HttpDataCookie.Values.Remove(key);
                HttpDataCookie.Values.Add(key, urlEncode);
            }
        }

        public string Get(string key)
        {
            lock (_httpContext)
            {
                if (HttpDataCookie == null || HttpDataCookie.Values[key] == null)
                {
                    return null;
                }

                return _httpContext.Server.UrlDecode(HttpDataCookie.Values[key]);
            }
        }

        public string Pop(string key)
        {
            lock (_httpContext)
            {
                var value = Get(key);

                if (value == null)
                {
                    return null;
                }

                HttpDataCookie.Values.Remove(key);

                return value;
            }
        }

        private HttpCookie HttpDataCookie
        {
            get
            {
                if (_httpDataCookie == null)
                {
                    _httpDataCookie = GetOrCreateDataCookie(_httpContext);
                }
                return _httpDataCookie;
            }
        }

        private static HttpCookie GetOrCreateDataCookie(HttpContextBase context)
        {
            var requestDataCookie = context.Request.Cookies.Get(UserDataCookieName);
            var responseDataCookie = context.Response.Cookies.Get(UserDataCookieName);

            var dataCookie = new HttpCookie(UserDataCookieName);

            if (requestDataCookie != null && requestDataCookie.Values.HasKeys() && (responseDataCookie == null || !responseDataCookie.Values.HasKeys()))
            {
                foreach (var key in requestDataCookie.Values.AllKeys)
                {
                    dataCookie.Values.Add(key, requestDataCookie.Values[key]);
                }
            }

            dataCookie = responseDataCookie != null && responseDataCookie.Values.HasKeys() ? responseDataCookie : dataCookie;

            AddOrUpdateResponseCookie(context, dataCookie);

            return dataCookie;
        }

        private static HttpCookie CreateExpiredCookie(string name)
        {
            var cookie = new HttpCookie(name)
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            return cookie;
        }

        private static void AddOrUpdateResponseCookie(HttpContextBase context, HttpCookie cookie)
        {
            if (context.Response.Cookies.Get(cookie.Name) == null)
            {
                context.Response.Cookies.Add(cookie);
            }
            else
            {
                context.Response.Cookies.Set(cookie);
            }
        }
    }
}
