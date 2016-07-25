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
    using ViewModels.VacancyPosting;

    [TestFixture]
    public class CreateVacancyTests : TestBase
    {
        private const string Ukprn = "12345";
        private const string EdsUrn = "112";
        private const int EmployerId = 1;
        private const int ProviderSiteId = 3;
        private const int VacancyPartyId = 4;

        private NewVacancyViewModel _validNewVacancyViewModelWithReferenceNumber;

        private readonly Vacancy _existingVacancy = new Vacancy()
        {
            OwnerPartyId = 2
        };

        private readonly VacancyParty _vacancyParty = new VacancyParty
        {
            VacancyPartyId = VacancyPartyId,
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
                OwnerParty = new VacancyPartyViewModel()
            };
            
            MockVacancyPostingService.Setup(mock => mock.GetVacancyByReferenceNumber(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value))
                .Returns(_existingVacancy);
            MockVacancyPostingService.Setup(mock => mock.CreateApprenticeshipVacancy(It.IsAny<Vacancy>()))
                .Returns<Vacancy>(v => v);
            MockVacancyPostingService.Setup(mock => mock.SaveVacancy(It.IsAny<Vacancy>()))
                .Returns<Vacancy>(v => v);
            MockVacancyPostingService.Setup(mock => mock.SaveVacancy(It.IsAny<Vacancy>()))
                .Returns<Vacancy>(v => v);
            MockReferenceDataService.Setup(mock => mock.GetSectors())
                .Returns(new List<Sector>
                {
                    new Sector
                    {
                        Id = 1,
                        Standards =
                            new List<Standard>
                            {
                                new Standard {Id = 1, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Intermediate},
                                new Standard {Id = 2, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Advanced},
                                new Standard {Id = 3, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Higher},
                                new Standard {Id = 4, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.FoundationDegree},
                                new Standard {Id = 5, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Degree},
                                new Standard {Id = 6, ApprenticeshipSectorId = 1, ApprenticeshipLevel = ApprenticeshipLevel.Masters}
                            }
                    }
                });
            MockProviderService.Setup(s => s.GetVacancyParty(ProviderSiteId, EdsUrn))
                .Returns(_vacancyParty);
            MockProviderService.Setup(s => s.GetVacancyParty(VacancyPartyId, true))
                .Returns(_vacancyParty);
            MockProviderService.Setup(s => s.GetProvider(Ukprn))
                .Returns(new Provider());
            MockEmployerService.Setup(s => s.GetEmployer(EmployerId)).Returns(new Fixture().Build<Employer>().Create());

            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>()))
                .Returns(new NewVacancyViewModel());
        }

        [Test]
        public void ShouldUpdateIfVacancyReferenceIsPresent()
        {
            // Arrange.
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);
            MockProviderService.Setup(m => m.GetVacancyParty(It.IsAny<int>(), true)).Returns(new VacancyParty());
            MockEmployerService.Setup(m => m.GetEmployer(It.IsAny<int>())).Returns(new Fixture().Create<Employer>());
            
            var provider = GetVacancyPostingProvider();

            // Act.
            var viewModel = provider.UpdateVacancy(_validNewVacancyViewModelWithReferenceNumber, Ukprn);

            // Assert.
            MockVacancyPostingService.Verify(mock =>
                mock.GetVacancyByReferenceNumber(_validNewVacancyViewModelWithReferenceNumber.VacancyReferenceNumber.Value), Times.Once);
            MockVacancyPostingService.Verify(mock => mock.GetNextVacancyReferenceNumber(), Times.Never);
            MockVacancyPostingService.Verify(mock =>
                mock.UpdateVacancy(It.IsAny<Vacancy>()), Times.Once);

            viewModel.VacancyReferenceNumber.Should().HaveValue();
        }

        [Test]
        public void ShouldUpdateVacancyWithTheEmployerAddress()
        {
            // Arrange.
            var employerPostalAddress = new Fixture().Create<PostalAddress>();
            var vvm = new Fixture().Build<NewVacancyViewModel>().Create();
            MockMapper.Setup(m => m.Map<Vacancy, NewVacancyViewModel>(It.IsAny<Vacancy>())).Returns(vvm);
            MockProviderService.Setup(m => m.GetVacancyParty(It.IsAny<int>(), true)).Returns(new VacancyParty());
            MockEmployerService.Setup(m => m.GetEmployer(It.IsAny<int>()))
                .Returns(new Fixture().Build<Employer>().With(e => e.Address, employerPostalAddress).Create());
            var provider = GetVacancyPostingProvider();

            // Act.
            provider.UpdateVacancy(_validNewVacancyViewModelWithReferenceNumber, Ukprn);

            // Assert.
            MockVacancyPostingService.Verify(mock =>
                mock.UpdateVacancy(It.Is<Vacancy>(v => v.Address == employerPostalAddress)));
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
            var result = provider.GetNewVacancyViewModel(VacancyPartyId, vacancyGuid, null);

            // Assert
            MockVacancyPostingService.Verify(s => s.GetVacancy(vacancyGuid), Times.Once);
            MockProviderService.Verify(s => s.GetVacancyParty(VacancyPartyId, true), Times.Once);
            MockEmployerService.Verify(s => s.GetEmployer(EmployerId), Times.Once);
            result.Should()
                .Match<NewVacancyViewModel>(
                    r =>
                        r.OwnerParty.EmployerDescription == _vacancyParty.EmployerDescription &&
                        r.OwnerParty.ProviderSiteId == ProviderSiteId);
        }
    }
}