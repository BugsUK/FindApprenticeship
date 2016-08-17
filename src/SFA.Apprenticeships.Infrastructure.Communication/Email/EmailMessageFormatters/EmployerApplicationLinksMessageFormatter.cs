namespace SFA.Apprenticeships.Infrastructure.Communication.Email.EmailMessageFormatters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Application.Interfaces;
    using Application.Interfaces.Communications;
    using Configuration;
    using Newtonsoft.Json;
    using SendGrid;
    using SFA.Infrastructure.Interfaces;

    public class EmployerApplicationLinksMessageFormatter : EmailMessageFormatter
    {
        private readonly string _siteDomainName;

        public EmployerApplicationLinksMessageFormatter(IConfigurationService configurationService)
        {
            _siteDomainName = configurationService.Get<CommunicationConfiguration>().RecruitSiteDomainName;
        }

        public override void PopulateMessage(EmailRequest request, ISendGrid message)
        {
            var sendgridToken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(CommunicationTokens.EmployerApplicationLinks);

            var applicationLinksJson = request.Tokens.Single(t => t.Key == CommunicationTokens.EmployerApplicationLinks).Value;
            var applicationLinks = JsonConvert.DeserializeObject<Dictionary<string, string>>(applicationLinksJson) ?? new Dictionary<string, string>();

            var linksStringBuilder = new StringBuilder("<ul>");
            foreach (var applicationLink in applicationLinks)
            {
                linksStringBuilder.Append($"<li><a href=\"https://{_siteDomainName}{applicationLink.Value}\">{applicationLink.Key}</a></li>");
            }
            linksStringBuilder.Append("</ul>");

            message.AddSubstitution(sendgridToken, new List<string> { linksStringBuilder.ToString() });

            var tokens = new List<CommunicationTokens> { CommunicationTokens.ApplicationVacancyTitle, CommunicationTokens.ProviderName, CommunicationTokens.EmployerApplicationLinksExpiry };

            foreach (var token in request.Tokens.Where(t => tokens.Contains(t.Key)))
            {
                var sendgridtoken = SendGridTokenManager.GetEmailTemplateTokenForCommunicationToken(token.Key);

                message.AddSubstitution(sendgridtoken,
                    new List<string>
                    {
                        token.Value
                    });
            }
        }
    }
}