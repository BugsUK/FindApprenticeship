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
    public partial class Settings : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Candidate.ViewModels.Account.SettingsViewModel>
    {
        public Settings()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Account\Settings.cshtml"
  
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

WriteLiteral(">Your account settings</h1>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n        <p>\r\n");

WriteLiteral("            ");

            
            #line 13 "..\..\Views\Account\Settings.cshtml"
       Write(Html.RouteLink("Find an apprenticeship", CandidateRouteNames.ApprenticeshipSearch, null, new { id = "find-apprenticeship-link", @class = "page-link" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </p>\r\n");

            
            #line 15 "..\..\Views\Account\Settings.cshtml"
    
            
            #line default
            #line hidden
            
            #line 15 "..\..\Views\Account\Settings.cshtml"
     if (Model.TraineeshipFeature.ShowTraineeshipsLink)
    {

            
            #line default
            #line hidden
WriteLiteral("        <p>\r\n");

WriteLiteral("            ");

            
            #line 18 "..\..\Views\Account\Settings.cshtml"
       Write(Html.RouteLink("Find a traineeship", CandidateRouteNames.TraineeshipSearch, null, new { id = "find-traineeship-link", @class = "page-link" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </p>\r\n");

            
            #line 20 "..\..\Views\Account\Settings.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</div>\r\n\r\n<div");

WriteLiteral(" class=\"panel-indent\"");

WriteLiteral(">\r\n    <p");

WriteLiteral(" class=\"text\"");

WriteLiteral(">Your changes will be seen on any draft or new applications. Submitted applicatio" +
"ns will continue to show your old details.</p>\r\n</div>\r\n\r\n");

            
            #line 28 "..\..\Views\Account\Settings.cshtml"
 using (Html.BeginRouteForm(CandidateRouteNames.Settings, FormMethod.Post))
{
    
            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\Account\Settings.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 30 "..\..\Views\Account\Settings.cshtml"
                                                           


            
            #line default
            #line hidden
WriteLiteral("    <fieldset>\r\n        <legend");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Personal details</legend>\r\n");

WriteLiteral("        ");

            
            #line 34 "..\..\Views\Account\Settings.cshtml"
   Write(Html.FormTextFor(m => m.Firstname, containerHtmlAttributes: new { @class = "form-group-compound" }, controlHtmlAttributes: new { type = "text", autocorrect = "off" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 35 "..\..\Views\Account\Settings.cshtml"
   Write(Html.FormTextFor(m => m.Lastname, controlHtmlAttributes: new { type = "text", autocorrect = "off" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 36 "..\..\Views\Account\Settings.cshtml"
   Write(Html.EditorFor(r => r.DateOfBirth));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 37 "..\..\Views\Account\Settings.cshtml"
   Write(Html.EditorFor(a => a.Address, new { AnalyticsDSCUri = "/settings/findaddress" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("        ");

            
            #line 38 "..\..\Views\Account\Settings.cshtml"
   Write(Html.FormTextFor(m => m.PhoneNumber, controlHtmlAttributes: new { @class = "form-control", type = "tel" }, verified: Model.VerifiedMobile));

            
            #line default
            #line hidden
WriteLiteral("\r\n        \r\n        <div");

WriteLiteral(" id=\"accountSettings2\"");

WriteLiteral(">\r\n            <h3");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">How we contact you</h3>\r\n\r\n            <div");

WriteLiteral(" class=\"form-group form-group-compound\"");

WriteLiteral(">\r\n                <p");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(">Receive notifications?</p>\r\n                <span");

WriteLiteral(" class=\"form-hint\"");

WriteLiteral(">If you don\'t select an option you won\'t receive notifications</span>\r\n");

WriteLiteral("                ");

            
            #line 46 "..\..\Views\Account\Settings.cshtml"
           Write(Html.FormUnvalidatedCheckBoxFor(m => m.AllowEmailComms, labelHtmlAttributes: new { @class = "block-label" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 47 "..\..\Views\Account\Settings.cshtml"
                
            
            #line default
            #line hidden
            
            #line 47 "..\..\Views\Account\Settings.cshtml"
                 if (Model.SmsEnabled)
                {
                    
            
            #line default
            #line hidden
            
            #line 49 "..\..\Views\Account\Settings.cshtml"
               Write(Html.FormUnvalidatedCheckBoxFor(m => m.AllowSmsComms, labelHtmlAttributes: new { @class = "block-label" }));

            
            #line default
            #line hidden
            
            #line 49 "..\..\Views\Account\Settings.cshtml"
                                                                                                                               
                }

            
            #line default
            #line hidden
WriteLiteral("            </div>\r\n            \r\n            <div");

WriteLiteral(" class=\"text para-btm-margin\"");

WriteLiteral(">\r\n                <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">Choose to be notified when:</p>\r\n                <ul");

WriteLiteral(" class=\"list-text list-checkradio\"");

WriteLiteral(">\r\n                    <li>\r\n");

WriteLiteral("                        ");

            
            #line 57 "..\..\Views\Account\Settings.cshtml"
                   Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendApplicationSubmitted));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </li>\r\n                    <li>\r\n");

WriteLiteral("                        ");

            
            #line 60 "..\..\Views\Account\Settings.cshtml"
                   Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendApplicationStatusChanges));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </li>\r\n                    <li>\r\n");

WriteLiteral("                        ");

            
            #line 63 "..\..\Views\Account\Settings.cshtml"
                   Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendApprenticeshipApplicationsExpiring));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </li>\r\n                    <li>\r\n");

WriteLiteral("                        ");

            
            #line 66 "..\..\Views\Account\Settings.cshtml"
                   Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendSavedSearchAlerts));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </li>\r\n                    <li>\r\n");

WriteLiteral("                        ");

            
            #line 69 "..\..\Views\Account\Settings.cshtml"
                   Write(Html.FormUnvalidatedCheckBoxFor(m => m.SendMarketingCommunications));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </li>\r\n                </ul>\r\n            </div>\r\n        <" +
"/div>\r\n\r\n    </fieldset>\r\n");

            
            #line 76 "..\..\Views\Account\Settings.cshtml"


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" id=\"update-details-button\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(">Update details</button>\r\n    </div>\r\n");

            
            #line 80 "..\..\Views\Account\Settings.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n    <script>\r\n        $(function () {\r\n            $(\"#find-addresses\").address" +
"Lookup({\r\n                url: \'");

            
            #line 87 "..\..\Views\Account\Settings.cshtml"
                 Write(Url.Action("Addresses", "Location"));

            
            #line default
            #line hidden
WriteLiteral("\',\r\n                selectlist: \'#address-select\'\r\n            });\r\n        });\r\n" +
"    </script>\r\n");

});

        }
    }
}
#pragma warning restore 1591
