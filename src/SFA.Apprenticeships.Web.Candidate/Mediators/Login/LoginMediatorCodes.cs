﻿namespace SFA.Apprenticeships.Web.Candidate.Mediators.Login
{
    public class LoginMediatorCodes
    {
        public class Index
        {
            public const string ValidationError = "Login.Index.ValidationError";
            public const string AccountLocked = "Login.Index.AccountLocked";
            public const string PendingActivation = "Login.Index.PendingActivation";
            public const string ReturnUrl = "Login.Index.ReturnUrl";
            public const string ApprenticeshipApply = "Login.Index.ApprenticeshipApply";
            public const string ApprenticeshipDetails = "Login.Index.ApprenticeshipDetails";
            public const string LoginFailed = "Login.Index.LoginFailed";
            public const string TermsAndConditionsNeedAccepted = "Login.Index.TermsAndConditionsNeedAccepted";
            public const string Ok = "Login.Index.Ok";
        }

        public class Unlock
        {
            public const string ValidationError = "Login.Unlock.ValidationError";
            public const string UnlockedSuccessfully = "Login.Unlock.Successfully";
            public const string UserInIncorrectState = "Login.Unlock.UserInIncorrectState";
            public const string AccountEmailAddressOrUnlockCodeInvalid = "Login.Unlock.AccountEmailAddressOrUnlockCodeInvalid";
            public const string AccountUnlockCodeExpired = "Login.Unlock.UnlockCodeExpired";
            public const string AccountUnlockFailed = "Login.Unlock.Failed";
        }

        public class Resend
        {
            public const string ValidationError = "Login.Resend.ValidationError";
            public const string ResentSuccessfully = "Login.Resend.Successfully";
            public const string ResendFailed = "Login.Resend.Failed";
        }

        public class ForgottenPassword
        {
            public const string FailedToSendResetCode = "RegisterMediatorCodes.ForgottenPassword.FailedToSendResetCode";
            public const string PasswordSent = "RegisterMediatorCodes.ForgottenPassword.PasswordSent";
            public const string FailedValidation = "RegisterMediatorCodes.ForgottenPassword.FailedValidation";
        }

        public class ResetPassword
        {
            public const string FailedValidation = "RegisterMediatorCodes.ResetPassword.FailedValidation";
            public const string InvalidResetCode = "RegisterMediatorCodes.ResetPassword.InvalidResetCode";
            public const string FailedToResetPassword = "RegisterMediatorCodes.ResetPassword.FailedToResetPassword";
            public const string UserAccountLocked = "RegisterMediatorCodes.ResetPassword.UserAccountLocked";
            public const string SuccessfullyResetPassword = "RegisterMediatorCodes.ResetPassword.SuccessfullyResetPassword";
        }

        public class ForgottenEmail
        {
            public const string FailedValidation = "RegisterMediatorCodes.ForgottenEmail.FailedValidation";
            public const string FailedToSendEmail = "RegisterMediatorCodes.ForgottenEmail.FailedToSendEmail";
            public const string EmailSent = "RegisterMediatorCodes.ForgottenEmail.EmailSent";
        }
    }
}
