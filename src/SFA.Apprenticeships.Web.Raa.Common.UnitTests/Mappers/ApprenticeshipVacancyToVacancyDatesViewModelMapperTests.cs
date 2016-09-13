namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using System;
    using Common.Mappers;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Vacancy;

    [TestFixture]
    [Parallelizable]
    public class ApprenticeshipVacancyToVacancyDatesViewModelMapperTests
    {
        private IMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new RaaCommonWebMappers();
        }

        [Test]
        public void ShouldMapVacancyStatus()
        {
            const VacancyStatus vacancyStatus = VacancyStatus.Live;

            //Arrange
            var source = new Fixture()
                .Build<Vacancy>()
                .With(av => av.Status, vacancyStatus)
                .Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyDatesViewModel>(source);

            //Assert
            viewModel.VacancyStatus.Should().Be(vacancyStatus);
        }

        [Test]
        public void ShouldMapClosingDateAndComment()
        {
            var today = DateTime.Today;
            const string comment = "AString";

            //Arrange
            var source = new Fixture()
                .Build<Vacancy>()
                .With(av => av.ClosingDate, today)
                .With(av => av.ClosingDateComment, comment)
                .Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyDatesViewModel>(source);

            //Assert
            viewModel.ClosingDate.Date.Should().Be(today);
            viewModel.ClosingDateComment.Should().Be(comment);
        }

        [Test]
        public void ShouldMapPossibleStartDateAndComment()
        {
            var today = DateTime.Today;
            const string comment = "AString";

            //Arrange
            var source = new Fixture()
                .Build<Vacancy>()
                .With(av => av.PossibleStartDate, today)
                .With(av => av.PossibleStartDateComment, comment)
                .Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyDatesViewModel>(source);

            //Assert
            viewModel.PossibleStartDate.Date.Should().Be(today);
            viewModel.PossibleStartDateComment.Should().Be(comment);
        }
    }
}
