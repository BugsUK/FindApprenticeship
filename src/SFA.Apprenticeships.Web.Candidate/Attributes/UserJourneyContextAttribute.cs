namespace SFA.Apprenticeships.Web.Candidate.Attributes
{
    using System;
    using System.Web.Mvc;
    using Common.Configuration;
    using Controllers;

    public class UserJourneyContextAttribute : ActionFilterAttribute
    {
        public const string UserJourneyKey = "UserJourney";
        private const string ApprenticeshipsMainCaption = "Find an apprenticeship";
        private const string TraineeshipsMainCaption = "Find a traineeship";

        private readonly UserJourney _userJourney;

        public UserJourneyContextAttribute(UserJourney userJourney)
        {
            _userJourney = userJourney;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var candidateController = filterContext.Controller as CandidateControllerBase;

            if (candidateController != null )
            {
                var userDataProvider = candidateController.UserData;
                var userJourneyValue = userDataProvider.Get(UserJourneyKey);
                if (userJourneyValue != null && _userJourney == UserJourney.None)
                {
                    candidateController.ViewBag.UserJourney = (UserJourney) Enum.Parse(typeof (UserJourney), userJourneyValue);
                }
                else if (userJourneyValue == null && _userJourney == UserJourney.None)
                {
                    userDataProvider.Push(UserJourneyKey, UserJourney.Apprenticeship.ToString());
                    candidateController.ViewBag.UserJourney = UserJourney.Apprenticeship;
                }
                else
                {
                    userDataProvider.Push(UserJourneyKey, _userJourney.ToString());
                    candidateController.ViewBag.UserJourney = _userJourney;
                }

                var userJourney = (UserJourney)candidateController.ViewBag.UserJourney;
                var webConfiguration = candidateController.ConfigurationService.Get<CommonWebConfiguration>();
                switch (userJourney)
                {
                    case UserJourney.Traineeship:
                        candidateController.ViewBag.FeedbackUrl = webConfiguration.TraineeshipFeedbackUrl;
                        break;
                    default:
                        candidateController.ViewBag.FeedbackUrl = webConfiguration.ApprenticeshipFeedbackUrl;
                        break;
                }

                candidateController.ViewBag.UserJourneyMainCaption = _userJourney == UserJourney.Apprenticeship
                    ? ApprenticeshipsMainCaption
                    : TraineeshipsMainCaption;
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public enum UserJourney
    {
        Apprenticeship,
        Traineeship,
        None
    }
}