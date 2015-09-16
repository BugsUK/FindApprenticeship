﻿using System.Security.Claims;
using System.Web;

namespace SFA.Apprenticeships.Web.Common.Providers
{
    public interface ICookieAuthorizationDataProvider
    {
        void AddClaim(Claim claim, HttpContextBase httpContext, string username);

        Claim[] GetClaims(HttpContextBase httpContext, string username);
    }
}