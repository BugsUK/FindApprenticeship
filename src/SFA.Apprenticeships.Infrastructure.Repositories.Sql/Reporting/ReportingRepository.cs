namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Common;
    using Domain.Raa.Interfaces.Reporting;
    using Domain.Raa.Interfaces.Reporting.Models;
    using SFA.Infrastructure.Interfaces;

    public class ReportingRepository : IReportingRepository
    {
        private readonly ILogService _logger;
        private readonly IGetOpenConnection _getOpenConnection;

        public ReportingRepository(IGetOpenConnection getOpenConnection, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _logger = logger;
        }

        public List<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate)
        {
            _logger.Debug($"Executing ReportVacanciesList report with toDate {toDate} and fromdate {fromDate}...");

            var response = new List<ReportVacanciesResultItem>();

            var command = new SqlCommand("dbo.ReportVacanciesList", (SqlConnection)_getOpenConnection.GetOpenConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("ManagedBy", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Type", SqlDbType.Int).Value = -1;
            command.Parameters.Add("lscRegion", SqlDbType.Int).Value = -1;
            command.Parameters.Add("localauthority", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Postcode", SqlDbType.VarChar).Value = "n/a";
            command.Parameters.Add("sector", SqlDbType.Int).Value = -1;
            command.Parameters.Add("framework", SqlDbType.Int).Value = -1;
            command.Parameters.Add("vacancyType", SqlDbType.Int).Value = -1;
            command.Parameters.Add("dateFrom", SqlDbType.DateTime).Value = fromDate;
            command.Parameters.Add("dateTo", SqlDbType.DateTime).Value = toDate;
            command.Parameters.Add("VacancyStatus", SqlDbType.Int).Value = -1;
            command.Parameters.Add("ProviderSiteID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("RecAgentID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("EmployerID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("rowcount", SqlDbType.Int).Value = 0;

            command.CommandTimeout = 180;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                response.Add(new ReportVacanciesResultItem()
                {
                    vacancyid = reader[0].ToString(),
                    VacancyTitle = reader[1].ToString(),
                    VacancyType = reader[2].ToString(),
                    Reference = reader[3].ToString(),
                    EmployerName = reader[4].ToString(),
                    EmployerNameActual = reader[5].ToString(),
                    EmployerAnonymousName = reader[6].ToString(),
                    IsEmployerAnonymous = reader[7].ToString(),
                    Postcode = reader[8].ToString(),
                    Sector = reader[9].ToString(),
                    Framework = reader[10].ToString(),
                    FrameworkStatus = reader[11].ToString(),
                    LearningProvider = reader[12].ToString(),
                    NumberOfPositions = reader[13].ToString(),
                    DatePosted = reader[14].ToString(),
                    ClosingDate = reader[15].ToString(),
                    NoOfPositionsAvailable = reader[16].ToString(),
                    NoOfApplications = reader[17].ToString(),
                    Status = reader[18].ToString(),
                    DeliverySite = reader[19].ToString()
                });
            }

            _logger.Debug($"Done executing report with toDate {toDate} and fromdate {fromDate}.");

            return response;
        }

        public List<ReportUnsuccessfulCandidatesResultItem> ReportUnsuccessfulCandidates(string type, DateTime fromDate, DateTime toDate, string ageRange, string managedBy, string region)
        {
            _logger.Debug($"Executing ReportUnsuccessfulCandidates report with toDate {toDate} and fromdate {fromDate}...");

            var response = new List<ReportUnsuccessfulCandidatesResultItem>();

            var command = new SqlCommand("dbo.ReportUnsuccessfulCandidateApplications", (SqlConnection)_getOpenConnection.GetOpenConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("ManagedBy", SqlDbType.NVarChar).Value = managedBy;
            command.Parameters.Add("RegionID", SqlDbType.Int).Value = region;
            command.Parameters.Add("type", SqlDbType.Int).Value = type;
            command.Parameters.Add("LocalAuthority", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Postcode", SqlDbType.VarChar).Value = "n/a";
            command.Parameters.Add("FromDate", SqlDbType.DateTime).Value = fromDate;
            command.Parameters.Add("ToDate", SqlDbType.DateTime).Value = toDate;
            command.Parameters.Add("candidateAgeRange", SqlDbType.Int).Value = ageRange;
            command.Parameters.Add("points", SqlDbType.Int).Value = -1;
            command.Parameters.Add("MarketMessagesOnly", SqlDbType.Int).Value = 0;
            command.Parameters.Add("rowcount", SqlDbType.Int).Value = 0;

            command.CommandTimeout = 180;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                response.Add(new ReportUnsuccessfulCandidatesResultItem()
                {
                    candidateid = reader[0].ToString(),
                    FirstName = reader[1].ToString(),
                    MiddleName = reader[2].ToString(),
                    SurName = reader[3].ToString(),
                    Gender = reader[4].ToString(),
                    DateofBirth = Convert.ToDateTime(reader[5]).ToString("dd/MM/yyy"),
                    Disability = reader[6].ToString(),
                    AllowMarketingMessages = reader[7].ToString() == "TRUE" ? "Yes" : "No",
                    AddressLine1 = reader[8].ToString(),
                    AddressLine2 = reader[9].ToString(),
                    AddressLine3 = reader[10].ToString(),
                    AddressLine4 = reader[11].ToString(),
                    AddressLine5 = reader[12].ToString(),
                    Postcode = reader[13].ToString(),
                    Town = reader[14].ToString(),
                    AuthorityArea = reader[15].ToString(),
                    ShortAddress = reader[16].ToString(),
                    TelephoneNumber = reader[17].ToString(),
                    MobileNumber = reader[18].ToString(),
                    Email = reader[19].ToString(),
                    Unsuccessful = reader[20].ToString(),
                    Ongoing = reader[21].ToString(),
                    Withdrawn = reader[22].ToString(),
                    DateApplied = Convert.ToDateTime(reader[23]).ToString("dd/MM/yyy"),
                    VacancyClosingDate = Convert.ToDateTime(reader[24]).ToString("dd/MM/yyy"),
                    DateOfUnsuccessfulNotification = Convert.ToDateTime(reader[25]).ToString("dd/MM/yyy"),
                    LearningProvider = reader[26].ToString(),
                    LearningProviderUKPRN = reader[27].ToString(),
                    VacancyReferenceNumber = reader[28].ToString(),
                    VacancyTitle = reader[29].ToString(),
                    VacancyLevel = reader[30].ToString(),
                    Sector = reader[31].ToString(),
                    Framework = reader[32].ToString(),
                    UnsuccessfulReason = reader[33].ToString(),
                    Notes = reader[34].ToString(),
                    Points = reader[35].ToString()
                });
            }

            _logger.Debug($"Done executing report with toDate {toDate} and fromdate {fromDate}.");

            return response;
        }

        public List<ReportSuccessfulCandidatesResultItem> ReportSuccessfulCandidates(string type, DateTime fromDate, DateTime toDate, string ageRange, string managedBy,
            string region)
        {
            _logger.Debug($"Executing ReportSuccessfulCandidates report with toDate {toDate} and fromdate {fromDate}...");

            var response = new List<ReportSuccessfulCandidatesResultItem>();

            var command = new SqlCommand("dbo.ReportILRStartDateList", (SqlConnection)_getOpenConnection.GetOpenConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("Type", SqlDbType.Int).Value = type;
            command.Parameters.Add("ManagedBy", SqlDbType.VarChar).Value = managedBy;
            command.Parameters.Add("LSCRegion", SqlDbType.Int).Value = region;
            command.Parameters.Add("LocalAuthority", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Postcode", SqlDbType.VarChar).Value = "n/a";
            command.Parameters.Add("ProviderSiteID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("LSCInOut", SqlDbType.Int).Value = -1;
            command.Parameters.Add("StartDatePresent", SqlDbType.Int).Value = -1;
            command.Parameters.Add("AgeRange", SqlDbType.Int).Value = ageRange;
            command.Parameters.Add("FromDate", SqlDbType.DateTime).Value = fromDate;
            command.Parameters.Add("ToDate", SqlDbType.DateTime).Value = toDate;
            command.Parameters.Add("RecAgentID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Rowcount", SqlDbType.Int).Value = 0;
            
            command.CommandTimeout = 180;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                response.Add(new ReportSuccessfulCandidatesResultItem()
                {
                    Name = reader[0].ToString(),
                    Gender = reader[1].ToString(),
                    Postcode = reader[2].ToString(),
                    LearningProvider = reader[3].ToString(),
                    VacancyReferenceNumber = reader[4].ToString(),
                    VacancyTitle = reader[5].ToString(),
                    VacancyType = reader[6].ToString(),
                    Sector = reader[7].ToString(),
                    Framework = reader[8].ToString(),
                    FrameworkStatus = reader[9].ToString(),
                    Employer = reader[10].ToString(),
                    SuccessfulAppDate = reader[11].ToString(),
                    ILRStartDate = reader[12].ToString(),
                    ILRReference = reader[13].ToString()
                });
            }

            _logger.Debug($"Done executing report with toDate {toDate} and fromdate {fromDate}.");

            return response.OrderBy(r => r.SuccessfulAppDate).ToList();
        }

        public Dictionary<string, string> LocalAuthorityManagerGroups()
        {
            _logger.Debug($"Getting local authorities for report [dbo].[ReportGetManagedBy]...");

            var response = new Dictionary<string, string>();

            var command = new SqlCommand("dbo.ReportGetManagedBy", (SqlConnection)_getOpenConnection.GetOpenConnection());
            command.CommandType = CommandType.StoredProcedure;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                response.Add(reader[1].ToString(), reader[0].ToString());
            }

            _logger.Debug($"Done getting local authorities.");

            return response;
        }

        public Dictionary<string, string> GeoRegionsIncludingAll()
        {
            _logger.Debug($"Getting regions for report [dbo].[ReportGetGeoRegionsIncludingAll]...");

            var response = new Dictionary<string, string>();

            var command = new SqlCommand("dbo.ReportGetGeoRegionsIncludingAll", (SqlConnection)_getOpenConnection.GetOpenConnection());
            command.CommandType = CommandType.StoredProcedure;

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                response.Add(reader[1].ToString(), reader[0].ToString());
            }

            _logger.Debug($"Done getting regions.");

            return response;
        }

        public IList<ReportVacancyExtensionsResultItem> ReportVacancyExtensions(DateTime fromDate, DateTime toDate, int? providerUkprn, int? vacancyStatus)
        {
            var ukprn = providerUkprn?.ToString() ?? "ALL";
            var status = vacancyStatus?.ToString() ?? "ALL";

            _logger.Debug($"Executing vacancy extensions report with toDate {toDate} and fromdate {fromDate} for provider with ukprn {ukprn} and for vacancies with status {status}...");

            var response = new List<ReportVacancyExtensionsResultItem>();

            var command = new SqlCommand("dbo.ReportGetVacancyExtensions",
                (SqlConnection) _getOpenConnection.GetOpenConnection()) {CommandType = CommandType.StoredProcedure};

            command.Parameters.Add("startReportDateTime", SqlDbType.DateTime).Value = fromDate;
            command.Parameters.Add("endReportDateTime", SqlDbType.DateTime).Value = toDate;
            command.Parameters.Add("providerToStudyUkprn", SqlDbType.Int).Value = (object)providerUkprn ?? DBNull.Value;
            command.Parameters.Add("vacancyStatusToStudy", SqlDbType.Int).Value = (object)vacancyStatus ?? DBNull.Value;
            command.CommandTimeout = 3600;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                response.Add(new ReportVacancyExtensionsResultItem
                {
                    VacancyReferenceNumber = reader["VacancyReferenceNumber"].ToString(),
                    VacancyTitle = reader["VacancyTitle"].ToString(),
                    ProviderName = reader["ProviderName"].ToString(),
                    EmployerName = reader["EmployerName"].ToString(),
                    OriginalPostingDate = reader["OriginalPostingDate"] == DBNull.Value ? string.Empty : Convert.ToDateTime(reader["OriginalPostingDate"]).ToString("dd/MM/yyy"),
                    OriginalClosingDate = reader["OriginalClosingDate"] == DBNull.Value ? string.Empty : Convert.ToDateTime(reader["OriginalClosingDate"]).ToString("dd/MM/yyy"),
                    CurrentClosingDate = reader["CurrentClosingDate"] == DBNull.Value ? string.Empty : Convert.ToDateTime(reader["CurrentClosingDate"]).ToString("dd/MM/yyy"),
                    NumberOfVacancyExtensions = reader["NumberOfExtensions"].ToString(),
                    NumberOfSubmittedApplications = reader["NumberOfApplications"].ToString(),
                    VacancyStatus = reader["VacancyStatus"].ToString()
                });
            }

            _logger.Debug($"Done executing vacancy extensions report with toDate {toDate} and fromdate {fromDate} for provider with ukprn {ukprn} and for vacancies with status {status}.");

            return response;
        }
    }
}