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
    
    #line 1 "..\..\Views\Shared\DisplayTemplates\Application\DisabilityStatusQuestion.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Candidates;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Infrastructure.Presentation;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Raa.Common;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/Application/DisabilityStatusQuestion.cshtml")]
    public partial class DisabilityStatusQuestion : System.Web.Mvc.WebViewPage<DisabilityStatus?>
    {
        public DisabilityStatusQuestion()
        {
        }
        public override void Execute()
        {
            
            #line 4 "..\..\Views\Shared\DisplayTemplates\Application\DisabilityStatusQuestion.cshtml"
 if (Model.HasValue && Model.Value != DisabilityStatus.Unknown)
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" class=\"form-label\"");

WriteLiteral(" id=\"disability-label\"");

WriteLiteral(">Do you have a learning difficulty, disability or health problem?</p>\r\n");

            
            #line 8 "..\..\Views\Shared\DisplayTemplates\Application\DisabilityStatusQuestion.cshtml"
        
            
            #line default
            #line hidden
            
            #line 8 "..\..\Views\Shared\DisplayTemplates\Application\DisabilityStatusQuestion.cshtml"
         if (Model.Value == DisabilityStatus.PreferNotToSay)
        {

            
            #line default
            #line hidden
WriteLiteral("            <span");

WriteLiteral(" class=\"form-prepopped\"");

WriteLiteral(">Prefer not to say</span>\r\n");

            
            #line 11 "..\..\Views\Shared\DisplayTemplates\Application\DisabilityStatusQuestion.cshtml"
        }
        else
        {

            
            #line default
            #line hidden
WriteLiteral("            <span");

WriteLiteral(" class=\"form-prepopped\"");

WriteLiteral(">");

            
            #line 14 "..\..\Views\Shared\DisplayTemplates\Application\DisabilityStatusQuestion.cshtml"
                                    Write(Model.Value);

            
            #line default
            #line hidden
WriteLiteral("</span>\r\n");

            
            #line 15 "..\..\Views\Shared\DisplayTemplates\Application\DisabilityStatusQuestion.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");

            
            #line 17 "..\..\Views\Shared\DisplayTemplates\Application\DisabilityStatusQuestion.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
