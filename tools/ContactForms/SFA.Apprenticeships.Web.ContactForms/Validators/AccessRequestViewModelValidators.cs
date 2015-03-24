namespace SFA.Apprenticeships.Web.ContactForms.Validators
{
    using Constants;
    using FluentValidation;
    using ViewModels;

    public class AccessRequestViewModelServerValidator : AbstractValidator<AccessRequestViewModel>
    {
        public AccessRequestViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    public class AccessRequestViewModelClientValidator : AbstractValidator<AccessRequestViewModel>
    {
        public AccessRequestViewModelClientValidator()
        {
            this.AddCommonRules();
        }
    }

    internal static class AccessRequestViewModelValidationRules
    {
        internal static void AddCommonRules(this AbstractValidator<AccessRequestViewModel> validator)
        {
            validator.RuleFor(x => x.Firstname)
                 .Length(0, 35)
                 .WithMessage(AccessRequestViewModelMessages.FirstnameMessages.TooLongErrorText)
                 .NotEmpty()
                 .WithMessage(AccessRequestViewModelMessages.FirstnameMessages.RequiredErrorText)
                 .Matches(AccessRequestViewModelMessages.FirstnameMessages.WhiteListRegularExpression)
                 .WithMessage(AccessRequestViewModelMessages.FirstnameMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Lastname)
                 .Length(0, 35)
                 .WithMessage(AccessRequestViewModelMessages.LastnameMessages.TooLongErrorText)
                 .NotEmpty()
                 .WithMessage(AccessRequestViewModelMessages.LastnameMessages.RequiredErrorText)
                 .Matches(AccessRequestViewModelMessages.LastnameMessages.WhiteListRegularExpression)
                 .WithMessage(AccessRequestViewModelMessages.LastnameMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Companyname)
                .Length(0, 35)
                .WithMessage(AccessRequestViewModelMessages.CompanynameMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AccessRequestViewModelMessages.CompanynameMessages.RequiredErrorText)
                .Matches(AccessRequestViewModelMessages.CompanynameMessages.WhiteListRegularExpression)
                .WithMessage(AccessRequestViewModelMessages.CompanynameMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Email)
                .Length(0, 100)
                .WithMessage(AccessRequestViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AccessRequestViewModelMessages.EmailAddressMessages.RequiredErrorText)
                .Matches(AccessRequestViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(AccessRequestViewModelMessages.EmailAddressMessages.WhiteListErrorText);

            validator.RuleFor(x => x.ConfirmEmail)
                .Length(0, 100)
                .WithMessage(AccessRequestViewModelMessages.ConfirmEmailAddressMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AccessRequestViewModelMessages.ConfirmEmailAddressMessages.RequiredErrorText)
                .Matches(AccessRequestViewModelMessages.ConfirmEmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(AccessRequestViewModelMessages.ConfirmEmailAddressMessages.WhiteListErrorText);
            
            validator.RuleFor(x => x.WorkPhoneNumber)
               .Length(8, 16)
               .WithMessage(AccessRequestViewModelMessages.WorkPhoneNumberMessages.LengthErrorText)
               .NotEmpty()
               .WithMessage(AccessRequestViewModelMessages.WorkPhoneNumberMessages.RequiredErrorText)
               .Matches(AccessRequestViewModelMessages.WorkPhoneNumberMessages.WhiteListRegularExpression)
               .WithMessage(AccessRequestViewModelMessages.WorkPhoneNumberMessages.WhiteListErrorText);

            validator.RuleFor(x => x.MobileNumber)
                .Length(8, 16)
                .WithMessage(AccessRequestViewModelMessages.MobileNumberMessages.LengthErrorText)
                .Matches(AccessRequestViewModelMessages.MobileNumberMessages.WhiteListRegularExpression)
                .WithMessage(AccessRequestViewModelMessages.MobileNumberMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Position)
                .Length(0, 35)
                .WithMessage(AccessRequestViewModelMessages.PositionMessages.TooLongErrorText)
                .NotEmpty()
                .WithMessage(AccessRequestViewModelMessages.PositionMessages.RequiredErrorText)
                .Matches(AccessRequestViewModelMessages.PositionMessages.WhiteListRegularExpression)
                .WithMessage(AccessRequestViewModelMessages.PositionMessages.WhiteListErrorText);

            validator.RuleFor(x => x.AdditionalEmail)
                .Length(0, 100)
                .WithMessage(AccessRequestViewModelMessages.EmailAddressMessages.TooLongErrorText)
                .Matches(AccessRequestViewModelMessages.EmailAddressMessages.WhiteListRegularExpression)
                .WithMessage(AccessRequestViewModelMessages.EmailAddressMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Systemname)
                .Length(0, 100)
                .WithMessage(AccessRequestViewModelMessages.SystemnameMessages.TooLongErrorText)
                .Matches(AccessRequestViewModelMessages.SystemnameMessages.WhiteListRegularExpression)
                 .WithMessage(AccessRequestViewModelMessages.SystemnameMessages.WhiteListErrorText);

            validator.RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(AccessRequestViewModelMessages.NameTitleMessages.RequiredErrorText);

            validator.RuleFor(x => x.UserType)
                .NotEmpty()
                .WithMessage(AccessRequestViewModelMessages.UserTypeMessages.RequiredErrorText);
        }

        internal static void AddServerRules(this AbstractValidator<AccessRequestViewModel> validator)
        {
            validator.RuleFor(x => x.ConfirmEmail)
                .NotEmpty()
                .WithMessage(AccessRequestViewModelMessages.ConfirmEmailAddressMessages.RequiredErrorText)
                .Equal(y => y.Email)
                .WithMessage(AccessRequestViewModelMessages.ConfirmEmailAddressMessages.ConfirmEmailNotMatchingErrorText);
        }
    }
}
