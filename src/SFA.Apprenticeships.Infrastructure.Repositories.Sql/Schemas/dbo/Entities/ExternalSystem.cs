namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo.Entities
{
    using System;

    public class ExternalSystem
    {
        public int ID { get; set; }
        public Guid SystemCode { get; set; }
        public string SystemName { get; set; }
        public int OrganisationId { get; set; }
        public int OrganisationType { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public bool IsNasDisabled { get; set; }
        public bool IsUserEnabled { get; set; }
        public ExternalSystemPermission ExternalSystemPermission { get; set; }
    }
}