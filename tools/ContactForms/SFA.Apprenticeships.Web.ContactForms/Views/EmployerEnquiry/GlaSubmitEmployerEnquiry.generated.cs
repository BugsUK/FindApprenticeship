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

namespace ASP
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
    using SFA.Apprenticeships.Web.ContactForms;
    
    #line 1 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
    using SFA.Apprenticeships.Web.ContactForms.Framework;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/EmployerEnquiry/GlaSubmitEmployerEnquiry.cshtml")]
    public partial class _Views_EmployerEnquiry_GlaSubmitEmployerEnquiry_cshtml : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.ContactForms.ViewModels.EmployerEnquiryViewModel>
    {
        public _Views_EmployerEnquiry_GlaSubmitEmployerEnquiry_cshtml()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
  
    ViewBag.Title = "GLA employer enquiry form";
    Layout = "~/Views/Shared/_LayoutNoHeaderFooter.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n        <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">Enquiry form</h1>\r\n    </div>\r\n</div>\r\n\r\n\r\n<div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n    <p>\r\n        If you are a London business and interested in taking on an a" +
"pprentice or finding out more, please complete the form below.\r\n    </p>\r\n\r\n    " +
"<p>\r\n        The data you enter in the form below is supplied directly to our pa" +
"rtner, the <a");

WriteLiteral(" rel=\"external\"");

WriteLiteral(" href=\"https://www.gov.uk/further-education-skills/apprenticeships\"");

WriteLiteral(@">National Apprenticeship Service </a> who will contact you within 2 working days to provide you with information you need.
    </p>

    <p>
        <b>If you need more immediate advice, please call NAS on 08000 150 600.</b>
    </p>

    <p>
        Different organisations deal with apprenticeships in <a");

WriteLiteral(" href=\"http://www.mappit.org.uk/\"");

WriteLiteral(" rel=\"external\"");

WriteLiteral(">Scotland</a>, <a");

WriteLiteral(" href=\"http://www.nidirect.gov.uk/index/information-and-services/education-and-le" +
"arning/14-19/starter-skills-16-18/apprenticeships.htm\"");

WriteLiteral(" rel=\"external\"");

WriteLiteral(">Northern Ireland</a> and <a");

WriteLiteral(" href=\"https://ams.careerswales.com/Public/Default.aspx?mode=vacancy&type=ams\"");

WriteLiteral(" rel=\"external\"");

WriteLiteral(">Wales</a>\r\n    </p>\r\n</div>\r\n\r\n\r\n\r\n");

            
            #line 36 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
 using (Html.BeginForm("GlaSubmitEmployerEnquiry", "EmployerEnquiry", FormMethod.Post))
{
    
            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 38 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
                                                           


            
            #line default
            #line hidden
WriteLiteral("    <fieldset>\r\n        <legend");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Your details</legend>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 44 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.FormLabelFor(m => m.Title));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 45 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.DropDownListFor(m => m.Title, Model.TitleList));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 46 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.ValidationMessageFor(model => model.Title));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n");

WriteLiteral("        ");

            
            #line 48 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
   Write(Html.FormTextFor(
            m => m.Firstname,
            controlHtmlAttributes: new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("        ");

            
            #line 52 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
   Write(Html.FormTextFor(
            m => m.Lastname,
            controlHtmlAttributes: new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("        ");

            
            #line 56 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
   Write(Html.FormTextFor(
            m => m.Position,
            controlHtmlAttributes: new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("        ");

            
            #line 60 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
   Write(Html.FormTextFor(
            m => m.WorkPhoneNumber, controlHtmlAttributes: new { @class = "form-control", type = "tel" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("        ");

            
            #line 63 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
   Write(Html.FormTextFor(
            m => m.MobileNumber, controlHtmlAttributes: new { @class = "form-control", type = "tel" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("        ");

            
            #line 66 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
   Write(Html.FormTextFor(
            m => m.Email,
            controlHtmlAttributes: new { @class = "form-control", type = "email" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n        <legend");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Company details</legend>\r\n\r\n");

WriteLiteral("        ");

            
            #line 72 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
   Write(Html.FormTextFor(
            m => m.Companyname,
            controlHtmlAttributes: new { @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

WriteLiteral("        ");

            
            #line 76 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
   Write(Html.EditorFor(a => a.Address));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 79 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.FormLabelFor(m => m.EmployeesCount));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 80 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.DropDownListFor(m => m.EmployeesCount, Model.EmployeesCountList));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 81 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.ValidationMessageFor(model => model.EmployeesCount));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 84 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.FormLabelFor(m => m.WorkSector));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 85 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.DropDownListFor(m => m.WorkSector, Model.WorkSectorList));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 86 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.ValidationMessageFor(model => model.WorkSector));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <legend");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Your enquiry</legend>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(" style=\"display: none\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 90 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.FormLabelFor(m => m.EnquiryRelatesTo));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 91 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.DropDownListFor(m => m.EnquiryRelatesTo, Model.EnquiryRelatesToList));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 92 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.ValidationMessageFor(model => model.EnquiryRelatesTo));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n");

WriteLiteral("        ");

            
            #line 94 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
   Write(Html.FormTextAreaFor(
            m => m.EnquiryDescription,
             controlHtmlAttributes: new { @class = "form-control", @rows = 7 }));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n        <legend");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Additional questions</legend>\r\n\r\n        <div");

WriteLiteral(" class=\"form-group text\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 101 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.FormLabelFor(m => m.PreviousExperienceType));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 102 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.DropDownListFor(m => m.PreviousExperienceType, Model.PreviousExperienceTypeList));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 103 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.ValidationMessageFor(model => model.PreviousExperienceType));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"form-group text\"");

WriteLiteral(">\r\n");

WriteLiteral("            ");

            
            #line 106 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.FormLabelFor(m => m.EnquirySource));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 107 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.DropDownListFor(m => m.EnquirySource, Model.EnquirySourceList));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 108 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
       Write(Html.ValidationMessageFor(model => model.EnquirySource));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"hidden\"");

WriteLiteral("><strong>");

            
            #line 110 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
                               Write(Html.Honeypot("UserName"));

            
            #line default
            #line hidden
WriteLiteral("</strong></div>\r\n    </fieldset>\r\n");

WriteLiteral("    <br />\r\n");

WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <p>\r\n            <button");

WriteLiteral(" class=\"button\"");

WriteLiteral(" id=\"submit-query-button\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" >Send Enquiry</button>\r\n        </p>\r\n    </div>\r\n");

            
            #line 118 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
}

            
            #line default
            #line hidden
DefineSection("scripts", () => {

WriteLiteral("\r\n    <script>\r\n        $(function () {\r\n\r\n            $(\"#find-addresses\").addre" +
"ssLookup({\r\n                url: \'");

            
            #line 125 "..\..\Views\EmployerEnquiry\GlaSubmitEmployerEnquiry.cshtml"
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
