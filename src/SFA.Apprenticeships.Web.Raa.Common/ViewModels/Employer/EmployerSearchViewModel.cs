namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Employer
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;
    using Web.Common.ViewModels;

    public enum EmployerFilterType
    {
        Undefined,
        EdsUrn,
        NameAndLocation,
        NameOrLocation
    }

    public class EmployerSearchViewModel
    {
        public EmployerSearchViewModel()
        {
            Employers = new PageableViewModel<EmployerViewModel> { CurrentPage = 1 };
        }

        public EmployerSearchViewModel(EmployerSearchViewModel viewModel) : this()
        {
            ProviderSiteId = viewModel.ProviderSiteId;
            FilterType = viewModel.FilterType;
            EdsUrn = viewModel.EdsUrn;
            Name = viewModel.Name;
            Location = viewModel.Location;
            if (viewModel.Employers != null)
            {
                Employers.CurrentPage = viewModel.Employers.CurrentPage;
            }
            VacancyGuid = viewModel.VacancyGuid;
            ComeFromPreview = viewModel.ComeFromPreview;
            PerformSearch = viewModel.PerformSearch;
        }

        public int ProviderSiteId { get; set; }

        public EmployerFilterType FilterType { get; set; }

        [Display(Name = EmployerSearchViewModelMessages.EdsUrn.LabelText)]
        public string EdsUrn { get; set; }

        [Display(Name = EmployerSearchViewModelMessages.Name.LabelText)]
        public string Name { get; set; }

        [Display(Name = EmployerSearchViewModelMessages.Location.LabelText)]
        public string Location { get; set; }

        public PageableViewModel<EmployerViewModel> Employers { get; set; }

        public bool NoResults => Employers == null || Employers.ResultsCount == 0;

        public object RouteValues => new
        {
            ProviderSiteId,
            FilterType,
            EdsUrn,
            Name,
            Location,
            VacancyGuid,
            ComeFromPreview,
            PerformSearch
        };

        public Guid? VacancyGuid { get; set; }

        public bool ComeFromPreview { get; set; }

        public bool PerformSearch { get; set; }
    }
}