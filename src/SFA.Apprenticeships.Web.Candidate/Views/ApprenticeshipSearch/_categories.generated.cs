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
    
    #line 1 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships;
    
    #line default
    #line hidden
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipSearch/_categories.cshtml")]
    public partial class categories : System.Web.Mvc.WebViewPage<ApprenticeshipSearchViewModel>
    {
        public categories()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 5 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
  
    var divClass = Model.SearchMode == ApprenticeshipSearchMode.Category ? "active" : "";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div");

WriteLiteral(" id=\"categories\"");

WriteAttribute("class", Tuple.Create(" class=\"", 232), Tuple.Create("\"", 280)
, Tuple.Create(Tuple.Create("", 240), Tuple.Create("form-group", 240), true)
, Tuple.Create(Tuple.Create(" ", 250), Tuple.Create("tabbed-element", 251), true)
, Tuple.Create(Tuple.Create(" ", 265), Tuple.Create("tab2", 266), true)
            
            #line 9 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
, Tuple.Create(Tuple.Create(" ", 270), Tuple.Create<System.Object, System.Int32>(divClass
            
            #line default
            #line hidden
, 271), false)
);

WriteLiteral(">\r\n    <span");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(" id=\"categoryLabelTour\"");

WriteLiteral(">Browse by category</span>\r\n    <div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n");

            
            #line 12 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
        
            
            #line default
            #line hidden
            
            #line 12 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
          
            if (Model.Categories != null && Model.Categories.Any())
            {
                var categories = Model.Categories.ToList();
                var categorySplitCount = categories.Count/2 + categories.Count%2;


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                        <ul");

WriteLiteral(" id=\"category-list-left\"");

WriteLiteral(" class=\"font-xsmall list-text list-checkradio\"");

WriteLiteral(">\r\n");

            
            #line 21 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                            
            
            #line default
            #line hidden
            
            #line 21 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                             for (var i = 0; i < categorySplitCount; i++)
                            {
                                var category = categories[i];
                                var inputId = string.Format("category-{0}", category.CodeName.ToLower());
                                var checkedAttr = Model.Category == category.CodeName ? "checked" : "";

            
            #line default
            #line hidden
WriteLiteral("                                <li>\r\n                                    <input " +
"");

            
            #line 27 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                                      Write(checkedAttr);

            
            #line default
            #line hidden
WriteLiteral(" type=\"radio\" name=\"Category\" id=\"");

            
            #line 27 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                                                                                    Write(inputId);

            
            #line default
            #line hidden
WriteLiteral("\" value=\"");

            
            #line 27 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                                                                                                     Write(category.CodeName);

            
            #line default
            #line hidden
WriteLiteral("\"><label");

WriteAttribute("for", Tuple.Create(" for=\"", 1385), Tuple.Create("\"", 1399)
            
            #line 27 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                                                          , Tuple.Create(Tuple.Create("", 1391), Tuple.Create<System.Object, System.Int32>(inputId
            
            #line default
            #line hidden
, 1391), false)
);

WriteLiteral(">");

            
            #line 27 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                                                                                                                                               Write(FullNameFormatter.Format(category.FullName));

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                                </li>\r\n");

            
            #line 29 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                            }

            
            #line default
            #line hidden
WriteLiteral("                        </ul>\r\n                    </div>\r\n                </div>" +
"\r\n");

            
            #line 33 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"


            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n                    <ul");

WriteLiteral(" id=\"category-list-right\"");

WriteLiteral(" class=\"font-xsmall list-text list-checkradio\"");

WriteLiteral(">\r\n");

            
            #line 36 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 36 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                         for (var i = categorySplitCount; i < categories.Count; i++)
                        {
                            var category = categories[i];
                            var inputId = string.Format("category-{0}", category.CodeName.ToLower());
                            var checkedAttr = Model.Category == category.CodeName ? "checked" : "";

            
            #line default
            #line hidden
WriteLiteral("                            <li>\r\n                                <input ");

            
            #line 42 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                                  Write(checkedAttr);

            
            #line default
            #line hidden
WriteLiteral(" type=\"radio\" name=\"Category\" id=\"");

            
            #line 42 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                                                                                Write(inputId);

            
            #line default
            #line hidden
WriteLiteral("\" value=\"");

            
            #line 42 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                                                                                                 Write(category.CodeName);

            
            #line default
            #line hidden
WriteLiteral("\"><label");

WriteAttribute("for", Tuple.Create(" for=\"", 2290), Tuple.Create("\"", 2304)
            
            #line 42 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                                                      , Tuple.Create(Tuple.Create("", 2296), Tuple.Create<System.Object, System.Int32>(inputId
            
            #line default
            #line hidden
, 2296), false)
);

WriteLiteral(">");

            
            #line 42 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                                                                                                                                           Write(FullNameFormatter.Format(category.FullName));

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                            </li>\r\n");

            
            #line 44 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </ul>\r\n                </div>\r\n");

            
            #line 47 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
            }
            else
            {

            
            #line default
            #line hidden
WriteLiteral("                <div");

WriteLiteral(" id=\"category-load-failed\"");

WriteLiteral(" class=\"field-validation-error\"");

WriteLiteral(">Category search is currently unavailable. Please try again or use the keyword se" +
"arch</div>\r\n");

            
            #line 51 "..\..\Views\ApprenticeshipSearch\_categories.cshtml"
            }
        
            
            #line default
            #line hidden
WriteLiteral("\r\n    </div>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
