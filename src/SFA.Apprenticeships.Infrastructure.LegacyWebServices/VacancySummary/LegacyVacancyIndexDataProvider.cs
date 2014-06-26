﻿using SFA.Apprenticeships.Domain.Entities.Vacancies;

namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.VacancySummary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.VacancyEtl;
    using Domain.Interfaces.Mapping;
    using Common.Wcf;
    using Configuration;
    using VacancySummaryProxy;

    public class LegacyVacancyIndexDataProvider : IVacancyIndexDataProvider
    {
        private readonly IWcfService<IVacancySummary> _service;
        private readonly ILegacyServicesConfiguration _legacyServicesConfiguration;
        private readonly IMapper _mapper;

         public LegacyVacancyIndexDataProvider(ILegacyServicesConfiguration legacyServicesConfiguration, IWcfService<IVacancySummary> service, IMapper mapper)
        {
            _legacyServicesConfiguration = legacyServicesConfiguration;
            _service = service;
            _mapper = mapper;
        }

        public int GetVacancyPageCount(VacancyLocationType vacancyLocationType)
        {
            var vacancySummaryRequest = new VacancySummaryRequest
            {
                ExternalSystemId = _legacyServicesConfiguration.SystemId,
                PublicKey = _legacyServicesConfiguration.PublicKey,
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    PageIndex = 1,
                    VacancyLocationType =  vacancyLocationType.ToString()
                }
            };

            var rs = default(VacancySummaryResponse);
            _service.Use(client => rs = client.Get(vacancySummaryRequest));

            if (rs == null || rs.ResponseData == null)
            {
                return 0;
            }

            return rs.ResponseData.TotalPages;
        }

        public IEnumerable<Domain.Entities.Vacancies.VacancySummary> GetVacancySummaries(VacancyLocationType vacancyLocationType, int page = 1)
        {
            var vacancySummaryRequest = new VacancySummaryRequest
            {
                ExternalSystemId = _legacyServicesConfiguration.SystemId,
                PublicKey = _legacyServicesConfiguration.PublicKey,
                MessageId = Guid.NewGuid(),
                VacancySearchCriteria = new VacancySearchData
                {
                    PageIndex = page,
                    VacancyLocationType = vacancyLocationType.ToString()
                }
            };

            var rs = default(VacancySummaryResponse);
            _service.Use(client => rs = client.Get(vacancySummaryRequest));

            if (rs == null || 
                rs.ResponseData == null || 
                rs.ResponseData.SearchResults == null ||
                rs.ResponseData.SearchResults.Length == 0)
            {
                return Enumerable.Empty<Domain.Entities.Vacancies.VacancySummary>().ToList();
            }

           return _mapper.Map<VacancySummaryData[], IEnumerable<Domain.Entities.Vacancies.VacancySummary>>(rs.ResponseData.SearchResults);
        }
    }
}
