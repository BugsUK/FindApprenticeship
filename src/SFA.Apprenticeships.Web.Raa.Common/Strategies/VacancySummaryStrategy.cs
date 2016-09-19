namespace SFA.Apprenticeships.Web.Raa.Common.Strategies
{
    using System;
    using System.Collections.Generic;
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
    using VacancySummary = Domain.Entities.Raa.Vacancies.VacancySummary;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    [Obsolete("This is a refactoring of the VacancyProvider and has not been fully implemented. It's existance is purely for the POC Web Api project (do not delete!)")]
    public class VacancySummaryStrategy : IVacancySummaryStrategy
    {
        private IProviderService _providerService;
        private IVacancyPostingService _vacancyPostingService;
        private readonly IDictionary<VacancyType, ICommonApplicationService> _commonApplicationService;
        private IApprenticeshipApplicationService _apprenticeshipApplicationService;
        private ITraineeshipApplicationService _traineeshipApplicationService;
        private IEmployerService _employerService;
        private IDateTimeService _dateTimeService;
        private IMapper _mapper;

        public VacancySummaryStrategy(IProviderService providerService, 
            IVacancyPostingService vacancyPostingService,
            IApprenticeshipApplicationService apprenticeshipApplicationService,
            ITraineeshipApplicationService traineeshipApplicationService,
            IEmployerService employerService,
            IDateTimeService dateTimeService,
            IMapper mapper)
        {
            _providerService = providerService;
            _vacancyPostingService = vacancyPostingService;
            _apprenticeshipApplicationService = apprenticeshipApplicationService;
            _traineeshipApplicationService = traineeshipApplicationService;
            _employerService = employerService;
            _dateTimeService = dateTimeService;
            _mapper = mapper;

            _commonApplicationService = new Dictionary<VacancyType, ICommonApplicationService>() {
                { VacancyType.Apprenticeship, apprenticeshipApplicationService },
                { VacancyType.Traineeship,    traineeshipApplicationService }
            };
        }

        public List<VacancySummaryViewModel> GetVacancySummaries(string query, int providerSiteId, int providerId, VacancyType vacancyType, VacanciesSummaryFilterTypes filter, string orderByField, Order order, int pageSize, int requestedPage)
        {
            var isVacancySearch = !string.IsNullOrEmpty(query);
            // filter?

            var vacancyParties = _providerService.GetVacancyParties(providerSiteId);

            var minimalVacancyDetails = _vacancyPostingService.GetMinimalVacancyDetails(
                    vacancyParties.Select(vp => vp.VacancyPartyId), providerId)
                .Values
                .SelectMany(a => a)
                .Where(
                    v => //(v.VacancyType == vacanciesSummarySearch.VacancyType || v.VacancyType == VacancyType.Unknown) &&
                            v.Status != VacancyStatus.Withdrawn && v.Status != VacancyStatus.Deleted);

            var hasVacancies = minimalVacancyDetails.Any();

            var vacanciesToCountNewApplicationsFor = minimalVacancyDetails.Where(v => v.Status.CanHaveApplicationsOrClickThroughs() && v.Status != VacancyStatus.Completed).Select(a => a.VacancyId);

            var applicationCountsByVacancyId = _commonApplicationService[vacancyType].GetCountsForVacancyIds(vacanciesToCountNewApplicationsFor);

            // Apply each of the filters to the vacancies

            // Get vacancies for selected filter
            var filteredVacancies = (IEnumerable<IMinimalVacancyDetails>)Filter(minimalVacancyDetails, filter, applicationCountsByVacancyId).ToList();

            var vacancyPartyIds = new HashSet<int>(filteredVacancies.Select(v => v.OwnerPartyId));
            var employers = _employerService.GetMinimalEmployerDetails(vacancyParties.Where(vp => vacancyPartyIds.Contains(vp.VacancyPartyId)).Select(vp => vp.EmployerId).Distinct(), false);
            var vacancyPartyToEmployerMap = vacancyParties.ToDictionary(vp => vp.VacancyPartyId, vp => employers.SingleOrDefault(e => e.EmployerId == vp.EmployerId));

            filteredVacancies = filteredVacancies.Select(v =>
            {
                v.EmployerName = vacancyPartyToEmployerMap.GetValue(v.OwnerPartyId).FullName; // vacancyPartyToEmployerMap[v.OwnerPartyId].Name;
                v.ApplicationOrClickThroughCount = v.OfflineVacancy.HasValue && v.OfflineVacancy.Value ? v.ApplicationOrClickThroughCount : applicationCountsByVacancyId[v.VacancyId].AllApplications;
                return v;
            });

            // try to filter as soon as we can to not get too many vacancies from DB
            if (isVacancySearch)
            {
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
            }

            var vacanciesToFetch = Sort(filteredVacancies, orderByField, filter, order).GetCurrentPage(new PageableSearchViewModel(pageSize, requestedPage)).ToList();

            var vacanciesWithoutEmployerName =
                _vacancyPostingService.GetVacancySummariesByIds(vacanciesToFetch.Select(v => v.VacancyId));

            var vacancies = Sort(vacanciesWithoutEmployerName.Select(v =>
            {
                v.EmployerName = vacancyPartyToEmployerMap.GetValue(v.OwnerPartyId).FullName;
                v.ApplicationOrClickThroughCount = v.OfflineVacancy.HasValue && v.OfflineVacancy.Value ? v.OfflineApplicationClickThroughCount : applicationCountsByVacancyId[v.VacancyId].AllApplications;
                return v;
            }), orderByField, filter, order);

            var vacancySummaries = vacancies.Select(v => _mapper.Map<VacancySummary, VacancySummaryViewModel>(v)).ToList();

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

            foreach (var vacancySummary in vacancySummaries)
            {
                vacancySummary.EmployerName = vacancyPartyToEmployerMap.GetValue(vacancySummary.OwnerPartyId).FullName;
                vacancySummary.ApplicationCount = applicationCountsByVacancyIdForPage[vacancySummary.VacancyId].AllApplications;
                vacancySummary.NewApplicationCount = applicationCountsByVacancyIdForPage[vacancySummary.VacancyId].NewApplications;
                vacancySummary.LocationAddresses = _mapper.Map<IEnumerable<VacancyLocation>, IEnumerable<VacancyLocationAddressViewModel>>(vacancyLocationsByVacancyId.GetValueOrEmpty(vacancySummary.VacancyId)).ToList();
            }

            return vacancySummaries;
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
    }

    public enum Order
    {
        Ascending = 1,
        Descending = 2
    }
}