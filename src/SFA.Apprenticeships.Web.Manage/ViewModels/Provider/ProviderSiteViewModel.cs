namespace SFA.Apprenticeships.Web.Manage.ViewModels.Provider
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.Provider;

    [Validator(typeof(ProviderSiteViewModelValidator))]
    public class ProviderSiteViewModel
    {
        public string Ern { get; set; }

        [Display(Name = ProviderSiteViewModelMessages.NameMessages.LabelText)]
        public string Name { get; set; }

        [Display(Name = ProviderSiteViewModelMessages.EmailAddressMessages.LabelText)]
        public string EmailAddress { get; set; }

        [Display(Name = ProviderSiteViewModelMessages.PhoneNumberMessages.LabelText)]
        public string PhoneNumber { get; set; }
    }
}