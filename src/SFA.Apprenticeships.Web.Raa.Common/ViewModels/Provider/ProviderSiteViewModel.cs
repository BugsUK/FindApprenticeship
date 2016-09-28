namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using System.ComponentModel.DataAnnotations;
    using Web.Common.ViewModels.Locations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.Provider;

    [Validator(typeof(ProviderSiteViewModelClientValidator))]
    public class ProviderSiteViewModel
    {
        public int ProviderSiteId { get; set; }

        public int ProviderId { get; set; }

        [Display(Name = ProviderSiteViewModelMessages.EdsUrn.LabelText)]
        public string EdsUrn { get; set; }

        [Display(Name = ProviderSiteViewModelMessages.FullName.LabelText)]
        public string FullName { get; set; }

        [Display(Name = ProviderSiteViewModelMessages.TradingName.LabelText)]
        public string TradingName { get; set; }

        [Display(Name = ProviderSiteViewModelMessages.DisplayName.LabelText)]
        public string DisplayName => $"{FullName}, {Address.Town}";

        public string EmployerDescription { get; set; }

        public string CandidateDescription { get; set; }

        public string ContactDetailsForEmployer { get; set; }

        public string ContactDetailsForCandidate { get; set; }

        public AddressViewModel Address { get; set; }

        [Display(Name = ProviderSiteViewModelMessages.WebPage.LabelText)]
        public string WebPage { get; set; }
    }
}