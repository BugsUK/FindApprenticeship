namespace SFA.Apprenticeship.Api.AvService.Providers.Version51
{
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;
    using Infrastructure.Interfaces;
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
        private readonly ApiConfiguration _apiConfiguration;

        private readonly IApprenticeshipVacancyReadRepository _apprenticeshipVacancyReadRepository;
        private readonly IApprenticeshipVacancyMapper _apprenticeshipVacancyMapper;
        private readonly IApprenticeshipVacancyQueryMapper _apprenticeshipVacancyQueryMapper;

        public VacancyDetailsProvider(
            IConfigurationService configurationService,
            IApprenticeshipVacancyReadRepository apprenticeshipVacancyReadRepository,
            IApprenticeshipVacancyMapper apprenticeshipVacancyMapper,
            IApprenticeshipVacancyQueryMapper apprenticeshipVacancyQueryMapper)
        {
            _apiConfiguration = configurationService.Get<ApiConfiguration>();
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
            _apprenticeshipVacancyMapper = apprenticeshipVacancyMapper;
            _apprenticeshipVacancyQueryMapper = apprenticeshipVacancyQueryMapper;
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
                var searchResult = _apprenticeshipVacancyMapper.MapToVacancyFullData(vacancy);

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
            var query = _apprenticeshipVacancyQueryMapper.MapToApprenticeshipVacancyQuery(request.VacancySearchCriteria);
            int totalResultsCount;

            var vacancies = _apprenticeshipVacancyReadRepository.Find(query, out totalResultsCount);

            var searchResults = vacancies
                .Select(_apprenticeshipVacancyMapper.MapToVacancyFullData)
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
