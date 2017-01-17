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
    using SFA.Apprenticeships.Infrastructure.Presentation;
    using SFA.Apprenticeships.Web.Candidate;
    using SFA.Apprenticeships.Web.Candidate.Constants;
    using SFA.Apprenticeships.Web.Candidate.Constants.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.Helpers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Login;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Common.ViewModels.Locations;
    using SFA.Apprenticeships.Web.Common.Views.Shared.DisplayTemplates;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Account/VerifyMobile.cshtml")]
    public partial class VerifyMobile : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Candidate.ViewModels.Account.VerifyMobileViewModel>
    {
        public VerifyMobile()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Account\VerifyMobile.cshtml"
  
    ViewBag.Title = "Verify your mobile number - Find an apprenticeship";
    Layout = "~/Views/Shared/_Layout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"grid-row\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"column-two-thirds\"");

WriteLiteral(">\r\n        <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">Verify your mobile number</h1>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"column-one-third\"");

WriteLiteral(">\r\n        <ul");

WriteLiteral(" class=\"list sfa-align-right-tablet sfa-xlarge-top-margin\"");

WriteLiteral(">\r\n            <li>\r\n");

WriteLiteral("                ");

            
            #line 14 "..\..\Views\Account\VerifyMobile.cshtml"
           Write(Html.RouteLink("Find an apprenticeship", CandidateRouteNames.ApprenticeshipSearch, null, new { id = "find-apprenticeship-link"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </li>\r\n");

            
            #line 16 "..\..\Views\Account\VerifyMobile.cshtml"
            
            
            #line default
            #line hidden
            
            #line 16 "..\..\Views\Account\VerifyMobile.cshtml"
             if (Model.TraineeshipFeature.ShowTraineeshipsLink)
            {

            
            #line default
            #line hidden
WriteLiteral("                <li>\r\n");

WriteLiteral("                    ");

            
            #line 19 "..\..\Views\Account\VerifyMobile.cshtml"
               Write(Html.RouteLink("Find a traineeship", CandidateRouteNames.TraineeshipSearch, null, new { id = "find-traineeship-link"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </li>\r\n");

            
            #line 21 "..\..\Views\Account\VerifyMobile.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </ul>\r\n    </div>\r\n</div>\r\n\r\n<div");

WriteLiteral(" class=\"grid-row\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"column-two-thirds\"");

WriteLiteral(">\r\n        <p>To receive notifications via text message, you\'ll need to verify yo" +
"ur mobile number by entering the code that was sent to your mobile phone.</p>\r\n\r" +
"\n\r\n");

            
            #line 31 "..\..\Views\Account\VerifyMobile.cshtml"
        
            
            #line default
            #line hidden
            
            #line 31 "..\..\Views\Account\VerifyMobile.cshtml"
         using (Html.BeginRouteForm(CandidateRouteNames.VerifyMobile, FormMethod.Post, new { @id = "verify-mobile-form" }))
        {
            
            
            #line default
            #line hidden
            
            #line 33 "..\..\Views\Account\VerifyMobile.cshtml"
       Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 33 "..\..\Views\Account\VerifyMobile.cshtml"
                                    
            
            
            #line default
            #line hidden
            
            #line 34 "..\..\Views\Account\VerifyMobile.cshtml"
       Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 34 "..\..\Views\Account\VerifyMobile.cshtml"
                                                                   


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <label");

WriteLiteral(" class=\"form-label-bold\"");

WriteLiteral(">Mobile number</label>\r\n                    <span");

WriteLiteral(" class=\"form-prepopped\"");

WriteLiteral(" id=\"phoneNumber\"");

WriteLiteral(">");

            
            #line 38 "..\..\Views\Account\VerifyMobile.cshtml"
                                                             Write(Model.PhoneNumber);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                    \r\n");

WriteLiteral("                    ");

            
            #line 40 "..\..\Views\Account\VerifyMobile.cshtml"
               Write(Html.HiddenFor(m => m.PhoneNumber));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("                    ");

            
            #line 41 "..\..\Views\Account\VerifyMobile.cshtml"
               Write(Html.HiddenFor(m => m.ReturnUrl));

            
            #line default
            #line hidden
WriteLiteral("\r\n                </div>\r\n");

            
            #line 43 "..\..\Views\Account\VerifyMobile.cshtml"


                
            
            #line default
            #line hidden
            
            #line 45 "..\..\Views\Account\VerifyMobile.cshtml"
           Write(Html.FormTextFor(
        m => m.VerifyMobileCode,
        controlHtmlAttributes: new { @maxlength = "4", autofocus = "autofocus" },
        containerHtmlAttributes: new { @class = "form-group-withlink" }));

            
            #line default
            #line hidden
            
            #line 48 "..\..\Views\Account\VerifyMobile.cshtml"
                                                                        

            
            #line default
            #line hidden
WriteLiteral("            <div");

WriteLiteral(" class=\"form-group inline\"");

WriteLiteral(">\r\n                <button");

WriteLiteral(" class=\"button\"");

WriteLiteral(" id=\"verify-code-button\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" name=\"VerifyMobileAction\"");

WriteLiteral(" value=\"VerifyMobile\"");

WriteLiteral(">Verify number</button>\r\n                <button");

WriteLiteral(" class=\"button sfa-hide-button\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" name=\"VerifyMobileAction\"");

WriteLiteral(" value=\"VerifyMobile\"");

WriteLiteral(" tabindex=\"-1\"");

WriteLiteral(">Verify number</button>\r\n                <button");

WriteLiteral(" id=\"ResendMobileVerificationCodeLink\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" name=\"VerifyMobileAction\"");

WriteLiteral(" class=\"button sfa-button-secondary\"");

WriteLiteral(" value=\"Resend\"");

WriteLiteral(" formnovalidate>Resend code</button>\r\n            </div>\r\n");

            
            #line 54 "..\..\Views\Account\VerifyMobile.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n</div>\r\n\r\n");

        }
    }
}
#pragma warning restore 1591
