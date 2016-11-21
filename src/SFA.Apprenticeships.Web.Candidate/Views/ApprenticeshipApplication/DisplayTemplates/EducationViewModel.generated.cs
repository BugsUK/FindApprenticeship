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

namespace SFA.Apprenticeships.Web.Candidate.Views.ApprenticeshipApplication.DisplayTemplates
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipApplication/DisplayTemplates/EducationViewModel.cshtml")]
    public partial class EducationViewModel_ : System.Web.Mvc.WebViewPage<EducationViewModel>
    {
        public EducationViewModel_()
        {
        }
        public override void Execute()
        {
WriteLiteral("<section");

WriteLiteral(" id=\"applyEducation\"");

WriteLiteral(" class=\"section-border nobreak-print\"");

WriteLiteral(">\r\n    <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">\r\n        Education\r\n");

            
            #line 6 "..\..\Views\ApprenticeshipApplication\DisplayTemplates\EducationViewModel.cshtml"
        
            
            #line default
            #line hidden
            
            #line 6 "..\..\Views\ApprenticeshipApplication\DisplayTemplates\EducationViewModel.cshtml"
         if (ViewBag.VacancyId != null)
        {

            
            #line default
            #line hidden
WriteLiteral("            <a");

WriteLiteral(" class=\"icon-with-text\"");

WriteAttribute("href", Tuple.Create(" href=\'", 237), Tuple.Create("\'", 343)
            
            #line 8 "..\..\Views\ApprenticeshipApplication\DisplayTemplates\EducationViewModel.cshtml"
, Tuple.Create(Tuple.Create("", 244), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.ApprenticeshipApply, new {id = ViewBag.VacancyId})
            
            #line default
            #line hidden
, 244), false)
, Tuple.Create(Tuple.Create("", 328), Tuple.Create("#applyEducation", 328), true)
);

WriteLiteral(">\r\n                <i");

WriteLiteral(" class=\"the-icon fa fa-pencil\"");

WriteLiteral("></i><span");

WriteLiteral(" class=\"the-text\"");

WriteLiteral(">Edit section</span>\r\n            </a>\r\n");

            
            #line 11 "..\..\Views\ApprenticeshipApplication\DisplayTemplates\EducationViewModel.cshtml"

        }

            
            #line default
            #line hidden
WriteLiteral("    </h2>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" class=\"form-label-bold\"");

WriteLiteral(">Name of most recent school/college</p>\r\n        <span");

WriteAttribute("id", Tuple.Create(" id=\"", 604), Tuple.Create("\"", 658)
            
            #line 16 "..\..\Views\ApprenticeshipApplication\DisplayTemplates\EducationViewModel.cshtml"
, Tuple.Create(Tuple.Create("", 609), Tuple.Create<System.Object, System.Int32>(Html.IdFor(m => m.NameOfMostRecentSchoolCollege)
            
            #line default
            #line hidden
, 609), false)
);

WriteLiteral(" class=\"form-prepopped\"");

WriteLiteral(">");

            
            #line 16 "..\..\Views\ApprenticeshipApplication\DisplayTemplates\EducationViewModel.cshtml"
                                                                                       Write(Html.DisplayTextFor(m => m.NameOfMostRecentSchoolCollege));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"inline-fixed\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" class=\"form-label-bold\"");

WriteLiteral(">Years attended</p>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <span");

WriteLiteral(" class=\"form-hint\"");

WriteLiteral(">Started</span>\r\n            <span");

WriteAttribute("id", Tuple.Create(" id=\"", 952), Tuple.Create("\"", 985)
            
            #line 22 "..\..\Views\ApprenticeshipApplication\DisplayTemplates\EducationViewModel.cshtml"
, Tuple.Create(Tuple.Create("", 957), Tuple.Create<System.Object, System.Int32>(Html.IdFor(m => m.FromYear)
            
            #line default
            #line hidden
, 957), false)
);

WriteLiteral(" class=\"form-prepopped\"");

WriteLiteral(">");

            
            #line 22 "..\..\Views\ApprenticeshipApplication\DisplayTemplates\EducationViewModel.cshtml"
                                                                      Write(Html.DisplayTextFor(m => m.FromYear));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <span");

WriteLiteral(" class=\"form-hint\"");

WriteLiteral(">Finished</span>\r\n            <span");

WriteAttribute("id", Tuple.Create(" id=\"", 1176), Tuple.Create("\"", 1207)
            
            #line 26 "..\..\Views\ApprenticeshipApplication\DisplayTemplates\EducationViewModel.cshtml"
, Tuple.Create(Tuple.Create("", 1181), Tuple.Create<System.Object, System.Int32>(Html.IdFor(m => m.ToYear)
            
            #line default
            #line hidden
, 1181), false)
);

WriteLiteral(" class=\"form-prepopped\"");

WriteLiteral(">");

            
            #line 26 "..\..\Views\ApprenticeshipApplication\DisplayTemplates\EducationViewModel.cshtml"
                                                                    Write(Html.DisplayTextFor(m => m.ToYear));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n        </div>\r\n    </div>\r\n</section>");

        }
    }
}
#pragma warning restore 1591
