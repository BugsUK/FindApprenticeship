namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using System.Threading.Tasks;
    using Builders;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    [Parallelizable]
    public class SubmitVacancyTests : TestsBase
    {
        [Test]
        public async Task ShouldSubmitVacancy()
        {
            var vvm = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Draft, VacancyType.Apprenticeship);

            VacancyPostingProvider.Setup(p => p.GetVacancy(vvm.VacancyReferenceNumber)).Returns(Task.FromResult(vvm));
            VacancyPostingProvider.Setup(p => p.SubmitVacancy(It.IsAny<int>())).Returns(vvm);
            var mediator = GetMediator();

            var result = await mediator.SubmitVacancy(vvm.VacancyReferenceNumber, false);

            result.Code.Should().Be(VacancyPostingMediatorCodes.SubmitVacancy.SubmitOk);
        }

        [Test]
        public async Task ShouldResubmitVacancy()
        {
            var vvm = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Referred, VacancyType.Apprenticeship);
            vvm.ResubmitOption = true;

            VacancyPostingProvider.Setup(p => p.GetVacancy(vvm.VacancyReferenceNumber)).Returns(Task.FromResult(vvm));
            VacancyPostingProvider.Setup(p => p.SubmitVacancy(It.IsAny<int>())).Returns(vvm);
            var mediator = GetMediator();

            var result = await mediator.SubmitVacancy(vvm.VacancyReferenceNumber, true);

            result.Code.Should().Be(VacancyPostingMediatorCodes.SubmitVacancy.ResubmitOk);
        }

        [Test]
        public async Task ShouldReturnValidationErrorIfNotOptedIn()
        {
            var vvm = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Referred, VacancyType.Apprenticeship);
            vvm.ResubmitOption = false;

            VacancyPostingProvider.Setup(p => p.GetVacancy(vvm.VacancyReferenceNumber)).Returns(Task.FromResult(vvm));
            var mediator = GetMediator();

            var result = await mediator.SubmitVacancy(vvm.VacancyReferenceNumber, false);

            result.Code.Should().Be(VacancyPostingMediatorCodes.SubmitVacancy.FailedValidation);
            result.ValidationResult.Errors.Should().NotBeNull();
            result.ValidationResult.Errors.Count.Should().Be(1);
        }
    }
}