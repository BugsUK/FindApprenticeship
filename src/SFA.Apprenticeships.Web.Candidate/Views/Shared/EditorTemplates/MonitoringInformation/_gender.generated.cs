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

namespace SFA.Apprenticeships.Web.Candidate.Views.Shared.EditorTemplates.MonitoringInformation
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
    using SFA.Apprenticeships.Web.Common.Views.Shared.DisplayTemplates;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/MonitoringInformation/_gender.cshtml")]
    public partial class gender : System.Web.Mvc.WebViewPage<MonitoringInformationViewModel>
    {
        public gender()
        {
        }
        public override void Execute()
        {
WriteLiteral("<div");

WriteLiteral(" class=\"form-group inline clearfix\"");

WriteLiteral(">\r\n    <p");

WriteLiteral(" class=\"form-label-bold\"");

WriteLiteral(" id=\"gender-label\"");

WriteLiteral(">Are you?</p>\r\n    <label");

WriteLiteral(" for=\"gender-male\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 6 "..\..\Views\Shared\EditorTemplates\MonitoringInformation\_gender.cshtml"
   Write(Html.RadioButtonFor(m => m.Gender, 1, new { id = "gender-male", aria_labelledby = "gender-label" }));

            
            #line default
            #line hidden
WriteLiteral(" Male\r\n    </label>\r\n\r\n    <label");

WriteLiteral(" for=\"gender-female\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 10 "..\..\Views\Shared\EditorTemplates\MonitoringInformation\_gender.cshtml"
   Write(Html.RadioButtonFor(m => m.Gender, 2, new { id = "gender-female", aria_labelledby = "gender-label" }));

            
            #line default
            #line hidden
WriteLiteral(" Female\r\n    </label>\r\n\r\n    <label");

WriteLiteral(" for=\"gender-other\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 14 "..\..\Views\Shared\EditorTemplates\MonitoringInformation\_gender.cshtml"
   Write(Html.RadioButtonFor(m => m.Gender, 3, new { id = "gender-other", aria_labelledby = "gender-label" }));

            
            #line default
            #line hidden
WriteLiteral(" Other\r\n    </label>\r\n\r\n    <label");

WriteLiteral(" for=\"gender-prefer-not-to-say\"");

WriteLiteral(" id=\"gender-prefer-not-to-say-label\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 18 "..\..\Views\Shared\EditorTemplates\MonitoringInformation\_gender.cshtml"
   Write(Html.RadioButtonFor(m => m.Gender, 4, new { id = "gender-prefer-not-to-say", aria_labelledby = "gender-prefer-not-to-say-label" }));

            
            #line default
            #line hidden
WriteLiteral(" Prefer not to say\r\n    </label>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
