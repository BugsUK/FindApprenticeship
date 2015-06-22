namespace SFA.Apprenticeships.Web.Candidate.Mediators.Home
{
    public class HomeMediatorCodes
    {
        public class GetContactMessageViewModel
        {
            public const string Successful = "HomeMediatorCodes.GetContactMessageViewModel.Successful";
        }

        public class SendContactMessage
        {
            public const string Error = "HomeMediatorCodes.SendContactMessage.Error";
            public const string SuccessfullySent = "HomeMediatorCodes.SendContactMessage.SuccessfullySent";
            public const string ValidationError = "HomeMediatorCodes.SendContactMessage.ValidationError";
        }

        public class GetFeedbackViewModel
        {
            public const string Successful = "HomeMediatorCodes.GetFeedbackViewModel.Successful";
        }

        public class SendFeedback
        {
            public const string Error = "HomeMediatorCodes.SendFeedback.Error";
            public const string SuccessfullySent = "HomeMediatorCodes.SendFeedback.SuccessfullySent";
            public const string ValidationError = "HomeMediatorCodes.SendFeedback.ValidationError";
        }
    }
}
