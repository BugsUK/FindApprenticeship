namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System.Collections.Generic;
    using Application.Interfaces.Providers;
    using Application.Interfaces.Vacancies;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class GetNextAvailableVacancy
    {
        [Test]
        public void GetNextAvailableVacancyShouldReturnNullIfThereIsntAnyMoreVacancies()
        {
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var vacancyLockingService = new Mock<IVacancyLockingService>();
            var providerService = new Mock<IProviderService>();
            VacancySummary vacancySummary = null;

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                .Returns(new List<VacancySummary>());

            vacancyLockingService.Setup(
                vls => vls.GetNextAvailableVacancy(It.IsAny<string>(), It.IsAny<List<VacancySummary>>()))
                .Returns(vacancySummary);

            providerService.Setup(ps => ps.GetProvider(It.IsAny<int>())).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(vacancyPostingService)
                    .With(vacancyLockingService)
                    .With(providerService)
                    .Build();

            var result = vacancyProvider.GetNextAvailableVacancy();

            result.Should().BeNull();
        }

        [Test]
        public void GetNextAvailableVacancyShouldReturnTheNextVacancyIfThereIsAnyAvailable()
        {
            const int vacancyReferenceNumber = 1;

            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var vacancyLockingService = new Mock<IVacancyLockingService>();
            var providerService = new Mock<IProviderService>();
            var vacancySummary = new VacancySummary { VacancyReferenceNumber = vacancyReferenceNumber };

            vacancyPostingService.Setup(
                avr => avr.GetWithStatus(VacancyStatus.Submitted, VacancyStatus.ReservedForQA))
                .Returns(new List<VacancySummary>());

            vacancyLockingService.Setup(
                vls => vls.GetNextAvailableVacancy(It.IsAny<string>(), It.IsAny<List<VacancySummary>>()))
                .Returns(vacancySummary);

            providerService.Setup(ps => ps.GetProvider(It.IsAny<int>())).Returns(new Provider());

            var vacancyProvider =
                new VacancyProviderBuilder()
                    .With(vacancyPostingService)
                    .With(vacancyLockingService)
                    .With(providerService)
                    .Build();

            var result = vacancyProvider.GetNextAvailableVacancy();

            result.VacancyReferenceNumber.Should().Be(vacancyReferenceNumber);
        }
    }
}