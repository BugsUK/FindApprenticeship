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
    using SFA.Apprenticeships.Web.Candidate;
    using SFA.Apprenticeships.Web.Candidate.Constants;
    using SFA.Apprenticeships.Web.Candidate.Constants.ViewModels;
    
    #line 1 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipSearch/_searchUpdate.cshtml")]
    public partial class searchUpdate : System.Web.Mvc.WebViewPage<ApprenticeshipSearchViewModel>
    {
        public searchUpdate()
        {
        }
        public override void Execute()
        {
WriteLiteral("<section");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n    <div>\r\n        <fieldset");

WriteLiteral(" class=\"search-filter\"");

WriteLiteral(">\r\n\r\n            <legend");

WriteLiteral(" class=\"heading-medium mob-collpanel-trigger\"");

WriteLiteral(" id=\"editSearchToggle\"");

WriteLiteral(">Edit search</legend>\r\n            <div");

WriteLiteral(" class=\"mob-collpanel toggle-content--mob\"");

WriteLiteral(" id=\"editSearchPanel\"");

WriteLiteral(">\r\n                <nav");

WriteLiteral(" class=\"tabbed-nav\"");

WriteLiteral(">\r\n");

            
            #line 11 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 11 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                      
                        var searchTabControlUrl = Url.ApprenticeshipSearchViewModelAction("results", new ApprenticeshipSearchViewModel(Model) {SearchMode = ApprenticeshipSearchMode.Keyword});
                        var browseTabControlUrl = Url.ApprenticeshipSearchViewModelAction("results", new ApprenticeshipSearchViewModel(Model) {SearchMode = ApprenticeshipSearchMode.Category});
                        var searchTabControlClass = Model.SearchMode == ApprenticeshipSearchMode.Keyword ? " active" : "";
                        var browseTabControlClass = Model.SearchMode == ApprenticeshipSearchMode.Category ? " active" : "";

                        var categoriesElementControlClass = Model.SearchMode == ApprenticeshipSearchMode.Category && Model.Categories != null && Model.Categories.Any() ? " active" : "";
                        var categoriesTabClass = Model.Categories != null && Model.Categories.Any() ? " tab2" : "";
                        var elementControlClass = Model.SearchMode == ApprenticeshipSearchMode.Keyword ? searchTabControlClass : categoriesElementControlClass;                        
                    
            
            #line default
            #line hidden
WriteLiteral("\r\n                    <a");

WriteAttribute("href", Tuple.Create(" href=\"", 1610), Tuple.Create("\"", 1637)
            
            #line 21 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
, Tuple.Create(Tuple.Create("", 1617), Tuple.Create<System.Object, System.Int32>(searchTabControlUrl
            
            #line default
            #line hidden
, 1617), false)
);

WriteLiteral(" id=\"search-tab-control\"");

WriteAttribute("class", Tuple.Create(" class=\"", 1662), Tuple.Create("\"", 1703)
, Tuple.Create(Tuple.Create("", 1670), Tuple.Create("tabbed-tab", 1670), true)
            
            #line 21 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
            , Tuple.Create(Tuple.Create(" ", 1680), Tuple.Create<System.Object, System.Int32>(searchTabControlClass
            
            #line default
            #line hidden
, 1681), false)
);

WriteLiteral(" tab=\"#tab1\"");

WriteLiteral(">Search</a>\r\n                    <a");

WriteAttribute("href", Tuple.Create(" href=\"", 1751), Tuple.Create("\"", 1778)
            
            #line 22 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
, Tuple.Create(Tuple.Create("", 1758), Tuple.Create<System.Object, System.Int32>(browseTabControlUrl
            
            #line default
            #line hidden
, 1758), false)
);

WriteLiteral(" id=\"browse-tab-control\"");

WriteAttribute("class", Tuple.Create(" class=\"", 1803), Tuple.Create("\"", 1844)
, Tuple.Create(Tuple.Create("", 1811), Tuple.Create("tabbed-tab", 1811), true)
            
            #line 22 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
            , Tuple.Create(Tuple.Create(" ", 1821), Tuple.Create<System.Object, System.Int32>(browseTabControlClass
            
            #line default
            #line hidden
, 1822), false)
);

WriteLiteral(" tab=\"#tab2\"");

WriteLiteral(">Browse</a>\r\n                </nav>\r\n                <div");

WriteLiteral(" class=\"tabbed-content active\"");

WriteLiteral(">\r\n\r\n");

WriteLiteral("                    ");

            
            #line 26 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
               Write(Html.Partial("_categoriesAndSubCategories", Model));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    \r\n");

WriteLiteral("                    ");

            
            #line 28 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
               Write(Html.Partial("_refineSearch", Model));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("                    ");

            
            #line 30 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
               Write(Html.FormTextFor(m => m.Keywords, hintText: "", containerHtmlAttributes: new { @class = "tabbed-element tab1" + searchTabControlClass }, labelHtmlAttributes: new { id = "keyword-label" }, hintHtmlAttributes: new { id = "keyword-hint" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("                    ");

            
            #line 32 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
               Write(Html.FormTextFor(m => m.Location, hintText: "", containerHtmlAttributes: new { @class = "tabbed-element tab1" + categoriesTabClass + elementControlClass}));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 34 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 34 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                     if (Model.LocationSearches != null && Model.LocationSearches.Length > 0)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <details open");

WriteAttribute("class", Tuple.Create(" class=\"", 2700), Tuple.Create("\"", 2804)
, Tuple.Create(Tuple.Create("", 2708), Tuple.Create("open", 2708), true)
, Tuple.Create(Tuple.Create(" ", 2712), Tuple.Create("form-group", 2713), true)
, Tuple.Create(Tuple.Create(" ", 2723), Tuple.Create("form-group-compound", 2724), true)
, Tuple.Create(Tuple.Create(" ", 2743), Tuple.Create("tabbed-element", 2744), true)
, Tuple.Create(Tuple.Create(" ", 2758), Tuple.Create("tab1", 2759), true)
            
            #line 36 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                    , Tuple.Create(Tuple.Create(" ", 2763), Tuple.Create<System.Object, System.Int32>(categoriesTabClass
            
            #line default
            #line hidden
, 2764), false)
            
            #line 36 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                                        , Tuple.Create(Tuple.Create(" ", 2783), Tuple.Create<System.Object, System.Int32>(elementControlClass
            
            #line default
            #line hidden
, 2784), false)
);

WriteLiteral(" id=\"locationSuggestions\"");

WriteLiteral(">\r\n                            <summary");

WriteLiteral(" tabindex=\"0\"");

WriteLiteral(" aria-describedby=\"locSuggestionsAria\"");

WriteLiteral(">Did you mean:</summary>\r\n                            <p");

WriteLiteral(" class=\"visuallyhidden\"");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(" id=\"locSuggestionsAria\"");

WriteLiteral("></p>\r\n                            <div");

WriteLiteral(" class=\"detail-content panel-indent\"");

WriteLiteral(">\r\n                                <ul");

WriteLiteral(" id=\"location-suggestions\"");

WriteLiteral(" class=\"list-text list-max-11\"");

WriteLiteral(">\r\n");

            
            #line 41 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                                    
            
            #line default
            #line hidden
            
            #line 41 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                                     foreach (var locationSearch in Model.LocationSearches)
                                    {

            
            #line default
            #line hidden
WriteLiteral("                                        <li><a");

WriteAttribute("href", Tuple.Create(" href=\"", 3392), Tuple.Create("\"", 3437)
            
            #line 43 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
, Tuple.Create(Tuple.Create("", 3399), Tuple.Create<System.Object, System.Int32>(Url.Action("results", locationSearch)
            
            #line default
            #line hidden
, 3399), false)
);

WriteLiteral(">");

            
            #line 43 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                                                                                        Write(locationSearch.Location);

            
            #line default
            #line hidden
WriteLiteral("</a></li>\r\n");

            
            #line 44 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                                    }

            
            #line default
            #line hidden
WriteLiteral("                                </ul>\r\n                            </div>\r\n      " +
"                  </details>\r\n");

            
            #line 48 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 3671), Tuple.Create("\"", 3750)
, Tuple.Create(Tuple.Create("", 3679), Tuple.Create("form-group", 3679), true)
, Tuple.Create(Tuple.Create(" ", 3689), Tuple.Create("tabbed-element", 3690), true)
, Tuple.Create(Tuple.Create(" ", 3704), Tuple.Create("tab1", 3705), true)
            
            #line 49 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
, Tuple.Create(Tuple.Create(" ", 3709), Tuple.Create<System.Object, System.Int32>(categoriesTabClass
            
            #line default
            #line hidden
, 3710), false)
            
            #line 49 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
  , Tuple.Create(Tuple.Create(" ", 3729), Tuple.Create<System.Object, System.Int32>(elementControlClass
            
            #line default
            #line hidden
, 3730), false)
);

WriteLiteral(">\r\n                        <label");

WriteLiteral(" for=\" loc-within\"");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">\r\n                        Within</label>\r\n");

WriteLiteral("                        ");

            
            #line 52 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                   Write(Html.DropDownListFor(m => m.WithinDistance, Model.Distances, new { @id = "loc-within", @name = "WithinDistance" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 4057), Tuple.Create("\"", 4136)
, Tuple.Create(Tuple.Create("", 4065), Tuple.Create("form-group", 4065), true)
, Tuple.Create(Tuple.Create(" ", 4075), Tuple.Create("tabbed-element", 4076), true)
, Tuple.Create(Tuple.Create(" ", 4090), Tuple.Create("tab1", 4091), true)
            
            #line 54 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
, Tuple.Create(Tuple.Create(" ", 4095), Tuple.Create<System.Object, System.Int32>(categoriesTabClass
            
            #line default
            #line hidden
, 4096), false)
            
            #line 54 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
  , Tuple.Create(Tuple.Create(" ", 4115), Tuple.Create<System.Object, System.Int32>(elementControlClass
            
            #line default
            #line hidden
, 4116), false)
);

WriteLiteral(">\r\n                        <label");

WriteLiteral(" for=\"apprenticeship-level\"");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Apprenticeship level</label>\r\n");

WriteLiteral("                        ");

            
            #line 56 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                   Write(Html.DropDownListFor(m => m.ApprenticeshipLevel, Model.ApprenticeshipLevels, new { @id = "apprenticeship-level", @name = "ApprenticeshipLevel" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 4471), Tuple.Create("\"", 4550)
, Tuple.Create(Tuple.Create("", 4479), Tuple.Create("form-group", 4479), true)
, Tuple.Create(Tuple.Create(" ", 4489), Tuple.Create("tabbed-element", 4490), true)
, Tuple.Create(Tuple.Create(" ", 4504), Tuple.Create("tab1", 4505), true)
            
            #line 58 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
, Tuple.Create(Tuple.Create(" ", 4509), Tuple.Create<System.Object, System.Int32>(categoriesTabClass
            
            #line default
            #line hidden
, 4510), false)
            
            #line 58 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
  , Tuple.Create(Tuple.Create(" ", 4529), Tuple.Create<System.Object, System.Int32>(elementControlClass
            
            #line default
            #line hidden
, 4530), false)
);

WriteLiteral(">\r\n                        <button");

WriteLiteral(" class=\"button no-btm-margin\"");

WriteLiteral(" id=\"search-button\"");

WriteLiteral(" name=\"SearchAction\"");

WriteLiteral(" value=\"Search\"");

WriteLiteral(">Update results</button>\r\n                    </div>\r\n                    <div");

WriteAttribute("class", Tuple.Create(" class=\"", 4746), Tuple.Create("\"", 4849)
, Tuple.Create(Tuple.Create("", 4754), Tuple.Create("form-group", 4754), true)
, Tuple.Create(Tuple.Create(" ", 4764), Tuple.Create("map-container", 4765), true)
, Tuple.Create(Tuple.Create(" ", 4778), Tuple.Create("hide-nojs", 4779), true)
, Tuple.Create(Tuple.Create(" ", 4788), Tuple.Create("tabbed-element", 4789), true)
, Tuple.Create(Tuple.Create(" ", 4803), Tuple.Create("tab1", 4804), true)
            
            #line 61 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
      , Tuple.Create(Tuple.Create(" ", 4808), Tuple.Create<System.Object, System.Int32>(categoriesTabClass
            
            #line default
            #line hidden
, 4809), false)
            
            #line 61 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
                          , Tuple.Create(Tuple.Create(" ", 4828), Tuple.Create<System.Object, System.Int32>(elementControlClass
            
            #line default
            #line hidden
, 4829), false)
);

WriteLiteral(">\r\n                        <div");

WriteLiteral(" id=\"map-canvas\"");

WriteLiteral(" style=\"width: 100%; height: 250px\"");

WriteLiteral("></div>\r\n                    </div>\r\n\r\n");

WriteLiteral("                    ");

            
            #line 65 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
               Write(Html.HiddenFor(m => m.Latitude));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 66 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
               Write(Html.HiddenFor(m => m.Longitude));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 67 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
               Write(Html.Hidden("Hash", Model.LatLonLocHash()));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 68 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
               Write(Html.HiddenFor(m => m.SearchMode));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    ");

WriteLiteral("\r\n                    <input");

WriteLiteral(" id=\"LocationType\"");

WriteLiteral(" name=\"LocationType\"");

WriteLiteral(" type=\"hidden\"");

WriteAttribute("value", Tuple.Create(" value=\"", 5469), Tuple.Create("\"", 5496)
            
            #line 70 "..\..\Views\ApprenticeshipSearch\_searchUpdate.cshtml"
      , Tuple.Create(Tuple.Create("", 5477), Tuple.Create<System.Object, System.Int32>(Model.LocationType
            
            #line default
            #line hidden
, 5477), false)
);

WriteLiteral(" />\r\n\r\n                </div>\r\n            </div>\r\n        </fieldset>\r\n    </div" +
">\r\n</section>");

        }
    }
}
#pragma warning restore 1591
