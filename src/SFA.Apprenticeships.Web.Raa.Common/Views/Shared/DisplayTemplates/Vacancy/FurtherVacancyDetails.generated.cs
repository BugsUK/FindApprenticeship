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
    using SFA.Apprenticeships.Domain.Entities.Raa.Vacancies;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Infrastructure.Presentation;
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
    public partial class FurtherVacancyDetails : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy.FurtherVacancyDetailsViewModel>
    {
        public FurtherVacancyDetails()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 9 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
  
    var heading = Model.VacancyType == SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType.Traineeship ? "Enter opportunity details" : "Enter further details";
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
WriteLiteral("\r\n");

            
            #line 24 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
Write(Html.HiddenFor(m => m.VacancySource));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<section>\r\n    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <fieldset");

WriteLiteral(" class=\"form-group inline-fixed\"");

WriteLiteral(">\r\n");

            
            #line 29 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
            
            
            #line default
            #line hidden
            
            #line 29 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
              
                var workingWeeklabelText = Model.VacancyType == SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType.Traineeship ? VacancyViewModelMessages.WorkingWeek.TraineeshipLabelText : null;
                var workingWeekDataValLength = Model.VacancyType == SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType.Traineeship ? VacancyViewModelMessages.WorkingWeek.TraineeshipTooLongErrorText : VacancyViewModelMessages.WorkingWeek.TooLongErrorText;
                var workingWeekDataValRegex = Model.VacancyType == SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType.Traineeship ? VacancyViewModelMessages.WorkingWeek.TraineeshipWhiteListErrorText : VacancyViewModelMessages.WorkingWeek.WhiteListErrorText;
            
            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 34 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.FormTextAreaFor(m => m.WorkingWeek, workingWeeklabelText, containerHtmlAttributes: new {@baseClassName = "working-week"}, controlHtmlAttributes: new {@class = "width-all-1-1", type = "text", size = 12, data_val_length = workingWeekDataValLength, data_val_regex = workingWeekDataValRegex}));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </fieldset>\r\n");

            
            #line 36 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
        
            
            #line default
            #line hidden
            
            #line 36 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
         if (Model.VacancyType == SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType.Traineeship)
        {
            
            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.HiddenFor(m => m.HoursPerWeek));

            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                                
        }
        else
        {

            
            #line default
            #line hidden
WriteLiteral("            <fieldset");

WriteLiteral(" class=\"form-group inline-fixed\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 43 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
           Write(Html.FormTextFor(m => m.HoursPerWeek, controlHtmlAttributes: new {@class = "form-control-small", type = "tel", size = 12}, containerHtmlAttributes: new {style = "margin-bottom: 15px"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </fieldset>\r\n");

            
            #line 45 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 46 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                           Write(Html.EditorFor(m => m.WorkingWeekComment, "Comment", Html.GetLabelFor(m => m.WorkingWeekComment, Model.VacancyType == SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType.Traineeship ? VacancyViewModelMessages.WorkingWeekComment.TraineeshipLabelText : null)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n\r\n");

            
            #line 48 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
        
            
            #line default
            #line hidden
            
            #line 48 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
         if (Model.VacancyType == SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType.Traineeship)
        {
            
            
            #line default
            #line hidden
            
            #line 50 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.HiddenFor(m => m.WageObject.Type));

            
            #line default
            #line hidden
            
            #line 50 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                                   
            
            
            #line default
            #line hidden
            
            #line 51 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.HiddenFor(m => m.Wage));

            
            #line default
            #line hidden
            
            #line 51 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                        
            
            
            #line default
            #line hidden
            
            #line 52 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.HiddenFor(m => m.WageObject.Unit));

            
            #line default
            #line hidden
            
            #line 52 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                                   
            
            
            #line default
            #line hidden
            
            #line 53 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.HiddenFor(m => m.WageComment));

            
            #line default
            #line hidden
            
            #line 53 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                               
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

WriteAttribute("name", Tuple.Create(" name=\"", 3536), Tuple.Create("\"", 3601)
            
            #line 59 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 3543), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.WageObject.Type).ToString().ToLower()
            
            #line default
            #line hidden
, 3543), false)
);

WriteLiteral("></a>\r\n                    <label");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(" for=\"weekly-wage\"");

WriteLiteral(">Wage</label>\r\n                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 3711), Tuple.Create("\"", 3858)
, Tuple.Create(Tuple.Create("", 3719), Tuple.Create("form-group", 3719), true)
            
            #line 61 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 3729), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.WageObject.Type))
            
            #line default
            #line hidden
, 3730), false)
);

WriteLiteral(" data-editable-x=\"\"");

WriteLiteral(">\r\n                        ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"custom-wage\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(" data-target=\"custom-wage-panel\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 64 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                       Write(Html.RadioButtonFor(model => model.WageObject.Type, WageType.Custom, new {id = "custom-wage", aria_controls = "wage-type-panel"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            Custom wage\r\n                        </label>\r\n    " +
"                    ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"national-minimum-wage\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 69 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                       Write(Html.RadioButtonFor(model => model.WageObject.Type, WageType.NationalMinimum, new {id = "national-minimum-wage", aria_controls = "wage-type-panel"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            National Minimum Wage\r\n                        </la" +
"bel>\r\n                        ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"apprenticeship-minimum-wage\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 74 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                       Write(Html.RadioButtonFor(model => model.WageObject.Type, WageType.ApprenticeshipMinimum, new {id = "apprenticeship-minimum-wage", aria_controls = "wage-type-panel"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            National Minimum Wage for apprentices\r\n            " +
"            </label>\r\n");

WriteLiteral("                        ");

            
            #line 77 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                   Write(Html.ValidationMessageFor(m => m.WageObject.Type));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                    <div");

WriteLiteral(" id=\"custom-wage-panel\"");

WriteLiteral(" class=\"toggle-content blocklabel-content\"");

WriteLiteral(">\r\n                        <fieldset");

WriteLiteral(" class=\"form-group inline-fixed\"");

WriteLiteral(">\r\n                            £\r\n");

WriteLiteral("                            ");

            
            #line 82 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                       Write(Html.FormTextFor(m => m.Wage, containerHtmlAttributes: new {@class = "form-group-compound"}, labelHtmlAttributes: new {style = "Display: none"}, controlHtmlAttributes: new {@class = "form-control-large", type = "tel", size = 12}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                            ");

            
            #line 83 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                       Write(Html.DropDownListFor(m => m.WageObject.Unit, Model.WageUnits));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </fieldset>\r\n                    </div>\r\n              " +
"  </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 87 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                   Write(Html.EditorFor(m => m.WageComment, "Comment", Html.GetLabelFor(m => m.WageComment)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n            </div>\r\n");

            
            #line 89 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("\r\n        <fieldset");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <div");

WriteAttribute("class", Tuple.Create(" class=\"", 6118), Tuple.Create("\"", 6258)
, Tuple.Create(Tuple.Create("", 6126), Tuple.Create("form-group", 6126), true)
            
            #line 92 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 6136), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.Duration))
            
            #line default
            #line hidden
, 6137), false)
);

WriteLiteral(">\r\n                <a");

WriteAttribute("name", Tuple.Create(" name=\"", 6280), Tuple.Create("\"", 6338)
            
            #line 93 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 6287), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.Duration).ToString().ToLower()
            
            #line default
            #line hidden
, 6287), false)
);

WriteLiteral("></a>\r\n");

WriteLiteral("                ");

            
            #line 94 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
           Write(Html.LabelFor(m => m.Duration, new {@class = "form-label"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 95 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
           Write(Html.TextBoxFor(m => m.Duration, new {@class = "form-control-large form-control", type = "tel", size = 12}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 96 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
           Write(Html.DropDownListFor(m => m.DurationType, Model.DurationTypes));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 97 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
           Write(Html.ValidationMessageWithSeverityFor(m => m.Duration, Html.GetValidationType(m => m.Duration)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n");

            
            #line 100 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
            
            
            #line default
            #line hidden
            
            #line 100 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
             if (Model.VacancySource != VacancySource.Raa )
            {
                
            
            #line default
            #line hidden
            
            #line 102 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
           Write(Html.FormTextAreaFor(model => model.ExpectedDuration, controlHtmlAttributes: new { type = "text", @class = "form-control-1-1" }));

            
            #line default
            #line hidden
            
            #line 102 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                                                                                                                                 
            }

            
            #line default
            #line hidden
WriteLiteral("        </fieldset>\r\n        <fieldset");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 106 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.EditorFor(m => m.DurationComment, "Comment", Html.GetLabelFor(m => m.DurationComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </fieldset>\r\n\r\n        <fieldset");

WriteAttribute("class", Tuple.Create(" class=\"", 7211), Tuple.Create("\"", 7398)
, Tuple.Create(Tuple.Create("", 7219), Tuple.Create("form-group", 7219), true)
, Tuple.Create(Tuple.Create(" ", 7229), Tuple.Create("inline-fixed", 7230), true)
, Tuple.Create(Tuple.Create(" ", 7242), Tuple.Create("date-input", 7243), true)
            
            #line 109 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 7253), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.VacancyDatesViewModel.ClosingDate))
            
            #line default
            #line hidden
, 7254), false)
);

WriteLiteral(">\r\n            <a");

WriteAttribute("name", Tuple.Create(" name=\"", 7416), Tuple.Create("\"", 7517)
            
            #line 110 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 7423), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.VacancyDatesViewModel.ClosingDate).ToString().Replace(".", "_").ToLower()
            
            #line default
            #line hidden
, 7423), false)
);

WriteLiteral("></a>\r\n            <legend");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">");

            
            #line 111 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                  Write(Model.GetMetadata(m => m.VacancyDatesViewModel.ClosingDate).DisplayName);

            
            #line default
            #line hidden
WriteLiteral("</legend>\r\n");

WriteLiteral("            ");

            
            #line 112 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.EditorFor(m => m.VacancyDatesViewModel.ClosingDate));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 113 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.ValidationMessageWithSeverityFor(m => m.VacancyDatesViewModel.ClosingDate, Html.GetValidationType(m => m.VacancyDatesViewModel.ClosingDate)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </fieldset>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 115 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                           Write(Html.EditorFor(m => m.VacancyDatesViewModel.ClosingDateComment, "Comment", Html.GetLabelFor(m => m.VacancyDatesViewModel.ClosingDateComment)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n\r\n        <fieldset");

WriteAttribute("class", Tuple.Create(" class=\"", 8100), Tuple.Create("\"", 8293)
, Tuple.Create(Tuple.Create("", 8108), Tuple.Create("form-group", 8108), true)
, Tuple.Create(Tuple.Create(" ", 8118), Tuple.Create("inline-fixed", 8119), true)
, Tuple.Create(Tuple.Create(" ", 8131), Tuple.Create("date-input", 8132), true)
            
            #line 117 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 8142), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.VacancyDatesViewModel.PossibleStartDate))
            
            #line default
            #line hidden
, 8143), false)
);

WriteLiteral(">\r\n            <a");

WriteAttribute("name", Tuple.Create(" name=\"", 8311), Tuple.Create("\"", 8418)
            
            #line 118 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 8318), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.VacancyDatesViewModel.PossibleStartDate).ToString().Replace(".", "_").ToLower()
            
            #line default
            #line hidden
, 8318), false)
);

WriteLiteral("></a>\r\n            <legend");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">");

            
            #line 119 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                                  Write(Model.GetMetadata(m => m.VacancyDatesViewModel.PossibleStartDate).DisplayName);

            
            #line default
            #line hidden
WriteLiteral("</legend>\r\n");

WriteLiteral("            ");

            
            #line 120 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.EditorFor(m => m.VacancyDatesViewModel.PossibleStartDate));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 121 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
       Write(Html.ValidationMessageWithSeverityFor(m => m.VacancyDatesViewModel.PossibleStartDate, Html.GetValidationType(m => m.VacancyDatesViewModel.PossibleStartDate)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </fieldset>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">");

            
            #line 123 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
                           Write(Html.EditorFor(m => m.VacancyDatesViewModel.PossibleStartDateComment, "Comment", Html.GetLabelFor(m => m.VacancyDatesViewModel.PossibleStartDateComment)));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n");

            
            #line 124 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
        
            
            #line default
            #line hidden
            
            #line 124 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
          
            var longDescriptionlabelText = Model.VacancyType == SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType.Traineeship ? VacancyViewModelMessages.LongDescription.TraineeshipLabelText : null;
            var longDescriptionDataValLength = Model.VacancyType == SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType.Traineeship ? VacancyViewModelMessages.LongDescription.TraineeshipTooLongErrorText : VacancyViewModelMessages.LongDescription.TooLongErrorText;
            var longDescriptionDataValRegex = Model.VacancyType == SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType.Traineeship ? VacancyViewModelMessages.LongDescription.TraineeshipWhiteListInvalidCharacterErrorText : VacancyViewModelMessages.LongDescription.WhiteListInvalidCharacterErrorText;
        
            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 129 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
   Write(Html.FormTextAreaFor(m => m.LongDescription, longDescriptionlabelText, controlHtmlAttributes: new {id = "LongDescription", @class = "ckeditor", type = "text", size = 12, rows = 22, data_val_length = longDescriptionDataValLength, data_val_regex = longDescriptionDataValRegex}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 130 "..\..\Views\Shared\DisplayTemplates\Vacancy\FurtherVacancyDetails.cshtml"
   Write(Html.EditorFor(m => m.LongDescriptionComment, "Comment", Html.GetLabelFor(m => m.LongDescriptionComment, Model.VacancyType == SFA.Apprenticeships.Domain.Entities.Raa.Vacancies.VacancyType.Traineeship ? VacancyViewModelMessages.LongDescriptionComment.TraineeshipLabelText : null)));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n</section>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
