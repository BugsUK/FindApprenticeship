namespace SFA.Apprenticeships.Web.Common.Constants.ViewModels
{
    using Constants;

    public static class MonitoringInformationViewModelMessages
    {
        public static class AnythingWeCanDoToSupportYourInterviewMessages
        {
            public const string LabelText = "Is there anything we can do to support your interview?";
            public const string HintText = "Provide details of interview support";
            public const string TooLongErrorText = "Interview support details must not be more than 4000 characters";
            public const string WhiteListRegularExpression = Whitelists.FreetextWhitelist.RegularExpression;
            public const string WhiteListErrorText = "Interview support details " + Whitelists.FreetextWhitelist.ErrorText;
        }
    }
}