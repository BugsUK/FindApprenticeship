using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using SFA.Apprenticeships.Web.Raa.Common.Validators.Provider;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
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
