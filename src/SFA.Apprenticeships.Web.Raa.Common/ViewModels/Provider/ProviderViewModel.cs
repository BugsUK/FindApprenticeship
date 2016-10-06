using FluentValidation.Attributes;
using SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels;
using SFA.Apprenticeships.Web.Raa.Common.Validators.Provider;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider
{
    using Domain.Entities.Users;

    [Validator(typeof(ProviderViewModelClientValidator))]
    public class ProviderViewModel
    {
        public ProviderViewModel()
        {
            ProviderSiteViewModels = new List<ProviderSiteViewModel>();
        }

        public int ProviderId { get; set; }
        [Display(Name = ProviderViewModelMessages.Ukprn.LabelText)]
        public string Ukprn { get; set; }
        [Display(Name = ProviderViewModelMessages.FullName.LabelText)]
        public string FullName { get; set; }
        [Display(Name = ProviderViewModelMessages.TradingName.LabelText)]
        public string TradingName { get; set; }
        public IEnumerable<ProviderSiteViewModel> ProviderSiteViewModels { get; set; }
        public bool IsMigrated { get; set; }
        [Display(Name = ProviderViewModelMessages.ProviderStatusType.LabelText)]
        public ProviderStatuses ProviderStatusType { get; set; }
        public string VacanciesReferenceNumbers { get; set; }
    }
}