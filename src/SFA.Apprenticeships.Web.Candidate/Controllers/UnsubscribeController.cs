namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Application.Interfaces.Communications;
    using Common.Constants;
    using Constants;
    using SFA.Infrastructure.Interfaces;
    using Mediators.Unsubscribe;

    public class UnsubscribeController : CandidateControllerBase
    {
        private readonly IUnsubscribeMediator _unsubscribeMediator;

        public UnsubscribeController(
            IConfigurationService configurationService,
            IUnsubscribeMediator unsubscribeMediator,
            ILogService logService)
            : base(configurationService, logService)
        {
            _unsubscribeMediator = unsubscribeMediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid subscriberId, int subscriptionTypeId)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = UserContext == null ? default(Guid?) : UserContext.CandidateId;
                var response = _unsubscribeMediator.Unsubscribe(candidateId, subscriberId, (SubscriptionTypes)subscriptionTypeId);

                SetUserMessage(response.Message.Text, response.Message.Level);

                switch (response.Code)
                {
                    case UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedShowAccountSettings:
                        return RedirectToRoute(CandidateRouteNames.Settings);

                    case UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedShowSavedSearchesSettings:
                        return RedirectToRoute(CandidateRouteNames.SavedSearchesSettings);

                    case UnsubscribeMediatorCodes.Unsubscribe.UnsubscribedNotSignedIn:
                        return RedirectToRoute(RouteNames.SignIn);

                    case UnsubscribeMediatorCodes.Unsubscribe.Error:
                        break;
                }

                return RedirectToRoute(CandidateRouteNames.MyApplications);
            });
        }
    }
}