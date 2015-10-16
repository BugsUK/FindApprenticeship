namespace SFA.Apprenticeships.Web.Candidate
{
    using System;
    using Common.Configuration;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Common.IoC;
    using Infrastructure.Logging.IoC;
    using Microsoft.WindowsAzure;
    using StructureMap;
    using Views;

    public partial class _403 : ErrorBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var container = new Container(x =>
                {
                    x.AddRegistry<LoggingRegistry>();
                    x.AddRegistry<CommonRegistry>();
                });
                var configurationService = container.GetInstance<IConfigurationService>();
                var message = configurationService.Get<CommonWebConfiguration>().WebsiteOfflineMessage;

                OfflineMessageLabel.Text = message;
            }

            SetTitle("403");

            var userJourney = GetUserJourney();
            HeaderTitle.InnerText = userJourney == "Apprenticeship" ? "Find an apprenticeship" : "Find a traineeship";
        }
    }
}