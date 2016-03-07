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

namespace SFA.Apprenticeships.Web.Raa.Common.Views.Shared.DisplayTemplates.Vacancy
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
    
    #line 1 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.Extensions;
    
    #line default
    #line hidden
    
    #line 2 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
    using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Shared/DisplayTemplates/Vacancy/_OfflineVacancy.cshtml")]
    public partial class OfflineVacancy : System.Web.Mvc.WebViewPage<VacancyViewModel>
    {
        public OfflineVacancy()
        {
        }
        public override void Execute()
        {
            
            #line 5 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
  
    var editableItemClass = ViewData["editableItemClass"];

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 9 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
 if (Model.NewVacancyViewModel.OfflineVacancy.Value)
{

            
            #line default
            #line hidden
WriteLiteral("    <section");

WriteLiteral(" class=\"section-border\"");

WriteLiteral(" id=\"offline-vacancy\"");

WriteLiteral(" style=\"\"");

WriteLiteral(">\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 349), Tuple.Create("\"", 375)
            
            #line 12 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 357), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 357), false)
);

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">\r\n                Employer\'s application instructions\r\n");

WriteLiteral("                ");

            
            #line 15 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
           Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.NewVacancyViewModel.OfflineApplicationInstructions, Model.NewVacancyViewModel.OfflineApplicationInstructionsComment, Model.BasicDetailsLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n            </h2>\r\n            <p");

WriteLiteral(" id=\"application-instructions\"");

WriteLiteral(" class=\"preserve-formatting\"");

WriteLiteral(">");

            
            #line 17 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
                                                                    Write(Model.NewVacancyViewModel.OfflineApplicationInstructions);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

WriteLiteral("            ");

            
            #line 18 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
       Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.NewVacancyViewModel.OfflineApplicationInstructions, Model.BasicDetailsLink, Model.NewVacancyViewModel.OfflineApplicationInstructionsComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n        <div");

WriteAttribute("class", Tuple.Create(" class=\"", 1141), Tuple.Create("\"", 1167)
            
            #line 20 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 1149), Tuple.Create<System.Object, System.Int32>(editableItemClass
            
            #line default
            #line hidden
, 1149), false)
);

WriteLiteral(">\r\n            <p");

WriteLiteral(" class=\"no-btm-margin\"");

WriteLiteral(">This apprenticeship requires you to apply through the employer\'s website.</p>\r\n " +
"           <a");

WriteLiteral(" id=\"external-employer-website\"");

WriteLiteral(" rel=\"external\"");

WriteAttribute("href", Tuple.Create(" href=\"", 1347), Tuple.Create("\"", 1402)
            
            #line 22 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 1354), Tuple.Create<System.Object, System.Int32>(Model.NewVacancyViewModel.OfflineApplicationUrl
            
            #line default
            #line hidden
, 1354), false)
);

WriteLiteral(" target=\"_blank\"");

WriteLiteral(">");

            
            #line 22 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
                                                                                                                                Write(Model.NewVacancyViewModel.OfflineApplicationUrl);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n");

WriteLiteral("            ");

            
            #line 23 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
       Write(Html.Partial(CommentViewModel.PartialIconView, Html.GetCommentViewModel(Model, m => m.NewVacancyViewModel.OfflineApplicationUrl, Model.NewVacancyViewModel.OfflineApplicationUrlComment, Model.BasicDetailsLink)));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("            ");

            
            #line 24 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
       Write(Html.Partial(EditLinkViewModel.PartialView, Html.GetEditLinkViewModel(Model, m => m.NewVacancyViewModel.OfflineApplicationUrl, Model.BasicDetailsLink, Model.NewVacancyViewModel.OfflineApplicationUrlComment)));

            
            #line default
            #line hidden
WriteLiteral("\r\n        </div>\r\n    </section>\r\n");

            
            #line 27 "..\..\Views\Shared\DisplayTemplates\Vacancy\_OfflineVacancy.cshtml"
}
            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
