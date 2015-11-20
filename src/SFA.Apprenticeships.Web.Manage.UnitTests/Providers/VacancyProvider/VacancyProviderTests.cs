namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.VacancyProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading;
    using Application.Interfaces.DateTime;
    using Application.Interfaces.Providers;
    using Application.Interfaces.ReferenceData;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Locations;
    using Common.Configuration;
    using Common.ViewModels;
    using Common.ViewModels.Locations;
    using Raa.Common.Configuration;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class VacancyProviderTests
    {
        const int QAVacancyTimeout = 10;

        [Test]
        public void UpdateVacancyBasicDetailsWithComments()
        {
            //Arrange
            const long vacancyReferenceNumber = 1;
            const string ukprn = "ukprn";

            var newVacancyVM = new NewVacancyViewModel()
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                TitleComment = "TitleComment",
                ApprenticeshipLevelComment = "App level comment",
                OfflineApplicationUrlComment = "qerty",
                ShortDescriptionComment = "shorty",
                FrameworkCodeNameComment = "frames",
                OfflineApplicationInstructionsComment = "offlineeee",
                Title = "tit",
                ShortDescription = "sjort",
                OfflineApplicationUrl = "http://www.google.co.uk/",
                OfflineApplicationInstructions = "isntruct me",
                StandardId = 1,
                ProviderSiteEmployerLink = new ProviderSiteEmployerLinkViewModel()
                {
                    Employer = new EmployerViewModel()
                    {
                        Address = new AddressViewModel()
                    }
                }
            };

            var appVacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                ClosingDate = DateTime.Now,
                DateSubmitted = DateTime.Now,
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    Employer = new Employer()
                    {
                        Address = new Address()
                    }
                },
                Ukprn = ukprn,
                Status = ProviderVacancyStatuses.PendingQA
            };

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });
            var referenceDataService = new Mock<IReferenceDataService>();

            var sectorList = new List<Sector>()
            {
                new Sector()
                {
                    Id = 1,
                    Name = "name",
                    Standards = new List<Standard>
                    {
                        new Standard()
                        {
                            ApprenticeshipLevel = ApprenticeshipLevel.Advanced,
                            ApprenticeshipSectorId = 1,
                            Name = "Name"
                        }
                    }
                }
            };

            referenceDataService.Setup(m => m.GetSectors()).Returns(sectorList);

            vacancyPostingService.Setup(
                vps => vps.GetVacancy(vacancyReferenceNumber)).Returns(appVacancy);

            vacancyPostingService.Setup(vps => vps.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>())).Returns(appVacancy);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .With(referenceDataService)
                    .Build();

            //Act
            var result = vacancyProvider.UpdateVacancyWithComments(newVacancyVM);

            //Assert
            vacancyPostingService.Verify(vps => vps.GetVacancy(vacancyReferenceNumber), Times.Once);
            vacancyPostingService.Verify(vps => vps.SaveApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(av => av.VacancyReferenceNumber == vacancyReferenceNumber)));
            result.VacancyReferenceNumber.Should().Be(newVacancyVM.VacancyReferenceNumber);
            result.ApprenticeshipLevelComment.Should().Be(newVacancyVM.ApprenticeshipLevelComment);
            result.FrameworkCodeNameComment.Should().Be(newVacancyVM.FrameworkCodeNameComment);
            result.OfflineApplicationInstructionsComment.Should().Be(newVacancyVM.OfflineApplicationInstructionsComment);
            result.OfflineApplicationUrlComment.Should().Be(newVacancyVM.OfflineApplicationUrlComment);
            result.ShortDescriptionComment.Should().Be(newVacancyVM.ShortDescriptionComment);
            result.TitleComment.Should().Be(newVacancyVM.TitleComment);
            result.ApprenticeshipLevel.Should().Be(newVacancyVM.ApprenticeshipLevel);
            result.FrameworkCodeName.Should().Be(newVacancyVM.FrameworkCodeName);
            result.OfflineApplicationInstructions.Should().Be(newVacancyVM.OfflineApplicationInstructions);
            result.OfflineApplicationUrl.Should().Be(newVacancyVM.OfflineApplicationUrl);
            result.ShortDescription.Should().Be(newVacancyVM.ShortDescription);
            result.Title.Should().Be(newVacancyVM.Title);
        }

        [Test]
        public void UpdateVacancyBasicDetailsShouldExpectVacancyReferenceNumber()
        {
            //Arrange
            var newVacancyVM = new NewVacancyViewModel()
            {
                TitleComment = "TitleComment",
                ApprenticeshipLevelComment = "App level comment",
                OfflineApplicationUrlComment = "qerty",
                ShortDescriptionComment = "shorty",
                FrameworkCodeNameComment = "frames",
                OfflineApplicationInstructionsComment = "offlineeee",
                Title = "tit",
                ShortDescription = "sjort",
                OfflineApplicationUrl = "www.google.co.uk",
                OfflineApplicationInstructions = "isntruct me"
            };

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
        public void UpdateVacancyRequirementsAndProspectsWithComments()
        {
            //Arrange
            const long vacancyReferenceNumber = 1;
            const string ukprn = "ukprn";

            var vacancyVm = new VacancyRequirementsProspectsViewModel()
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                DesiredQualifications = "desi",
                DesiredQualificationsComment = "desiComment",
                FutureProspects = "fp",
                FutureProspectsComment = "fpc",
                DesiredSkills = "ds",
                DesiredSkillsComment = "ds com",
                PersonalQualities = "p quals",
                PersonalQualitiesComment = "p quals comm",
                ThingsToConsider = "considerations",
                ThingsToConsiderComment = "rttt"
            };

            var appVacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                ClosingDate = DateTime.Now,
                DateSubmitted = DateTime.Now,
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    Employer = new Employer()
                    {
                        Address = new Address()
                    }
                },
                Ukprn = ukprn,
                Status = ProviderVacancyStatuses.PendingQA
            };

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var providerService = new Mock<IProviderService>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            vacancyPostingService.Setup(
                vps => vps.GetVacancy(vacancyReferenceNumber)).Returns(appVacancy);

            vacancyPostingService.Setup(vps => vps.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>())).Returns(appVacancy);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder().With(vacancyPostingService)
                    .With(providerService)
                    .With(configurationService)
                    .Build();

            //Act
            var result = vacancyProvider.UpdateVacancyWithComments(vacancyVm);

            //Assert
            vacancyPostingService.Verify(vps => vps.GetVacancy(vacancyReferenceNumber), Times.Once);
            vacancyPostingService.Verify(vps => vps.SaveApprenticeshipVacancy(It.Is<ApprenticeshipVacancy>(av => av.VacancyReferenceNumber == vacancyReferenceNumber)));
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
            var providerService = new Mock<IProviderService>();
            const string ukprn = "ukprn";
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            apprenticeshipVacancyRepository.Setup(
                avr => avr.GetWithStatus(new List<ProviderVacancyStatuses> { ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA }))
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
                new VacancyProviderBuilder().With(apprenticeshipVacancyRepository)
                    .With(providerService)
                    .With(configurationService)
                    .Build();

            //Act
            vacancyProvider.GetPendingQAVacancies();

            //Assert
            apprenticeshipVacancyRepository.Verify(avr => avr.GetWithStatus(new List<ProviderVacancyStatuses> { ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA }));
            providerService.Verify(ps => ps.GetProvider(ukprn), Times.Once);
        }

        [Test]
        public void ApproveVacancyShouldCallRepositorySaveWithStatusAsLive()
        {
            //Arrange
            long vacancyReferenceNumber = 1;
            var vacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();
            var apprenticeshipVacancyWriteRepository = new Mock<IApprenticeshipVacancyWriteRepository>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            apprenticeshipVacancyReadRepository.Setup(r => r.Get(vacancyReferenceNumber)).Returns(vacancy);
            var vacancyProvider =
                new VacancyProviderBuilder().With(apprenticeshipVacancyWriteRepository)
                    .With(apprenticeshipVacancyReadRepository)
                    .With(configurationService)
                    .Build();

            //Act
            vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

            //Assert
            apprenticeshipVacancyReadRepository.Verify(r => r.Get(vacancyReferenceNumber));
            apprenticeshipVacancyWriteRepository.Verify(
                r =>
                    r.Save(
                        It.Is<ApprenticeshipVacancy>(
                            av =>
                                av.VacancyReferenceNumber == vacancyReferenceNumber &&
                                av.Status == ProviderVacancyStatuses.Live)));
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

            var apprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();
            var apprenticeshipVacancyWriteRepository = new Mock<IApprenticeshipVacancyWriteRepository>();
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            apprenticeshipVacancyReadRepository.Setup(r => r.Get(vacancyReferenceNumber)).Returns(vacancy);
            var vacancyProvider =
                new VacancyProviderBuilder().With(apprenticeshipVacancyWriteRepository)
                    .With(apprenticeshipVacancyReadRepository)
                    .With(configurationService)
                    .Build();

            //Act
            vacancyProvider.RejectVacancy(vacancyReferenceNumber);

            //Assert
            apprenticeshipVacancyReadRepository.Verify(r => r.Get(vacancyReferenceNumber));
            apprenticeshipVacancyWriteRepository.Verify(
                r =>
                    r.Save(
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
            var apprenticeshipVacancyRepository = new Mock<IApprenticeshipVacancyReadRepository>();
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

            apprenticeshipVacancyRepository.Setup(
                avr => avr.GetWithStatus(new List<ProviderVacancyStatuses> { ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA }))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder().With(apprenticeshipVacancyRepository)
                    .With(providerService)
                    .With(configurationService)
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

            apprenticeshipVacancyRepository.Setup(
                avr => avr.GetWithStatus(new List<ProviderVacancyStatuses> { ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA }))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder().With(apprenticeshipVacancyRepository)
                    .With(providerService)
                    .With(configurationService)
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

            apprenticeshipVacancyRepository.Setup(
                avr => avr.GetWithStatus(new List<ProviderVacancyStatuses> { ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA }))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder().With(apprenticeshipVacancyRepository)
                    .With(providerService)
                    .With(configurationService)
                    .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);

            //Act
            var vacancies = vacancyProvider.GetPendingQAVacanciesOverview();

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

            apprenticeshipVacancyRepository.Setup(
                avr => avr.GetWithStatus(new List<ProviderVacancyStatuses> { ProviderVacancyStatuses.PendingQA, ProviderVacancyStatuses.ReservedForQA }))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder().With(apprenticeshipVacancyRepository)
                    .With(providerService)
                    .With(timeService)
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
            var providerSite = new Fixture().Build<ProviderSite>().Create();
            var apprenticeshipVacancyWriteRepository = new Mock<IApprenticeshipVacancyWriteRepository>();
            apprenticeshipVacancyWriteRepository.Setup(r => r.ReserveVacancyForQA(vacancyReferenceNumber)).Returns(reservedVacancy);
            var providerService = new Mock<IProviderService>();
            providerService.Setup(s => s.GetProviderSite(It.IsAny<string>(), It.IsAny<string>())).Returns(providerSite);
            var referenceDataService = new Mock<IReferenceDataService>();
            referenceDataService.Setup(s => s.GetSubCategoryByCode(It.IsAny<string>())).Returns(new Category());
            var configurationService = new Mock<IConfigurationService>();
            configurationService.Setup(x => x.Get<ManageWebConfiguration>())
                .Returns(new ManageWebConfiguration { QAVacancyTimeout = QAVacancyTimeout });
            configurationService.Setup(x => x.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration { BlacklistedCategoryCodes = "" });

            var vacancyProvider =
                new VacancyProviderBuilder().With(apprenticeshipVacancyWriteRepository)
                    .With(providerService)
                    .With(referenceDataService)
                    .With(configurationService)
                    .Build();

            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);

            //Act
            var vacancy = vacancyProvider.ReserveVacancyForQA(vacancyReferenceNumber);

            //Assert
            apprenticeshipVacancyWriteRepository.Verify();
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
            vacancyPostingService.Setup(vp => vp.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(new ApprenticeshipVacancy());
            viewModel.ClosingDateComment = closingDateComment;
            viewModel.DurationComment = durationComment;
            viewModel.LongDescriptionComment = longDescriptionComment;
            viewModel.PossibleStartDateComment = possibleStartDateComment;
            viewModel.WageComment = wageComment;
            viewModel.WorkingWeekComment = workingWeekComment;

            provider.UpdateVacancyWithComments(viewModel);

            vacancyPostingService.Verify(vp => vp.GetVacancy(vacancyReferenceNumber));
            vacancyPostingService.Verify(
                vp =>
                    vp.SaveApprenticeshipVacancy(
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
            vacancyPostingService.Setup(vp => vp.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
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
                    vp.SaveApprenticeshipVacancy(
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
                ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(20)),
                PossibleStartDate = new DateViewModel(DateTime.UtcNow.AddDays(30)),
                Duration = 3,
                DurationType = DurationType.Years,
                LongDescription = "A description",
                WageType = WageType.ApprenticeshipMinimumWage,
                HoursPerWeek = 30,
                WorkingWeek = "A working week"
            };
        }
    }
}