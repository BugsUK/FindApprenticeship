using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbVacancySummary = SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities.VacancySummary;

namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using Application.Interfaces;
    using Common;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories;
    using Domain.Raa.Interfaces.Repositories.Models;

    public class VacancySummaryRepository : IVacancySummaryRepository
    {
        private IGetOpenConnection _getOpenConnection;
        private IMapper _mapper;
        private IDateTimeService _dateTimeService;
        private ILogService _logger;
        private ICurrentUserService _currentUserService;
        private IConfigurationService _configurationService;

        public VacancySummaryRepository(IGetOpenConnection getOpenConnection, IMapper mapper, IDateTimeService dateTimeService,
            ILogService logger, ICurrentUserService currentUserService, IConfigurationService configurationService)
        {
            _getOpenConnection = getOpenConnection;
            _mapper = mapper;
            _dateTimeService = dateTimeService;
            _logger = logger;
            _currentUserService = currentUserService;
            _configurationService = configurationService;
        }

        public IList<VacancySummary> GetSummariesForProvider(VacancySummaryQuery query)
        {
            var sqlParams = new
            {
                Skip = query.PageSize * (query.RequestedPage - 1),
                Take = query.PageSize,
                Query = query.SearchString,
                query.ProviderId,
                query.ProviderSiteId
            };

            string orderByField = "v.Title";
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
                        orderByField = @"(CASE v.ApplyOutsideNAVMS
 			                                WHEN 1 THEN 0
			                                ELSE dbo.GetNewApplicantCount(v.VacancyId)
		                                END), CreatedDate"; //created date
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
                        break;
                    default:
                        throw new ArgumentException($"{query.Filter}");
                }
            }

            var filterSql = GetFilterSql(query.Filter);

            var sql = $@"SELECT	v.{string.Join(", v.", VacancyRepositoryResources.VacancySummaryColumns)},
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
                            dbo.GetFirstSubmittedDate(v.VacancyID) AS FirstSubmittedDate,
		                    dbo.GetSubmittedDate(v.VacancyID) AS SubmittedDate,
		                    dbo.GetCreatedDate(v.VacancyID) AS CreatedDate,
                            e.FullName AS EmployerName
                    FROM	Vacancy v
                    JOIN	VacancyOwnerRelationship o
                    ON		o.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipId
                    JOIN	Employer e
                    ON		o.EmployerId = e.EmployerId
                    WHERE	o.ProviderSiteID = @ProviderSiteId
                    AND		(v.VacancyManagerId = @ProviderSiteId
                    OR		v.DeliveryOrganisationId = @ProviderSiteId)
                    --Text search
                    AND		((@query IS NULL OR @query = '')
		                    OR (CAST(v.VacancyReferenceNumber AS VARCHAR(255)) = @query
			                    OR v.Title LIKE '%' + @query + '%'
			                    OR e.FullName LIKe '%' + @query + '%')
		                    )
                    {filterSql.ToString()}
                    ORDER BY {orderByField} {(query.Order == Order.Descending ? "DESC" : "")}
                    OFFSET (@skip) ROWS FETCH NEXT (@take) ROWS ONLY";


            var vacancies = _getOpenConnection.Query<DbVacancySummary>(sql, sqlParams);

            var mapped = _mapper.Map<IList<DbVacancySummary>, IList<VacancySummary>>(vacancies);

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
                        COUNT(CASE WHEN dbo.GetNewApplicantCount(v.VacancyId) > 0 THEN 1 END) AS NewApplicationsCount,
                        COUNT(CASE {GetFilterSql(VacanciesSummaryFilterTypes.Completed, "WHEN")} THEN 1 END) AS CompletedCount
                        FROM	Vacancy v
                        JOIN	VacancyOwnerRelationship o
                        ON		o.VacancyOwnerRelationshipId = v.VacancyOwnerRelationshipId
                        WHERE	o.ProviderSiteID = @ProviderSiteId
                        AND		(v.VacancyManagerId = @ProviderSiteId
                        OR		v.DeliveryOrganisationId = @ProviderSiteId)
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
