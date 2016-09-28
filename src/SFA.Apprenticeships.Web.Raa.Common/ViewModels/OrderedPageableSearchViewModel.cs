namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels
{
    using Domain.Raa.Interfaces.Repositories.Models;

    //TODO: Use this class and PageableSearchViewModel throughout RAA

    /// <summary>
    /// Would normally favour composition over inheritance however view models that may be used to generate URLs need to be flat lists of properties.
    /// As a result, inhereting for reuse
    /// </summary>
    public class OrderedPageableSearchViewModel : PageableSearchViewModel
    {
        public OrderedPageableSearchViewModel() : this(null, Order.Ascending)
        {
            
        }

        public OrderedPageableSearchViewModel(string orderByField) : this(orderByField, Order.Ascending)
        {

        }

        public OrderedPageableSearchViewModel(string orderByField, Order order)
        {
            SetValues(orderByField, order);
        }

        public OrderedPageableSearchViewModel(OrderedPageableSearchViewModel viewModel) : base(viewModel)
        {
            SetValues(viewModel);
        }

        public OrderedPageableSearchViewModel(PageableSearchViewModel viewModel, string orderByField, Order order) : base(viewModel)
        {
            SetValues(orderByField, order);
        }

        public OrderedPageableSearchViewModel(OrderedPageableSearchViewModel viewModel, int currentPage) : base(viewModel, currentPage)
        {
            SetValues(viewModel);
        }

        public OrderedPageableSearchViewModel(PageableSearchViewModel viewModel, string orderByField, Order order, int currentPage) : base(viewModel, currentPage)
        {
            SetValues(orderByField, order);
        }

        protected void SetValues(OrderedPageableSearchViewModel viewModel)
        {
            SetValues(viewModel.OrderByField, viewModel.Order);
        }

        protected void SetValues(string orderByField, Order order)
        {
            OrderByField = orderByField;
            Order = order;
        }

        public string OrderByField { get; set; }

        public Order Order { get; set; }

        public override object RouteValues => new
        {
            OrderByField,
            Order,
            PageSize,
            CurrentPage
        };
    }
}