namespace SFA.Apprenticeships.Infrastructure.Communication.UnitTests.Sms.SmsMessageFormatters
{
    using System.Collections.Generic;
    using Communication.Sms;
    using Communication.Sms.SmsMessageFormatters;

    public class SmsDailyDigestMessageFormatterBuilder
    {
        private List<SmsTemplateConfiguration> _templates;

        public SmsDailyDigestMessageFormatterBuilder WithMessageTemplate(string messageTemplate)
        {
            _templates = new List<SmsTemplateConfiguration>
            {
                new SmsTemplateConfiguration
                {
                    Name = SmsDailyDigestMessageFormatter.TemplateName,
                    Message = messageTemplate
                }
            };

            return this;
        }

        public SmsDailyDigestMessageFormatter Build()
        {
            return new SmsDailyDigestMessageFormatter(_templates);
        }
    }
}