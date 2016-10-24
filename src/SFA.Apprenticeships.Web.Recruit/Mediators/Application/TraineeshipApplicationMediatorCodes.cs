namespace SFA.Apprenticeships.Web.Recruit.Mediators.Application
{
    public class TraineeshipApplicationMediatorCodes
    {
        public class Review
        {
            public const string Ok = "TraineeshipApplicationMediatorCodes.Review.Ok";
        }
        
        public class ReviewSaveAndContinue
        {
            public const string Ok = "TraineeshipApplicationMediatorCodes.ReviewSaveAndContinue.Ok";
            public const string FailedValidation = "TraineeshipApplicationMediatorCodes.ReviewSaveAndContinue.FailedValidation";
            public const string Error = "TraineeshipApplicationMediatorCodes.ReviewSaveAndContinue.Error";
        }

        public class View
        {
            public const string Ok = "TraineeshipApplicationMediatorCodes.View.Ok";
            public const string LinkExpired = "TraineeshipApplicationMediatorCodes.View.LinkExpired";
        }
    }
}
