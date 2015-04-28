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
    
    #line 1 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
    using SFA.Apprenticeships.Application.Interfaces.Vacancies;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Candidate;
    using SFA.Apprenticeships.Web.Candidate.Constants;
    using SFA.Apprenticeships.Web.Candidate.Constants.ViewModels;
    
    #line 4 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
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

            
            #line 8 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
  
    ViewBag.Title = "Results - Find an apprenticeship";
    Layout = "~/Views/Shared/_Layout.cshtml";

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
            resultMessage = "We've found <b class=\"bold-medium\">1</b> <a id='localLocationTypeLink' href=" + locationTypeLink + ">apprenticeship in your selected area</a>.";
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
            var message = "We've found <b class=\"bold-medium\">{0}</b> <a id='localLocationTypeLink' href=" + locationTypeLink + ">apprenticeships in your selected area</a>.";
            resultMessage = string.Format(message, Model.TotalLocalHits);
        }
        else
        {
            resultMessage = string.Format("We've found <b class=\"bold-medium\">{0}</b> apprenticeships in your selected area.", Model.TotalLocalHits);
        }
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
                nationalResultsMessage = string.Format("{1} <a id='nationwideLocationTypeLink' href={0}>1 apprenticeship nationwide</a>.", locationTypeLink, nationalResultsMessagePrefix);
            }
            else
            {
                nationalResultsMessage = nationalResultsMessagePrefix + " 1 apprenticeship nationwide.";
            }
        }
        else
        {
            if (Model.VacancySearch.LocationType == ApprenticeshipLocationType.NonNational)
            {
                nationalResultsMessage = string.Format("{2} <a id='nationwideLocationTypeLink' href={1}>{0} apprenticeships nationwide</a>.", Model.TotalNationalHits, locationTypeLink, nationalResultsMessagePrefix);
            }
            else
            {
                nationalResultsMessage = string.Format("{1} {0} apprenticeships nationwide.", Model.TotalNationalHits, nationalResultsMessagePrefix);
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

WriteAttribute("content", Tuple.Create(" content=\"", 3759), Tuple.Create("\"", 3790)
            
            #line 84 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
, Tuple.Create(Tuple.Create("", 3769), Tuple.Create<System.Object, System.Int32>(Model.TotalLocalHits
            
            #line default
            #line hidden
, 3769), false)
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

            
            #line 91 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                                                       Write(Html.Raw(resultMessage));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n            <p");

WriteLiteral(" id=\"national-results-message\"");

WriteLiteral(">");

            
            #line 92 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                                        Write(Html.Raw(nationalResultsMessage));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 93 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
            
            
            #line default
            #line hidden
            
            #line 93 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
             if (!string.IsNullOrEmpty(Model.VacancySearch.Location))
            {

            
            #line default
            #line hidden
WriteLiteral("                <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">\r\n                    <a");

WriteLiteral(" id=\"receiveSaveSearchAlert\"");

WriteAttribute("href", Tuple.Create(" \r\n                       href=\"", 4333), Tuple.Create("\"", 4475)
            
            #line 97 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
, Tuple.Create(Tuple.Create("", 4365), Tuple.Create<System.Object, System.Int32>(Url.ApprenticeshipSearchViewModelAction("savesearch", new ApprenticeshipSearchViewModel(Model.VacancySearch))
            
            #line default
            #line hidden
, 4365), false)
);

WriteLiteral("\r\n                       onclick=\"Webtrends.multiTrack({ element: this, argsa: [\'" +
"DCS.dcsuri\', \'/apprenticeships/receivealerts\', \'WT.dl\', \'99\', \'WT.ti\', \'Search R" +
"esults Receive Alerts\'] })\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-bell-o\"");

WriteLiteral("></i>Receive alerts for this search</a>\r\n                </p>\r\n");

            
            #line 100 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </div>\r\n\r\n");

            
            #line 104 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
    
            
            #line default
            #line hidden
            
            #line 104 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
     using (Html.BeginRouteForm(CandidateRouteNames.ApprenticeshipResults, FormMethod.Get))
    {
        Html.Partial("ValidationSummary", ViewData.ModelState);
        Html.RenderPartial("_searchUpdate", Model.VacancySearch);


            
            #line default
            #line hidden
WriteLiteral("        <section");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n");

            
            #line 110 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
            
            
            #line default
            #line hidden
            
            #line 110 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
              
                if (Model.TotalLocalHits == 0 && Model.TotalNationalHits == 0)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <p");

WriteLiteral(" id=\"search-no-results-title\"");

WriteLiteral(">There are currently no apprenticeships that match your search.</p>\r\n");

WriteLiteral("                    <p>Try editing your search:</p>\r\n");

WriteLiteral("                    <ul");

WriteLiteral(" id=\"search-no-results\"");

WriteLiteral(">\r\n");

            
            #line 116 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 116 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                         if (Model.VacancySearch.SearchMode == ApprenticeshipSearchMode.Keyword)
                        {
                            if (VacancyHelper.IsVacancyReference(Model.VacancySearch.Keywords))
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <li");

WriteLiteral(" id=\"search-no-results-reference-number\"");

WriteLiteral(">try a different reference number</li>\r\n");

            
            #line 121 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                            }
                            else
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <li");

WriteLiteral(" id=\"search-no-results-keywords\"");

WriteLiteral(">using different keywords</li>\r\n");

            
            #line 125 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                            }
                        }

            
            #line default
            #line hidden
WriteLiteral("                        ");

            
            #line 127 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                         if (Model.VacancySearch.SearchMode == ApprenticeshipSearchMode.Category)
                        {
                            if (Model.VacancySearch.SubCategories == null || Model.VacancySearch.SubCategories.Length == 0)
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <li");

WriteLiteral(" id=\"search-no-results-category\"");

WriteLiteral(">try a different category</li>\r\n");

            
            #line 132 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                            }
                            else
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <li");

WriteLiteral(" id=\"search-no-results-sub-category\"");

WriteLiteral(">select a different sub-category or sub-categories</li>\r\n");

            
            #line 136 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                            }
                        }

            
            #line default
            #line hidden
WriteLiteral("                        <li>expanding your search location</li>\r\n");

            
            #line 139 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 139 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                         if (Model.VacancySearch.ApprenticeshipLevel != "All")
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <li");

WriteLiteral(" id=\"search-no-results-apprenticeship-levels\"");

WriteLiteral(">using a different level, or change to all levels</li>\r\n");

            
            #line 142 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </ul>\r\n");

            
            #line 144 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                }
                else
                {

            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteLiteral(" id=\"pagedList\"");

WriteLiteral(">\r\n");

            
            #line 148 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 148 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                           Html.RenderPartial("_searchResults", Model); 
            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n");

            
            #line 150 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                }
            
            
            #line default
            #line hidden
WriteLiteral("\r\n        </section>\r\n");

            
            #line 153 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 158 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
Write(Scripts.Render("~/bundles/nas/locationsearch"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 159 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
Write(Scripts.Render("~/bundles/nas/apprenticeships/results"));

            
            #line default
            #line hidden
WriteLiteral(";\r\n\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        $(function() {\r\n            $(\"#Location\").locationMatch({\r\n          " +
"      url: \'");

            
            #line 164 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                 Write(Url.Action("location", "Location"));

            
            #line default
            #line hidden
WriteLiteral(@"',
                longitude: '#Longitude',
                latitude: '#Latitude',
                latlonhash: '#Hash'
            });

            $('#sort-results').change(function() {
                $('#SearchAction').val(""Sort"");
                $(""form"").submit();
            });

            $('#results-per-page').change(function() {
                $('#SearchAction').val(""Sort"");
                $(""form"").submit();
            });

            $('#search-button').click(function() {
                $('#LocationType').val(""NonNational"");
            });

            initSavedVacancies({
                saveUrl: '");

            
            #line 185 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                     Write(Url.Action("SaveVacancy", "ApprenticeshipApplication"));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                deleteUrl: \'");

            
            #line 186 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
                       Write(Url.Action("DeleteSavedVacancy", "ApprenticeshipApplication"));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                title: true\r\n            });\r\n\r\n            $(\'#receiveSaveSe" +
"archAlert\').click(function() {\r\n                var $this = $(this),\r\n          " +
"          $href = $this.attr(\'href\');\r\n\r\n                //Append current result" +
"s detail view settings to query string so they are saved.\r\n                $href" +
" = $href + \'&\' + GetSearchResultsDetailsValues();\r\n\r\n                $this.attr(" +
"\'href\', $href);\r\n            });\r\n\r\n            $(\'#chooseDetails input\').each(f" +
"unction() {\r\n                var $this = $(this),\r\n                    $thisId =" +
" $this.attr(\'id\');\r\n\r\n                var $value = GetSearchResultsDetailsValue(" +
"$thisId);\r\n\r\n                if ($value != null) {\r\n                    var $cur" +
"rentlyChecked = $this.is(\':checked\');\r\n                    $this.prop(\"checked\"," +
" $value);\r\n                    if ($currentlyChecked !== $value) {\r\n            " +
"            $(\'[data-show=\"\' + $thisId + \'\"]\').toggle();\r\n                    }\r" +
"\n                }\r\n            });\r\n\r\n            //Write the new, complete coo" +
"kie with the current view of the display settings\r\n            SetSearchResultsD" +
"etailsCookieValue();\r\n\r\n            $(\'#chooseDetails input\').on(\'change\', funct" +
"ion() {\r\n                var $this = $(this),\r\n                    $thisId = $th" +
"is.attr(\'id\');\r\n\r\n                $(\'[data-show=\"\' + $thisId + \'\"]\').toggle();\r\n" +
"\r\n                //Write the new, complete cookie with the current view of the " +
"display settings\r\n                SetSearchResultsDetailsCookieValue();\r\n       " +
"     });\r\n        });\r\n\r\n        jQuery.extend({\r\n            getQueryParameters" +
": function(str) {\r\n                return (str || document.location.search).repl" +
"ace(/(^\\?)/, \'\').split(\"&\").map(function(n) { return n = n.split(\"=\"), this[n[0]" +
"] = n[1], this }.bind({}))[0];\r\n            }\r\n        });\r\n\r\n        jQuery.ext" +
"end({\r\n            getCookieValues: function (str) {\r\n                var cookie" +
"Values = {};\r\n\r\n                var cookie = $.cookie(str);\r\n\r\n                i" +
"f (typeof cookie !== \'undefined\' && cookie != null) {\r\n                    var c" +
"ookieComponents = cookie.split(\"&\");\r\n                    $.each(cookieComponent" +
"s, function (index, value) {\r\n                        var cookieComponent = valu" +
"e.split(\'=\');\r\n                        cookieValues[cookieComponent[0]] = cookie" +
"Component[1];\r\n                    });\r\n                }\r\n\r\n                ret" +
"urn cookieValues;\r\n            }\r\n        });\r\n\r\n        function GetSearchResul" +
"tsDetailsValue(searchResultDetail) {\r\n            //Get URL param first\r\n       " +
"     var queryParams = $.getQueryParameters();\r\n            var queryValue = que" +
"ryParams[searchResultDetail];\r\n            if (typeof queryValue !== \'undefined\'" +
" && queryValue != null) {\r\n                return queryValue.toLowerCase() === \'" +
"true\';\r\n            }\r\n\r\n            //If that fails use cookie value\r\n         " +
"   var cookieValues = $.getCookieValues(\'NAS.SearchResultsDetails\');\r\n          " +
"  var cookieValue = cookieValues[searchResultDetail];\r\n            if (typeof co" +
"okieValue !== \'undefined\' && cookieValue != null) {\r\n                return cook" +
"ieValue.toLowerCase() === \'true\';\r\n            }\r\n\r\n            //Otherwise use " +
"defaults\r\n            return null;\r\n        }\r\n\r\n        function GetSearchResul" +
"tsDetailsValues() {\r\n            //Assemble query string or cookie compatible va" +
"lue from inputs\r\n            var detailsValue = $(\'#chooseDetails input\').toArra" +
"y().map(function (value) {\r\n                return $(value).attr(\'id\') + \"=\" + $" +
"(value).is(\':checked\');\r\n            }).join(\"&\");\r\n\r\n            return details" +
"Value;\r\n        }\r\n\r\n        function SetSearchResultsDetailsCookieValue() {\r\n  " +
"          //Assemble cookie value from inputs\r\n            var cookieValue = Get" +
"SearchResultsDetailsValues();\r\n\r\n            //Need the cookie not to be encoded" +
" so it\'s compatible with MVC\r\n            $.cookie.raw = true;\r\n\r\n            //" +
"Write the new, complete cookie with the current view of the display settings\r\n  " +
"          $.cookie(\'NAS.SearchResultsDetails\', cookieValue);\r\n        }\r\n    </s" +
"cript>\r\n\r\n");

WriteLiteral("    ");

            
            #line 293 "..\..\Views\ApprenticeshipSearch\Results.cshtml"
Write(Scripts.Render("~/bundles/nas/resultsMap"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

});

        }
    }
}
#pragma warning restore 1591
