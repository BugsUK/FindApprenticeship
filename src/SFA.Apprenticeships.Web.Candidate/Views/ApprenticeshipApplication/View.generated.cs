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

namespace SFA.Apprenticeships.Web.Candidate.Views.ApprenticeshipApplication
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
    
    #line 1 "..\..\Views\ApprenticeshipApplication\View.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Applications;
    
    #line default
    #line hidden
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipApplication/View.cshtml")]
    public partial class View : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Candidate.ViewModels.Applications.ApprenticeshipApplicationViewModel>
    {
        public View()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Views\ApprenticeshipApplication\View.cshtml"
  
    ViewBag.Title = "View your application - Find an apprenticeship";
    Layout = "~/Views/Shared/_Layout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"grid-row\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"column-two-thirds\"");

WriteLiteral(">\r\n        <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">Your application</h1>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"column-one-third\"");

WriteLiteral(">\r\n        <ul");

WriteLiteral(" class=\"list sfa-align-right-tablet sfa-xlarge-top-margin\"");

WriteLiteral(">\r\n            <li>\r\n                <a");

WriteLiteral(" class=\"print-trigger hide-nojs\"");

WriteLiteral(" href=\"\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-print\"");

WriteLiteral("></i>Print this page</a>\r\n            </li>\r\n        </ul>\r\n    </div>\r\n</div>\r\n\r" +
"\n<div");

WriteLiteral(" class=\"grid-row\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"column-two-thirds\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"hgroup-medium\"");

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(" id=\"vacancy-title\"");

WriteLiteral(">");

            
            #line 25 "..\..\Views\ApprenticeshipApplication\View.cshtml"
                                                     Write(Model.VacancyDetail.Title);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n            <p");

WriteLiteral(" class=\"heading-secondary \"");

WriteLiteral(" id=\"vacancy-employer\"");

WriteLiteral(">");

            
            #line 26 "..\..\Views\ApprenticeshipApplication\View.cshtml"
                                                           Write(Model.VacancyDetail.EmployerName);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n        </div>\r\n        <p");

WriteLiteral("  id=\"vacancy-summary\"");

WriteLiteral(">");

            
            #line 28 "..\..\Views\ApprenticeshipApplication\View.cshtml"
                            Write(Model.VacancyDetail.Description);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"column-one-third para-btm-margin\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" class=\"sfa-small-bottom-margin\"");

WriteLiteral(">This is your submitted application.</p>\r\n        <p");

WriteLiteral(" class=\"sfa-small-bottom-margin\"");

WriteLiteral(">\r\n            You applied on ");

            
            #line 33 "..\..\Views\ApprenticeshipApplication\View.cshtml"
                      Write(Html.DisplayFor(m => m.DateApplied, "Date"));

            
            #line default
            #line hidden
WriteLiteral(".\r\n");

            
            #line 34 "..\..\Views\ApprenticeshipApplication\View.cshtml"
            
            
            #line default
            #line hidden
            
            #line 34 "..\..\Views\ApprenticeshipApplication\View.cshtml"
             if (Model.Status == ApplicationStatuses.Successful)
            {

            
            #line default
            #line hidden
WriteLiteral("                <span");

WriteLiteral(" class=\"inl-block\"");

WriteLiteral(">This application was successful.</span>\r\n");

            
            #line 37 "..\..\Views\ApprenticeshipApplication\View.cshtml"
            }
            else if (Model.Status == ApplicationStatuses.Unsuccessful)
            {

            
            #line default
            #line hidden
WriteLiteral("                <span");

WriteLiteral(" class=\"inl-block\"");

WriteLiteral(">This application was unsuccessful.</span>\r\n");

WriteLiteral("            <p");

WriteLiteral(" class=\"hide-print\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" id=\"return-to-my-feedback\"");

WriteAttribute("href", Tuple.Create(" href=\"", 1781), Tuple.Create("\"", 1851)
            
            #line 42 "..\..\Views\ApprenticeshipApplication\View.cshtml"
, Tuple.Create(Tuple.Create("", 1788), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.CandidateApplicationFeedback)
            
            #line default
            #line hidden
, 1788), false)
);

WriteLiteral(">Return to your feedback</a>\r\n            </p>\r\n");

            
            #line 44 "..\..\Views\ApprenticeshipApplication\View.cshtml"
            }
            else if (Model.Status == ApplicationStatuses.ExpiredOrWithdrawn)
            {

            
            #line default
            #line hidden
WriteLiteral("                <span");

WriteLiteral(" class=\"inl-block\"");

WriteLiteral(">This apprenticeship expired or was withdrawn.</span>\r\n");

            
            #line 48 "..\..\Views\ApprenticeshipApplication\View.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("            </p>\r\n        </div>\r\n    </div>\r\n\r\n");

WriteLiteral("    ");

            
            #line 53 "..\..\Views\ApprenticeshipApplication\View.cshtml"
Write(Html.DisplayFor(m => m.Candidate));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 54 "..\..\Views\ApprenticeshipApplication\View.cshtml"
Write(Html.DisplayFor(m => m.Candidate.Education));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 55 "..\..\Views\ApprenticeshipApplication\View.cshtml"
Write(Html.DisplayFor(m => m.Candidate.Qualifications, "Application/Qualifications"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 56 "..\..\Views\ApprenticeshipApplication\View.cshtml"
Write(Html.DisplayFor(m => m.Candidate.WorkExperience, "Application/WorkExperience"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 57 "..\..\Views\ApprenticeshipApplication\View.cshtml"
Write(Html.DisplayFor(m => m.Candidate.TrainingCourses, "Application/TrainingCourses"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 58 "..\..\Views\ApprenticeshipApplication\View.cshtml"
Write(Html.DisplayFor(m => m.Candidate.AboutYou));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    <section");

WriteLiteral(" class=\"sfa-section-bordered\"");

WriteLiteral(">\r\n        <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">\r\n            Additional questions\r\n        </h2>\r\n");

WriteLiteral("        ");

            
            #line 64 "..\..\Views\ApprenticeshipApplication\View.cshtml"
   Write(Html.DisplayFor(m => m.Candidate.EmployerQuestionAnswers));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 65 "..\..\Views\ApprenticeshipApplication\View.cshtml"
   Write(Html.DisplayFor(m => m.Candidate.MonitoringInformation, "MonitoringInformation/_disability"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </section>\r\n\r\n    <div");

WriteLiteral(" class=\"form-group hide-print\"");

WriteLiteral(">\r\n        <p>\r\n            <a");

WriteLiteral(" id=\"return-to-my-applications\"");

WriteAttribute("href", Tuple.Create(" href=\"", 2980), Tuple.Create("\"", 3036)
            
            #line 70 "..\..\Views\ApprenticeshipApplication\View.cshtml"
, Tuple.Create(Tuple.Create("", 2987), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.MyApplications)
            
            #line default
            #line hidden
, 2987), false)
);

WriteLiteral(" class=\"button\"");

WriteLiteral(">Return to my applications</a>\r\n        </p>\r\n    </div>");

        }
    }
}
#pragma warning restore 1591
