namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using Common.Constants;
    using Common.UnitTests.Mediators;
    using FluentAssertions;
    using Manage.Mediators.Vacancy;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    [Parallelizable]
    public class ReserveForQATests
    {
        [Test]
        public void ShouldIncludeAMessageIfTheVacancyReservedIsNotTheRequestedOne()
        {
            const int vacancyReferenceNumber = 1;
            const int anotherVacancyReferenceNumber = 2;
            var viewModel = VacancyMediatorTestHelper.GetValidVacancyViewModel(anotherVacancyReferenceNumber);

            var provider = new Mock<IVacancyQAProvider>();

            provider.Setup(p => p.ReserveVacancyForQA(vacancyReferenceNumber)).Returns(viewModel);

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.ReserveVacancyForQA(vacancyReferenceNumber);

            result.Code.Should().Be(VacancyMediatorCodes.ReserveVacancyForQA.NextAvailableVacancy);
            result.ViewModel.VacancyReferenceNumber.Should().NotBe(vacancyReferenceNumber);
            result.ViewModel.VacancyReferenceNumber.Should().Be(anotherVacancyReferenceNumber);
            result.AssertMessage(VacancyMediatorCodes.ReserveVacancyForQA.NextAvailableVacancy,
                "The first vacancy you chose is already being reviewed by another adviser. This is the next available option.",
                UserMessageLevel.Info);
        }

        [Test]
        public void ShouldNotIncludeAnyMessageIfTheVacancyReservedIsTheRequestedOne()
        {
            const int vacancyReferenceNumber = 1;
            var viewModel = VacancyMediatorTestHelper.GetValidVacancyViewModel(vacancyReferenceNumber);

            var provider = new Mock<IVacancyQAProvider>();

            provider.Setup(p => p.ReserveVacancyForQA(vacancyReferenceNumber)).Returns(viewModel);

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.ReserveVacancyForQA(vacancyReferenceNumber);

            result.AssertCode(VacancyMediatorCodes.ReserveVacancyForQA.Ok);
            result.ViewModel.VacancyReferenceNumber.Should().Be(vacancyReferenceNumber);
        }

        [Test]
        public void ShouldReturnAMessageIfNoVacancyIsAvailable()
        {
            const int vacancyReferenceNumber = 1;
            VacancyViewModel viewModel = null;

            var provider = new Mock<IVacancyQAProvider>();
            provider.Setup(p => p.ReserveVacancyForQA(vacancyReferenceNumber)).Returns(viewModel);

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            var result = mediator.ReserveVacancyForQA(vacancyReferenceNumber);

            result.AssertMessage(VacancyMediatorCodes.ReserveVacancyForQA.NoVacanciesAvailable,
                "All vacancies have been reviewed.", UserMessageLevel.Info);
            result.ViewModel.Should().BeNull();
        }
    }
}