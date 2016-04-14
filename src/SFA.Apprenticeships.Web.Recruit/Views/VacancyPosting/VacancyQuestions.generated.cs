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

namespace SFA.Apprenticeships.Web.Recruit.Views.VacancyPosting
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
    
    #line 2 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Raa.Vacancies;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Infrastructure.Presentation;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    #line 3 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates;
    using SFA.Apprenticeships.Web.Recruit;
    
    #line 4 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
    using SFA.Apprenticeships.Web.Recruit.Constants;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/VacancyPosting/VacancyQuestions.cshtml")]
    public partial class VacancyQuestions : System.Web.Mvc.WebViewPage<VacancyQuestionsViewModel>
    {
        public VacancyQuestions()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 6 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
 using (Html.BeginRouteForm(RecruitmentRouteNames.VacancyQuestions, FormMethod.Post))
{
    
            
            #line default
            #line hidden
            
            #line 8 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
Write(Html.DisplayFor(m => m, VacancyQuestionsViewModel.PartialView));

            
            #line default
            #line hidden
            
            #line 8 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
                                                                   

    var saveButtonText = (Model.Status == VacancyStatus.Referred || Model.ComeFromPreview) ? "Save and return to Preview" : "Save and preview " + (Model.VacancyType == VacancyType.Traineeship ? "opportunity" : "vacancy");


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(" name=\"VacancyQuestions\"");

WriteLiteral(" value=\"VacancyQuestions\"");

WriteLiteral(">");

            
            #line 13 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
                                                                                         Write(saveButtonText);

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n        <button");

WriteLiteral(" id=\"VacancyQuestionsAndExit\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button-link\"");

WriteLiteral(" name=\"VacancyQuestions\"");

WriteLiteral(" value=\"VacancyQuestionsAndExit\"");

WriteLiteral(">Save and exit</button>\r\n");

            
            #line 15 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
        
            
            #line default
            #line hidden
            
            #line 15 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
         if (Model.ComeFromPreview)
        {
            
            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
       Write(Html.RouteLink("Cancel", RecruitmentRouteNames.PreviewVacancy, new { vacancyReferenceNumber = Model.VacancyReferenceNumber }));

            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
                                                                                                                                          
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");

            
            #line 20 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 24 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
Write(Scripts.Render("~/bundles/autosave"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    <script>\r\n        ");

WriteLiteral("\r\n\r\n        $(window).on(\'load\', function() {\r\n            autoSave.initialise({\r" +
"\n                formSelector: \"form\",\r\n                timeout: 10000,\r\n       " +
"         postUrl: \'");

            
            #line 33 "..\..\Views\VacancyPosting\VacancyQuestions.cshtml"
                     Write(Url.RouteUrl(RecruitmentRouteNames.AutoSaveVacancyQuestions));

            
            #line default
            #line hidden
WriteLiteral("\'\r\n            });\r\n        });\r\n    </script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
