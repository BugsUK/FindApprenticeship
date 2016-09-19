namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
    using System.ComponentModel.DataAnnotations;
    using Constants.ViewModels;

    public class VacancyApplicationsSearchViewModel : OrderedPageableSearchViewModel
    {
        public const string OrderByFieldLastName = "LastName";
        public const string OrderByFieldFirstName = "FirstName";
        public const string OrderByFieldSubmitted = "Submitted";
        public const string OrderByFieldManagerNotes = "ManagerNotes";
        public const string OrderByFieldStatus = "Status";

        public VacancyApplicationsSearchViewModel() : base(OrderByFieldLastName)
        {

        }

        protected VacancyApplicationsSearchViewModel(VacancyApplicationsSearchViewModel viewModel) : base(viewModel)
        {
            SetProperties(viewModel);
        }

        public VacancyApplicationsSearchViewModel(VacancyApplicationsSearchViewModel viewModel, VacancyApplicationsFilterTypes filterType) : base(viewModel, 1)
        {
            SetProperties(viewModel);
            FilterType = filterType;
        }

        public VacancyApplicationsSearchViewModel(VacancyApplicationsSearchViewModel viewModel, string orderByField, Order order) : base(viewModel, orderByField, order)
        {
            SetProperties(viewModel);
        }

        public VacancyApplicationsSearchViewModel(VacancyApplicationsSearchViewModel viewModel, int currentPage) : base(viewModel, currentPage)
        {
            SetProperties(viewModel);
        }

        private void SetProperties(VacancyApplicationsSearchViewModel viewModel)
        {
            VacancyReferenceNumber = viewModel.VacancyReferenceNumber;
            FilterType = viewModel.FilterType;
            ApplicantId = viewModel.ApplicantId;
            FirstName = viewModel.FirstName;
            LastName = viewModel.LastName;
            Postcode = viewModel.Postcode;
        }

        public int VacancyReferenceNumber { get; set; }
        public VacancyApplicationsFilterTypes FilterType { get; set; }
        [Display(Name = CandidateSearchViewModelMessages.ApplicantId.LabelText)]
        public string ApplicantId { get; set; }
        [Display(Name = CandidateSearchViewModelMessages.FirstName.LabelText)]
        public string FirstName { get; set; }
        [Display(Name = CandidateSearchViewModelMessages.LastName.LabelText)]
        public string LastName { get; set; }
        [Display(Name = CandidateSearchViewModelMessages.Postcode.LabelText, Description = CandidateSearchViewModelMessages.Postcode.HintText)]
        public string Postcode { get; set; }

        public override object RouteValues => new
        {
            VacancyReferenceNumber,
            FilterType,
            ApplicantId,
            FirstName,
            LastName,
            Postcode,
            OrderByField,
            Order,
            PageSize,
            CurrentPage
        };

        public bool IsCandidateSearch()
        {
            return !(string.IsNullOrEmpty(ApplicantId) && string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(Postcode));
        }
    }
}