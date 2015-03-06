
namespace SFA.Apprenticeships.Web.ContactForms.Mappers
{
    using Common.Extensions;
    using Domain.Entities;
    using Interfaces;
    using ViewModels;

    public class ReferenceDataMapper : IDomainToViewModelMapper<ReferenceData, ReferenceDataViewModel>, IViewModelToDomainMapper<ReferenceDataViewModel, ReferenceData>
    {
        public ReferenceDataViewModel ConvertToViewModel(ReferenceData domain)
        {
            domain.ThrowIfNull("ReferenceData", "domain object of type ReferenceData object can't be null");            

            return new ReferenceDataViewModel()
            {
                Id = domain.Id,
                Description = domain.Description
            };
        }
                 
        public ReferenceData ConvertToDomain(ReferenceDataViewModel viewModel)
        {
            viewModel.ThrowIfNull("ReferenceDataViewModel", "viewModel object of type ReferenceDataViewModel can't be null");

            return new ReferenceData()
            {
                Id = viewModel.Id,
                Description = viewModel.Description
            };
        }
    }
}