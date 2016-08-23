namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Application.Interfaces.Communications;
    using Configuration;

    using SFA.Apprenticeships.Application.Interfaces;

    public abstract class SmsMessageFormatter
    {
        protected string Message;

        private readonly IEnumerable<SmsTemplate> _templateConfigurations;

        protected SmsMessageFormatter(IConfigurationService configurationService)
        {
            var config = configurationService.Get<SmsConfiguration>();
            _templateConfigurations = config.Templates;
        }

        protected SmsTemplate GetTemplateConfiguration(string templateName)
        {
            var template = _templateConfigurations.FirstOrDefault(each => each.Name == templateName);

            if (template != null)
            {
                return template;
            }

            var errorMessage = string.Format("GetTemplateConfiguration : Invalid SMS template name: {0}", templateName);

            throw new ConfigurationErrorsException(errorMessage);
        }

        public abstract string GetMessage(IEnumerable<CommunicationToken> communicationTokens);
    }
}
