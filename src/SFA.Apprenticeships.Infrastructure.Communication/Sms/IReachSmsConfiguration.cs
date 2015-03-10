namespace SFA.Apprenticeships.Infrastructure.Communication.Sms
{
    using System.Collections.Generic;

    public interface IReachSmsConfiguration
    {
        string Username { get; }

        string Password { get; }

        string Originator { get; }

        string Url { get; }

        string CallbackUrl { get; }

        IEnumerable<SmsTemplateConfiguration> Templates { get; }
    }
}