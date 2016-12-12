using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using ActionResults;
    using Application.Interfaces;
    using Attributes;
    using Common.Attributes;
    using Common.Constants;
    using Common.Providers;
    using Constants;
    using Constants.ViewModels;
    using Domain.Entities.Vacancies;
    using Extensions;
    using FluentValidation.Mvc;
    using Mediators.Search;
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.VacancySearch;

    [UserJourneyContext(UserJourney.Apprenticeship, Order = 2)]
    public class ApprenticeshipSearchController : CandidateControllerBase
    {
        private readonly IApprenticeshipSearchMediator _apprenticeshipSearchMediator;
        private readonly IHelpCookieProvider _helpCookieProvider;

        public ApprenticeshipSearchController(IApprenticeshipSearchMediator apprenticeshipSearchMediator, IHelpCookieProvider helpCookieProvider,
            IConfigurationService configurationService,
            ILogService logService)
            : base(configurationService, logService)
        {
            _apprenticeshipSearchMediator = apprenticeshipSearchMediator;
            _helpCookieProvider = helpCookieProvider;
        }

        [HttpGet]
        [SessionTimeout]
        public async Task<ActionResult> Index(ApprenticeshipSearchMode searchMode = ApprenticeshipSearchMode.Keyword, bool reset = false)
        {
            return await Task.Run<ActionResult>(() =>
            {
                //Originally done in PopulateSortType
                ModelState.Remove("SortType");

                var candidateId = UserContext == null ? default(Guid?) : UserContext.CandidateId;
                var response = _apprenticeshipSearchMediator.Index(candidateId, searchMode, reset);

                switch (response.Code)
                {
                    case ApprenticeshipSearchMediatorCodes.Index.Ok:
                        {
                            ViewBag.ShowSearchTour = _helpCookieProvider.ShowSearchTour(HttpContext, candidateId);
                            return View(response.ViewModel);
                        }
                }

                throw new InvalidMediatorCodeException(response.Code);

            });
        }

        [HttpPost]
        [ClearSearchReturnUrl(false)]
        public async Task<ActionResult> Index(ApprenticeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                ViewBag.SearchReturnUrl = (Request != null && Request.Url != null) ? Request.Url.PathAndQuery : null;

                var candidateId = UserContext == null ? default(Guid?) : UserContext.CandidateId;
                var response = _apprenticeshipSearchMediator.SearchValidation(candidateId, model);

                switch (response.Code)
                {
                    case ApprenticeshipSearchMediatorCodes.SearchValidation.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View("Index", response.ViewModel);
                    case ApprenticeshipSearchMediatorCodes.SearchValidation.CandidateNotLoggedIn:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipSearch);
                    case ApprenticeshipSearchMediatorCodes.SearchValidation.Ok:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipResults, model.RouteValues);
                    case ApprenticeshipSearchMediatorCodes.SearchValidation.RunSavedSearch:
                        {
                            // ReSharper disable once PossibleInvalidOperationException
                            var savedSearchResponse = _apprenticeshipSearchMediator.RunSavedSearch(candidateId.Value, model);

                            switch (savedSearchResponse.Code)
                            {
                                case ApprenticeshipSearchMediatorCodes.SavedSearch.SavedSearchNotFound:
                                case ApprenticeshipSearchMediatorCodes.SavedSearch.RunSaveSearchFailed:
                                    SetUserMessage(savedSearchResponse.Message.Text, savedSearchResponse.Message.Level);
                                    return RedirectToRoute(CandidateRouteNames.ApprenticeshipSearch);
                                case ApprenticeshipSearchMediatorCodes.SavedSearch.Ok:
                                    ModelState.Clear();
                                    return Redirect(savedSearchResponse.ViewModel.SearchUrl.Value);
                            }

                            throw new InvalidMediatorCodeException(savedSearchResponse.Code);
                        }
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [SessionTimeout]
        public async Task<ActionResult> Results(ApprenticeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                ViewBag.SearchReturnUrl = (Request != null && Request.Url != null) ? Request.Url.PathAndQuery : null;

                var candidateId = UserContext == null ? default(Guid?) : UserContext.CandidateId;
                var response = _apprenticeshipSearchMediator.Results(candidateId, model);

                switch (response.Code)
                {
                    case ApprenticeshipSearchMediatorCodes.Results.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);
                    case ApprenticeshipSearchMediatorCodes.Results.HasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);
                    case ApprenticeshipSearchMediatorCodes.Results.ExactMatchFound:
                        ViewBag.SearchReturnUrl = null;
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, response.Parameters);
                    case ApprenticeshipSearchMediatorCodes.Results.Ok:
                        ModelState.Remove("Location");
                        ModelState.Remove("Latitude");
                        ModelState.Remove("Longitude");
                        ModelState.Remove("SortType");
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [AuthorizeCandidate(Roles = UserRoleNames.Activated)]
        public async Task<ActionResult> SaveSearch(ApprenticeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = UserContext.CandidateId;
                var response = _apprenticeshipSearchMediator.SaveSearch(candidateId, model);

                switch (response.Code)
                {
                    case ApprenticeshipSearchMediatorCodes.SaveSearch.HasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return new RedirectResult(Url.ApprenticeshipSearchViewModelRouteUrl(CandidateRouteNames.ApprenticeshipResults, model));
                    case ApprenticeshipSearchMediatorCodes.SaveSearch.Ok:
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return new RedirectResult(Url.ApprenticeshipSearchViewModelRouteUrl(CandidateRouteNames.ApprenticeshipResults, model));
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        public async Task<ActionResult> DetailsWithDistance(int id, string distance)
        {
            return await Task.Run<ActionResult>(() =>
            {
                UserData.Push(CandidateDataItemNames.VacancyDistance, distance);
                UserData.PushLastViewedVacancyId(id, VacancyType.Apprenticeship);

                return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [RobotsIndexPage(true)]
        [SessionTimeout]
        public async Task<ActionResult> Details(string id)
        {
            return await Task.Run(() =>
            {
                var candidateId = GetCandidateId();

                var response = _apprenticeshipSearchMediator.Details(id, candidateId);

                return GetDetailsResult(response);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [RobotsIndexPage(true)]
        [SessionTimeout]
        public async Task<ActionResult> DetailsByReferenceNumber(string vacancyReferenceNumber)
        {
            return await Task.Run(() =>
            {
                var candidateId = GetCandidateId();

                var response = _apprenticeshipSearchMediator.DetailsByReferenceNumber(vacancyReferenceNumber, candidateId);

                return GetDetailsResult(response);
            });
        }

        private ActionResult GetDetailsResult(MediatorResponse<ApprenticeshipVacancyDetailViewModel> response)
        {
            switch (response.Code)
            {
                case ApprenticeshipSearchMediatorCodes.Details.VacancyNotFound:
                    return new ApprenticeshipNotFoundResult();
                case ApprenticeshipSearchMediatorCodes.Details.VacancyHasError:
                    ModelState.Clear();
                    SetUserMessage(response.Message.Text, response.Message.Level);
                    return View("Details", response.ViewModel);
                case ApprenticeshipSearchMediatorCodes.Details.VacancyExpired:
                    SetUserMessage(ApprenticeshipSearchViewModelMessages.DetailsMessages.ExpiredVacancy, UserMessageLevel.Warning);
                    return View("Details", response.ViewModel);
                case ApprenticeshipSearchMediatorCodes.Details.Ok:
                    return View("Details", response.ViewModel);
            }

            throw new InvalidMediatorCodeException(response.Code);
        }

        [HttpGet]
        public async Task<ActionResult> RedirectToExternalWebsite(string id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var response = _apprenticeshipSearchMediator.RedirectToExternalWebsite(id);

                switch (response.Code)
                {
                    case ApprenticeshipSearchMediatorCodes.RedirectToExternalWebsite.VacancyNotFound:
                        return new ApprenticeshipNotFoundResult();
                    case ApprenticeshipSearchMediatorCodes.RedirectToExternalWebsite.VacancyHasError:
                        return RedirectToRoute(CandidateRouteNames.ApprenticeshipDetails, new { id });
                    case ApprenticeshipSearchMediatorCodes.RedirectToExternalWebsite.Ok:
                        return new RedirectResult(response.ViewModel.VacancyUrl);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }
    }
}