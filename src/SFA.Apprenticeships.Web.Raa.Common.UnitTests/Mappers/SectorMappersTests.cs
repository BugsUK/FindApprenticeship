namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using Common.Mappers;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using ViewModels.Admin;

    [TestFixture]
    [Parallelizable]
    public class SectorMappersTests
    {
        [Test]
        public void ShouldCreateMap()
        {
            new SectorMapper().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ViewModelToDomainMapperTest()
        {
            var viewModel = new SectorViewModel()
            {
                ApprenticeshipOccupationId = 1,
                Name = "Test"
            };

            var mapper = new SectorMapper();

            var standard = mapper.Map<SectorViewModel, Sector>(viewModel);

            standard.ApprenticeshipOccupationId.Should().Be(1);
            standard.Name.Should().Be("Test");
        }

        [Test]
        public void DomainToViewModelMapperTest()
        {
            //TODO: Test the reverse of above
            var model = new Sector()
            {
                ApprenticeshipOccupationId = 3,
                Name = "Test"
            };

            var mapper = new SectorMapper();

            var standard = mapper.Map<Sector, SectorViewModel>(model);

            standard.ApprenticeshipOccupationId.Should().Be(3);
            standard.Name.Should().Be("Test");
        }
    }
}