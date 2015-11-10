using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels;
using SFA.Apprenticeships.Web.Recruit.Validators.ProviderUser;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser
{
    [Validator(typeof(VerifyEmailViewModelValidator))]
    public class VerifyEmailViewModel
    {
        [Display(Name = VerifyEmailViewModelMessages.VerificationCode.LabelText)]
        public string VerificationCode { get; set; }

        public string EmailAddress { get; set; }
    }
}