﻿namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Application.Interfaces.Communications;

    public abstract class SmsMessageFormatter
    {
        protected string Message;

        private readonly IEnumerable<SmsTemplateConfiguration> _templateConfigurations;

        protected SmsMessageFormatter(IEnumerable<SmsTemplateConfiguration> templateConfigurations)
        {
            _templateConfigurations = templateConfigurations;
        }

        protected SmsTemplateConfiguration GetTemplateConfiguration(string templateName)
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
