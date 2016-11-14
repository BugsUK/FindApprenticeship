namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.Provider;

    [Validator(typeof(ProviderSiteSearchViewModelClientValidator))]
    public class ProviderSiteSearchViewModel
    {
        [Display(Name = ProviderSiteSearchViewModelMessages.Id.LabelText)]
        public string Id { get; set; }
        [Display(Name = ProviderSiteSearchViewModelMessages.EdsUrn.LabelText)]
        public string EdsUrn { get; set; }
        [Display(Name = ProviderSiteSearchViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        public bool PerformSearch { get; set; }
    }
}