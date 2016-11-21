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
    
    #line 2 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 3 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Raa.Vacancies;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Infrastructure.Presentation;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    #line 4 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates;
    using SFA.Apprenticeships.Web.Recruit;
    
    #line 5 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
    using SFA.Apprenticeships.Web.Recruit.Constants;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/VacancyPosting/ConfirmEmployer.cshtml")]
    public partial class ConfirmEmployer : System.Web.Mvc.WebViewPage<VacancyOwnerRelationshipViewModel>
    {
        public ConfirmEmployer()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 7 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
  
    ViewBag.Title = "Recruit an Apprentice - Check employer information";

    var saveButtonText = (Model.Status == VacancyStatus.Referred || Model.ComeFromPreview) &&
        Model.IsEmployerLocationMainApprenticeshipLocation.HasValue &&
        Model.IsEmployerLocationMainApprenticeshipLocation.Value == true ? "Save and return to Preview" : "Save and continue";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"grid-row\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"hgroup\"");

WriteLiteral(">\r\n        <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">\r\n            Check employer information\r\n        </h1>\r\n    </div>\r\n</div>\r\n\r\n");

            
            #line 23 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
 using (Html.BeginRouteForm(RecruitmentRouteNames.ConfirmEmployer, FormMethod.Post))
{
    
            
            #line default
            #line hidden
            
            #line 25 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
Write(Html.DisplayFor(m => m, VacancyOwnerRelationshipViewModel.PartialView));

            
            #line default
            #line hidden
            
            #line 25 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
                                                                           


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"column-full\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" class=\"button\"");

WriteLiteral(" id=\"confirmEmployer\"");

WriteLiteral(" name=\"ConfirmEmployer\"");

WriteLiteral(" value=\"ConfirmEmployer\"");

WriteLiteral(">");

            
            #line 28 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
                                                                                              Write(saveButtonText);

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n");

WriteLiteral("        ");

            
            #line 29 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
   Write(Html.RouteLink("Choose a different employer", RecruitmentRouteNames.SelectExistingEmployer, new { providerSiteId = Model.ProviderSiteId, vacancyGuid = Model.VacancyGuid, comeFromPreview = Model.ComeFromPreview }, new { @class = "button-link" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 30 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
        
            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
         if (Model.ComeFromPreview)
        {
            
            
            #line default
            #line hidden
            
            #line 32 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
       Write(Html.RouteLink("Cancel", RecruitmentRouteNames.PreviewVacancy, new { vacancyReferenceNumber = Model.VacancyReferenceNumber }));

            
            #line default
            #line hidden
            
            #line 32 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
                                                                                                                                          
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");

            
            #line 35 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        var comeFromPreview = \"");

            
            #line 39 "..\..\Views\VacancyPosting\ConfirmEmployer.cshtml"
                          Write(Model.ComeFromPreview.ToString());

            
            #line default
            #line hidden
WriteLiteral(@""";

        $(""#NumberOfPositionsJS"").attr(""id"", ""NumberOfPositions"").attr(""Name"", ""NumberOfPositions"");
        $(""#location-type-main-location"").on('click', function () {
            if (comeFromPreview === ""True"") {
                $(""#confirmEmployer"").text(""Save and return to Preview"");
            }
        });

        $(""#location-type-different-location"").on('click', function () {
            $(""#confirmEmployer"").text(""Save and continue"");
        });
    </script>

    ");

WriteLiteral("\r\n");

});

        }
    }
}
#pragma warning restore 1591
