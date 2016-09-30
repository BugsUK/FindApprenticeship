namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Api
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Api;

    public class ApiUserViewModel
    {
        public Guid ExternalSystemId { get; set; }
        public string CompanyId { get; set; }
        public ApiBusinessCategory BusinessCategory { get; set; }
        public ApiEmployeeType EmployeeType { get; set; }
        public IList<ApiEndpoint> AuthorisedApiEndpoints { get; set; }
        public string FullName { get; set; }
        public string TradingName { get; set; }

        public string CompanyName
        {
            get
            {
                if(FullName == TradingName)
                {
                    return FullName;
                }
                else
                {
                    return $"{TradingName} ({FullName})";
                }
            }
        }
    }
}