namespace SFA.Apprenticeship.Api.AvService.Providers.Version51
{
    using System.Collections.Generic;
    using Apprenticeships.Domain.Entities.Reference;
    using Apprenticeships.Domain.Interfaces.Repositories;
    using DataContracts.Version51;
    using Infrastructure.Interfaces;

    public class ReferenceDataProvider : IReferenceDataProvider
    {
        private readonly IReferenceRepository _referenceRepository;
        private readonly IMapper _mapper;

        public ReferenceDataProvider(IReferenceRepository referenceRepository, IMapper mapper)
        {
            _referenceRepository = referenceRepository;
            _mapper = mapper;
        }

        public List<CountyData> GetCounties()
        {
            var counties = _referenceRepository.GetCounties();

            var countyDataList = _mapper.Map<IList<County>, List<CountyData>>(counties);

            return countyDataList;
        }
    }
}