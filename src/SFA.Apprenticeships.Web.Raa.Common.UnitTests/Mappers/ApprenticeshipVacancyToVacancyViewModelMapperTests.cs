namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Mapping;
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
    }
}
