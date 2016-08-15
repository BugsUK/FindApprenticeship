namespace SFA.Apprenticeships.Web.Raa.Common.ViewModels
{
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

        public string OrderByField { get; set; }

        public Order Order { get; set; }
        
        public virtual object RouteValues => new
        {
            OrderByField,
            Order
        };
    }
}