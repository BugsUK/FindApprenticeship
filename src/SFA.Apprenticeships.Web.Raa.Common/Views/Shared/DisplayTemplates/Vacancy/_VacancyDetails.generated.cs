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

WriteLiteral(" class=\"section-border grid-row\"");

WriteLiteral(" id=\"vacancy-info\"");

WriteLiteral(">\r\n    <h2");

WriteLiteral(" class=\"column-full heading-large\"");

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

WriteLiteral(" class=\"column-one-third\"");

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

WriteAttribute("class", Tuple.Create(" class=\"", 1322), Tuple.Create("\"", 1348)
            
            #line 35 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 1330), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 1330), false)
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

WriteAttribute("class", Tuple.Create(" class=\"", 2232), Tuple.Create("\"", 2258)
            
            #line 47 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 2240), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 2240), false)
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

WriteAttribute("class", Tuple.Create(" class=\"", 3273), Tuple.Create("\"", 3299)
            
            #line 63 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 3281), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 3281), false)
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

WriteAttribute("class", Tuple.Create(" class=\"", 4324), Tuple.Create("\"", 4365)
            
            #line 76 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 4332), Tuple.Create<System.Object, System.Int32>(editableApprenticeshipLevelClass
            
            #line default
            #line hidden
, 4332), false)
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
WriteLiteral("            <div>\r\n                <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Reference number</h3>\r\n");

            
            #line 92 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                
            
            #line default
            #line hidden
            
            #line 92 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                 if (Model.NewVacancyViewModel.LocationAddresses!=null && Model.NewVacancyViewModel.LocationAddresses.Count > 1)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <p>Will be generated on approval</p>\r\n");

            
            #line 95 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                }
                else
                {

            
            #line default
            #line hidden
WriteLiteral("                    <p");

WriteLiteral(" id=\"vacancy-reference-id\"");

WriteLiteral(">");

            
            #line 98 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                            Write(Model.VacancyReferenceNumber.GetVacancyReference());

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 99 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            </div>\r\n");

            
            #line 101 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            
            
            #line default
            #line hidden
            
            #line 101 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
             if (Model.IsSingleLocation || Model.IsNationwideLocation)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 5926), Tuple.Create("\"", 5952)
            
            #line 103 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 5934), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 5934), false)
);

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Positions</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-reference-id\"");

WriteLiteral(">");

            
            #line 105 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                            Write(Model.NewVacancyViewModel.NumberOfPositions);

            
            #line default
            #line hidden
WriteLiteral(" available</p>\r\n");

WriteLiteral("                    ");

            
            #line 106 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.NewVacancyViewModel.NumberOfPositions, Model.EmployerLink, Model.NewVacancyViewModel.NumberOfPositionsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n");

            
            #line 108 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            }
            else
            {
                if (Model.NewVacancyViewModel.LocationAddresses != null && Model.NewVacancyViewModel.LocationAddresses.Count() == 1)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 6591), Tuple.Create("\"", 6617)
            
            #line 113 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 6599), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 6599), false)
);

WriteLiteral(">\r\n                        <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Positions</h3>\r\n                        <p");

WriteLiteral(" id=\"vacancy-reference-id\"");

WriteLiteral(">");

            
            #line 115 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                                Write(Model.NewVacancyViewModel.LocationAddresses.First().NumberOfPositions);

            
            #line default
            #line hidden
WriteLiteral(" available</p>\r\n");

WriteLiteral("                        ");

            
            #line 116 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                   Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.NewVacancyViewModel.LocationAddresses.First().NumberOfPositions, Model.EmployerLink, Model.NewVacancyViewModel.NumberOfPositionsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n");

            
            #line 118 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                }
            }

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 121 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            
            
            #line default
            #line hidden
            
            #line 121 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
             if (Model.VacancyType == VacancyType.Traineeship)
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteAttribute("class", Tuple.Create(" class=\"", 7234), Tuple.Create("\"", 7265)
, Tuple.Create(Tuple.Create("", 7242), Tuple.Create("text", 7242), true)
            
            #line 123 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 7246), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 7247), false)
);

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">\r\n                        Contact details\r\n");

WriteLiteral("                        ");

            
            #line 126 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                   Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.TrainingDetailsViewModel.ContactName, Model.TrainingDetailsViewModel.ContactDetailsComment, Model.TrainingDetailsLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </h3>\r\n");

            
            #line 128 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 128 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                     if (string.IsNullOrEmpty(Model.TrainingDetailsViewModel.ContactName) && string.IsNullOrEmpty(Model.TrainingDetailsViewModel.ContactNumber) && string.IsNullOrEmpty(Model.TrainingDetailsViewModel.ContactEmail))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <span>None specified. This message will not appear on the" +
" vacancy when it goes live</span>\r\n");

            
            #line 131 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">");

            
            #line 134 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                               Write(Model.TrainingDetailsViewModel.ContactName);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("                        <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">");

            
            #line 135 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                               Write(Model.TrainingDetailsViewModel.ContactNumber);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("                        <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">");

            
            #line 136 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                               Write(Model.TrainingDetailsViewModel.ContactEmail);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 137 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 138 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.TrainingDetailsViewModel.ContactName, Model.TrainingDetailsLink, Model.TrainingDetailsViewModel.ContactDetailsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n");

            
            #line 140 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"column-two-thirds\"");

WriteLiteral(">\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 8727), Tuple.Create("\"", 8758)
, Tuple.Create(Tuple.Create("", 8735), Tuple.Create("text", 8735), true)
            
            #line 144 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create(" ", 8739), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 8740), false)
);

WriteLiteral(" itemprop=\"responsibilities\"");

WriteLiteral(">\r\n");

            
            #line 145 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            
            
            #line default
            #line hidden
            
            #line 145 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
             if (!Model.IsCandidateView)
            {

            
            #line default
            #line hidden
WriteLiteral("                <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(" id=\"long-description-header\"");

WriteLiteral(">\r\n");

            
            #line 148 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 148 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                     if (Model.VacancyType == VacancyType.Traineeship)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <span>Work placement</span>\r\n");

            
            #line 151 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    }
                    else
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <span>Vacancy description</span>\r\n");

            
            #line 155 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 156 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.FurtherVacancyDetailsViewModel.LongDescription, Model.FurtherVacancyDetailsViewModel.LongDescriptionComment, Model.SummaryLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </h3>\r\n");

            
            #line 158 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("            <p");

WriteLiteral(" id=\"vacancy-full-descrpition\"");

WriteAttribute("class", Tuple.Create(" class=\"", 9537), Tuple.Create("\"", 9630)
            
            #line 159 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 9545), Tuple.Create<System.Object, System.Int32>(Model.FurtherVacancyDetailsViewModel.LongDescription.GetPreserveFormattingCssClass()
            
            #line default
            #line hidden
, 9545), false)
);

WriteLiteral(">");

            
            #line 159 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                                                                                                                       Write(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.EscapeHtmlEncoding(Html, Model.FurtherVacancyDetailsViewModel.LongDescription));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("            ");

            
            #line 160 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
       Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.FurtherVacancyDetailsViewModel.LongDescription, Model.SummaryLink, Model.FurtherVacancyDetailsViewModel.LongDescriptionComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n");

            
            #line 162 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
        
            
            #line default
            #line hidden
            
            #line 162 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
         if (Model.VacancyType == VacancyType.Traineeship)
        {

            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteAttribute("class", Tuple.Create(" class=\"", 10105), Tuple.Create("\"", 10131)
            
            #line 164 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 10113), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 10113), false)
);

WriteLiteral(">\r\n                <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">\r\n                    Training to be provided\r\n");

WriteLiteral("                    ");

            
            #line 167 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.TrainingDetailsViewModel.TrainingProvided, Model.TrainingDetailsViewModel.TrainingProvidedComment, Model.TrainingDetailsLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </h3>\r\n                <p");

WriteLiteral(" id=\"vacancy-training-to-be-provided\"");

WriteAttribute("class", Tuple.Create(" class=\"", 10537), Tuple.Create("\"", 10625)
            
            #line 169 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 10545), Tuple.Create<System.Object, System.Int32>(Model.TrainingDetailsViewModel.TrainingProvided.GetPreserveFormattingCssClass()
            
            #line default
            #line hidden
, 10545), false)
);

WriteLiteral(">");

            
            #line 169 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                                                                                                                             Write(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.EscapeHtmlEncoding(Html, Model.TrainingDetailsViewModel.TrainingProvided));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("                ");

            
            #line 170 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
           Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.TrainingDetailsViewModel.TrainingProvided, Model.TrainingDetailsLink, Model.TrainingDetailsViewModel.TrainingProvidedComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n");

WriteLiteral("            <div");

WriteAttribute("class", Tuple.Create(" class=\"", 11030), Tuple.Create("\"", 11056)
            
            #line 172 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
, Tuple.Create(Tuple.Create("", 11038), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 11038), false)
);

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">\r\n                        Future prospects\r\n");

WriteLiteral("                        ");

            
            #line 176 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                   Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.VacancyRequirementsProspectsViewModel.FutureProspects, Model.VacancyRequirementsProspectsViewModel.FutureProspectsComment, Model.RequirementsProspectsLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-future-prospects\"");

WriteLiteral(" itemprop=\"incentives\"");

WriteAttribute("class", Tuple.Create(" class=\"", 11556), Tuple.Create("\"", 11656)
            
            #line 178 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
 , Tuple.Create(Tuple.Create("", 11564), Tuple.Create<System.Object, System.Int32>(Model.VacancyRequirementsProspectsViewModel.FutureProspects.GetPreserveFormattingCssClass()
            
            #line default
            #line hidden
, 11564), false)
);

WriteLiteral(">");

            
            #line 178 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
                                                                                                                                                                            Write(SFA.Apprenticeships.Web.Common.Framework.HtmlExtensions.EscapeHtmlEncoding(Html, Model.VacancyRequirementsProspectsViewModel.FutureProspects));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("                    ");

            
            #line 179 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
               Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.VacancyRequirementsProspectsViewModel.FutureProspects, Model.RequirementsProspectsLink, Model.VacancyRequirementsProspectsViewModel.FutureProspectsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n            </div>\r\n");

            
            #line 182 "..\..\Views\Shared\DisplayTemplates\Vacancy\_VacancyDetails.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</section>");

        }
    }
}
#pragma warning restore 1591
