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

namespace SFA.Apprenticeships.Web.Candidate.Views.Account
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
    using SFA.Apprenticeships.Web.Candidate.Helpers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels;
    
    #line 1 "..\..\Views\Account\Settings.cshtml"
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Account;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Locations;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Login;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Account/Settings.cshtml")]
    public partial class Settings : System.Web.Mvc.WebViewPage<SettingsViewModel>
    {
        public Settings()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Views\Account\Settings.cshtml"
  
    ViewBag.Title = "Settings - Find an apprenticeship";
    Layout = "~/Views/Shared/_Layout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n        <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">Settings</h1>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n        <p>\r\n");

WriteLiteral("            ");

            
            #line 14 "..\..\Views\Account\Settings.cshtml"
       Write(Html.RouteLink("Find an apprenticeship", CandidateRouteNames.ApprenticeshipSearch, null, new { id = "find-apprenticeship-link", @class = "page-link" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </p>\r\n");

            
            #line 16 "..\..\Views\Account\Settings.cshtml"
    
            
            #line default
            #line hidden
            
            #line 16 "..\..\Views\Account\Settings.cshtml"
     if (Model.TraineeshipFeature.ShowTraineeshipsLink)
    {

            
            #line default
            #line hidden
WriteLiteral("        <p>\r\n");

WriteLiteral("            ");

            
            #line 19 "..\..\Views\Account\Settings.cshtml"
       Write(Html.RouteLink("Find a traineeship", CandidateRouteNames.TraineeshipSearch, null, new { id = "find-traineeship-link", @class = "page-link" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </p>\r\n");

            
            #line 21 "..\..\Views\Account\Settings.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</div>\r\n\r\n");

            
            #line 25 "..\..\Views\Account\Settings.cshtml"
 using (Html.BeginRouteForm(CandidateRouteNames.Settings, FormMethod.Post))
{
    
            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\Account\Settings.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 27 "..\..\Views\Account\Settings.cshtml"
                            
    
            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\Account\Settings.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 28 "..\..\Views\Account\Settings.cshtml"
                                                           


            
            #line default
            #line hidden
WriteLiteral("    <fieldset>\r\n\r\n");

            
            #line 32 "..\..\Views\Account\Settings.cshtml"
        
            
            #line default
            #line hidden
            
            #line 32 "..\..\Views\Account\Settings.cshtml"
          
            var yourAccountTabClass = Model.Mode == SettingsViewModel.SettingsMode.YourAccount ? " active" : "";
            var savedSearchesTabClass = Model.Mode == SettingsViewModel.SettingsMode.SavedSearches ? " active" : "";
        
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n        <nav");

WriteLiteral(" class=\"tabbed-nav\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 38 "..\..\Views\Account\Settings.cshtml"
       Write(Html.RouteLink("Your account", CandidateRouteNames.Settings, null, new {@id = "your-account-tab-control", @class = "tabbed-tab no-js" + yourAccountTabClass, tab = "#tab1"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 39 "..\..\Views\Account\Settings.cshtml"
       Write(Html.RouteLink("Saved searches", CandidateRouteNames.SavedSearchesSettings, null, new { @id = "saved-searches-tab-control", @class = "tabbed-tab no-js" + savedSearchesTabClass, tab = "#tab2" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </nav>\r\n        \r\n");

WriteLiteral("        ");

            
            #line 42 "..\..\Views\Account\Settings.cshtml"
   Write(Html.HiddenFor(m => m.Mode));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 1781), Tuple.Create("\"", 1824)
, Tuple.Create(Tuple.Create("", 1789), Tuple.Create("tabbed-content", 1789), true)
            
            #line 44 "..\..\Views\Account\Settings.cshtml"
, Tuple.Create(Tuple.Create(" ", 1803), Tuple.Create<System.Object, System.Int32>(yourAccountTabClass
            
            #line default
            #line hidden
, 1804), false)
);

WriteLiteral(">\r\n\r\n            <legend");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Personal details</legend>\r\n\r\n            <div");

WriteLiteral(" class=\"panel-indent\"");

WriteLiteral(">\r\n                <p");

WriteLiteral(" class=\"text\"");

WriteLiteral(">Any changes you make will be seen on draft or new applications. Submitted applic" +
"ations will continue to show your old details.</p>\r\n            </div>\r\n\r\n");

WriteLiteral("            ");

            
            #line 52 "..\..\Views\Account\Settings.cshtml"
       Write(Html.FormTextFor(m => m.Firstname, containerHtmlAttributes: new {@class = "form-group-compound"}, controlHtmlAttributes: new {type = "text", autocorrect = "off"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 53 "..\..\Views\Account\Settings.cshtml"
       Write(Html.FormTextFor(m => m.Lastname, controlHtmlAttributes: new {type = "text", autocorrect = "off"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 54 "..\..\Views\Account\Settings.cshtml"
       Write(Html.EditorFor(r => r.DateOfBirth));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 55 "..\..\Views\Account\Settings.cshtml"
       Write(Html.EditorFor(a => a.Address, new {AnalyticsDSCUri = "/settings/findaddress"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 56 "..\..\Views\Account\Settings.cshtml"
       Write(Html.FormTextFor(m => m.PhoneNumber, controlHtmlAttributes: new {@class = "form-control", type = "tel"}, verified: Model.VerifiedMobile));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n            <div");

WriteLiteral(" id=\"accountSettings2\"");

WriteLiteral(">\r\n\r\n                <h3");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">How we contact you</h3>\r\n\r\n                <div");

WriteLiteral(" class=\"form-group form-group-compound\"");

WriteLiteral(">\r\n                    <p");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Receive notifications?</p>\r\n                    <span");

WriteLiteral(" class=\"form-hint\"");

WriteLiteral(">If you don\'t select an option you won\'t receive notifications</span>\r\n");

WriteLiteral("                    ");

            
            #line 65 "..\..\Views\Account\Settings.cshtml"
               Write(Html.FormUnvalidatedCheckBoxFor(m => m.AllowEmailComms, labelHtmlAttributes: new {@class = "block-label"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 66 "..\..\Views\Account\Settings.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 66 "..\..\Views\Account\Settings.cshtml"
                     if (Model.SmsEnabled)
                    {
                        
            
            #line default
            #line hidden
            
            #line 68 "..\..\Views\Account\Settings.cshtml"
                   Write(Html.FormUnvalidatedCheckBoxFor(m => m.AllowSmsComms, labelHtmlAttributes: new {@class = "block-label"}));

            
            #line default
            #line hidden
            
            #line 68 "..\..\Views\Account\Settings.cshtml"
                                                                                                                                 
                    }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n\r\n                <div");

WriteLiteral(" class=\"text para-btm-margin\"");

WriteLiteral(">\r\n                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">Choose to be notified when:</p>\r\n                    <ul");

WriteLiteral(" class=\"list-text list-checkradio\"");

WriteLiteral(">\r\n                        <li>\r\n");

WriteLiteral("                            ");

            
            #line 76 "..\..\Views\Account\Settings.cshtml"
                       Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendApplicationSubmitted));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </li>\r\n                        <li>\r\n");

WriteLiteral("                            ");

            
            #line 79 "..\..\Views\Account\Settings.cshtml"
                       Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendApplicationStatusChanges));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </li>\r\n                        <li>\r\n");

WriteLiteral("                            ");

            
            #line 82 "..\..\Views\Account\Settings.cshtml"
                       Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendApprenticeshipApplicationsExpiring));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </li>\r\n                        <li>\r\n");

WriteLiteral("                            ");

            
            #line 85 "..\..\Views\Account\Settings.cshtml"
                       Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendMarketingCommunications));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </li>\r\n                    </ul>\r\n                </div" +
">\r\n            </div>\r\n\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                <button");

WriteLiteral(" id=\"update-details-button\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(">Save settings</button>\r\n            </div>\r\n        </div>\r\n\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 4536), Tuple.Create("\"", 4581)
, Tuple.Create(Tuple.Create("", 4544), Tuple.Create("tabbed-content", 4544), true)
            
            #line 96 "..\..\Views\Account\Settings.cshtml"
, Tuple.Create(Tuple.Create(" ", 4558), Tuple.Create<System.Object, System.Int32>(savedSearchesTabClass
            
            #line default
            #line hidden
, 4559), false)
);

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(" id=\"savedSearchHeading\"");

WriteLiteral(">Saved searches</h2>\r\n            <div");

WriteLiteral(" class=\"form-group form-group-compound\"");

WriteLiteral(">\r\n                <p");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Receive notifications?</p>\r\n                <span");

WriteLiteral(" class=\"form-hint\"");

WriteLiteral(">If you don\'t select an option you won\'t receive notifications</span>\r\n");

WriteLiteral("                ");

            
            #line 101 "..\..\Views\Account\Settings.cshtml"
           Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendSavedSearchAlertsViaEmail, labelHtmlAttributes: new { @class = "block-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 102 "..\..\Views\Account\Settings.cshtml"
                
            
            #line default
            #line hidden
            
            #line 102 "..\..\Views\Account\Settings.cshtml"
                 if (Model.SmsEnabled)
                {
                    
            
            #line default
            #line hidden
            
            #line 104 "..\..\Views\Account\Settings.cshtml"
               Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendSavedSearchAlertsViaText, labelHtmlAttributes: new { @class = "block-label" }));

            
            #line default
            #line hidden
            
            #line 104 "..\..\Views\Account\Settings.cshtml"
                                                                                                                                              
                }

            
            #line default
            #line hidden
WriteLiteral("            </div>\r\n\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" id=\"savedSearch\"");

WriteLiteral(">\r\n                \r\n");

            
            #line 110 "..\..\Views\Account\Settings.cshtml"
                
            
            #line default
            #line hidden
            
            #line 110 "..\..\Views\Account\Settings.cshtml"
                  
                    var noSavedSearchesTextStyle = (Model.SavedSearches != null && Model.SavedSearches.Count > 0) ? "style=\"display: none\"" : "";
                
            
            #line default
            #line hidden
WriteLiteral("\r\n                <p");

WriteLiteral(" class=\"savedInitalText text\"");

WriteLiteral(" id=\"noSavedSearchesText\"");

WriteLiteral(" ");

            
            #line 113 "..\..\Views\Account\Settings.cshtml"
                                                                    Write(Html.Raw(noSavedSearchesTextStyle));

            
            #line default
            #line hidden
WriteLiteral(">\r\n                    You currently don\'t have any active saved searches, <a");

WriteAttribute("href", Tuple.Create(" href=\"", 5733), Tuple.Create("\"", 5795)
            
            #line 114 "..\..\Views\Account\Settings.cshtml"
 , Tuple.Create(Tuple.Create("", 5740), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.ApprenticeshipSearch)
            
            #line default
            #line hidden
, 5740), false)
);

WriteLiteral(">use the search</a> and you can receive alerts when we find an apprenticeship tha" +
"t matches your criteria.\r\n                </p>\r\n");

            
            #line 116 "..\..\Views\Account\Settings.cshtml"
                
            
            #line default
            #line hidden
            
            #line 116 "..\..\Views\Account\Settings.cshtml"
                 if (Model.SavedSearches != null)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteLiteral(" id=\"savedSearches\"");

WriteLiteral(" class=\"toggle-content text\"");

WriteLiteral(" style=\"display: block;\"");

WriteLiteral(">\r\n");

            
            #line 119 "..\..\Views\Account\Settings.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 119 "..\..\Views\Account\Settings.cshtml"
                         for (var i = 0; i < Model.SavedSearches.Count; i++)
                        {
                            var savedSearch = Model.SavedSearches[i];
                            var checkedListItemClass = savedSearch.AlertsEnabled ? "class=\"selected\"" : "";


            
            #line default
            #line hidden
WriteLiteral("                            <div");

WriteLiteral(" class=\"para-btm-margin saved-search\"");

WriteAttribute("id", Tuple.Create(" id=\"", 6451), Tuple.Create("\"", 6471)
            
            #line 124 "..\..\Views\Account\Settings.cshtml"
, Tuple.Create(Tuple.Create("", 6456), Tuple.Create<System.Object, System.Int32>(savedSearch.Id
            
            #line default
            #line hidden
, 6456), false)
);

WriteLiteral(">\r\n                                <a");

WriteAttribute("href", Tuple.Create(" href=\"", 6509), Tuple.Create("\"", 6554)
            
            #line 125 "..\..\Views\Account\Settings.cshtml"
, Tuple.Create(Tuple.Create("", 6516), Tuple.Create<System.Object, System.Int32>(Html.Raw(savedSearch.SearchUrl.Value)
            
            #line default
            #line hidden
, 6516), false)
);

WriteLiteral(" title=\"Run search\"");

WriteLiteral(">");

            
            #line 125 "..\..\Views\Account\Settings.cshtml"
                                                                                               Write(savedSearch.Name);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n");

            
            #line 126 "..\..\Views\Account\Settings.cshtml"
                                
            
            #line default
            #line hidden
            
            #line 126 "..\..\Views\Account\Settings.cshtml"
                                 if (savedSearch.DateProcessed.HasValue)
                                {

            
            #line default
            #line hidden
WriteLiteral("                                    <span");

WriteLiteral(" class=\"inl-block font-xsmall\"");

WriteLiteral(">(Last alert: ");

            
            #line 128 "..\..\Views\Account\Settings.cshtml"
                                                                                Write(savedSearch.DateProcessed.Value.ToFriendlyDaysAgo());

            
            #line default
            #line hidden
WriteLiteral(")</span>\r\n");

            
            #line 129 "..\..\Views\Account\Settings.cshtml"
                                }

            
            #line default
            #line hidden
WriteLiteral("                                <ul");

WriteLiteral(" class=\"list-text\"");

WriteLiteral(">\r\n                                    <li ");

            
            #line 131 "..\..\Views\Account\Settings.cshtml"
                                   Write(Html.Raw(checkedListItemClass));

            
            #line default
            #line hidden
WriteLiteral(">\r\n");

            
            #line 132 "..\..\Views\Account\Settings.cshtml"
                                        
            
            #line default
            #line hidden
            
            #line 132 "..\..\Views\Account\Settings.cshtml"
                                          
                                            var index = i;
                                        
            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                                        ");

            
            #line 135 "..\..\Views\Account\Settings.cshtml"
                                   Write(Html.HiddenFor(m => m.SavedSearches[index].Id));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                                        ");

            
            #line 136 "..\..\Views\Account\Settings.cshtml"
                                   Write(Html.FormUnvalidatedCheckBoxFor(m => m.SavedSearches[index].AlertsEnabled, controlHtmlAttributes: new {@class = "no-left-margin"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                                    </li>\r\n");

            
            #line 138 "..\..\Views\Account\Settings.cshtml"
                                    
            
            #line default
            #line hidden
            
            #line 138 "..\..\Views\Account\Settings.cshtml"
                                     if (!string.IsNullOrEmpty(savedSearch.SubCategoriesFullNames))
                                    {

            
            #line default
            #line hidden
WriteLiteral("                                        <li><b>Sub-categories:</b> ");

            
            #line 140 "..\..\Views\Account\Settings.cshtml"
                                                              Write(savedSearch.SubCategoriesFullNames);

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");

            
            #line 141 "..\..\Views\Account\Settings.cshtml"
                                    }

            
            #line default
            #line hidden
WriteLiteral("                                    ");

            
            #line 142 "..\..\Views\Account\Settings.cshtml"
                                     if (savedSearch.ApprenticeshipLevel != "All")
                                    {

            
            #line default
            #line hidden
WriteLiteral("                                        <li><b>Apprenticeship level:</b> ");

            
            #line 144 "..\..\Views\Account\Settings.cshtml"
                                                                    Write(savedSearch.ApprenticeshipLevel);

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");

            
            #line 145 "..\..\Views\Account\Settings.cshtml"
                                    }

            
            #line default
            #line hidden
WriteLiteral("                                    <li>\r\n                                       " +
" <a");

WriteAttribute("href", Tuple.Create(" href=\"", 8117), Tuple.Create("\"", 8205)
            
            #line 147 "..\..\Views\Account\Settings.cshtml"
, Tuple.Create(Tuple.Create("", 8124), Tuple.Create<System.Object, System.Int32>(Url.Action("DeleteSavedSearch", new {id = savedSearch.Id, isJavascript = false})
            
            #line default
            #line hidden
, 8124), false)
);

WriteAttribute("id", Tuple.Create(" id=\"", 8206), Tuple.Create("\"", 8226)
            
            #line 147 "..\..\Views\Account\Settings.cshtml"
                                                        , Tuple.Create(Tuple.Create("", 8211), Tuple.Create<System.Object, System.Int32>(savedSearch.Id
            
            #line default
            #line hidden
, 8211), false)
);

WriteLiteral(" class=\"link-unimp icon-black delete-saved-search-link\"");

WriteLiteral(">\r\n                                            <fa");

WriteLiteral(" class=\"fa fa-times-circle\"");

WriteLiteral("></fa>Delete saved search\r\n                                        </a>\r\n        " +
"                            </li>\r\n                                </ul>\r\n      " +
"                      </div>\r\n");

            
            #line 153 "..\..\Views\Account\Settings.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </div>\r\n");

            
            #line 155 "..\..\Views\Account\Settings.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            </div>\r\n\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                <button");

WriteLiteral(" id=\"update-details-button\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(">Save settings</button>\r\n            </div>\r\n        </div>\r\n\r\n    </fieldset>\r\n");

            
            #line 164 "..\..\Views\Account\Settings.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script>\r\n        $(function () {\r\n            $(\"#find-addresses\").address" +
"Lookup({\r\n                url: \'");

            
            #line 171 "..\..\Views\Account\Settings.cshtml"
                 Write(Url.Action("Addresses", "Location"));

            
            #line default
            #line hidden
WriteLiteral(@"',
                selectlist: '#address-select'
            });
        });

        $("".delete-saved-search-link"").on('click', function () {
            var $this = $(this),
                $href = $this.attr('href').replace(""isJavascript=False"", ""isJavascript=true"").replace(""isJavascript=false"", ""isJavascript=true""),
                $id = $this.attr('id');

            $.ajax({
                url: $href,
                complete: function (result) {

                    if (result.status == 200) {
                        $(""#"" + $id).hide();

                        if ($("".saved-search:visible"").length == 0) {
                            $(""#noSavedSearchesText"").show();
                        }
                    }
                }
            });

            return false;
        });
    </script>
");

});

        }
    }
}
#pragma warning restore 1591
