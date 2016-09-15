namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Candidates;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Applications;
    using Domain.Entities.Candidates;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Users;
    using Domain.Interfaces.Repositories;
    using Infrastructure.Presentation;
    using ViewModels.Application;
    using ViewModels.Application.Apprenticeship;
    using ViewModels.Application.Traineeship;
    using ViewModels.Candidate;
    using Application.Interfaces;
    using ViewModels;
    using Web.Common.Configuration;
    using Web.Common.ViewModels;

    public class CandidateProvider : ICandidateProvider
    {        
        private readonly CultureInfo _dateCultureInfo = new CultureInfo("en-GB");
        private readonly ICandidateSearchService _candidateSearchService;
        private readonly IMapper _mapper;
        private readonly ICandidateApplicationService _candidateApplicationService;
        private readonly IApprenticeshipApplicationService _apprenticeshipApplicationService;
        private readonly ITraineeshipApplicationService _traineeshipApplicationService;
        private readonly IVacancyPostingService _vacancyPostingService;
        private readonly IProviderService _providerService;
        private readonly IEmployerService _employerService;
        private readonly ILogService _logService;
        private readonly IConfigurationService _configurationService;

        public CandidateProvider(ICandidateSearchService candidateSearchService, IMapper mapper, ICandidateApplicationService candidateApplicationService, IApprenticeshipApplicationService apprenticeshipApplicationService, ITraineeshipApplicationService traineeshipApplicationService, IVacancyPostingService vacancyPostingService, IProviderService providerService, IEmployerService employerService, ILogService logService, IConfigurationService configurationService)
        {
            _candidateSearchService = candidateSearchService;
            _mapper = mapper;
            _candidateApplicationService = candidateApplicationService;
            _apprenticeshipApplicationService = apprenticeshipApplicationService;
            _traineeshipApplicationService = traineeshipApplicationService;
            _vacancyPostingService = vacancyPostingService;
            _providerService = providerService;
            _employerService = employerService;
            _logService = logService;
            _configurationService = configurationService;
        }

        public CandidateSearchResultsViewModel SearchCandidates(CandidateSearchViewModel searchViewModel)
        {
            var dateOfBirth = string.IsNullOrEmpty(searchViewModel.DateOfBirth) ? (DateTime?)null : DateTime.Parse(searchViewModel.DateOfBirth, _dateCultureInfo);
            var request = new CandidateSearchRequest(searchViewModel.FirstName, searchViewModel.LastName, dateOfBirth, searchViewModel.Postcode);
            var candidates = _candidateSearchService.SearchCandidates(request) ?? new List<CandidateSummary>();

            var results = new CandidateSearchResultsViewModel
            {
                SearchViewModel = searchViewModel,
                Candidates = new PageableViewModel<CandidateSummaryViewModel>
                {
                    Page =
                        candidates.OrderBy(c => c.LastName)
                            .ThenBy(c => c.FirstName)
                            .ThenBy(c => c.Address.Postcode)
                            .ThenBy(c => c.Address.AddressLine1)
                            .Skip((searchViewModel.CurrentPage - 1)*searchViewModel.PageSize)
                            .Take(searchViewModel.PageSize)
                            .Select(c => _mapper.Map<CandidateSummary, CandidateSummaryViewModel>(c))
                            .ToList(),
                    ResultsCount = candidates.Count,
                    CurrentPage = searchViewModel.CurrentPage,
                    TotalNumberOfPages =
                        candidates.Count == 0
                            ? 1
                            : (int) Math.Ceiling((double) candidates.Count/searchViewModel.PageSize)
                }
            };

            return results;
        }

        public CandidateApplicationsViewModel GetCandidateApplications(Guid candidateId)
        {
            var webSettings = _configurationService.Get<CommonWebConfiguration>();
            var domainUrl = webSettings.SiteDomainName;            
            _logService.Debug("Calling CandidateApprenticeshipApplicationProvider to get the applications for candidate ID: {0}.",
                candidateId);

            try
            {
                var candidate = _candidateApplicationService.GetCandidate(candidateId);
                var candidateName =
                    new Name(candidate.RegistrationDetails.FirstName, candidate.RegistrationDetails.MiddleNames,
                        candidate.RegistrationDetails.LastName).GetDisplayText();

                var apprenticeshipApplicationSummaries = _candidateApplicationService.GetApprenticeshipApplications(candidateId);
                var apprenticeshipApplications = apprenticeshipApplicationSummaries
                    .Select(each => new CandidateApprenticeshipApplicationViewModel(each))
                    .ToList();

                var traineeshipApplicationSummaries = _candidateApplicationService.GetTraineeshipApplications(candidateId);
                var traineeshipApplications = traineeshipApplicationSummaries
                    .Select(each => new CandidateTraineeshipApplicationViewModel
                    {
                        ApplicationId = each.ApplicationId,
                        VacancyId = each.LegacyVacancyId,
                        VacancyStatus = each.VacancyStatus,
                        Title = each.Title,
                        EmployerName = each.EmployerName,
                        IsArchived = each.IsArchived,
                        DateApplied = each.DateApplied
                    })
                    .ToList();

                return new CandidateApplicationsViewModel
                {
                    CandidateId = candidateId,
                    CandidateName = candidateName,
                    CandidateApprenticeshipApplications = apprenticeshipApplications,
                    CandidateTraineeshipApplications = traineeshipApplications,
                    NextStepUrl = string.Format($"https://{domainUrl}/nextsteps")
                };
            }
            catch (Exception e)
            {
                var message = $"Get MyApplications failed for candidate ID: {candidateId}.";

                _logService.Error(message, e);

                throw;
            }
        }

        public ApprenticeshipApplicationViewModel GetCandidateApprenticeshipApplication(Guid applicationId)
        {
            var application = _apprenticeshipApplicationService.GetApplication(applicationId);
            var viewModel = ConvertToApprenticeshipApplicationViewModel(application);

            return viewModel;
        }

        public TraineeshipApplicationViewModel GetCandidateTraineeshipApplication(Guid applicationId)
        {
            var application = _traineeshipApplicationService.GetApplication(applicationId);
            var viewModel = ConvertToTraineeshipApplicationViewModel(application);

            return viewModel;
        }

        public CandidateApplicationSummariesViewModel GetCandidateApplicationSummaries(CandidateApplicationsSearchViewModel searchViewModel)
        {
            var candidateId = searchViewModel.CandidateGuid;
            var candidate = _candidateApplicationService.GetCandidate(candidateId);

            var apprenticeshipApplicationSummaries = _mapper.Map<IEnumerable<ApprenticeshipApplicationSummary>, IEnumerable<CandidateApplicationSummaryViewModel>>(_candidateApplicationService.GetApprenticeshipApplications(candidateId));
            var traineeshipApplicationSummaries = _mapper.Map<IEnumerable<TraineeshipApplicationSummary>, IEnumerable<CandidateApplicationSummaryViewModel>>(_candidateApplicationService.GetTraineeshipApplications(candidateId));

            var candidateApplicationSummaries = apprenticeshipApplicationSummaries.Union(traineeshipApplicationSummaries);

            var page = GetOrderedApplicationSummaries(searchViewModel.OrderByField, searchViewModel.Order, candidateApplicationSummaries);

            var viewModel = new CandidateApplicationSummariesViewModel
            {
                CandidateApplicationsSearch = searchViewModel,
                ApplicantDetails = _mapper.Map<Candidate, ApplicantDetailsViewModel>(candidate),
                ApplicationSummaries = new PageableViewModel<CandidateApplicationSummaryViewModel> { Page = page.ToList() }
            };

            return viewModel;
        }

        private static IEnumerable<CandidateApplicationSummaryViewModel> GetOrderedApplicationSummaries(string orderByField, Order order, IEnumerable<CandidateApplicationSummaryViewModel> applications)
        {
            IEnumerable<CandidateApplicationSummaryViewModel> page;
            switch (orderByField)
            {
                case CandidateApplicationsSearchViewModel.OrderByFieldVacancyTitle:
                    page = order == Order.Descending
                        ? applications.OrderByDescending(a => a.VacancyTitle)
                        : applications.OrderBy(a => a.VacancyTitle);
                    break;
                case CandidateApplicationsSearchViewModel.OrderByFieldEmployer:
                    page = order == Order.Descending
                        ? applications.OrderByDescending(a => a.EmployerName)
                        : applications.OrderBy(a => a.EmployerName);
                    break;
                case CandidateApplicationsSearchViewModel.OrderByFieldSubmitted:
                    page = order == Order.Descending
                        ? applications.OrderByDescending(a => a.DateApplied)
                        : applications.OrderBy(a => a.DateApplied);
                    break;
                case CandidateApplicationsSearchViewModel.OrderByFieldStatus:
                    page = order == Order.Descending
                        ? applications.OrderByDescending(a => a.Status)
                        : applications.OrderBy(a => a.Status);
                    break;
                default:
                    page = applications;
                    break;
            }
            return page;
        }

        #region Helpers

        private ApprenticeshipApplicationViewModel ConvertToApprenticeshipApplicationViewModel(ApprenticeshipApplicationDetail application)
        {
            var vacancy = _vacancyPostingService.GetVacancy(application.Vacancy.Id);
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, false);
            var viewModel = _mapper.Map<ApprenticeshipApplicationDetail, ApprenticeshipApplicationViewModel>(application);
            viewModel.Vacancy = _mapper.Map<Vacancy, ApplicationVacancyViewModel>(vacancy);
            viewModel.Vacancy.EmployerName = employer.Name;

            return viewModel;
        }

        private TraineeshipApplicationViewModel ConvertToTraineeshipApplicationViewModel(TraineeshipApplicationDetail application)
        {
            var vacancy = _vacancyPostingService.GetVacancy(application.Vacancy.Id);
            var vacancyParty = _providerService.GetVacancyParty(vacancy.OwnerPartyId, false);  // Closed vacancies can certainly have non-current vacancy parties
            var employer = _employerService.GetEmployer(vacancyParty.EmployerId, false);
            var viewModel = _mapper.Map<TraineeshipApplicationDetail, TraineeshipApplicationViewModel>(application);
            viewModel.Vacancy = _mapper.Map<Vacancy, ApplicationVacancyViewModel>(vacancy);
            viewModel.Vacancy.EmployerName = employer.Name;

            return viewModel;
        }

        #endregion
    }
}