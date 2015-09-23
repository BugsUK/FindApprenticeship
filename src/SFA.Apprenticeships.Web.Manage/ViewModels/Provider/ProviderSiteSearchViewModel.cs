namespace SFA.Apprenticeships.Web.Manage.ViewModels.Provider
{
    using FluentValidation.Attributes;
    using System.ComponentModel.DataAnnotations;
    using Validators.Provider;

    [Validator(typeof(ProviderSiteSearchViewModelValidator))]
    public class ProviderSiteSearchViewModel
    {
        public ProviderSiteSearchViewModel()
        {
            SiteSearchMode = ProviderSiteSearchMode.EmployerReferenceNumber;
        }

        public ProviderSiteSearchMode SiteSearchMode { get; set; }

        [Display(Name = ProviderSiteSearchViewModelMessages.EmployerReferenceNumberMessages.LabelText)]
        public string EmployerReferenceNumber { get; set; }
    }
}
