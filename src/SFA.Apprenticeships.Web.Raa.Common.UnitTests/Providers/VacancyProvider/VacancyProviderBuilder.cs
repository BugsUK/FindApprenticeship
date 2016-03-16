namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Common.Providers;
    using Moq;
    using SFA.Infrastructure.Interfaces;

    public class VacancyProviderBuilder
    {
        private readonly Mock<IApprenticeshipApplicationService> _apprenticeshipApplicationService =
            new Mock<IApprenticeshipApplicationService>();

        private readonly Mock<ILogService> _logService = new Mock<ILogService>();

        private readonly Mock<ITraineeshipApplicationService> _traineeshipApplicationService =
            new Mock<ITraineeshipApplicationService>();

        private Mock<IVacancyLockingService> _vacancyLockingService = new Mock<IVacancyLockingService>();
        private Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private Mock<IDateTimeService> _dateTimeService = new Mock<IDateTimeService>();
        private Mock<IEmployerService> _employerService = new Mock<IEmployerService>();
        private Mock<IMapper> _mapper = new Mock<IMapper>();
        private Mock<IProviderService> _providerService = new Mock<IProviderService>();
        private Mock<IReferenceDataService> _referenceDataService = new Mock<IReferenceDataService>();
        private Mock<IVacancyPostingService> _vacancyPostingServcie = new Mock<IVacancyPostingService>();
        private Mock<ICurrentUserService> _currentUserService = new Mock<ICurrentUserService>();

        public VacancyProviderBuilder()
        {
            _dateTimeService.Setup(s => s.UtcNow).Returns(DateTime.UtcNow);
        }

        public IVacancyQAProvider Build()
        {
            return new VacancyProvider(_logService.Object,
                _configurationService.Object,
                _vacancyPostingServcie.Object,
                _referenceDataService.Object,
                _providerService.Object,
                _employerService.Object,
                _dateTimeService.Object,
                _mapper.Object,
                _apprenticeshipApplicationService.Object,
                _traineeshipApplicationService.Object,
                _vacancyLockingService.Object,
                _currentUserService.Object);
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

        public VacancyProviderBuilder With(Mock<IEmployerService> employerService)
        {
            _employerService = employerService;
            return this;
        }

        public VacancyProviderBuilder With(Mock<IMapper> mapper)
        {
            _mapper = mapper;
            return this;
        }

        public VacancyProviderBuilder With(Mock<IVacancyLockingService> vacancyLockingService)
        {
            _vacancyLockingService = vacancyLockingService;
            return this;
        }

        public VacancyProviderBuilder With(Mock<ICurrentUserService> currentUserService)
        {
            _currentUserService = currentUserService;
            return this;
        }
    }
}