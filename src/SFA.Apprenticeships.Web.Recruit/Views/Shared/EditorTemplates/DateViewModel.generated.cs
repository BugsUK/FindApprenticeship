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

namespace SFA.Apprenticeships.Web.Recruit.Views.Shared.EditorTemplates
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
    using SFA.Apprenticeships.Web.Common.Constants;
    
    #line 1 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
    using SFA.Apprenticeships.Web.Common.Extensions;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    
    #line 2 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
    using SFA.Apprenticeships.Web.Common.Validators;
    
    #line default
    #line hidden
    
    #line 3 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
    using SFA.Apprenticeships.Web.Common.Validators.Extensions;
    
    #line default
    #line hidden
    
    #line 4 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
    using SFA.Apprenticeships.Web.Common.ViewModels;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates;
    using SFA.Apprenticeships.Web.Recruit;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/EditorTemplates/DateViewModel.cshtml")]
    public partial class DateViewModel_ : System.Web.Mvc.WebViewPage<DateViewModel>
    {
        public DateViewModel_()
        {
        }
        public override void Execute()
        {
            
            #line 7 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
  
    var classes = new List<ValidationType>();

    classes.Add(Html.GetValidationType(m => m));
    classes.Add(Html.GetValidationType(m => m.Day));
    classes.Add(Html.GetValidationType(m => m.Month));
    classes.Add(Html.GetValidationType(m => m.Year));

    var item = classes.Distinct().OrderByDescending(v => v).First();

    var errorClass = HtmlExtensions.GetValidationCssClass(item);

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteAttribute("class", Tuple.Create(" class=\"", 652), Tuple.Create("\"", 682)
, Tuple.Create(Tuple.Create("", 660), Tuple.Create("form-group", 660), true)
            
            #line 20 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
, Tuple.Create(Tuple.Create(" ", 670), Tuple.Create<System.Object, System.Int32>(errorClass
            
            #line default
            #line hidden
, 671), false)
);

WriteLiteral(">\r\n    <fieldset>\r\n        <legend>\r\n            <span");

WriteLiteral(" class=\"form-label-bold\"");

WriteLiteral(">");

            
            #line 23 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
                                     Write(Html.DisplayNameFor(m => m));

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n            <span");

WriteLiteral(" class=\"form-hint\"");

WriteLiteral(" id=\"example-dob-hint\"");

WriteLiteral(">");

            
            #line 24 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
                                                     Write(Model.GetMetadata(m => m).DisplayName);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n");

WriteLiteral("            ");

            
            #line 25 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
       Write(Html.ValidationMessageWithSeverityFor(m => m, Html.GetValidationType(m => m)));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 26 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
       Write(Html.ValidationMessageWithSeverityFor(m => m.Day, Html.GetValidationType(m => m.Day)));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 27 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
       Write(Html.ValidationMessageWithSeverityFor(m => m.Month, Html.GetValidationType(m => m.Month)));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 28 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
       Write(Html.ValidationMessageWithSeverityFor(m => m.Year, Html.GetValidationType(m => m.Year)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </legend>\r\n        <div");

WriteLiteral(" class=\"form-date\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"form-group form-group-day\"");

WriteLiteral(">\r\n                <label");

WriteLiteral(" class=\"form-label-bold\"");

WriteLiteral(" for=\"example-dob-day\"");

WriteLiteral(">Day</label>\r\n");

WriteLiteral("                ");

            
            #line 33 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
           Write(Html.TextBoxFor(m => m.Day, new { name = Html.NameFor(m => m).ToString().Replace(".", "_").ToLower(), type = "number", pattern = "[0-9]*", min = "0", max = "31", @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                ");

WriteLiteral("\r\n            </div>\r\n            <div");

WriteLiteral(" class=\"form-group form-group-month\"");

WriteLiteral(">\r\n                <label");

WriteLiteral(" class=\"form-label-bold\"");

WriteLiteral(" for=\"example-dob-month\"");

WriteLiteral(">Month</label>\r\n");

WriteLiteral("                ");

            
            #line 38 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
           Write(Html.TextBoxFor(m => m.Month, new { pattern = "[0-9]*", min = "0", max = "12", @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                ");

WriteLiteral("\r\n            </div>\r\n            <div");

WriteLiteral(" class=\"form-group form-group-year\"");

WriteLiteral(">\r\n                <label");

WriteLiteral(" class=\"form-label-bold\"");

WriteLiteral(" for=\"example-dob-year\"");

WriteLiteral(">Year</label>\r\n");

WriteLiteral("                ");

            
            #line 43 "..\..\Views\Shared\EditorTemplates\DateViewModel.cshtml"
           Write(Html.TextBoxFor(m => m.Year, new { pattern = "[0-9]*", min = "0", @class = "form-control" }));

            
            #line default
            #line hidden
WriteLiteral("\r\n                ");

WriteLiteral("\r\n            </div>\r\n        </div>\r\n    </fieldset>\r\n</div>\r\n\r\n");

WriteLiteral("\r\n");

        }
    }
}
#pragma warning restore 1591
