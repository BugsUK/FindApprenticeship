namespace SFA.Apprenticeship.Api.AvService.Providers.Version51
{
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Reference;
    using Apprenticeships.Domain.Interfaces.Repositories;
    using DataContracts.Version51;
    using Mappers.Version51;

    public class ReferenceDataProvider : IReferenceDataProvider
    {
        private readonly IReferenceRepository _referenceRepository;
        private readonly ICountyMapper _mapper;

        public ReferenceDataProvider(IReferenceRepository referenceRepository, ICountyMapper mapper)
        {
            _referenceRepository = referenceRepository;
            _mapper = mapper;
        }

        public List<CountyData> GetCounties()
        {
            var counties = _referenceRepository.GetCounties() ?? Enumerable.Empty<County>();

            var countyDataList = _mapper.MapToCountyDatas(counties);

            return countyDataList;
        }
    }
}