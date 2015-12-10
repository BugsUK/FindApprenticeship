namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mappers
{
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Mapping;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Recruit.Mappers;
    using ViewModels.Application;

    [TestFixture]
    public class RecruitMappersTests
    {
        private IMapper _mapper;

        [TestFixtureSetUp]
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
        public void ShouldMapChildObjects()
        {
            //Arrange
            var source = new Fixture().Build<ApprenticeshipVacancy>().Create();

            //Act
            var viewModel = _mapper.Map<ApprenticeshipVacancy, VacancyApplicationsViewModel>(source);

            //Assert
            viewModel.Should().NotBeNull();
            viewModel.VacancyReferenceNumber.Should().Be(source.VacancyReferenceNumber);
            viewModel.Title.Should().Be(source.Title);
            viewModel.EmployerName.Should().Be(source.ProviderSiteEmployerLink.Employer.Name);
            viewModel.ShortDescription.Should().Be(source.ShortDescription);
            viewModel.Status.Should().Be(source.Status);
        }
    }
}