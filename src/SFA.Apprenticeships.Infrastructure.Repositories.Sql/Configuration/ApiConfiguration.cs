namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Configuration
{
    using System;

    public class ApiConfiguration
    {
        public string EmployerInformationUrl { get; set; }
        public string EmployerInformationText { get; set; }
        public Guid Salt { get; set; }
    }
}