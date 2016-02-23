namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Providers.ApprenticeshipVacancyProvider
{
    using Application.Interfaces.Candidates;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Candidate.Mappers;
    using Candidate.Providers;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Moq;

    public class ApprenticeshipVacancyProviderBuilder
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        private Mock<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>> _vacancySearchService;
        private Mock<ICandidateService> _candidateService;
        private readonly Mock<IVacancyPostingService> _vacancyPostingService;

        public ApprenticeshipVacancyProviderBuilder()
        {
            _mapper = new ApprenticeshipCandidateWebMappers();
            _logger = new Mock<ILogService>().Object;

            _vacancySearchService = new Mock<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>>();
            _candidateService = new Mock<ICandidateService>();
            _vacancyPostingService = new Mock<IVacancyPostingService>();
        }

        public ApprenticeshipVacancyProviderBuilder With(Mock<IVacancySearchService<ApprenticeshipSearchResponse, ApprenticeshipVacancyDetail, ApprenticeshipSearchParameters>> vacancySearchService)
        {
            _vacancySearchService = vacancySearchService;
            return this;
        }

        public ApprenticeshipVacancyProviderBuilder With(Mock<ICandidateService> candidateService)
        {
            _candidateService = candidateService;
            return this;
        }

        public ApprenticeshipVacancyProvider Build()
        {
            var provider = new ApprenticeshipVacancyProvider(_vacancySearchService.Object, _candidateService.Object, _mapper, _logger, _vacancyPostingService.Object);
            return provider;
        }
    }
}