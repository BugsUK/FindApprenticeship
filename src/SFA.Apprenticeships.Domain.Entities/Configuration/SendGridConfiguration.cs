namespace SFA.Apprenticeships.Domain.Entities.Configuration
{
    using System;
    using System.Collections.Generic;

    public class SendGridConfiguration
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Originator { get; set; }

        public string Url { get; set; }

        public string CallbackUrl { get; set; }

        public IEnumerable<EmailTemplate> Templates { get; set; } 
    }

    public class EmailTemplate
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FromEmail { get; set; }
    }
}
