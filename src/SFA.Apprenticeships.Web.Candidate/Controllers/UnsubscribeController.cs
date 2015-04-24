﻿namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Common.Constants;
    using Constants;
    using Domain.Interfaces.Configuration;
    using Mediators.Unsubscribe;

    public class UnsubscribeController : CandidateControllerBase
    {
        private readonly IUnsubscribeMediator _unsubscribeMediator;

        public UnsubscribeController(
            IConfigurationService configurationService,
            IUnsubscribeMediator unsubscribeMediator)
            : base(configurationService)
        {
            _unsubscribeMediator = unsubscribeMediator;
        }

        public async Task<ActionResult> Index(Guid subscriberId, int subscriptionTypeId)
        {
            return await Task.Run<ActionResult>(() =>
            {
                var candidateId = UserContext == null ? default(Guid?) : UserContext.CandidateId;
                var response = _unsubscribeMediator.Unsubscribe(candidateId, subscriberId, subscriptionTypeId);

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