using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels;
using SFA.Apprenticeships.Web.Raa.Common.Validators.Provider;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    [Validator(typeof(ProviderViewModelValidator))]
    public class ProviderViewModel
    {
        public ProviderViewModel()
        {
            ProviderSiteViewModels = new List<ProviderSiteViewModel>();
        }

        public int ProviderId { get; set; }

        [Display(Name = ProviderViewModelMessages.ProviderNameMessages.LabelText)]
        public string ProviderName { get; set; }
        public IEnumerable<ProviderSiteViewModel> ProviderSiteViewModels { get; set; }
    }
}