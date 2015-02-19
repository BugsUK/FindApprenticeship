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

namespace SFA.Apprenticeships.Web.Candidate.Views.Shared
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
    
    #line 1 "..\..\Views\Shared\_Layout.cshtml"
    using SFA.Apprenticeships.Web.Candidate.Controllers;
    
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
    
    #line 2 "..\..\Views\Shared\_Layout.cshtml"
    using SFA.Apprenticeships.Web.Common.Framework;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/_Layout.cshtml")]
    public partial class Layout : System.Web.Mvc.WebViewPage<dynamic>
    {
        public Layout()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n<!DOCTYPE html>\r\n<!--[if lt IE 7]><html class=\"no-js lt-ie9 lt-ie8 lt-ie7\"><![e" +
"ndif]-->\r\n<!--[if IE 7]><html class=\"no-js lt-ie9 lt-ie8\"><![endif]-->\r\n<!--[if " +
"IE 8]><html class=\"no-js lt-ie9\"><![endif]-->\r\n<!--[if gt IE 8]><!-->\r\n<html");

WriteLiteral(" lang=\"en-GB\"");

WriteLiteral(" class=\"no-js not-ie8\"");

WriteLiteral(">\r\n<!--<![endif]-->\r\n<head>\r\n    <meta");

WriteLiteral(" charset=\"utf-8\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" http-equiv=\"X-UA-Compatible\"");

WriteLiteral(" content=\"IE=edge\"");

WriteLiteral(">\r\n    <title>");

            
            #line 14 "..\..\Views\Shared\_Layout.cshtml"
      Write(ViewBag.Title);

            
            #line default
            #line hidden
WriteLiteral("</title>\r\n    <meta");

WriteLiteral(" name=\"description\"");

WriteLiteral(" content=\"We’ve introduced a new way to find and apply for an apprenticeship in E" +
"ngland.\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width, initial-scale=1\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"format-detection\"");

WriteLiteral(" content=\"telephone=no\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"format-detection\"");

WriteLiteral(" content=\"date=no\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"format-detection\"");

WriteLiteral(" content=\"address=no\"");

WriteLiteral(">\r\n    <meta");

WriteLiteral(" name=\"DCSext.Authenticated\"");

WriteAttribute("content", Tuple.Create(" content=\"", 927), Tuple.Create("\"", 978)
            
            #line 20 "..\..\Views\Shared\_Layout.cshtml"
, Tuple.Create(Tuple.Create("", 937), Tuple.Create<System.Object, System.Int32>(Request.IsAuthenticated ? "Yes" : "No"
            
            #line default
            #line hidden
, 937), false)
);

WriteLiteral(" />\r\n");

WriteLiteral("    ");

            
            #line 21 "..\..\Views\Shared\_Layout.cshtml"
Write(RenderSection("metatags", false));

            
            #line default
            #line hidden
WriteLiteral("\r\n    <!--[if gt IE 8]><!-->");

            
            #line 22 "..\..\Views\Shared\_Layout.cshtml"
                     Write(Styles.Render(Url.CdnContent("css/main-1.5.0.1098.css", "~/Content/_assets")));

            
            #line default
            #line hidden
WriteLiteral("<!--<![endif]-->\r\n    <!--[if lte IE 8]>");

            
            #line 23 "..\..\Views\Shared\_Layout.cshtml"
                 Write(Styles.Render(Url.CdnContent("css/main-ie8-1.5.0.1098.css", "~/Content/_assets")));

            
            #line default
            #line hidden
WriteLiteral("<![endif]-->\r\n    <!--[if lte IE 8]>");

            
            #line 24 "..\..\Views\Shared\_Layout.cshtml"
                 Write(Styles.Render(Url.CdnContent("css/fonts-ie8.css", "~/Content/_assets")));

            
            #line default
            #line hidden
WriteLiteral("<![endif]-->\r\n    <!--[if gte IE 9]><!-->");

            
            #line 25 "..\..\Views\Shared\_Layout.cshtml"
                      Write(Styles.Render(Url.CdnContent("css/fonts.css", "~/Content/_assets")));

            
            #line default
            #line hidden
WriteLiteral("<!--<![endif]-->\r\n    <link");

WriteLiteral(" rel=\"shortcut icon\"");

WriteAttribute("href", Tuple.Create(" href=\"", 1511), Tuple.Create("\"", 1573)
            
            #line 26 "..\..\Views\Shared\_Layout.cshtml"
, Tuple.Create(Tuple.Create("", 1518), Tuple.Create<System.Object, System.Int32>(Url.CdnContent("img/favicon.ico", "~/Content/_assets")
            
            #line default
            #line hidden
, 1518), false)
);

WriteLiteral(" type=\"image/x-icon\"");

WriteLiteral(">\r\n");

WriteLiteral("    ");

            
            #line 27 "..\..\Views\Shared\_Layout.cshtml"
Write(Styles.Render(Url.CdnContent("css/font-awesome/css/font-awesome.min.css", "~/Content/_assets")));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 28 "..\..\Views\Shared\_Layout.cshtml"
Write(Html.Partial("_Scripts"));

            
            #line default
            #line hidden
WriteLiteral("\r\n</head>\r\n<body>\r\n");

            
            #line 31 "..\..\Views\Shared\_Layout.cshtml"
    
            
            #line default
            #line hidden
            
            #line 31 "..\..\Views\Shared\_Layout.cshtml"
     if (ViewBag.ShowEuCookieDirective == true)
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" id=\"global-cookie-message\"");

WriteLiteral(" class=\"cookie-banner\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"content-container\"");

WriteLiteral(" role=\"alert\"");

WriteLiteral(">\r\n                <span");

WriteLiteral(" class=\"copy-16\"");

WriteLiteral(">GOV.UK uses cookies to make the site simpler. <a");

WriteAttribute("href", Tuple.Create(" href=\'", 2011), Tuple.Create("\'", 2060)
            
            #line 35 "..\..\Views\Shared\_Layout.cshtml"
             , Tuple.Create(Tuple.Create("", 2018), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.Privacy)
            
            #line default
            #line hidden
, 2018), false)
);

WriteLiteral(">Find out more about cookies</a></span>\r\n            </div>\r\n        </div>\r\n");

            
            #line 38 "..\..\Views\Shared\_Layout.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 40 "..\..\Views\Shared\_Layout.cshtml"
    
            
            #line default
            #line hidden
            
            #line 40 "..\..\Views\Shared\_Layout.cshtml"
     if (!string.IsNullOrEmpty(ViewBag.PlannedOutageMessage))
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" id=\"planned-outage-message\"");

WriteLiteral(" class=\"maintenance-banner\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"content-container\"");

WriteLiteral(" role=\"alert\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"font-xsmall maintenance-content\"");

WriteLiteral(">\r\n                    <div>");

            
            #line 45 "..\..\Views\Shared\_Layout.cshtml"
                    Write(Html.Raw(ViewBag.PlannedOutageMessage));

            
            #line default
            #line hidden
WriteLiteral("</div>\r\n                    <a");

WriteAttribute("href", Tuple.Create(" href=\"", 2498), Tuple.Create("\"", 2588)
            
            #line 46 "..\..\Views\Shared\_Layout.cshtml"
, Tuple.Create(Tuple.Create("", 2505), Tuple.Create<System.Object, System.Int32>(Url.Action("DismissPlannedOutageMessage", "Account", new { isJavascript = false })
            
            #line default
            #line hidden
, 2505), false)
);

WriteLiteral(" class=\"maintenance-close\"");

WriteLiteral(">\r\n                        <i");

WriteLiteral(" id=\"dismiss-planned-outage-message\"");

WriteLiteral(" class=\"icon-black fa fa-times-circle\"");

WriteLiteral("></i>\r\n                    </a>\r\n                </div>\r\n            </div>\r\n    " +
"    </div>\r\n");

            
            #line 52 "..\..\Views\Shared\_Layout.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n    <div");

WriteLiteral(" class=\"skiplink-container\"");

WriteLiteral(">\r\n        <div>\r\n            <a");

WriteLiteral(" href=\"#main\"");

WriteLiteral(" class=\"skiplink\"");

WriteLiteral(">Skip to main content</a>\r\n        </div>\r\n    </div>\r\n    <header");

WriteLiteral(" role=\"banner\"");

WriteLiteral(" class=\"global-header\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"global-header__wrapper\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"global-header__logo\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\"http://gov.uk\"");

WriteLiteral(" title=\"Go to the GOV.UK homepage\"");

WriteLiteral(" class=\"govuk-logo\"");

WriteLiteral(">\r\n                    <img");

WriteAttribute("src", Tuple.Create(" src=\"", 3234), Tuple.Create("\"", 3309)
            
            #line 63 "..\..\Views\Shared\_Layout.cshtml"
, Tuple.Create(Tuple.Create("", 3240), Tuple.Create<System.Object, System.Int32>(Url.CdnContent("img/gov.uk_logotype_crown.png", "~/Content/_assets")
            
            #line default
            #line hidden
, 3240), false)
);

WriteLiteral(" alt=\"Crown\"");

WriteLiteral(">\r\n                    GOV.UK\r\n                </a>\r\n            </div>\r\n");

WriteLiteral("            ");

            
            #line 67 "..\..\Views\Shared\_Layout.cshtml"
       Write(RenderSection("TitleLink", true));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </header>\r\n    <div");

WriteLiteral(" class=\"content-container\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"phase-notice gov-border\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"prototype-warning\"");

WriteLiteral(">\r\n                <strong");

WriteLiteral(" class=\"beta__label\"");

WriteLiteral(">Beta</strong> <span>This is a new service – your <a");

WriteLiteral(" rel=\"external\"");

WriteLiteral(" href=\"https://www.surveymonkey.com/s/MFNR7NZ\"");

WriteLiteral(" target=\"_blank\"");

WriteLiteral(">feedback</a> will help to improve it.</span>\r\n            </div>\r\n");

WriteLiteral("            ");

            
            #line 75 "..\..\Views\Shared\_Layout.cshtml"
       Write(Html.Partial("_LoginPartial"));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <main");

WriteLiteral(" role=\"main\"");

WriteLiteral(" id=\"main\"");

WriteLiteral(">\r\n");

            
            #line 78 "..\..\Views\Shared\_Layout.cshtml"
            
            
            #line default
            #line hidden
            
            #line 78 "..\..\Views\Shared\_Layout.cshtml"
              
                var controller = ViewContext.Controller as CandidateControllerBase;

                if (controller != null)
                {
                    var infoMessage = controller.UserData.Pop(UserMessageConstants.InfoMessage);
                    var successMessage = controller.UserData.Pop(UserMessageConstants.SuccessMessage);
                    var warningMessage = controller.UserData.Pop(UserMessageConstants.WarningMessage);
                    var errorMessage = controller.UserData.Pop(UserMessageConstants.ErrorMessage);

                    if (infoMessage != null)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" role=\"alert\"");

WriteLiteral(" class=\"panel-info\"");

WriteLiteral(">\r\n                            <p");

WriteLiteral(" id=\"InfoMessageText\"");

WriteLiteral(">");

            
            #line 91 "..\..\Views\Shared\_Layout.cshtml"
                                               Write(Html.Raw(infoMessage));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                        </div>\r\n");

            
            #line 93 "..\..\Views\Shared\_Layout.cshtml"
                    }
                    if (successMessage != null)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" role=\"alert\"");

WriteLiteral(" class=\"panel-success\"");

WriteLiteral(">\r\n                            <p");

WriteLiteral(" id=\"SuccessMessageText\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-check\"");

WriteLiteral("></i>");

            
            #line 97 "..\..\Views\Shared\_Layout.cshtml"
                                                                             Write(Html.Raw(successMessage));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                        </div>\r\n");

            
            #line 99 "..\..\Views\Shared\_Layout.cshtml"
                    }
                    if (warningMessage != null)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" role=\"alert\"");

WriteLiteral(" class=\"panel-warning\"");

WriteLiteral(">\r\n                            <p");

WriteLiteral(" id=\"WarningMessageText\"");

WriteLiteral(">");

            
            #line 103 "..\..\Views\Shared\_Layout.cshtml"
                                                  Write(Html.Raw(warningMessage));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                        </div>\r\n");

            
            #line 105 "..\..\Views\Shared\_Layout.cshtml"
                    }
                    if (errorMessage != null)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <div");

WriteLiteral(" role=\"alert\"");

WriteLiteral(" class=\"panel-danger\"");

WriteLiteral(">\r\n                            <p");

WriteLiteral(" id=\"ErrorMessageText\"");

WriteLiteral(">");

            
            #line 109 "..\..\Views\Shared\_Layout.cshtml"
                                                Write(Html.Raw(errorMessage));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                        </div>\r\n");

            
            #line 111 "..\..\Views\Shared\_Layout.cshtml"
                    }
                }
            
            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 114 "..\..\Views\Shared\_Layout.cshtml"
       Write(RenderBody());

            
            #line default
            #line hidden
WriteLiteral("\r\n        </main>\r\n");

            
            #line 116 "..\..\Views\Shared\_Layout.cshtml"
        
            
            #line default
            #line hidden
            
            #line 116 "..\..\Views\Shared\_Layout.cshtml"
         if (ViewBag.EnableWebTrends == true)
        {

            
            #line default
            #line hidden
WriteLiteral("            <noscript>\r\n                <img");

WriteLiteral(" alt=\"dcsimg\"");

WriteLiteral(" id=\"dcsimg\"");

WriteLiteral(" width=\"1\"");

WriteLiteral(" height=\"1\"");

WriteAttribute("src", Tuple.Create(" src=\"", 5860), Tuple.Create("\"", 6022)
, Tuple.Create(Tuple.Create("", 5866), Tuple.Create("//stats.matraxis.net/", 5866), true)
            
            #line 119 "..\..\Views\Shared\_Layout.cshtml"
             , Tuple.Create(Tuple.Create("", 5887), Tuple.Create<System.Object, System.Int32>(ViewBag.WebTrendsDscId
            
            #line default
            #line hidden
, 5887), false)
, Tuple.Create(Tuple.Create("", 5910), Tuple.Create("/njs.gif?dcsuri=/nojavascript&amp;WT.js=No&amp;WT.tv=10.4.11&amp;WT.dl=0&amp;dcss" +
"ip=", 5910), true)
            
            #line 119 "..\..\Views\Shared\_Layout.cshtml"
                                                                                                                        , Tuple.Create(Tuple.Create("", 5994), Tuple.Create<System.Object, System.Int32>(ViewBag.WebTrendsDomainName
            
            #line default
            #line hidden
, 5994), false)
);

WriteLiteral(" />\r\n            </noscript>\r\n");

            
            #line 121 "..\..\Views\Shared\_Layout.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n\r\n    <footer");

WriteLiteral(" class=\"gov-border\"");

WriteLiteral(" role=\"contentinfo\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"footer__wrapper\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"footer__meta\"");

WriteLiteral(">\r\n                <ul");

WriteLiteral(" class=\"footer__nav\"");

WriteLiteral(">\r\n                    <li");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral("><a");

WriteLiteral(" class=\"bold-medium\"");

WriteAttribute("href", Tuple.Create(" href=\'", 6322), Tuple.Create("\'", 6372)
            
            #line 128 "..\..\Views\Shared\_Layout.cshtml"
, Tuple.Create(Tuple.Create("", 6329), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.Helpdesk)
            
            #line default
            #line hidden
, 6329), false)
);

WriteLiteral(">Contact us</a></li>\r\n                    <li");

WriteLiteral(" class=\"footer__link\"");

WriteLiteral("><a");

WriteAttribute("href", Tuple.Create(" href=\'", 6442), Tuple.Create("\'", 6491)
            
            #line 129 "..\..\Views\Shared\_Layout.cshtml"
, Tuple.Create(Tuple.Create("", 6449), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.Privacy)
            
            #line default
            #line hidden
, 6449), false)
);

WriteLiteral(">Privacy and cookies</a></li>\r\n                    <li");

WriteLiteral(" class=\"footer__link\"");

WriteLiteral("><a");

WriteAttribute("href", Tuple.Create(" href=\'", 6570), Tuple.Create("\'", 6617)
            
            #line 130 "..\..\Views\Shared\_Layout.cshtml"
, Tuple.Create(Tuple.Create("", 6577), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.Terms)
            
            #line default
            #line hidden
, 6577), false)
);

WriteLiteral(">Terms and conditions</a></li>\r\n                    <li");

WriteLiteral(" class=\"footer__link\"");

WriteLiteral(">Built by the <a");

WriteLiteral(" href=\"http://gov.uk/sfa\"");

WriteLiteral(" target=\"_blank\"");

WriteLiteral(">Skills Funding Agency</a></li>\r\n                    <li");

WriteLiteral(" class=\"footer__ogl\"");

WriteLiteral("><a");

WriteLiteral(" href=\"http://www.nationalarchives.gov.uk/doc/open-government-licence/version/2\"");

WriteLiteral(" class=\"ir ogl-logo\"");

WriteLiteral(">OGL</a>All content is available under the <a");

WriteLiteral(" href=\"http://www.nationalarchives.gov.uk/doc/open-government-licence/version/2\"");

WriteLiteral(">Open Government Licence v2.0</a>, except where otherwise stated</li>\r\n          " +
"      </ul>\r\n                <a");

WriteLiteral(" class=\"footer__copyright\"");

WriteLiteral(" href=\"http://www.nationalarchives.gov.uk/information-management/our-services/cro" +
"wn-copyright.htm\"");

WriteLiteral(" target=\"_blank\"");

WriteLiteral(">\r\n                    <img");

WriteAttribute("src", Tuple.Create(" src=\"", 7334), Tuple.Create("\"", 7402)
            
            #line 135 "..\..\Views\Shared\_Layout.cshtml"
, Tuple.Create(Tuple.Create("", 7340), Tuple.Create<System.Object, System.Int32>(Url.CdnContent("img/govuk-crest-2x.png", "~/Content/_assets")
            
            #line default
            #line hidden
, 7340), false)
);

WriteLiteral(" width=\"125\"");

WriteLiteral(" height=\"102\"");

WriteLiteral(" alt=\"Crown copyright logo\"");

WriteLiteral(">\r\n                    <p>&copy; Crown copyright</p>\r\n                </a>\r\n     " +
"       </div>\r\n        </div>\r\n    </footer>\r\n\r\n");

            
            #line 142 "..\..\Views\Shared\_Layout.cshtml"
    
            
            #line default
            #line hidden
            
            #line 142 "..\..\Views\Shared\_Layout.cshtml"
     if (ViewBag.ShowAbout != null && ViewBag.ShowAbout == true)
    {
        Html.RenderPartial("_about");
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n    <!-- Placed at the end of the document so the pages load faster -->\r\n");

WriteLiteral("    ");

            
            #line 148 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/jquery"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 149 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/fastclick"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 150 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/underscore"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n        if (typeof jQuery === \'undefined\') {\r\n            var e = document.cre" +
"ateElement(\"script\");\r\n\r\n            e.src = \'");

            
            #line 156 "..\..\Views\Shared\_Layout.cshtml"
                Write(Url.Content("~/Content/_assets/js/vendor/jquery-1.11.1.js"));

            
            #line default
            #line hidden
WriteLiteral(@"';
            e.type = ""text/javascript"";
            document.getElementsByTagName(""head"")[0].appendChild(e);
        }

        $(function () {
            $(""#dismiss-planned-outage-message"").click(function (event) {

                event.preventDefault();

                var request = $.ajax({
                    type: ""GET"",
                    url: '");

            
            #line 168 "..\..\Views\Shared\_Layout.cshtml"
                     Write(Url.Action("DismissPlannedOutageMessage", "Account", new { isJavascript = true }));

            
            #line default
            #line hidden
WriteLiteral("\'\r\n                });\r\n\r\n                request.done(function () {\r\n\r\n         " +
"           $(\"#planned-outage-message\").hide();\r\n\r\n                });\r\n        " +
"    });\r\n        });\r\n    </script>\r\n\r\n");

WriteLiteral("    ");

            
            #line 180 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/nascript"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 181 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/vendor"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 182 "..\..\Views\Shared\_Layout.cshtml"
Write(Scripts.Render("~/bundles/nas"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("    ");

            
            #line 184 "..\..\Views\Shared\_Layout.cshtml"
Write(RenderSection("scripts", required: false));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n</body>\r\n</html>\r\n");

        }
    }
}
#pragma warning restore 1591
