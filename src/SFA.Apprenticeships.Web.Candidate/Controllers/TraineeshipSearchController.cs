using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ActionResults;
    using Attributes;
    using Common.Attributes;
    using Constants;
    using Domain.Entities.Vacancies;
    using SFA.Infrastructure.Interfaces;
    using Extensions;
    using FluentValidation.Mvc;
    using Mediators;
    using Mediators.Search;

    using SFA.Apprenticeships.Application.Interfaces;

    using ViewModels.VacancySearch;

    [UserJourneyContext(UserJourney.Traineeship, Order = 2)]
    public class TraineeshipSearchController : CandidateControllerBase
    {
        private readonly ITraineeshipSearchMediator _traineeshipSearchMediator;

        public TraineeshipSearchController(ITraineeshipSearchMediator traineeshipSearchMediator,
            IConfigurationService configurationService,
            ILogService logService)
            : base(configurationService, logService)
        {
            _traineeshipSearchMediator = traineeshipSearchMediator;
        }

        [HttpGet]
        public async Task<ActionResult> Overview()
        {
            return await Task.Run<ActionResult>(() => View());
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = UserContext == null ? default(Guid?) : UserContext.CandidateId;
                var response = _traineeshipSearchMediator.Index(candidateId);

                return View(response.ViewModel);
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [SessionTimeout]
        public async Task<ActionResult> Results(TraineeshipSearchViewModel model)
        {
            return await Task.Run<ActionResult>(() =>
            {
                ViewBag.SearchReturnUrl = (Request != null && Request.Url != null) ? Request.Url.PathAndQuery : null;

                var response = _traineeshipSearchMediator.Results(model);

                switch (response.Code)
                {
                    case TraineeshipSearchMediatorCodes.Results.ValidationError:
                        ModelState.Clear();
                        response.ValidationResult.AddToModelState(ModelState, string.Empty);
                        return View(response.ViewModel);

                    case TraineeshipSearchMediatorCodes.Results.HasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);

                    case TraineeshipSearchMediatorCodes.Results.Ok:
                        ModelState.Remove("Location");
                        ModelState.Remove("Latitude");
                        ModelState.Remove("Longitude");
                        return View(response.ViewModel);                        
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
                UserData.PushLastViewedVacancyId(id, VacancyType.Traineeship);

                return RedirectToRoute(CandidateRouteNames.TraineeshipDetails, new { id });
            });
        }

        [HttpGet]
        [ClearSearchReturnUrl(false)]
        [RobotsIndexPage(true)]
        [SessionTimeout]
        public async Task<ActionResult> Details(string id)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = GetCandidateId();

                var response = _traineeshipSearchMediator.Details(id, candidateId);

                switch (response.Code)
                {
                    case TraineeshipSearchMediatorCodes.Details.VacancyNotFound:
                        return new TraineeshipNotFoundResult();

                    case TraineeshipSearchMediatorCodes.Details.VacancyHasError:
                        ModelState.Clear();
                        SetUserMessage(response.Message.Text, response.Message.Level);
                        return View(response.ViewModel);

                    case TraineeshipSearchMediatorCodes.Details.Ok:
                        return View(response.ViewModel);
                }

                throw new InvalidMediatorCodeException(response.Code);
            });
        }
    }
}
