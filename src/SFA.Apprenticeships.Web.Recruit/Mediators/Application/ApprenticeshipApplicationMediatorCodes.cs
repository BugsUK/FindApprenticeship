namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    public class ApprenticeshipApplicationMediatorCodes
    {
        public class Review
        {
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

        public class ReviewSaveAndExit
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.Ok";
            public const string FailedValidation = "ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.FailedValidation";
            public const string Error = "ApprenticeshipApplicationMediatorCodes.ReviewSaveAndExit.Error";
        }

        public class ConfirmSuccessfulDecision
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ConfirmSuccessfulDecision.Ok";
        }

        public class SendSuccessfulDecision
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.SendSuccessfulDecision.Ok";
        }

        public class ConfirmUnsuccessfulDecision
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ConfirmUnsuccessfulDecision.Ok";
        }

        public class SendUnsuccessfulDecision
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.SendUnsuccessfulDecision.Ok";
        }
    }
}
