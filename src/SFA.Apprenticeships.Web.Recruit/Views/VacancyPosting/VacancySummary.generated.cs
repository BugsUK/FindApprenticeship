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
    
    #line 2 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 3 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Infrastructure.Presentation;
    using SFA.Apprenticeships.Web.Common.Constants;
    
    #line 5 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
    using SFA.Apprenticeships.Web.Common.Extensions;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    #line 6 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
    using SFA.Apprenticeships.Web.Common.Validators.Extensions;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.Extensions;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates;
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.EditorTemplates;
    using SFA.Apprenticeships.Web.Recruit;
    
    #line 4 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
    using SFA.Apprenticeships.Web.Recruit.Constants;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/VacancyPosting/VacancySummary.cshtml")]
    public partial class VacancySummary : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy.VacancySummaryViewModel>
    {
        public VacancySummary()
        {
        }
        public override void Execute()
        {
            
            #line 8 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
  
    ViewBag.Title = "Recruit an Apprentice - Enter further details";

    var saveButtonText = "Save and continue";
    var saveButtonValue = "VacancySummary";

    if (Model.Status == ProviderVacancyStatuses.RejectedByQA || Model.ComeFromPreview)
    {
        saveButtonText = "Save and return to Preview";
        saveButtonValue = "VacancySummaryAndPreview";
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">\r\n    Enter further details\r\n</h1>\r\n\r\n");

            
            #line 25 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
 using (Html.BeginRouteForm(RecruitmentRouteNames.VacancySummary, FormMethod.Post))
{
    
            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                            
    
            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                                                           

    
            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
Write(Html.HiddenFor(m => m.VacancyReferenceNumber));

            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                                                  
    
            
            #line default
            #line hidden
            
            #line 31 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
Write(Html.HiddenFor(m => m.Status));

            
            #line default
            #line hidden
            
            #line 31 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                                  
    
            
            #line default
            #line hidden
            
            #line 32 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
Write(Html.HiddenFor(m => m.WarningsHash));

            
            #line default
            #line hidden
            
            #line 32 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                                        
    
            
            #line default
            #line hidden
            
            #line 33 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
Write(Html.HiddenFor(m => m.ComeFromPreview));

            
            #line default
            #line hidden
            
            #line 33 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                                           

            
            #line default
            #line hidden
WriteLiteral("    <section>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <fieldset");

WriteLiteral(" class=\"form-group inline-fixed\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 37 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
           Write(Html.FormTextAreaFor(m => m.WorkingWeek, containerHtmlAttributes: new {@baseClassName = "working-week"}, controlHtmlAttributes: new {@class = "width-all-1-1", type = "text", size = 12}));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </fieldset>\r\n\r\n            <fieldset");

WriteLiteral(" class=\"form-group inline-fixed\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 41 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
           Write(Html.FormTextFor(m => m.HoursPerWeek, controlHtmlAttributes: new { @class = "form-control-small", type = "tel", size = 12 }, containerHtmlAttributes: new {style = "margin-bottom: 15px"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </fieldset>\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 43 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                               Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.WorkingWeekComment)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n            <div");

WriteLiteral(" class=\"blocklabel-single-container\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <a");

WriteAttribute("name", Tuple.Create(" name=\"", 2073), Tuple.Create("\"", 2131)
            
            #line 46 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
, Tuple.Create(Tuple.Create("", 2080), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.WageType).ToString().ToLower()
            
            #line default
            #line hidden
, 2080), false)
);

WriteLiteral("></a>\r\n                    <label");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(" for=\"weekly-wage\"");

WriteLiteral(">Wage</label>\r\n                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 2241), Tuple.Create("\"", 2381)
, Tuple.Create(Tuple.Create("", 2249), Tuple.Create("form-group", 2249), true)
            
            #line 48 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
, Tuple.Create(Tuple.Create(" ", 2259), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.WageType))
            
            #line default
            #line hidden
, 2260), false)
);

WriteLiteral(" data-editable-x=\"\"");

WriteLiteral(">\r\n                        ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"custom-wage\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(" data-target=\"custom-wage-panel\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 51 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                       Write(Html.RadioButtonFor(model => model.WageType, WageType.Custom, new {id = "custom-wage", aria_controls = "wage-type-panel"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            Custom wage\r\n                        </label>\r\n    " +
"                    ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"national-minimum-wage\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 56 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                       Write(Html.RadioButtonFor(model => model.WageType, WageType.NationalMinimumWage, new {id = "national-minimum-wage", aria_controls = "wage-type-panel"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            National Minimum Wage\r\n                        </la" +
"bel>\r\n                        ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"apprenticeship-minimum-wage\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 61 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                       Write(Html.RadioButtonFor(model => model.WageType, WageType.ApprenticeshipMinimumWage, new {id = "apprenticeship-minimum-wage", aria_controls = "wage-type-panel"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            National Minimum Wage for apprentices\r\n            " +
"            </label>\r\n");

WriteLiteral("                        ");

            
            #line 64 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                   Write(Html.ValidationMessageFor(m => m.WageType));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                    <div");

WriteLiteral(" id=\"custom-wage-panel\"");

WriteLiteral(" class=\"toggle-content blocklabel-content\"");

WriteLiteral(">\r\n                        <fieldset");

WriteLiteral(" class=\"form-group inline-fixed\"");

WriteLiteral(">\r\n                            £\r\n");

WriteLiteral("                            ");

            
            #line 69 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                       Write(Html.FormTextFor(m => m.Wage, containerHtmlAttributes: new {@class = "form-group-compound"}, labelHtmlAttributes: new {style = "Display: none"}, controlHtmlAttributes: new {@class = "form-control-large", type = "tel", size = 12}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                            ");

            
            #line 70 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                       Write(Html.DropDownListFor(m => m.WageUnit, Model.WageUnits));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </fieldset>\r\n                    </div>\r\n              " +
"  </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 74 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                                   Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.WageComment)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n            </div>\r\n\r\n            <fieldset");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 4598), Tuple.Create("\"", 4738)
, Tuple.Create(Tuple.Create("", 4606), Tuple.Create("form-group", 4606), true)
            
            #line 78 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
, Tuple.Create(Tuple.Create(" ", 4616), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.Duration))
            
            #line default
            #line hidden
, 4617), false)
);

WriteLiteral(">\r\n                    <a");

WriteAttribute("name", Tuple.Create(" name=\"", 4764), Tuple.Create("\"", 4822)
            
            #line 79 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
, Tuple.Create(Tuple.Create("", 4771), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.Duration).ToString().ToLower()
            
            #line default
            #line hidden
, 4771), false)
);

WriteLiteral("></a>\r\n");

WriteLiteral("                    ");

            
            #line 80 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
               Write(Html.LabelFor(m => m.Duration, new {@class = "form-label"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 81 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
               Write(Html.TextBoxFor(m => m.Duration, new {@class = "form-control-large form-control", type = "tel", size = 12}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 82 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
               Write(Html.DropDownListFor(m => m.DurationType, Model.DurationTypes));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 83 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
               Write(Html.ValidationMessageWithSeverityFor(m => m.Duration, Html.GetValidationType(m => m.Duration)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n            </fieldset>\r\n            <fieldset");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 87 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
           Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.DurationComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </fieldset>\r\n\r\n            <fieldset");

WriteAttribute("class", Tuple.Create(" class=\"", 5478), Tuple.Create("\"", 5665)
, Tuple.Create(Tuple.Create("", 5486), Tuple.Create("form-group", 5486), true)
, Tuple.Create(Tuple.Create(" ", 5496), Tuple.Create("inline-fixed", 5497), true)
, Tuple.Create(Tuple.Create(" ", 5509), Tuple.Create("date-input", 5510), true)
            
            #line 90 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
, Tuple.Create(Tuple.Create(" ", 5520), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.VacancyDatesViewModel.ClosingDate))
            
            #line default
            #line hidden
, 5521), false)
);

WriteLiteral(">\r\n                <a");

WriteAttribute("name", Tuple.Create(" name=\"", 5687), Tuple.Create("\"", 5788)
            
            #line 91 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
, Tuple.Create(Tuple.Create("", 5694), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.VacancyDatesViewModel.ClosingDate).ToString().Replace(".", "_").ToLower()
            
            #line default
            #line hidden
, 5694), false)
);

WriteLiteral("></a>\r\n                <legend");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">");

            
            #line 92 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                                      Write(Model.GetMetadata(m => m.VacancyDatesViewModel.ClosingDate).DisplayName);

            
            #line default
            #line hidden
WriteLiteral("</legend>\r\n");

WriteLiteral("                ");

            
            #line 93 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
           Write(Html.EditorFor(m => m.VacancyDatesViewModel.ClosingDate));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 94 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
           Write(Html.ValidationMessageWithSeverityFor(m => m.VacancyDatesViewModel.ClosingDate, Html.GetValidationType(m => m.VacancyDatesViewModel.ClosingDate)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </fieldset>\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 96 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                               Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.VacancyDatesViewModel.ClosingDateComment)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n            <fieldset");

WriteAttribute("class", Tuple.Create(" class=\"", 6351), Tuple.Create("\"", 6544)
, Tuple.Create(Tuple.Create("", 6359), Tuple.Create("form-group", 6359), true)
, Tuple.Create(Tuple.Create(" ", 6369), Tuple.Create("inline-fixed", 6370), true)
, Tuple.Create(Tuple.Create(" ", 6382), Tuple.Create("date-input", 6383), true)
            
            #line 97 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
, Tuple.Create(Tuple.Create(" ", 6393), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.VacancyDatesViewModel.PossibleStartDate))
            
            #line default
            #line hidden
, 6394), false)
);

WriteLiteral(">\r\n                <a");

WriteAttribute("name", Tuple.Create(" name=\"", 6566), Tuple.Create("\"", 6673)
            
            #line 98 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
, Tuple.Create(Tuple.Create("", 6573), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.VacancyDatesViewModel.PossibleStartDate).ToString().Replace(".", "_").ToLower()
            
            #line default
            #line hidden
, 6573), false)
);

WriteLiteral("></a>\r\n                <legend");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">");

            
            #line 99 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                                      Write(Model.GetMetadata(m => m.VacancyDatesViewModel.PossibleStartDate).DisplayName);

            
            #line default
            #line hidden
WriteLiteral("</legend>\r\n");

WriteLiteral("                ");

            
            #line 100 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
           Write(Html.EditorFor(m => m.VacancyDatesViewModel.PossibleStartDate));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 101 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
           Write(Html.ValidationMessageWithSeverityFor(m => m.VacancyDatesViewModel.PossibleStartDate, Html.GetValidationType(m => m.VacancyDatesViewModel.PossibleStartDate)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </fieldset>\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 103 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                               Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.VacancyDatesViewModel.PossibleStartDateComment)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");

WriteLiteral("            ");

            
            #line 104 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
       Write(Html.FormTextAreaFor(m => m.LongDescription, controlHtmlAttributes: new {@class = "width-all-1-1 form-textarea-large", type = "text", size = 12, rows = 22}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 105 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
       Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.LongDescriptionComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </section>\r\n");

WriteLiteral("    <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" id=\"vacancySummaryButton\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(" name=\"VacancySummary\"");

WriteAttribute("value", Tuple.Create(" value=\"", 7669), Tuple.Create("\"", 7693)
            
            #line 109 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                    , Tuple.Create(Tuple.Create("", 7677), Tuple.Create<System.Object, System.Int32>(saveButtonValue
            
            #line default
            #line hidden
, 7677), false)
);

WriteLiteral(">");

            
            #line 109 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                                                                                                                 Write(saveButtonText);

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n        <button");

WriteLiteral(" id=\"vacancySummaryAndExit\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button-link\"");

WriteLiteral(" name=\"VacancySummary\"");

WriteLiteral(" value=\"VacancySummaryAndExit\"");

WriteLiteral(">Save and exit</button>\r\n");

            
            #line 111 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
        
            
            #line default
            #line hidden
            
            #line 111 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
         if (Model.ComeFromPreview)
        {
            
            
            #line default
            #line hidden
            
            #line 113 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
       Write(Html.RouteLink("Cancel", RecruitmentRouteNames.PreviewVacancy, new { vacancyReferenceNumber = Model.VacancyReferenceNumber }));

            
            #line default
            #line hidden
            
            #line 113 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
                                                                                                                                          
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");

            
            #line 116 "..\..\Views\VacancyPosting\VacancySummary.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
