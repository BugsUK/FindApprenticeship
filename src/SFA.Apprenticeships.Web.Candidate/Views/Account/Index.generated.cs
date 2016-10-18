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

namespace SFA.Apprenticeships.Web.Candidate.Views.Account
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
    
    #line 1 "..\..\Views\Account\Index.cshtml"
    using System.Web.Mvc.Html;
    
    #line default
    #line hidden
    using System.Web.Optimization;
    using System.Web.Routing;
    using System.Web.Security;
    using System.Web.UI;
    using System.Web.WebPages;
    
    #line 2 "..\..\Views\Account\Index.cshtml"
    using SFA.Apprenticeships.Domain.Entities.Applications;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Infrastructure.Presentation;
    using SFA.Apprenticeships.Web.Candidate;
    using SFA.Apprenticeships.Web.Candidate.Constants;
    using SFA.Apprenticeships.Web.Candidate.Constants.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.Helpers;
    using SFA.Apprenticeships.Web.Candidate.ViewModels;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Candidate;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Login;
    
    #line 3 "..\..\Views\Account\Index.cshtml"
    using SFA.Apprenticeships.Web.Candidate.ViewModels.MyApplications;
    
    #line default
    #line hidden
    using SFA.Apprenticeships.Web.Candidate.ViewModels.Register;
    using SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch;
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Common.ViewModels.Locations;
    using SFA.Apprenticeships.Web.Common.Views.Shared.DisplayTemplates;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/Account/Index.cshtml")]
    public partial class Index : System.Web.Mvc.WebViewPage<MyApplicationsViewModel>
    {
        public Index()
        {
        }
        public override void Execute()
        {
WriteLiteral("\r\n");

            
            #line 7 "..\..\Views\Account\Index.cshtml"
  
    ViewBag.Title = "My applications - Find an apprenticeship";
    Layout = "~/Views/Shared/_Layout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n");

            
            #line 12 "..\..\Views\Account\Index.cshtml"
 if (Model.DeletedVacancyId != null && Model.DeletedVacancyTitle != null)
{
    const string deleteMessage = "You've successfully removed the <a id='vacancyDeletedLink' href={0}>{1}</a> apprenticeship";
    var deletedMessageWithLink = string.Format(deleteMessage, Url.RouteUrl(CandidateRouteNames.ApprenticeshipDetails, new { id = Model.DeletedVacancyId }), Model.DeletedVacancyTitle);
     

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" role=\"alert\"");

WriteLiteral(" aria-live=\"assertive\"");

WriteLiteral(" class=\"panel-info\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" id=\"VacancyDeletedInfoMessageText\"");

WriteLiteral(">");

            
            #line 18 "..\..\Views\Account\Index.cshtml"
                                         Write(Html.Raw(deletedMessageWithLink));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n    </div>\r\n");

            
            #line 20 "..\..\Views\Account\Index.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 22 "..\..\Views\Account\Index.cshtml"
 if (Model.TraineeshipFeature.ShowTraineeshipsPrompt)
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" role=\"alert\"");

WriteLiteral(" aria-live=\"assertive\"");

WriteLiteral(" id=\"traineeshipPrompt\"");

WriteLiteral(" class=\"toggle-content panel-info\"");

WriteLiteral(" style=\"display: block;\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n            <p>\r\n                Have you considered a traineeship? Traineeshi" +
"ps are designed to get someone ready for an apprenticeship.\r\n            </p>\r\n " +
"           <p>\r\n                <a");

WriteLiteral(" id=\"traineeship-overview-link\"");

WriteAttribute("href", Tuple.Create(" \r\n                   href=\"", 1311), Tuple.Create("\"", 1393)
            
            #line 31 "..\..\Views\Account\Index.cshtml"
, Tuple.Create(Tuple.Create("", 1339), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.TraineeshipOverview)
            
            #line default
            #line hidden
, 1339), false)
);

WriteLiteral("\r\n                   onlick=\"Webtrends.multiTrack({ element: this, argsa: [\'DCS.d" +
"csuri\', \'/traineeships/promptaccept\', \'WT.dl\', \'99\', \'WT.ti\', \'Traineeship Promp" +
"t Accepted\'] })\"");

WriteLiteral(">More about traineeships</a>\r\n            </p>\r\n            <p>\r\n                " +
"<a");

WriteLiteral(" id=\"dismiss-traineeship-prompts-link\"");

WriteLiteral(" \r\n                   class=\"notInterested link-unimp icon-black\"");

WriteAttribute("href", Tuple.Create(" \r\n                   href=\"", 1757), Tuple.Create("\"", 1845)
            
            #line 37 "..\..\Views\Account\Index.cshtml"
, Tuple.Create(Tuple.Create("", 1785), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.DismissTraineeshipPrompts)
            
            #line default
            #line hidden
, 1785), false)
);

WriteLiteral("\r\n                   onclick=\"Webtrends.multiTrack({ element: this, argsa: [\'DCS." +
"dcsuri\', \'/traineeships/promptdecline\', \'WT.dl\', \'99\', \'WT.ti\', \'Traineeship Pro" +
"mpt Declined\'] })\"");

WriteLiteral("><i");

WriteLiteral(" class=\"copy-16 fa fa-times-circle\"");

WriteLiteral("></i>Don\'t show me this again</a>\r\n            </p>\r\n        </div>\r\n    </div>\r\n" +
"");

            
            #line 42 "..\..\Views\Account\Index.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n");

            
            #line 44 "..\..\Views\Account\Index.cshtml"
 if (Model.ApplicationStatusNotifications.Any())
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" role=\"alert\"");

WriteLiteral(" aria-live=\"assertive\"");

WriteLiteral(" id=\"statusPrompt\"");

WriteLiteral(" class=\"toggle-content panel-info\"");

WriteLiteral(" style=\"display: block;\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n");

            
            #line 48 "..\..\Views\Account\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 48 "..\..\Views\Account\Index.cshtml"
             foreach (var applicationNotification in Model.ApplicationStatusNotifications)
            {
                var successOrUnsuccessful = applicationNotification.ApplicationStatus == ApplicationStatuses.Successful ? "successful" : "unsuccessful";

            
            #line default
            #line hidden
WriteLiteral("                <p>\r\n                    Your application for ");

            
            #line 52 "..\..\Views\Account\Index.cshtml"
                                    Write(applicationNotification.Title);

            
            #line default
            #line hidden
WriteLiteral(" has been ");

            
            #line 52 "..\..\Views\Account\Index.cshtml"
                                                                            Write(successOrUnsuccessful);

            
            #line default
            #line hidden
WriteLiteral(".\r\n");

            
            #line 53 "..\..\Views\Account\Index.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 53 "..\..\Views\Account\Index.cshtml"
                     if (applicationNotification.UnsuccessfulReason != null)
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <a>Read feedback</a>\r\n");

            
            #line 56 "..\..\Views\Account\Index.cshtml"
                    }                        

            
            #line default
            #line hidden
WriteLiteral("                </p>\r\n");

            
            #line 58 "..\..\Views\Account\Index.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("            <p>\r\n                <a");

WriteAttribute("href", Tuple.Create(" href=\"", 3006), Tuple.Create("\"", 3189)
            
            #line 60 "..\..\Views\Account\Index.cshtml"
, Tuple.Create(Tuple.Create("", 3013), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.DismissApplicationNotifications, 
                        new { Lastupdated = Model.ApplicationStatusNotificationsLastUpdatedDateTimeTicks })
            
            #line default
            #line hidden
, 3013), false)
);

WriteLiteral(" class=\"notInterested link-unimp icon-black\"");

WriteLiteral(" id=\"\"");

WriteLiteral("><fa");

WriteLiteral(" class=\"fa fa-times-circle\"");

WriteLiteral("></fa>Dismiss this message</a>\r\n            </p>\r\n        </div>\r\n    </div>\r\n");

            
            #line 65 "..\..\Views\Account\Index.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n        <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(">My applications</h1>\r\n    </div>\r\n    <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"page-link\"");

WriteLiteral(">\r\n");

            
            #line 73 "..\..\Views\Account\Index.cshtml"
            
            
            #line default
            #line hidden
            
            #line 73 "..\..\Views\Account\Index.cshtml"
             if (Model.AllApprenticeshipApplications.Any() || Model.TraineeshipApplications.Any())
            {

            
            #line default
            #line hidden
WriteLiteral("                <ul");

WriteLiteral(" class=\"list-text small-btm-margin\"");

WriteLiteral(">\r\n                    <li>");

            
            #line 76 "..\..\Views\Account\Index.cshtml"
                   Write(Html.RouteLink("Find an apprenticeship", CandidateRouteNames.ApprenticeshipSearch, null, new {id = "find-apprenticeship-link", @class = "link-unimp"}));

            
            #line default
            #line hidden
WriteLiteral("</li>\r\n                </ul>\r\n");

            
            #line 78 "..\..\Views\Account\Index.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("            ");

            
            #line 79 "..\..\Views\Account\Index.cshtml"
             if (Model.TraineeshipFeature.ShowTraineeshipsLink)
            {

            
            #line default
            #line hidden
WriteLiteral("                <ul");

WriteLiteral(" class=\"list-text\"");

WriteLiteral(">\r\n                    <li>\r\n");

WriteLiteral("                        ");

            
            #line 83 "..\..\Views\Account\Index.cshtml"
                   Write(Html.RouteLink("Find a traineeship", CandidateRouteNames.TraineeshipSearch, null, new {id = "find-traineeship-link", @class = "link-unimp"}));

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </li>\r\n                </ul>\r\n");

            
            #line 86 "..\..\Views\Account\Index.cshtml"
            }

            
            #line default
            #line hidden
WriteLiteral("        </div>\r\n    </div>\r\n</div>\r\n\r\n");

            
            #line 91 "..\..\Views\Account\Index.cshtml"
Write(Html.DisplayFor(m => m, MyApplicationsViewModel.PartialView));

            
            #line default
            #line hidden
        }
    }
}
#pragma warning restore 1591
