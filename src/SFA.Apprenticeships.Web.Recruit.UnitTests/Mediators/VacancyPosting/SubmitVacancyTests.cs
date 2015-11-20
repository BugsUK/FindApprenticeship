namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Common.Constants;
    using Common.Mediators;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Raa.Common.Constants.ViewModels;
    using Raa.Common.ViewModels.Provider;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    public class SubmitVacancyTests : TestsBase
    {
        [Test]
        public void ShouldSubmitVacancy()
        {
            var vvm = new Fixture().Build<VacancyViewModel>().Create();

            VacancyPostingProvider.Setup(p => p.GetVacancy(vvm.VacancyReferenceNumber)).Returns(vvm);
            var mediator = GetMediator();

            var result = mediator.SubmitVacancy(vvm.VacancyReferenceNumber, true);

            result.Code.Should().Be(VacancyPostingMediatorCodes.SubmitVacancy.Ok);
        }

        [Test]
        public void ShouldReturnValidationErrorIfNotOptedIn()
        {
            var vvm = new Fixture().Build<VacancyViewModel>().Create();
            vvm.ResubmitOptin = false;

            VacancyPostingProvider.Setup(p => p.GetVacancy(vvm.VacancyReferenceNumber)).Returns(vvm);
            var mediator = GetMediator();

            var result = mediator.SubmitVacancy(vvm.VacancyReferenceNumber, false);

            result.Code.Should().Be(VacancyPostingMediatorCodes.SubmitVacancy.FailedValidation);
            result.ValidationResult.Errors.Should().NotBeNull();
            result.ValidationResult.Errors.Count.Should().Be(1);
        }
    }
}