namespace SFA.Apprenticeships.Web.Raa.Common.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Application.Interfaces;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Infrastructure.Presentation;
    using Providers;
    using ViewModels;
    using ViewModels.ProviderUser;
    using ViewModels.Vacancy;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels;
    using VacancySummary = Domain.Entities.Raa.Vacancies.VacancySummary;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    //[Obsolete("This is a refactoring of the VacancyProvider and has not been fully implemented. It's existance is purely for the POC Web Api project (do not delete!)")]
    public class VacancySummaryStrategy : IVacancySummaryStrategy
    {
        private readonly IProviderService _providerService;
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IDictionary<VacancyType, ICommonApplicationService> _commonApplicationService;
        private readonly IEmployerService _employerService;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;

        public VacancySummaryStrategy(IProviderService providerService,
            IVacancyPostingService vacancyPostingService,
            IApprenticeshipApplicationService apprenticeshipApplicationService,
            ITraineeshipApplicationService traineeshipApplicationService,
            IEmployerService employerService,
            IDateTimeService dateTimeService,
            IMapper mapper,
            ILogService logService)
        {
            _providerService = providerService;
            _vacancyPostingService = vacancyPostingService;
            _employerService = employerService;
            _dateTimeService = dateTimeService;
            _mapper = mapper;
            _logService = logService;

            _commonApplicationService = new Dictionary<VacancyType, ICommonApplicationService>() {
                { VacancyType.Apprenticeship, apprenticeshipApplicationService },
                { VacancyType.Traineeship,    traineeshipApplicationService }
            };
        }

        public PageableViewModel<VacancySummaryViewModel> GetVacancySummaries(string query, int providerSiteId, int providerId, VacancyType vacancyType, VacanciesSummaryFilterTypes filter, string orderByField, Order order, int pageSize, int requestedPage)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var isVacancySearch = !string.IsNullOrEmpty(query);
            if (isVacancySearch)
            {
                //When searching, the ﬁlters (lottery numbers) are ignored and the search applies to all vacancies
                filter = VacanciesSummaryFilterTypes.All;
            }

            // Although the most straightforward thing to do would be to get by Vacancy.VacancyManagerId, this is sometimes null.
            // TODO: It may be that we should (and AVMS did) try Vacancy.VacancyManagerId first and then fall back to VacancyParty (aka VacancyOwnerRelationship)
            var vacancyParties = _providerService.GetVacancyParties(providerSiteId);
            _logService.Info($"Retrieved vacancy parties {stopwatch.ElapsedMilliseconds}ms elapsed");

            var ownedProviderSites = _providerService.GetOwnedProviderSites(providerId);

            var minimalVacancyDetails = _vacancyPostingService.GetMinimalVacancyDetails(
                vacancyParties.Select(vp => vp.VacancyPartyId), providerId, ownedProviderSites.Select(ps => ps.ProviderSiteId))
                .Values
                .SelectMany(a => a)
                .Where(
                    v => (v.VacancyType == vacancyType || v.VacancyType == VacancyType.Unknown)
                         && v.Status != VacancyStatus.Withdrawn && v.Status != VacancyStatus.Deleted);
            _logService.Info($"Retrieved minimal vacancy details {stopwatch.ElapsedMilliseconds}ms elapsed");

           var vacanciesToCountNewApplicationsFor = minimalVacancyDetails.Where(v => v.OfflineVacancy == false).Select(a => a.VacancyId);

            var applicationCountsByVacancyId = _commonApplicationService[vacancyType].GetCountsForVacancyIds(vacanciesToCountNewApplicationsFor);
            _logService.Info($"Retrieved application counts {stopwatch.ElapsedMilliseconds}ms elapsed");

            // Apply each of the filters to the vacancies

            // Get vacancies for selected filter
            var filteredVacancies = (IEnumerable<IMinimalVacancyDetails>)Filter(minimalVacancyDetails, filter, applicationCountsByVacancyId).ToList();
            _logService.Info($"Filtered vacancies {stopwatch.ElapsedMilliseconds}ms elapsed");

            var vacancyPartyIds = new HashSet<int>(filteredVacancies.Select(v => v.OwnerPartyId));
            var employers = _employerService.GetMinimalEmployerDetails(vacancyParties.Where(vp => vacancyPartyIds.Contains(vp.VacancyPartyId)).Select(vp => vp.EmployerId).Distinct(), false);
            var vacancyPartyToEmployerMap = vacancyParties.ToDictionary(vp => vp.VacancyPartyId, vp => employers.SingleOrDefault(e => e.EmployerId == vp.EmployerId));
            _logService.Info($"Retrieved employers {stopwatch.ElapsedMilliseconds}ms elapsed");

            filteredVacancies = filteredVacancies.Select(v =>
            {
                v.EmployerName = vacancyPartyToEmployerMap.GetValue(v.OwnerPartyId).FullName; // vacancyPartyToEmployerMap[v.OwnerPartyId].Name;
                v.ApplicationOrClickThroughCount = v.OfflineVacancy.HasValue && v.OfflineVacancy.Value ? v.ApplicationOrClickThroughCount : applicationCountsByVacancyId[v.VacancyId].AllApplications;
                return v;
            });

            // try to filter as soon as we can to not get too many vacancies from DB
            if (isVacancySearch)
            {
                _logService.Info($"Running vacancy search {stopwatch.ElapsedMilliseconds}ms elapsed");
                // If doing a search then all the vacancies have been fetched and after filtering need to be cut down to the current page

                string vacancyReference;
                if (VacancyHelper.TryGetVacancyReference(query, out vacancyReference))
                {
                    var vacancyReferenceNumber = int.Parse(vacancyReference);
                    filteredVacancies = filteredVacancies.Where(v => v.VacancyReferenceNumber == vacancyReferenceNumber);
                }
                else
                {
                    filteredVacancies = filteredVacancies.Where(v =>
                        (!string.IsNullOrEmpty(v.Title) && v.Title.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                        v.EmployerName.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0
                    );
                }
                _logService.Info($"Vacancy search completed {stopwatch.ElapsedMilliseconds}ms elapsed");
            }

            var vacanciesToFetch = GetCurrentPage(Sort(filteredVacancies, orderByField, filter, order), pageSize, requestedPage).ToList();
            _logService.Info($"Sorted vacancies {stopwatch.ElapsedMilliseconds}ms elapsed");

            var vacanciesWithoutEmployerName =
                _vacancyPostingService.GetVacancySummariesByIds(vacanciesToFetch.Select(v => v.VacancyId));
            _logService.Info($"Retrieved vacancy summaries {stopwatch.ElapsedMilliseconds}ms elapsed");

            var vacancies = Sort(vacanciesWithoutEmployerName.Select(v =>
            {
                v.EmployerName = vacancyPartyToEmployerMap.GetValue(v.OwnerPartyId).FullName;
                v.ApplicationOrClickThroughCount = v.OfflineVacancy.HasValue && v.OfflineVacancy.Value ? v.OfflineApplicationClickThroughCount : applicationCountsByVacancyId[v.VacancyId].AllApplications;
                return v;
            }), orderByField, filter, order);

            var vacancySummaries = vacancies.Select(v => _mapper.Map<VacancySummary, VacancySummaryViewModel>(v)).ToList();
            _logService.Info($"Mapped vacancy summaries {stopwatch.ElapsedMilliseconds}ms elapsed");

            var applicationCountsByVacancyIdForPage = applicationCountsByVacancyId;

            if (isVacancySearch || filter == VacanciesSummaryFilterTypes.All || filter == VacanciesSummaryFilterTypes.Completed)
            {
                //Get counts again but just for the page of vacancies
                applicationCountsByVacancyIdForPage =
                    _commonApplicationService[vacancyType].GetCountsForVacancyIds(
                        vacancySummaries.Where(v => v.Status.CanHaveApplicationsOrClickThroughs())
                            .Select(a => a.VacancyId));
            }

            var vacancyLocationsByVacancyId = _vacancyPostingService.GetVacancyLocationsByVacancyIds(vacancyPartyIds);
            _logService.Info($"Retrieved vacancy locations {stopwatch.ElapsedMilliseconds}ms elapsed");

            foreach (var vacancySummary in vacancySummaries)
            {
                vacancySummary.EmployerName = vacancyPartyToEmployerMap.GetValue(vacancySummary.OwnerPartyId).FullName;
                vacancySummary.ApplicationCount = applicationCountsByVacancyIdForPage[vacancySummary.VacancyId].AllApplications;
                vacancySummary.NewApplicationCount = applicationCountsByVacancyIdForPage[vacancySummary.VacancyId].NewApplications;
                vacancySummary.LocationAddresses = _mapper.Map<IEnumerable<VacancyLocation>, IEnumerable<VacancyLocationAddressViewModel>>(vacancyLocationsByVacancyId.GetValueOrEmpty(vacancySummary.VacancyId)).ToList();
            }

            var vacancyPage = new PageableViewModel<VacancySummaryViewModel>
            {
                Page = vacancySummaries,
                ResultsCount = vacancySummaries.Count,
                CurrentPage = requestedPage,
                TotalNumberOfPages = TotalPages(filteredVacancies, pageSize)
            };

            _logService.Info($"Complete {stopwatch.ElapsedMilliseconds}ms elapsed");

            return vacancyPage;
        }

        private IEnumerable<T> Filter<T>(IEnumerable<T> data, VacanciesSummaryFilterTypes vacanciesSummaryFilterType, IReadOnlyDictionary<int, IApplicationCounts> applicationCountsByVacancyId) where T : IMinimalVacancyDetails
        {
            switch (vacanciesSummaryFilterType)
            {
                case VacanciesSummaryFilterTypes.All: return data;
                case VacanciesSummaryFilterTypes.Live: return data.Where(v => v.Status == VacancyStatus.Live);
                case VacanciesSummaryFilterTypes.Submitted: return data.Where(v => v.Status.EqualsAnyOf(VacancyStatus.Submitted, VacancyStatus.ReservedForQA));
                case VacanciesSummaryFilterTypes.Rejected: return data.Where(v => v.Status == VacancyStatus.Referred);
                case VacanciesSummaryFilterTypes.ClosingSoon:
                    return data.Where(v =>
                        v.Status == VacancyStatus.Live &&
                        v.LiveClosingDate >= _dateTimeService.UtcNow.Date &&
                        v.LiveClosingDate.AddDays(-5) < _dateTimeService.UtcNow);
                case VacanciesSummaryFilterTypes.Closed: return data.Where(v => v.Status == VacancyStatus.Closed);
                case VacanciesSummaryFilterTypes.Draft: return data.Where(v => v.Status == VacancyStatus.Draft);
                case VacanciesSummaryFilterTypes.NewApplications:
                    return data.Where(v => applicationCountsByVacancyId[v.VacancyId].NewApplications > 0);
                case VacanciesSummaryFilterTypes.Completed: return data.Where(v => v.Status == VacancyStatus.Completed);
                default: throw new ArgumentException($"{vacanciesSummaryFilterType}");
            }
        }

        private IEnumerable<T> Sort<T>(IEnumerable<T> data, string orderByField, VacanciesSummaryFilterTypes filterType, Order order) where T : IMinimalVacancyDetails
        {
            if (string.IsNullOrEmpty(orderByField))
            {
                switch (filterType)
                {
                    case VacanciesSummaryFilterTypes.ClosingSoon:
                    case VacanciesSummaryFilterTypes.NewApplications:
                        return data.OrderBy(v => v.LiveClosingDate).ThenByDescending(v => v.VacancyId < 0 ? 1000000 - v.VacancyId : v.VacancyId);
                    case VacanciesSummaryFilterTypes.Closed:
                    case VacanciesSummaryFilterTypes.Live:
                    case VacanciesSummaryFilterTypes.Completed:
                        return data.OrderBy(v => v.EmployerName).ThenBy(v => v.Title);
                    case VacanciesSummaryFilterTypes.All:
                    case VacanciesSummaryFilterTypes.Submitted:
                    case VacanciesSummaryFilterTypes.Rejected:
                    case VacanciesSummaryFilterTypes.Draft:
                        // Requirement is "most recently created first" (Faizal 30/6/2016).
                        // Previously there was no ordering in the code and it was coming out in natural database order
                        return data.OrderByDescending(v => v.VacancyId < 0 ? 1000000 - v.VacancyId : v.VacancyId);
                    default:
                        throw new ArgumentException($"{filterType}");
                }
            }

            switch (orderByField)
            {
                case VacanciesSummarySearchViewModel.OrderByFieldTitle:
                    return order == Order.Descending ? data.OrderByDescending(v => v.Title) : data.OrderBy(v => v.Title);
                case VacanciesSummarySearchViewModel.OrderByEmployer:
                    return order == Order.Descending ? data.OrderByDescending(v => v.EmployerName) : data.OrderBy(v => v.EmployerName);
                case VacanciesSummarySearchViewModel.OrderByApplications:
                    return order == Order.Descending ? data.OrderByDescending(v => v.ApplicationOrClickThroughCount) : data.OrderBy(v => v.ApplicationOrClickThroughCount);
                default:
                    throw new ArgumentException($"{orderByField}");
            }
        }

        private int TotalPages<T>(IEnumerable<T> enumerable, int pageSize)
        {
            // TODO: This looks overly complicated
            return enumerable.Any() ? (int)Math.Ceiling((double)enumerable.Count() / pageSize) : 1;
        }

        private IEnumerable<T> GetCurrentPage<T>(IEnumerable<T> enumerable, int pageSize, int currentPage)
        {
            return enumerable.Skip((currentPage - 1) * pageSize).Take(pageSize);
        }
    }
}
