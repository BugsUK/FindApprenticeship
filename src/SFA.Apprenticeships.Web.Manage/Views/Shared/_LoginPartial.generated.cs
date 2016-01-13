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

namespace SFA.Apprenticeships.Web.Manage.Views.Shared
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
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Manage;
    
    #line 1 "..\..\Views\Shared\_LoginPartial.cshtml"
    using SFA.Apprenticeships.Web.Manage.Constants;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Shared\_LoginPartial.cshtml"
    using SFA.Apprenticeships.Web.Manage.Controllers;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates;
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.EditorTemplates;
    
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

            
            #line 4 "..\..\Views\Shared\_LoginPartial.cshtml"
 if (Request.IsAuthenticated)
{

            
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

            
            #line 8 "..\..\Views\Shared\_LoginPartial.cshtml"
                                                   Write(User.Identity.Name);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n            <a");

WriteLiteral(" id=\"signout-link\"");

WriteAttribute("href", Tuple.Create(" href=\"", 344), Tuple.Create("\"", 394)
            
            #line 9 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 351), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(ManagementRouteNames.SignOut)
            
            #line default
            #line hidden
, 351), false)
);

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-sign-out\"");

WriteLiteral("></i>Sign out</a>\r\n        </div>\r\n        <div>\r\n            <a");

WriteLiteral(" class=\"account-link\"");

WriteLiteral(" id=\"applicationsLink\"");

WriteAttribute("href", Tuple.Create(" href=\"", 528), Tuple.Create("\"", 580)
            
            #line 12 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 535), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(ManagementRouteNames.Dashboard)
            
            #line default
            #line hidden
, 535), false)
);

WriteLiteral(">Agency home</a>\r\n        </div>\r\n    </div>\r\n");

            
            #line 15 "..\..\Views\Shared\_LoginPartial.cshtml"
}
else
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"account-info\"");

WriteLiteral(" id=\"bannerSignedOut\"");

WriteLiteral(">\r\n        <a");

WriteAttribute("href", Tuple.Create(" href=\"", 702), Tuple.Create("\"", 751)
            
            #line 19 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 709), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(ManagementRouteNames.SignIn)
            
            #line default
            #line hidden
, 709), false)
);

WriteLiteral(" id=\"loginLink\"");

WriteLiteral(" title=\"Sign in\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-sign-in\"");

WriteLiteral("></i>Sign in</a>\r\n    </div>\r\n");

            
            #line 21 "..\..\Views\Shared\_LoginPartial.cshtml"
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
