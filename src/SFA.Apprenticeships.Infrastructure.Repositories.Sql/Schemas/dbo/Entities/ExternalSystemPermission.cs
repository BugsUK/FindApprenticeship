namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo.Entities
{
    using System;

    public class ExternalSystemPermission
    {
        public Guid Username { get; set; }
        public byte[] Password { get; set; }
        public string UserParameters { get; set; }
        public string Businesscategory { get; set; }
        public string Company { get; set; }
        public string Employeetype { get; set; }
        public Guid Salt { get; set; }
        public string FullName { get; set; }
        public string TradingName { get; set; }
    }
}