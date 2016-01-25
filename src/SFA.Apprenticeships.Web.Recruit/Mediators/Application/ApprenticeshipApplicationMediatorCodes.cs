namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    public class ApprenticeshipApplicationMediatorCodes
    {
        public class Review
        {
            public const string Ok = "ApprenticeshipApplicationMediatorCodes.Review.Ok";
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
    }
}