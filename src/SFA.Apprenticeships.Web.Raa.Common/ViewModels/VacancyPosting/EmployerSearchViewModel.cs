using SFA.Apprenticeships.Web.Raa.Common.Constants.ViewModels;

namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Web.Common.ViewModels;

    public enum EmployerFilterType
    {
        Undefined,
        EdsErn,
        NameAndLocation
    }

    public class EmployerSearchViewModel
    {
        public EmployerSearchViewModel()
        {
            EmployerResultsPage = new PageableViewModel<EmployerResultViewModel> { CurrentPage = 1 };
        }

        public EmployerSearchViewModel(EmployerSearchViewModel viewModel) : this()
        {
            ProviderSiteId = viewModel.ProviderSiteId;
            FilterType = viewModel.FilterType;
            EdsErn = viewModel.EdsErn;
            Name = viewModel.Name;
            Location = viewModel.Location;
            if (viewModel.EmployerResultsPage != null)
            {
                EmployerResultsPage.CurrentPage = viewModel.EmployerResultsPage.CurrentPage;
            }
            VacancyGuid = viewModel.VacancyGuid;
            ComeFromPreview = viewModel.ComeFromPreview;
        }

        public int ProviderSiteId { get; set; }

        public EmployerFilterType FilterType { get; set; }

        [Display(Name = EmployerSearchViewModelMessages.Ern.LabelText)]
        public string EdsErn { get; set; }

        [Display(Name = EmployerSearchViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        [Display(Name = EmployerSearchViewModelMessages.Location.LabelText)]
        public string Location { get; set; }

        //TODO: Merge these properties
        public IEnumerable<EmployerResultViewModel> EmployerResults { get; set; }

        public PageableViewModel<EmployerResultViewModel> EmployerResultsPage { get; set; }

        public object RouteValues => new
        {
            ProviderSiteId,
            FilterType,
            EdsErn,
            Name,
            Location,
            VacancyGuid,
            ComeFromPreview
        };

        public Guid? VacancyGuid { get; set; }

        public bool ComeFromPreview { get; set; }
    }
}