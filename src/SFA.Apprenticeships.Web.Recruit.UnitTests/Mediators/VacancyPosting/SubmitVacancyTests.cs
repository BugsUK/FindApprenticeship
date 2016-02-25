namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Builders;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    public class SubmitVacancyTests : TestsBase
    {
        [Test]
        public void ShouldSubmitVacancy()
        {
            var vvm = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Draft, VacancyType.Apprenticeship);

            VacancyPostingProvider.Setup(p => p.GetVacancy(vvm.VacancyReferenceNumber)).Returns(vvm);
            VacancyPostingProvider.Setup(p => p.SubmitVacancy(It.IsAny<int>())).Returns(vvm);
            var mediator = GetMediator();

            var result = mediator.SubmitVacancy(vvm.VacancyReferenceNumber, false);

            result.Code.Should().Be(VacancyPostingMediatorCodes.SubmitVacancy.SubmitOk);
        }

        [Test]
        public void ShouldResubmitVacancy()
        {
            var vvm = new VacancyViewModelBuilder().BuildValid(VacancyStatus.RejectedByQA, VacancyType.Apprenticeship);
            vvm.ResubmitOption = true;

            VacancyPostingProvider.Setup(p => p.GetVacancy(vvm.VacancyReferenceNumber)).Returns(vvm);
            VacancyPostingProvider.Setup(p => p.SubmitVacancy(It.IsAny<int>())).Returns(vvm);
            var mediator = GetMediator();

            var result = mediator.SubmitVacancy(vvm.VacancyReferenceNumber, true);

            result.Code.Should().Be(VacancyPostingMediatorCodes.SubmitVacancy.ResubmitOk);
        }

        [Test]
        public void ShouldReturnValidationErrorIfNotOptedIn()
        {
            var vvm = new VacancyViewModelBuilder().BuildValid(VacancyStatus.RejectedByQA, VacancyType.Apprenticeship);
            vvm.ResubmitOption = false;

            VacancyPostingProvider.Setup(p => p.GetVacancy(vvm.VacancyReferenceNumber)).Returns(vvm);
            var mediator = GetMediator();

            var result = mediator.SubmitVacancy(vvm.VacancyReferenceNumber, false);

            result.Code.Should().Be(VacancyPostingMediatorCodes.SubmitVacancy.FailedValidation);
            result.ValidationResult.Errors.Should().NotBeNull();
            result.ValidationResult.Errors.Count.Should().Be(1);
        }
    }
}