namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using Application.Interfaces;
    using Common;
    using Configuration;
    using Domain.Entities.Raa.Api;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Entities;

    public class ApiUserRepository : IApiUserRepository
    {
        private const string SetPasswordSql = @"DECLARE @DbPassword [varchar](64)
SET @DbPassword = @password
UPDATE ExternalSystemPermission 
SET Password = CONVERT(VARBINARY(25), HASHBYTES('SHA1', @DbPassword), 1)
WHERE Username = @Username";

        private readonly IGetOpenConnection _getOpenConnection;
        private readonly IConfigurationService _configurationService;

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

        private static readonly char[] ApiPasswordCharacters =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
            'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '-', '_', '='
        };

        public ApiUserRepository(IGetOpenConnection getOpenConnection, IConfigurationService configurationService)
        {
            _getOpenConnection = getOpenConnection;
            _configurationService = configurationService;
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

        public ApiUser GetApiUser(string companyId)
        {
            var externalSystemPermission = GetExternalSystemPermission(companyId);
            return externalSystemPermission != null ? MapApiUser(externalSystemPermission) : null;
        }

        private ExternalSystemPermission GetExternalSystemPermission(string companyId)
        {
            var sqls = new[]
            {
                "SELECT '' AS UserParameters, 'LearningProvider' AS Businesscategory, UKPRN as Company, 'TrainingProviderInterfaceAdmin' AS Employeetype, FullName, TradingName FROM Provider WHERE UKPRN = @CompanyId",
                "SELECT '' AS UserParameters, 'Employer' AS Businesscategory, EDSURN as Company, 'EmployerInterfaceAdmin' AS Employeetype, FullName, TradingName FROM Employer WHERE EDSURN = @CompanyId",
                "SELECT '' AS UserParameters, 'ThirdParty' AS Businesscategory, EDSURN as Company, 'NasInterfaceAdmin' AS Employeetype, ThirdPartyName AS FullName, ThirdPartyName AS TradingName FROM ThirdParty WHERE EDSURN = @CompanyId"
            };

            foreach (var sql in sqls)
            {
                var externalSystemPermission = _getOpenConnection.Query<ExternalSystemPermission>(sql, new { companyId }).SingleOrDefault();
                if (externalSystemPermission != null)
                {
                    return externalSystemPermission;
                }
            }

            return null;
        }

        public ApiUser Create(ApiUser apiUser)
        {
            var apiConfiguration = _configurationService.Get<ApiConfiguration>();

            var externalSystemPermission = GetExternalSystemPermission(apiUser.CompanyId);

            externalSystemPermission.Username = apiUser.ExternalSystemId == Guid.Empty ? Guid.NewGuid() : apiUser.ExternalSystemId;
            externalSystemPermission.Password = new byte[64];
            externalSystemPermission.UserParameters = string.Join(",", apiUser.AuthorisedApiEndpoints.Select(ae => ApiEndpointsCodeMap[ae]));
            externalSystemPermission.Salt = apiConfiguration.Salt;

            _getOpenConnection.Insert(externalSystemPermission);

            var password = string.IsNullOrEmpty(apiUser.Password) ? GetApiPassword() : apiUser.Password;

            _getOpenConnection.MutatingQuery<ExternalSystemPermission>(SetPasswordSql, new { password, externalSystemPermission.Username });

            var createdApiUser = GetApiUser(externalSystemPermission.Username);
            createdApiUser.Password = password;

            return createdApiUser;
        }

        public ApiUser Update(ApiUser apiUser)
        {
            var sql = "SELECT * FROM ExternalSystemPermission esp WHERE Username = @ExternalSystemId";

            var externalSystemPermission = _getOpenConnection.Query<ExternalSystemPermission>(sql, new { apiUser.ExternalSystemId }).Single();

            externalSystemPermission.UserParameters = string.Join(",", apiUser.AuthorisedApiEndpoints.Select(ae => ApiEndpointsCodeMap[ae]));

            _getOpenConnection.UpdateSingle(externalSystemPermission);

            return GetApiUser(apiUser.ExternalSystemId);
        }

        public ApiUser ResetApiUserPassword(Guid externalSystemId)
        {
            var password = GetApiPassword();

            _getOpenConnection.MutatingQuery<ExternalSystemPermission>(SetPasswordSql, new { password, Username = externalSystemId });

            var apiUser = GetApiUser(externalSystemId);
            apiUser.Password = password;

            return apiUser;
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

        private static string GetApiPassword()
        {
            const int length = 16;

            var identifier = new char[length];
            var randomData = new byte[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomData);
            }

            for (var idx = 0; idx < identifier.Length; idx++)
            {
                var pos = randomData[idx] % ApiPasswordCharacters.Length;
                identifier[idx] = ApiPasswordCharacters[pos];
            }

            return new string(identifier);
        }
    }
}