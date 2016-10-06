using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbVacancySummary = SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities.VacancySummary;

namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using Common;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;

    public class VacancySummaryRepository : IVacancySummaryRepository
    {
        private readonly IGetOpenConnection _getOpenConnection;
        private static readonly VacancyMappers Mapper = new VacancyMappers();

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
 			                                WHEN 1 THEN 0
			                                ELSE dbo.GetNewApplicantCount(v.VacancyId)
		                                END)";
                    break;
                case VacancySummaryOrderByColumn.Employer:
                    orderByField = "e.FullName";
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

            var sql = $@"SELECT COUNT(*) OVER () AS TotalResultCount,
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
		                    CASE v.ApplyOutsideNAVMS
 			                    WHEN 1 THEN COALESCE(v.NoOfOfflineApplicants, 0)
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
                            e.FullName AS EmployerName
                    FROM	Vacancy v
                    JOIN	VacancyOwnerRelationship o
                    ON		o.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipId
                    JOIN	Employer e
                    ON		o.EmployerId = e.EmployerId
                    JOIN	ProviderSiteRelationship s
					ON		s.ProviderSiteId = o.ProviderSiteId
                    WHERE	o.ProviderSiteID = @ProviderSiteId
                    AND		(v.VacancyManagerId = @ProviderSiteId
                            OR v.DeliveryOrganisationId = @ProviderSiteId)
                    AND		s.ProviderId = @providerId
					AND		s.ProviderSiteRelationshipTypeId = 1
                    AND     (v.VacancyTypeId = {(int)query.VacancyType} OR v.VacancyTypeId = {(int)VacancyType.Unknown})
                    --Text search
                    AND		((@query IS NULL OR @query = '')
		                    OR (CAST(v.VacancyReferenceNumber AS VARCHAR(255)) = @query
			                    OR v.Title LIKE '%' + @query + '%'
			                    OR e.FullName LIKe '%' + @query + '%')
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
                query.ProviderSiteId
            };

            var sql = $@"SELECT
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.Live,"WHEN")} THEN 1 END) AS LiveCount,
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
                        JOIN	ProviderSiteRelationship s
					    ON		s.ProviderSiteId = o.ProviderSiteId
                        WHERE	o.ProviderSiteID = @ProviderSiteId
                        AND		(v.VacancyManagerId = @ProviderSiteId
                        OR		v.DeliveryOrganisationId = @ProviderSiteId)
                        AND     (v.VacancyTypeId = {(int)query.VacancyType} OR v.VacancyTypeId = {(int)VacancyType.Unknown})
                        AND		s.ProviderId = @providerId
					    AND		s.ProviderSiteRelationshipTypeId = 1
                    ";

            var counts = _getOpenConnection.Query<VacancyCounts>(sql, sqlParams);

            // only one item returns from this query
            return counts.FirstOrDefault();
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
                    filterSql.Append($"{sqlFilterKeyword} (v.VacancyStatusId = {(int)VacancyStatus.Live} AND v.ApplicationClosingDate >= GETDATE() AND v.ApplicationClosingDate < (GETDATE() + 5))");
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
                default: throw new ArgumentException($"{filter}");
            }

            return filterSql.ToString();
        }
    }
}
