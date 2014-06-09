﻿namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancyDetail
{
    using System;
    using System.Linq;
    using Application.Interfaces.Vacancy;
    using Domain.Entities.Vacancy;
    using Domain.Interfaces.Mapping;
    using Common.Wcf;
    using Configuration;
    using VacancyDetailProxy;

    public class LegacyVacancyDataProvider : IVacancyDataProvider
    {
        private readonly IWcfService<IVacancyDetails> _service;
        private readonly ILegacyServicesConfiguration _legacyServicesConfiguration;
        private readonly IMapper _mapper;

         public LegacyVacancyDataProvider(ILegacyServicesConfiguration legacyServicesConfiguration, IWcfService<IVacancyDetails> service, IMapper mapper)
        {
            _legacyServicesConfiguration = legacyServicesConfiguration;
            _service = service;
            _mapper = mapper;
        }

        public VacancyDetail GetVacancyDetails(int vacancyId)
        {
            var vacancyDetailRequest = new VacancyDetailsRequest
            {
                ExternalSystemId = _legacyServicesConfiguration.SystemId,
                PublicKey = _legacyServicesConfiguration.PublicKey,
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancyDetailProxy.VacancySearchData
                {
                    VacancyReferenceId = vacancyId,
                    PageIndex = 1,
                    VacancyLocationType = VacancyDetailProxy.VacancyDetailsSearchLocationType.National
                }
            };

            var rs = default(VacancyDetailsResponse);
            _service.Use(client => rs = client.Get(vacancyDetailRequest));

            if (rs == null ||
                rs.SearchResults == null ||
                rs.SearchResults.SearchResults == null ||
                rs.SearchResults.SearchResults.Length == 0)
            {
                return default(VacancyDetail);
            }

            return _mapper.Map<VacancyFullData, VacancyDetail>(rs.SearchResults.SearchResults.First());
        }
    }
}
