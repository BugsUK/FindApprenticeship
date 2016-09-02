namespace SFA.Apprenticeships.Application.Interfaces.Communications
{
    public static class ErrorCodes
    {
        public const string EmailApiError = "Communication.EmailApiError";
        public const string EmailFormatError = "Communication.EmailFormatError";
        public const string EmailError = "Communication.EmailError";
        public const string SmsError = "Communication.SmsError";
        public const string SmsErrorInvalidMobileNumber = "Communication.SmsError.InvalidMobileNumber";
    }
}
