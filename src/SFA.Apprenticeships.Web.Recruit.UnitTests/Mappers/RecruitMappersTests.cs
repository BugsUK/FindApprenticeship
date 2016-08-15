namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mappers
{
    using SFA.Infrastructure.Interfaces;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Raa.Common.ViewModels.Application;
    using Recruit.Mappers;

    [TestFixture]
    [Parallelizable]
    public class RecruitMappersTests
    {
        private IMapper _mapper;

        [OneTimeSetUp]
        public void Setup()
        {
            _mapper = new RecruitMappers();
        }

        [Test]
        public void ShouldCreateMap()
        {
            new RecruitMappers().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapVacancyApplicationsViewModel()
        {
            //Arrange
            var source = new Fixture().Build<Vacancy>().Create();

            //Act
            var viewModel = _mapper.Map<Vacancy, VacancyApplicationsViewModel>(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.Title.Should().Be(source.Title);
            viewModel.ShortDescription.Should().Be(source.ShortDescription);
            viewModel.Status.Should().Be(source.Status);
        }

        [Test]
        public void ShouldMapApprenticeshipApplicationSummary()
        {
            //Arrange
            var source = new Fixture().Build<ApprenticeshipApplicationSummary>().Create();

            //Act
            var viewModel = _mapper.Map<ApprenticeshipApplicationSummary, ApplicationSummaryViewModel>(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.FirstName.Should().Be(source.CandidateDetails.FirstName);
            viewModel.LastName.Should().Be(source.CandidateDetails.LastName);
            viewModel.ApplicantName.Should().Be(source.CandidateDetails.FirstName + " " + source.CandidateDetails.LastName);
            viewModel.DateApplied.Should().Be(source.DateApplied.Value);
            viewModel.Status.Should().Be(source.Status);
        }
   }
}
