using SFA.Apprenticeships.Application.Interfaces.VacancyPosting;
using SFA.Apprenticeships.Domain.Entities.Locations;
using SFA.Apprenticeships.Web.Common.Configuration;
using SFA.Apprenticeships.Web.Common.ViewModels.Locations;
using SFA.Apprenticeships.Web.Raa.Common.Configuration;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Provider;
using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

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

    [TestFixture]
    public class VacancyProviderTests
    {
        const int QAVacancyTimeout = 10;

        [Test]
        public void UpdateVacancyWithComments()
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
                OfflineApplicationUrl = "www.google.co.uk",
                OfflineApplicationInstructions = "isntruct me",
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
            var result = vacancyProvider.UpdateVacancy(newVacancyVM);

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
        }

        [Test]
        public void UpdateVacancyShouldExpectVacancyReferenceNumber()
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
            Action action = () => vacancyProvider.UpdateVacancy(newVacancyVM);

            //Assert
            action.ShouldThrow<ArgumentNullException>();
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
                                av.Status == ProviderVacancyStatuses.RejectedByQA)));
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
            apprenticeshipVacancyWriteRepository.Setup(r => r.ReserveVacancyForQA(vacancyReferenceNumber, username)).Returns(reservedVacancy);
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
    }
}