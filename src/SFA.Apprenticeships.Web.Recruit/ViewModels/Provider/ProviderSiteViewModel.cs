namespace SFA.Apprenticeships.Web.Recruit.ViewModels.Provider
{
    using System.ComponentModel.DataAnnotations;
    using Common.ViewModels.Locations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.Provider;

    [Validator(typeof(ProviderSiteViewModelValidator))]
    public class ProviderSiteViewModel
    {
        public string Ern { get; set; }

        [Display(Name = ProviderSiteViewModelMessages.NameMessages.LabelText)]
        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string EmployerDescription { get; set; }

        public string CandidateDescription { get; set; }

        public string ContactDetailsForEmployer { get; set; }

        public string ContactDetailsForCandidate { get; set; }

        public AddressViewModel Address { get; set; }
    }
}