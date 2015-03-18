namespace SFA.Apprenticeships.Web.ContactForms.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using FluentValidation.Attributes;
    using Constants;
    using Validators;

    [Validator(typeof(AccessRequestViewModelClientValidator))]
    public class AccessRequestViewModel
    {
        [Display(Name = AccessRequestViewModelMessages.UserTypeMessages.LabelText)]
        public string UserType { get; set; }
        public SelectList UserTypeList { get; set; }
        [Display(Name = AccessRequestViewModelMessages.NameTitleMessages.LabelText)]
        public string Title { get; set; }
        public SelectList TitleList { get; set; }
        [Display(Name = AccessRequestViewModelMessages.EmailAddressMessages.LabelText)]
        public string Email { get; set; }
        [Display(Name = AccessRequestViewModelMessages.ConfirmEmailAddressMessages.LabelText)]
        public string ConfirmEmail { get; set; }
        [Display(Name = AccessRequestViewModelMessages.FirstnameMessages.LabelText)]
        public string Firstname { get; set; }
        [Display(Name = AccessRequestViewModelMessages.LastnameMessages.LabelText)]
        public string Lastname { get; set; }
        [Display(Name = AccessRequestViewModelMessages.CompanynameMessages.LabelText)]
        public string Companyname { get; set; }
        [Display(Name = AccessRequestViewModelMessages.PositionMessages.LabelText)]
        public string Position { get; set; }
        [Display(Name = AccessRequestViewModelMessages.WorkPhoneNumberMessages.LabelText, Description = AccessRequestViewModelMessages.WorkPhoneNumberMessages.HintText)]
        public string WorkPhoneNumber { get; set; }
        [Display(Name = AccessRequestViewModelMessages.MobileNumberMessages.LabelText)]
        public string MobileNumber { get; set; }
        [Display(Name = AccessRequestViewModelMessages.HasVacanciesAdvertisedMessages.LabelText)]
        public bool HasApprenticeshipVacancies { get; set; }
        public AddressViewModel Address { get; set; }
        [Display(Name = AccessRequestViewModelMessages.AccessRequestServicesMessages.LabelText)]
        public AccessRequestServicesViewModel ServiceTypes { get; set; }
        [Display(Name = AccessRequestViewModelMessages.ContactnameMessages.LabelText)]
        public string Contactname { get; set; }
        [Display(Name = AccessRequestViewModelMessages.AdditionalMobileNumberMessages.LabelText)]
        public string AdditionalPhoneNumber { get; set; }
        [Display(Name = AccessRequestViewModelMessages.AdditionalEmailEmailMessages.LabelText)]
        public string AdditionalEmail { get; set; }
        [Display(Name = AccessRequestViewModelMessages.SystemnameMessages.LabelText)]
        public string Systemname { get; set; }

    }
}