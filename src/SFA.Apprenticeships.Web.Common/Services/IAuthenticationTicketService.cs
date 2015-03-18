namespace SFA.Apprenticeships.Web.Common.Services
{
    using System;
    using System.Web;
    using System.Web.Security;

    public interface IAuthenticationTicketService
    {
        FormsAuthenticationTicket GetTicket();

        void RefreshTicket();

        string[] GetClaims(FormsAuthenticationTicket ticket);

        void Clear();

        void SetAuthenticationCookie(string userId, params string[] claims);

        DateTime GetExpirationTimeFrom(FormsAuthenticationTicket ticket);
    }
}
