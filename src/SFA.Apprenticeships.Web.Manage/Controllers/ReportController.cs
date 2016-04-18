namespace SFA.Apprenticeships.Web.Manage.Controllers
{
    using System;
    using System.Net;
    using System.Security.Principal;
    using System.Web.Mvc;
    using System.Web.UI.WebControls;
    using Attributes;
    using Common.Configuration;
    using Domain.Entities.Raa;
    using Microsoft.Reporting.WebForms;
    using SFA.Infrastructure.Interfaces;

    [AuthorizeUser(Roles = Roles.Raa)]
    public class ReportController : ManagementControllerBase
    {
        private readonly ReportServerConfiguration _reportServerConfiguration;

        public ReportController(IConfigurationService configurationService)
        {
            _reportServerConfiguration = configurationService.Get<ReportServerConfiguration>();
        }

        public ActionResult Index()
        {
            var reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Remote;

            reportViewer.ServerReport.ReportPath = "/NAVMS/VacanciesCSV";
            reportViewer.ServerReport.ReportServerUrl = new Uri(_reportServerConfiguration.ReportServerUrl);
            reportViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials(_reportServerConfiguration);

            reportViewer.Width = Unit.Pixel(900);
            reportViewer.Height = Unit.Pixel(1200);

            ViewBag.ReportViewer = reportViewer;

            return View();
        }

        public class ReportServerCredentials : IReportServerCredentials
        {
            private readonly ReportServerConfiguration _reportServerConfiguration;

            public ReportServerCredentials(ReportServerConfiguration reportServerConfiguration)
            {
                _reportServerConfiguration = reportServerConfiguration;
            }

            public bool GetFormsCredentials(out Cookie authCookie, out string userName, out string password, out string authority)
            {
                authCookie = null;
                userName = null;
                password = null;
                authority = null;

                // Not using form credentials
                return false;
            }

            public WindowsIdentity ImpersonationUser
            {
                get
                {
                    // Use the default Windows user.  Credentials will be
                    // provided by the NetworkCredentials property.
                    return null;
                }
            }

            public ICredentials NetworkCredentials
            {
                get
                {
                    return new NetworkCredential(_reportServerConfiguration.NetworkUsername, _reportServerConfiguration.NetworkPassword, _reportServerConfiguration.NetworkDomain);
                }
            }
        }
    }
}