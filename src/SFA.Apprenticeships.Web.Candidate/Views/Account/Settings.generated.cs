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

WriteLiteral(">\r\n");

WriteLiteral("        ");

            
            #line 13 "..\..\Views\Account\Settings.cshtml"
   Write(Html.RouteLink("Find an apprenticeship", CandidateRouteNames.ApprenticeshipSearch, null, new { id = "find-apprenticeship-link", @class = "page-link small-btm-margin" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 15 "..\..\Views\Account\Settings.cshtml"
        
            
            #line default
            #line hidden
            
            #line 15 "..\..\Views\Account\Settings.cshtml"
         if (Model.TraineeshipFeature.ShowTraineeshipsLink)
        {
            
            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\Account\Settings.cshtml"
       Write(Html.RouteLink("Find a traineeship", CandidateRouteNames.TraineeshipSearch, null, new { id = "find-traineeship-link", @class = "page-link small-btm-margin" }));

            
            #line default
            #line hidden
            
            #line 17 "..\..\Views\Account\Settings.cshtml"
                                                                                                                                                                           
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</div>\r\n\r\n");

            
            #line 22 "..\..\Views\Account\Settings.cshtml"
 using (Html.BeginRouteForm(CandidateRouteNames.Settings, FormMethod.Post))
{
    
            
            #line default
            #line hidden
            
            #line 24 "..\..\Views\Account\Settings.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 24 "..\..\Views\Account\Settings.cshtml"
                            
    
            
            #line default
            #line hidden
            
            #line 25 "..\..\Views\Account\Settings.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 25 "..\..\Views\Account\Settings.cshtml"
                                                           


            
            #line default
            #line hidden
WriteLiteral("    <fieldset>\r\n\r\n");

            
            #line 29 "..\..\Views\Account\Settings.cshtml"
        
            
            #line default
            #line hidden
            
            #line 29 "..\..\Views\Account\Settings.cshtml"
          
    var yourAccountTabClass = Model.Mode == SettingsViewModel.SettingsMode.YourAccount ? " active" : "";
    var savedSearchesTabClass = Model.Mode == SettingsViewModel.SettingsMode.SavedSearches ? " active" : "";
        
            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n        <nav");

WriteLiteral(" class=\"tabbed-nav\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 35 "..\..\Views\Account\Settings.cshtml"
       Write(Html.RouteLink("Your account", CandidateRouteNames.Settings, null, new { @id = "your-account-tab-control", @class = "tabbed-tab no-js" + yourAccountTabClass, tab = "#tab1" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 36 "..\..\Views\Account\Settings.cshtml"
       Write(Html.RouteLink("Saved searches", CandidateRouteNames.SavedSearchesSettings, null, new { @id = "saved-searches-tab-control", @class = "tabbed-tab no-js" + savedSearchesTabClass, tab = "#tab2" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </nav>\r\n\r\n");

WriteLiteral("        ");

            
            #line 39 "..\..\Views\Account\Settings.cshtml"
   Write(Html.HiddenFor(m => m.Mode));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 1749), Tuple.Create("\"", 1792)
, Tuple.Create(Tuple.Create("", 1757), Tuple.Create("tabbed-content", 1757), true)
            
            #line 41 "..\..\Views\Account\Settings.cshtml"
, Tuple.Create(Tuple.Create(" ", 1771), Tuple.Create<System.Object, System.Int32>(yourAccountTabClass
            
            #line default
            #line hidden
, 1772), false)
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

            
            #line 49 "..\..\Views\Account\Settings.cshtml"
       Write(Html.FormTextFor(m => m.Firstname, containerHtmlAttributes: new { @class = "form-group-compound" }, controlHtmlAttributes: new { type = "text", autocorrect = "off" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 50 "..\..\Views\Account\Settings.cshtml"
       Write(Html.FormTextFor(m => m.Lastname, controlHtmlAttributes: new { type = "text", autocorrect = "off" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 51 "..\..\Views\Account\Settings.cshtml"
       Write(Html.EditorFor(r => r.DateOfBirth));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 52 "..\..\Views\Account\Settings.cshtml"
       Write(Html.EditorFor(a => a.Address, new { AnalyticsDSCUri = "/settings/findaddress" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 53 "..\..\Views\Account\Settings.cshtml"
       Write(Html.FormTextFor(m => m.PhoneNumber, controlHtmlAttributes: new { @class = "form-control", type = "tel" }, verified: Model.VerifiedMobile));

            
            #line default
            #line hidden
WriteLiteral("\r\n            \r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                <p");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Email</p>\r\n                <span");

WriteLiteral(" class=\"form-prepopped\"");

WriteLiteral(">");

            
            #line 57 "..\..\Views\Account\Settings.cshtml"
                                        Write(Model.Username);

            
            #line default
            #line hidden
WriteLiteral("</span><a");

WriteLiteral(" class=\"inl-block\"");

WriteAttribute("href", Tuple.Create(" href=\"", 2877), Tuple.Create("\"", 2921)
            
            #line 57 "..\..\Views\Account\Settings.cshtml"
              , Tuple.Create(Tuple.Create("", 2884), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(RouteNames.UpdateEmail)
            
            #line default
            #line hidden
, 2884), false)
);

WriteLiteral(">Change email address</a>\r\n            </div>\r\n\r\n            <div");

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

            
            #line 67 "..\..\Views\Account\Settings.cshtml"
               Write(Html.FormUnvalidatedCheckBoxFor(m => m.AllowEmailComms, labelHtmlAttributes: new { @class = "block-label allowCommsCheck" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 68 "..\..\Views\Account\Settings.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 68 "..\..\Views\Account\Settings.cshtml"
                     if (Model.SmsEnabled)
                    {
                        
            
            #line default
            #line hidden
            
            #line 70 "..\..\Views\Account\Settings.cshtml"
                   Write(Html.FormUnvalidatedCheckBoxFor(m => m.AllowSmsComms, labelHtmlAttributes: new { @class = "block-label allowCommsCheck" }));

            
            #line default
            #line hidden
            
            #line 70 "..\..\Views\Account\Settings.cshtml"
                                                                                                                                                   
                    }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n\r\n                <div");

WriteLiteral(" class=\"text para-btm-margin\"");

WriteLiteral(">\r\n                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">Choose to be notified when:</p>\r\n                    <ul");

WriteLiteral(" class=\"list-text list-checkradio\"");

WriteLiteral(" id=\"notificationPrefs\"");

WriteLiteral(">\r\n                        <li>\r\n");

WriteLiteral("                            ");

            
            #line 78 "..\..\Views\Account\Settings.cshtml"
                       Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendApplicationStatusChanges));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </li>\r\n                        <li>\r\n");

WriteLiteral("                            ");

            
            #line 81 "..\..\Views\Account\Settings.cshtml"
                       Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendApprenticeshipApplicationsExpiring));

            
            #line default
            #line hidden
WriteLiteral("\r\n                        </li>\r\n                        <li>\r\n");

WriteLiteral("                            ");

            
            #line 84 "..\..\Views\Account\Settings.cshtml"
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

WriteAttribute("class", Tuple.Create(" class=\"", 4697), Tuple.Create("\"", 4742)
, Tuple.Create(Tuple.Create("", 4705), Tuple.Create("tabbed-content", 4705), true)
            
            #line 95 "..\..\Views\Account\Settings.cshtml"
, Tuple.Create(Tuple.Create(" ", 4719), Tuple.Create<System.Object, System.Int32>(savedSearchesTabClass
            
            #line default
            #line hidden
, 4720), false)
);

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(" id=\"savedSearchHeading\"");

WriteLiteral(">Saved searches</h2>\r\n            <div");

WriteLiteral(" class=\"form-group form-group-compound\"");

WriteLiteral(">\r\n                <p");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Receive saved search alerts?</p>\r\n                <span");

WriteLiteral(" class=\"form-hint text\"");

WriteLiteral(">If you don\'t select an option you won\'t receive alerts when we find new apprenti" +
"ceships matching your saved searches.</span>\r\n");

WriteLiteral("                ");

            
            #line 100 "..\..\Views\Account\Settings.cshtml"
           Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendSavedSearchAlertsViaEmail, labelHtmlAttributes: new { @class = "block-label allowSavedComms" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 101 "..\..\Views\Account\Settings.cshtml"
                
            
            #line default
            #line hidden
            
            #line 101 "..\..\Views\Account\Settings.cshtml"
                 if (Model.SmsEnabled)
                {
                    
            
            #line default
            #line hidden
            
            #line 103 "..\..\Views\Account\Settings.cshtml"
               Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendSavedSearchAlertsViaText, labelHtmlAttributes: new { @class = "block-label allowSavedComms" }));

            
            #line default
            #line hidden
            
            #line 103 "..\..\Views\Account\Settings.cshtml"
                                                                                                                                                              
                }

            
            #line default
            #line hidden
WriteLiteral("            </div>\r\n\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" id=\"savedSearch\"");

WriteLiteral(">\r\n\r\n");

            
            #line 109 "..\..\Views\Account\Settings.cshtml"
                
            
            #line default
            #line hidden
            
            #line 109 "..\..\Views\Account\Settings.cshtml"
                  
    var noSavedSearchesTextStyle = (Model.SavedSearches != null && Model.SavedSearches.Count > 0) ? "style=\"display: none\"" : "";
                
            
            #line default
            #line hidden
WriteLiteral("\r\n                <p");

WriteLiteral(" class=\"savedInitalText text\"");

WriteLiteral(" id=\"noSavedSearchesText\"");

WriteLiteral(" ");

            
            #line 112 "..\..\Views\Account\Settings.cshtml"
                                                                    Write(Html.Raw(noSavedSearchesTextStyle));

            
            #line default
            #line hidden
WriteLiteral(">\r\n                    You currently don\'t have any active saved searches. If you" +
" <a");

WriteAttribute("href", Tuple.Create(" href=\"", 5968), Tuple.Create("\"", 6030)
            
            #line 113 "..\..\Views\Account\Settings.cshtml"
        , Tuple.Create(Tuple.Create("", 5975), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.ApprenticeshipSearch)
            
            #line default
            #line hidden
, 5975), false)
);

WriteLiteral(">set up a saved search</a> we can alert you when we find a suitable apprenticeshi" +
"p.\r\n                </p>\r\n");

            
            #line 115 "..\..\Views\Account\Settings.cshtml"
                
            
            #line default
            #line hidden
            
            #line 115 "..\..\Views\Account\Settings.cshtml"
                 if (Model.SavedSearches != null)
                {

            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteLiteral(" id=\"savedSearches\"");

WriteLiteral(" class=\"toggle-content text\"");

WriteLiteral(" style=\"display: block;\"");

WriteLiteral(">\r\n");

            
            #line 118 "..\..\Views\Account\Settings.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 118 "..\..\Views\Account\Settings.cshtml"
                         for (var i = 0; i < Model.SavedSearches.Count; i++)
                        {
                            var index = i;
                            var savedSearch = Model.SavedSearches[i];
                            var checkedListItemClass = savedSearch.AlertsEnabled ? "class=\"selected\"" : "";


            
            #line default
            #line hidden
WriteLiteral("                            <div");

WriteLiteral(" class=\"para-btm-margin saved-search\"");

WriteAttribute("id", Tuple.Create(" id=\"", 6708), Tuple.Create("\"", 6728)
            
            #line 124 "..\..\Views\Account\Settings.cshtml"
, Tuple.Create(Tuple.Create("", 6713), Tuple.Create<System.Object, System.Int32>(savedSearch.Id
            
            #line default
            #line hidden
, 6713), false)
);

WriteLiteral(">\r\n                                <a");

WriteAttribute("href", Tuple.Create(" href=\"", 6766), Tuple.Create("\"", 6811)
            
            #line 125 "..\..\Views\Account\Settings.cshtml"
, Tuple.Create(Tuple.Create("", 6773), Tuple.Create<System.Object, System.Int32>(Html.Raw(savedSearch.SearchUrl.Value)
            
            #line default
            #line hidden
, 6773), false)
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

WriteLiteral(">\r\n");

            
            #line 131 "..\..\Views\Account\Settings.cshtml"
                                    
            
            #line default
            #line hidden
            
            #line 131 "..\..\Views\Account\Settings.cshtml"
                                     if (!string.IsNullOrEmpty(savedSearch.SubCategoriesFullNames))
                                    {

            
            #line default
            #line hidden
WriteLiteral("                                        <li><b>Sub-categories:</b> ");

            
            #line 133 "..\..\Views\Account\Settings.cshtml"
                                                              Write(savedSearch.SubCategoriesFullNames);

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");

            
            #line 134 "..\..\Views\Account\Settings.cshtml"
                                    }

            
            #line default
            #line hidden
WriteLiteral("                                    ");

            
            #line 135 "..\..\Views\Account\Settings.cshtml"
                                     if (savedSearch.ApprenticeshipLevel != "All")
                                    {

            
            #line default
            #line hidden
WriteLiteral("                                        <li><b>Apprenticeship level:</b> ");

            
            #line 137 "..\..\Views\Account\Settings.cshtml"
                                                                    Write(savedSearch.ApprenticeshipLevel);

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n");

            
            #line 138 "..\..\Views\Account\Settings.cshtml"
                                    }

            
            #line default
            #line hidden
WriteLiteral("                                    <li ");

            
            #line 139 "..\..\Views\Account\Settings.cshtml"
                                   Write(Html.Raw(checkedListItemClass));

            
            #line default
            #line hidden
WriteLiteral(">\r\n");

WriteLiteral("                                        ");

            
            #line 140 "..\..\Views\Account\Settings.cshtml"
                                   Write(Html.HiddenFor(m => m.SavedSearches[index].Id));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                                        ");

            
            #line 141 "..\..\Views\Account\Settings.cshtml"
                                   Write(Html.FormUnvalidatedCheckBoxFor(m => m.SavedSearches[index].AlertsEnabled, controlHtmlAttributes: new { @class = "no-left-margin" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                                    </li>\r\n                                    " +
"<li>\r\n                                        <a");

WriteAttribute("href", Tuple.Create(" href=\"", 8229), Tuple.Create("\"", 8317)
            
            #line 144 "..\..\Views\Account\Settings.cshtml"
, Tuple.Create(Tuple.Create("", 8236), Tuple.Create<System.Object, System.Int32>(Url.Action("DeleteSavedSearch", new {id = savedSearch.Id, isJavascript = false})
            
            #line default
            #line hidden
, 8236), false)
);

WriteAttribute("id", Tuple.Create(" id=\"", 8318), Tuple.Create("\"", 8338)
            
            #line 144 "..\..\Views\Account\Settings.cshtml"
                                                        , Tuple.Create(Tuple.Create("", 8323), Tuple.Create<System.Object, System.Int32>(savedSearch.Id
            
            #line default
            #line hidden
, 8323), false)
);

WriteLiteral(" class=\"link-unimp icon-black delete-saved-search-link\"");

WriteLiteral(">\r\n                                            <i");

WriteLiteral(" class=\"fa fa-times-circle\"");

WriteLiteral("></i>Delete saved search\r\n                                        </a>\r\n         " +
"                           </li>\r\n                                </ul>\r\n       " +
"                     </div>\r\n");

            
            #line 150 "..\..\Views\Account\Settings.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </div>\r\n");

            
            #line 152 "..\..\Views\Account\Settings.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("\r\n            <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                <button");

WriteLiteral(" id=\"update-details-button\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(">Save settings</button>\r\n            </div>\r\n        </div>\r\n\r\n    </fieldset>\r\n");

            
            #line 160 "..\..\Views\Account\Settings.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script>\r\n        $(function () {\r\n            $(\"#find-addresses\").address" +
"Lookup({\r\n                url: \'");

            
            #line 167 "..\..\Views\Account\Settings.cshtml"
                 Write(Url.Action("Addresses", "Location"));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                selectlist: \'#address-select\'\r\n            });\r\n\r\n           " +
" $(\".delete-saved-search-link\").on(\'click\', function () {\r\n                var $" +
"this = $(this),\r\n                    $href = $this.attr(\'href\').replace(\"isJavas" +
"cript=False\", \"isJavascript=true\").replace(\"isJavascript=false\", \"isJavascript=t" +
"rue\"),\r\n                    $id = $this.attr(\'id\');\r\n\r\n                $.ajax({\r" +
"\n                    url: $href,\r\n                    complete: function (result" +
") {\r\n\r\n                        if (result.status == 200) {\r\n                    " +
"        $(\"#\" + $id).hide();\r\n\r\n                            if ($(\".saved-search" +
":visible\").length == 0) {\r\n                                $(\"#noSavedSearchesTe" +
"xt\").show();\r\n                            }\r\n                        }\r\n        " +
"            }\r\n                });\r\n\r\n                return false;\r\n           " +
" });\r\n\r\n            function disableCommsChecks() {\r\n                if (!$(\'#Al" +
"lowEmailComms\').is(\':checked\') && !$(\'#AllowSmsComms\').is(\':checked\')) {\r\n      " +
"              $(\'#notificationPrefs\').find(\'input[type=\"checkbox\"]\').parent().ad" +
"dClass(\'disabled\');\r\n                } else {\r\n                    $(\'#notificat" +
"ionPrefs\').find(\'input[type=\"checkbox\"]\').parent().removeClass(\'disabled\');\r\n   " +
"             }\r\n            }\r\n\r\n            function disableSavedCommsChecks() " +
"{\r\n                if (!$(\'#SendSavedSearchAlertsViaEmail\').is(\':checked\') && !$" +
"(\'#SendSavedSearchAlertsViaText\').is(\':checked\')) {\r\n                    $(\'#sav" +
"edSearches\').find(\'input[type=\"checkbox\"]\').parent().addClass(\'disabled\');\r\n    " +
"            } else {\r\n                    $(\'#savedSearches\').find(\'input[type=\"" +
"checkbox\"]\').parent().removeClass(\'disabled\');\r\n                }\r\n            }" +
"\r\n\r\n            $(\'.allowCommsCheck\').on(\'click\', function () {\r\n               " +
" disableCommsChecks();\r\n            });\r\n\r\n            $(\'.allowSavedComms\').on(" +
"\'click\', function () {\r\n                disableSavedCommsChecks();\r\n            " +
"});\r\n\r\n            disableCommsChecks();\r\n            disableSavedCommsChecks();" +
"\r\n\r\n\r\n        });\r\n\r\n        \r\n\r\n\r\n    </script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
