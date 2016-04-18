namespace SFA.Apprenticeships.Web.Recruit.Mediators.Home
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
    }
}
