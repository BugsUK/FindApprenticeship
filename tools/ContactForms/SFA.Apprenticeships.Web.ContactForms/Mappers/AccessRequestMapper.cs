namespace SFA.Apprenticeships.Web.ContactForms.Mappers
{
    using Domain.Entities;
    using Interfaces;
    using ViewModels;

    public class AccessRequestMapper : IDomainToViewModelMapper<AccessRequest, AccessRequestViewModel>, IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest>
    {
        public AccessRequestViewModel ConvertToViewModel(AccessRequest domain)
        {
            throw new System.NotImplementedException();
        }

        public AccessRequest ConvertToDomain(AccessRequestViewModel viewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}