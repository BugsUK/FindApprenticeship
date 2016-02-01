namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.VacancyProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Application.Interfaces.VacancyPosting;
    using Common.Configuration;
    using Common.ViewModels;
    using Domain.Entities.Locations;
    using Moq.Language.Flow;
    using Raa.Common.Configuration;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class VacancyProviderTests
    {
        const int QAVacancyTimeout = 10;

        [Test]
        public void UpdateVacancyBasicDetailsWithComments()
        {
            //Arrange
            const string ukprn = "ukprn";

            var newVacancyVM = new Fixture().Build<NewVacancyViewModel>().Create();

            var sectorList = new List<Sector>()
            {
                new Fixture().Build<Sector>().Create()
            };

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });
            var referenceDataService = new Mock<IReferenceDataService>();
            referenceDataService.Setup(m => m.GetSectors()).Returns(sectorList);
            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());
            
            //Arrange: get AV, update retrieved AV with NVVM, save modified AV returning same modified AV, map AV to new NVVM with same properties as input
            vacancyPostingService.Setup(vps => vps.GetVacancy(newVacancyVM.VacancyReferenceNumber.Value)).Returns(
                (long refNo) =>
                {
                    return new Fixture().Build<ApprenticeshipVacancy>()
                        .With(av => av.VacancyReferenceNumber, newVacancyVM.VacancyReferenceNumber.Value)
                        .With(av => av.OfflineApplicationInstructionsComment, Guid.NewGuid().ToString())
                        .With(av => av.OfflineApplicationUrlComment, Guid.NewGuid().ToString())
                        .With(av => av.ShortDescriptionComment, Guid.NewGuid().ToString())
                        .With(av => av.TitleComment, Guid.NewGuid().ToString())
                        .With(av => av.OfflineApplicationUrl, string.Format("http://www.google.com/{0}", Guid.NewGuid()))
                        .With(av => av.OfflineApplicationInstructions, Guid.NewGuid().ToString())
                        .With(av => av.ShortDescription, Guid.NewGuid().ToString())
                        .With(av => av.Title, Guid.NewGuid().ToString())
                        .Create();
                });

            vacancyPostingService.Setup(vps => vps.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>())).Returns((ApprenticeshipVacancy av) => av );

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<ApprenticeshipVacancy, NewVacancyViewModel>(It.IsAny<ApprenticeshipVacancy>()))
                .Returns((ApprenticeshipVacancy av) => newVacancyVM);

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .With(referenceDataService)
                    .With(mapper)
                    .Build();

            //Act
            var result = vacancyProvider.UpdateVacancyWithComments(newVacancyVM);

            //Assert
            vacancyPostingService.Verify(vps => vps.GetVacancy(newVacancyVM.VacancyReferenceNumber.Value), Times.Once);
            vacancyPostingService.Verify(vps => vps.ShallowSaveApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(av => av.VacancyReferenceNumber == newVacancyVM.VacancyReferenceNumber.Value)));
            result.VacancyReferenceNumber.Should().Be(newVacancyVM.VacancyReferenceNumber);
            result.OfflineApplicationInstructionsComment.Should().Be(newVacancyVM.OfflineApplicationInstructionsComment);
            result.OfflineApplicationUrlComment.Should().Be(newVacancyVM.OfflineApplicationUrlComment);
            result.ShortDescriptionComment.Should().Be(newVacancyVM.ShortDescriptionComment);
            result.TitleComment.Should().Be(newVacancyVM.TitleComment);
            result.OfflineApplicationInstructions.Should().Be(newVacancyVM.OfflineApplicationInstructions);
            result.OfflineApplicationUrl.Should().Be(newVacancyVM.OfflineApplicationUrl);
            result.ShortDescription.Should().Be(newVacancyVM.ShortDescription);
            result.Title.Should().Be(newVacancyVM.Title);
        }

        [Test]
        public void UpdateVacancyBasicDetailsShouldExpectVacancyReferenceNumber()
        {
            //Arrange
            var newVacancyVM = new Fixture().Build<NewVacancyViewModel>()
                .With(vm => vm.VacancyReferenceNumber, null)
                .Create();

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .Build();

            //Act
            Action action = () => vacancyProvider.UpdateVacancyWithComments(newVacancyVM);

            //Assert
            action.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void UpdateTrainingDetailsWithComments()
        {
            //Arrange
            const string ukprn = "ukprn";

            var trainingDetailsViewModel = new Fixture().Build<TrainingDetailsViewModel>().Create();

            var sectorList = new List<Sector>()
            {
                new Fixture().Build<Sector>().Create()
            };

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });
            var referenceDataService = new Mock<IReferenceDataService>();
            referenceDataService.Setup(m => m.GetSectors()).Returns(sectorList);
            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            //Arrange: get AV, update retrieved AV with NVVM, save modified AV returning same modified AV, map AV to new NVVM with same properties as input
            vacancyPostingService.Setup(vps => vps.GetVacancy(trainingDetailsViewModel.VacancyReferenceNumber.Value)).Returns(
                (long refNo) =>
                {
                    return new Fixture().Build<ApprenticeshipVacancy>()
                        .With(av => av.VacancyReferenceNumber, trainingDetailsViewModel.VacancyReferenceNumber.Value)
                        .With(av => av.ApprenticeshipLevelComment, Guid.NewGuid().ToString())
                        .With(av => av.FrameworkCodeNameComment, Guid.NewGuid().ToString())
                        .With(av => av.ApprenticeshipLevel, trainingDetailsViewModel.ApprenticeshipLevel)
                        .With(av => av.FrameworkCodeName, Guid.NewGuid().ToString())
                        .With(av => av.StandardIdComment, Guid.NewGuid().ToString())
                        .With(av => av.StandardId, null)
                        .Create();
                });

            vacancyPostingService.Setup(vps => vps.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>())).Returns((ApprenticeshipVacancy av) => av);

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<ApprenticeshipVacancy, TrainingDetailsViewModel>(It.IsAny<ApprenticeshipVacancy>()))
                .Returns((ApprenticeshipVacancy av) => trainingDetailsViewModel);

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .With(referenceDataService)
                    .With(mapper)
                    .Build();

            //Act
            var result = vacancyProvider.UpdateVacancyWithComments(trainingDetailsViewModel);

            //Assert
            vacancyPostingService.Verify(vps => vps.GetVacancy(trainingDetailsViewModel.VacancyReferenceNumber.Value), Times.Once);
            vacancyPostingService.Verify(vps => vps.ShallowSaveApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(av => av.VacancyReferenceNumber == trainingDetailsViewModel.VacancyReferenceNumber.Value)));
            result.VacancyReferenceNumber.Should().Be(trainingDetailsViewModel.VacancyReferenceNumber);
            result.ApprenticeshipLevelComment.Should().Be(trainingDetailsViewModel.ApprenticeshipLevelComment);
            result.FrameworkCodeNameComment.Should().Be(trainingDetailsViewModel.FrameworkCodeNameComment);
            result.StandardIdComment.Should().Be(trainingDetailsViewModel.StandardIdComment);
            result.StandardId.Should().Be(trainingDetailsViewModel.StandardId);
            result.ApprenticeshipLevel.Should().Be(trainingDetailsViewModel.ApprenticeshipLevel);
            result.FrameworkCodeName.Should().Be(trainingDetailsViewModel.FrameworkCodeName);
        }

        [Test]
        public void UpdateVacancyRequirementsAndProspectsWithComments()
        {
            //Arrange
            var vacancyVm = new Fixture().Build<VacancyRequirementsProspectsViewModel>()
                .Create();

            var appVacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(x => x.VacancyReferenceNumber, vacancyVm.VacancyReferenceNumber)
                .With(x => x.Status, ProviderVacancyStatuses.PendingQA)
                .Create();

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            vacancyPostingService.Setup(
                vps => vps.GetVacancy(vacancyVm.VacancyReferenceNumber)).Returns(appVacancy);

            vacancyPostingService.Setup(vps => vps.ShallowSaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>())).Returns(appVacancy);

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .Build();

            //Act
            var result = vacancyProvider.UpdateVacancyWithComments(vacancyVm);

            //Assert
            vacancyPostingService.Verify(vps => vps.GetVacancy(vacancyVm.VacancyReferenceNumber), Times.Once);
            vacancyPostingService.Verify(vps => vps.ShallowSaveApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(av => av.VacancyReferenceNumber == vacancyVm.VacancyReferenceNumber)));
            result.VacancyReferenceNumber.Should().Be(vacancyVm.VacancyReferenceNumber);
            result.DesiredQualifications.Should().Be(vacancyVm.DesiredQualifications);
            result.DesiredQualificationsComment.Should().Be(vacancyVm.DesiredQualificationsComment);
            result.DesiredSkills.Should().Be(vacancyVm.DesiredSkills);
            result.DesiredSkillsComment.Should().Be(vacancyVm.DesiredSkillsComment);
            result.FutureProspectsComment.Should().Be(vacancyVm.FutureProspectsComment);
            result.FutureProspects.Should().Be(vacancyVm.FutureProspects);
            result.PersonalQualitiesComment.Should().Be(vacancyVm.PersonalQualitiesComment);
            result.PersonalQualities.Should().Be(vacancyVm.PersonalQualities);
            result.ThingsToConsiderComment.Should().Be(vacancyVm.ThingsToConsiderComment);
            result.ThingsToConsider.Should().Be(vacancyVm.ThingsToConsider);
        }

        [Test]
        public void GetVacanciesPendingQAShouldCallRepositoryWithPendingQAAsDesiredStatus()
        {
            //Arrange
            var apprenticeshipVacancyRepository = new Mock<IApprenticeshipVacancyReadRepository>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            const string ukprn = "ukprn";
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA))
                .Returns(new List<ApprenticeshipVacancy>
                {
                    new ApprenticeshipVacancy
                    {
                        ClosingDate = DateTime.Now,
                        DateSubmitted = DateTime.Now,
                        ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                        {
                            Employer = new Employer()
                        },
                        Ukprn = ukprn,
                        Status = ProviderVacancyStatuses.PendingQA
                    }
                });

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .With(configurationService)
                    .Build();

            //Act
            vacancyProvider.GetPendingQAVacancies();

            //Assert
            vacancyPostingService.Verify(avr => avr.GetWithStatus(ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA));
            providerService.Verify(ps => ps.GetProvider(ukprn), Times.Once);
        }

        [Test]
        public void ApproveVacancy()
        {
            //Arrange
            long vacancyReferenceNumber = 1;
            var vacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(x => x.VacancyReferenceNumber, vacancyReferenceNumber)
                .With(x => x.LocationAddresses, null)
                .Create();

            var configurationService = new Mock<IConfigurationService>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            vacancyPostingService.Setup(r => r.GetVacancy(vacancyReferenceNumber)).Returns(vacancy);
            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(configurationService)
                    .With(vacancyPostingService)
                    .Build();

            //Act
            vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

            //Assert
            vacancyPostingService.Verify(r => r.GetVacancy(vacancyReferenceNumber));
            vacancyPostingService.Verify(
                r =>
                    r.ShallowSaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(
                            av =>
                                av.VacancyReferenceNumber == vacancyReferenceNumber &&
                                av.Status == ProviderVacancyStatuses.Live)));
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void ApproveMultilocationVacancy(int locationAddressCount)
        {
            //Arrange
            long vacancyReferenceNumber = 1;
            var locationAddresses = new Fixture().Build<VacancyLocationAddress>()
                .CreateMany(locationAddressCount).ToList();

            var vacancy = new Fixture().Build<ApprenticeshipVacancy>()
                .With(x => x.VacancyReferenceNumber, vacancyReferenceNumber)
                .With(x => x.LocationAddresses, locationAddresses)
                .Create();

            var vacancyPostingService = new Mock<IVacancyPostingService>();

            vacancyPostingService.Setup(r => r.GetVacancy(vacancyReferenceNumber))
                .Returns(vacancy);

            //set up so that a bunch of vacancy reference numbers are created that are not the same as the one supplied above
            var fixture = new Fixture {RepeatCount = locationAddressCount};
            var vacancyNumbers = fixture.Create<List<long>>();
            vacancyPostingService.Setup(r => r.GetNextVacancyReferenceNumber()).ReturnsInOrder(vacancyNumbers.ToArray());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(vacancyPostingService)
                    .Build();

            //Act
            vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

            //Assert
            //get the submitted vacancy once
            vacancyPostingService.Verify(r => r.GetVacancy(vacancyReferenceNumber), Times.Once);
            //save the original vacancy with a status of ParentVacancy
            vacancyPostingService.Verify(
                r =>
                    r.ShallowSaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(
                            av =>
                                av.VacancyReferenceNumber == vacancyReferenceNumber &&
                                av.Status == ProviderVacancyStatuses.ParentVacancy)));

            //save new vacancies with a status of Live
            foreach (var number in vacancyNumbers)
            {
                vacancyPostingService.Verify(r =>
                    r.CreateApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(av => av.VacancyReferenceNumber == number
                    && av.Status == ProviderVacancyStatuses.Live)), Times.Once);
            }

            //save new vacancies with only one of the new addresses and the position count
            foreach (var location in locationAddresses)
            {
                vacancyPostingService.Verify(r => r.CreateApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(av
                    => av.LocationAddresses.Single().Address.Postcode == location.Address.Postcode
                       && av.LocationAddresses.Single().Address.AddressLine1 == location.Address.AddressLine1
                       && av.LocationAddresses.Single().Address.AddressLine2 == location.Address.AddressLine2
                       && av.LocationAddresses.Single().Address.AddressLine3 == location.Address.AddressLine3
                       && av.LocationAddresses.Single().Address.AddressLine4 == location.Address.AddressLine4
                       && av.LocationAddresses.Single().Address.AddressLine4 == location.Address.AddressLine4
                       && av.LocationAddresses.Single().NumberOfPositions == location.NumberOfPositions
                       && av.Status == ProviderVacancyStatuses.Live
                       && av.ParentVacancyId == vacancy.EntityId
                       && av.NumberOfPositions == location.NumberOfPositions)));
            }

            //save the submitted vacancy once
            vacancyPostingService.Verify( r => r.ShallowSaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Once);
            
            //Create each child vacancy once
            vacancyPostingService.Verify( r => r.CreateApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()), Times.Exactly(locationAddressCount));
        }

        [Test]
        public void RejectVacancyShouldCallRepositorySaveWithStatusAsRejectedByQA()
        {
            //Arrange
            long vacancyReferenceNumber = 1;
            var vacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            vacancyPostingService.Setup(r => r.GetVacancy(vacancyReferenceNumber)).Returns(vacancy);
            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(configurationService)
                    .With(vacancyPostingService)
                    .Build();

            //Act
            vacancyProvider.RejectVacancy(vacancyReferenceNumber);

            //Assert
            vacancyPostingService.Verify(r => r.GetVacancy(vacancyReferenceNumber));
            vacancyPostingService.Verify(
                r =>
                    r.ShallowSaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(
                            av =>
                                av.VacancyReferenceNumber == vacancyReferenceNumber &&
                                av.Status == ProviderVacancyStatuses.RejectedByQA &&
                                av.QAUserName == null)));
        }

        [Test]
        public void GetPendingQAVacanciesShouldReturnVacanciesWithoutQAUserName()
        {
            //Arrange
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            const string ukprn = "ukprn";
            const int vacancyReferenceNumber = 1;
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            var apprenticeshipVacancies = new List<ApprenticeshipVacancy>
            {
                new ApprenticeshipVacancy
                {
                    ClosingDate = DateTime.Now,
                    DateSubmitted = DateTime.Now,
                    ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                    {
                        Employer = new Employer()
                    },
                    Ukprn = ukprn,
                    VacancyReferenceNumber = vacancyReferenceNumber,
                    Status = ProviderVacancyStatuses.PendingQA
                }
            };

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(configurationService)
                    .With(vacancyPostingService)
                    .Build();

            //Act
            var vacancies = vacancyProvider.GetPendingQAVacancies();

            //Assert
            vacancies.Should().HaveCount(1);
            vacancies.First().VacancyReferenceNumber.Should().Be(vacancyReferenceNumber);
        }

        [Test]
        public void GetPendingQAVacanciesShouldReturnVacanciesWithCurrentUsersQAUserName()
        {
            //Arrange
            var apprenticeshipVacancyRepository = new Mock<IApprenticeshipVacancyReadRepository>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            const string ukprn = "ukprn";
            const int vacancyReferenceNumberOK = 1;
            const int vacancyReferenceNumberNonOK = 2;
            const string username = "qa@test.com";
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            var apprenticeshipVacancies = new List<ApprenticeshipVacancy>
            {
                new ApprenticeshipVacancy
                {
                    ClosingDate = DateTime.Now,
                    DateSubmitted = DateTime.Now,
                    ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                    {
                        Employer = new Employer()
                    },
                    Ukprn = ukprn,
                    VacancyReferenceNumber = vacancyReferenceNumberOK,
                    Status = ProviderVacancyStatuses.ReservedForQA,
                    QAUserName = username,
                    DateStartedToQA = DateTime.UtcNow
                },
                new ApprenticeshipVacancy
                {
                    ClosingDate = DateTime.Now,
                    DateSubmitted = DateTime.Now,
                    ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                    {
                        Employer = new Employer()
                    },
                    Ukprn = ukprn,
                    VacancyReferenceNumber = vacancyReferenceNumberNonOK,
                    Status = ProviderVacancyStatuses.ReservedForQA,
                    QAUserName = "qa1@test.com",
                    DateStartedToQA = DateTime.UtcNow
                }
            };

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(configurationService)
                    .With(vacancyPostingService)
                    .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);

            //Act
            var vacancies = vacancyProvider.GetPendingQAVacancies();

            //Assert
            vacancies.Should().HaveCount(1);
            vacancies.First().VacancyReferenceNumber.Should().Be(vacancyReferenceNumberOK);
        }

        [Test]
        public void GetPendingQAVacanciesOverviewShouldReturnAllVacancies()
        {
            //Arrange
            var apprenticeshipVacancyRepository = new Mock<IApprenticeshipVacancyReadRepository>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            const string ukprn = "ukprn";
            const int vacancyReferenceNumberOK = 1;
            const int vacancyReferenceNumberNonOK = 2;
            const string username = "qa@test.com";
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            var apprenticeshipVacancies = new List<ApprenticeshipVacancy>
            {
                new ApprenticeshipVacancy
                {
                    ClosingDate = DateTime.Now,
                    DateSubmitted = DateTime.Now,
                    ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                    {
                        Employer = new Employer()
                    },
                    Ukprn = ukprn,
                    VacancyReferenceNumber = vacancyReferenceNumberOK,
                    Status = ProviderVacancyStatuses.ReservedForQA,
                    QAUserName = username,
                    DateStartedToQA = DateTime.UtcNow
                },
                new ApprenticeshipVacancy
                {
                    ClosingDate = DateTime.Now,
                    DateSubmitted = DateTime.Now,
                    ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                    {
                        Employer = new Employer()
                    },
                    Ukprn = ukprn,
                    VacancyReferenceNumber = vacancyReferenceNumberNonOK,
                    Status = ProviderVacancyStatuses.ReservedForQA,
                    QAUserName = "qa1@test.com",
                    DateStartedToQA = DateTime.UtcNow
                }
            };

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(vacancyPostingService)
                    .With(configurationService)
                    .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);

            //Act
            var vacancies = vacancyProvider.GetPendingQAVacanciesOverview(new DashboardVacancySummariesSearchViewModel()).Vacancies;

            //Assert
            vacancies.Should().HaveCount(2);
            vacancies.Count(v => v.CanBeReservedForQaByCurrentUser).Should().Be(1);
            vacancies.Count(v => !v.CanBeReservedForQaByCurrentUser).Should().Be(1);
            vacancies.Single(v => v.CanBeReservedForQaByCurrentUser).VacancyReferenceNumber.Should().Be(vacancyReferenceNumberOK);
            vacancies.Single(v => !v.CanBeReservedForQaByCurrentUser).VacancyReferenceNumber.Should().Be(vacancyReferenceNumberNonOK);
        }

        [Test]
        public void GetPendingQAVacanciesShouldNotReturnVacanciesWithQADateBeforeTimeout()
        {
            //Arrange
            const int GreaterThanTimeout = 20;
            const int LesserThanTimeout = 2;
            const string ukprn = "ukprn";
            const int vacancyReferenceNumberOK = 1;
            const int vacancyReferenceNumberNonOK = 2;

            var apprenticeshipVacancyRepository = new Mock<IApprenticeshipVacancyReadRepository>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var timeService = new Mock<IDateTimeService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });
            timeService.Setup(ts => ts.UtcNow()).Returns(DateTime.UtcNow);
            var apprenticeshipVacancies = new List<ApprenticeshipVacancy>
            {
                new ApprenticeshipVacancy
                {
                    ClosingDate = DateTime.Now,
                    DateSubmitted = DateTime.Now,
                    ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                    {
                        Employer = new Employer()
                    },
                    Ukprn = ukprn,
                    VacancyReferenceNumber = vacancyReferenceNumberOK,
                    QAUserName = "someUserName",
                    DateStartedToQA = DateTime.UtcNow.AddMinutes(-GreaterThanTimeout),
                    Status = ProviderVacancyStatuses.ReservedForQA
                },
                new ApprenticeshipVacancy
                {
                    ClosingDate = DateTime.Now,
                    DateSubmitted = DateTime.Now,
                    ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                    {
                        Employer = new Employer()
                    },
                    Ukprn = ukprn,
                    VacancyReferenceNumber = vacancyReferenceNumberNonOK,
                    QAUserName = "someUserName",
                    DateStartedToQA = DateTime.UtcNow.AddMinutes(-LesserThanTimeout),
                    Status = ProviderVacancyStatuses.ReservedForQA
                }
            };

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(providerService)
                    .With(timeService)
                    .With(vacancyPostingService)
                    .With(configurationService)
                    .Build();

            //Act
            var vacancies = vacancyProvider.GetPendingQAVacancies();

            //Assert
            vacancies.Should().HaveCount(1);
            vacancies.First().VacancyReferenceNumber.Should().Be(vacancyReferenceNumberOK);
            configurationService.Verify(x => x.Get<ManageWebConfiguration>());
        }

        [Test]
        public void ReserveForQA_UsernameIsSavedFromCurrentPrinciple()
        {
            //Arrange
            const long vacancyReferenceNumber = 123456L;
            const string username = "qa@test.com";
            var reservedVacancy =
                new Fixture().Build<ApprenticeshipVacancy>()
                    .With(av => av.Status, ProviderVacancyStatuses.ReservedForQA)
                    .With(av => av.StandardId, null)
                    .Create();
            var vacancyWithReservedStatus = new Fixture().Build<VacancyViewModel>()
                .With(vvm=> vvm.Status, ProviderVacancyStatuses.ReservedForQA)
                .Create();
            var providerSite = new Fixture().Build<ProviderSite>().Create();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            vacancyPostingService.Setup(r => r.ReserveVacancyForQA(vacancyReferenceNumber)).Returns(reservedVacancy);
            var providerService = new Mock<IProviderService>();
            providerService.Setup(s => s.GetProviderSite(It.IsAny<string>(), It.IsAny<string>())).Returns(providerSite);
            var referenceDataService = new Mock<IReferenceDataService>();
            referenceDataService.Setup(s => s.GetSubCategoryByCode(It.IsAny<string>())).Returns(new Category());
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });
            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<ApprenticeshipVacancy, VacancyViewModel>(reservedVacancy))
                .Returns(vacancyWithReservedStatus);

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(referenceDataService)
                    .With(configurationService)
                    .With(vacancyPostingService)
                    .With(mapper)
                    .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);

            //Act
            var vacancy = vacancyProvider.ReserveVacancyForQA(vacancyReferenceNumber);

            //Assert
            vacancyPostingService.Verify();
            vacancy.Status.Should().Be(ProviderVacancyStatuses.ReservedForQA);
        }

        [Test]
        public void ShouldSaveCommentsWhenUpdatingVacancySummaryViewModel()
        {
            const int vacancyReferenceNumber = 1;
            const string closingDateComment = "Closing date comment";
            const string workingWeekComment = "Working week comment";
            const string wageComment = "Wage comment";
            const string durationComment = "Duration comment";
            const string longDescriptionComment = "Long description comment";
            const string possibleStartDateComment = "Possible start date comment";

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var configService = new Mock<IConfigurationService>();
            configService.Setup(m => m.Get<CommonWebConfiguration>()).Returns(new CommonWebConfiguration() {BlacklistedCategoryCodes = string.Empty});
            var provider = new VacancyProviderBuilder().With(vacancyPostingService).With(configService).Build();
            var viewModel = GetValidVacancySummaryViewModel(vacancyReferenceNumber);
            vacancyPostingService.Setup(vp => vp.GetVacancy(vacancyReferenceNumber)).Returns(new ApprenticeshipVacancy());
            vacancyPostingService.Setup(vp => vp.ShallowSaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(new ApprenticeshipVacancy());
            viewModel.VacancyDatesViewModel.ClosingDateComment = closingDateComment;
            viewModel.DurationComment = durationComment;
            viewModel.LongDescriptionComment = longDescriptionComment;
            viewModel.VacancyDatesViewModel.PossibleStartDateComment = possibleStartDateComment;
            viewModel.WageComment = wageComment;
            viewModel.WorkingWeekComment = workingWeekComment;

            provider.UpdateVacancyWithComments(viewModel);

            vacancyPostingService.Verify(vp => vp.GetVacancy(vacancyReferenceNumber));
            vacancyPostingService.Verify(
                vp =>
                    vp.ShallowSaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(
                            v =>
                                v.ClosingDateComment == closingDateComment &&
                                v.DurationComment == durationComment &&
                                v.LongDescriptionComment == longDescriptionComment &&
                                v.PossibleStartDateComment == possibleStartDateComment &&
                                v.WageComment == wageComment &&
                                v.WorkingWeekComment == workingWeekComment)));
        }

        [Test]
        public void ShouldSaveCommentsWhenUpdatingVacancyQuestionsViewModel()
        {
            const int vacancyReferenceNumber = 1;
            const string firstQuestionComment = "First question comment";
            const string secondQuestionComment = "Second question comment";

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var provider = new VacancyProviderBuilder().With(vacancyPostingService).Build();

            vacancyPostingService.Setup(vp => vp.GetVacancy(vacancyReferenceNumber)).Returns(new ApprenticeshipVacancy());
            vacancyPostingService.Setup(vp => vp.ShallowSaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(new ApprenticeshipVacancy());

            var viewModel = new VacancyQuestionsViewModel
            {
                FirstQuestionComment = firstQuestionComment,
                SecondQuestionComment = secondQuestionComment,
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            provider.UpdateVacancyWithComments(viewModel);

            vacancyPostingService.Verify(vp => vp.GetVacancy(vacancyReferenceNumber));
            vacancyPostingService.Verify(
                vp =>
                    vp.ShallowSaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(
                            v =>
                                v.FirstQuestionComment == firstQuestionComment &&
                                v.SecondQuestionComment == secondQuestionComment)));

        }

        private static VacancySummaryViewModel GetValidVacancySummaryViewModel(int vacancyReferenceNumber)
        {
            return new VacancySummaryViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                VacancyDatesViewModel = new VacancyDatesViewModel { 
                    ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(20)),
                    PossibleStartDate = new DateViewModel(DateTime.UtcNow.AddDays(30))
                },
                Duration = 3,
                DurationType = DurationType.Years,
                LongDescription = "A description",
                WageType = WageType.ApprenticeshipMinimumWage,
                HoursPerWeek = 30,
                WorkingWeek = "A working week"
            };
        }
    }

    public static class MoqExtensions
    {
        public static void ReturnsInOrder<T, TResult>(this ISetup<T, TResult> setup,
          params TResult[] results) where T : class
        {
            setup.Returns(new Queue<TResult>(results).Dequeue);
        }
    }
}