﻿namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.UnitTests.Builders;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    public class SubmitVacancyTests : TestsBase
    {
        [Test]
        public void ShouldSubmitVacancy()
        {
            var vvm = new VacancyViewModelBuilder().BuildValid(ProviderVacancyStatuses.Draft);

            VacancyPostingProvider.Setup(p => p.GetVacancy(vvm.VacancyReferenceNumber)).Returns(vvm);
            VacancyPostingProvider.Setup(p => p.SubmitVacancy(It.IsAny<int>())).Returns(vvm);
            var mediator = GetMediator();

            var result = mediator.SubmitVacancy(vvm.VacancyReferenceNumber, false);

            result.Code.Should().Be(VacancyPostingMediatorCodes.SubmitVacancy.SubmitOk);
        }

        [Test]
        public void ShouldResubmitVacancy()
        {
            var vvm = new VacancyViewModelBuilder().BuildValid(ProviderVacancyStatuses.RejectedByQA);
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
            var vvm = new VacancyViewModelBuilder().BuildValid(ProviderVacancyStatuses.RejectedByQA);
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