namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System.Collections.Generic;
    using Configuration;

    public interface ITwillioConfiguration
    {
        string AccountSid { get; }

        string AuthToken { get; }

        string MobileNumberFrom { get; }

        IEnumerable<SmsTemplate> Templates { get; }
    }
}