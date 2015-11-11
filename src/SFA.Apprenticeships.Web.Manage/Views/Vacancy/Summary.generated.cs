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
    
    #line 2 "..\..\Views\Vacancy\Summary.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Common.Constants;
    
    #line 3 "..\..\Views\Vacancy\Summary.cshtml"
    using SFA.Apprenticeships.Web.Common.Extensions;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    #line 4 "..\..\Views\Vacancy\Summary.cshtml"
    using SFA.Apprenticeships.Web.Common.Validators.Extensions;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Manage;
    
    #line 5 "..\..\Views\Vacancy\Summary.cshtml"
    using SFA.Apprenticeships.Web.Manage.Constants;
    
    #line default
    #line hidden
    
    #line 6 "..\..\Views\Vacancy\Summary.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Vacancy/Summary.cshtml")]
    public partial class Summary : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy.VacancySummaryViewModel>
    {
        public Summary()
        {
        }
        public override void Execute()
        {
            
            #line 7 "..\..\Views\Vacancy\Summary.cshtml"
  
    ViewBag.Title = "Recruit an Apprentice - Enter vacancy summary";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">\r\n    Enter vacancy details\r\n</h1>\r\n\r\n");

            
            #line 15 "..\..\Views\Vacancy\Summary.cshtml"
 using (Html.BeginRouteForm(ManagementRouteNames.Summary, FormMethod.Post))
{
    
            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\Vacancy\Summary.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\Vacancy\Summary.cshtml"
                            
    
            
            #line default
            #line hidden
            
            #line 18 "..\..\Views\Vacancy\Summary.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 18 "..\..\Views\Vacancy\Summary.cshtml"
                                                           

    
            
            #line default
            #line hidden
            
            #line 20 "..\..\Views\Vacancy\Summary.cshtml"
Write(Html.HiddenFor(m => m.VacancyReferenceNumber));

            
            #line default
            #line hidden
            
            #line 20 "..\..\Views\Vacancy\Summary.cshtml"
                                                  
    
            
            #line default
            #line hidden
            
            #line 21 "..\..\Views\Vacancy\Summary.cshtml"
Write(Html.Hidden("acceptWarnings", false));

            
            #line default
            #line hidden
            
            #line 21 "..\..\Views\Vacancy\Summary.cshtml"
                                          //TODO: put in validation summary


            
            #line default
            #line hidden
WriteLiteral("    <section>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <fieldset");

WriteLiteral(" class=\"form-group inline-fixed\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 26 "..\..\Views\Vacancy\Summary.cshtml"
           Write(Html.FormTextAreaFor(m => m.WorkingWeek, containerHtmlAttributes: new {@baseClassName = "working-week"}, controlHtmlAttributes: new {@class = "width-all-1-2", type = "text", size = 12, autofocus = "autofocus"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 27 "..\..\Views\Vacancy\Summary.cshtml"
           Write(Html.FormTextFor(m => m.HoursPerWeek, containerHtmlAttributes: new {@class = "form-group-compound"}, labelHtmlAttributes: new {style = "Display: none"}, controlHtmlAttributes: new {@class = "form-control-small", type = "tel", size = 12}));

            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\Vacancy\Summary.cshtml"
                                                                                                                                                                                                                                                         Write(VacancyViewModelMessages.HoursPerWeek.LabelText);

            
            #line default
            #line hidden
WriteLiteral("\r\n            </fieldset>\r\n\r\n            <div");

WriteLiteral(" class=\"blocklabel-single-container\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <a");

WriteAttribute("name", Tuple.Create(" name=\"", 1614), Tuple.Create("\"", 1672)
            
            #line 32 "..\..\Views\Vacancy\Summary.cshtml"
, Tuple.Create(Tuple.Create("", 1621), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.WageType).ToString().ToLower()
            
            #line default
            #line hidden
, 1621), false)
);

WriteLiteral("></a>\r\n                    <label");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(" for=\"weekly-wage\"");

WriteLiteral(">Wage</label>\r\n                    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" data-editable-x=\"\"");

WriteLiteral(">\r\n                        ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"custom-wage\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(" data-target=\"custom-wage-panel\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 37 "..\..\Views\Vacancy\Summary.cshtml"
                       Write(Html.RadioButtonFor(model => model.WageType, WageType.Custom, new { id = "custom-wage", aria_controls = "wage-type-panel" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            Custom wage\r\n                        </label>\r\n    " +
"                    ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"national-minimum-wage\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 42 "..\..\Views\Vacancy\Summary.cshtml"
                       Write(Html.RadioButtonFor(model => model.WageType, WageType.NationalMinimumWage, new { id = "national-minimum-wage", aria_controls = "wage-type-panel" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            National Minimum Wage\r\n                        </la" +
"bel>\r\n                        ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"apprenticeship-minimum-wage\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 47 "..\..\Views\Vacancy\Summary.cshtml"
                       Write(Html.RadioButtonFor(model => model.WageType, WageType.ApprenticeshipMinimumWage, new { id = "apprenticeship-minimum-wage", aria_controls = "wage-type-panel" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                            Apprentice National Minimum Wage\r\n                 " +
"       </label>\r\n                    </div>\r\n                    <div");

WriteLiteral(" id=\"custom-wage-panel\"");

WriteLiteral(" class=\"toggle-content blocklabel-content\"");

WriteLiteral(">\r\n                        <fieldset");

WriteLiteral(" class=\"form-group inline-fixed\"");

WriteLiteral(">\r\n                            £\r\n");

WriteLiteral("                            ");

            
            #line 54 "..\..\Views\Vacancy\Summary.cshtml"
                       Write(Html.FormTextFor(m => m.Wage, containerHtmlAttributes: new { @class = "form-group-compound" }, labelHtmlAttributes: new { style = "Display: none" }, controlHtmlAttributes: new { @class = "form-control-large", type = "tel", size = 12 }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                            ");

            
            #line 55 "..\..\Views\Vacancy\Summary.cshtml"
                       Write(Html.DropDownListFor(m => m.WageUnit, Model.WageUnits));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </fieldset>\r\n                    </div>\r\n              " +
"  </div>\r\n            </div>\r\n\r\n            <fieldset");

WriteLiteral(" class=\"form-group inline-fixed\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 62 "..\..\Views\Vacancy\Summary.cshtml"
           Write(Html.FormTextFor(m => m.Duration, containerHtmlAttributes: new { @class = "form-group-compound" }, controlHtmlAttributes: new { @class = "form-control-large", type = "tel", size = 12 }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 63 "..\..\Views\Vacancy\Summary.cshtml"
           Write(Html.DropDownListFor(m => m.DurationType, Model.DurationTypes));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </fieldset>\r\n\r\n            <fieldset");

WriteAttribute("class", Tuple.Create(" class=\"", 4162), Tuple.Create("\"", 4288)
, Tuple.Create(Tuple.Create("", 4170), Tuple.Create("form-group", 4170), true)
, Tuple.Create(Tuple.Create(" ", 4180), Tuple.Create("inline-fixed", 4181), true)
, Tuple.Create(Tuple.Create(" ", 4193), Tuple.Create("date-input", 4194), true)
            
            #line 66 "..\..\Views\Vacancy\Summary.cshtml"
, Tuple.Create(Tuple.Create(" ", 4204), Tuple.Create<System.Object, System.Int32>(HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.ClosingDate))
            
            #line default
            #line hidden
, 4205), false)
);

WriteLiteral(">\r\n                <a");

WriteAttribute("name", Tuple.Create(" name=\"", 4310), Tuple.Create("\"", 4371)
            
            #line 67 "..\..\Views\Vacancy\Summary.cshtml"
, Tuple.Create(Tuple.Create("", 4317), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.ClosingDate).ToString().ToLower()
            
            #line default
            #line hidden
, 4317), false)
);

WriteLiteral("></a>\r\n                <legend");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">");

            
            #line 68 "..\..\Views\Vacancy\Summary.cshtml"
                                      Write(Model.GetMetadata(m => m.ClosingDate).DisplayName);

            
            #line default
            #line hidden
WriteLiteral("</legend>\r\n");

WriteLiteral("                ");

            
            #line 69 "..\..\Views\Vacancy\Summary.cshtml"
           Write(Html.EditorFor(m => m.ClosingDate));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 70 "..\..\Views\Vacancy\Summary.cshtml"
           Write(Html.ValidationMessageWithSeverityFor(m => m.ClosingDate, Html.GetValidationType(m => m.ClosingDate)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </fieldset>\r\n            <fieldset");

WriteAttribute("class", Tuple.Create(" class=\"", 4702), Tuple.Create("\"", 4834)
, Tuple.Create(Tuple.Create("", 4710), Tuple.Create("form-group", 4710), true)
, Tuple.Create(Tuple.Create(" ", 4720), Tuple.Create("inline-fixed", 4721), true)
, Tuple.Create(Tuple.Create(" ", 4733), Tuple.Create("date-input", 4734), true)
            
            #line 72 "..\..\Views\Vacancy\Summary.cshtml"
, Tuple.Create(Tuple.Create(" ", 4744), Tuple.Create<System.Object, System.Int32>(HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.PossibleStartDate))
            
            #line default
            #line hidden
, 4745), false)
);

WriteLiteral(">\r\n                <a");

WriteAttribute("name", Tuple.Create(" name=\"", 4856), Tuple.Create("\"", 4923)
            
            #line 73 "..\..\Views\Vacancy\Summary.cshtml"
, Tuple.Create(Tuple.Create("", 4863), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.PossibleStartDate).ToString().ToLower()
            
            #line default
            #line hidden
, 4863), false)
);

WriteLiteral("></a>\r\n                <legend");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">");

            
            #line 74 "..\..\Views\Vacancy\Summary.cshtml"
                                      Write(Model.GetMetadata(m => m.PossibleStartDate).DisplayName);

            
            #line default
            #line hidden
WriteLiteral("</legend>\r\n");

WriteLiteral("                ");

            
            #line 75 "..\..\Views\Vacancy\Summary.cshtml"
           Write(Html.EditorFor(m => m.PossibleStartDate));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                ");

            
            #line 76 "..\..\Views\Vacancy\Summary.cshtml"
           Write(Html.ValidationMessageWithSeverityFor(m => m.PossibleStartDate, Html.GetValidationType(m => m.PossibleStartDate)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </fieldset>\r\n");

WriteLiteral("            ");

            
            #line 78 "..\..\Views\Vacancy\Summary.cshtml"
       Write(Html.FormTextAreaFor(m => m.LongDescription, controlHtmlAttributes: new {@class = "width-all-1-1 form-textarea-large", type = "text", size = 12, rows = 22}));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </section>\r\n");

WriteLiteral("    <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(">Save</button>\r\n    </div>\r\n");

            
            #line 84 "..\..\Views\Vacancy\Summary.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
