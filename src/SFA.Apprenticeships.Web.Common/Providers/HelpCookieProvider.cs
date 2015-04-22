namespace SFA.Apprenticeships.Web.Common.Providers
{
    using System;
    using System.Web;
    using Application.Interfaces.Candidates;

    public class HelpCookieProvider : IHelpCookieProvider
    {
        private readonly ICandidateService _candidateService;
        
        public const string CookieName = "NAS.Help";

        public enum CookieKeys
        {
            ShowSearchTour
        }

        public HelpCookieProvider(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        public bool ShowSearchTour(HttpContextBase httpContext, Guid? candidateId)
        {
            var httpCookie = httpContext.Request.Cookies.Get(CookieName) ?? new HttpCookie(CookieName) { Expires = DateTime.UtcNow.AddMonths(12) };

            var cookieValue = httpCookie[CookieKeys.ShowSearchTour.ToString()];

            if (cookieValue == candidateId.ToString())
            {
                return false;
            }

            if (candidateId.HasValue && (string.IsNullOrEmpty(cookieValue) || cookieValue != candidateId.ToString()))
            {
                var candidate = _candidateService.GetCandidate(candidateId.Value);
                var showSearchTour = cookieValue != Guid.Empty.ToString() && candidate.HelpPreferences.ShowSearchTour;
                if (candidate.HelpPreferences.ShowSearchTour)
                {
                    candidate.HelpPreferences.ShowSearchTour = false;
                    _candidateService.SaveCandidate(candidate);
                }

                httpCookie[CookieKeys.ShowSearchTour.ToString()] = candidateId.ToString();

                httpContext.Response.Cookies.Add(httpCookie);

                return showSearchTour;
            }

            if (!string.IsNullOrEmpty(cookieValue))
            {
                return false;
            }

            httpCookie[CookieKeys.ShowSearchTour.ToString()] = Guid.Empty.ToString();

            httpContext.Response.Cookies.Add(httpCookie);

            return true;
        }
    }
}