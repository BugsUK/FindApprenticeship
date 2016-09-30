namespace SFA.Apprenticeships.Domain.Entities.Raa.Api
{
    using System;
    using System.Collections.Generic;

    public class ApiUser
    {
        public Guid ExternalSystemId { get; set; }
        public string CompanyId { get; set; }
        public ApiBusinessCategory BusinessCategory { get; set; }
        public ApiEmployeeType EmployeeType { get; set; }
        public IList<ApiEndpoint> AuthorisedApiEndpoints { get; set; }
        public string FullName { get; set; }
        public string TradingName { get; set; }
    }
}