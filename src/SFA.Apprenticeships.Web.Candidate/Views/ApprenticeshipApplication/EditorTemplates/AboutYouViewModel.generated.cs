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

namespace SFA.Apprenticeships.Web.Candidate.Views.ApprenticeshipApplication.EditorTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipApplication/EditorTemplates/AboutYouViewModel.cshtml")]
    public partial class AboutYouViewModel_ : System.Web.Mvc.WebViewPage<AboutYouViewModel>
    {
        public AboutYouViewModel_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<fieldset");

WriteLiteral(" id=\"applyAboutYou\"");

WriteLiteral(" class=\"fieldset-with-border\"");

WriteLiteral(">\r\n    <legend");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">About you</legend>\r\n    <div");

WriteLiteral(" class=\"form-group text\"");

WriteLiteral(">\r\n        <details");

WriteLiteral(" id=\"appTourAbout\"");

WriteLiteral(">\r\n            <summary");

WriteLiteral(" aria-controls=\"helpText\"");

WriteLiteral(" role=\"button\"");

WriteLiteral(" aria-expanded=\"false\"");

WriteLiteral(">Help with this section</summary>\r\n            <p");

WriteLiteral(" class=\"panel panel-border-narrow\"");

WriteLiteral(" id=\"helpText\"");

WriteLiteral(" role=\"region\"");

WriteLiteral(" aria-hidden=\"true\"");

WriteLiteral(@">
                Think carefully about how you answer these questions. Where possible,
                keep the apprenticeship that you're applying for in mind, and tailor your
                answers to match the role.
            </p>
        </details>
    </div>

");

WriteLiteral("    ");

            
            #line 16 "..\..\Views\ApprenticeshipApplication\EditorTemplates\AboutYouViewModel.cshtml"
Write(Html.FormTextAreaFor(m => m.WhatAreYourStrengths, controlHtmlAttributes: new { @data_val_length_max = "4000", rows = "4", role = "textbox", aria_multiline = "true", @class = "appTourStrengths" }, hintHtmlAttributes: new { @class = "text" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 17 "..\..\Views\ApprenticeshipApplication\EditorTemplates\AboutYouViewModel.cshtml"
Write(Html.FormTextAreaFor(m => m.WhatDoYouFeelYouCouldImprove, controlHtmlAttributes: new { @data_val_length_max = "4000", rows = "4", role = "textbox", aria_multiline = "true", @class = "appTourSkills" }, hintHtmlAttributes: new { @class = "text" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 18 "..\..\Views\ApprenticeshipApplication\EditorTemplates\AboutYouViewModel.cshtml"
Write(Html.FormTextAreaFor(m => m.WhatAreYourHobbiesInterests, controlHtmlAttributes: new { @data_val_length_max = "4000", rows = "4", role = "textbox", aria_multiline = "true", @class = "appTourHobbies" }, hintHtmlAttributes: new { @class = "text" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n</fieldset>\r\n");

        }
    }
}
#pragma warning restore 1591
