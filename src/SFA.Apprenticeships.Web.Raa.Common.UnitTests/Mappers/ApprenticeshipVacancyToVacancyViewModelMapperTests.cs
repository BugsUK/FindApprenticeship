namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using SFA.Infrastructure.Interfaces;
    using Common.Mappers;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using ViewModels.Vacancy;

    [TestFixture]
    public class ApprenticeshipVacancyToVacancyViewModelMapperTests
    {
        private IMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new RaaCommonWebMappers();
        }

        [Test]
        public void ShouldMapChildObjects()
        {
            //Arrange
            var source = new Fixture().Build<ApprenticeshipVacancy>().Create();

            //Act
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyViewModel>(source);

            //Assert
            viewModel.VacancyReferenceNumber.Should().Be(source.VacancyReferenceNumber);
            viewModel.Status.Should().Be(source.Status);
            viewModel.Should().NotBeNull();
            viewModel.NewVacancyViewModel.Should().NotBeNull();
            viewModel.NewVacancyViewModel.ProviderSiteEmployerLink.Should().NotBeNull();
            viewModel.NewVacancyViewModel.ProviderSiteEmployerLink.Employer.Should().NotBeNull();
            viewModel.NewVacancyViewModel.ProviderSiteEmployerLink.Employer.Address.Should().NotBeNull();
            viewModel.NewVacancyViewModel.ProviderSiteEmployerLink.Employer.Address.GeoPoint.Should().NotBeNull();
            viewModel.VacancySummaryViewModel.Should().NotBeNull();
            viewModel.VacancyQuestionsViewModel.Should().NotBeNull();
            viewModel.VacancyRequirementsProspectsViewModel.Should().NotBeNull();
        }

        [Test]
        public void NoDurationSpecified()
        {
            //Arrange
            var source = new Fixture().Build<ApprenticeshipVacancy>().With(av => av.Duration, null).Create();

            //Act
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyViewModel>(source);

            //Assert
            viewModel.VacancySummaryViewModel.Duration.Should().Be(null);
        }

        [Test]
        public void NoPossibleStartDateSpecified()
        {
            //Arrange
            var source = new Fixture().Build<ApprenticeshipVacancy>().With(av => av.PossibleStartDate, null).Create();

            //Act
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyViewModel>(source);

            //Assert
            viewModel.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate.Should().NotBe(null);
            viewModel.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate.Day.Should().Be(null);
            viewModel.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate.Month.Should().Be(null);
            viewModel.VacancySummaryViewModel.VacancyDatesViewModel.PossibleStartDate.Year.Should().Be(null);
        }

        [Test]
        public void NoClosingDateSpecified()
        {
            //Arrange
            var source = new Fixture().Build<ApprenticeshipVacancy>().With(av => av.ClosingDate, null).Create();

            //Act
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyViewModel>(source);

            //Assert
            viewModel.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Should().NotBe(null);
            viewModel.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Day.Should().Be(null);
            viewModel.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Month.Should().Be(null);
            viewModel.VacancySummaryViewModel.VacancyDatesViewModel.ClosingDate.Year.Should().Be(null);
        }

        [Test]
        public void OfflineApplicationClickThroughCount()
        {
            //Arrange
            var source = new Fixture().Build<ApprenticeshipVacancy>().With(av => av.OfflineApplicationClickThroughCount, 3).Create();

            //Act
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyViewModel>(source);

            //Assert
            viewModel.OfflineApplicationClickThroughCount.Should().Be(3);
        }
    }
}
