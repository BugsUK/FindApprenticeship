namespace SFA.Apprenticeships.Infrastructure.UnitTests.Raa
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.ReferenceData;
    using Application.Vacancies.Entities;
    using Application.Vacancy;
    using Application.VacancyPosting.Strategies;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Messaging;
    using Domain.Raa.Interfaces.Repositories;
    using Infrastructure.Raa.Mappers;
    using Infrastructure.Raa.Strategies;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using VacancySummary = Domain.Entities.Raa.Vacancies.VacancySummary;
    using VacancyType = Domain.Entities.Raa.Vacancies.VacancyType;

    [TestFixture]
    public class PublishVacancySummaryUpdateStrategyTests
    {
        private readonly Mock<IVacancyReadRepository> _mockApprenticeshipVacancyReadRepository = new Mock<IVacancyReadRepository>();
        private readonly Mock<IVacancyWriteRepository> _mockApprenticeshipVacancyWriteRepository = new Mock<IVacancyWriteRepository>();
        private readonly Mock<IProviderService> _mockProviderService = new Mock<IProviderService>();
        private readonly Mock<IEmployerService> _mockEmployerService = new Mock<IEmployerService>();
        private readonly Mock<IReferenceDataProvider> _mockReferenceDataProvider = new Mock<IReferenceDataProvider>();
        private readonly Mock<IServiceBus> _mockServiceBus = new Mock<IServiceBus>();
        private readonly Mock<IVacancySummaryService> _mockVacancySummaryService = new Mock<IVacancySummaryService>();

        private IPublishVacancySummaryUpdateStrategy _publishVacancySummaryUpdateStrategy;

        [SetUp]
        public void SetUp()
        {
            _mockVacancySummaryService.Setup(r => r.GetByIds(It.IsAny<IEnumerable<int>>())).Returns(new List<VacancySummary> { new VacancySummary
            {
                DateQAApproved = DateTime.UtcNow,
                PossibleStartDate = DateTime.UtcNow,
                ClosingDate = DateTime.UtcNow,
                Wage = new Wage(WageType.ApprenticeshipMinimum, 0, null, null, null, WageUnit.NotApplicable, 0, null)
            } });
            _mockProviderService.Setup(r => r.GetVacancyOwnerRelationships(It.IsAny<IEnumerable<int>>(), It.IsAny<bool>())).Returns(new Dictionary<int, VacancyOwnerRelationship> { { 1, new VacancyOwnerRelationship() } });
            _mockEmployerService.Setup(r => r.GetEmployers(It.IsAny<IEnumerable<int>>(), It.IsAny<bool>())).Returns(new List<Employer> { new Employer() });
            _mockProviderService.Setup(r => r.GetProviderSites(It.IsAny<IEnumerable<int>>())).Returns(new Dictionary<int, ProviderSite> { { 2, new ProviderSite() } });
            _mockProviderService.Setup(r => r.GetProviders(It.IsAny<IEnumerable<int>>())).Returns(new List<Provider> { new Provider() });

            _publishVacancySummaryUpdateStrategy = new PublishVacancySummaryUpdateStrategy(_mockApprenticeshipVacancyReadRepository.Object, _mockProviderService.Object, _mockEmployerService.Object, _mockReferenceDataProvider.Object, new VacancySummaryUpdateMapper(), _mockServiceBus.Object, new Mock<ILogService>().Object, _mockVacancySummaryService.Object);
        }

        [Test]
        public void PublishIndexUpdateForApprenticeship()
        {
            //Arrange
            var apprenticeshipVacancy = new Vacancy
            {
                Status = VacancyStatus.Live,
                VacancyType = VacancyType.Apprenticeship
            };
            _mockApprenticeshipVacancyWriteRepository.Setup(r => r.Create(apprenticeshipVacancy)).Returns(apprenticeshipVacancy);

            //Act
            _publishVacancySummaryUpdateStrategy.PublishVacancySummaryUpdate(apprenticeshipVacancy);

            //Assert
            _mockServiceBus.Verify(sb => sb.PublishMessage((ApprenticeshipSummaryUpdate)null), Times.Never);
            _mockServiceBus.Verify(sb => sb.PublishMessage(It.IsAny<ApprenticeshipSummaryUpdate>()), Times.Once);
        }

        [Test]
        public void PublishIndexUpdateForTraineeship()
        {
            //Arrange
            var apprenticeshipVacancy = new Vacancy
            {
                Status = VacancyStatus.Live,
                VacancyType = VacancyType.Traineeship
            };
            _mockApprenticeshipVacancyWriteRepository.Setup(r => r.Create(apprenticeshipVacancy)).Returns(apprenticeshipVacancy);

            //Act
            _publishVacancySummaryUpdateStrategy.PublishVacancySummaryUpdate(apprenticeshipVacancy);

            //Assert
            _mockServiceBus.Verify(sb => sb.PublishMessage((TraineeshipSummaryUpdate)null), Times.Never);
            _mockServiceBus.Verify(sb => sb.PublishMessage(It.IsAny<TraineeshipSummaryUpdate>()), Times.Once);
        }

        [Test]
        public void DoNotPublishIndexUpdateForApprenticeship()
        {
            //Arrange
            var apprenticeshipVacancy = new Vacancy
            {
                Status = VacancyStatus.Draft,
                VacancyType = VacancyType.Apprenticeship
            };
            _mockApprenticeshipVacancyWriteRepository.Setup(r => r.Create(apprenticeshipVacancy)).Returns(apprenticeshipVacancy);

            //Act
            _publishVacancySummaryUpdateStrategy.PublishVacancySummaryUpdate(apprenticeshipVacancy);

            //Assert
            _mockServiceBus.Verify(sb => sb.PublishMessage(It.IsAny<ApprenticeshipSummaryUpdate>()), Times.Never);
        }

        [Test]
        public void DoNotPublishIndexUpdateForTraineeship()
        {
            //Arrange
            var apprenticeshipVacancy = new Vacancy
            {
                Status = VacancyStatus.Draft,
                VacancyType = VacancyType.Traineeship
            };
            _mockApprenticeshipVacancyWriteRepository.Setup(r => r.Create(apprenticeshipVacancy)).Returns(apprenticeshipVacancy);

            //Act
            _publishVacancySummaryUpdateStrategy.PublishVacancySummaryUpdate(apprenticeshipVacancy);

            //Assert
            _mockServiceBus.Verify(sb => sb.PublishMessage(It.IsAny<TraineeshipSummaryUpdate>()), Times.Never);
        }
    }
}