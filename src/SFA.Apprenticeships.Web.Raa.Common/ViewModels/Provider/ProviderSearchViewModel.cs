namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using System.ComponentModel.DataAnnotations;
    using Validators.Provider;

    [Validator(typeof(ProviderSearchViewModelClientValidator))]
    public class ProviderSearchViewModel
    {
        [Display(Name = ProviderSearchViewModelMessages.Id.LabelText)]
        public string Id { get; set; }
        [Display(Name = ProviderSearchViewModelMessages.Ukprn.LabelText)]
        public string Ukprn { get; set; }
        [Display(Name = ProviderSearchViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        public bool PerformSearch { get; set; }

        public string VacancyReferenceNumbers { get; set; }
    }
}