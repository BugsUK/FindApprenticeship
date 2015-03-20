namespace SFA.Apprenticeships.Domain.Entities.Configuration
{
    using System;
    using System.Collections.Generic;

    public class SendGridConfiguration
    {
        string Username { get; set; }

        string Password { get; set; }

        string Originator { get; set; }

        string Url { get; set; }

        string CallbackUrl { get; set; }

        IEnumerable<IEmailTemplate> Templates { get; set; } 
    }

    public interface IEmailTemplate
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string FromEmail { get; set; }
    }
}
