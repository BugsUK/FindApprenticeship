namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using ViewModels.Admin;

    [TestFixture]
    [Parallelizable]
    public class StandardMappersTests
    {
        [Test]
        public void ShouldCreateMap()
        {
            new StandardMappers().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ViewModelToDomainMapperTest()
        {
            var viewModel = new StandardViewModel
            {
                ApprenticeshipLevel = ApprenticeshipLevel.FoundationDegree,
                ApprenticeshipSectorId = 42,
                Name = "Test"
            };

            var mapper = new StandardMappers();

            var standard = mapper.Map<StandardViewModel, Standard>(viewModel);

            standard.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.FoundationDegree);
            standard.ApprenticeshipSectorId.Should().Be(42);
            standard.Name.Should().Be("Test");
        }

        [Test]
        public void DomainToViewModelMapperTest()
        {
            //TODO: Test the reverse of above
            var model = new Standard
            {
                ApprenticeshipLevel = ApprenticeshipLevel.Advanced,
                ApprenticeshipSectorId = 23,
                Name = "Test"
            };

            var mapper = new StandardMappers();

            var standard = mapper.Map<Standard, StandardViewModel>(model);

            standard.ApprenticeshipLevel.Should().Be(ApprenticeshipLevel.Advanced);
            standard.ApprenticeshipSectorId.Should().Be(23);
            standard.Name.Should().Be("Test");
        }
    }
}