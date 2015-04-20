namespace SFA.Apprenticeships.Infrastructure.Communication.Configuration
{
    using System;
    using System.Collections.Generic;

    public class EmailConfiguration
    {
        public string Username { get; set; }
        
        public string Password { get; set; }

        public string SiteDomainName { get; set; }

        public IEnumerable<EmailTemplate> Templates { get; set; }

        public int SubCategoriesFullNamesLimit { get; set; }
    }

    public class EmailTemplate
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FromEmail { get; set; }
    }
}
