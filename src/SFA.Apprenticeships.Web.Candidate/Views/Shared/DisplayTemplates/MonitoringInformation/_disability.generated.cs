﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SFA.Apprenticeships.Web.Candidate.Views.Shared.DisplayTemplates.MonitoringInformation
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
    using SFA.Apprenticeships.Web.Candidate;
    using SFA.Apprenticeships.Web.Candidate.Constants;
    using SFA.Apprenticeships.Web.Candidate.Constants.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.Helpers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Locations;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Login;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/MonitoringInformation/_disability.cshtml")]
    public partial class disability : System.Web.Mvc.WebViewPage<MonitoringInformationViewModel>
    {
        public disability()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <p");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Is there anything we can do to support your interview?</p>\r\n");

            
            #line 5 "..\..\Views\Shared\DisplayTemplates\MonitoringInformation\_disability.cshtml"
    
            
            #line default
            #line hidden
            
            #line 5 "..\..\Views\Shared\DisplayTemplates\MonitoringInformation\_disability.cshtml"
     if (Model.RequiresSupportForInterview)
    {

            
            #line default
            #line hidden
WriteLiteral("        <span");

WriteAttribute("id", Tuple.Create(" id=\"", 218), Tuple.Create("\"", 280)
            
            #line 7 "..\..\Views\Shared\DisplayTemplates\MonitoringInformation\_disability.cshtml"
, Tuple.Create(Tuple.Create("", 223), Tuple.Create<System.Object, System.Int32>(Html.IdFor(m => m.AnythingWeCanDoToSupportYourInterview)
            
            #line default
            #line hidden
, 223), false)
);

WriteLiteral(" class=\"form-prepopped prewrap\"");

WriteLiteral(">");

            
            #line 7 "..\..\Views\Shared\DisplayTemplates\MonitoringInformation\_disability.cshtml"
                                                                                                       Write(Html.DisplayFor(m => m.AnythingWeCanDoToSupportYourInterview));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n");

            
            #line 8 "..\..\Views\Shared\DisplayTemplates\MonitoringInformation\_disability.cshtml"
    }
    else
    {

            
            #line default
            #line hidden
WriteLiteral("        <span");

WriteAttribute("id", Tuple.Create(" id=\"", 421), Tuple.Create("\"", 483)
            
            #line 11 "..\..\Views\Shared\DisplayTemplates\MonitoringInformation\_disability.cshtml"
, Tuple.Create(Tuple.Create("", 426), Tuple.Create<System.Object, System.Int32>(Html.IdFor(m => m.AnythingWeCanDoToSupportYourInterview)
            
            #line default
            #line hidden
, 426), false)
);

WriteLiteral(" class=\"form-prepopped prewrap\"");

WriteLiteral(">I don\'t have any interview support requirements</span>\r\n");

            
            #line 12 "..\..\Views\Shared\DisplayTemplates\MonitoringInformation\_disability.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>");

        }
    }
}
#pragma warning restore 1591
