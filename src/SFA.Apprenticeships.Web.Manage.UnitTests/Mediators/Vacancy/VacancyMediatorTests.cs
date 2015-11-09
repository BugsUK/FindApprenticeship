namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.UnitTests.Mediators;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentAssertions;
    using Manage.Mediators.Vacancy;
    using Manage.Providers;
    using Moq;
    using NUnit.Framework;
    using ViewModels;

    [TestFixture]
    public class VacancyMediatorTests
    {
        [Test]
        public void ShouldUpdateTheStatusOfTheVacancyToLive()
        {
            const long vacancyReferenceNumber = 1;
            var pendingQAVacancies = GetPendingVacancies(new[]
            {
                vacancyReferenceNumber
            });
            var provider = new Mock<IVacancyProvider>();
            provider.Setup(p => p.GetPendingQAVacancies()).Returns(pendingQAVacancies.ToList());

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            mediator.ApproveVacancy(vacancyReferenceNumber);

            provider.Verify(p => p.ApproveVacancy(vacancyReferenceNumber), Times.Once);
        }

        [Test]
        public void ShouldUpdateTheStatusOfTheVacancyToDraft()
        {
            //Arrange
            const long vacancyReferenceNumber = 1;
            var pendingQAVacancies = GetPendingVacancies(new[]
            {
                vacancyReferenceNumber
            });
            var provider = new Mock<IVacancyProvider>();
            provider.Setup(p => p.GetPendingQAVacancies()).Returns(pendingQAVacancies.ToList());

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            //Act
            mediator.RejectVacancy(vacancyReferenceNumber);

            //Assert
            provider.Verify(p => p.RejectVacancy(vacancyReferenceNumber), Times.Once);
        }

        [Test]
        public void ShouldReturnTheNextAvailableVacancyAfterApprovingOne()
        {
            const long vacancyReferenceNumber = 1;
            const long nextVacancyReferenceNumber = 2;
            const long anotherVacancyReferenceNumber = 3;

            var pendingQAVacancies = GetPendingVacancies(new[]
            {
                nextVacancyReferenceNumber,
                anotherVacancyReferenceNumber
            });

            var provider = new Mock<IVacancyProvider>();

            provider.Setup(p => p.GetPendingQAVacancies()).Returns(pendingQAVacancies.ToList());

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.ApproveVacancy(vacancyReferenceNumber);
            result.AssertCode(VacancyMediatorCodes.ApproveVacancy.Ok);
            result.ViewModel.VacancyReferenceNumber.Should().Be(nextVacancyReferenceNumber);
        }

        [Test]
        public void ShouldReturnNoAvailableVacanciesIfThereArentAnyAvailableVacancies()
        {
            const long vacancyReferenceNumber = 1;

            List<DashboardVacancySummaryViewModel> pendingQAVacancies = null;

            var provider = new Mock<IVacancyProvider>();

            provider.Setup(p => p.GetPendingQAVacancies()).Returns(pendingQAVacancies);

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.ApproveVacancy(vacancyReferenceNumber);
            result.AssertCode(VacancyMediatorCodes.ApproveVacancy.NoAvailableVacancies);
        }

        private IEnumerable<DashboardVacancySummaryViewModel> GetPendingVacancies(IEnumerable<long> vacancyReferenceNumbers )
        {
            return vacancyReferenceNumbers.Select(vacancyReferenceNumber => new DashboardVacancySummaryViewModel
            {
                VacancyReferenceNumber = vacancyReferenceNumber,
                Status = ProviderVacancyStatuses.PendingQA
            });
        }
    }
}