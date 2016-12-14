namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System.Threading.Tasks;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    [Parallelizable]
    public class CloneVacancyTests : TestsBase
    {
        [Test]
        public async Task ShouldReturnVacancyInIncorrectStateErrorCodeIfVacancyIsInReferredStatus()
        {
            const int vacancyReferenceNumber = 1;

            VacancyPostingProvider.Setup(p => p.GetVacancy(vacancyReferenceNumber)).Returns(Task.FromResult(new VacancyViewModel {Status = VacancyStatus.Referred}));
            var mediator = GetMediator();

            var result = await mediator.CloneVacancy(vacancyReferenceNumber);

            result.Code.Should().Be(VacancyPostingMediatorCodes.CloneVacancy.VacancyInIncorrectState);
        }
    }
}