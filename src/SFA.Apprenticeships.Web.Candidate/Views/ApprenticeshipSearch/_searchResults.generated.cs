﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SFA.Apprenticeships.Web.Candidate.Views.ApprenticeshipSearch
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
    
    #line 1 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Candidate;
    using SFA.Apprenticeships.Web.Candidate.Constants;
    using SFA.Apprenticeships.Web.Candidate.Constants.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.Helpers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Locations;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Login;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipSearch/_searchResults.cshtml")]
    public partial class searchResults : System.Web.Mvc.WebViewPage<ApprenticeshipSearchResponseViewModel>
    {
        public searchResults()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 5 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
 if (Model.TotalLocalHits > 0 || Model.TotalNationalHits > 0)
{

            
            #line default
            #line hidden
WriteLiteral("    <fieldset>\r\n        <legend");

WriteLiteral(" class=\"visuallyhidden\"");

WriteLiteral(">Search items</legend>\r\n        <div");

WriteLiteral(" class=\"float-right-wrap\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"form-group form-group-compound\"");

WriteLiteral(">\r\n                <label");

WriteLiteral(" for=\"sort-results\"");

WriteLiteral(" class=\"heading-medium inline\"");

WriteLiteral(">Sort results</label>\r\n");

WriteLiteral("                ");

            
            #line 12 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
           Write(Html.DropDownList("sortType", Model.SortTypes, new { @id = "sort-results" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                <noscript>\r\n                    <button");

WriteLiteral(" class=\"button show-nojs\"");

WriteLiteral(" name=\"SearchAction\"");

WriteLiteral(" value=\"Sort\"");

WriteLiteral(">Sort</button>\r\n                </noscript>\r\n                <input");

WriteLiteral(" id=\"SearchAction\"");

WriteLiteral(" name=\"SearchAction\"");

WriteLiteral(" value=\"Search\"");

WriteLiteral(" type=\"hidden\"");

WriteLiteral(" />\r\n            </div>\r\n        </div>\r\n    </fieldset>\r\n");

            
            #line 20 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("<ul");

WriteLiteral(" class=\"search-results\"");

WriteLiteral(">\r\n");

            
            #line 22 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
    
            
            #line default
            #line hidden
            
            #line 22 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
     if (Model.Vacancies != null)
    {
        var itemPosition = 1 + (Model.PageSize * Model.PrevPage);
        foreach (var vacancy in Model.Vacancies)
        {
            var webTrendItemPositionTracker = "Webtrends.multiTrack({ element: this, argsa: ['DCS.dcsuri', '/apprenticeships/results/itemposition/" + vacancy.Id + "', 'WT.dl', '99', 'WT.ti', 'Apprenticeships Search – Item Position Clicked', 'DCSext.ipos', '" + itemPosition + "'] })";

            
            #line default
            #line hidden
WriteLiteral("            <li");

WriteLiteral(" class=\"search-results__item section-border\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"hgroup-medium top-align width-all-3-4\"");

WriteLiteral(">\r\n                    <h2");

WriteLiteral(" class=\"heading-medium vacancy-title-link\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 31 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                    Write(vacancy.VacancyLocationType == ApprenticeshipLocationType.NonNational ?
                            Html.ActionLink(@vacancy.Title, "DetailsWithDistance", new { id = vacancy.Id, distance = vacancy.DistanceAsString }, new { @class = "vacancy-link", data_vacancy_id = vacancy.Id, onclick = webTrendItemPositionTracker, data_lat = vacancy.Location.Latitude, data_lon = vacancy.Location.Longitude }) :
                            Html.ActionLink(@vacancy.Title, "DetailsWithDistance", new { id = vacancy.Id, distance = vacancy.DistanceAsString }, new { @class = "vacancy-link", data_vacancy_id = vacancy.Id, onclick = webTrendItemPositionTracker })
                        );

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </h2>\r\n                    <p");

WriteAttribute("id", Tuple.Create(" id=\"", 2293), Tuple.Create("\"", 2321)
, Tuple.Create(Tuple.Create("", 2298), Tuple.Create("posted-date-", 2298), true)
            
            #line 36 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
, Tuple.Create(Tuple.Create("", 2310), Tuple.Create<System.Object, System.Int32>(vacancy.Id
            
            #line default
            #line hidden
, 2310), false)
);

WriteLiteral(" class=\"subtitle\"");

WriteLiteral(">");

            
            #line 36 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                                                                Write(vacancy.EmployerName);

            
            #line default
            #line hidden
WriteLiteral("<span");

WriteLiteral(" class=\"font-xsmall\"");

WriteLiteral("> (Added ");

            
            #line 36 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                                                                                                                       Write(vacancy.PostedDate.ToFriendlyDaysAgo());

            
            #line default
            #line hidden
WriteLiteral(")</span></p>\r\n                </div>\r\n");

            
            #line 38 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                
            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                 if (ViewBag.IsCandidateActivated != null && ViewBag.IsCandidateActivated)
                {
                    var applicationStatus = vacancy.CandidateApplicationStatus.HasValue
                        ? vacancy.CandidateApplicationStatus.ToString()
                        : "Unsaved";


            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteLiteral(" class=\"top-align ta-right width-all-1-4\"");

WriteLiteral(">\r\n                        <a");

WriteLiteral(" href=\"#\"");

WriteAttribute("id", Tuple.Create(" id=\"", 2888), Tuple.Create("\"", 2922)
, Tuple.Create(Tuple.Create("", 2893), Tuple.Create("save-vacancy-link-", 2893), true)
            
            #line 45 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
, Tuple.Create(Tuple.Create("", 2911), Tuple.Create<System.Object, System.Int32>(vacancy.Id
            
            #line default
            #line hidden
, 2911), false)
);

WriteLiteral(" class=\"hidden fake-link link-unimp save-vacancy-link\"");

WriteLiteral(" data-vacancy-id=\"");

            
            #line 45 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                                                                                                                                         Write(vacancy.Id);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(" data-application-status=\"");

            
            #line 45 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                                                                                                                                                                               Write(applicationStatus);

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa\"");

WriteLiteral("></i></a>\r\n");

WriteLiteral("                        ");

            
            #line 46 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                   Write(Html.ActionLink("Resume", "Resume", "ApprenticeshipApplication", new { id = vacancy.Id }, new { @class = "hidden resume-link", @id = "resume-link-" + @vacancy.Id }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        <p");

WriteAttribute("id", Tuple.Create(" id=\"", 3294), Tuple.Create("\"", 3324)
, Tuple.Create(Tuple.Create("", 3299), Tuple.Create("applied-label-", 3299), true)
            
            #line 47 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
, Tuple.Create(Tuple.Create("", 3313), Tuple.Create<System.Object, System.Int32>(vacancy.Id
            
            #line default
            #line hidden
, 3313), false)
);

WriteLiteral(" class=\"hidden applied-label\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-check\"");

WriteLiteral("></i>Applied</p>\r\n                    </div>\r\n");

            
            #line 49 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("                <p>");

            
            #line 50 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
              Write(vacancy.Description);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                <div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 3555), Tuple.Create("\"", 3658)
            
            #line 52 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
, Tuple.Create(Tuple.Create("", 3563), Tuple.Create<System.Object, System.Int32>(vacancy.VacancyLocationType == ApprenticeshipLocationType.NonNational ? "grid grid-2-3" : ""
            
            #line default
            #line hidden
, 3563), false)
);

WriteLiteral(">\r\n                        <ul");

WriteLiteral(" class=\"list-text no-btm-margin inner-block-padr\"");

WriteLiteral(">\r\n\r\n");

            
            #line 55 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                            
            
            #line default
            #line hidden
            
            #line 55 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                             if (vacancy.VacancyLocationType == ApprenticeshipLocationType.NonNational)
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <li>\r\n                                    <b>Dist" +
"ance:</b> <span");

WriteLiteral(" class=\"distance-value\"");

WriteLiteral(">");

            
            #line 58 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                                                                             Write(vacancy.DistanceAsString);

            
            #line default
            #line hidden
WriteLiteral("</span> miles<span");

WriteLiteral(" class=\"summary-style fake-link hide-nojs journey-trigger\"");

WriteLiteral(">Journey time</span>\r\n                                    <div");

WriteLiteral(" class=\"detail-content toggle-content hide-nojs\"");

WriteLiteral(">\r\n                                        <select");

WriteLiteral(" class=\"select-mode\"");

WriteLiteral(" name=\"\"");

WriteLiteral(">\r\n                                            <option");

WriteLiteral(" value=\"DRIVING\"");

WriteLiteral(">Driving</option>\r\n                                            <option");

WriteLiteral(" value=\"TRANSIT\"");

WriteLiteral(">Bus/Train</option>\r\n                                            <option");

WriteLiteral(" value=\"WALKING\"");

WriteLiteral(">Walking</option>\r\n                                            <option");

WriteLiteral(" value=\"BICYCLING\"");

WriteLiteral(">Cycling</option>\r\n                                        </select>\r\n\r\n         " +
"                               <span");

WriteLiteral(" class=\"journey-time\"");

WriteLiteral("></span>\r\n                                    </div>\r\n                           " +
"     </li>\r\n");

            
            #line 70 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                            }

            
            #line default
            #line hidden
WriteLiteral("                            <li><b>Closing date:</b> <span");

WriteLiteral(" class=\"closing-date-value\"");

WriteLiteral(" data-date=\"");

            
            #line 71 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                                                                                            Write(vacancy.ClosingDate.ToString("u"));

            
            #line default
            #line hidden
WriteLiteral("\"");

WriteLiteral(">");

            
            #line 71 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                                                                                                                                Write(vacancy.ClosingDate.ToFriendlyClosingWeek());

            
            #line default
            #line hidden
WriteLiteral("</span></li>\r\n                            <li><b>Possible start date:</b> <span");

WriteLiteral(" class=\"start-date-value\"");

WriteLiteral(">");

            
            #line 72 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                                                                                      Write(Html.DisplayFor(m => vacancy.StartDate, "Date"));

            
            #line default
            #line hidden
WriteLiteral("</span></li>\r\n\r\n");

            
            #line 74 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                            
            
            #line default
            #line hidden
            
            #line 74 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                             if (vacancy.VacancyLocationType == ApprenticeshipLocationType.National)
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <li>This apprenticeship has multiple positions na" +
"tionwide.</li>\r\n");

            
            #line 77 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                            }

            
            #line default
            #line hidden
WriteLiteral("                        </ul>\r\n                    </div>\r\n");

            
            #line 80 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 80 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                     if (vacancy.VacancyLocationType == ApprenticeshipLocationType.NonNational)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" class=\"hide-tablet\"");

WriteLiteral(">\r\n                            <span");

WriteLiteral(" style=\"margin-left: 0;\"");

WriteLiteral(" class=\"summary-style fake-link mob-map-trigger map-closed\"");

WriteLiteral(">Show/hide map</span>\r\n                        </div>\r\n");

WriteLiteral("                        <div");

WriteLiteral(" class=\"grid grid-1-3 map-container hide-nojs small-btm-margin toggle-content--mo" +
"b\"");

WriteLiteral(">\r\n                            <a");

WriteAttribute("href", Tuple.Create(" href=\"", 6034), Tuple.Create("\"", 6144)
, Tuple.Create(Tuple.Create("", 6041), Tuple.Create("https://www.google.com/maps/dir/LocationLatLon/\'", 6041), true)
            
            #line 86 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
     , Tuple.Create(Tuple.Create("", 6089), Tuple.Create<System.Object, System.Int32>(vacancy.Location.Latitude
            
            #line default
            #line hidden
, 6089), false)
, Tuple.Create(Tuple.Create("", 6115), Tuple.Create(",", 6115), true)
            
            #line 86 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                                , Tuple.Create(Tuple.Create("", 6116), Tuple.Create<System.Object, System.Int32>(vacancy.Location.Longitude
            
            #line default
            #line hidden
, 6116), false)
, Tuple.Create(Tuple.Create("", 6143), Tuple.Create("\'", 6143), true)
);

WriteLiteral(" target=\"_blank\"");

WriteLiteral(" rel=\"external\"");

WriteLiteral(" class=\"map-links fake-link font-xxsmall view-googlemaps\"");

WriteLiteral(">Open map</a>\r\n                            <div");

WriteLiteral(" class=\"map\"");

WriteLiteral("></div>\r\n                        </div>\r\n");

            
            #line 89 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n\r\n            </li>\r\n");

            
            #line 93 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
            itemPosition++;
        }
    }

            
            #line default
            #line hidden
WriteLiteral("</ul>\r\n\r\n");

            
            #line 98 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
   Html.RenderPartial("_pagination", Model); 
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 100 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
 if (Model.TotalLocalHits > 5 || Model.TotalNationalHits > 5)
{

            
            #line default
            #line hidden
WriteLiteral("<div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n    <label");

WriteLiteral(" for=\"results-per-page\"");

WriteLiteral(" class=\"heading-small inline\"");

WriteLiteral(">Display results</label>\r\n");

WriteLiteral("    ");

            
            #line 104 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
Write(Html.DropDownList("resultsPerPage", Model.ResultsPerPageSelectList, new { @id = "results-per-page" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <noscript>\r\n        <button");

WriteLiteral(" class=\"button show-nojs\"");

WriteLiteral(" name=\"ChangeResultsPerPageAction\"");

WriteLiteral(" value=\"ResultsPerPage\"");

WriteLiteral(">View</button>\r\n    </noscript>\r\n</div>\r\n");

            
            #line 109 "..\..\Views\ApprenticeshipSearch\_searchResults.cshtml"
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
