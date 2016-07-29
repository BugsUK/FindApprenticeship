namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    public class TraineeshipApplicationMediatorCodes
    {
        public class Review
        {
            public const string Ok = "TraineeshipApplicationMediatorCodes.Review.Ok";
        }
        
        public class ReviewSaveAndExit
        {
            public const string Ok = "TraineeshipApplicationMediatorCodes.ReviewSaveAndExit.Ok";
            public const string FailedValidation = "TraineeshipApplicationMediatorCodes.ReviewSaveAndExit.FailedValidation";
            public const string Error = "TraineeshipApplicationMediatorCodes.ReviewSaveAndExit.Error";
        }

        public class View
        {
            public const string Ok = "TraineeshipApplicationMediatorCodes.View.Ok";
            public const string LinkExpired = "TraineeshipApplicationMediatorCodes.View.LinkExpired";
        }
    }
}
