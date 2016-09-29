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
    
    #line 5 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
    using HtmlExtensions = SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions;
    
    #line default
    #line hidden
    
    #line 1 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Raa.Vacancies;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
    using SFA.Apprenticeships.Infrastructure.Presentation;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Raa.Common;
    
    #line 3 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.Extensions;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/Vacancy/_VacancyDetails.cshtml")]
    public partial class VacancyDetails : System.Web.Mvc.WebViewPage<VacancyViewModel>
    {
        public VacancyDetails()
        {
        }
        public override void Execute()
        {
            
            #line 8 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
  
    var editableItemClass = ViewData["editableItemClass"];
    var editableApprenticeshipLevel = string.IsNullOrWhiteSpace(Model.StandardName);
    var editableApprenticeshipLevelClass = (!Model.IsEditable || !editableApprenticeshipLevel) ? "" : "editable-item";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<section");

WriteLiteral(" class=\"section-border grid-wrapper\"");

WriteLiteral(" id=\"vacancy-info\"");

WriteLiteral(">\r\n    <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">\r\n");

            
            #line 16 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
        
            
            #line default
            #line hidden
            
            #line 16 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
         if (Model.VacancyType == VacancyType.Traineeship)
        {

            
            #line default
            #line hidden
WriteLiteral("            <span>Traineeship details</span>\r\n");

            
            #line 19 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
        }
        else
        {

            
            #line default
            #line hidden
WriteLiteral("            <span>Apprenticeship summary</span>\r\n");

            
            #line 23 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </h2>\r\n    <div");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n\r\n");

            
            #line 28 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            
            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
             if (Model.VacancyType == VacancyType.Traineeship)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div>\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Training provider</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-provider-name\"");

WriteLiteral(">");

            
            #line 32 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                             Write(Model.ProviderSite.TradingName);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                </div>\r\n");

            
            #line 34 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 1311), Tuple.Create("\"", 1337)
            
            #line 35 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 1319), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 1319), false)
);

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">\r\n                        Traineeship sector\r\n");

WriteLiteral("                        ");

            
            #line 38 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                   Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.TrainingDetailsViewModel.SectorCodeName, Model.TrainingDetailsViewModel.SectorCodeNameComment, Model.TrainingDetailsLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </h3>\r\n                    <p");

WriteLiteral(" id=\"traineeship-sector\"");

WriteLiteral(">");

            
            #line 40 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                          Write(Html.DisplayFor(m => m.SectorName));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("                    ");

            
            #line 41 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.TrainingDetailsViewModel.SectorCodeName, Model.TrainingDetailsLink, Model.TrainingDetailsViewModel.SectorCodeNameComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n");

            
            #line 43 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 45 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
       Write(Html.Partial("DisplayTemplates/Vacancy/_WorkingWeekAndWage", Model, new ViewDataDictionary { { "editableItemClass", editableItemClass } }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n            <div");

WriteAttribute("class", Tuple.Create(" class=\"", 2221), Tuple.Create("\"", 2247)
            
            #line 47 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 2229), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 2229), false)
);

WriteLiteral(">\r\n                <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">\r\n");

            
            #line 49 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 49 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                     if (Model.VacancyType == VacancyType.Traineeship)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <span>Expected duration</span>\r\n");

            
            #line 52 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <span>Expected apprenticeship duration</span>\r\n");

            
            #line 56 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 57 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(ValidationResultViewModel.PartialView, Html.GetValidationResultViewModel(Model, m => m.FurtherVacancyDetailsViewModel.Duration, ViewData.ModelState, Model.SummaryLink, Model.FurtherVacancyDetailsViewModel.DurationComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </h3>\r\n                <p");

WriteLiteral(" id=\"vacancy-expected-duration\"");

WriteLiteral(">");

            
            #line 59 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                             Write(Model.FurtherVacancyDetailsViewModel.DurationTypeDisplayText);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("                ");

            
            #line 60 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
           Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.FurtherVacancyDetailsViewModel.Duration, Model.SummaryLink, Model.FurtherVacancyDetailsViewModel.DurationComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n            <div");

WriteAttribute("class", Tuple.Create(" class=\"", 3262), Tuple.Create("\"", 3288)
            
            #line 63 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 3270), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 3270), false)
);

WriteLiteral(">\r\n                <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">\r\n                    Possible start date\r\n");

WriteLiteral("                    ");

            
            #line 66 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(ValidationResultViewModel.PartialView, Html.GetValidationResultViewModel(Model, m => m.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, ViewData.ModelState, Model.SummaryLink, Model.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDateComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </h3>\r\n                <p");

WriteLiteral(" id=\"vacancy-start-date\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 69 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.DisplayFor(m => Model.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate.Date));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </p>\r\n");

WriteLiteral("                ");

            
            #line 71 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
           Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate, Model.SummaryLink, Model.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDateComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n\r\n");

            
            #line 74 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            
            
            #line default
            #line hidden
            
            #line 74 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
             if (Model.VacancyType == VacancyType.Apprenticeship)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 4313), Tuple.Create("\"", 4354)
            
            #line 76 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 4321), Tuple.Create<System.Object, System.Int32>(editableApprenticeshipLevelClass
            
            #line default
            #line hidden
, 4321), false)
);

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">\r\n                        Apprenticeship level\r\n");

WriteLiteral("                        ");

            
            #line 79 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                   Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.TrainingDetailsViewModel.ApprenticeshipLevel, Model.TrainingDetailsViewModel.ApprenticeshipLevelComment, Model.TrainingDetailsLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-type\"");

WriteLiteral(" itemprop=\"employmentType\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 82 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                   Write(Html.DisplayFor(m => m.TrainingDetailsViewModel.ApprenticeshipLevel));

            
            #line default
            #line hidden
WriteLiteral(" Level Apprenticeship\r\n                    </p>\r\n");

            
            #line 84 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 84 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                     if (editableApprenticeshipLevel)
                    {
                        
            
            #line default
            #line hidden
            
            #line 86 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                   Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.TrainingDetailsViewModel.ApprenticeshipLevel, Model.TrainingDetailsLink, Model.TrainingDetailsViewModel.ApprenticeshipLevelComment)));

            
            #line default
            #line hidden
            
            #line 86 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                                                                                                                                                                                                                                 
                    }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n");

            
            #line 89 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            }            

            
            #line default
            #line hidden
WriteLiteral("            ");

            
            #line 90 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
             if (Model.IsSingleLocation)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 5404), Tuple.Create("\"", 5430)
            
            #line 92 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 5412), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 5412), false)
);

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Positions</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-reference-id\"");

WriteLiteral(">");

            
            #line 94 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                            Write(Model.NewVacancyViewModel.NumberOfPositions);

            
            #line default
            #line hidden
WriteLiteral(" available</p>\r\n");

WriteLiteral("                    ");

            
            #line 95 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.NewVacancyViewModel.NumberOfPositions, Model.EmployerLink, Model.NewVacancyViewModel.NumberOfPositionsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n");

            
            #line 97 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            }
            else
            {
                if (Model.NewVacancyViewModel.LocationAddresses != null && Model.NewVacancyViewModel.LocationAddresses.Count() == 1)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 6069), Tuple.Create("\"", 6095)
            
            #line 102 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 6077), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 6077), false)
);

WriteLiteral(">\r\n                        <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Positions</h3>\r\n                        <p");

WriteLiteral(" id=\"vacancy-reference-id\"");

WriteLiteral(">");

            
            #line 104 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                                Write(Model.NewVacancyViewModel.LocationAddresses.First().NumberOfPositions);

            
            #line default
            #line hidden
WriteLiteral(" available</p>\r\n");

WriteLiteral("                        ");

            
            #line 105 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                   Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.NewVacancyViewModel.LocationAddresses.First().NumberOfPositions, Model.EmployerLink, Model.NewVacancyViewModel.NumberOfPositionsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n");

            
            #line 107 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                }
            }

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 110 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            
            
            #line default
            #line hidden
            
            #line 110 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
             if (Model.VacancyType == VacancyType.Traineeship)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 6712), Tuple.Create("\"", 6743)
, Tuple.Create(Tuple.Create("", 6720), Tuple.Create("text", 6720), true)
            
            #line 112 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 6724), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 6725), false)
);

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">\r\n                        Contact details\r\n");

WriteLiteral("                        ");

            
            #line 115 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                   Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.TrainingDetailsViewModel.ContactName, Model.TrainingDetailsViewModel.ContactDetailsComment, Model.TrainingDetailsLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </h3>\r\n");

            
            #line 117 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 117 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                     if (string.IsNullOrEmpty(Model.TrainingDetailsViewModel.ContactName) && string.IsNullOrEmpty(Model.TrainingDetailsViewModel.ContactNumber) && string.IsNullOrEmpty(Model.TrainingDetailsViewModel.ContactEmail))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <span>None specified. This message will not appear on the" +
" vacancy when it goes live</span>\r\n");

            
            #line 120 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">");

            
            #line 123 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                               Write(Model.TrainingDetailsViewModel.ContactName);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("                        <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">");

            
            #line 124 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                               Write(Model.TrainingDetailsViewModel.ContactNumber);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("                        <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">");

            
            #line 125 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                               Write(Model.TrainingDetailsViewModel.ContactEmail);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 126 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 127 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.TrainingDetailsViewModel.ContactName, Model.TrainingDetailsLink, Model.TrainingDetailsViewModel.ContactDetailsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n");

            
            #line 129 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 8201), Tuple.Create("\"", 8232)
, Tuple.Create(Tuple.Create("", 8209), Tuple.Create("text", 8209), true)
            
            #line 133 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 8213), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 8214), false)
);

WriteLiteral(" itemprop=\"responsibilities\"");

WriteLiteral(">\r\n");

            
            #line 134 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            
            
            #line default
            #line hidden
            
            #line 134 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
             if (!Model.IsCandidateView)
            {

            
            #line default
            #line hidden
WriteLiteral("                <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(" id=\"long-description-header\"");

WriteLiteral(">\r\n");

            
            #line 137 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 137 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                     if (Model.VacancyType == VacancyType.Traineeship)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <span>Work placement</span>\r\n");

            
            #line 140 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <span>Vacancy description</span>\r\n");

            
            #line 144 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 145 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.FurtherVacancyDetailsViewModel.LongDescription, Model.FurtherVacancyDetailsViewModel.LongDescriptionComment, Model.SummaryLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </h3>\r\n");

            
            #line 147 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("            <p");

WriteLiteral(" id=\"vacancy-full-descrpition\"");

WriteAttribute("class", Tuple.Create(" class=\"", 9011), Tuple.Create("\"", 9104)
            
            #line 148 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 9019), Tuple.Create<System.Object, System.Int32>(Model.FurtherVacancyDetailsViewModel.LongDescription.GetPreserveFormattingCssClass()
            
            #line default
            #line hidden
, 9019), false)
);

WriteLiteral(">");

            
            #line 148 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                                                                                                                      Write(Html.Raw(HtmlExtensions.EscapeHtmlEncoding(Model.FurtherVacancyDetailsViewModel.LongDescription)));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("            ");

            
            #line 149 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
       Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.FurtherVacancyDetailsViewModel.LongDescription, Model.SummaryLink, Model.FurtherVacancyDetailsViewModel.LongDescriptionComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n");

            
            #line 151 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
        
            
            #line default
            #line hidden
            
            #line 151 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
         if (Model.VacancyType == VacancyType.Traineeship)
        {

            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteAttribute("class", Tuple.Create(" class=\"", 9540), Tuple.Create("\"", 9566)
            
            #line 153 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 9548), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 9548), false)
);

WriteLiteral(">\r\n                <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">\r\n                    Training to be provided\r\n");

WriteLiteral("                    ");

            
            #line 156 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.TrainingDetailsViewModel.TrainingProvided, Model.TrainingDetailsViewModel.TrainingProvidedComment, Model.TrainingDetailsLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </h3>\r\n                <p");

WriteLiteral(" id=\"vacancy-training-to-be-provided\"");

WriteAttribute("class", Tuple.Create(" class=\"", 9972), Tuple.Create("\"", 10060)
            
            #line 158 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 9980), Tuple.Create<System.Object, System.Int32>(Model.TrainingDetailsViewModel.TrainingProvided.GetPreserveFormattingCssClass()
            
            #line default
            #line hidden
, 9980), false)
);

WriteLiteral(">");

            
            #line 158 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                                                                                                                            Write(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.EscapeHtmlEncoding(Html.Raw(Model.TrainingDetailsViewModel.TrainingProvided)));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("                ");

            
            #line 159 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
           Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.TrainingDetailsViewModel.TrainingProvided, Model.TrainingDetailsLink, Model.TrainingDetailsViewModel.TrainingProvidedComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n");

WriteLiteral("            <div");

WriteAttribute("class", Tuple.Create(" class=\"", 10467), Tuple.Create("\"", 10493)
            
            #line 161 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 10475), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 10475), false)
);

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">\r\n                        Future prospects\r\n");

WriteLiteral("                        ");

            
            #line 165 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                   Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.VacancyRequirementsProspectsViewModel.FutureProspects, Model.VacancyRequirementsProspectsViewModel.FutureProspectsComment, Model.RequirementsProspectsLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-future-prospects\"");

WriteLiteral(" itemprop=\"incentives\"");

WriteAttribute("class", Tuple.Create(" class=\"", 10993), Tuple.Create("\"", 11093)
            
            #line 167 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
 , Tuple.Create(Tuple.Create("", 11001), Tuple.Create<System.Object, System.Int32>(Model.VacancyRequirementsProspectsViewModel.FutureProspects.GetPreserveFormattingCssClass()
            
            #line default
            #line hidden
, 11001), false)
);

WriteLiteral(">");

            
            #line 167 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                                                                                                                                                           Write(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.EscapeHtmlEncoding(Html.Raw(Model.VacancyRequirementsProspectsViewModel.FutureProspects)));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("                    ");

            
            #line 168 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.VacancyRequirementsProspectsViewModel.FutureProspects, Model.RequirementsProspectsLink, Model.VacancyRequirementsProspectsViewModel.FutureProspectsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n            </div>\r\n");

            
            #line 171 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</section>");

        }
    }
}
#pragma warning restore 1591
