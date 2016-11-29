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
    
    #line 3 "..\..\Views\Shared\_LoginPartial.cshtml"
    using Roles = SFA.Apprenticeships.Domain.Entities.Raa.Roles;
    
    #line default
    #line hidden
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

            
            #line 5 "..\..\Views\Shared\_LoginPartial.cshtml"
 if (Request.IsAuthenticated)
{

            
            #line default
            #line hidden
WriteLiteral("        <div");

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

WriteAttribute("href", Tuple.Create(" href=\"", 354), Tuple.Create("\"", 404)
            
            #line 9 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 361), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(ManagementRouteNames.SignOut)
            
            #line default
            #line hidden
, 361), false)
);

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-sign-out\"");

WriteLiteral("></i>Sign out</a>\r\n        </div>\r\n");

WriteLiteral("        <div>\r\n");

            
            #line 12 "..\..\Views\Shared\_LoginPartial.cshtml"
            
            
            #line default
            #line hidden
            
            #line 12 "..\..\Views\Shared\_LoginPartial.cshtml"
             if (User.IsInRole(Roles.Admin))
            {

            
            #line default
            #line hidden
WriteLiteral("                <a");

WriteLiteral(" class=\"account-link\"");

WriteLiteral(" id=\"adminLink\"");

WriteAttribute("href", Tuple.Create(" href=\"", 596), Tuple.Create("\"", 648)
            
            #line 14 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 603), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(ManagementRouteNames.AdminList)
            
            #line default
            #line hidden
, 603), false)
);

WriteLiteral(">Admin</a>\r\n");

            
            #line 15 "..\..\Views\Shared\_LoginPartial.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("            <a");

WriteLiteral(" class=\"account-link\"");

WriteLiteral(" id=\"applicationsLink\"");

WriteAttribute("href", Tuple.Create(" href=\"", 733), Tuple.Create("\"", 785)
            
            #line 16 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 740), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(ManagementRouteNames.Dashboard)
            
            #line default
            #line hidden
, 740), false)
);

WriteLiteral(">Agency home</a>\r\n        </div>\r\n");

            
            #line 18 "..\..\Views\Shared\_LoginPartial.cshtml"
}
else
{

            
            #line default
            #line hidden
WriteLiteral("    <a");

WriteAttribute("href", Tuple.Create(" href=\"", 838), Tuple.Create("\"", 887)
            
            #line 21 "..\..\Views\Shared\_LoginPartial.cshtml"
, Tuple.Create(Tuple.Create("", 845), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(ManagementRouteNames.SignIn)
            
            #line default
            #line hidden
, 845), false)
);

WriteLiteral(" id=\"loginLink\"");

WriteLiteral(" title=\"Sign in\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-sign-in\"");

WriteLiteral("></i>Sign in</a>\r\n");

            
            #line 22 "..\..\Views\Shared\_LoginPartial.cshtml"
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
