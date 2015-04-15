﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
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
    
    #line 1 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipSearch/Index.cshtml")]
    public partial class Index : System.Web.Mvc.WebViewPage<ApprenticeshipSearchViewModel>
    {
        public Index()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
  
    ViewBag.Title = "Find an apprenticeship";
    Layout = "~/Views/Shared/_Layout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"hgroup\"");

WriteLiteral(">\r\n    <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">Find an apprenticeship</h1>\r\n    <p");

WriteLiteral(" class=\"subtitle\"");

WriteLiteral(">Search and apply for an apprenticeship in England</p>\r\n</div>\r\n<div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(" id=\"searchHome\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"grid-wrapper controls-3-4\"");

WriteLiteral(">\r\n        <section");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n");

            
            #line 15 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 15 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
             using (Html.BeginRouteForm(CandidateRouteNames.ApprenticeshipSearchValidation, FormMethod.Get, new { @id = "#searchForm" }))
            {
                
            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
           Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                                                                       


            
            #line default
            #line hidden
WriteLiteral("                <nav");

WriteLiteral(" class=\"tabbed-nav\"");

WriteLiteral(">\r\n");

            
            #line 20 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 20 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                      
                        var keywordTabControlClass = Model.SearchMode == ApprenticeshipSearchMode.Keyword ? " active" : "";
                        var categoriesTabControlClass = Model.SearchMode == ApprenticeshipSearchMode.Category ? " active" : "";
                        var savedSearchesTabControlClass = Model.SearchMode == ApprenticeshipSearchMode.SavedSearches ? " active" : "";

                        var anyCategories = Model.Categories != null && Model.Categories.Any();
                        var categoriesElementControlClass = Model.SearchMode == ApprenticeshipSearchMode.Category && anyCategories ? " active" : "";
                        var categoriesTabClass = anyCategories ? " tab2" : "";
                        
                        var anySavedSearches = Model.SavedSearches != null && Model.SavedSearches.Any();
                        var savedSearchesElementControlClass = Model.SearchMode == ApprenticeshipSearchMode.SavedSearches && anySavedSearches ? " active" : "";
                        var savedSearchesTabClass = anySavedSearches ? " tab3" : "";

                        string elementControlClass = string.Empty;

                        switch (Model.SearchMode)
                        {
                            case ApprenticeshipSearchMode.Keyword:
                                elementControlClass = keywordTabControlClass;
                                break;
                            case ApprenticeshipSearchMode.Category:
                                elementControlClass = categoriesElementControlClass;
                                break;
                            case ApprenticeshipSearchMode.SavedSearches:
                                break;
                        }
                    
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("                    ");

            
            #line 48 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.RouteLink("Search", CandidateRouteNames.ApprenticeshipSearch, new { searchMode = ApprenticeshipSearchMode.Keyword }, new { @id = "keywords-tab-control", @class = "tabbed-tab" + keywordTabControlClass, tab = "#tab1" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 49 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.RouteLink("Browse", CandidateRouteNames.ApprenticeshipSearch, new { searchMode = ApprenticeshipSearchMode.Category }, new { @id = "categories-tab-control", @class = "tabbed-tab" + categoriesTabControlClass, tab = "#tab2" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    \r\n");

            
            #line 51 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 51 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                     if (anySavedSearches)
                    {
                        
            
            #line default
            #line hidden
            
            #line 53 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                   Write(Html.RouteLink("Saved searches", CandidateRouteNames.ApprenticeshipSearch, new { searchMode = ApprenticeshipSearchMode.SavedSearches }, new { @id = "saved-searches-tab-control", @class = "tabbed-tab" + savedSearchesTabControlClass, tab = "#tab3" }));

            
            #line default
            #line hidden
            
            #line 53 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                                                                                                                                                                                                                                                                                 
                    }

            
            #line default
            #line hidden
WriteLiteral("                </nav>\r\n");

            
            #line 56 "..\..\Views\ApprenticeshipSearch\Index.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"tabbed-content active\"");

WriteLiteral(">\r\n");

WriteLiteral("                    ");

            
            #line 58 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.Partial("_categories", Model));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    \r\n");

WriteLiteral("                    ");

            
            #line 60 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.Partial("_savedSearches", Model));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    \r\n");

WriteLiteral("                    ");

            
            #line 62 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.DropDownListFor(m => m.SearchField, Model.SearchFields, new { @class = "refineSearchOption hidden input-withlink__select all-select" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 63 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.FormTextFor(m => m.Keywords, controlHtmlAttributes: new { autofocus = "autofocus", aria_describedby = "keywordsHint" }, containerHtmlAttributes: new { @class = "tabbed-element tab1" + keywordTabControlClass }, labelHtmlAttributes: new { id = "keyword-label" }, hintHtmlAttributes: new { id = "keyword-hint" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                    <p");

WriteLiteral(" class=\"visuallyhidden\"");

WriteLiteral(" id=\"keywordsHint\"");

WriteLiteral(">Try words that describe the type of apprenticeship you want, for example “carpen" +
"try” or “mechanics”.</p>\r\n                    \r\n");

WriteLiteral("                    ");

            
            #line 67 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.FormTextFor(m => m.Location, containerHtmlAttributes: new { @class = "tabbed-element tab1 " + keywordTabControlClass + categoriesTabClass + elementControlClass }, hintHtmlAttributes: new { id = "geoLocateContainer" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 4690), Tuple.Create("\"", 4789)
, Tuple.Create(Tuple.Create("", 4698), Tuple.Create("inline", 4698), true)
, Tuple.Create(Tuple.Create(" ", 4704), Tuple.Create("tabbed-element", 4705), true)
, Tuple.Create(Tuple.Create(" ", 4719), Tuple.Create("tab1", 4720), true)
            
            #line 69 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 4724), Tuple.Create<System.Object, System.Int32>(keywordTabControlClass
            
            #line default
            #line hidden
, 4725), false)
            
            #line 69 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
  , Tuple.Create(Tuple.Create(" ", 4748), Tuple.Create<System.Object, System.Int32>(categoriesTabClass
            
            #line default
            #line hidden
, 4749), false)
            
            #line 69 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                      , Tuple.Create(Tuple.Create(" ", 4768), Tuple.Create<System.Object, System.Int32>(elementControlClass
            
            #line default
            #line hidden
, 4769), false)
);

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                            <label");

WriteLiteral(" for=\"loc-within\"");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Within</label>\r\n");

WriteLiteral("                            ");

            
            #line 72 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                       Write(Html.DropDownListFor(m => m.WithinDistance, Model.Distances, new { @id = "loc-within", @name = "WithinDistance" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                            <label");

WriteLiteral(" for=\"apprenticeship-level\"");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship level</label>\r\n");

WriteLiteral("                            ");

            
            #line 76 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                       Write(Html.DropDownListFor(m => m.ApprenticeshipLevel, Model.ApprenticeshipLevels, new { @id = "apprenticeship-level", @name = "ApprenticeshipLevel" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </div>\r\n                    </div>\r\n\r\n                 " +
"   <div");

WriteLiteral(" class=\"form-group form-group-compound\"");

WriteLiteral(">\r\n                        <button");

WriteAttribute("class", Tuple.Create(" class=\"", 5603), Tuple.Create("\"", 5661)
, Tuple.Create(Tuple.Create("", 5611), Tuple.Create("button", 5611), true)
, Tuple.Create(Tuple.Create(" ", 5617), Tuple.Create("tabbed-element", 5618), true)
, Tuple.Create(Tuple.Create(" ", 5632), Tuple.Create("tab1", 5633), true)
            
            #line 81 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 5637), Tuple.Create<System.Object, System.Int32>(keywordTabControlClass
            
            #line default
            #line hidden
, 5638), false)
);

WriteLiteral(" id=\"search-button\"");

WriteLiteral(">Search</button>\r\n                        <button");

WriteAttribute("class", Tuple.Create(" class=\"", 5730), Tuple.Create("\"", 5810)
, Tuple.Create(Tuple.Create("", 5738), Tuple.Create("button", 5738), true)
, Tuple.Create(Tuple.Create(" ", 5744), Tuple.Create("tabbed-element", 5745), true)
            
            #line 82 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 5759), Tuple.Create<System.Object, System.Int32>(categoriesTabClass
            
            #line default
            #line hidden
, 5760), false)
            
            #line 82 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 5779), Tuple.Create<System.Object, System.Int32>(categoriesElementControlClass
            
            #line default
            #line hidden
, 5780), false)
);

WriteLiteral(" id=\"browse-button\"");

WriteLiteral(">Browse</button>\r\n                        <button");

WriteAttribute("class", Tuple.Create(" class=\"", 5879), Tuple.Create("\"", 5965)
, Tuple.Create(Tuple.Create("", 5887), Tuple.Create("button", 5887), true)
, Tuple.Create(Tuple.Create(" ", 5893), Tuple.Create("tabbed-element", 5894), true)
            
            #line 83 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 5908), Tuple.Create<System.Object, System.Int32>(savedSearchesTabClass
            
            #line default
            #line hidden
, 5909), false)
            
            #line 83 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
   , Tuple.Create(Tuple.Create(" ", 5931), Tuple.Create<System.Object, System.Int32>(savedSearchesElementControlClass
            
            #line default
            #line hidden
, 5932), false)
);

WriteLiteral(" id=\"run-saved-search-button\"");

WriteLiteral(">Run search</button>\r\n                    </div>\r\n\r\n                    <p");

WriteAttribute("class", Tuple.Create(" class=\"", 6069), Tuple.Create("\"", 6148)
, Tuple.Create(Tuple.Create("", 6077), Tuple.Create("tabbed-element", 6077), true)
            
            #line 86 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 6091), Tuple.Create<System.Object, System.Int32>(savedSearchesTabClass
            
            #line default
            #line hidden
, 6092), false)
            
            #line 86 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 6114), Tuple.Create<System.Object, System.Int32>(savedSearchesElementControlClass
            
            #line default
            #line hidden
, 6115), false)
);

WriteLiteral(">\r\n                        You can edit your saved searches in the\r\n");

WriteLiteral("                        ");

            
            #line 88 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                   Write(Html.RouteLink("alert settings", CandidateRouteNames.SavedSearchesSettings, null, new { id = "saved-searches-settings-link" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        section.\r\n                    </p>\r\n\r\n                 " +
"   <div");

WriteAttribute("class", Tuple.Create(" class=\"", 6456), Tuple.Create("\"", 6576)
, Tuple.Create(Tuple.Create("", 6464), Tuple.Create("tabbed-element", 6464), true)
, Tuple.Create(Tuple.Create(" ", 6478), Tuple.Create("disp-block", 6479), true)
, Tuple.Create(Tuple.Create(" ", 6489), Tuple.Create("form-group", 6490), true)
, Tuple.Create(Tuple.Create(" ", 6500), Tuple.Create("tab1", 6501), true)
            
            #line 92 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create(" ", 6505), Tuple.Create<System.Object, System.Int32>(keywordTabControlClass
            
            #line default
            #line hidden
, 6506), false)
            
            #line 92 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                 , Tuple.Create(Tuple.Create(" ", 6529), Tuple.Create<System.Object, System.Int32>(categoriesTabClass
            
            #line default
            #line hidden
, 6530), false)
            
            #line 92 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                                     , Tuple.Create(Tuple.Create(" ", 6549), Tuple.Create<System.Object, System.Int32>(categoriesTabControlClass
            
            #line default
            #line hidden
, 6550), false)
);

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 93 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                   Write(Html.RouteLink("Reset search options", CandidateRouteNames.ApprenticeshipSearch, null, new { @id = "reset-search-options-link" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n\r\n");

WriteLiteral("                    ");

            
            #line 96 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.HiddenFor(m => m.Latitude));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 97 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.HiddenFor(m => m.Longitude));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 98 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.Hidden("Hash", Model.LatLonLocHash()));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 99 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.HiddenFor(m => m.LocationType));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 100 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.HiddenFor(m => m.ResultsPerPage));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 101 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
               Write(Html.HiddenFor(m => m.SearchMode));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n");

            
            #line 103 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </section>\r\n        <aside");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"inner-block\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"gov-border\"");

WriteLiteral(">\r\n                    <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(">Help</h2>\r\n                    <ul");

WriteLiteral(" class=\"list-text\"");

WriteLiteral(">\r\n                        <li");

WriteLiteral(" class=\"hide-nojs\"");

WriteLiteral("><a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" id=\"runSearchHelp\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-question-circle\"");

WriteLiteral("></i>How to search effectively</a> (Interactive walkthrough)</li>\r\n              " +
"          <li>\r\n                            <a");

WriteLiteral(" href=\"https://www.gov.uk/apprenticeships-guide\"");

WriteLiteral(" target=\"_blank\"");

WriteLiteral(">About apprenticeships</a>\r\n                        </li>\r\n                    </" +
"ul>\r\n                    <ul");

WriteLiteral(" class=\"list-text\"");

WriteLiteral(">\r\n                        <li>0800 015 0400</li>\r\n                        <li><a" +
"");

WriteAttribute("href", Tuple.Create(" href=\'", 7903), Tuple.Create("\'", 7953)
            
            #line 117 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
, Tuple.Create(Tuple.Create("", 7910), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.Helpdesk)
            
            #line default
            #line hidden
, 7910), false)
);

WriteLiteral(">Contact us</a></li>\r\n                    </ul>\r\n                    \r\n          " +
"      </div>\r\n            </div>\r\n        </aside>\r\n    </div>\r\n</div>\r\n\r\n<ol");

WriteLiteral(" id=\"firstSearchTour\"");

WriteLiteral(" class=\"alwayshidden\"");

WriteLiteral(">\r\n    <li");

WriteLiteral(" class=\"startTourGuide\"");

WriteLiteral(" data-id=\"runSearchHelp\"");

WriteLiteral(" data-button=\"Start tour\"");

WriteLiteral(">\r\n        <h3");

WriteLiteral(" class=\"heading-small med-btm-margin\"");

WriteLiteral(">Start tour</h3>\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">If you\'d like some help on how to use the search to find apprenticeships that ar" +
"e suitable for you, start the tour here.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" class=\"startNextTourGuide\"");

WriteLiteral(" data-id=\"keywords-tab-control\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">If you know the type of apprenticeship you’re looking for use the keyword search" +
".</p>\r\n    </li>\r\n</ol>\r\n\r\n<ol");

WriteLiteral(" id=\"searchTour\"");

WriteLiteral(" class=\"alwayshidden\"");

WriteLiteral(">\r\n    <li");

WriteLiteral(" data-id=\"keywords-tab-control\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">If you know the type of apprenticeship you’re looking for use the keyword search" +
".</p>\r\n    </li>\r\n    <li");

WriteLiteral(" class=\"browseHelp\"");

WriteLiteral(" data-id=\"categories-tab-control\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">If you want to see what apprenticeships are available in different industries yo" +
"u can search by category (eg construction or retail).</p>\r\n    </li>\r\n    <li");

WriteLiteral(" class=\"savedHelp\"");

WriteLiteral(" data-id=\"saved-searches-tab-control\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">If you have saved searches you can run them from here. You can save as many diff" +
"erent searches as you like.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"SearchField\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">Select this option if you\'d like to refine your search by job title, employer or" +
" reference number.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" class=\"browseHelp\"");

WriteLiteral(" data-id=\"Keywords\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">Type your keyword here, alternatively you can leave it blank to look for any app" +
"renticeship in your selected area.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"getLocation\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">If you\'d like to use your device\'s position to get your current post code, click" +
" here and then click \"Allow\" when prompted.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"Location\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">You can search for apprenticeships in any location in England. This can be near " +
"your home or where you’d like to work.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"loc-within\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">Choose how far from your location you want to search.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"apprenticeship-level\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">You can also search by level. Intermediate is equivalent to 5 GCSE passes, Advan" +
"ced is 2 A-level passes and a Higher can lead to a foundation degree.</p>\r\n    <" +
"/li>\r\n    <li");

WriteLiteral(" data-id=\"reset-search-options-link\"");

WriteLiteral(" data-button=\"Finish\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">If you want to start with blank fields, you can clear the form with the button.<" +
"/p>\r\n    </li>\r\n\r\n</ol>\r\n\r\n<ol");

WriteLiteral(" id=\"browseTour\"");

WriteLiteral(" class=\"alwayshidden\"");

WriteLiteral(">\r\n    <li");

WriteLiteral(" class=\"browseHelp\"");

WriteLiteral(" data-id=\"categories-tab-control\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">If you want to see what apprenticeships are available in different industries yo" +
"u can search by category (eg construction or retail).</p>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"categoryLabelTour\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">Select a category from this list, once you browse you\'ll then see a list of sub-" +
"categories you can select on the results page.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"getLocation\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">If you\'d like to use your device\'s position to get your current post code, click" +
" here and then click \"Allow\" when prompted.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"Location\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">You can search for apprenticeships in any location in England. This can be near " +
"your home or where you’d like to work.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"loc-within\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">Choose how far from your location you want to search.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"apprenticeship-level\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">You can also search by level. Intermediate is equivalent to 5 GCSE passes, Advan" +
"ced is 2 A-level passes and a Higher can lead to a foundation degree.</p>\r\n    <" +
"/li>\r\n    <li");

WriteLiteral(" data-id=\"reset-search-options-link\"");

WriteLiteral(" data-button=\"Finish\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">If you want to start with blank fields, you can clear the form with the button.<" +
"/p>\r\n    </li>\r\n</ol>\r\n\r\n<ol");

WriteLiteral(" id=\"savedSearchTour\"");

WriteLiteral(" class=\"alwayshidden\"");

WriteLiteral(">\r\n    <li");

WriteLiteral(" class=\"savedHelp\"");

WriteLiteral(" data-id=\"saved-searches-tab-control\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">If you have saved searches you can run them from here. You can save as many diff" +
"erent searches as you like.</p>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"saved-searches-list\"");

WriteLiteral(" data-button=\"Finish\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">Select one of your saved searches and then run your saved search.</p>\r\n    </li>" +
"\r\n</ol>\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("   \r\n");

WriteLiteral("    ");

            
            #line 205 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
Write(Scripts.Render("~/bundles/nas/locationsearch"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    <script>\r\n        $(\"#Location\").locationMatch({\r\n            url: \'");

            
            #line 209 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
             Write(Url.Action("location", "Location"));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            longitude: \'#");

            
            #line 210 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                     Write(Html.IdFor(m => m.Longitude));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            latitude: \'#");

            
            #line 211 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                    Write(Html.IdFor(m => m.Latitude));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n            latlonhash: \'#");

            
            #line 212 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
                      Write(Html.IdFor(m => m.Hash));

            
            #line default
            #line hidden
WriteLiteral(@"'
        });

        $(""#keywords-tab-control"").click(function () {
            $(""#SearchMode"").val(""Keyword"");
        });

        $(""#categories-tab-control"").click(function () {
            $(""#SearchMode"").val(""Category"");
        });

        $(""#saved-searches-tab-control"").click(function () {
            $(""#SearchMode"").val(""SavedSearches"");
        });
    </script>

");

WriteLiteral("    ");

            
            #line 228 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
Write(Scripts.Render("~/bundles/nas/geoLocater"));

            
            #line default
            #line hidden
WriteLiteral("\r\n                                                \r\n");

WriteLiteral("    ");

            
            #line 230 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
Write(Scripts.Render("~/bundles/joyride"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 231 "..\..\Views\ApprenticeshipSearch\Index.cshtml"
Write(Scripts.Render("~/bundles/nas/searchTour"));

            
            #line default
            #line hidden
WriteLiteral("\r\n    \r\n");

});

        }
    }
}
#pragma warning restore 1591
