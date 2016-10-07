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

        public class ReviewRevertToInProgress
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ReviewRevertToInProgress.Ok";
            public const string FailedValidation = "ApprenticeshipApplicationMediatorCodes.ReviewRevertToInProgress.FailedValidation";
            public const string Error = "ApprenticeshipApplicationMediatorCodes.ReviewRevertToInProgress.Error";
        }

        public class ReviewSaveAndContinue
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.Ok";
            public const string FailedValidation = "ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.FailedValidation";
            public const string Error = "ApprenticeshipApplicationMediatorCodes.ReviewSaveAndContinue.Error";
        }

        public class PromoteToInProgress
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.Ok";
            public const string FailedValidation = "ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.FailedValidation";
            public const string Error = "ApprenticeshipApplicationMediatorCodes.PromoteToInProgress.Error";
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
            public const string NoApplicationId = "ApprenticeshipApplicationMediatorCodes.ConfirmRevertToInProgress.NoApplicationId";
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.ConfirmRevertToInProgress.Ok";
        }

        public class RevertToInProgress
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.RevertToInProgress.Ok";
        }
    }
}
