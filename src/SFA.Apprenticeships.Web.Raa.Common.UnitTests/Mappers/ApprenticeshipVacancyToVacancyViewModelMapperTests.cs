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
        private IMapper mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            mapper = new RaaCommonWebMappers();
        }

        [Test]
        public void ShouldMapChildObjects()
        {
            //Arrange
            var source = new Fixture().Build<ApprenticeshipVacancy>().Create();
            VacancyViewModel result = null;

            //Act
            var vacancyVM = mapper.Map<ApprenticeshipVacancy, VacancyViewModel>(source);

            //Assert
            vacancyVM.VacancyReferenceNumber.Should().Be(source.VacancyReferenceNumber);
            vacancyVM.Status.Should().Be(source.Status);
            vacancyVM.Should().NotBeNull();
            vacancyVM.NewVacancyViewModel.Should().NotBeNull();
            vacancyVM.NewVacancyViewModel.ProviderSiteEmployerLink.Should().NotBeNull();
            vacancyVM.NewVacancyViewModel.ProviderSiteEmployerLink.Employer.Should().NotBeNull();
            vacancyVM.NewVacancyViewModel.ProviderSiteEmployerLink.Employer.Address.Should().NotBeNull();
            vacancyVM.NewVacancyViewModel.ProviderSiteEmployerLink.Employer.Address.GeoPoint.Should().NotBeNull();
            vacancyVM.VacancySummaryViewModel.Should().NotBeNull();
            vacancyVM.VacancyQuestionsViewModel.Should().NotBeNull();
            vacancyVM.VacancyRequirementsProspectsViewModel.Should().NotBeNull();
        }
    }
}
