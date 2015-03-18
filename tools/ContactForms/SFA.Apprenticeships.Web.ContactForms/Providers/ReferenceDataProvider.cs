namespace SFA.Apprenticeships.Web.ContactForms.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces;
    using Common.Extensions;
    using Domain.Entities;
    using Domain.Enums;
    using Interfaces;
    using Mappers.Interfaces;
    using ViewModels;

    public class ReferenceDataProvider : IReferenceDataProvider
    {
        private readonly IReferenceDataService _referenceDataService;
        private readonly IDomainToViewModelMapper<ReferenceData, ReferenceDataViewModel> _referenceDataDomainToViewModelMapper;

        public ReferenceDataProvider(IReferenceDataService referenceDataService, IDomainToViewModelMapper<ReferenceData, ReferenceDataViewModel> referenceDataDomainToViewModelMapper)
        {
            _referenceDataService = referenceDataService;
            _referenceDataDomainToViewModelMapper = referenceDataDomainToViewModelMapper;
        }

        public ReferenceDataListViewModel GetReferenceData(ReferenceDataTypes type)
        {
            try
            {
                var referenceData = _referenceDataService.Get(type);

                var referenceDataViewModel = referenceData.Select(m => _referenceDataDomainToViewModelMapper.ConvertToViewModel(m));
                IEnumerable<ReferenceDataViewModel> referenceDataViewModels = referenceDataViewModel as IList<ReferenceDataViewModel> ?? referenceDataViewModel.ToList();
                if (!referenceDataViewModels.IsNullOrEmpty())
                {
                    return new ReferenceDataListViewModel(referenceDataViewModels);
                }
                return new ReferenceDataListViewModel();

            }
            catch (System.Exception)
            {
                //todo: log error using preferred logging mechanism
                return new ReferenceDataListViewModel();
            }
        }

    }
}