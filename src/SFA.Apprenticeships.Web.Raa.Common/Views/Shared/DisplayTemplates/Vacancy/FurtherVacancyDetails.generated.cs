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
    
    #line 2 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Common.Constants;
    
    #line 4 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
    using SFA.Apprenticeships.Web.Common.Extensions;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    #line 5 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
    using SFA.Apprenticeships.Web.Common.Validators.Extensions;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Raa.Common;
    
    #line 6 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels;
    
    #line default
    #line hidden
    
    #line 7 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.Extensions;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/Vacancy/FurtherVacancyDetails.cshtml")]
    public partial class FurtherVacancyDetails : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy.VacancySummaryViewModel>
    {
        public FurtherVacancyDetails()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 9 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
  
    var heading = Model.VacancyType == VacancyType.Traineeship ? "Enter opportunity details" : "Enter further details";
    ViewBag.Title = "Recruit an Apprentice - " + heading;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1");

WriteLiteral(" id=\"heading\"");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">");

            
            #line 14 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                   Write(heading);

            
            #line default
            #line hidden
WriteLiteral("</h1>\r\n\r\n");

            
            #line 16 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 17 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 19 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
Write(Html.HiddenFor(m => m.VacancyReferenceNumber));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 20 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
Write(Html.HiddenFor(m => m.Status));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 21 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
Write(Html.HiddenFor(m => m.VacancyType));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 22 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
Write(Html.HiddenFor(m => m.WarningsHash));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 23 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
Write(Html.HiddenFor(m => m.ComeFromPreview));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<section>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <fieldset");

WriteLiteral(" class=\"form-group inline-fixed\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 28 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.FormTextAreaFor(m => m.WorkingWeek, Model.VacancyType == VacancyType.Traineeship ? VacancyViewModelMessages.WorkingWeek.TraineeshipLabelText : null, containerHtmlAttributes: new {@baseClassName = "working-week"}, controlHtmlAttributes: new {@class = "width-all-1-1", type = "text", size = 12}));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </fieldset>\r\n");

            
            #line 30 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
        
            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
         if (Model.VacancyType == VacancyType.Traineeship)
        {
            
            
            #line default
            #line hidden
            
            #line 32 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.HiddenFor(m => m.HoursPerWeek));

            
            #line default
            #line hidden
            
            #line 32 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                                
        }
        else
        {

            
            #line default
            #line hidden
WriteLiteral("            <fieldset");

WriteLiteral(" class=\"form-group inline-fixed\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 37 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
           Write(Html.FormTextFor(m => m.HoursPerWeek, controlHtmlAttributes: new {@class = "form-control-small", type = "tel", size = 12}, containerHtmlAttributes: new {style = "margin-bottom: 15px"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </fieldset>\r\n");

            
            #line 39 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 40 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                           Write(Html.EditorFor(m => m.WorkingWeekComment, "Comment", Html.GetLabelFor(m => m.WorkingWeekComment, Model.VacancyType == VacancyType.Traineeship ? VacancyViewModelMessages.WorkingWeekComment.TraineeshipLabelText : null)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n\r\n");

            
            #line 42 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
        
            
            #line default
            #line hidden
            
            #line 42 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
         if (Model.VacancyType == VacancyType.Traineeship)
        {
            
            
            #line default
            #line hidden
            
            #line 44 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.HiddenFor(m => m.WageType));

            
            #line default
            #line hidden
            
            #line 44 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                            
            
            
            #line default
            #line hidden
            
            #line 45 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.HiddenFor(m => m.Wage));

            
            #line default
            #line hidden
            
            #line 45 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                        
            
            
            #line default
            #line hidden
            
            #line 46 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.HiddenFor(m => m.WageUnit));

            
            #line default
            #line hidden
            
            #line 46 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                            
            
            
            #line default
            #line hidden
            
            #line 47 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.HiddenFor(m => m.WageComment));

            
            #line default
            #line hidden
            
            #line 47 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                               
        }
        else
        {

            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteLiteral(" class=\"blocklabel-single-container\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <a");

WriteAttribute("name", Tuple.Create(" name=\"", 2520), Tuple.Create("\"", 2578)
            
            #line 53 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 2527), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.WageType).ToString().ToLower()
            
            #line default
            #line hidden
, 2527), false)
);

WriteLiteral("></a>\r\n                    <label");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(" for=\"weekly-wage\"");

WriteLiteral(">Wage</label>\r\n                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 2688), Tuple.Create("\"", 2828)
, Tuple.Create(Tuple.Create("", 2696), Tuple.Create("form-group", 2696), true)
            
            #line 55 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 2706), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.WageType))
            
            #line default
            #line hidden
, 2707), false)
);

WriteLiteral(" data-editable-x=\"\"");

WriteLiteral(">\r\n                        ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"custom-wage\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(" data-target=\"custom-wage-panel\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 58 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
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

            
            #line 63 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
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

            
            #line 68 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                       Write(Html.RadioButtonFor(model => model.WageType, WageType.ApprenticeshipMinimumWage, new {id = "apprenticeship-minimum-wage", aria_controls = "wage-type-panel"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            National Minimum Wage for apprentices\r\n            " +
"            </label>\r\n");

WriteLiteral("                        ");

            
            #line 71 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
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

            
            #line 76 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                       Write(Html.FormTextFor(m => m.Wage, containerHtmlAttributes: new {@class = "form-group-compound"}, labelHtmlAttributes: new {style = "Display: none"}, controlHtmlAttributes: new {@class = "form-control-large", type = "tel", size = 12}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                            ");

            
            #line 77 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                       Write(Html.DropDownListFor(m => m.WageUnit, Model.WageUnits));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </fieldset>\r\n                    </div>\r\n              " +
"  </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 81 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                   Write(Html.EditorFor(m => m.WageComment, "Comment", Html.GetLabelFor(m => m.WageComment)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n            </div>\r\n");

            
            #line 83 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("\r\n        <fieldset");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <div");

WriteAttribute("class", Tuple.Create(" class=\"", 5061), Tuple.Create("\"", 5201)
, Tuple.Create(Tuple.Create("", 5069), Tuple.Create("form-group", 5069), true)
            
            #line 86 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 5079), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.Duration))
            
            #line default
            #line hidden
, 5080), false)
);

WriteLiteral(">\r\n                <a");

WriteAttribute("name", Tuple.Create(" name=\"", 5223), Tuple.Create("\"", 5281)
            
            #line 87 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 5230), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.Duration).ToString().ToLower()
            
            #line default
            #line hidden
, 5230), false)
);

WriteLiteral("></a>\r\n");

WriteLiteral("                ");

            
            #line 88 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
           Write(Html.LabelFor(m => m.Duration, new {@class = "form-label"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 89 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
           Write(Html.TextBoxFor(m => m.Duration, new {@class = "form-control-large form-control", type = "tel", size = 12}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 90 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
           Write(Html.DropDownListFor(m => m.DurationType, Model.DurationTypes));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 91 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
           Write(Html.ValidationMessageWithSeverityFor(m => m.Duration, Html.GetValidationType(m => m.Duration)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </fieldset>\r\n        <fieldset");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 95 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.EditorFor(m => m.DurationComment, "Comment", Html.GetLabelFor(m => m.DurationComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </fieldset>\r\n\r\n        <fieldset");

WriteAttribute("class", Tuple.Create(" class=\"", 5914), Tuple.Create("\"", 6101)
, Tuple.Create(Tuple.Create("", 5922), Tuple.Create("form-group", 5922), true)
, Tuple.Create(Tuple.Create(" ", 5932), Tuple.Create("inline-fixed", 5933), true)
, Tuple.Create(Tuple.Create(" ", 5945), Tuple.Create("date-input", 5946), true)
            
            #line 98 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 5956), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.VacancyDatesViewModel.ClosingDate))
            
            #line default
            #line hidden
, 5957), false)
);

WriteLiteral(">\r\n            <a");

WriteAttribute("name", Tuple.Create(" name=\"", 6119), Tuple.Create("\"", 6220)
            
            #line 99 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 6126), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.VacancyDatesViewModel.ClosingDate).ToString().Replace(".", "_").ToLower()
            
            #line default
            #line hidden
, 6126), false)
);

WriteLiteral("></a>\r\n            <legend");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">");

            
            #line 100 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                  Write(Model.GetMetadata(m => m.VacancyDatesViewModel.ClosingDate).DisplayName);

            
            #line default
            #line hidden
WriteLiteral("</legend>\r\n");

WriteLiteral("            ");

            
            #line 101 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.EditorFor(m => m.VacancyDatesViewModel.ClosingDate));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 102 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.ValidationMessageWithSeverityFor(m => m.VacancyDatesViewModel.ClosingDate, Html.GetValidationType(m => m.VacancyDatesViewModel.ClosingDate)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </fieldset>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 104 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                           Write(Html.EditorFor(m => m.VacancyDatesViewModel.ClosingDateComment, "Comment", Html.GetLabelFor(m => m.VacancyDatesViewModel.ClosingDateComment)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n\r\n        <fieldset");

WriteAttribute("class", Tuple.Create(" class=\"", 6803), Tuple.Create("\"", 6996)
, Tuple.Create(Tuple.Create("", 6811), Tuple.Create("form-group", 6811), true)
, Tuple.Create(Tuple.Create(" ", 6821), Tuple.Create("inline-fixed", 6822), true)
, Tuple.Create(Tuple.Create(" ", 6834), Tuple.Create("date-input", 6835), true)
            
            #line 106 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 6845), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.VacancyDatesViewModel.PossibleStartDate))
            
            #line default
            #line hidden
, 6846), false)
);

WriteLiteral(">\r\n            <a");

WriteAttribute("name", Tuple.Create(" name=\"", 7014), Tuple.Create("\"", 7121)
            
            #line 107 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 7021), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.VacancyDatesViewModel.PossibleStartDate).ToString().Replace(".", "_").ToLower()
            
            #line default
            #line hidden
, 7021), false)
);

WriteLiteral("></a>\r\n            <legend");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">");

            
            #line 108 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                  Write(Model.GetMetadata(m => m.VacancyDatesViewModel.PossibleStartDate).DisplayName);

            
            #line default
            #line hidden
WriteLiteral("</legend>\r\n");

WriteLiteral("            ");

            
            #line 109 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.EditorFor(m => m.VacancyDatesViewModel.PossibleStartDate));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 110 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.ValidationMessageWithSeverityFor(m => m.VacancyDatesViewModel.PossibleStartDate, Html.GetValidationType(m => m.VacancyDatesViewModel.PossibleStartDate)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </fieldset>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 112 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                           Write(Html.EditorFor(m => m.VacancyDatesViewModel.PossibleStartDateComment, "Comment", Html.GetLabelFor(m => m.VacancyDatesViewModel.PossibleStartDateComment)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");

WriteLiteral("        ");

            
            #line 113 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
   Write(Html.FormTextAreaFor(m => m.LongDescription, Model.VacancyType == VacancyType.Traineeship ? VacancyViewModelMessages.LongDescription.TraineeshipLabelText : null, controlHtmlAttributes: new {@class = "width-all-1-1 form-textarea-large", type = "text", size = 12, rows = 22}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 114 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
   Write(Html.EditorFor(m => m.LongDescriptionComment, "Comment", Html.GetLabelFor(m => m.LongDescriptionComment, Model.VacancyType == VacancyType.Traineeship ? VacancyViewModelMessages.LongDescriptionComment.TraineeshipLabelText : null)));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n</section>\r\n");

        }
    }
}
#pragma warning restore 1591
