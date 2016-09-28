namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.Provider;

    [Validator(typeof(ProviderSearchViewModelClientValidator))]
    public class ProviderSearchViewModel
    {
        [Display(Name = ProviderSearchViewModelMessages.Ukprn.LabelText)]
        public string Ukprn { get; set; }
        [Display(Name = ProviderSearchViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        public bool PerformSearch { get; set; }
    }
}