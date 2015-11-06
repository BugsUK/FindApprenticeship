namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.VacancyProvider
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Providers;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Repositories;
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
    }
}