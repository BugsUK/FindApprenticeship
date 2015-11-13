namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.VacancyProvider
{
    using Application.Interfaces.DateTime;
    using Application.Interfaces.Logging;
    using Application.Interfaces.Providers;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using Moq;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.VacancyPosting;
    using Raa.Common.Providers;


    public class VacancyProviderBuilder
    {
        private Mock<IApprenticeshipVacancyReadRepository> _apprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();
        private Mock<IApprenticeshipVacancyWriteRepository> _apprenticeshipVacancyWriteRepository = new Mock<IApprenticeshipVacancyWriteRepository>();
        private Mock<IProviderService> _providerService = new Mock<IProviderService>();
        private Mock<IDateTimeService> _dateTimeService = new Mock<IDateTimeService>();
        private Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private Mock<IReferenceDataService> _referenceDataService = new Mock<IReferenceDataService>();
        private Mock<IVacancyPostingService> _vacancyPostingServcie = new Mock<IVacancyPostingService>();
        private Mock<ILogService> _logService = new Mock<ILogService>();

        public IVacancyQAProvider Build()
        {
            return new VacancyPostingProvider(_logService.Object, _configurationService.Object, _vacancyPostingServcie.Object,
                _referenceDataService.Object, _providerService.Object, _dateTimeService.Object, _apprenticeshipVacancyReadRepository.Object,
                _apprenticeshipVacancyWriteRepository.Object);
        }

        public VacancyProviderBuilder With(
            Mock<IApprenticeshipVacancyWriteRepository> apprenticeshipVacancyWriteRepository)
        {
            _apprenticeshipVacancyWriteRepository = apprenticeshipVacancyWriteRepository;
            return this;
        }

        public VacancyProviderBuilder With(
            Mock<IApprenticeshipVacancyReadRepository> apprenticeshipVacancyReadRepository)
        {
            _apprenticeshipVacancyReadRepository = apprenticeshipVacancyReadRepository;
            return this;
        }

        public VacancyProviderBuilder With(
            Mock<IProviderService> providerService)
        {
            _providerService = providerService;
            return this;
        }

        public VacancyProviderBuilder With(Mock<IDateTimeService> dateTimeService)
        {
            _dateTimeService = dateTimeService;
            return this;
        }

        public VacancyProviderBuilder With(Mock<IConfigurationService> configurationService)
        {
            _configurationService = configurationService;
            return this;
        }

        public VacancyProviderBuilder With(Mock<IReferenceDataService> referenceDataService)
        {
            _referenceDataService = referenceDataService;
            return this;
        }

        public VacancyProviderBuilder With(Mock<IVacancyPostingService> vacancyPostingServiceService)
        {
            _vacancyPostingServcie = vacancyPostingServiceService;
            return this;
        }
    }
}