namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Home
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using FluentValidation.Attributes;

    using Constants.ViewModels;
    using Validators;

    [Validator(typeof(ContactMessageClientViewModelValidator))]
    public class ContactMessageViewModel
    {
        [Display(Name = ContactMessageViewModelMessages.FullNameMessages.LabelText)]
        public string Name { get; set; }

        [Display(Name = ContactMessageViewModelMessages.EmailAddressMessages.LabelText)]
        public string Email { get; set; }

        public string Enquiry { get; set; }

        [Display(Name = ContactMessageViewModelMessages.DetailsMessages.LabelText)]
        public string Details { get; set; }                
    }
}