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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipSearch/_categoriesAndSubCategories.cshtml")]
    public partial class categoriesAndSubCategories : System.Web.Mvc.WebViewPage<ApprenticeshipSearchViewModel>
    {
        public categoriesAndSubCategories()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
  
    //TODO: create style for tabbed-element that defaults to hidden like tabbed-content. Remove inline style
    var divClass = "class=\"form-group tabbed-element tab2\"";
    if (Model.SearchMode != ApprenticeshipSearchMode.Category)
    {
        divClass += " style=\"display: none\"";
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n<div ");

            
            #line 11 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
Write(Html.Raw(divClass));

            
            #line default
            #line hidden
WriteLiteral(">\r\n    <ul");

WriteLiteral(" class=\"copy-16 list-text list-checkradio\"");

WriteLiteral(">\r\n");

            
            #line 13 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
        
            
            #line default
            #line hidden
            
            #line 13 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
         foreach (var category in Model.Categories)
        {
            var inputId = string.Format("category{0}", category.CodeName);
            var subCategories = category.SubCategories.ToList();
            var checkedAttr = Model.Category == category.CodeName ? " checked" : "";
            var openAttr = Model.Category == category.CodeName && Model.SubCategories != null && Model.SubCategories.Any() ? "open" : "";

            
            #line default
            #line hidden
WriteLiteral("            <li>\r\n                <input ");

            
            #line 20 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                  Write(checkedAttr);

            
            #line default
            #line hidden
WriteLiteral(" type=\"radio\" name=\"Category\" id=\"");

            
            #line 20 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                                                                Write(inputId);

            
            #line default
            #line hidden
WriteLiteral("\" value=\"");

            
            #line 20 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                                                                                 Write(category.CodeName);

            
            #line default
            #line hidden
WriteLiteral("\"><label");

WriteAttribute("for", Tuple.Create(" for=\"", 988), Tuple.Create("\"", 1002)
            
            #line 20 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                                       , Tuple.Create(Tuple.Create("", 994), Tuple.Create<System.Object, System.Int32>(inputId
            
            #line default
            #line hidden
, 994), false)
);

WriteLiteral(">");

            
            #line 20 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                                                                                                                           Write(category.FullName);

            
            #line default
            #line hidden
WriteLiteral("</label>\r\n                <details ");

            
            #line 21 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                    Write(openAttr);

            
            #line default
            #line hidden
WriteLiteral(">\r\n                    <summary");

WriteLiteral(" class=\"copy-16\"");

WriteLiteral(">");

            
            #line 22 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                                        Write(subCategories.Count);

            
            #line default
            #line hidden
WriteLiteral(" more sub-categories</summary>\r\n                    <ul");

WriteLiteral(" class=\"copy-16 list-text list-checkradio\"");

WriteLiteral(">\r\n");

            
            #line 24 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 24 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                         foreach (var subCategory in subCategories)
                        {
                            checkedAttr = Model.SubCategories != null && Model.SubCategories.Contains(subCategory.CodeName) ? " checked" : "";
                            var checkboxId = string.Format("subCategory{0}", subCategory.CodeName);
                            var labelText = subCategory.FullName;
                            if (subCategory.Count.HasValue)
                            {
                                labelText = string.Format("{0} ({1})", labelText, subCategory.Count.Value);
                            }

            
            #line default
            #line hidden
WriteLiteral("                            <li><input ");

            
            #line 33 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                                  Write(checkedAttr);

            
            #line default
            #line hidden
WriteLiteral(" type=\"checkbox\" name=\"SubCategories\" id=\"");

            
            #line 33 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                                                                                        Write(checkboxId);

            
            #line default
            #line hidden
WriteLiteral("\" value=\"");

            
            #line 33 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                                                                                                            Write(subCategory.CodeName);

            
            #line default
            #line hidden
WriteLiteral("\"><label");

WriteAttribute("for", Tuple.Create(" for=\"", 2016), Tuple.Create("\"", 2033)
            
            #line 33 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                                                                    , Tuple.Create(Tuple.Create("", 2022), Tuple.Create<System.Object, System.Int32>(checkboxId
            
            #line default
            #line hidden
, 2022), false)
);

WriteLiteral(">");

            
            #line 33 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                                                                                                                                                            Write(labelText);

            
            #line default
            #line hidden
WriteLiteral("</label></li>\r\n");

            
            #line 34 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                    </ul>\r\n                </details>\r\n            </li>\r\n");

            
            #line 38 "..\..\Views\ApprenticeshipSearch\_categoriesAndSubCategories.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </ul>\r\n</div>");

        }
    }
}
#pragma warning restore 1591
