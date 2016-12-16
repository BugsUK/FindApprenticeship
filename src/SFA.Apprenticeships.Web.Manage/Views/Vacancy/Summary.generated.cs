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
    using SFA.Apprenticeships.Infrastructure.Presentation;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Manage;
    
    #line 2 "..\..\Views\Vacancy\Summary.cshtml"
    using SFA.Apprenticeships.Web.Manage.Constants;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\Vacancy\Summary.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Vacancy/Summary.cshtml")]
    public partial class Summary : System.Web.Mvc.WebViewPage<FurtherVacancyDetailsViewModel>
    {
        public Summary()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 5 "..\..\Views\Vacancy\Summary.cshtml"
 using (Html.BeginRouteForm(ManagementRouteNames.Summary, FormMethod.Post, new { id = "vacancy-summary-form" }))
{
    
            
            #line default
            #line hidden
            
            #line 7 "..\..\Views\Vacancy\Summary.cshtml"
Write(Html.HiddenFor(m => m.AcceptWarnings));

            
            #line default
            #line hidden
            
            #line 7 "..\..\Views\Vacancy\Summary.cshtml"
                                          

    
            
            #line default
            #line hidden
            
            #line 9 "..\..\Views\Vacancy\Summary.cshtml"
Write(Html.DisplayFor(m => m, FurtherVacancyDetailsViewModel.PartialView));

            
            #line default
            #line hidden
            
            #line 9 "..\..\Views\Vacancy\Summary.cshtml"
                                                                        


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"column-one-half\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(">Save</button>\r\n");

WriteLiteral("        ");

            
            #line 13 "..\..\Views\Vacancy\Summary.cshtml"
   Write(Html.RouteLink("Cancel", ManagementRouteNames.ReviewVacancy, new { vacancyReferenceNumber = Model.VacancyReferenceNumber }));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n");

            
            #line 15 "..\..\Views\Vacancy\Summary.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 19 "..\..\Views\Vacancy\Summary.cshtml"
Write(Html.Partial("HtmlTextEditor"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

});

        }
    }
}
#pragma warning restore 1591
