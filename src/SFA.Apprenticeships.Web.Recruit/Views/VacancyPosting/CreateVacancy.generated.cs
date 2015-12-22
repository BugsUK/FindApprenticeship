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
    using SFA.Apprenticeships.Infrastructure.Presentation;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    #line 5 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.Extensions;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates;
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.EditorTemplates;
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
            
            #line 6 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
  
    ViewBag.Title = "Recruit an Apprentice - Select framework and level";

    const string selected = "selected";

    var isApplicationThroughRAAYes = !Model.OfflineVacancy ? selected : null;
    var isApplicationThroughRAANo = Model.OfflineVacancy ? selected : null;

    var frameworksSelected = Model.TrainingType == TrainingType.Frameworks ? selected : null;
    var standardsSelected = Model.TrainingType == TrainingType.Standards ? selected : null;

    var saveButtonText = "Save and continue";
    var saveButtonValue = "CreateVacancy";

    if (Model.Status == ProviderVacancyStatuses.RejectedByQA || Model.ComeFromPreview)
    {
        saveButtonText = "Save and return to Preview";
        saveButtonValue = "CreateVacancyAndPreview";
    }


            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">\r\n    Enter basic vacancy details\r\n</h1>\r\n\r\n");

            
            #line 32 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
 using (Html.BeginRouteForm(RecruitmentRouteNames.CreateVacancy, FormMethod.Post))
{
    
            
            #line default
            #line hidden
            
            #line 34 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 34 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                            
    
            
            #line default
            #line hidden
            
            #line 35 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(m => m.IsEmployerLocationMainApprenticeshipLocation));

            
            #line default
            #line hidden
            
            #line 35 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                        ;
    
            
            #line default
            #line hidden
            
            #line 36 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(m => m.NumberOfPositions));

            
            #line default
            #line hidden
            
            #line 36 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                             
    
            
            #line default
            #line hidden
            
            #line 37 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(m => m.ComeFromPreview));

            
            #line default
            #line hidden
            
            #line 37 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                           
    
            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                           

    
            
            #line default
            #line hidden
            
            #line 40 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.FormTextAreaFor(m => m.Title, controlHtmlAttributes: new { type = "text", size = 12, @class = "width-all-1-1 form-textarea-small" }));

            
            #line default
            #line hidden
            
            #line 40 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                                                              
    
            
            #line default
            #line hidden
            
            #line 41 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.TitleComment)));

            
            #line default
            #line hidden
            
            #line 41 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                            
    
            
            #line default
            #line hidden
            
            #line 42 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.FormTextAreaFor(m => m.ShortDescription, controlHtmlAttributes: new { type = "text", size = 12, @class = "width-all-1-1 form-textarea-medium" }));

            
            #line default
            #line hidden
            
            #line 42 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                                                                          
    
            
            #line default
            #line hidden
            
            #line 43 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.ShortDescriptionComment)));

            
            #line default
            #line hidden
            
            #line 43 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                       


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"blocklabel-single-container\"");

WriteLiteral(">\r\n            <div");

WriteAttribute("class", Tuple.Create(" class=\"", 2113), Tuple.Create("\"", 2301)
, Tuple.Create(Tuple.Create("", 2121), Tuple.Create("form-group", 2121), true)
            
            #line 47 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create(" ", 2131), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.TrainingType))
            
            #line default
            #line hidden
, 2132), false)
, Tuple.Create(Tuple.Create(" ", 2257), Tuple.Create("inline", 2258), true)
, Tuple.Create(Tuple.Create(" ", 2264), Tuple.Create("clearfix", 2265), true)
, Tuple.Create(Tuple.Create(" ", 2273), Tuple.Create("blocklabel-single", 2274), true)
, Tuple.Create(Tuple.Create(" ", 2291), Tuple.Create("hide-nojs", 2292), true)
);

WriteLiteral(">\r\n                <a");

WriteAttribute("name", Tuple.Create(" name=\"", 2323), Tuple.Create("\"", 2385)
            
            #line 48 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 2330), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.TrainingType).ToString().ToLower()
            
            #line default
            #line hidden
, 2330), false)
);

WriteLiteral("></a>\r\n                <label");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship type</label>\r\n                ");

WriteLiteral("\r\n                <label");

WriteLiteral(" data-target=\"training-type-frameworks-panel\"");

WriteLiteral(" for=\"training-type-frameworks\"");

WriteAttribute("class", Tuple.Create(" class=\"", 2627), Tuple.Create("\"", 2666)
, Tuple.Create(Tuple.Create("", 2635), Tuple.Create("block-label", 2635), true)
            
            #line 51 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                     , Tuple.Create(Tuple.Create(" ", 2646), Tuple.Create<System.Object, System.Int32>(frameworksSelected
            
            #line default
            #line hidden
, 2647), false)
);

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 52 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.RadioButtonFor(model => model.TrainingType, TrainingType.Frameworks, new {id = "training-type-frameworks", aria_controls = "training-type-frameworks-panel"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    Framework\r\n                </label>\r\n                ");

WriteLiteral("\r\n                <label");

WriteLiteral(" data-target=\"training-type-standards-panel\"");

WriteLiteral(" for=\"training-type-standards\"");

WriteAttribute("class", Tuple.Create(" class=\"", 3073), Tuple.Create("\"", 3111)
, Tuple.Create(Tuple.Create("", 3081), Tuple.Create("block-label", 3081), true)
            
            #line 56 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                   , Tuple.Create(Tuple.Create(" ", 3092), Tuple.Create<System.Object, System.Int32>(standardsSelected
            
            #line default
            #line hidden
, 3093), false)
);

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 57 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.RadioButtonFor(model => model.TrainingType, TrainingType.Standards, new {id = "training-type-standards", aria_controls = "training-type-standards-panel"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    Standard\r\n                </label>\r\n");

WriteLiteral("                ");

            
            #line 60 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
           Write(Html.ValidationMessageFor(m => m.TrainingType));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n            <div");

WriteLiteral(" id=\"training-type-frameworks-panel\"");

WriteLiteral(" class=\"toggle-content blocklabel-content\"");

WriteLiteral(">\r\n                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 3557), Tuple.Create("\"", 3706)
, Tuple.Create(Tuple.Create("", 3565), Tuple.Create("form-group", 3565), true)
            
            #line 64 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create(" ", 3575), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.FrameworkCodeName))
            
            #line default
            #line hidden
, 3576), false)
);

WriteLiteral(">\r\n                    <a");

WriteAttribute("name", Tuple.Create(" name=\"", 3732), Tuple.Create("\"", 3799)
            
            #line 65 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 3739), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.FrameworkCodeName).ToString().ToLower()
            
            #line default
            #line hidden
, 3739), false)
);

WriteLiteral("></a>\r\n                    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 3833), Tuple.Create("\"", 3878)
            
            #line 66 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 3839), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.FrameworkCodeName)
            
            #line default
            #line hidden
, 3839), false)
);

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship framework</label>\r\n");

WriteLiteral("                    ");

            
            #line 67 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.DropDownListFor(m => m.FrameworkCodeName, Model.SectorsAndFrameworks, new {@class = "para-btm-margin chosen-select", style = "min-width: 50%; margin-bottom: 15px;" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 68 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.FrameworkCodeNameComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 69 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.ValidationMessageFor(m => m.FrameworkCodeName));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n\r\n                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 4353), Tuple.Create("\"", 4504)
, Tuple.Create(Tuple.Create("", 4361), Tuple.Create("form-group", 4361), true)
            
            #line 72 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create(" ", 4371), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.ApprenticeshipLevel))
            
            #line default
            #line hidden
, 4372), false)
);

WriteLiteral(">\r\n                    <a");

WriteAttribute("name", Tuple.Create(" name=\"", 4530), Tuple.Create("\"", 4599)
            
            #line 73 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 4537), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.ApprenticeshipLevel).ToString().ToLower()
            
            #line default
            #line hidden
, 4537), false)
);

WriteLiteral("></a>\r\n                    <label");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship level</label>\r\n                    ");

WriteLiteral("\r\n                    <label");

WriteLiteral(" for=\"apprenticeship-level-intermediate\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 77 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                   Write(Html.RadioButtonFor(model => model.ApprenticeshipLevel, ApprenticeshipLevel.Intermediate, new {id = "apprenticeship-level-intermediate", aria_labelledby = "apprenticeship-level-label"}));

            
            #line default
            #line hidden
WriteLiteral(" Intermediate\r\n                    </label>\r\n                    ");

WriteLiteral("\r\n                    <label");

WriteLiteral(" for=\"apprenticeship-level-advanced\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 81 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                   Write(Html.RadioButtonFor(model => model.ApprenticeshipLevel, ApprenticeshipLevel.Advanced, new {id = "apprenticeship-level-advanced", aria_labelledby = "apprenticeship-level-label"}));

            
            #line default
            #line hidden
WriteLiteral(" Advanced\r\n                    </label>\r\n                    ");

WriteLiteral("\r\n                    <label");

WriteLiteral(" for=\"apprenticeship-level-higher\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 85 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                   Write(Html.RadioButtonFor(model => model.ApprenticeshipLevel, ApprenticeshipLevel.Higher, new {id = "apprenticeship-level-higher", aria_labelledby = "apprenticeship-level-label"}));

            
            #line default
            #line hidden
WriteLiteral(" Higher\r\n                    </label>\r\n                    ");

WriteLiteral("\r\n                    <label");

WriteLiteral(" for=\"apprenticeship-level-degree\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 89 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                   Write(Html.RadioButtonFor(model => model.ApprenticeshipLevel, ApprenticeshipLevel.Degree, new {id = "apprenticeship-level-degree", aria_labelledby = "apprenticeship-level-label"}));

            
            #line default
            #line hidden
WriteLiteral(" Degree\r\n                    </label>\r\n");

WriteLiteral("                    ");

            
            #line 91 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.ValidationMessageFor(m => m.ApprenticeshipLevel));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 92 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.ApprenticeshipLevelComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n            </div>\r\n\r\n            <div");

WriteLiteral(" id=\"training-type-standards-panel\"");

WriteLiteral(" class=\"toggle-content blocklabel-content\"");

WriteLiteral(">\r\n                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 6617), Tuple.Create("\"", 6759)
, Tuple.Create(Tuple.Create("", 6625), Tuple.Create("form-group", 6625), true)
            
            #line 97 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create(" ", 6635), Tuple.Create<System.Object, System.Int32>(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.GetValidationCssClass(Html.GetValidationType(m => m.StandardId))
            
            #line default
            #line hidden
, 6636), false)
);

WriteLiteral(">\r\n                    <a");

WriteAttribute("name", Tuple.Create(" name=\"", 6785), Tuple.Create("\"", 6845)
            
            #line 98 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 6792), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.StandardId).ToString().ToLower()
            
            #line default
            #line hidden
, 6792), false)
);

WriteLiteral("></a>\r\n                    <label");

WriteAttribute("for", Tuple.Create(" for=\"", 6879), Tuple.Create("\"", 6917)
            
            #line 99 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 6885), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.StandardId)
            
            #line default
            #line hidden
, 6885), false)
);

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship standard</label>\r\n                    <select");

WriteAttribute("name", Tuple.Create(" name=\"", 6998), Tuple.Create("\"", 7037)
            
            #line 100 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 7005), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.StandardId)
            
            #line default
            #line hidden
, 7005), false)
);

WriteAttribute("id", Tuple.Create(" id=\"", 7038), Tuple.Create("\"", 7075)
            
            #line 100 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 7043), Tuple.Create<System.Object, System.Int32>(Html.NameFor(m => m.StandardId)
            
            #line default
            #line hidden
, 7043), false)
);

WriteLiteral(" class=\"para-btm-margin chosen-select\"");

WriteLiteral(" style=\"min-width: 50%; margin-bottom: 15px;\"");

WriteLiteral(">\r\n                        <option");

WriteLiteral(" value=\"\"");

WriteLiteral(">Choose from the list of standards</option>\r\n");

            
            #line 102 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 102 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                         foreach (var standardGroup in Model.Standards.GroupBy(s => s.Sector))
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <optgroup");

WriteAttribute("label", Tuple.Create(" label=\"", 7407), Tuple.Create("\"", 7433)
            
            #line 104 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 7415), Tuple.Create<System.Object, System.Int32>(standardGroup.Key
            
            #line default
            #line hidden
, 7415), false)
);

WriteLiteral(">\r\n");

            
            #line 105 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                
            
            #line default
            #line hidden
            
            #line 105 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                 foreach (var standard in standardGroup)
                                {

            
            #line default
            #line hidden
WriteLiteral("                                    <option");

WriteAttribute("value", Tuple.Create(" value=\"", 7589), Tuple.Create("\"", 7609)
            
            #line 107 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 7597), Tuple.Create<System.Object, System.Int32>(standard.Id
            
            #line default
            #line hidden
, 7597), false)
);

WriteLiteral(" ");

            
            #line 107 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                             Write(standard.Id == Model.StandardId ? "selected" : "");

            
            #line default
            #line hidden
WriteLiteral(" data-apprenticeship-level=\"");

            
            #line 107 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                                                            Write(standard.ApprenticeshipLevel);

            
            #line default
            #line hidden
WriteLiteral("\">\r\n");

WriteLiteral("                                        ");

            
            #line 108 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                   Write(standard.Name);

            
            #line default
            #line hidden
WriteLiteral("\r\n                                    </option>\r\n");

            
            #line 110 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                }

            
            #line default
            #line hidden
WriteLiteral("                            </optgroup>\r\n");

            
            #line 112 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </select>\r\n\r\n");

WriteLiteral("                    ");

            
            #line 115 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.ValidationMessageFor(m => m.StandardId));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 116 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.StandardIdComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n\r\n");

            
            #line 119 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                
            
            #line default
            #line hidden
            
            #line 119 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                  
                    var style = Model.StandardId.HasValue ? "" : "display: none";
                
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" id=\"apprenticeship-level-container\"");

WriteAttribute("style", Tuple.Create(" style=\"", 8354), Tuple.Create("\"", 8368)
            
            #line 123 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
   , Tuple.Create(Tuple.Create("", 8362), Tuple.Create<System.Object, System.Int32>(style
            
            #line default
            #line hidden
, 8362), false)
);

WriteLiteral(">\r\n                    <label");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship level</label>\r\n                    <p");

WriteLiteral(" id=\"apprenticeship-level-name\"");

WriteLiteral(">");

            
            #line 125 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                 Write(Html.DisplayFor(m => m.ApprenticeshipLevel));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                </div>\r\n            </div>\r\n        </div>\r\n    </div>\r\n");

            
            #line 130 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"


            
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

            
            #line 140 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                     Write(isApplicationThroughRAAYes);

            
            #line default
            #line hidden
WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 141 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.RadioButtonFor(model => model.OfflineVacancy, false, new { id = "apprenticeship-online-vacancy", aria_labelledby = "apprenticeship-vacancy-management-type-label" }));

            
            #line default
            #line hidden
WriteLiteral(" Yes\r\n                </label>\r\n\r\n                ");

WriteLiteral("\r\n                <label");

WriteLiteral(" for=\"apprenticeship-offline-vacancy\"");

WriteLiteral(" class=\"block-label\"");

WriteLiteral(" data-target=\"offline-panel\"");

WriteLiteral(" ");

            
            #line 145 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                       Write(isApplicationThroughRAANo);

            
            #line default
            #line hidden
WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 146 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
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

            
            #line 152 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.FormTextFor(m => m.OfflineApplicationUrl, controlHtmlAttributes: new { @class = "width-all-1-1", type = "text", size = 12, id = "apprenticeship-offline-application-url" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 153 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.OfflineApplicationUrlComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    ");

WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 157 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.FormTextAreaFor(m => m.OfflineApplicationInstructions, controlHtmlAttributes: new { type = "text", size = 12, @class = "width-all-1-1 form-textarea-medium", id = "apprenticheship-offline-application-instructions" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 158 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
               Write(Html.Partial("_comment", Html.GetCommentViewModel(m => m.OfflineApplicationInstructionsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    <br />\r\n                </div>\r\n            </div>\r\n       " +
" </div>\r\n    </div>\r\n");

            
            #line 164 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"

    
            
            #line default
            #line hidden
            
            #line 165 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(model => model.VacancyReferenceNumber));

            
            #line default
            #line hidden
            
            #line 165 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                          
    
            
            #line default
            #line hidden
            
            #line 166 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(model => model.Ukprn));

            
            #line default
            #line hidden
            
            #line 166 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                         
    
            
            #line default
            #line hidden
            
            #line 167 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(model => model.ProviderSiteEmployerLink.ProviderSiteErn));

            
            #line default
            #line hidden
            
            #line 167 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                            
    
            
            #line default
            #line hidden
            
            #line 168 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(model => model.ProviderSiteEmployerLink.Employer.Ern));

            
            #line default
            #line hidden
            
            #line 168 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                         ;
    
            
            #line default
            #line hidden
            
            #line 169 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(model => model.VacancyGuid));

            
            #line default
            #line hidden
            
            #line 169 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                               
    
            
            #line default
            #line hidden
            
            #line 170 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
Write(Html.HiddenFor(model => model.Status));

            
            #line default
            #line hidden
            
            #line 170 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                          


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" id=\"createVacancyButton\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(" name=\"CreateVacancy\"");

WriteAttribute("value", Tuple.Create(" value=\"", 11499), Tuple.Create("\"", 11523)
            
            #line 173 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                 , Tuple.Create(Tuple.Create("", 11507), Tuple.Create<System.Object, System.Int32>(saveButtonValue
            
            #line default
            #line hidden
, 11507), false)
);

WriteLiteral(">");

            
            #line 173 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                               Write(saveButtonText);

            
            #line default
            #line hidden
WriteLiteral("</button>\r\n        <button");

WriteLiteral(" id=\"createVacancyAndExit\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button-link\"");

WriteLiteral(" name=\"CreateVacancy\"");

WriteLiteral(" value=\"CreateVacancyAndExit\"");

WriteLiteral(">Save and exit</button>\r\n");

            
            #line 175 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
        
            
            #line default
            #line hidden
            
            #line 175 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
         if (Model.ComeFromPreview)
        {
            
            
            #line default
            #line hidden
            
            #line 177 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
       Write(Html.RouteLink("Cancel", RecruitmentRouteNames.PreviewVacancy, new { vacancyReferenceNumber = Model.VacancyReferenceNumber }));

            
            #line default
            #line hidden
            
            #line 177 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                                                          
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");

            
            #line 180 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script>\r\n        $(\"#");

            
            #line 185 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
       Write(Html.NameFor(m => m.StandardId));

            
            #line default
            #line hidden
WriteLiteral("\").change(function () {\r\n            var apprenticeshipLevel = $(\"#");

            
            #line 186 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                     Write(Html.NameFor(m => m.StandardId));

            
            #line default
            #line hidden
WriteLiteral(" option:selected\").attr(\"data-apprenticeship-level\");\r\n            if (apprentice" +
"shipLevel === \"");

            
            #line 187 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                    Write(ApprenticeshipLevel.FoundationDegree.ToString());

            
            #line default
            #line hidden
WriteLiteral("\" || apprenticeshipLevel === \"");

            
            #line 187 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                                                                                                  Write(ApprenticeshipLevel.Masters.ToString());

            
            #line default
            #line hidden
WriteLiteral("\") {\r\n                apprenticeshipLevel = \"");

            
            #line 188 "..\..\Views\VacancyPosting\CreateVacancy.cshtml"
                                  Write(ApprenticeshipLevel.Degree.ToString());

            
            #line default
            #line hidden
WriteLiteral("\";\r\n            }\r\n            $(\"#apprenticeship-level-name\").text(apprenticeshi" +
"pLevel);\r\n            $(\"#apprenticeship-level-container\").show();\r\n        });\r" +
"\n    </script>\r\n");

});

WriteLiteral("\r\n");

        }
    }
}
#pragma warning restore 1591
