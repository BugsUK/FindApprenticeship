namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using System.Linq;
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using Constants.ViewModels;
    using FluentAssertions;
    using Manage.Mediators.Vacancy;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class RejectVacancyTests
    {
        [Test]
        public void ShouldGetStatusOkIfNoProblem()
        {
            const int vacancyReferenceNumber = 1;
            var provider = new Mock<IVacancyQAProvider>();
            provider.Setup(p => p.GetNextAvailableVacancy()).Returns(new DashboardVacancySummaryViewModel());

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.RejectVacancy(vacancyReferenceNumber);
            result.Code.Should().Be(VacancyMediatorCodes.RejectVacancy.Ok);
        }

        [Test]
        public void ShouldRejectTheVacancy()
        {
            const int vacancyReferenceNumber = 1;
            var pendingQAVacancies = VacancyMediatorTestHelper.GetPendingVacancies(new[]
            {
                vacancyReferenceNumber
            });
            var provider = new Mock<IVacancyQAProvider>();
            provider.Setup(p => p.GetPendingQAVacancies()).Returns(pendingQAVacancies.ToList());

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            mediator.RejectVacancy(vacancyReferenceNumber);

            provider.Verify(p => p.RejectVacancy(vacancyReferenceNumber), Times.Once);
        }

        [Test]
        public void ShouldGetTheNextVacancyIfVacancyIsRejected()
        {
            const int vacancyReferenceNumber = 1;
            var pendingQAVacancies = VacancyMediatorTestHelper.GetPendingVacancies(new[]
            {
                vacancyReferenceNumber
            });
            var provider = new Mock<IVacancyQAProvider>();
            provider.Setup(p => p.GetPendingQAVacancies()).Returns(pendingQAVacancies.ToList());

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            mediator.RejectVacancy(vacancyReferenceNumber);

            provider.Verify(p => p.GetNextAvailableVacancy(), Times.Once);
        }

        [Test]
        public void ShouldReturnTheNextAvailableVacancyAfterRejectingOne()
        {
            const int vacancyReferenceNumber = 1;
            const int nextVacancyReferenceNumber = 2;

            var provider = new Mock<IVacancyQAProvider>();

            provider.Setup(p => p.GetNextAvailableVacancy())
                .Returns(new DashboardVacancySummaryViewModel {VacancyReferenceNumber = nextVacancyReferenceNumber});

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.RejectVacancy(vacancyReferenceNumber);
            result.AssertCode(VacancyMediatorCodes.RejectVacancy.Ok);
            result.ViewModel.VacancyReferenceNumber.Should().Be(nextVacancyReferenceNumber);
        }

        [Test]
        public void ShouldReturnNoAvailableVacanciesIfThereArentAnyAvailableVacancies()
        {
            const int vacancyReferenceNumber = 1;
            DashboardVacancySummaryViewModel nullVacancy = null;

            var provider = new Mock<IVacancyQAProvider>();

            provider.Setup(p => p.GetNextAvailableVacancy()).Returns(nullVacancy);

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.RejectVacancy(vacancyReferenceNumber);
            result.AssertCode(VacancyMediatorCodes.RejectVacancy.NoAvailableVacancies);
        }

        [Test]
        public void ShouldReturnInvalidVacancyIfTheVacancyIsNotAvailableToQA()
        {
            const int vacancyReferenceNumber = 1;
            var provider = new Mock<IVacancyQAProvider>();

            provider.Setup(p => p.RejectVacancy(vacancyReferenceNumber)).Returns(QAActionResult.InvalidVacancy);

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.RejectVacancy(vacancyReferenceNumber);
            result.AssertMessage(VacancyMediatorCodes.RejectVacancy.InvalidVacancy, VacancyViewModelMessages.InvalidVacancy, UserMessageLevel.Error);
        }
    }
}