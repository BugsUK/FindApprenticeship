namespace SFA.Apprenticeships.Web.ContactForms.Mappers
{
    using Common.Extensions;
    using Domain.Entities;
    using Interfaces;
    using ViewModels;

    public class AccessRequestMapper : IDomainToViewModelMapper<AccessRequest, AccessRequestViewModel>, IViewModelToDomainMapper<AccessRequestViewModel, AccessRequest>
    {
        private IDomainToViewModelMapper<Address, AddressViewModel> _addressDomainToViewModelMapper;
        private IViewModelToDomainMapper<AddressViewModel, Address> _addressViewModelToDomainMapper;
        public AccessRequestMapper(IDomainToViewModelMapper<Address, AddressViewModel> addressDomainToViewModelMapper,
                                     IViewModelToDomainMapper<AddressViewModel, Address> addressViewModelToDomainMapper)
        {
            _addressDomainToViewModelMapper = addressDomainToViewModelMapper;
            _addressViewModelToDomainMapper = addressViewModelToDomainMapper;
        }

        public AccessRequestViewModel ConvertToViewModel(AccessRequest domain)
        {
            return new AccessRequestViewModel()
            {
                Address = _addressDomainToViewModelMapper.ConvertToViewModel(domain.Address),
                Email = domain.Email,
                Firstname = domain.Firstname,
                Lastname = domain.Lastname,
                Companyname = domain.Companyname,
                WorkPhoneNumber = domain.WorkPhoneNumber,
                MobileNumber = domain.MobileNumber,
                Title = domain.Title,
                Position = domain.Position,
                HasApprenticeshipVacancies = domain.HasApprenticeshipVacancies,
                UserType = domain.UserType,
                Systemname = domain.Systemname,
                Contactname = domain.Contactname,
                AdditionalEmail = domain.AdditionalEmail,
                AdditionalPhoneNumber = domain.AdditionalPhoneNumber,
                ServiceTypes = new AccessRequestServicesViewModel(){ PostedServiceIds = domain.SelectedServiceTypeIds.Split(',') }
            };
        }

        public AccessRequest ConvertToDomain(AccessRequestViewModel viewModel)
        {
            viewModel.ThrowIfNull("AccessRequestViewModel", "viewModel object of type AccessRequestViewModel can't be null");

            string selectedServiceTypeIds = string.Empty; 
            if (viewModel.ServiceTypes != null)
            {
                selectedServiceTypeIds = string.Join(",", viewModel.ServiceTypes.PostedServiceIds);
            }

            return new AccessRequest()
            {
                Address = _addressViewModelToDomainMapper.ConvertToDomain(viewModel.Address),
                Email = viewModel.Email,                
                Firstname = viewModel.Firstname,
                Lastname = viewModel.Lastname,
                Companyname = viewModel.Companyname,
                WorkPhoneNumber = viewModel.WorkPhoneNumber,
                MobileNumber = viewModel.MobileNumber,
                Title = viewModel.Title,
                Position = viewModel.Position,
                HasApprenticeshipVacancies = viewModel.HasApprenticeshipVacancies,
                UserType= viewModel.UserType,
                Systemname= viewModel.Systemname,
                Contactname= viewModel.Contactname,
                AdditionalEmail= viewModel.AdditionalEmail,
                AdditionalPhoneNumber= viewModel.AdditionalPhoneNumber,
                SelectedServiceTypeIds = selectedServiceTypeIds
            };
        }
    }
}