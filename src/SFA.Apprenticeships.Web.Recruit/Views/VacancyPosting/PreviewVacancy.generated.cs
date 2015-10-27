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

namespace SFA.Apprenticeships.Web.Recruit.Views.VacancyPosting
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
    using SFA.Apprenticeships.Web.Common.Constants;
    using SFA.Apprenticeships.Web.Common.Framework;
    using SFA.Apprenticeships.Web.Common.Models.Common;
    using SFA.Apprenticeships.Web.Recruit;
    
    #line 2 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
    using SFA.Apprenticeships.Web.Recruit.Constants;
    
    #line default
    #line hidden
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/VacancyPosting/PreviewVacancy.cshtml")]
    public partial class PreviewVacancy : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Recruit.ViewModels.Vacancy.VacancyViewModel>
    {
        public PreviewVacancy()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
  
    ViewBag.Title = "Recruit an Apprentice - Preview vacancy";

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n<div>\r\n    <div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"hgroup text\"");

WriteLiteral(">\r\n                <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(" id=\"vacancy-title\"");

WriteLiteral(" itemprop=\"title\"");

WriteLiteral(">");

            
            #line 11 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                          Write(Model.Title);

            
            #line default
            #line hidden
WriteLiteral(" (Preview)</h1>\r\n                <p");

WriteLiteral(" class=\"subtitle\"");

WriteLiteral(" id=\"vacancy-subtitle-employer-name\"");

WriteLiteral(" itemprop=\"hiringOrganization\"");

WriteLiteral(">");

            
            #line 12 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                                                 Write(Model.ProviderSiteEmployerLink.Employer.Name);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n            </div>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n            <p");

WriteLiteral(" class=\"page-link hide-nojs\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" class=\"print-trigger\"");

WriteLiteral(" href=\"\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-print\"");

WriteLiteral("></i>Print this page</a>\r\n            </p>\r\n        </div>\r\n    </div>\r\n\r\n       " +
" <section");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(" itemprop=\"description\"");

WriteLiteral(">\r\n                        <p");

WriteLiteral(" id=\"vacancy-description\"");

WriteLiteral(">");

            
            #line 26 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                               Write(Model.ShortDescription);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n   " +
"         <div");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n                <p");

WriteLiteral(" id=\"vacancy-closing-date\"");

WriteLiteral(" class=\"copy-16\"");

WriteLiteral(">Closing date: ");

            
            #line 31 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                      Write(Model.ClosingDate.Value.ToFriendlyClosingToday());

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n            </div>\r\n        </section>\r\n        <section");

WriteLiteral(" class=\"section-border grid-wrapper\"");

WriteLiteral(" id=\"vacancy-info\"");

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Apprenticeship summary</h2>\r\n            <div");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Weekly wage</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-wage\"");

WriteLiteral(">");

            
            #line 39 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                    Write(Model.WeeklyWage);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Working week</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-working-week\"");

WriteLiteral(" itemprop=\"workHours\"");

WriteLiteral(">");

            
            #line 42 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                 Write(Model.WorkingWeek);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Apprenticeship duration</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-expected-duration\"");

WriteLiteral(">");

            
            #line 44 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                  Write(string.IsNullOrWhiteSpace(@Model.Duration) ? "Not specified" : @Model.Duration);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Possible start date</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-start-date\"");

WriteLiteral(">");

            
            #line 46 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                          Write(Html.DisplayFor(m => Model.PossibleStartDate));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Apprenticeship level</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-type\"");

WriteLiteral(" itemprop=\"employmentType\"");

WriteLiteral(">");

            
            #line 48 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                              Write(Model.ApprenticeshipLevel);

            
            #line default
            #line hidden
WriteLiteral(" Level Apprenticeship</p>\r\n\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Reference number</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-reference-id\"");

WriteLiteral(">");

            
            #line 51 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                            Write(Model.VacancyReferenceNumber);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                </div>\r\n            </div>\r\n            <div");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(" itemprop=\"responsibilities\"");

WriteLiteral(">\r\n                    <p");

WriteLiteral(" id=\"vacancy-full-descrpition\"");

WriteLiteral(">");

            
            #line 56 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                Write(Html.Raw(Model.LongDescription));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                </div>\r\n            </div>\r\n\r\n        </section>\r\n        <" +
"section");

WriteLiteral(" class=\"section-border grid-wrapper\"");

WriteLiteral(" id=\"course-info\"");

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Requirements and prospects</h2>\r\n            <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n");

            
            #line 66 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 66 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                         if (!string.IsNullOrWhiteSpace(Model.DesiredSkills))
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Desired skills</h3>\r\n");

WriteLiteral("                            <p");

WriteLiteral(" id=\"vacancy-skills-required\"");

WriteLiteral(" itemprop=\"skills\"");

WriteLiteral(">");

            
            #line 69 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                         Write(Html.Raw(Model.DesiredSkills));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 70 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                        ");

            
            #line 71 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                         if (!string.IsNullOrWhiteSpace(Model.PersonalQualities))
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Personal qualities</h3>\r\n");

WriteLiteral("                            <p");

WriteLiteral(" id=\"vacancy-qualities-required\"");

WriteLiteral(" itemprop=\"qualities\"");

WriteLiteral(">");

            
            #line 74 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                               Write(Html.Raw(Model.PersonalQualities));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 75 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                        ");

            
            #line 76 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                         if (!string.IsNullOrWhiteSpace(Model.DesiredQualifications))
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Qualifications required</h3>\r\n");

WriteLiteral("                            <p");

WriteLiteral(" id=\"vacancy-qualifications-required\"");

WriteLiteral(" itemprop=\"qualifications\"");

WriteLiteral(">");

            
            #line 79 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                                         Write(Html.Raw(Model.DesiredQualifications));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 80 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </div>\r\n                </div>\r\n            </div>\r\n       " +
"     <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n");

            
            #line 87 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                    
            
            #line default
            #line hidden
            
            #line 87 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                     if (!string.IsNullOrWhiteSpace(Model.FutureProspects))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Future prospects</h3>\r\n");

WriteLiteral("                        <p");

WriteLiteral(" id=\"vacancy-future-prospects\"");

WriteLiteral(" itemprop=\"incentives\"");

WriteLiteral(">");

            
            #line 90 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                          Write(Html.Raw(Model.FutureProspects));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 91 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                    ");

            
            #line 92 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                     if (!string.IsNullOrWhiteSpace(Model.ThingsToConsider))
                    {

            
            #line default
            #line hidden
WriteLiteral("                        <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Things to consider</h3>\r\n");

WriteLiteral("                        <p");

WriteLiteral(" id=\"vacancy-reality-check\"");

WriteLiteral(" itemprop=\"incentives\"");

WriteLiteral(">");

            
            #line 95 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                       Write(Html.Raw(Model.ThingsToConsider));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 96 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                    }

            
            #line default
            #line hidden
WriteLiteral("                </div>\r\n            </div>\r\n        </section>\r\n        <section");

WriteLiteral(" class=\"section-border\"");

WriteLiteral(" id=\"employer-info\"");

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">About the employer</h2>\r\n            <div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n                            <p");

WriteLiteral(" id=\"vacancy-employer-description\"");

WriteLiteral(">");

            
            #line 106 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                            Write(Html.Raw(Model.ProviderSiteEmployerLink.Description));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                        </div>\r\n                    </div>\r\n               " +
" </div>\r\n            </div>\r\n            <div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"grid grid-1-2\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n                            <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Employer</h3>\r\n                            <p");

WriteLiteral(" id=\"vacancy-employer-name\"");

WriteAttribute("class", Tuple.Create(" class=\"", 5912), Tuple.Create("\"", 6009)
            
            #line 116 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 5920), Tuple.Create<System.Object, System.Int32>(string.IsNullOrEmpty(Model.ProviderSiteEmployerLink.WebsiteUrl) ? "no-btm-margin" : ""
            
            #line default
            #line hidden
, 5920), false)
);

WriteLiteral(">");

            
            #line 116 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                                                                                                       Write(Model.ProviderSiteEmployerLink.Employer.Name);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 117 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                            
            
            #line default
            #line hidden
            
            #line 117 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                             if (string.IsNullOrEmpty(Model.ProviderSiteEmployerLink.WebsiteUrl))
                            {

            
            #line default
            #line hidden
WriteLiteral("                                <p>\r\n                                    <a");

WriteLiteral(" itemprop=\"url\"");

WriteAttribute("href", Tuple.Create(" href=\"", 6282), Tuple.Create("\"", 6331)
            
            #line 120 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 6289), Tuple.Create<System.Object, System.Int32>(Model.ProviderSiteEmployerLink.WebsiteUrl
            
            #line default
            #line hidden
, 6289), false)
);

WriteLiteral("\r\n                                       id=\"vacancy-employer-website\"");

WriteLiteral("\r\n                                       target=\"_blank\"");

WriteAttribute("title", Tuple.Create("\r\n                                       title=\"", 6458), Tuple.Create("\"", 6559)
            
            #line 123 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 6506), Tuple.Create<System.Object, System.Int32>(Model.ProviderSiteEmployerLink.Employer.Name
            
            #line default
            #line hidden
, 6506), false)
, Tuple.Create(Tuple.Create(" ", 6551), Tuple.Create("Website", 6552), true)
);

WriteLiteral(" rel=\"external\"");

WriteLiteral(">");

            
            #line 123 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                                                               Write(Model.ProviderSiteEmployerLink.WebsiteUrl);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n                                </p>\r\n");

            
            #line 125 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                            }

            
            #line default
            #line hidden
WriteLiteral("                            <div");

WriteLiteral(" id=\"vacancy-address\"");

WriteLiteral(" itemscope");

WriteLiteral(" itemtype=\"http://schema.org/PostalAddress\"");

WriteLiteral(">\r\n                                <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Address</h3>\r\n                                <div");

WriteLiteral(" itemprop=\"address\"");

WriteLiteral(">\r\n                                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(" itemprop=\"streetAddress\"");

WriteLiteral(">");

            
            #line 129 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                                    Write(Model.ProviderSiteEmployerLink.Employer.Address.AddressLine1);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(" itemprop=\"streetAddress\"");

WriteLiteral(">");

            
            #line 130 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                                    Write(Model.ProviderSiteEmployerLink.Employer.Address.AddressLine2);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(" itemprop=\"addressLocality\"");

WriteLiteral(">");

            
            #line 131 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                                      Write(Model.ProviderSiteEmployerLink.Employer.Address.AddressLine3);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(" itemprop=\"addressRegion\"");

WriteLiteral(">");

            
            #line 132 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                                    Write(Model.ProviderSiteEmployerLink.Employer.Address.AddressLine4);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                    <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(" itemprop=\"postalCode\"");

WriteLiteral(">");

            
            #line 133 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                                 Write(Model.ProviderSiteEmployerLink.Employer.Address.Postcode);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                                </div>\r\n                            </div>\r" +
"\n                        </div>\r\n                    </div>\r\n                </d" +
"iv>\r\n                <div");

WriteLiteral(" class=\"grid grid-1-2 hide-print\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" id=\"vacancy-map\"");

WriteLiteral(" class=\"ad-details__map\"");

WriteLiteral(">\r\n                        <div");

WriteLiteral(" class=\"map-overlay\"");

WriteLiteral(" onclick=\" style.pointerEvents = \'none\' \"");

WriteLiteral("></div>\r\n                        <iframe");

WriteLiteral(" width=\"700\"");

WriteLiteral(" height=\"250\"");

WriteLiteral(" title=\"Map of location\"");

WriteLiteral(" style=\"border: 0\"");

WriteAttribute("src", Tuple.Create(" src=\"", 8184), Tuple.Create("\"", 8366)
, Tuple.Create(Tuple.Create("", 8190), Tuple.Create("https://www.google.com/maps/embed/v1/place?q=", 8190), true)
            
            #line 142 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                     , Tuple.Create(Tuple.Create("", 8235), Tuple.Create<System.Object, System.Int32>(Html.Raw(Model.ProviderSiteEmployerLink.Employer.Address.Postcode)
            
            #line default
            #line hidden
, 8235), false)
, Tuple.Create(Tuple.Create("", 8302), Tuple.Create(",+United+Kingdom&amp;key=AIzaSyCusA_0x4bJEjU-_gLOFiXMSBXKZYtvHz8", 8302), true)
);

WriteLiteral("></iframe>\r\n                        <p");

WriteLiteral(" class=\"nojs-notice\"");

WriteLiteral(">You must have JavaScript enabled to view a map of the location</p>\r\n            " +
"        </div>\r\n                </div>\r\n            </div>\r\n        </section>\r\n" +
"\r\n");

            
            #line 149 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
        
            
            #line default
            #line hidden
            
            #line 149 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
         if (Model.OfflineVacancy)
        {

            
            #line default
            #line hidden
WriteLiteral("            <section");

WriteLiteral(" class=\"section-border\"");

WriteLiteral(" id=\"offline-vacancy\"");

WriteLiteral(" style=\"\"");

WriteLiteral(">\r\n                <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Employer\'s application instructions</h2>\r\n                <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n                    <p");

WriteLiteral(" id=\"application-instructions\"");

WriteLiteral(">\r\n");

WriteLiteral("                        ");

            
            #line 155 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                   Write(Model.OfflineApplicationInstructions);

            
            #line default
            #line hidden
WriteLiteral("\r\n                    </p>\r\n                </div>\r\n                <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n                    <p");

WriteLiteral(" class=\"no-btm-margin\"");

WriteLiteral(">This apprenticeship requires you to apply through the employer\'s website.</p>\r\n " +
"                   <a");

WriteLiteral(" id=\"external-employer-website\"");

WriteLiteral(" rel=\"external\"");

WriteAttribute("href", Tuple.Create(" href=\"", 9233), Tuple.Create("\"", 9268)
            
            #line 160 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 9240), Tuple.Create<System.Object, System.Int32>(Model.OfflineApplicationUrl
            
            #line default
            #line hidden
, 9240), false)
);

WriteLiteral(" target=\"_blank\"");

WriteLiteral(">");

            
            #line 160 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                                                                                    Write(Model.OfflineApplicationUrl);

            
            #line default
            #line hidden
WriteLiteral("</a>\r\n                    <i");

WriteLiteral(" class=\"fa fa-check the-icon\"");

WriteLiteral(" style=\"color: green;\"");

WriteLiteral("></i>\r\n                </div>\r\n            </section>\r\n");

            
            #line 164 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("        \r\n        <section");

WriteLiteral(" class=\"section-border grid-wrapper\"");

WriteLiteral(" id=\"provider-info\"");

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral(">Training provider</h2>\r\n            <div");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"inner-block-padr\"");

WriteLiteral(">\r\n                    <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n");

            
            #line 171 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                        
            
            #line default
            #line hidden
            
            #line 171 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                         if (!string.IsNullOrWhiteSpace(Model.ProviderSite.CandidateDescription))
                        {

            
            #line default
            #line hidden
WriteLiteral("                            <p");

WriteLiteral(" id=\"vacancy-training-to-be-provided\"");

WriteLiteral(">");

            
            #line 173 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                               Write(Html.Raw(Model.ProviderSite.CandidateDescription));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n");

            
            #line 174 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                        }

            
            #line default
            #line hidden
WriteLiteral("                        <h3");

WriteLiteral(" class=\"heading-small\"");

WriteLiteral(">Apprenticeship framework</h3>\r\n                        <p");

WriteLiteral(" id=\"vacancy-framework\"");

WriteLiteral(">");

            
            #line 176 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                             Write(Html.Raw(Model.FrameworkName));

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                        <p");

WriteLiteral(" id=\"vacancy-provider-sector-pass-rate\"");

WriteLiteral(">The training provider does not yet have a sector success rate for this type of a" +
"pprenticeship training.</p>\r\n                    </div>\r\n                </div>\r" +
"\n            </div>\r\n            <div");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n                <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n                    <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Training provider</h3>\r\n                    <p");

WriteLiteral(" id=\"vacancy-provider-name\"");

WriteLiteral(">");

            
            #line 184 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                             Write(Model.ProviderSite.Name);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                </div>\r\n\r\n");

            
            #line 187 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                
            
            #line default
            #line hidden
            
            #line 187 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                 if (!string.IsNullOrWhiteSpace(Model.ProviderSite.ContactDetailsForCandidate))
                {

            
            #line default
            #line hidden
WriteLiteral("                    <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n                        <h3");

WriteLiteral(" class=\"bold-small\"");

WriteLiteral(">Contact</h3>\r\n                        <p");

WriteLiteral(" id=\"vacancy-provider-contact\"");

WriteLiteral(">");

            
            #line 191 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                    Write(Model.ProviderSite.ContactDetailsForCandidate);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n                    </div>\r\n");

            
            #line 193 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                }

            
            #line default
            #line hidden
WriteLiteral("            </div>\r\n\r\n        </section>\r\n\r\n");

            
            #line 198 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
 using (Html.BeginRouteForm(RecruitmentRouteNames.VacancySubmitted, FormMethod.Get))
{
    
            
            #line default
            #line hidden
            
            #line 200 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
Write(Html.HiddenFor(m => m.VacancyReferenceNumber));

            
            #line default
            #line hidden
            
            #line 200 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
                                                  


            
            #line default
            #line hidden
WriteLiteral("    <section>\r\n        <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n            <button");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" class=\"button\"");

WriteLiteral(">Submit for approval</button>\r\n            <a");

WriteLiteral(" id=\"dashboardLink\"");

WriteAttribute("href", Tuple.Create(" href=\"", 11396), Tuple.Create("\"", 11455)
            
            #line 205 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
, Tuple.Create(Tuple.Create("", 11403), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(RecruitmentRouteNames.RecruitmentHome)
            
            #line default
            #line hidden
, 11403), false)
);

WriteLiteral(">Return to dashboard</a>\r\n        </div>\r\n    </section>\r\n");

            
            #line 208 "..\..\Views\VacancyPosting\PreviewVacancy.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("</div>");

        }
    }
}
#pragma warning restore 1591
