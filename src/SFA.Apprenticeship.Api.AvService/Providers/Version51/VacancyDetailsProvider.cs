﻿namespace SFA.Apprenticeship.Api.AvService.Providers.Version51
{
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using Apprenticeships.Domain.Interfaces.Configuration;
    using Apprenticeships.Domain.Interfaces.Queries;
    using Apprenticeships.Domain.Interfaces.Repositories;
    using Configuration;
    using DataContracts.Version51;
    using Mappers.Version51;
    using MessageContracts.Version51;

    // TODO: REF: AV: VacancySearcher.cs
    // TODO: REF: AV: uspRetrieveFullVacancyXMLDetails.proc.sql

    public class VacancyDetailsProvider : IVacancyDetailsProvider
    {
        private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;
        private readonly ApiConfiguration _apiConfiguration;

        public VacancyDetailsProvider(
            IConfigurationService configurationService,
            IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository)
        {
            _apiConfiguration = configurationService.Get<ApiConfiguration>();
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
        }

        public VacancyDetailsResponse Get(VacancyDetailsRequest request)
        {
            return request.VacancySearchCriteria.VacancyReferenceId.HasValue
                ? GetVacancyDetails(request)
                : FindVacancyDetails(request);
        }

        #region Helpers

        private VacancyDetailsResponse GetVacancyDetails(VacancyDetailsRequest request)
        {
            // ReSharper disable once PossibleInvalidOperationException
            var vacancyReferenceId = request.VacancySearchCriteria.VacancyReferenceId.Value;
            var vacancy = _apprenticeshipVacancyReadRepository.Get(vacancyReferenceId);
            var searchResults = new List<VacancyFullData>();

            if (vacancy != null && vacancy.Status == ProviderVacancyStatuses.Live)
            {
                var searchResult = ApprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

                searchResults.Add(searchResult);
            }

            var response = new VacancyDetailsResponse
            {
                MessageId = request.MessageId,
                SearchResults = new VacancyDetailResponseData
                {
                    AVMSHeader = GetAvmsHeader(),
                    SearchResults = searchResults,
                    TotalPages = 1
                }
            };

            return response;
        }

        private VacancyDetailsResponse FindVacancyDetails(VacancyDetailsRequest request)
        {
            var query = ApprenticeshipVacancyQueryMapper.MapToApprenticeshipVacancyQuery(request.VacancySearchCriteria);
            int totalResultsCount;

            var vacancies = _apprenticeshipVacancyReadRepository.Find(query, out totalResultsCount);

            var searchResults = vacancies
                .Select(ApprenticeshipVacancyMapper.MapToVacancyFullData)
                .ToList();

            var response = new VacancyDetailsResponse
            {
                MessageId = request.MessageId,
                SearchResults = new VacancyDetailResponseData
                {
                    AVMSHeader = GetAvmsHeader(),
                    SearchResults = searchResults,
                    TotalPages = GetTotalPages(query, totalResultsCount)
                }
            };

            return response;
        }

        private WebInterfaceGenericDetailsData GetAvmsHeader()
        {
            return new WebInterfaceGenericDetailsData
            {
                ApprenticeshipVacanciesDescription = _apiConfiguration.EmployerInformationText,
                ApprenticeshipVacanciesURL = _apiConfiguration.EmployerInformationUrl
            };
        }

        private static int GetTotalPages(ApprenticeshipVacancyQuery query, int totalResultsCount)
        {
            return (totalResultsCount + query.PageSize - 1) / query.PageSize;
        }

        #endregion
    }
}