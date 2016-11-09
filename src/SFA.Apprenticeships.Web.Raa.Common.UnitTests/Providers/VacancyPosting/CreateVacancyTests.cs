namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using ViewModels.Provider;
    using ViewModels.Vacancy;
    
    [TestFixture]
    [Parallelizable]
    public class CreateVacancyTests : TestBase
    {
        private const string Ukprn = "12345";
        private const string EdsUrn = "112";
        private const int EmployerId = 1;
        private const int ProviderSiteId = 3;
        private const int VacancyOwnerRelationshipId = 4;

        private NewVacancyViewModel _validNewVacancyViewModelWithReferenceNumber;

        private readonly Vacancy _existingVacancy = new Vacancy()
        {
            VacancyOwnerRelationshipId = 2
        };

        private readonly VacancyOwnerRelationship _vacancyOwnerRelationship = new VacancyOwnerRelationship
        {
            VacancyOwnerRelationshipId = VacancyOwnerRelationshipId,
            ProviderSiteId = ProviderSiteId,
            EmployerId = EmployerId,
            EmployerDescription = "description"
        };

        [SetUp]
        public void SetUp()
        {
            _validNewVacancyViewModelWithReferenceNumber = new NewVacancyViewModel()
            {
                VacancyReferenceNumber = 1,
                OfflineVacancy = false,
                VacancyOwnerRelationship = new VacancyOwnerRelationshipViewModel()
            };

            MockVacancyPostingService.Setup(mock => mock.GetVacancyByReferenceNumber(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value))
                .Returns(_existingVacancy);
            MockVacancyPostingService.Setup(mock => mock.CreateVacancy(It.IsAny<Vacancy>()))
                .Returns<Vacancy>(v => v);
            MockReferenceDataService.Setup(mock => mock.GetSectors())
                .Returns(new List<Sector>
                {
                    new Sector
                    {
                        SectorId = 1,
                        Standards =
                            new List<Standard>
                            {
                                new Standard {StandardId = 1, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Intermediate},
                                new Standard {StandardId = 2, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Advanced},
                                new Standard {StandardId = 3, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Higher},
                                new Standard {StandardId = 4, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.FoundationDegree},
                                new Standard {StandardId = 5, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Degree},
                                new Standard {StandardId = 6, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Masters}
                            }
                    }
                });
            MockProviderService.Setup(s => s.GetVacancyOwnerRelationship(ProviderSiteId, EdsUrn))
                .Returns(_vacancyOwnerRelationship);
            MockProviderService.Setup(s => s.GetVacancyOwnerRelationship(VacancyOwnerRelationshipId, true))
                .Returns(_vacancyOwnerRelationship);
            MockProviderService.Setup(s => s.GetProvider(Ukprn, true))
                .Returns(new Provider());
            MockEmployerService.Setup(s => s.GetEmployer(EmployerId, It.IsAny<bool>())).Returns(new Fixture().Build<Employer>().Create());

            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>()))
                .Returns(new NewVacancyViewModel());
        }

        [Test]
        public void ShouldUpdateIfVacancyReferenceIsPresent()
        {
            // Arrange.
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);
            MockProviderService.Setup(m => m.GetVacancyOwnerRelationship(It.IsAny<int>(), true)).Returns(new VacancyOwnerRelationship());
            MockEmployerService.Setup(m => m.GetEmployer(It.IsAny<int>(), It.IsAny<bool>())).Returns(new Fixture().Create<Employer>());
            
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.UpdateVacancy(_validNewVacancyViewModelWithReferenceNumber);

            // Assert.
            MockVacancyPostingService.Verify(mock =>
                mock.GetVacancyByReferenceNumber(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value), Times.Once);
            MockVacancyPostingService.Verify(mock => mock.GetNextVacancyReferenceNumber(), Times.Never);
            MockVacancyPostingService.Verify(mock =>
                mock.UpdateVacancy(It.IsAny<Vacancy>()), Times.Once);

            viewModel.VacancyReferenceNumber.Should().HaveValue();
        }

       
        [Test]
        public void ShouldReturnANewVacancyIfVacancyGuidDoesNotExists()
        {
            // Arrange
            var vacancyGuid = Guid.NewGuid();
            Vacancy vacancy = null;
            
            MockVacancyPostingService.Setup(s => s.GetVacancy(vacancyGuid)).Returns(vacancy);
            var provider = GetVacancyPostingProvider();

            // Act
            var result = provider.GetNewVacancyViewModel(VacancyOwnerRelationshipId, vacancyGuid, null);

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.GetVacancyOwnerRelationship(VacancyOwnerRelationshipId, true), Times.Once);
            MockEmployerService.Verify(s => s.GetEmployer(EmployerId, It.IsAny<bool>()), Times.Once);
            result.Should()
                .Match<NewVacancyViewModel>(
                    r =>
                        r.VacancyOwnerRelationship.EmployerDescription == _vacancyOwnerRelationship.EmployerDescription &&
                        r.VacancyOwnerRelationship.ProviderSiteId == ProviderSiteId);
        }
    }
}