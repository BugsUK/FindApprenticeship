namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    public class ApprenticeshipApplicationMediatorCodes
    {
        public class Review
        {
            public const string NoApplicationId = "ApprenticeshipApplicationMediatorCodes.Review.NoApplicationId";
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.Review.Ok";
        }

        public class View
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.View.Ok";
            public const string LinkExpired = "ApprenticeshipApplicationMediatorCodes.View.LinkExpired";
        }

        public class ReviewAppointCandidate
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.Ok";
            public const string FailedValidation = "ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.FailedValidation";
            public const string Error = "ApprenticeshipApplicationMediatorCodes.ReviewAppointCandidate.Error";
        }

        public class ReviewRejectCandidate
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.Ok";
            public const string FailedValidation = "ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.FailedValidation";
            public const string Error = "ApprenticeshipApplicationMediatorCodes.ReviewRejectCandidate.Error";
        }

        public class ReviewRevertToViewed
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ReviewRevertToViewed.Ok";
            public const string FailedValidation = "ApprenticeshipApplicationMediatorCodes.ReviewRevertToViewed.FailedValidation";
            public const string Error = "ApprenticeshipApplicationMediatorCodes.ReviewRevertToViewed.Error";
        }

        public class ReviewSaveAndExit
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.Ok";
            public const string FailedValidation = "ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.FailedValidation";
            public const string Error = "ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.Error";
        }

        public class ConfirmSuccessfulDecision
        {
            public const string NoApplicationId = "ApprenticeshipApplicationMediatorCodes.ConfirmSuccessfulDecision.NoApplicationId";
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ConfirmSuccessfulDecision.Ok";
        }

        public class SendSuccessfulDecision
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.SendSuccessfulDecision.Ok";
        }

        public class ConfirmUnsuccessfulDecision
        {
            public const string NoApplicationId = "ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.NoApplicationId";
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.Ok";
        }

        public class SendUnsuccessfulDecision
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.SendUnsuccessfulDecision.Ok";
        }

        public class ConfirmRevertToInProgress
        {
            public const string NoApplicationId = "ApprenticeshipApplicationMediatorCodes.ConfirmRevertToViewed.NoApplicationId";
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ConfirmRevertToViewed.Ok";
        }

        public class RevertToViewed
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.RevertToViewed.Ok";
        }
    }
}
