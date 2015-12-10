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

namespace SFA.Apprenticeships.Web.Manage.Views.Vacancy
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
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Manage;
    
    #line 3 "..\..\Views\Vacancy\Review.cshtml"
    using SFA.Apprenticeships.Web.Manage.Constants;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Vacancy\Review.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates;
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.EditorTemplates;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Vacancy/Review.cshtml")]
    public partial class Review : System.Web.Mvc.WebViewPage<VacancyViewModel>
    {
        public Review()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Views\Vacancy\Review.cshtml"
  
    ViewBag.Title = "QA a Vacancy - Review vacancy";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 8 "..\..\Views\Vacancy\Review.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div>\r\n    <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">\r\n        Vacancy preview\r\n    </h1>\r\n\r\n");

WriteLiteral("    ");

            
            #line 15 "..\..\Views\Vacancy\Review.cshtml"
Write(Html.DisplayFor(m => m, VacancyViewModel.PartialView));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 17 "..\..\Views\Vacancy\Review.cshtml"
    
            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\Vacancy\Review.cshtml"
     using (Html.BeginRouteForm(ManagementRouteNames.ApproveVacancy, FormMethod.Post))
    {
        
            
            #line default
            #line hidden
            
            #line 19 "..\..\Views\Vacancy\Review.cshtml"
   Write(Html.HiddenFor(m => m.VacancyReferenceNumber));

            
            #line default
            #line hidden
            
            #line 19 "..\..\Views\Vacancy\Review.cshtml"
                                                      


            
            #line default
            #line hidden
WriteLiteral("        <section>\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                <button");

WriteLiteral(" id=\"btnApprove\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(" name=\"VacancyQAAction\"");

WriteLiteral(" value=\"Approve\"");

WriteLiteral(">Approve</button>\r\n                <button");

WriteLiteral(" id=\"btnReject\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button-link\"");

WriteLiteral(" name=\"VacancyQAAction\"");

WriteLiteral(" value=\"Reject\"");

WriteLiteral(">Refer</button>\r\n                <a");

WriteLiteral(" id=\"dashboardLink\"");

WriteAttribute("href", Tuple.Create(" href=\"", 898), Tuple.Create("\"", 950)
            
            #line 25 "..\..\Views\Vacancy\Review.cshtml"
, Tuple.Create(Tuple.Create("", 905), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(ManagementRouteNames.Dashboard)
            
            #line default
            #line hidden
, 905), false)
);

WriteLiteral(">Return to dashboard</a>\r\n            </div>\r\n        </section>\r\n");

            
            #line 28 "..\..\Views\Vacancy\Review.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>");

        }
    }
}
#pragma warning restore 1591
