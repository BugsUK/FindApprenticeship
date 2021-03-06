﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SFA.Apprenticeships.Web.Candidate.Views.Shared
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Mvc.Ajax;
    using System.Web.Mvc.Html;
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    using SFA.Apprenticeships.Infrastructure.Presentation;
    using SFA.Apprenticeships.Web.Candidate;
    using SFA.Apprenticeships.Web.Candidate.Constants;
    using SFA.Apprenticeships.Web.Candidate.Constants.ViewModels;
    
    #line 1 "..\..\Views\Shared\_LoginPartial.cshtml"
    using SFA.Apprenticeships.Web.Candidate.Controllers;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Candidate.Helpers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Login;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Common.ViewModels.Locations;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/_LoginPartial.cshtml")]
    public partial class LoginPartial : System.Web.Mvc.WebViewPage<dynamic>
    {
        public LoginPartial()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 3 "..\..\Views\Shared\_LoginPartial.cshtml"
 if (Request.IsAuthenticated)
{
    var controller = ViewContext.Controller as CandidateControllerBase;
    var fullName = (controller != null && controller.UserContext != null) ? controller.UserContext.FullName : string.Empty;


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"account-info\"");

WriteLiteral(" id=\"bannerSignedIn\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">\r\n            <span");

WriteLiteral(" class=\"your-name\"");

WriteLiteral(" id=\"bannerUserName\"");

WriteLiteral(">");

            
            #line 10 "..\..\Views\Shared\_LoginPartial.cshtml"
                                                   Write(fullName);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n            <a");

WriteLiteral(" id=\"signout-link\"");

WriteAttribute("href", Tuple.Create(" href=\"", 488), Tuple.Create("\"", 528)
            
            #line 11 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 495), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(RouteNames.SignOut)
            
            #line default
            #line hidden
, 495), false)
);

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-sign-out\"");

WriteLiteral("></i>Sign out</a>\r\n        </div>\r\n        <div>\r\n            <a");

WriteLiteral(" class=\"account-link\"");

WriteLiteral(" id=\"savedapplications-link\"");

WriteAttribute("href", Tuple.Create(" href=\"", 668), Tuple.Create("\"", 737)
            
            #line 14 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 675), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.MyApplications)
            
            #line default
            #line hidden
, 675), false)
, Tuple.Create(Tuple.Create("", 726), Tuple.Create("#dashDrafts", 726), true)
);

WriteLiteral("></a>\r\n            <a");

WriteLiteral(" class=\"account-link\"");

WriteLiteral(" id=\"myapplications-link\"");

WriteAttribute("href", Tuple.Create(" href=\"", 805), Tuple.Create("\"", 861)
            
            #line 15 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 812), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.MyApplications)
            
            #line default
            #line hidden
, 812), false)
);

WriteLiteral(">My applications<span");

WriteLiteral(" id=\"dashUpdatesNumber\"");

WriteLiteral("></span></a>\r\n            <a");

WriteLiteral(" class=\"account-link last-link\"");

WriteLiteral(" id=\"mysettings-link\"");

WriteAttribute("href", Tuple.Create(" href=\"", 986), Tuple.Create("\"", 1036)
            
            #line 16 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 993), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.Settings)
            
            #line default
            #line hidden
, 993), false)
);

WriteLiteral(">Settings</a>\r\n        </div>\r\n    </div>\r\n");

            
            #line 19 "..\..\Views\Shared\_LoginPartial.cshtml"
}
else
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"account-info\"");

WriteLiteral(" id=\"bannerSignedOut\"");

WriteLiteral(">\r\n");

            
            #line 23 "..\..\Views\Shared\_LoginPartial.cshtml"
        
            
            #line default
            #line hidden
            
            #line 23 "..\..\Views\Shared\_LoginPartial.cshtml"
         if (ViewBag.AllowReturnUrl != null && ViewBag.AllowReturnUrl && Request.Url != null)
        {

            
            #line default
            #line hidden
WriteLiteral("            <a");

WriteAttribute("href", Tuple.Create(" href=\"", 1265), Tuple.Create("\"", 1350)
            
            #line 25 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 1272), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(RouteNames.SignIn, new { ReturnUrl = Request.Url.PathAndQuery })
            
            #line default
            #line hidden
, 1272), false)
);

WriteLiteral(" id=\"loginLink\"");

WriteLiteral(" title=\"Sign in / Create account\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-sign-in\"");

WriteLiteral("></i>Sign in / Create account</a>\r\n");

            
            #line 26 "..\..\Views\Shared\_LoginPartial.cshtml"
        }
        else
        {

            
            #line default
            #line hidden
WriteLiteral("            <a");

WriteAttribute("href", Tuple.Create(" href=\"", 1509), Tuple.Create("\"", 1548)
            
            #line 29 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 1516), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(RouteNames.SignIn)
            
            #line default
            #line hidden
, 1516), false)
);

WriteLiteral(" id=\"loginLink\"");

WriteLiteral(" title=\"Sign in / Create account\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-sign-in\"");

WriteLiteral("></i>Sign in / Create account</a>\r\n");

            
            #line 30 "..\..\Views\Shared\_LoginPartial.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");

            
            #line 32 "..\..\Views\Shared\_LoginPartial.cshtml"
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
