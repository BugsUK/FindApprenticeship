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

namespace SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates.Application
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
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Raa.Common;
    
    #line 1 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/Application/WorkExperience.cshtml")]
    public partial class WorkExperience : System.Web.Mvc.WebViewPage<IEnumerable<WorkExperienceViewModel>>
    {
        public WorkExperience()
        {
        }
        public override void Execute()
        {
WriteLiteral("<section");

WriteLiteral(" class=\"section-border\"");

WriteLiteral(">\r\n    <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">\r\n        Work experience\r\n    </h2>\r\n\r\n");

            
            #line 9 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
    
            
            #line default
            #line hidden
            
            #line 9 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
     if (!Model.Any())
    {

            
            #line default
            #line hidden
WriteLiteral("        <p");

WriteLiteral(" id=\"no-work-experience\"");

WriteLiteral(">Applicant doesn\'t have any work experience</p>\r\n");

            
            #line 12 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 14 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
    
            
            #line default
            #line hidden
            
            #line 14 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
     foreach (WorkExperienceViewModel experience in Model)
    {

            
            #line default
            #line hidden
WriteLiteral("        <div");

WriteLiteral(" class=\"grid-3-4 nobreak-print\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"grid-wrapper work-history-item\"");

WriteLiteral(">\r\n\r\n                <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n                    <table");

WriteLiteral(" class=\"table-no-btm-border table-compound\"");

WriteLiteral(@">
                        <colgroup>
                            <col>
                            <col>
                        </colgroup>
                        <thead>
                            <tr>
                                <th>
                                    <span");

WriteLiteral(" class=\"heading-span\"");

WriteLiteral(@">Work experience</span>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <span");

WriteLiteral(" class=\"form-prepopped cell-span\"");

WriteLiteral(">");

            
            #line 35 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
                                                                      Write(experience.Employer);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                                    <span");

WriteLiteral(" class=\"form-prepopped cell-span work-hyphen\"");

WriteLiteral(">-</span>\r\n                                    <span");

WriteLiteral(" class=\"form-prepopped cell-span\"");

WriteLiteral(">");

            
            #line 37 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
                                                                      Write(experience.JobTitle);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                                    <div></div>\r\n                       " +
"             <span");

WriteLiteral(" class=\"form-prepopped cell-span prewrap\"");

WriteLiteral(">");

            
            #line 39 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
                                                                              Write(experience.Description);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                                </td>\r\n                            </tr>" +
"\r\n                        </tbody>\r\n                    </table>\r\n              " +
"  </div>\r\n                <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n                    <table");

WriteLiteral(" class=\"table-no-btm-border table-compound\"");

WriteLiteral(">\r\n                        <colgroup>\r\n                            <col");

WriteLiteral(" class=\"t30\"");

WriteLiteral(">\r\n                            <col");

WriteLiteral(" class=\"t30\"");

WriteLiteral(">\r\n                            <col");

WriteLiteral(" class=\"t25\"");

WriteLiteral(">\r\n                            <col");

WriteLiteral(" class=\"t15\"");

WriteLiteral(">\r\n                            <col>\r\n                        </colgroup>\r\n      " +
"                  <thead>\r\n                            <tr>\r\n                   " +
"             <th>\r\n                                    <span");

WriteLiteral(" class=\"heading-span\"");

WriteLiteral(">From</span>\r\n                                </th>\r\n                            " +
"    <th>\r\n                                    <span");

WriteLiteral(" class=\"heading-span\"");

WriteLiteral(@">To</span>
                                </th>
                                <th></th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <span");

WriteLiteral(" class=\"form-prepopped cell-span\"");

WriteLiteral(">");

            
            #line 69 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
                                                                      Write(experience.FromDate.GetMonthYearLabel());

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n                                </td>\r\n                                <" +
"td>\r\n                                    <span");

WriteLiteral(" class=\"form-prepopped cell-span\"");

WriteLiteral(">");

            
            #line 72 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
                                                                      Write(experience.ToDate.GetMonthYearLabel());

            
            #line default
            #line hidden
WriteLiteral(@"</span>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
");

            
            #line 82 "..\..\Views\Shared\DisplayTemplates\Application\WorkExperience.cshtml"
    }

            
            #line default
            #line hidden
WriteLiteral("\r\n</section>");

        }
    }
}
#pragma warning restore 1591