namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels
{
    using Domain.Raa.Interfaces.Repositories.Models;

    public class OrderedSearchViewModel
    {
        public OrderedSearchViewModel()
        {
            Order = Order.Ascending;
        }

        public OrderedSearchViewModel(string orderByField) : this()
        {
            OrderByField = orderByField;
        }

        public OrderedSearchViewModel(string orderByField, Order order) : this(orderByField)
        {
            Order = order;
        }

        public OrderedSearchViewModel(OrderedSearchViewModel viewModel) : this(viewModel.OrderByField, viewModel.Order)
        {
            
        }

        public string OrderByField { get; set; }

        public Order Order { get; set; }
    }
}