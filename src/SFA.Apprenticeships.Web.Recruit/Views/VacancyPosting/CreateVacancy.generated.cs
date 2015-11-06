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
    
    #line 2 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Recruit;
    
    #line 4 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
    using SFA.Apprenticeships.Web.Recruit.Constants;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/VacancyPosting/CreateVacancy.cshtml")]
    public partial class CreateVacancy : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy.NewVacancyViewModel>
    {
        public CreateVacancy()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
  
    ViewBag.Title = "Recruit an Apprentice - Select framework and level";

    const string selected = "selected";

    var isApplicationThroughRAAYes = !Model.OfflineVacancy ? selected : null;
    var isApplicationThroughRAANo = Model.OfflineVacancy ? selected : null;

    var frameworksSelected = Model.TrainingType == TrainingType.Frameworks ? selected : null;
    var standardsSelected = Model.TrainingType == TrainingType.Standards ? selected : null;

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">\r\n    Enter basic vacancy details\r\n</h1>\r\n\r\n");

            
            #line 21 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
 using (Html.BeginRouteForm(RecruitmentRouteNames.CreateVacancy, FormMethod.Post))
{
    
            
            #line default
            #line hidden
            
            #line 23 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 23 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                            
    
            
            #line default
            #line hidden
            
            #line 24 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 24 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                           

    
            
            #line default
            #line hidden
            
            #line 26 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.FormTextAreaFor(m => m.Title, controlHtmlAttributes: new { type = "text", size = 12, @class = "width-all-1-1 form-textarea-small" }));

            
            #line default
            #line hidden
            
            #line 26 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                                                              
    
            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.FormTextAreaFor(m => m.ShortDescription, controlHtmlAttributes: new { type = "text", size = 12, @class = "width-all-1-1 form-textarea-medium" }));

            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                                                                          


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"blocklabel-single-container\"");

WriteLiteral(">\r\n            <div");

WriteAttribute("class", Tuple.Create(" class=\"", 1416), Tuple.Create("\"", 1563)
, Tuple.Create(Tuple.Create("", 1424), Tuple.Create("form-group", 1424), true)
            
            #line 31 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create(" ", 1434), Tuple.Create<System.Object, System.Int32>(HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.TrainingType))
            
            #line default
            #line hidden
, 1435), false)
, Tuple.Create(Tuple.Create(" ", 1519), Tuple.Create("inline", 1520), true)
, Tuple.Create(Tuple.Create(" ", 1526), Tuple.Create("clearfix", 1527), true)
, Tuple.Create(Tuple.Create(" ", 1535), Tuple.Create("blocklabel-single", 1536), true)
, Tuple.Create(Tuple.Create(" ", 1553), Tuple.Create("hide-nojs", 1554), true)
);

WriteLiteral(">\r\n                <a");

WriteAttribute("name", Tuple.Create(" name=\"", 1585), Tuple.Create("\"", 1647)
            
            #line 32 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 1592), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.TrainingType).ToString().ToLower()
            
            #line default
            #line hidden
, 1592), false)
);

WriteLiteral("></a>\r\n                <label");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Training type</label>\r\n                ");

WriteLiteral("\r\n                <label");

WriteLiteral(" data-target=\"training-type-frameworks-panel\"");

WriteLiteral(" for=\"training-type-frameworks\"");

WriteAttribute("class", Tuple.Create(" class=\"", 1883), Tuple.Create("\"", 1922)
, Tuple.Create(Tuple.Create("", 1891), Tuple.Create("block-label", 1891), true)
            
            #line 35 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                     , Tuple.Create(Tuple.Create(" ", 1902), Tuple.Create<System.Object, System.Int32>(frameworksSelected
            
            #line default
            #line hidden
, 1903), false)
);

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 36 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.RadioButtonFor(model => model.TrainingType, TrainingType.Frameworks, new {id = "training-type-frameworks", aria_controls = "training-type-frameworks-panel"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    Frameworks\r\n                </label>\r\n                ");

WriteLiteral("\r\n                <label");

WriteLiteral(" data-target=\"training-type-standards-panel\"");

WriteLiteral(" for=\"training-type-standards\"");

WriteAttribute("class", Tuple.Create(" class=\"", 2330), Tuple.Create("\"", 2368)
, Tuple.Create(Tuple.Create("", 2338), Tuple.Create("block-label", 2338), true)
            
            #line 40 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                   , Tuple.Create(Tuple.Create(" ", 2349), Tuple.Create<System.Object, System.Int32>(standardsSelected
            
            #line default
            #line hidden
, 2350), false)
);

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 41 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.RadioButtonFor(model => model.TrainingType, TrainingType.Standards, new {id = "training-type-standards", aria_controls = "training-type-standards-panel"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    Standards\r\n                </label>\r\n");

WriteLiteral("                ");

            
            #line 44 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
           Write(Html.ValidationMessageFor(m => m.TrainingType));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n            <div");

WriteLiteral(" id=\"training-type-frameworks-panel\"");

WriteLiteral(" class=\"toggle-content blocklabel-content\"");

WriteLiteral(">\r\n                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 2815), Tuple.Create("\"", 2923)
, Tuple.Create(Tuple.Create("", 2823), Tuple.Create("form-group", 2823), true)
            
            #line 48 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create(" ", 2833), Tuple.Create<System.Object, System.Int32>(HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.FrameworkCodeName))
            
            #line default
            #line hidden
, 2834), false)
);

WriteLiteral(">\r\n                    <a");

WriteAttribute("name", Tuple.Create(" name=\"", 2949), Tuple.Create("\"", 3016)
            
            #line 49 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 2956), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.FrameworkCodeName).ToString().ToLower()
            
            #line default
            #line hidden
, 2956), false)
);

WriteLiteral("></a>\r\n                    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 3050), Tuple.Create("\"", 3095)
            
            #line 50 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 3056), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.FrameworkCodeName)
            
            #line default
            #line hidden
, 3056), false)
);

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship framework</label>\r\n");

WriteLiteral("                    ");

            
            #line 51 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.DropDownListFor(m => m.FrameworkCodeName, Model.SectorsAndFrameworks, new {@class = "para-btm-margin chosen-select", style = "min-width: 50%;"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 52 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.ValidationMessageFor(m => m.FrameworkCodeName));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n\r\n                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 3442), Tuple.Create("\"", 3552)
, Tuple.Create(Tuple.Create("", 3450), Tuple.Create("form-group", 3450), true)
            
            #line 55 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create(" ", 3460), Tuple.Create<System.Object, System.Int32>(HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.ApprenticeshipLevel))
            
            #line default
            #line hidden
, 3461), false)
);

WriteLiteral(">\r\n                    <a");

WriteAttribute("name", Tuple.Create(" name=\"", 3578), Tuple.Create("\"", 3647)
            
            #line 56 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 3585), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.ApprenticeshipLevel).ToString().ToLower()
            
            #line default
            #line hidden
, 3585), false)
);

WriteLiteral("></a>\r\n                    <label");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship level</label>\r\n                    <div");

WriteLiteral(" class=\"small-btm-margin clearfix\"");

WriteLiteral(">\r\n                        ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"apprenticeship-level-intermediate\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 61 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                       Write(Html.RadioButtonFor(model => model.ApprenticeshipLevel, ApprenticeshipLevel.Intermediate, new {id = "apprenticeship-level-intermediate", aria_labelledby = "apprenticeship-level-label"}));

            
            #line default
            #line hidden
WriteLiteral(" Intermediate\r\n                        </label>\r\n                    </div>\r\n\r\n  " +
"                  <div");

WriteLiteral(" class=\"small-btm-margin clearfix\"");

WriteLiteral(">\r\n                        ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"apprenticeship-level-advanced\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 68 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                       Write(Html.RadioButtonFor(model => model.ApprenticeshipLevel, ApprenticeshipLevel.Advanced, new {id = "apprenticeship-level-advanced", aria_labelledby = "apprenticeship-level-label"}));

            
            #line default
            #line hidden
WriteLiteral(" Advanced\r\n                        </label>\r\n                    </div>\r\n\r\n      " +
"              <div");

WriteLiteral(" class=\"small-btm-margin clearfix\"");

WriteLiteral(">\r\n                        ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"apprenticeship-level-higher\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 75 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                       Write(Html.RadioButtonFor(model => model.ApprenticeshipLevel, ApprenticeshipLevel.Higher, new {id = "apprenticeship-level-higher", aria_labelledby = "apprenticeship-level-label"}));

            
            #line default
            #line hidden
WriteLiteral(" Higher\r\n                        </label>\r\n                    </div>\r\n\r\n        " +
"            <div");

WriteLiteral(" class=\"small-btm-margin clearfix\"");

WriteLiteral(">\r\n                        ");

WriteLiteral("\r\n                        <label");

WriteLiteral(" for=\"apprenticeship-level-degree\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                            ");

            
            #line 82 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                       Write(Html.RadioButtonFor(model => model.ApprenticeshipLevel, ApprenticeshipLevel.Degree, new {id = "apprenticeship-level-degree", aria_labelledby = "apprenticeship-level-label"}));

            
            #line default
            #line hidden
WriteLiteral(" Degree\r\n                        </label>\r\n                    </div>\r\n");

WriteLiteral("                    ");

            
            #line 85 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.ValidationMessageFor(m => m.ApprenticeshipLevel));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n            </div>\r\n\r\n            <div");

WriteLiteral(" id=\"training-type-standards-panel\"");

WriteLiteral(" class=\"toggle-content blocklabel-content\"");

WriteLiteral(">\r\n                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 5983), Tuple.Create("\"", 6084)
, Tuple.Create(Tuple.Create("", 5991), Tuple.Create("form-group", 5991), true)
            
            #line 90 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create(" ", 6001), Tuple.Create<System.Object, System.Int32>(HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.StandardId))
            
            #line default
            #line hidden
, 6002), false)
);

WriteLiteral(">\r\n                    <a");

WriteAttribute("name", Tuple.Create(" name=\"", 6110), Tuple.Create("\"", 6170)
            
            #line 91 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 6117), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.StandardId).ToString().ToLower()
            
            #line default
            #line hidden
, 6117), false)
);

WriteLiteral("></a>\r\n                    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 6204), Tuple.Create("\"", 6242)
            
            #line 92 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 6210), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.StandardId)
            
            #line default
            #line hidden
, 6210), false)
);

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship standard</label>\r\n                    <select");

WriteAttribute("name", Tuple.Create(" name=\"", 6323), Tuple.Create("\"", 6362)
            
            #line 93 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 6330), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.StandardId)
            
            #line default
            #line hidden
, 6330), false)
);

WriteAttribute("id", Tuple.Create(" id=\"", 6363), Tuple.Create("\"", 6400)
            
            #line 93 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 6368), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.StandardId)
            
            #line default
            #line hidden
, 6368), false)
);

WriteLiteral(" class=\"para-btm-margin chosen-select\"");

WriteLiteral(" style=\"min-width: 50%;\"");

WriteLiteral(">\r\n                        <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">Choose from the list of standards</option>\r\n");

            
            #line 95 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 95 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                         foreach (var standardGroup in Model.Standards.GroupBy(s => s.Sector))
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <optgroup");

WriteAttribute("label", Tuple.Create(" label=\"", 6711), Tuple.Create("\"", 6737)
            
            #line 97 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 6719), Tuple.Create<System.Object, System.Int32>(standardGroup.Key
            
            #line default
            #line hidden
, 6719), false)
);

WriteLiteral(">\r\n");

            
            #line 98 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                
            
            #line default
            #line hidden
            
            #line 98 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                 foreach (var standard in standardGroup)
                                {

            
            #line default
            #line hidden
WriteLiteral("                                    <option");

WriteAttribute("value", Tuple.Create(" value=\"", 6893), Tuple.Create("\"", 6913)
            
            #line 100 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 6901), Tuple.Create<System.Object, System.Int32>(standard.Id
            
            #line default
            #line hidden
, 6901), false)
);

WriteLiteral(" ");

            
            #line 100 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                             Write(standard.Id == Model.StandardId ? "selected" : "");

            
            #line default
            #line hidden
WriteLiteral(" data-apprenticeship-level=\"");

            
            #line 100 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                                                            Write(standard.ApprenticeshipLevel);

            
            #line default
            #line hidden
WriteLiteral("\">\r\n");

WriteLiteral("                                        ");

            
            #line 101 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                   Write(standard.Name);

            
            #line default
            #line hidden
WriteLiteral("\r\n                                    </option>\r\n");

            
            #line 103 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                }

            
            #line default
            #line hidden
WriteLiteral("                            </optgroup>\r\n");

            
            #line 105 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </select>\r\n\r\n");

WriteLiteral("                    ");

            
            #line 108 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.ValidationMessageFor(m => m.StandardId));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <label");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship level</label>\r\n                    <p");

WriteLiteral(" id=\"apprenticeship-level-name\"");

WriteLiteral(">");

            
            #line 113 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                 Write(Html.DisplayFor(m => m.ApprenticeshipLevel));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n");

            
            #line 118 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"blocklabel-single-container\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"form-group inline clearfix blocklabel-single hide-nojs\"");

WriteLiteral(">\r\n                <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Manage application method</h3>\r\n                <p>\r\n                    Will th" +
"is vacancy be managed through the find an apprentice\r\n                    site?\r" +
"\n                </p>\r\n                ");

WriteLiteral("\r\n                <label");

WriteLiteral(" for=\"apprenticeship-online-vacancy\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(" data-target=\"online-panel\"");

WriteLiteral(" ");

            
            #line 128 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                     Write(isApplicationThroughRAAYes);

            
            #line default
            #line hidden
WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 129 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.RadioButtonFor(model => model.OfflineVacancy, false, new { id = "apprenticeship-online-vacancy", aria_labelledby = "apprenticeship-vacancy-management-type-label" }));

            
            #line default
            #line hidden
WriteLiteral(" Yes\r\n                </label>\r\n\r\n                ");

WriteLiteral("\r\n                <label");

WriteLiteral(" for=\"apprenticeship-offline-vacancy\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(" data-target=\"offline-panel\"");

WriteLiteral(" ");

            
            #line 133 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                       Write(isApplicationThroughRAANo);

            
            #line default
            #line hidden
WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 134 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.RadioButtonFor(model => model.OfflineVacancy, true, new { id = "apprenticeship-offline-vacancy", aria_labelledby = "apprenticeship-vacancy-management-type-label", aria_controls = "offline-panel" }));

            
            #line default
            #line hidden
WriteLiteral(" No\r\n                </label>\r\n            </div>\r\n            <div");

WriteLiteral(" id=\"offline-panel\"");

WriteLiteral(" class=\"toggle-content panel-indent blocklabel-content\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    ");

WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 140 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.FormTextFor(m => m.OfflineApplicationUrl, controlHtmlAttributes: new { @class = "width-all-1-2", type = "text", size = 12, id = "apprenticeship-offline-application-url" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    ");

WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 144 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.FormTextAreaFor(m => m.OfflineApplicationInstructions, controlHtmlAttributes: new { type = "text", size = 12, @class = "width-all-1-1 form-textarea-medium", id = "apprenticheship-offline-application-instructions" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    <br />\r\n                </div>\r\n            </div>\r\n       " +
" </div>\r\n    </div>\r\n");

            
            #line 150 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"

    
            
            #line default
            #line hidden
            
            #line 151 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(model => model.VacancyReferenceNumber));

            
            #line default
            #line hidden
            
            #line 151 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                          
    
            
            #line default
            #line hidden
            
            #line 152 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(model => model.Ukprn));

            
            #line default
            #line hidden
            
            #line 152 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                         
    
            
            #line default
            #line hidden
            
            #line 153 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(model => model.ProviderSiteEmployerLink.ProviderSiteErn));

            
            #line default
            #line hidden
            
            #line 153 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                            
    
            
            #line default
            #line hidden
            
            #line 154 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(model => model.ProviderSiteEmployerLink.Employer.Ern));

            
            #line default
            #line hidden
            
            #line 154 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                         ;
    
            
            #line default
            #line hidden
            
            #line 155 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(model => model.VacancyGuid));

            
            #line default
            #line hidden
            
            #line 155 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                               


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(" name=\"CreateVacancy\"");

WriteLiteral(" value=\"CreateVacancy\"");

WriteLiteral(">Save and continue</button>\r\n        <button");

WriteLiteral(" id=\"createVacancyAndExit\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button-link\"");

WriteLiteral(" name=\"CreateVacancy\"");

WriteLiteral(" value=\"CreateVacancyAndExit\"");

WriteLiteral(">Save and exit</button>\r\n    </div>\r\n");

            
            #line 161 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script>\r\n        $(\"#");

            
            #line 166 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
       Write(Html.NameFor(m => m.StandardId));

            
            #line default
            #line hidden
WriteLiteral("\").change(function () {\r\n            var apprenticeshipLevel = $(\"#");

            
            #line 167 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                     Write(Html.NameFor(m => m.StandardId));

            
            #line default
            #line hidden
WriteLiteral(" option:selected\").attr(\"data-apprenticeship-level\");\r\n            if (apprentice" +
"shipLevel === \"");

            
            #line 168 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                    Write(ApprenticeshipLevel.FoundationDegree.ToString());

            
            #line default
            #line hidden
WriteLiteral("\" || apprenticeshipLevel === \"");

            
            #line 168 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                                  Write(ApprenticeshipLevel.Masters.ToString());

            
            #line default
            #line hidden
WriteLiteral("\") {\r\n                apprenticeshipLevel = \"");

            
            #line 169 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                  Write(ApprenticeshipLevel.Degree.ToString());

            
            #line default
            #line hidden
WriteLiteral("\";\r\n            }\r\n            $(\"#apprenticeship-level-name\").text(apprenticeshi" +
"pLevel);\r\n        });\r\n    </script>\r\n");

});

WriteLiteral("\r\n");

        }
    }
}
#pragma warning restore 1591
