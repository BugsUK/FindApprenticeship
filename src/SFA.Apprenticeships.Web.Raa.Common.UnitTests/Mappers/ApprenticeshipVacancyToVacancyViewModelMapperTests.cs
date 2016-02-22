namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using SFA.Infrastructure.Interfaces;
    using Common.Mappers;
    using Domain.Entities.Raa.Vacancies;
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
            var source = new Fixture().Build<Vacancy>().Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyViewModel>(source);

            //Assert
            viewModel.VacancyReferenceNumber.Should().Be(source.VacancyReferenceNumber);
            viewModel.Status.Should().Be(source.Status);
            viewModel.Should().NotBeNull();
            viewModel.NewVacancyViewModel.Should().NotBeNull();
            viewModel.NewVacancyViewModel.VacancyParty.Should().NotBeNull();
            viewModel.NewVacancyViewModel.VacancyParty.Employer.Should().NotBeNull();
            viewModel.NewVacancyViewModel.VacancyParty.Employer.Address.Should().NotBeNull();
            viewModel.NewVacancyViewModel.VacancyParty.Employer.Address.GeoPoint.Should().NotBeNull();
            viewModel.FurtherVacancyDetailsViewModel.Should().NotBeNull();
            viewModel.VacancyQuestionsViewModel.Should().NotBeNull();
            viewModel.VacancyRequirementsProspectsViewModel.Should().NotBeNull();
        }

        [Test]
        public void NoDurationSpecified()
        {
            //Arrange
            var source = new Fixture().Build<Vacancy>().With(av => av.Duration, null).Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyViewModel>(source);

            //Assert
            viewModel.FurtherVacancyDetailsViewModel.Duration.Should().Be(null);
        }

        [Test]
        public void NoPossibleStartDateSpecified()
        {
            //Arrange
            var source = new Fixture().Build<Vacancy>().With(av => av.PossibleStartDate, null).Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyViewModel>(source);

            //Assert
            viewModel.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate.Should().NotBe(null);
            viewModel.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate.Day.Should().Be(null);
            viewModel.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate.Month.Should().Be(null);
            viewModel.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.PossibleStartDate.Year.Should().Be(null);
        }

        [Test]
        public void NoClosingDateSpecified()
        {
            //Arrange
            var source = new Fixture().Build<Vacancy>().With(av => av.ClosingDate, null).Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyViewModel>(source);

            //Assert
            viewModel.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Should().NotBe(null);
            viewModel.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Day.Should().Be(null);
            viewModel.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Month.Should().Be(null);
            viewModel.FurtherVacancyDetailsViewModel.VacancyDatesViewModel.ClosingDate.Year.Should().Be(null);
        }

        [Test]
        public void OfflineApplicationClickThroughCount()
        {
            //Arrange
            var source = new Fixture().Build<Vacancy>().With(av => av.OfflineApplicationClickThroughCount, 3).Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyViewModel>(source);

            //Assert
            viewModel.OfflineApplicationClickThroughCount.Should().Be(3);
        }
    }
}
