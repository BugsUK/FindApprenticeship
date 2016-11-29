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

namespace SFA.Apprenticeships.Web.Candidate.Views.Home
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Home/Helpdesk.cshtml")]
    public partial class Helpdesk : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Candidate.ViewModels.Home.ContactMessageViewModel>
    {
        public Helpdesk()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\Home\Helpdesk.cshtml"
  
    ViewBag.Title = "Contact us - Find an apprenticeship";
    Layout = "~/Views/Shared/_Layout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"hgroup\"");

WriteLiteral(">\r\n    <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">Contact us</h1>\r\n</div>\r\n\r\n");

            
            #line 12 "..\..\Views\Home\Helpdesk.cshtml"
 using (Html.BeginRouteForm(CandidateRouteNames.Helpdesk, FormMethod.Post))
{
    
            
            #line default
            #line hidden
            
            #line 14 "..\..\Views\Home\Helpdesk.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 14 "..\..\Views\Home\Helpdesk.cshtml"
                            
    
            
            #line default
            #line hidden
            
            #line 15 "..\..\Views\Home\Helpdesk.cshtml"
Write(Html.FormTextFor(
            m => m.Name,
            containerHtmlAttributes: new { @class = "form-group-compound" },
            controlHtmlAttributes: new { type = "text", autocorrect = "off", maxlength = "71" }));

            
            #line default
            #line hidden
            
            #line 18 "..\..\Views\Home\Helpdesk.cshtml"
                                                                                                

    
            
            #line default
            #line hidden
            
            #line 20 "..\..\Views\Home\Helpdesk.cshtml"
Write(Html.FormTextFor(
            m => m.Email,
            containerHtmlAttributes: new { @class = "form-group-compound" },
                controlHtmlAttributes: new { type = "email", spellcheck = "false", maxlength = "100" },
            hintHtmlAttributes: new { @class = "text" }));

            
            #line default
            #line hidden
            
            #line 24 "..\..\Views\Home\Helpdesk.cshtml"
                                                        


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <label");

WriteLiteral(" for=\"contact-subject\"");

WriteLiteral(" class=\"form-label-bold\"");

WriteLiteral(">Briefly describe your question</label>\r\n");

WriteLiteral("        ");

            
            #line 28 "..\..\Views\Home\Helpdesk.cshtml"
   Write(Html.DropDownListFor(m => m.SelectedEnquiry, Model.Enquiries, new { @id = "contact-subject", @class = "hide-nojs small-btm-margin select-inject form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n        <div");

WriteLiteral(" id=\"changeEmailAddress\"");

WriteLiteral(" class=\"selfServe panel panel-border-narrow toggle-content hide-nojs text\"");

WriteLiteral(">\r\n            <p>If you\'d like to change your email address visit the <a");

WriteAttribute("href", Tuple.Create(" href=\"", 1399), Tuple.Create("\"", 1443)
            
            #line 31 "..\..\Views\Home\Helpdesk.cshtml"
, Tuple.Create(Tuple.Create("", 1406), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(RouteNames.UpdateEmail)
            
            #line default
            #line hidden
, 1406), false)
);

WriteLiteral(">update email</a> page. If you\'re still having problems contact us using this for" +
"m.</p>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" id=\"forgottenEmailAddress\"");

WriteLiteral(" class=\"selfServe panel panel-border-narrow toggle-content hide-nojs text\"");

WriteLiteral(">\r\n            <p>If you\'ve forgotten your email address visit the <a");

WriteAttribute("href", Tuple.Create(" href=\"", 1733), Tuple.Create("\"", 1786)
            
            #line 35 "..\..\Views\Home\Helpdesk.cshtml"
, Tuple.Create(Tuple.Create("", 1740), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(RouteNames.ForgottenCredentials)
            
            #line default
            #line hidden
, 1740), false)
);

WriteLiteral(">forgotten email</a> page. If you\'re still having problems contact us using this " +
"form.</p>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" id=\"cantSignIn\"");

WriteLiteral(" class=\"selfServe panel panel-border-narrow toggle-content hide-nojs text\"");

WriteLiteral(">\r\n            <p>If you\'re having trouble signing in to your account visit the <" +
"a");

WriteAttribute("href", Tuple.Create(" href=\"", 2081), Tuple.Create("\"", 2134)
            
            #line 39 "..\..\Views\Home\Helpdesk.cshtml"
      , Tuple.Create(Tuple.Create("", 2088), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(RouteNames.ForgottenCredentials)
            
            #line default
            #line hidden
, 2088), false)
);

WriteLiteral(">forgotten password</a> page. If you\'re still having problems contact us using th" +
"is form.</p>\r\n        </div>\r\n\r\n        <div");

WriteLiteral(" id=\"cantActivate\"");

WriteLiteral(" class=\"selfServe panel panel-border-narrow toggle-content hide-nojs text\"");

WriteLiteral(">\r\n            <p><a");

WriteAttribute("href", Tuple.Create(" href=\"", 2372), Tuple.Create("\"", 2411)
            
            #line 43 "..\..\Views\Home\Helpdesk.cshtml"
, Tuple.Create(Tuple.Create("", 2379), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(RouteNames.SignIn)
            
            #line default
            #line hidden
, 2379), false)
);

WriteLiteral(@">Activate your account</a> by signing in. You'll need the email address and password you used to create your account.</p>
            <p>If you haven't received your activation code, you can re-send it from the activation page. Make sure you've checked your junk email folder.</p>
            <p>If you're still having problems contact us using this form.</p>
        </div>

");

WriteLiteral("        ");

            
            #line 48 "..\..\Views\Home\Helpdesk.cshtml"
   Write(Html.FormTextFor(
            m => m.Enquiry,
            containerHtmlAttributes: new { @class = "form-group-compound" },
          controlHtmlAttributes: new { type = "text", autocorrect = "off", @class = "select-injected", maxlength = "100" }, labelText: string.Empty));

            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n");

            
            #line 53 "..\..\Views\Home\Helpdesk.cshtml"
    
            
            #line default
            #line hidden
            
            #line 53 "..\..\Views\Home\Helpdesk.cshtml"
Write(Html.FormTextAreaFor(m => m.Details,
                controlHtmlAttributes: new { @data_val_length_max = "4000", rows = "4", role = "textbox", aria_multiline = "true" },
                hintHtmlAttributes: new { @class = "text" }));

            
            #line default
            #line hidden
            
            #line 55 "..\..\Views\Home\Helpdesk.cshtml"
                                                            

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <button");

WriteLiteral(" id=\"send-contact-form-button\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(">Send</button>\r\n    </div>\r\n");

            
            #line 59 "..\..\Views\Home\Helpdesk.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("<section");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n    <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(@">Apprenticeship helpline</h2>
    <p>
        Contact the helpline if you’d prefer to speak to somebody, or if
        you're having difficulty with the contact form.
    </p>
    <p>
        Phone:
        <br>0800 015 0400
    </p>
    <p>
        Email: <a");

WriteLiteral(" href=\"mailto:nationalhelpdesk@findapprenticeship.service.gov.uk\"");

WriteLiteral(">nationalhelpdesk@findapprenticeship.service.gov.uk</a>\r\n    </p>\r\n    <p>\r\n     " +
"   Contact <a");

WriteLiteral(" href=\"https://nationalcareersservice.direct.gov.uk/Pages/Home.aspx\"");

WriteLiteral("\r\n                   rel=\"external\"");

WriteLiteral(">National Careers Service</a> (NCS) if you have a\r\n        question about whether" +
" an apprenticeship is right for you.\r\n    </p>\r\n</section>\r\n");

        }
    }
}
#pragma warning restore 1591
