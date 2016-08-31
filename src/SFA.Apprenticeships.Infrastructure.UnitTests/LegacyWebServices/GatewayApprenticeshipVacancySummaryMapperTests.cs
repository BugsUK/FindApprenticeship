namespace SFA.Apprenticeships.Infrastructure.UnitTests.LegacyWebServices
{
    using System;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentAssertions;
    using Infrastructure.LegacyWebServices.GatewayServiceProxy;
    using Infrastructure.LegacyWebServices.Mappers;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class GatewayVacancySummaryMapperTests
    {
        [Ignore("These are going to be deleted and are not to be used")]
        [Test]
        public void ShouldCreateAMap()
        {
            // Act.
            new LegacyVacancySummaryMapper().Mapper.AssertConfigurationIsValid();
        }

        [Test]
        public void ShouldMapAllOneToOneFields()
        {
            // Arrange.
            var src = new VacancySummary
            {
                VacancyId = 42,
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                ClosingDate = DateTime.Today.AddDays(1),
                EmployerName = "EmployerName",
                VacancyTitle = "VacancyTitle"
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();

            dest.Id.Should().Be(src.VacancyId);
            dest.ClosingDate.Should().Be(src.ClosingDate ?? DateTime.UtcNow);
            dest.EmployerName.Should().Be(src.EmployerName);
            dest.Title.Should().Be(src.VacancyTitle);
        }

        [Test]
        public void ShouldMapVacancyLocationTypeStandard()
        {
            // Arrange.
            var src = new VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard"
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.NonNational);
        }

        [Test]
        public void ShouldMapVacancyLocationTypeMultipleLocation()
        {
            // Arrange.
            var src = new VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "MultipleLocation"
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.NonNational);
        }

        [Test]
        public void ShouldMapVacancyLocationTypeNational()
        {
            // Arrange.
            var src = new VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "National"
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyLocationType.Should().Be(ApprenticeshipLocationType.National);
        }

        [Test]
        public void ShouldMapVacancyLocationWhenSpecified()
        {
            // Arrange.
            var src = new VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "Standard",
                Address = new VacancySummaryAddress
                {
                    Latitude = 1.0m,
                    Longitude = 2.0m
                }
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();

            dest.Location.Latitude.Should().Be(1.0);
            dest.Location.Longitude.Should().Be(2.0);
        }

        [Test]
        public void ShouldMapNullAddress()
        {
            // Arrange.
            var src = new VacancySummary
            {
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "MultipleLocation",
                Address = null
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.Location.Latitude.Should().Be(0.0);
            dest.Location.Longitude.Should().Be(0.0);
        }

        [Test]
        public void ShouldMapNonNullVacancyReference()
        {
            // Arrange.
            var src = new VacancySummary
            {
                VacancyReference = 42,
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "MultipleLocation"
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyReference.Should().Be("42");
        }

        [Test]
        public void ShouldMapNullVacancyReference()
        {
            // Arrange.
            var src = new VacancySummary
            {
                VacancyReference = null,
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "MultipleLocation"
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.VacancyReference.Should().BeNull();
        }

        [TestCase(5, 5)]
        [TestCase(null, 1)]
        public void ShouldMapNumberOfPositions(int? numberOfPositions, int expectedNumberOfPositions)
        {
            // Arrange.
            var src = new VacancySummary
            {
                VacancyReference = null,
                VacancyType = "IntermediateLevelApprenticeship",
                VacancyLocationType = "MultipleLocation",
                NumberOfPositions = (short?)numberOfPositions
            };

            // Act.
            var dest = new LegacyVacancySummaryMapper().Map<VacancySummary, ApprenticeshipSummary>(src);

            // Assert.
            dest.Should().NotBeNull();
            dest.NumberOfPositions.Should().Be(expectedNumberOfPositions);
        }
    }
}