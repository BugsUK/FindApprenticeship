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

namespace SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates.Vacancy
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
    
    #line 2 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Raa.Vacancies;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Infrastructure.Presentation;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Raa.Common;
    
    #line 3 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.Extensions;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/Vacancy/RequirementsProspects.cshtml")]
    public partial class RequirementsProspects : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy.VacancyRequirementsProspectsViewModel>
    {
        public RequirementsProspects()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 5 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
  
    ViewBag.Title = "Recruit an Apprentice - Enter vacancy requirements and prospects";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">\r\n    Requirements and prospects\r\n</h1>\r\n\r\n");

            
            #line 13 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 14 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 16 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
Write(Html.HiddenFor(m => m.VacancyReferenceNumber));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 17 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
Write(Html.HiddenFor(m => m.Status));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 18 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
Write(Html.HiddenFor(m => m.VacancyType));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 19 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
Write(Html.HiddenFor(m => m.ComeFromPreview));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 20 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
Write(Html.HiddenFor(m => m.VacancySource));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<section>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 24 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.FormTextAreaFor(m => m.DesiredSkills, controlHtmlAttributes: new {@class = "ckeditor", type = "text", id = "DesiredSkills"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 25 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.EditorFor(m => m.DesiredSkillsComment, "Comment", Html.GetLabelFor(m => m.DesiredSkillsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 26 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.ValidationMessageFor(m => m.DesiredSkillsComment));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 27 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.FormTextAreaFor(m => m.PersonalQualities, controlHtmlAttributes: new {@class = "ckeditor", type = "text", id = "PersonalQualities"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 28 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.EditorFor(m => m.PersonalQualitiesComment, "Comment", Html.GetLabelFor(m => m.PersonalQualitiesComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 29 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.ValidationMessageFor(m => m.PersonalQualitiesComment));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 30 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
        
            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
         if (Model.VacancyType == VacancyType.Traineeship)
        {
            
            
            #line default
            #line hidden
            
            #line 32 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
       Write(Html.HiddenFor(m => m.DesiredQualifications));

            
            #line default
            #line hidden
            
            #line 32 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
                                                         
            
            
            #line default
            #line hidden
            
            #line 33 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
       Write(Html.HiddenFor(m => m.DesiredQualificationsComment));

            
            #line default
            #line hidden
            
            #line 33 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
                                                                
        }
        else
        {
            
            
            #line default
            #line hidden
            
            #line 37 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
       Write(Html.FormTextAreaFor(m => m.DesiredQualifications, controlHtmlAttributes: new {@class = "ckeditor", type = "text", id = "DesiredQualifications"}));

            
            #line default
            #line hidden
            
            #line 37 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
                                                                                                                                                              
            
            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
       Write(Html.EditorFor(m => m.DesiredQualificationsComment, "Comment", Html.GetLabelFor(m => m.DesiredQualificationsComment)));

            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
                                                                                                                                  
            
            
            #line default
            #line hidden
            
            #line 39 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
       Write(Html.ValidationMessageFor(m => m.DesiredQualificationsComment));

            
            #line default
            #line hidden
            
            #line 39 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
                                                                           
        }

            
            #line default
            #line hidden
WriteLiteral("        ");

            
            #line 41 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.FormTextAreaFor(m => m.FutureProspects, controlHtmlAttributes: new {@class = "form-control form-control-3-4 form-textarea-medium", type = "text"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 42 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.EditorFor(m => m.FutureProspectsComment, "Comment", Html.GetLabelFor(m => m.FutureProspectsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 43 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.ValidationMessageFor(m => m.FutureProspectsComment));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 44 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.FormTextAreaFor(m => m.ThingsToConsider, controlHtmlAttributes: new {@class = "form-control form-control-3-4 form-textarea-medium", type = "text"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 45 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.EditorFor(m => m.ThingsToConsiderComment, "Comment", Html.GetLabelFor(m => m.ThingsToConsiderComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 46 "..\..\Views\Shared\DisplayTemplates\Vacancy\RequirementsProspects.cshtml"
   Write(Html.ValidationMessageFor(m => m.ThingsToConsiderComment));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n</section>\r\n");

        }
    }
}
#pragma warning restore 1591
