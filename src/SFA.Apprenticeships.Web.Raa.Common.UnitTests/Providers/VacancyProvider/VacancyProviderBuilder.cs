namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.VacancyProvider
{
    using System;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using SFA.Infrastructure.Interfaces;
    using Application.Interfaces.Providers;
    using Moq;
    using Application.Interfaces.ReferenceData;
    using Application.Interfaces.Users;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Raa.Users;
    using Raa.Common.Providers;


    public class VacancyProviderBuilder
    {
        private Mock<IProviderService> _providerService = new Mock<IProviderService>();
        private Mock<IEmployerService> _employerService = new Mock<IEmployerService>();
        private Mock<IUserProfileService> _userProfileService = new Mock<IUserProfileService>();
        private Mock<IDateTimeService> _dateTimeService = new Mock<IDateTimeService>();
        private Mock<IConfigurationService> _configurationService = new Mock<IConfigurationService>();
        private Mock<IReferenceDataService> _referenceDataService = new Mock<IReferenceDataService>();
        private Mock<IVacancyPostingService> _vacancyPostingServcie = new Mock<IVacancyPostingService>();
        private Mock<IApprenticeshipApplicationService> _apprenticeshipApplicationService = new Mock<IApprenticeshipApplicationService>();
        private Mock<ITraineeshipApplicationService> _traineeshipApplicationService = new Mock<ITraineeshipApplicationService>();
        private Mock<ILogService> _logService = new Mock<ILogService>();
        private Mock<IMapper> _mapper = new Mock<IMapper>();

        public VacancyProviderBuilder()
        {
            _userProfileService.Setup(s => s.GetAgencyUser(It.IsAny<string>())).Returns(new AgencyUser());
            _dateTimeService.Setup(s => s.UtcNow).Returns(DateTime.UtcNow);
        }

        public IVacancyQAProvider Build()
        {
            return new VacancyProvider(_logService.Object, _configurationService.Object, _vacancyPostingServcie.Object,
                _referenceDataService.Object, _providerService.Object, _employerService.Object, _dateTimeService.Object, 
                _mapper.Object, _apprenticeshipApplicationService.Object, _traineeshipApplicationService.Object, _userProfileService.Object);
        }
        
        public VacancyProviderBuilder With(
            Mock<IProviderService> providerService)
        {
            _providerService = providerService;
            return this;
        }

        public VacancyProviderBuilder With(
            Mock<IUserProfileService> userProfileService)
        {
            _userProfileService = userProfileService;
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
    }
}