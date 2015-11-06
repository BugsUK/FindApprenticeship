namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.VacancyProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.DateTime;
    using Application.Interfaces.Providers;
    using Common.Configuration;
    using Configuration;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyProviderTests
    {
        [Test]
        public void GetVacanciesPendingQAShouldCallRepositoryWithPendingQAAsDesiredStatus()
        {
            var apprenticeshipVacancyRepository = new Mock<IApprenticeshipVacancyReadRepository>();
            var providerService = new Mock<IProviderService>();
            var ukprn = "ukprn";

            apprenticeshipVacancyRepository.Setup(
                avr => avr.GetWithStatus(new List<ProviderVacancyStatuses> {ProviderVacancyStatuses.PendingQA}))
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
                        Ukprn = ukprn
                    }
                });

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider = new VacancyProviderBuilder().With(apprenticeshipVacancyRepository).With(providerService).Build();
            vacancyProvider.GetPendingQAVacancies();

            apprenticeshipVacancyRepository.Verify(avr => avr.GetWithStatus(new List<ProviderVacancyStatuses> {ProviderVacancyStatuses.PendingQA}));
            providerService.Verify(ps => ps.GetProvider(ukprn), Times.Once);
        }

        [Test]
        public void AppoveVacancyShouldCallRepositorySaveWithStatusAsLive()
        {
            long vacancyReferenceNumber = 1;
            var vacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = vacancyReferenceNumber
            };

            var apprenticeshipVacancyReadRepository = new Mock<IApprenticeshipVacancyReadRepository>();
            var apprenticeshipVacancyWriteRepository = new Mock<IApprenticeshipVacancyWriteRepository>();

            apprenticeshipVacancyReadRepository.Setup(r => r.Get(vacancyReferenceNumber)).Returns(vacancy);
            var vacancyProvider =
                new VacancyProviderBuilder().With(apprenticeshipVacancyWriteRepository)
                    .With(apprenticeshipVacancyReadRepository)
                    .Build();

            vacancyProvider.ApproveVacancy(vacancyReferenceNumber);

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
        public void GetPendingQAVacanciesShouldReturnVacanciesWithoutQAUserName()
        {
            var apprenticeshipVacancyRepository = new Mock<IApprenticeshipVacancyReadRepository>();
            var providerService = new Mock<IProviderService>();
            var ukprn = "ukprn";

            var vacancyReferenceNumber = 1;
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
                avr => avr.GetWithStatus(new List<ProviderVacancyStatuses> { ProviderVacancyStatuses.PendingQA }))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider = new VacancyProviderBuilder().With(apprenticeshipVacancyRepository).With(providerService).Build();

            var vacancies = vacancyProvider.GetPendingQAVacancies();
            vacancies.Should().HaveCount(1);
            vacancies.First().VacancyReferenceNumber.Should().Be(vacancyReferenceNumber);
        }

        [Test]
        public void GetPendingQAVacanciesShouldNotReturnVacanciesWithQADateBeforeTimeout()
        {
            const int QAVacancyTimeout = 10;
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
                    Status = ProviderVacancyStatuses.PendingQA
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
                    Status = ProviderVacancyStatuses.PendingQA
                }
            };

            apprenticeshipVacancyRepository.Setup(
                avr => avr.GetWithStatus(new List<ProviderVacancyStatuses> { ProviderVacancyStatuses.PendingQA }))
                .Returns(apprenticeshipVacancies);

            providerService.Setup(ps => ps.GetProvider(ukprn)).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder().With(apprenticeshipVacancyRepository)
                    .With(providerService)
                    .With(timeService)
                    .With(configurationService)
                    .Build();

            var vacancies = vacancyProvider.GetPendingQAVacancies();
            vacancies.Should().HaveCount(1);
            vacancies.First().VacancyReferenceNumber.Should().Be(vacancyReferenceNumberOK);
            configurationService.Verify(x => x.Get<ManageWebConfiguration>());
        }
    }
}