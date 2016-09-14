namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels.Application
{
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
            VacancyReferenceNumber = viewModel.VacancyReferenceNumber;
            FilterType = viewModel.FilterType;
        }

        public VacancyApplicationsSearchViewModel(VacancyApplicationsSearchViewModel viewModel, VacancyApplicationsFilterTypes filterType) : base(viewModel, 1)
        {
            VacancyReferenceNumber = viewModel.VacancyReferenceNumber;
            FilterType = filterType;
        }

        public VacancyApplicationsSearchViewModel(VacancyApplicationsSearchViewModel viewModel, string orderByField, Order order) : base(viewModel, orderByField, order)
        {
            VacancyReferenceNumber = viewModel.VacancyReferenceNumber;
            FilterType = viewModel.FilterType;
        }

        public VacancyApplicationsSearchViewModel(VacancyApplicationsSearchViewModel viewModel, int currentPage) : base(viewModel, currentPage)
        {
            VacancyReferenceNumber = viewModel.VacancyReferenceNumber;
            FilterType = viewModel.FilterType;
        }

        public int VacancyReferenceNumber { get; set; }
        public VacancyApplicationsFilterTypes FilterType { get; set; }

        public override object RouteValues => new
        {
            VacancyReferenceNumber,
            FilterType,
            OrderByField,
            Order,
            PageSize,
            CurrentPage
        };
    }
}