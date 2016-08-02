namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using Common;
    using Domain.Entities.Raa.Reporting;
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

        public IList<ApplicationsReceivedResultItem> GetApplicationsReceivedResultItems(DateTime dateFrom, DateTime dateTo, int providerSiteId)
        {
            _logger.Info($"Executing GetApplicationsReceivedResultItems report with dateFrom {dateFrom} and dateTo {dateTo} for providerSiteId {providerSiteId}...");

            var response = new List<ApplicationsReceivedResultItem>();

            var command = new SqlCommand("dbo.ReportApplicationsReceivedList", (SqlConnection) _getOpenConnection.GetOpenConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("ManagedBy", SqlDbType.VarChar).Value = -1;
            command.Parameters.Add("GeoRegion", SqlDbType.Int).Value = -1;
            command.Parameters.Add("LocalAuthority", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Postcode", SqlDbType.VarChar).Value = "-1";
            command.Parameters.Add("FromDate", SqlDbType.DateTime).Value = dateFrom;
            command.Parameters.Add("ToDate", SqlDbType.DateTime).Value = dateTo;
            command.Parameters.Add("Sector", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Framework", SqlDbType.Int).Value = -1;
            command.Parameters.Add("ProviderSIteID", SqlDbType.Int).Value = providerSiteId;
            command.Parameters.Add("AgeRange", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Gender", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Ethnicity", SqlDbType.Int).Value = -1;
            command.Parameters.Add("ApplicationStatus", SqlDbType.Int).Value = -1;
            command.Parameters.Add("InOrOutOfRegion", SqlDbType.Int).Value = -1;
            command.Parameters.Add("RecAgentID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("rowcount", SqlDbType.Int).Value = 0;

            command.CommandTimeout = 180;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                response.Add(new ApplicationsReceivedResultItem
                {
                    CandidateName = reader["CandidateName"].ToString(),
                    Email = reader["Email"].ToString(),
                    AddressLine1 = reader["AddressLine1"].ToString(),
                    AddressLine2 = reader["AddressLine2"].ToString(),
                    AddressLine3 = reader["AddressLine3"].ToString(),
                    AddressLine4 = reader["AddressLine4"].ToString(),
                    AddressLine5 = reader["AddressLine5"].ToString(),
                    Town = reader["Town"].ToString(),
                    County = reader["County"].ToString(),
                    Postcode = reader["Postcode"].ToString(),
                    ShortAddress = reader["ShortAddress"].ToString(),
                    CandidateTelephone = reader["CandidateTelephone"].ToString(),
                    School = reader["School"].ToString(),
                    DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]).ToString("dd/MM/yyy"),
                    EthnicOrigin = reader["EthnicOrigin"].ToString(),
                    Gender = reader["Gender"].ToString(),
                    Sector = reader["Sector"].ToString(),
                    Framework = reader["Framework"].ToString(),
                    FrameworkStatus = reader["FrameworkStatus"].ToString(),
                    Employer = reader["Employer"].ToString(),
                    VacancyPostcode = reader["VacancyPostcode"].ToString(),
                    TrainingProvider = reader["TrainingProvider"].ToString(),
                    ApplicationClosingDate = Convert.ToDateTime(reader["ApplicationClosingDate"]).ToString("dd/MM/yyy"),
                    ApplicationDate = Convert.ToDateTime(reader["ApplicationDate"]).ToString("dd/MM/yyy"),
                    ApplicationStatus = reader["ApplicationStatus"].ToString(),
                    AllocatedTo = reader["AllocatedTo"].ToString(),
                    VacancyID = reader["VacancyID"].ToString()
                });
            }

            _logger.Info($"Done executing report with dateFrom {dateFrom} and dateTo {dateTo} for providerSiteId {providerSiteId}.");

            return response;
        }

        public IList<CandidatesWithApplicationsResultItem> GetCandidatesWithApplicationsResultItems(DateTime dateFrom, DateTime dateTo, int providerSiteId)
        {
            _logger.Info($"Executing GetCandidatesWithApplicationsResultItems report with dateFrom {dateFrom} and dateTo {dateTo} for providerSiteId {providerSiteId}...");

            var response = new List<CandidatesWithApplicationsResultItem>();

            var command = new SqlCommand("dbo.ReportCandidatesWithApplicationsList", (SqlConnection)_getOpenConnection.GetOpenConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add("ManagedBy", SqlDbType.VarChar).Value = -1;
            command.Parameters.Add("GeoRegion", SqlDbType.Int).Value = -1;
            command.Parameters.Add("type", SqlDbType.Int).Value = -1;
            command.Parameters.Add("LocalAuthority", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Postcode", SqlDbType.VarChar).Value = "n/a";
            command.Parameters.Add("GenderID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("EthnicityID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("DisabilityID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("AgeRange", SqlDbType.Int).Value = -1;
            command.Parameters.Add("ProviderSiteID", SqlDbType.Int).Value = providerSiteId;
            command.Parameters.Add("ApplicationStatus", SqlDbType.Int).Value = -1;
            command.Parameters.Add("EmployerID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("FromDate", SqlDbType.DateTime).Value = dateFrom;
            command.Parameters.Add("ToDate", SqlDbType.DateTime).Value = dateTo;
            command.Parameters.Add("LSCInOut", SqlDbType.Int).Value = -1;
            command.Parameters.Add("VacancyReferenceNumber", SqlDbType.Int).Value = -1;
            command.Parameters.Add("VacancyTitle", SqlDbType.VarChar).Value = "";
            command.Parameters.Add("RecAgentID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("rowcount", SqlDbType.Int).Value = 0;

            command.CommandTimeout = 180;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                response.Add(new CandidatesWithApplicationsResultItem
                {
                    CandidateId = reader["CandidateId"].ToString(),
                    Name = reader["Name"].ToString(),
                    DateofBirth = Convert.ToDateTime(reader["DateofBirth"]).ToString("dd/MM/yyy"),
                    Gender = reader["Gender"].ToString(),
                    Email = reader["Email"].ToString(),
                    Ethnicity = reader["Ethnicity"].ToString(),
                    Disability = reader["Disability"].ToString(),
                    Postcode = reader["Postcode"].ToString(),
                    LastSchool = reader["LastSchool"].ToString(),
                    SchoolName = reader["SchoolName"].ToString(),
                    SchoolAddress1 = reader["SchoolAddress1"].ToString(),
                    SchoolAddress2 = reader["SchoolAddress2"].ToString(),
                    SchoolArea = reader["SchoolArea"].ToString(),
                    SchoolTown = reader["SchoolTown"].ToString(),
                    SchoolCounty = reader["SchoolCounty"].ToString(),
                    SchoolPostcode = reader["SchoolPostcode"].ToString(),
                    LearningProvider = reader["LearningProvider"].ToString(),
                    VacancyReferenceNumber = reader["VacancyReferenceNumber"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    MiddleNames = reader["MiddleNames"].ToString(),
                    Surname = reader["Surname"].ToString(),
                    addressLine1 = reader["addressLine1"].ToString(),
                    addressLine2 = reader["addressLine2"].ToString(),
                    addressLine3 = reader["addressLine3"].ToString(),
                    town = reader["town"].ToString(),
                    CandidateRegion = reader["CandidateRegion"].ToString(),
                    FullName = reader["FullName"].ToString(),
                    LandlineNumber = reader["LandlineNumber"].ToString(),
                    MobileNumber = reader["MobileNumber"].ToString(),
                    DateRegistered = Convert.ToDateTime(reader["DateRegistered"]).ToString("dd/MM/yyy"),
                    DateLastLoggedOn = Convert.ToDateTime(reader["DateLastLoggedOn"]).ToString("dd/MM/yyy"),
                    EmployerName = reader["EmployerName"].ToString(),
                    empAddressLine1 = reader["empAddressLine1"].ToString(),
                    empAddressLine2 = reader["empAddressLine2"].ToString(),
                    empAddressLine3 = reader["empAddressLine3"].ToString(),
                    empAddressLine4 = reader["empAddressLine4"].ToString(),
                    empAddressLine5 = reader["empAddressLine5"].ToString(),
                    empTown = reader["empTown"].ToString(),
                    empCounty = reader["empCounty"].ToString(),
                    empPostCode = reader["empPostCode"].ToString(),
                    VacancyTitle = reader["VacancyTitle"].ToString(),
                    VancacyType = reader["VancacyType"].ToString(),
                    VacancyCategory = reader["VacancyCategory"].ToString(),
                    VacancySector = reader["VacancySector"].ToString(),
                    ApplicationStatus = reader["ApplicationStatus"].ToString(),
                    NumberOfDaysApplicationAtThisStatus = reader["NumberOfDaysApplicationAtThisStatus"].ToString(),
                    ApplicationHistoryEventDate = Convert.ToDateTime(reader["ApplicationHistoryEventDate"]).ToString("dd/MM/yyy HH:mm"),
                    ApplicationStatusSetDate = Convert.ToDateTime(reader["ApplicationStatusSetDate"]).ToString("dd/MM/yyy HH:mm"),
                    VacancyClosingDate = Convert.ToDateTime(reader["VacancyClosingDate"]).ToString("dd/MM/yyy HH:mm"),
                    addressLine4 = reader["addressLine4"].ToString(),
                    addressLine5 = reader["addressLine5"].ToString(),
                    ShortAddress = reader["ShortAddress"].ToString(),
                    VacancyStatus = reader["VacancyStatus"].ToString()
                });
            }

            _logger.Info($"Done executing report with dateFrom {dateFrom} and dateTo {dateTo} for providerSiteId {providerSiteId}.");

            return response;
        }

        public InformationRadiatorData GetInformationRadiatorData()
        {
            var data = new InformationRadiatorData();

            var command = new SqlCommand(
                @"SELECT 
(SELECT COUNT(*) FROM Vacancy WHERE VacancyId < -1) as TotalVacanciesSubmittedViaRaa,
(SELECT COUNT(*) FROM Vacancy WHERE VacancyId < -1 AND VacancyStatusId = 2) as TotalVacanciesApprovedViaRaa,
(SELECT COUNT(*)
FROM [dbo].[Application] a
WHERE a.VacancyId < -1) as TotalApplicationsSubmittedForRaaVacancies,
(SELECT COUNT(*)
FROM [dbo].[Application] a
JOIN ApplicationHistory ah ON a.ApplicationId = ah.ApplicationId
WHERE a.VacancyId < -1 AND a.ApplicationStatusTypeId = 5 and ah.ApplicationHistoryEventSubTypeId = 5) as TotalUnsuccessfulApplicationsViaRaa,
(SELECT COUNT(*)
FROM [dbo].[Application] a
JOIN ApplicationHistory ah ON a.ApplicationId = ah.ApplicationId
WHERE a.VacancyId < -1 AND a.ApplicationStatusTypeId = 6 and ah.ApplicationHistoryEventSubTypeId = 6) as TotalSuccessfulApplicationsViaRaa"
                , (SqlConnection) _getOpenConnection.GetOpenConnection());

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                data = new InformationRadiatorData
                {
                    TotalVacanciesSubmittedViaRaa = Convert.ToInt32(reader["TotalVacanciesSubmittedViaRaa"]),
                    TotalVacanciesApprovedViaRaa = Convert.ToInt32(reader["TotalVacanciesApprovedViaRaa"]),
                    TotalApplicationsSubmittedForRaaVacancies = Convert.ToInt32(reader["TotalApplicationsSubmittedForRaaVacancies"]),
                    TotalUnsuccessfulApplicationsViaRaa = Convert.ToInt32(reader["TotalUnsuccessfulApplicationsViaRaa"]),
                    TotalSuccessfulApplicationsViaRaa = Convert.ToInt32(reader["TotalSuccessfulApplicationsViaRaa"]),
                };
            }

            return data;
        }

        public IList<ReportRegisteredCandidatesResultItem> ReportRegisteredCandidates(DateTime fromDate, DateTime toDate)
        {
            _logger.Debug($"Executing ReportRegisteredCandidates report with toDate {toDate} and fromdate {fromDate}...");

            var response = new List<ReportRegisteredCandidatesResultItem>();

            var command = new SqlCommand("dbo.ReportRegisteredCandidatesList", (SqlConnection)_getOpenConnection.GetOpenConnection());
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("LSCRegion", SqlDbType.Int).Value = -1;
            command.Parameters.Add("type", SqlDbType.Int).Value = -1;
            command.Parameters.Add("LocalAuthority", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Postcode", SqlDbType.VarChar).Value = "n/a";
            command.Parameters.Add("FromDate", SqlDbType.DateTime).Value = fromDate;
            command.Parameters.Add("ToDate", SqlDbType.DateTime).Value = toDate;
            command.Parameters.Add("AgeRange", SqlDbType.Int).Value = -1;
            command.Parameters.Add("IncludeDeregisteredCandidates", SqlDbType.Int).Value = 0;
            command.Parameters.Add("MarketMessagesOnly", SqlDbType.Int).Value = 0;
            command.Parameters.Add("EthnicityID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("GenderID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("ProviderSiteID", SqlDbType.Int).Value = -1;
            command.Parameters.Add("Rowcount", SqlDbType.Int).Value = 0;
            command.Parameters.Add("ManagedBy", SqlDbType.Int).Value = -1;

            command.CommandTimeout = 180;
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                response.Add(new ReportRegisteredCandidatesResultItem()
                {
                    CandidateId = Convert.ToInt32(reader["CandidateId"].ToString()),
                    Name = reader["Name"].ToString(),
                    DateofBirth = Convert.ToDateTime(reader["DateofBirth"]).ToString("dd/MM/yyy"),
                    Region = reader["Region"].ToString(),
                    AddressLine1 = reader["AddressLine1"].ToString(),
                    AddressLine2 = reader["AddressLine2"].ToString(),
                    AddressLine3 = reader["AddressLine3"].ToString(),
                    AddressLine4 = reader["AddressLine4"].ToString(),
                    AddressLine5 = reader["AddressLine5"].ToString(),
                    Town = reader["Town"].ToString(),
                    County = reader["County"].ToString(),
                    Postcode = reader["Postcode"].ToString(),
                    ShortAddress = reader["ShortAddress"].ToString(),
                    LastSchool = reader["LastSchool"].ToString(),
                    DateLastActive = Convert.ToDateTime(reader["DateLastActive"]).ToString("dd/MM/yyy"),
                    RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]).ToString("dd/MM/yyy"),
                    Address = reader["Address"].ToString(),
                    LandlineNumber = reader["LandlineNumber"].ToString(),
                    Email = reader["Email"].ToString(),
                    Gender = reader["Gender"].ToString(),
                    ApplicationCount = Convert.ToInt32(reader["ApplicationCount"].ToString()),
                    FirstName = reader["FirstName"].ToString(),
                    MiddleNames = reader["MiddleNames"].ToString(),
                    Surname = reader["Surname"].ToString(),
                    MobileNumber = reader["MobileNumber"].ToString(),
                    Ethnicity = reader["Ethnicity"].ToString(),
                    Inactive = reader["Inactive"].ToString() != "0",
                    Sector = reader["Sector"].ToString(),
                    Framework = reader["Framework"].ToString(),
                    Keyword = reader["Keyword"].ToString(),
                    CandidateStatus = reader["CandidateStatus"].ToString(),
                    DregisteredCandidate = reader["DregisteredCandidate"].ToString() != "0",
                    AllowMarketingMessages = Convert.ToBoolean(reader["AllowMarketingMessages"].ToString()) ? "Yes" : "No"
                });
            }

            _logger.Debug($"Done executing report with toDate {toDate} and fromdate {fromDate}.");

            return response.ToList();
        }
    }
}