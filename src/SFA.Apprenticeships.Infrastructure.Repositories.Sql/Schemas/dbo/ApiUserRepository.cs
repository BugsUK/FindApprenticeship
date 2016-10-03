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

        private static readonly IDictionary<ApiEndpoint, string> ApiEndpointsCodeMap = new Dictionary<ApiEndpoint, string>
        {
            {ApiEndpoint.VacancySummary, "VSI"},
            {ApiEndpoint.VacancyDetail, "VDI"},
            {ApiEndpoint.ReferenceData, "RDS"},
            {ApiEndpoint.BulkVacancyUpload, "BVU"},
            {ApiEndpoint.ApplicationTracking, "ATS"}
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

        public ApiUser Create(ApiUser apiUser)
        {
            throw new NotImplementedException();
        }

        public ApiUser Update(ApiUser apiUser)
        {
            var sql = "SELECT * FROM ExternalSystemPermission esp WHERE Username = @ExternalSystemId";

            var externalSystemPermission = _getOpenConnection.Query<ExternalSystemPermission>(sql, new { apiUser.ExternalSystemId }).Single();

            externalSystemPermission.UserParameters = string.Join(",", apiUser.AuthorisedApiEndpoints.Select(ae => ApiEndpointsCodeMap[ae]));

            _getOpenConnection.UpdateSingle(externalSystemPermission);

            return GetApiUser(apiUser.ExternalSystemId);
        }

        private static ApiUser MapApiUser(ExternalSystemPermission externalSystemPermission)
        {
            var apiUser = new ApiUser
            {
                ExternalSystemId = externalSystemPermission.Username,
                CompanyId = externalSystemPermission.Company,
                BusinessCategory = (ApiBusinessCategory)Enum.Parse(typeof(ApiBusinessCategory), externalSystemPermission.Businesscategory),
                EmployeeType = (ApiEmployeeType)Enum.Parse(typeof(ApiEmployeeType), externalSystemPermission.Employeetype),
                AuthorisedApiEndpoints = externalSystemPermission.UserParameters.Split(',').Where(s => ApiEndpointsMap.ContainsKey(s)).Select(s => ApiEndpointsMap[s]).ToList(),
                FullName = externalSystemPermission.FullName,
                TradingName = externalSystemPermission.TradingName
            };

            return apiUser;
        }
    }
}