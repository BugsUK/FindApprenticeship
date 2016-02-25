namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using System;
    using Common.Mappers;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Vacancy;

    [TestFixture]
    public class ApprenticeshipVacancyToVacancyDatesViewModelMapperTests
    {
        private IMapper _mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mapper = new RaaCommonWebMappers();
        }

        [Test]
        public void ShouldMapVacancyReferenceNumber()
        {
            const long vacancyReferenceNumber = 1;

            //Arrange
            var source = new Fixture().Build<Vacancy>()
                .With(av => av.VacancyReferenceNumber, vacancyReferenceNumber).Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyDatesViewModel>(source);

            //Assert
            viewModel.VacancyReferenceNumber.Should().Be(vacancyReferenceNumber);
        }

        [Test]
        public void ShouldMapClosingDate()
        {
            var today = DateTime.Today;
            const string aString = "AString";

            //Arrange
            var source = new Fixture().Build<Vacancy>()
                .With(av => av.ClosingDate, today)
                .With(av => av.ClosingDateComment, aString)
                .Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyDatesViewModel>(source);

            //Assert
            viewModel.ClosingDate.Date.Should().Be(today);
            viewModel.ClosingDateComment.Should().Be(aString);
        }

        [Test]
        public void ShouldMapPossibleStartDate()
        {
            var today = DateTime.Today;
            const string aString = "AString";

            //Arrange
            var source = new Fixture().Build<Vacancy>()
                .With(av => av.PossibleStartDate, today)
                .With(av => av.PossibleStartDateComment, aString)
                .Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyDatesViewModel>(source);

            //Assert
            viewModel.PossibleStartDate.Date.Should().Be(today);
            viewModel.PossibleStartDateComment.Should().Be(aString);
        }
    }
}
