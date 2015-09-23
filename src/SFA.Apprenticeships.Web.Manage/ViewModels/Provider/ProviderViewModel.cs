using System.Collections.Generic;

namespace SFA.Apprenticeships.Web.Manage.ViewModels.Provider
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators.Provider;

    [Validator(typeof(ProviderViewModelValidator))]
    public class ProviderViewModel
    {
        public ProviderViewModel()
        {
            ProviderSiteViewModels = new List<ProviderSiteViewModel>();
        }

        [Display(Name = ProviderViewModelMessages.ProviderNameMessages.LabelText)]
        public string ProviderName { get; set; }
        public IEnumerable<ProviderSiteViewModel> ProviderSiteViewModels { get; set; }
    }
}