namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Api;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Entities;

    public class ApiUserRepository : IApiUserRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;

        private static readonly IDictionary<string, ApiEndpoint> ApiEndpointsMap = new Dictionary<string, ApiEndpoint>
        {
            {"VSI", ApiEndpoint.VacancySummary},
            {"VDI", ApiEndpoint.VacancyDetail},
            {"RDS", ApiEndpoint.ReferenceData},
            {"BVU", ApiEndpoint.BulkVacancyUpload},
            {"ATS", ApiEndpoint.ApplicationTracking}
        };

        public ApiUserRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IEnumerable<ApiUser> SearchApiUsers(ApiUserSearchParameters searchParameters)
        {
            var sql = @"SELECT esp.*, 
  COALESCE(p.FullName, e.FullName, tp.ThirdPartyName) AS FullName, 
  COALESCE(p.TradingName, e.TradingName, tp.ThirdPartyName) AS TradingName 
  FROM ExternalSystemPermission esp
  LEFT JOIN Provider p ON esp.Company = p.UKPRN
  LEFT JOIN Employer e ON esp.Company = e.EDSURN
  LEFT JOIN ThirdParty tp ON esp.Company = tp.EDSURN WHERE ";
            if (!string.IsNullOrEmpty(searchParameters.ExternalSystemId))
            {
                sql += "Username = @ExternalSystemId ";
            }
            if (!string.IsNullOrEmpty(searchParameters.Id))
            {
                sql += "Company = @Id ";
            }
            if (!string.IsNullOrEmpty(searchParameters.Name))
            {
                sql += @"p.FullName LIKE '%' + @name + '%' OR p.TradingName LIKE '%' + @name + '%' 
OR e.FullName LIKE '%' + @name + '%' OR e.TradingName LIKE '%' + @name + '%' 
OR tp.ThirdPartyName LIKE '%' + @name + '%'";
            }
            sql += "ORDER BY FullName";

            var externalSystemPermissions = _getOpenConnection.Query<ExternalSystemPermission>(sql, searchParameters);

            return externalSystemPermissions.Select(MapApiUser);
        }

        public ApiUser GetApiUser(Guid externalSystemId)
        {
            var sql = @"SELECT esp.*, 
  COALESCE(p.FullName, e.FullName, tp.ThirdPartyName) AS FullName, 
  COALESCE(p.TradingName, e.TradingName, tp.ThirdPartyName) AS TradingName 
  FROM ExternalSystemPermission esp
  LEFT JOIN Provider p ON esp.Company = p.UKPRN
  LEFT JOIN Employer e ON esp.Company = e.EDSURN
  LEFT JOIN ThirdParty tp ON esp.Company = tp.EDSURN 
  WHERE Username = @ExternalSystemId";

            var externalSystemPermission = _getOpenConnection.Query<ExternalSystemPermission>(sql, new { externalSystemId }).Single();

            return MapApiUser(externalSystemPermission);
        }

        private static ApiUser MapApiUser(ExternalSystemPermission externalSystemPermission)
        {
            var apiUser = new ApiUser
            {
                ExternalSystemId = externalSystemPermission.Username,
                CompanyId = externalSystemPermission.Company,
                BusinessCategory = (ApiBusinessCategory)Enum.Parse(typeof(ApiBusinessCategory), externalSystemPermission.Businesscategory),
                EmployeeType = (ApiEmployeeType)Enum.Parse(typeof(ApiEmployeeType), externalSystemPermission.Employeetype),
                AuthorisedApiEndpoints = externalSystemPermission.UserParameters.Split(',').Select(s => ApiEndpointsMap[s]).ToList(),
                FullName = externalSystemPermission.FullName,
                TradingName = externalSystemPermission.TradingName
            };

            return apiUser;
        }
    }
}