﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
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
    
    #line 1 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
    using SFA.Apprenticeships.Application.Interfaces.Vacancies;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Candidate;
    using SFA.Apprenticeships.Web.Candidate.Constants;
    using SFA.Apprenticeships.Web.Candidate.Constants.ViewModels;
    
    #line 3 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
    using SFA.Apprenticeships.Web.Candidate.Extensions;
    
    #line default
    #line hidden
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipSearch/Results.cshtml")]
    public partial class Results : System.Web.Mvc.WebViewPage<ApprenticeshipSearchResponseViewModel>
    {
        public Results()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 7 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
  
    ViewBag.Title = "Results - Find an apprenticeship";
    Layout = "~/Views/Shared/_Layout.cshtml";

    //Any copy changes here also need reflected in the resultsSearch.js file.
    var locationTypeLink = Model.VacancySearch.LocationType == ApprenticeshipLocationType.National 
            ? Url.ApprenticeshipSearchViewModelRouteUrl(CandidateRouteNames.ApprenticeshipResults, new ApprenticeshipSearchViewModel(Model.VacancySearch) {LocationType = ApprenticeshipLocationType.NonNational, SearchAction = SearchAction.LocationTypeChanged, PageNumber = 1}) 
            : Url.ApprenticeshipSearchViewModelRouteUrl(CandidateRouteNames.ApprenticeshipResults, new ApprenticeshipSearchViewModel(Model.VacancySearch) {LocationType = ApprenticeshipLocationType.National, SearchAction = SearchAction.LocationTypeChanged, SortType = VacancySearchSortType.Distance, PageNumber = 1});

    string resultMessage;
    string nationalResultsMessage;

    if (Model.TotalLocalHits == 0)
    {
        resultMessage = "";
    }
    else if (Model.TotalLocalHits == 1)
    {
        if (Model.VacancySearch.LocationType == ApprenticeshipLocationType.National)
        {
            resultMessage = "We've found <b class=\"bold-medium\">1</b> <a id='localLocationTypeLink' class='update-results' href=" + locationTypeLink + ">apprenticeship in your selected area</a>.";
        }
        else
        {
            resultMessage = "We've found <b class=\"bold-medium\">1</b> apprenticeship in your selected area.";
        }
    }
    else
    {
        if (Model.VacancySearch.LocationType == ApprenticeshipLocationType.National)
        {
            var message = "We've found <b class=\"bold-medium\">{0}</b> <a id='localLocationTypeLink' class='update-results' href=" + locationTypeLink + ">apprenticeships in your selected area</a>.";
            resultMessage = string.Format(message, Model.TotalLocalHits);
        }
        else
        {
            resultMessage = string.Format("We've found <b class=\"bold-medium\">{0}</b> apprenticeships in your selected area.", Model.TotalLocalHits);
        }
    }

    if (Model.TotalLocalHits == 0 && Model.TotalNationalHits != 0)
    {
        resultMessage = "We were unable to find any apprenticeships in your selected area.";
    }

    if (Model.TotalNationalHits == 0)
    {
        nationalResultsMessage = "";
    }
    else
    {
        var nationalResultsMessagePrefix = Model.TotalLocalHits == 0 ? "We've found" : "We've also found";

        if (Model.TotalNationalHits == 1)
        {

            if (Model.VacancySearch.LocationType == ApprenticeshipLocationType.NonNational)
            {
                nationalResultsMessage = string.Format("{1} <a id='nationwideLocationTypeLink' class='update-results' href={0}>1 apprenticeship with positions across England</a>.", locationTypeLink, nationalResultsMessagePrefix);
            }
            else
            {
                nationalResultsMessage = nationalResultsMessagePrefix + " 1 apprenticeship with positions across England.";
            }
        }
        else
        {
            if (Model.VacancySearch.LocationType == ApprenticeshipLocationType.NonNational)
            {
                nationalResultsMessage = string.Format("{2} <a id='nationwideLocationTypeLink' class='update-results' href={1}>{0} apprenticeships with positions across England</a>.", Model.TotalNationalHits, locationTypeLink, nationalResultsMessagePrefix);
            }
            else
            {
                nationalResultsMessage = string.Format("{1} {0} apprenticeships with positions across England.", Model.TotalNationalHits, nationalResultsMessagePrefix);
            }
        }
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

DefineSection("metatags", () => {

WriteLiteral("\r\n    ");

WriteLiteral("\r\n    <meta");

WriteLiteral(" name=\"WT.oss_r\"");

WriteAttribute("content", Tuple.Create(" content=\"", 4130), Tuple.Create("\"", 4161)
            
            #line 89 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
, Tuple.Create(Tuple.Create("", 4140), Tuple.Create<System.Object, System.Int32>(Model.TotalLocalHits
            
            #line default
            #line hidden
, 4140), false)
);

WriteLiteral(" />\r\n");

});

WriteLiteral("\r\n<div");

WriteLiteral(" class=\"search-results-wrapper grid-wrapper\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"hgroup\"");

WriteLiteral(">\r\n        <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">Search results</h1>\r\n        <div");

WriteLiteral(" id=\"vacancy-result-summary\"");

WriteLiteral(">\r\n            <p");

WriteLiteral(" id=\"result-message\"");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">");

            
            #line 96 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                                                       Write(Html.Raw(resultMessage));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n            <p");

WriteLiteral(" id=\"national-results-message\"");

WriteLiteral(" class=\"\"");

WriteLiteral(">");

            
            #line 97 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                                                 Write(Html.Raw(nationalResultsMessage));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 98 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
            
            
            #line default
            #line hidden
            
            #line 98 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
             if (!string.IsNullOrEmpty(Model.VacancySearch.Location))
            {

            
            #line default
            #line hidden
WriteLiteral("                <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">\r\n                    <a");

WriteLiteral(" id=\"receiveSaveSearchAlert\"");

WriteAttribute("href", Tuple.Create(" \r\n                       href=\"", 4713), Tuple.Create("\"", 4855)
            
            #line 102 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
, Tuple.Create(Tuple.Create("", 4745), Tuple.Create<System.Object, System.Int32>(Url.ApprenticeshipSearchViewModelAction("savesearch", new ApprenticeshipSearchViewModel(Model.VacancySearch))
            
            #line default
            #line hidden
, 4745), false)
);

WriteLiteral("\r\n                       onclick=\"Webtrends.multiTrack({ element: this, argsa: [\'" +
"DCS.dcsuri\', \'/apprenticeships/receivealerts\', \'WT.dl\', \'99\', \'WT.ti\', \'Search R" +
"esults Receive Alerts\'] })\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-bell-o\"");

WriteLiteral("></i>Receive alerts for this search</a>\r\n                </p>\r\n");

            
            #line 105 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </div>\r\n\r\n");

            
            #line 109 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
    
            
            #line default
            #line hidden
            
            #line 109 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
     using (Html.BeginRouteForm(CandidateRouteNames.ApprenticeshipResults, FormMethod.Get))
    {
        Html.Partial("ValidationSummary", ViewData.ModelState);
        Html.RenderPartial("_searchUpdate", Model.VacancySearch);


            
            #line default
            #line hidden
WriteLiteral("        <section");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" id=\"pagedList\"");

WriteLiteral(">\r\n");

            
            #line 116 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                
            
            #line default
            #line hidden
            
            #line 116 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                   Html.RenderPartial("_searchResults", Model); 
            
            #line default
            #line hidden
WriteLiteral("\r\n            </div>\r\n        </section>\r\n");

            
            #line 119 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n\r\n<div");

WriteLiteral(" class=\"saving-prompt toggle-content hide-nojs\"");

WriteLiteral(" id=\"ajaxLoading\"");

WriteLiteral(">\r\n    Loading\r\n</div>\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 128 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
Write(Scripts.Render("~/bundles/nas/locationsearch"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 129 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
Write(Scripts.Render("~/bundles/cookie"));

            
            #line default
            #line hidden
WriteLiteral(" \r\n    <script>\r\n        var searchUrl = \'");

            
            #line 131 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                    Write(Url.RouteUrl(CandidateRouteNames.ApprenticeshipResults));

            
            #line default
            #line hidden
WriteLiteral("\';\r\n        var locationUrl = \'");

            
            #line 132 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                      Write(Url.Action("location", "Location"));

            
            #line default
            #line hidden
WriteLiteral("\';\r\n    </script>\r\n");

WriteLiteral("    ");

            
            #line 134 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
Write(Scripts.Render("~/bundles/nas/results"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <script>\r\n        initSavedVacancies({\r\n            saveUrl:  \'");

            
            #line 137 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                  Write(Url.Action("SaveVacancy", "ApprenticeshipApplication"));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            deleteUrl: \'");

            
            #line 138 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                   Write(Url.Action("DeleteSavedVacancy", "ApprenticeshipApplication"));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            title: true});\r\n    </script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
