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

namespace SFA.Apprenticeships.Web.Candidate.Views.ApprenticeshipApplication
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
    [System.Web.WebPages.PageVirtualPathAttribute("~/Views/ApprenticeshipApplication/Apply.cshtml")]
    public partial class Apply : System.Web.Mvc.WebViewPage<SFA.Apprenticeships.Web.Candidate.ViewModels.Applications.ApprenticeshipApplicationViewModel>
    {
        public Apply()
        {
        }
        public override void Execute()
        {
            
            #line 3 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
  
    ViewBag.Title = "Application form - Find an apprenticeship";
    Layout = "~/Views/Shared/_Layout.cshtml";

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"progress-indicator progress-indicator-horizontal\"");

WriteLiteral(">\r\n    <ul>\r\n        <li");

WriteLiteral(" class=\"active\"");

WriteLiteral("><span");

WriteLiteral(" class=\"hide-tablet\"");

WriteLiteral(">Step </span>1<span");

WriteLiteral(" class=\"hide-tablet\"");

WriteLiteral("> of 3</span>. Application form</li>\r\n        <li><span");

WriteLiteral(" class=\"hide-tablet\"");

WriteLiteral(">Step </span>2<span");

WriteLiteral(" class=\"hide-tablet\"");

WriteLiteral("> of 3</span>. Check your application</li>\r\n        <li><span");

WriteLiteral(" class=\"hide-tablet\"");

WriteLiteral(">Step </span>3<span");

WriteLiteral(" class=\"hide-tablet\"");

WriteLiteral("> of 3</span>. Submitted</li>\r\n    </ul>\r\n</div>\r\n");

            
            #line 14 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
 if (Model.DateUpdated.HasValue)
{

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"panel-info\"");

WriteLiteral(">\r\n        <p");

WriteLiteral(" class=\"autosave\"");

WriteLiteral(" id=\"applicationSavedTopMessage\"");

WriteLiteral(">Last saved at ");

            
            #line 17 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                                     Write(Html.DisplayFor(m => m.DateUpdated.Value, "DateTimeSaved"));

            
            #line default
            #line hidden
WriteLiteral(" to <a");

WriteAttribute("href", Tuple.Create(" href=\"", 878), Tuple.Create("\"", 934)
            
            #line 17 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                                   , Tuple.Create(Tuple.Create("", 885), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.MyApplications)
            
            #line default
            #line hidden
, 885), false)
);

WriteLiteral(" title=\"My Applications\"");

WriteLiteral(">my applications</a></p>\r\n    </div>\r\n");

            
            #line 19 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
}

            
            #line default
            #line hidden
WriteLiteral("\r\n<noscript>\r\n    <div");

WriteLiteral(" class=\"panel-warning\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n            <h2");

WriteLiteral(" class=\"heading-large\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-info-circle\"");

WriteLiteral(@"></i>Save your application regularly</h2>
            <p>For security reasons you'll be signed out after 60 minutes.</p>
            <p>Save your application after you complete each section to ensure you don't lose any of your application.</p>
        </div>
    </div>
</noscript>

");

            
            #line 31 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
 using (Html.BeginForm(null, "ApprenticeshipApplication", new { id = Model.VacancyId }, FormMethod.Post, new { id = "application-form", autocomplete = "off" }))
{

            
            #line default
            #line hidden
WriteLiteral("    <button");

WriteLiteral(" class=\"button hide-button\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" name=\"ApplicationAction\"");

WriteLiteral(" value=\"Apply\"");

WriteLiteral(" tabindex=\"-1\"");

WriteLiteral(">Apply</button>\r\n");

            
            #line 34 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
    
            
            #line default
            #line hidden
            
            #line 34 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Html.AntiForgeryToken());

            
            #line default
            #line hidden
            
            #line 34 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                            

            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"grid-wrapper\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"grid grid-2-3\"");

WriteLiteral(">\r\n            <h1");

WriteLiteral(" class=\"heading-xlarge\"");

WriteLiteral(" id=\"appTourStart\"");

WriteLiteral(">Application form</h1>\r\n        </div>\r\n        <div");

WriteLiteral(" class=\"grid grid-1-3\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"page-link hide-nojs\"");

WriteLiteral(">\r\n                <a");

WriteLiteral(" href=\"#\"");

WriteLiteral(" id=\"runApplyTour\"");

WriteLiteral("><i");

WriteLiteral(" class=\"fa fa-question-circle\"");

WriteLiteral("></i>How to apply for an apprenticeship</a>\r\n                <p>(interactive walk" +
"through)</p>\r\n            </div>\r\n        </div>\r\n    </div>\r\n");

WriteLiteral("    <section");

WriteLiteral(" class=\"section-border\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" class=\"text\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"hgroup-medium\"");

WriteLiteral(">\r\n");

WriteLiteral("                ");

            
            #line 49 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
           Write(Html.HiddenFor(m => m.VacancyDetail.Title));

            
            #line default
            #line hidden
WriteLiteral("\r\n                <h2");

WriteLiteral(" class=\"heading-medium\"");

WriteLiteral(" id=\"vacancy-title\"");

WriteLiteral(">");

            
            #line 50 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                         Write(Model.VacancyDetail.Title);

            
            #line default
            #line hidden
WriteLiteral("</h2>\r\n");

WriteLiteral("                ");

            
            #line 51 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
           Write(Html.HiddenFor(m => m.VacancyDetail.EmployerName));

            
            #line default
            #line hidden
WriteLiteral("\r\n                <p");

WriteLiteral(" class=\"subtitle\"");

WriteLiteral(" id=\"vacancy-employer\"");

WriteLiteral(">");

            
            #line 52 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                     Write(Model.VacancyDetail.EmployerName);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n            </div>\r\n");

WriteLiteral("            ");

            
            #line 54 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
       Write(Html.HiddenFor(m => m.VacancyDetail.Description));

            
            #line default
            #line hidden
WriteLiteral("\r\n            <p");

WriteLiteral(" id=\"vacancy-summary\"");

WriteLiteral(">");

            
            #line 55 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                               Write(Model.VacancyDetail.Description);

            
            #line default
            #line hidden
WriteLiteral("</p>\r\n            <p");

WriteLiteral(" id=\"appTourSummary\"");

WriteLiteral(">\r\n                <a");

WriteAttribute("href", Tuple.Create(" href=\"", 2858), Tuple.Create("\"", 2951)
            
            #line 57 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
, Tuple.Create(Tuple.Create("", 2865), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.ApprenticeshipDetails, new { id = Model.VacancyId })
            
            #line default
            #line hidden
, 2865), false)
);

WriteLiteral(">View apprenticeship</a>\r\n            </p>\r\n        </div>\r\n    </section>\r\n");

            
            #line 61 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"


    
            
            #line default
            #line hidden
            
            #line 63 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Html.Partial("ValidationSummary", ViewData.ModelState));

            
            #line default
            #line hidden
            
            #line 63 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                           


    
            
            #line default
            #line hidden
            
            #line 66 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Html.DisplayFor(m => m.Candidate));

            
            #line default
            #line hidden
            
            #line 66 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                      
    
            
            #line default
            #line hidden
            
            #line 67 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Html.EditorFor(m => m.Candidate.Education));

            
            #line default
            #line hidden
            
            #line 67 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                               
    
            
            #line default
            #line hidden
            
            #line 68 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Html.HiddenFor(m => m.VacancyId));

            
            #line default
            #line hidden
            
            #line 68 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                     
    
            
            #line default
            #line hidden
            
            #line 69 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Html.HiddenFor(m => m.IsJavascript, new { @id = "hidden-flag-javascript" }));

            
            #line default
            #line hidden
            
            #line 69 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                                                

    Html.RenderPartial("_qualificationsJS", Model);
    Html.RenderPartial("_qualificationsNonJS", Model);

    Html.RenderPartial("_workExperiencesJS", Model);
    Html.RenderPartial("_workExperiencesNonJS", Model);

    Html.RenderPartial("_trainingCoursesJS", Model);
    Html.RenderPartial("_trainingCoursesNonJS", Model);

    
            
            #line default
            #line hidden
            
            #line 80 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Html.EditorFor(m => m.Candidate.AboutYou));

            
            #line default
            #line hidden
            
            #line 80 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                              
    
            
            #line default
            #line hidden
            
            #line 81 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Html.EditorFor(m => m.Candidate.EmployerQuestionAnswers));

            
            #line default
            #line hidden
            
            #line 81 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                             
    
            
            #line default
            #line hidden
            
            #line 82 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Html.EditorFor(m => m.Candidate.MonitoringInformation, "MonitoringInformation/_disability"));

            
            #line default
            #line hidden
            
            #line 82 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                                                                


            
            #line default
            #line hidden
WriteLiteral("    <div");

WriteLiteral(" class=\"form-group\"");

WriteLiteral(">\r\n        ");

WriteLiteral("\r\n        <button");

WriteLiteral(" id=\"apply-button\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" name=\"ApplicationAction\"");

WriteLiteral(" class=\"button no-check-for-dirty-form\"");

WriteLiteral(" value=\"Apply\"");

WriteLiteral(">Save and continue</button>\r\n        <div");

WriteLiteral(" class=\"panel-danger toggle-content hide-nojs\"");

WriteLiteral(" id=\"unsavedChanges\"");

WriteLiteral(">\r\n            <div");

WriteLiteral(" class=\"text toggle-content hide-nojs\"");

WriteLiteral(" id=\"unsavedQuals\"");

WriteLiteral(">\r\n                <p>You\'ve still got <a");

WriteLiteral(" href=\"#qualifications-panel\"");

WriteLiteral(">unsaved qualifications</a>. Make sure you click \"Save this qualification\" after " +
"entering each one.</p>\r\n            </div>\r\n            <div");

WriteLiteral(" class=\"text toggle-content hide-nojs\"");

WriteLiteral(" id=\"unsavedWorkExp\"");

WriteLiteral(">\r\n                <p>You\'ve still got <a");

WriteLiteral(" href=\"#workexperience-panel\"");

WriteLiteral(">unsaved work experience</a>. Make sure you click \"Save this work experience\" aft" +
"er entering each one.</p>\r\n            </div>\r\n            <div");

WriteLiteral(" class=\"text toggle-content hide-nojs\"");

WriteLiteral(" id=\"unsavedTrainingCourse\"");

WriteLiteral(">\r\n                <p>You\'ve still got an <a");

WriteLiteral(" href=\"#training-history-panel\"");

WriteLiteral(">unsaved training course</a>. Make sure you click \"Save this training course\" aft" +
"er entering each one.</p>\r\n            </div>\r\n        </div>\r\n        <p");

WriteLiteral(" id=\"saveApplication\"");

WriteLiteral(">\r\n            <button");

WriteLiteral(" id=\"save-button\"");

WriteLiteral(" type=\"submit\"");

WriteLiteral(" name=\"ApplicationAction\"");

WriteLiteral(" class=\"button-link no-check-for-dirty-form cancel\"");

WriteLiteral(" value=\"Save\"");

WriteAttribute("onclick", Tuple.Create(" onclick=\'", 5241), Tuple.Create("\'", 5426)
, Tuple.Create(Tuple.Create("", 5251), Tuple.Create("Webtrends.multiTrack({", 5251), true)
, Tuple.Create(Tuple.Create(" ", 5273), Tuple.Create("element:", 5274), true)
, Tuple.Create(Tuple.Create(" ", 5282), Tuple.Create("this,", 5283), true)
, Tuple.Create(Tuple.Create(" ", 5288), Tuple.Create("argsa:", 5289), true)
, Tuple.Create(Tuple.Create(" ", 5295), Tuple.Create("[\"DCS.dcsuri\",", 5296), true)
, Tuple.Create(Tuple.Create(" ", 5310), Tuple.Create("\"/apprenticeship/apply/savedraft/", 5311), true)
            
            #line 99 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                                                                                                                                  , Tuple.Create(Tuple.Create("", 5344), Tuple.Create<System.Object, System.Int32>(Model.VacancyId
            
            #line default
            #line hidden
, 5344), false)
, Tuple.Create(Tuple.Create("", 5360), Tuple.Create("\",", 5360), true)
, Tuple.Create(Tuple.Create(" ", 5362), Tuple.Create("\"WT.dl\",", 5363), true)
, Tuple.Create(Tuple.Create(" ", 5371), Tuple.Create("\"99\",", 5372), true)
, Tuple.Create(Tuple.Create(" ", 5377), Tuple.Create("\"WT.ti\",", 5378), true)
, Tuple.Create(Tuple.Create(" ", 5386), Tuple.Create("\"Apprenticeship", 5387), true)
, Tuple.Create(Tuple.Create(" ", 5402), Tuple.Create("–", 5403), true)
, Tuple.Create(Tuple.Create(" ", 5404), Tuple.Create("Save", 5405), true)
, Tuple.Create(Tuple.Create(" ", 5409), Tuple.Create("Draft", 5410), true)
, Tuple.Create(Tuple.Create(" ", 5415), Tuple.Create("Form\"]", 5416), true)
, Tuple.Create(Tuple.Create(" ", 5422), Tuple.Create("});", 5423), true)
);

WriteLiteral(">Save</button>\r\n        </p>\r\n");

            
            #line 101 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
        
            
            #line default
            #line hidden
            
            #line 101 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
         if (Model.DateUpdated.HasValue)
        {

            
            #line default
            #line hidden
WriteLiteral("            <p");

WriteLiteral(" class=\"autosave\"");

WriteLiteral(" id=\"applicationSaved\"");

WriteLiteral(">Last saved at ");

            
            #line 103 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                               Write(Html.DisplayFor(m => m.DateUpdated.Value, "DateTimeSaved"));

            
            #line default
            #line hidden
WriteLiteral(" to <a");

WriteAttribute("href", Tuple.Create(" href=\"", 5643), Tuple.Create("\"", 5699)
            
            #line 103 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                            , Tuple.Create(Tuple.Create("", 5650), Tuple.Create<System.Object, System.Int32>(Url.RouteUrl(CandidateRouteNames.MyApplications)
            
            #line default
            #line hidden
, 5650), false)
);

WriteLiteral(" title=\"My Applications\"");

WriteLiteral(">my applications</a></p>\r\n");

            
            #line 104 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
        }

            
            #line default
            #line hidden
WriteLiteral("    </div>\r\n");

            
            #line 106 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"

}

            
            #line default
            #line hidden
WriteLiteral("\r\n<div");

WriteLiteral(" class=\"saving-prompt toggle-content hide-nojs\"");

WriteLiteral(" id=\"savedNotification\"");

WriteLiteral(">\r\n    Saved\r\n</div>\r\n\r\n<ol");

WriteLiteral(" id=\"appFormTour\"");

WriteLiteral(" class=\"alwayshidden\"");

WriteLiteral(">\r\n    <li");

WriteLiteral(" data-id=\"appTourStart\"");

WriteLiteral(" data-next-button=\"Start tour\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">\r\n            <p>\r\n                The following tips are taken from <a");

WriteLiteral(" href=\"https://nationalcareersservice.direct.gov.uk/advice/courses/typesoflearnin" +
"g/Pages/apprenticeship-application-help.aspx\"");

WriteLiteral("\r\n                                                     target=\"_blank\"");

WriteLiteral(">this page on the NCS website</a>.\r\n            </p>\r\n        </div>\r\n    </li>\r\n" +
"    <li");

WriteLiteral(" data-id=\"appTourStart\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">\r\n            <p");

WriteLiteral(" class=\"small-btm-margin\"");

WriteLiteral(">\r\n                Before you start your application, print off a copy of anythin" +
"g that\r\n                might help you, such as:\r\n            </p>\r\n            " +
"<ul");

WriteLiteral(" class=\"list-bullet\"");

WriteLiteral(@">
                <li>your curriculum vitae (CV)</li>
                <li>any practice application forms you have completed</li>
                <li>personal statement</li>
                <li>achievement portfolio</li>
                <li>copies of certificates.</li>
            </ul>
        </div>
    </li>
    <li");

WriteLiteral(" data-id=\"appTourSummary\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(@">
            <p>
                This is the summary of the apprenticeship you're applying for, clicking
                this link will take you to the details page for the apprenticeship.
            </p>
            <p>
                You could also print the apprenticeship details page, so that you can
                refer to it during your application.
            </p>
        </div>
    </li>
    <li");

WriteLiteral(" data-class=\"school-name\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">\r\n            <p>If you\'re currently at school/college, then enter the name of t" +
"his one.</p>\r\n        </div>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"appTourQuals\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(@">
            <p>
                Make sure you read the ""skills and qualifications required"" area of the
                apprenticeship details. Enter any qualifications that you've got, or
                are predicted to get.
            </p>
        </div>
    </li>
    <li");

WriteLiteral(" data-id=\"appTourWork\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(@">
            <p>
                Enter as many different positions that you've had, including your main
                duties. Make sure to mention any skills that are similar to those listed
                in the ""skills and qualifications required"" area.
            </p>
            <p>The main duties section is limited to 200 characters, so keep these brief.</p>
        </div>
    </li>
    <li");

WriteLiteral(" data-id=\"appTourTraining\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">\r\n            <p>Here you\'re able to add any training courses that you\'ve been o" +
"n, these might be courses that you\'ve been on through work, or something you\'ve " +
"paid for yourself.</p>\r\n        </div>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"appTourAbout\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(@">
            <p>
                This section is one of the most important parts of the application form,
                and it will help you to get through to interview.
            </p>
            <p>
                You need to be prepared to put in a lot of work to get this section looking
                really good.
            </p>
        </div>
    </li>
    <li");

WriteLiteral(" data-class=\"appTourStrengths\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(@">
            <p>
                It’s got to be punchy, easy to read, and make the employer want to read
                more. If you’re not sure what to write, ask a friend or teacher to
                list your three best qualities.
            </p>
            <p>
                Remember to give examples of your strengths. For example, if the employer
                asks for ‘good communication skills’ you need to tell them you have
                ‘good communication skills’ and back this up with evidence of when
                you used these skills.
            </p>
            <p>You've got 4000 characters here, so don't be afraid to go into detail.</p>
        </div>
    </li>
    <li");

WriteLiteral(" data-class=\"appTourSkills\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(@">
            <p>
                In this section you only need to write a good paragraph or a few sentences
                that answers the question. Try to do it in a clever way and link it
                directly to the job you’re applying for.
            </p>
            <p>
                For example, if you were going for a job in hairdressing you could say
                something like this:
            </p>
            <p>
                ‘I would like to improve my knowledge of the latest cutting and colouring
                techniques being used within the fashion industry and how these could
                be adapted to suit high street fashion trends.’
            </p>
        </div>
    </li>
    <li");

WriteLiteral(" data-class=\"appTourHobbies\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(@">
            <p>
                When choosing which personal interests to put on your application, advisers
                can help you to choose things that show a different side to your personality
                than your job skills. This shows employers that you’re a well-rounded
                person.
            </p>
            <p>
                We know this section can be hard to fill in, as many people are modest
                about their achievements outside of work and school. In fact, many
                people don’t see their activities as ‘interests’ at all.
            </p>
            <p>
                An adviser can talk to you about what you like to do in your spare time,
                and can help you to pick out things that will impress employers.
            </p>
        </div>
    </li>
    <li");

WriteLiteral(" data-id=\"appTourAdditional1\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(@">
            <p>
                These additional questions are important, think carefully about how you
                answer them and make sure you tailor your answer to the employer's
                requirements.
            </p>
        </div>
    </li>
    <li");

WriteLiteral(" data-id=\"apply-button\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(@">
            <p>
                Once you're happy with your application, continue to the next page where
                you can read through your answers and finally submit your application.
            </p>
            <p>
                Employers will be impressed if your application seems carefully considered,
                and like you’ve made the effort to understand the role and what’s required
                of you.
            </p>
            <p>
                You can create this impression by applying for fewer jobs but taking
                the time to make sure each application is tailored to that organisation
                and role.
            </p>
        </div>
    </li>
    <li");

WriteLiteral(" data-id=\"saveApplication\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">\r\n            <p>\r\n                You can save your application at any time, an" +
"d pick it up where you left\r\n                off from in \"my applications\"\r\n    " +
"        </p>\r\n        </div>\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"runApplyTour\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">\r\n            <p>You can start this tour again at any point from your applicatio" +
"n form.</p>\r\n        </div>\r\n    </li>\r\n    <li");

WriteLiteral(" class=\"joyride-withborder\"");

WriteLiteral(" data-id=\"headerLinkFAA\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">\r\n            <p>\r\n                Find an apprenticeship to apply for, using so" +
"me of the tips from this\r\n                tour.\r\n            </p>\r\n        </div" +
">\r\n    </li>\r\n    <li");

WriteLiteral(" data-id=\"myapplications-link\"");

WriteLiteral(" data-button=\"Finish\"");

WriteLiteral(">\r\n        <div");

WriteLiteral(" aria-live=\"polite\"");

WriteLiteral(">\r\n            <p>\r\n                If you\'ve got saved applications, try using t" +
"hese tips to tailor your\r\n                application to the apprenticeship you\'" +
"re applying for.\r\n            </p>\r\n        </div>\r\n    </li>\r\n</ol>\r\n\r\n\r\n");

DefineSection("scripts", () => {

WriteLiteral("\r\n\r\n");

WriteLiteral("    ");

            
            #line 304 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Scripts.Render("~/bundles/knockout"));

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(@">

        var qualificationData = null;
        var workExperienceData = null;
        var trainingCourseData = null;
        var currentYear = null;
        var whitelistregex = null;
        var yearRegex = null;
        var autoSaveTimeout = null;

        $(function() {
            document.getElementById(""hidden-flag-javascript"").value = ""True"";

            qualificationData = ");

            
            #line 319 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                           Write(Html.Raw(Json.Encode(Model.Candidate.Qualifications)));

            
            #line default
            #line hidden
WriteLiteral(";\r\n            workExperienceData = ");

            
            #line 320 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                            Write(Html.Raw(Json.Encode(Model.Candidate.WorkExperience)));

            
            #line default
            #line hidden
WriteLiteral(";\r\n            trainingCourseData = ");

            
            #line 321 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                            Write(Html.Raw(Json.Encode(Model.Candidate.TrainingCourses)));

            
            #line default
            #line hidden
WriteLiteral(";\r\n\r\n            currentYear = ");

            
            #line 323 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                     Write(Html.Raw(Model.CurrentYear));

            
            #line default
            #line hidden
WriteLiteral(";\r\n            whitelistregex = ");

            
            #line 324 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                        Write(Html.Raw(Json.Encode(Model.WhiteListRegex)));

            
            #line default
            #line hidden
WriteLiteral(";\r\n            yearRegex = ");

            
            #line 325 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                   Write(Html.Raw(Json.Encode(Model.FourDigitYearRegex)));

            
            #line default
            #line hidden
WriteLiteral(";\r\n\r\n            autoSaveTimeout = ");

            
            #line 327 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                         Write(Html.Raw(Json.Encode(Model.AutoSaveTimeInMinutes)));

            
            #line default
            #line hidden
WriteLiteral(" * 60 * 1000;\r\n\r\n            $(window).on(\'load\', function() {\r\n                d" +
"irtyFormDialog.initialise({\r\n                    formSelector: \"form\",\r\n        " +
"            classToExclude: \"no-check-for-dirty-form\",\r\n                    time" +
"out: ");

            
            #line 333 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                        Write(Model.SessionTimeout);

            
            #line default
            #line hidden
WriteLiteral(" * 1000,\r\n                    confirmationMessage: \'");

            
            #line 334 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                     Write(Model.ConfirmationMessage);

            
            #line default
            #line hidden
WriteLiteral("\'\r\n                });\r\n            });\r\n\r\n            var shouldShowQualMessage;" +
"\r\n\r\n            setTimeout(function() {\r\n                shouldShowQualMessage =" +
" false;\r\n            }, ");

            
            #line 342 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
          Write(Model.SessionTimeout);

            
            #line default
            #line hidden
WriteLiteral(@" * 1000);

            $(window).on('beforeunload', function (e){
                if(shouldShowQualMessage == false) {
                    return;
                } else {
                    //https://developer.mozilla.org/en-US/docs/Web/Events/beforeunload

                    if ($('#apply-button').hasClass('dirtyQuals') || $('#apply-button').hasClass('dirtyWorkExp')) {
                        (e || window.event).returnValue = '");

            
            #line 351 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                      Write(Model.ConfirmationMessage);

            
            #line default
            #line hidden
WriteLiteral("\'; //Gecko + IE\r\n                        return \'");

            
            #line 352 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                           Write(Model.ConfirmationMessage);

            
            #line default
            #line hidden
WriteLiteral(@"'; //Webkit, Safari, Chrome etc.
                    }
                }
                return;
            });

            setTimeout(function() {
                timeout_trigger();
            }, autoSaveTimeout);
        });



        function timeout_reset() {
            setTimeout(function() {
                timeout_trigger();
            }, autoSaveTimeout);
        }

        function timeout_trigger() {

            if (dirtyFormDialog.isFormDirty(""form"")) {
                Webtrends.multiTrack({ element: this, argsa: [""DCS.dcsuri"", ""/apprenticeship/apply/autosavedraft/");

            
            #line 374 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                                                                            Write(Model.VacancyId);

            
            #line default
            #line hidden
WriteLiteral("\", \"WT.dl\", \"99\", \"WT.ti\", \"Apprenticeship – Auto Save Draft Form\"] });\r\n        " +
"        var request = $.ajax({\r\n                    type: \"POST\",\r\n             " +
"       url: \'");

            
            #line 377 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                     Write(Url.RouteUrl(CandidateRouteNames.ApprenticeshipAutoSave, new { id = Model.VacancyId }));

            
            #line default
            #line hidden
WriteLiteral(@"',
                    cache: false,
                    timeout: 30000,
                    data: $(""#application-form"").serialize()
                });

                request.done(function(result) {

                    if (result.Status == ""succeeded"") {
                        var savedMessage = 'Last saved at ' + result.DateTimeMessage + ' to ' + '<a href=""");

            
            #line 386 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
                                                                                                     Write(Url.RouteUrl(CandidateRouteNames.MyApplications));

            
            #line default
            #line hidden
WriteLiteral("\" title=\"My Applications\">my applications</a>\';\r\n                        $(\"p.aut" +
"osave\").html(savedMessage);\r\n\r\n                        $(\'#savedNotification\').s" +
"how();\r\n                        setTimeout(function() { $(\"#savedNotification\")." +
"hide(); }, 5000);\r\n\r\n                        dirtyFormDialog.resetDirtyForm({\r\n " +
"                           formSelector: \"form\"\r\n                        });\r\n\r\n" +
"                        window.resetSessionTimeout();\r\n                    }\r\n\r\n" +
"                });\r\n\r\n                request.fail(function(jqXHR, textStatus, " +
"errorThrown) {\r\n\r\n                });\r\n            }\r\n\r\n            timeout_rese" +
"t();\r\n        }\r\n\r\n        function getCurrentYear() {\r\n            return curre" +
"ntYear;\r\n        }\r\n\r\n        function getQualificationData() {\r\n            ret" +
"urn qualificationData;\r\n        }\r\n\r\n        function getWorkExperienceData() {\r" +
"\n            return workExperienceData;\r\n        }\r\n\r\n        function getTraini" +
"ngCourseData() {\r\n            return trainingCourseData;\r\n        }\r\n\r\n        f" +
"unction getWhiteListRegex() {\r\n            return whitelistregex;\r\n        }\r\n\r\n" +
"        function getYearRegex() {\r\n            return yearRegex;\r\n        }\r\n\r\n " +
"       function getMonthLabel(index) {\r\n            var month = \"\";\r\n\r\n         " +
"   if (index === 0) {\r\n\r\n            } else {\r\n                var mths = [\'Jan\'" +
", \'Feb\', \'Mar\', \'Apr\', \'May\', \'June\', \'July\', \'Aug\', \'Sept\', \'Oct\', \'Nov\', \'Dec\'" +
"];\r\n                month = mths[index - 1];\r\n            }\r\n\r\n            retur" +
"n month;\r\n        }\r\n\r\n    </script>\r\n\r\n");

WriteLiteral("    ");

            
            #line 448 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Scripts.Render("~/bundles/nas/application"));

            
            #line default
            #line hidden
WriteLiteral("\r\n");

WriteLiteral("    ");

            
            #line 449 "..\..\Views\ApprenticeshipApplication\Apply.cshtml"
Write(Scripts.Render("~/bundles/joyride"));

            
            #line default
            #line hidden
WriteLiteral(@"
    <script>
        $(function () {
            function appTour() {
                $('#appFormTour').joyride({
                    'autoStart': true,
                    'nextButton': true,
                    'prev_button': true,
                    'tipAnimation': 'pop'
                });
            }

            $('#runApplyTour').on('click', function () {
                appTour();

                return false;
            });
        });
    </script>

");

});

        }
    }
}
#pragma warning restore 1591
