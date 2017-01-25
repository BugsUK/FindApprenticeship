using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbVacancySummary = SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities.VacancySummary;

namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using Common;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Queries;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;

    public class VacancySummaryRepository : IVacancySummaryRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private static readonly VacancyMappers Mapper = new VacancyMappers();
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(1);

        private const string CoreQuery = @"SELECT COUNT(*) OVER () AS TotalResultCount,
		                    v.VacancyId,
		                    v.VacancyOwnerRelationshipId,
		                    v.VacancyReferenceNumber,
		                    v.VacancyGuid,
		                    v.VacancyStatusId,
		                    v.VacancyTypeId,
		                    v.Title,
		                    v.AddressLine1,
		                    v.AddressLine2,
		                    v.AddressLine3,
		                    v.AddressLine4,
		                    v.AddressLine5,
		                    v.Town,
		                    v.CountyId,
		                    v.PostCode,
		                    v.ApplyOutsideNAVMS,
		                    v.ApplicationClosingDate,
		                    v.NoOfOfflineApplicants,
		                    CASE v.ApplyOutsideNAVMS
 			                    WHEN 1 THEN 0
			                    ELSE dbo.GetApplicantCount(v.VacancyId) 
		                    END
		                    AS ApplicantCount,
		                    CASE v.ApplyOutsideNAVMS
 			                    WHEN 1 THEN 0
			                    ELSE dbo.GetNewApplicantCount(v.VacancyId)
		                    END
		                    AS NewApplicantCount,
		                    dbo.GetFirstSubmittedDate(v.VacancyID) AS DateFirstSubmitted,
		                    dbo.GetSubmittedDate(v.VacancyID) AS DateSubmitted,
		                    dbo.GetCreatedDate(v.VacancyID) AS CreatedDate,
		                    e.FullName AS EmployerName,
		                    dbo.GetDateQAApproved(v.VacancyID) AS DateQAApproved,
		                    e.EmployerId,
		                    e.Town AS EmployerLocation,
		                    p.TradingName as ProviderTradingName,
		                    v.SubmissionCount,
		                    CASE
			                    WHEN (v.StandardId IS NULL) THEN af.CodeName
			                    ELSE NULL 
		                    END 
		                    AS FrameworkCodeName,
		                    el.CodeName AS ApprenticeshipLevel,
		                    ao.CodeName AS SectorCodeName,
		                    dbo.GetCreatedByProviderUsername(v.VacancyId) AS CreatedByProviderUsername,
		                    dbo.GetDateQAApproved(v.VacancyId) AS DateQAApproved,
		                    rt.TeamName AS RegionalTeam,
		                    COALESCE(v.StandardId, af.StandardId) AS StandardId,
                            v.StartedToQADateTime,
                            v.QAUserName,
                            v.ContractOwnerId,
                            v.ExpectedStartDate AS PossibleStartDate,
                            v.NumberOfPositions,
                            v.WageType,
                            v.WageUnitId,
                            v.WeeklyWage,
                            v.WageLowerBound,
                            v.WageUpperBound,
                            v.WageText,
                            v.HoursPerWeek,
                            v.ShortDescription,
                            v.DeliveryOrganisationId,
                            v.DurationValue AS Duration,
		                    v.ExpectedDuration,
		                    v.VacancyLocationTypeId,
		                    v.VacancyManagerId,
                            v.TrainingTypeId,
                            v.VacancyLocationTypeId,
                            v.VacancyManagerId,
                            v.WorkingWeek,  
                            v.MasterVacancyId,
                            v.Latitude,
                            v.Longitude,
                            v.GeocodeEasting,
                            v.GeocodeNorthing,
                            v.EmployerAnonymousName,
                            v.UpdatedDateTime,
                            CAST(CASE WHEN dbo.GetVacancyLocationCount(v.VacancyId) > 1 THEN 1 ELSE 0 END AS bit) AS IsMultiLocation
                    FROM	Vacancy v
                    JOIN	VacancyOwnerRelationship o
                    ON		o.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipId
                    JOIN	Employer e
                    ON		o.EmployerId = e.EmployerId
                    JOIN	Provider p
                    ON		p.ProviderID = v.ContractOwnerID
                    JOIN	ProviderSite s
                    ON      s.ProviderSiteId = v.VacancyManagerId
                    LEFT OUTER JOIN	ApprenticeshipFramework af
                    ON		af.ApprenticeshipFrameworkId = v.ApprenticeshipFrameworkId
                    LEFT OUTER JOIN	ApprenticeshipType AS at
                    ON		at.ApprenticeshipTypeId = v.ApprenticeshipType
                    LEFT OUTER JOIN	Reference.EducationLevel el
                    ON		el.EducationLevelId = at.EducationLevelId
                    LEFT OUTER JOIN	ApprenticeshipOccupation ao
                    ON		v.SectorId = ao.ApprenticeshipOccupationId
                    LEFT OUTER JOIN	RegionalTeamMappings t
                    ON		s.PostCode LIKE t.PostcodeStart + '[0-9]%'
                    LEFT OUTER JOIN	RegionalTeams rt
                    ON		rt.Id = t.RegionalTeam_Id";

        public VacancySummaryRepository(IGetOpenConnection getOpenConnection)
        {
            _getOpenConnection = getOpenConnection;
        }

        public IList<VacancySummary> GetSummariesForProvider(VacancySummaryQuery query, out int totalRecords)
        {
            var sqlParams = new
            {
                Skip = query.PageSize * (query.RequestedPage - 1),
                Take = query.PageSize,
                QueryMode = query.SearchMode,
                Query = query.SearchString,
                query.ProviderId,
                query.ProviderSiteId
            };

            string orderByField = "";
            switch (query.OrderByField)
            {
                case VacancySummaryOrderByColumn.Title:
                    orderByField = "v.Title";
                    break;
                case VacancySummaryOrderByColumn.Applications:
                    orderByField = @"(CASE v.ApplyOutsideNAVMS
 			                                WHEN 1 THEN COALESCE(v.NoOfOfflineApplicants, 0)
			                                ELSE dbo.GetApplicantCount(v.VacancyId)
		                                END)";
                    break;
                case VacancySummaryOrderByColumn.Employer:
                    orderByField = "e.FullName";
                    break;
                case VacancySummaryOrderByColumn.Location:
                    orderByField = $"v.Town {(query.Order == Order.Descending ? "DESC" : "")}, v.PostCode";
                    break;
            }

            if (query.OrderByField == VacancySummaryOrderByColumn.OrderByFilter)
            {
                switch (query.Filter)
                {
                    case VacanciesSummaryFilterTypes.ClosingSoon:
                    case VacanciesSummaryFilterTypes.NewApplications:
                        orderByField = @"v.ApplicationClosingDate, CreatedDate"; //created date
                        query.Order = Order.Descending;
                        break;
                    case VacanciesSummaryFilterTypes.Closed:
                    case VacanciesSummaryFilterTypes.Live:
                    case VacanciesSummaryFilterTypes.Completed:
                        orderByField = "e.FullName, v.Title";
                        break;
                    case VacanciesSummaryFilterTypes.All:
                    case VacanciesSummaryFilterTypes.Submitted:
                    case VacanciesSummaryFilterTypes.Rejected:
                    case VacanciesSummaryFilterTypes.Draft:
                        // Requirement is "most recently created first" (Faizal 30/6/2016).
                        // Previously there was no ordering in the code and it was coming out in natural database order
                        orderByField = "CreatedDate";
                        query.Order = Order.Descending;
                        break;
                    default:
                        throw new ArgumentException($"{query.Filter}");
                }
            }

            var filterSql = GetFilterSql(query.Filter);

            var sql = $@"{CoreQuery}
                    JOIN	ProviderSiteRelationship r
                    ON		r.ProviderSiteId = o.ProviderSiteId

                    WHERE	o.ProviderSiteID = @ProviderSiteId
                    AND		(v.VacancyManagerId = @ProviderSiteId
                            OR v.DeliveryOrganisationId = @ProviderSiteId)
                    AND		r.ProviderId = @providerId
					AND		r.ProviderSiteRelationshipTypeId = 1
                    AND     (v.VacancyTypeId = {(int)query.VacancyType} OR v.VacancyTypeId = {(int)VacancyType.Unknown})
                    --Text search
                    AND		((@query IS NULL OR @query = '')
		                    OR ((CAST(v.VacancyReferenceNumber AS VARCHAR(255)) = @query AND (@QueryMode = '{(int)VacancySearchMode.All}' OR @QueryMode = '{(int)VacancySearchMode.ReferenceNumber}'))
			                    OR (v.Title LIKE '%' + @query + '%' AND (@QueryMode = '{(int)VacancySearchMode.All}' OR @QueryMode = '{(int)VacancySearchMode.VacancyTitle}'))
			                    OR (e.FullName LIKE '%' + @query + '%' AND (@QueryMode = '{(int)VacancySearchMode.All}' OR @QueryMode = '{(int)VacancySearchMode.EmployerName}'))
                                OR (REPLACE(v.PostCode, ' ', '') LIKE REPLACE(@query, ' ', '') + '%' AND (@QueryMode = '{(int)VacancySearchMode.All}' OR @QueryMode = '{(int)VacancySearchMode.Postcode}')))
		                    )
                    AND     v.VacancyStatusId != 4
                    {filterSql}
                    ORDER BY {orderByField} {(query.Order == Order.Descending ? "DESC" : "")}
                    OFFSET (@skip) ROWS FETCH NEXT (@take) ROWS ONLY";


            var vacancies = _getOpenConnection.Query<DbVacancySummary>(sql, sqlParams);

            // return the total record count as well
            totalRecords = vacancies.Any() ? vacancies.First().TotalResultCount : 0;

            var mapped = Mapper.Map<IList<DbVacancySummary>, IList<VacancySummary>>(vacancies);

            return mapped;
        }

        public VacancyCounts GetLotteryCounts(VacancySummaryQuery query)
        {
            var sqlParams = new
            {
                query.ProviderId,
                query.ProviderSiteId,
                QueryMode = query.SearchMode,
                Query = query.SearchString
            };

            var sql = $@"SELECT
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.Live, "WHEN")} THEN 1 END) AS LiveCount,
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.Submitted, "WHEN")} THEN 1 END) AS SubmittedCount,
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.Rejected, "WHEN")} THEN 1 END) AS RejectedCount,
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.ClosingSoon, "WHEN")} THEN 1 END) AS ClosingSoonCount,
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.Closed, "WHEN")} THEN 1 END) AS ClosedCount,
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.Draft, "WHEN")} THEN 1 END) AS DraftCount,
                        SUM(dbo.GetNewApplicantCount(v.VacancyId)) AS NewApplicationsCount,
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.Completed, "WHEN")} THEN 1 END) AS CompletedCount
                        FROM	Vacancy v
                        JOIN	VacancyOwnerRelationship o
                        ON		o.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipId
                        JOIN	Employer e
                        ON		o.EmployerId = e.EmployerId
                        JOIN	ProviderSiteRelationship r
					    ON		r.ProviderSiteId = o.ProviderSiteId
                        WHERE	o.ProviderSiteID = @ProviderSiteId
                        AND		(v.VacancyManagerId = @ProviderSiteId
                        OR		v.DeliveryOrganisationId = @ProviderSiteId)
                        AND     (v.VacancyTypeId = {(int)query.VacancyType} OR v.VacancyTypeId = {(int)VacancyType.Unknown})
                        --Text search
                        AND		((@query IS NULL OR @query = '')
		                        OR ((CAST(v.VacancyReferenceNumber AS VARCHAR(255)) = @query AND (@QueryMode = '{(int)VacancySearchMode.All}' OR @QueryMode = '{(int)VacancySearchMode.ReferenceNumber}'))
			                        OR (v.Title LIKE '%' + @query + '%' AND (@QueryMode = '{(int)VacancySearchMode.All}' OR @QueryMode = '{(int)VacancySearchMode.VacancyTitle}'))
			                        OR (e.FullName LIKE '%' + @query + '%' AND (@QueryMode = '{(int)VacancySearchMode.All}' OR @QueryMode = '{(int)VacancySearchMode.EmployerName}'))
                                    OR (REPLACE(v.PostCode, ' ', '') LIKE REPLACE(@query, ' ', '') + '%' AND (@QueryMode = '{(int)VacancySearchMode.All}' OR @QueryMode = '{(int)VacancySearchMode.Postcode}')))
		                        )
                        AND		r.ProviderId = @providerId
					    AND		r.ProviderSiteRelationshipTypeId = 1
                    ";

            var counts = _getOpenConnection.Query<VacancyCounts>(sql, sqlParams);

            // only one item returns from this query
            return counts.FirstOrDefault();
        }

        public IList<VacancySummary> GetByStatus(VacancySummaryByStatusQuery query, out int totalRecords)
        {
            var sqlParams = new
            {
                Skip = query.PageSize * (query.RequestedPage - 1),
                Take = query.PageSize,
                Query = query.SearchString,
                QueryMode = query.SearchMode,
                VacancyStatuses = query.DesiredStatuses.Select(s => (int)s)
            };

            string orderByField = "";
            switch (query.OrderByField)
            {
                case VacancySummaryOrderByColumn.Title:
                    orderByField = "v.Title";
                    break;
                case VacancySummaryOrderByColumn.Provider:
                    orderByField = "p.TradingName";
                    break;
                case VacancySummaryOrderByColumn.DateSubmitted:
                    orderByField = "dbo.GetSubmittedDate(v.VacancyID)";
                    break;
                case VacancySummaryOrderByColumn.ClosingDate:
                    orderByField = "v.ApplicationClosingDate";
                    break;
                case VacancySummaryOrderByColumn.SubmissionCount:
                    orderByField = "v.SubmissionCount";
                    break;
                case VacancySummaryOrderByColumn.VacancyLocation:
                    orderByField = "v.Town";
                    break;
            }

            if (query.OrderByField == VacancySummaryOrderByColumn.OrderByFilter)
            {
                orderByField = "dbo.GetSubmittedDate(v.VacancyID)";
            }

            var filterSql = GetFilterSql(query.Filter);

            var sql = $@"{CoreQuery}

                    --Text search
                    WHERE	(((@query IS NULL OR @query = '') {(query.RegionalTeamName != RegionalTeam.Other ? $"AND rt.TeamName = '{query.RegionalTeamName}'" : "")})
		                        OR (p.TradingName LIKE '%' + @query + '%' AND (@QueryMode = '{(int)ManageVacancySearchMode.All}' OR @QueryMode = '{(int)ManageVacancySearchMode.Provider}'))
                                OR (REPLACE(v.PostCode, ' ', '') LIKE REPLACE(@query, ' ', '') + '%' AND (@QueryMode = '{(int)ManageVacancySearchMode.All}' OR @QueryMode = '{(int)ManageVacancySearchMode.VacancyPostcode}'))
		                    )
                    AND     v.VacancyStatusId IN @VacancyStatuses
                    {filterSql}
                    {(!string.IsNullOrEmpty(orderByField) ? ("ORDER BY " + orderByField + (query.Order == Order.Descending ? " DESC" : "")) : "")}
                    {(query.PageSize > 0 ? "OFFSET (@skip) ROWS FETCH NEXT (@take) ROWS ONLY" : "")}";


            var vacancies = _getOpenConnection.Query<DbVacancySummary>(sql, sqlParams);

            // return the total record count as well
            totalRecords = vacancies.Any() ? vacancies.First().TotalResultCount : 0;

            var mapped = Mapper.Map<IList<DbVacancySummary>, IList<VacancySummary>>(vacancies);

            return mapped;
        }

        public IList<RegionalTeamMetrics> GetRegionalTeamMetrics(VacancySummaryByStatusQuery query)
        {
            var sqlParams = new
            {
                VacancyStatuses = query.DesiredStatuses.Select(s => (int)s),
                query = query.SearchString,
                QueryMode = query.SearchMode,
            };

            var sql = $@"SELECT
                        rt.TeamName AS RegionalTeam,
                        COUNT(*) AS TotalCount,
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.SubmittedToday, "WHEN")} THEN 1 END) AS SubmittedTodayCount,
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.SubmittedYesterday, "WHEN")} THEN 1 END) AS SubmittedYesterdayCount,
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.SubmittedMoreThan48Hours, "WHEN")} THEN 1 END) AS SubmittedMoreThan48HoursCount,
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.Resubmitted, "WHEN")} THEN 1 END) AS ResubmittedCount
                        FROM	Vacancy v
                        JOIN	VacancyOwnerRelationship o
                        ON		o.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipId
                        JOIN	Employer e
                        ON		o.EmployerId = e.EmployerId
                        JOIN	Provider p
                        ON		p.ProviderID = v.ContractOwnerID
                        JOIN	ProviderSite s
                        ON      s.ProviderSiteId = o.ProviderSiteId
                        JOIN	RegionalTeamMappings t
                        ON		s.PostCode LIKE t.PostcodeStart + '[0-9]%'
                        JOIN	RegionalTeams rt
                        ON		rt.Id = t.RegionalTeam_Id
                        WHERE   v.VacancyStatusId IN @VacancyStatuses
                        AND  	(((@query IS NULL OR @query = ''))
		                            OR (p.TradingName LIKE '%' + @query + '%' AND (@QueryMode = '{(int)ManageVacancySearchMode.All}' OR @QueryMode = '{(int)ManageVacancySearchMode.Provider}'))
                                    OR (REPLACE(v.PostCode, ' ', '') LIKE REPLACE(@query, ' ', '') + '%' AND (@QueryMode = '{(int)ManageVacancySearchMode.All}' OR @QueryMode = '{(int)ManageVacancySearchMode.VacancyPostcode}'))
		                        )
                        GROUP BY rt.TeamName, rt.Id
                        ORDER BY rt.Id ASC
                        ";

            var counts = _getOpenConnection.Query<RegionalTeamMetrics>(sql, sqlParams).ToList();

            // fill in any blanks as the query won't return any all-zero regions
            var expectedRegions = new[]
            {
                RegionalTeam.EastMidlands,
                RegionalTeam.North,
                RegionalTeam.NorthWest,
                RegionalTeam.SouthEast,
                RegionalTeam.SouthWest,
                RegionalTeam.WestMidlands,
                RegionalTeam.YorkshireAndHumberside,
                RegionalTeam.EastAnglia
            };

            var fillItems = expectedRegions
                                .Where(w => counts.All(a => a.RegionalTeam != w))
                                .Select(s => new RegionalTeamMetrics() { RegionalTeam = s });

            counts.AddRange(fillItems);

            return counts.OrderBy(o => o.RegionalTeam).ToList();
        }

        public VacancySummary GetById(int vacancyId)
        {
            var summary = GetByIds(new[] { vacancyId });

            return summary.Any() ? summary.Single() : null;
        }

        public VacancySummary GetByReferenceNumber(int vacancyReferenceNumber)
        {
            var vacancyId = _getOpenConnection.QueryCached<int?>(_cacheDuration, VacancyRepository.SelectVacancyIdFromReferenceNumberSql,
                    new
                    {
                        vacancyReferenceNumber
                    }).SingleOrDefault();

            return vacancyId == null ? null : GetById(vacancyId.Value);
        }

        public List<VacancySummary> GetByIds(IEnumerable<int> vacancyIds)
        {
            var sqlParams = new
            {
                vacancyIds
            };

            var sql = $@"{CoreQuery}
                    WHERE	v.VacancyID IN @vacancyIds
";

            var vacancies = _getOpenConnection.Query<DbVacancySummary>(sql, sqlParams);

            var mapped = Mapper.Map<IList<DbVacancySummary>, List<VacancySummary>>(vacancies);

            return mapped;
        }

        public IList<VacancySummary> Find(ApprenticeshipVacancyQuery query, out int totalRecords)
        {
            var sqlParams = new
            {
                Skip = query.PageSize * (query.RequestedPage - 1),
                Take = query.PageSize,
                VacancyStatuses = query.DesiredStatuses.Select(s => (int)s),
                query.FrameworkCodeName,
                query.LatestClosingDate,
                query.LiveDate
            };

            var sql = $@"{CoreQuery}

                    WHERE	v.VacancyStatusId IN @VacancyStatuses
                    {(!string.IsNullOrEmpty(query.FrameworkCodeName) ? "AND     af.CodeName = @FrameworkCodeName" : "")}
                    {(query.EditedInRaa ? "AND     v.EditedInRaa = 1" : "")}
                    {(query.LiveDate.HasValue ? "AND     dbo.GetDateQAApproved(v.VacancyId) >= @LiveDate" : "")}
                    {(query.LatestClosingDate.HasValue ? "AND       v.ApplicationClosingDate <= @LatestClosingDate" : "")}
                    ORDER BY v.VacancyReferenceNumber
                    {(query.PageSize > 0 ? "OFFSET (@skip) ROWS FETCH NEXT (@take) ROWS ONLY" : "")}
            ";

            var vacancies = _getOpenConnection.Query<DbVacancySummary>(sql, sqlParams);

            // return the total record count as well
            totalRecords = vacancies.Any() ? vacancies.First().TotalResultCount : 0;

            var mapped = Mapper.Map<IList<DbVacancySummary>, List<VacancySummary>>(vacancies);

            return mapped;
        }


        private static string GetFilterSql(VacanciesSummaryFilterTypes filter, string sqlFilterKeyword = "AND")
        {
            var filterSql = new StringBuilder();
            switch (filter)
            {
                case VacanciesSummaryFilterTypes.All:
                    break;
                case VacanciesSummaryFilterTypes.Live:
                    filterSql.Append($"{sqlFilterKeyword} v.VacancyStatusId = {(int)VacancyStatus.Live}");
                    break;
                case VacanciesSummaryFilterTypes.Submitted:
                    filterSql.Append($"{sqlFilterKeyword} (v.VacancyStatusId = {(int)VacancyStatus.Submitted} OR v.VacancyStatusId = {(int)VacancyStatus.ReservedForQA})");
                    break;
                case VacanciesSummaryFilterTypes.Rejected:
                    filterSql.Append($"{sqlFilterKeyword} v.VacancyStatusId = {(int)VacancyStatus.Referred}");
                    break;
                case VacanciesSummaryFilterTypes.ClosingSoon:
                    filterSql.Append($"{sqlFilterKeyword} (v.VacancyStatusId = {(int)VacancyStatus.Live} AND v.ApplicationClosingDate >= CONVERT(DATE, GETDATE()) AND v.ApplicationClosingDate < (DATEADD(day,5,CONVERT(DATE, GETDATE()))))");
                    break;
                case VacanciesSummaryFilterTypes.Closed:
                    filterSql.Append($"{sqlFilterKeyword} v.VacancyStatusId = {(int)VacancyStatus.Closed}");
                    break;
                case VacanciesSummaryFilterTypes.Draft:
                    filterSql.Append($"{sqlFilterKeyword} v.VacancyStatusId = {(int)VacancyStatus.Draft}");
                    break;
                case VacanciesSummaryFilterTypes.NewApplications:
                    filterSql.Append($@"{sqlFilterKeyword} (CASE v.ApplyOutsideNAVMS
 			                    WHEN 1 THEN 0
			                    ELSE dbo.GetNewApplicantCount(v.VacancyId)
		                    END) > 0");
                    break;
                case VacanciesSummaryFilterTypes.Completed:
                    filterSql.Append($"{sqlFilterKeyword} v.VacancyStatusId = {(int)VacancyStatus.Completed}");
                    break;
                case VacanciesSummaryFilterTypes.SubmittedToday:
                    var todayFromDate = DateTime.Now.Date;
                    var todayToDate = DateTime.Now.AddDays(1).Date;
                    filterSql.Append($@"{sqlFilterKeyword} dbo.GetSubmittedDate(v.VacancyID) >= CAST('{todayFromDate:yyyy-MM-dd 00:00:00}' AS DATETIME)
                                        AND     dbo.GetSubmittedDate(v.VacancyID) < CAST('{todayToDate:yyyy-MM-dd 00:00:00}' AS DATETIME)");
                    break;
                case VacanciesSummaryFilterTypes.SubmittedYesterday:
                    DateTime yesterdayFromDate;
                    var yesterdayToDate = DateTime.Now.Date;

                    var dayOfWeek = (int)DateTime.Now.DayOfWeek;
                    if (dayOfWeek < 2) yesterdayFromDate = DateTime.Now.AddDays((dayOfWeek + 2) * -1);
                    else if (dayOfWeek == 2) yesterdayFromDate = DateTime.Now.AddDays(-3);
                    else yesterdayFromDate = DateTime.Now.AddDays(-1);

                    filterSql.Append($@"{sqlFilterKeyword} dbo.GetSubmittedDate(v.VacancyID) >= CAST('{yesterdayFromDate:yyyy-MM-dd 00:00:00}' AS DATETIME)
                                        AND     dbo.GetSubmittedDate(v.VacancyID) < CAST('{yesterdayToDate:yyyy-MM-dd 00:00:00}' AS DATETIME)");
                    break;
                case VacanciesSummaryFilterTypes.SubmittedMoreThan48Hours:
                    DateTime moreToDate;

                    var moreDayOfWeek = (int)DateTime.Now.DayOfWeek;
                    if (moreDayOfWeek == 0)
                        moreToDate = DateTime.Now.AddDays(-2);
                    else if (moreDayOfWeek == 2 || moreDayOfWeek == 1)
                        moreToDate = DateTime.Now.AddDays(-3);
                    else
                        moreToDate = DateTime.Now.AddDays(-1);

                    filterSql.Append($@"{sqlFilterKeyword} dbo.GetSubmittedDate(v.VacancyID) < CAST('{moreToDate:yyyy-MM-dd 00:00:00}' AS DATETIME)");
                    break;
                case VacanciesSummaryFilterTypes.Resubmitted:
                    filterSql.Append($"{sqlFilterKeyword} v.SubmissionCount > 1");
                    break;
                default: throw new ArgumentException($"{filter}");
            }

            return filterSql.ToString();
        }
    }
}
