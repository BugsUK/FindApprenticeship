namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using System.ComponentModel.DataAnnotations;
    using Web.Common.ViewModels.Locations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.Provider;

    [Validator(typeof(ProviderSiteViewModelValidator))]
    public class ProviderSiteViewModel
    {
        public string Ern { get; set; }

        public string Name { get; set; }

        [Display(Name = ProviderSiteViewModelMessages.NameMessages.LabelText)]
        public string DisplayName => $"{Name}, {Address.AddressLine4}";

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string EmployerDescription { get; set; }

        public string CandidateDescription { get; set; }

        public string ContactDetailsForEmployer { get; set; }

        public string ContactDetailsForCandidate { get; set; }

        public AddressViewModel Address { get; set; }
    }
}