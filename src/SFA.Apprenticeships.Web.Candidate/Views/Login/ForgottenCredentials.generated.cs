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

namespace SFA.Apprenticeships.Web.Candidate.Views.Login
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Login/ForgottenCredentials.cshtml")]
    public partial class ForgottenCredentials : System.Web.Mvc.WebViewPage<ForgottenCredentialsViewModel>
    {
        public ForgottenCredentials()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Login\ForgottenCredentials.cshtml"
  
    ViewBag.Title = "Can't access account - Find an apprenticeship";
    Layout = "~/Views/Shared/_Layout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"hgroup\"");

WriteLiteral(">\r\n    <h1");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Can\'t access your account?</h1>\r\n</div>\r\n\r\n");

            
            #line 12 "..\..\Views\Login\ForgottenCredentials.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 14 "..\..\Views\Login\ForgottenCredentials.cshtml"
 using (Html.BeginForm("ForgottenPassword", "Login", FormMethod.Post, new { @id = "forgotten-password-form" }))
{
    
            
            #line default
            #line hidden
            
            #line 16 "..\..\Views\Login\ForgottenCredentials.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 16 "..\..\Views\Login\ForgottenCredentials.cshtml"
                            


            
            #line default
            #line hidden
WriteLiteral("    <section>\r\n        <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(">If you have an account with the old service</h2>\r\n            <p");

WriteLiteral(" class=\"med-btm-margin\"");

WriteLiteral(">\r\n                <img");

WriteAttribute("src", Tuple.Create(" src=\"", 660), Tuple.Create("\"", 712)
, Tuple.Create(Tuple.Create("", 666), Tuple.Create<System.Object, System.Int32>(Href("~/Content/_assets/img/logo-apprenticeships.png")
, 666), false)
);

WriteLiteral(" width=\"133\"");

WriteLiteral(" height=\"50\"");

WriteLiteral(" alt=\"Logo from the old Apprenticeships vacancies service\"");

WriteLiteral(" title=\"Logo from the old Apprenticeships vacancies service\"");

WriteLiteral(" align=\"left\"");

WriteLiteral(" />\r\n                You won\'t be able to sign in using existing \"Apprenticeship " +
"vacancies\" details.\r\n            </p>\r\n            <p>You must <a");

WriteAttribute("href", Tuple.Create(" href=\"", 1014), Tuple.Create("\"", 1053)
            
            #line 25 "..\..\Views\Login\ForgottenCredentials.cshtml"
, Tuple.Create(Tuple.Create("", 1021), Tuple.Create<System.Object, System.Int32>(Url.Action("Index", "Register")
            
            #line default
            #line hidden
, 1021), false)
);

WriteLiteral(">create an account</a> to access this new service.</p>\r\n        </div>\r\n    </sec" +
"tion>\r\n");

WriteLiteral("    <section>\r\n        <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n            \r\n            <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(">Forgotten your password?</h2>\r\n            <p>\r\n                To reset your pa" +
"ssword we need to send you a 6-character code. If you can’t see it in your inbox" +
" within a few minutes, please check your spam folder.\r\n            </p>\r\n       " +
" </div>\r\n\r\n");

WriteLiteral("        ");

            
            #line 37 "..\..\Views\Login\ForgottenCredentials.cshtml"
   Write(Html.FormTextFor(m => m.ForgottenPasswordViewModel.EmailAddress, controlHtmlAttributes: new { @class = "linked-input-master", type = "email", spellcheck = "false"}, hintHtmlAttributes: new { @class = "text" }, labelText: "Enter email"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <button");

WriteLiteral(" class=\"button\"");

WriteLiteral(" id=\"forgottenpassword-button\"");

WriteLiteral(">Send code</button>\r\n        </div>\r\n    </section>\r\n");

            
            #line 43 "..\..\Views\Login\ForgottenCredentials.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n    <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(">If your account is locked</h2>\r\n    <p>\r\n        If your account is locked and y" +
"ou’ve been sent a code to unlock it, you\r\n        can <a");

WriteAttribute("href", Tuple.Create(" href=\"", 2080), Tuple.Create("\"", 2117)
            
            #line 49 "..\..\Views\Login\ForgottenCredentials.cshtml"
, Tuple.Create(Tuple.Create("", 2087), Tuple.Create<System.Object, System.Int32>(Url.Action("Unlock", "Login")
            
            #line default
            #line hidden
, 2087), false)
);

WriteLiteral(">enter it</a> now.\r\n    </p>\r\n</div>\r\n\r\n");

            
            #line 53 "..\..\Views\Login\ForgottenCredentials.cshtml"
 using (Html.BeginForm("ForgottenEmail", "Login", FormMethod.Post, new { @id = "forgotten-email-form" }))
{
    
            
            #line default
            #line hidden
            
            #line 55 "..\..\Views\Login\ForgottenCredentials.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 55 "..\..\Views\Login\ForgottenCredentials.cshtml"
                            


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n        <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(">If you\'ve forgotten your email address</h2>\r\n        <details>\r\n            <sum" +
"mary>Retrieve your email address</summary>\r\n            <div");

WriteLiteral(" class=\"detail-content\"");

WriteLiteral(">\r\n                <p>\r\n                    To retrieve the email address you use" +
"d to create your account, you\'ll need to verify your mobile number. Enter your n" +
"umber and we\'ll send your email address via text message.\r\n                </p>\r" +
"\n\r\n");

WriteLiteral("                ");

            
            #line 66 "..\..\Views\Login\ForgottenCredentials.cshtml"
           Write(Html.FormTextFor(m => m.ForgottenEmailViewModel.PhoneNumber, controlHtmlAttributes: new { type = "tel" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <button");

WriteLiteral(" class=\"button\"");

WriteLiteral(" id=\"forgottenemail-button\"");

WriteLiteral(">Send email address</button>\r\n                </div>\r\n            </div>\r\n       " +
" </details>\r\n    </div>\r\n");

            
            #line 74 "..\..\Views\Login\ForgottenCredentials.cshtml"
}

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
