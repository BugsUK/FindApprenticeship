namespace SFA.Apprenticeships.Web.Candidate.Mediators.Register
{
    public class RegisterMediatorCodes
    {
        public class Register
        {
            public const string ValidationFailed = "RegisterMediatorCodes.Register.ValidationFailed";
            public const string RegistrationFailed = "RegisterMediatorCodes.Register.RegistrationFailed";
            public const string SuccessfullyRegistered = "RegisterMediatorCodes.Register.SuccessfullyRegistered";
        }

        public class Activate
        {
            public const string SuccessfullyActivated = "RegisterMediatorCodes.Activate.SuccessfullyActivated";
            public const string InvalidActivationCode = "RegisterMediatorCodes.Activate.InvalidActivationCode";
            public const string FailedValidation = "RegisterMediatorCodes.Activate.FailedValidation";
            public const string ErrorActivating = "RegisterMediatorCodes.Activate.ErrorActivating";
        }

        public class UpdateMonitoringInformation
        {
            public const string SuccessfullyUpdated = "RegisterMediatorCodes.UpdateMonitoringInformation.SuccessfullyUpdated";
            public const string FailedValidation = "RegisterMediatorCodes.UpdateMonitoringInformation.FailedValidation";
            public const string ErrorUpdating = "RegisterMediatorCodes.UpdateMonitoringInformation.ErrorUpdating";
        }

        public class SendMobileVerificationCode
        {
            public const string Success = "RegisterMediatorCodes.SendMobileVerificationCode.Success";
            public const string NotRequired = "RegisterMediatorCodes.SendMobileVerificationCode.NotRequired";
            public const string Error = "RegisterMediatorCodes.SendMobileVerificationCode.Error";
        }
    }
}
