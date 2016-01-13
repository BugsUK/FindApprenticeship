namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Mappers
{
    using System;
    using SFA.Infrastructure.Interfaces;
    using Common.Mappers;
    using FluentAssertions;
    using NUnit.Framework;
    using Web.Common.ViewModels;

    [TestFixture]
    public class DateTimeToDateViewModelMapperTests
    {
        private IMapper mapper;

        [TestFixtureSetUp]
        public void Setup()
        {
            mapper = new RaaCommonWebMappers();
        }

        [TestCase(1, 1, 2000)]
        [TestCase(31, 12, 2070)]
        [TestCase(29, 2, 2012)] 
        public void ShouldMapDayMonthYear(int day, int month, int year)
        {
            //Arrange
            var dt = new DateTime(year, month, day);
            DateTime? source = new DateTime?(dt);
            DateViewModel destination = null;

            //Act
            destination = mapper.Map<DateTime?, DateViewModel>(source);

            //Assert
            destination.Day.Should().Be(source.Value.Day);
            destination.Month.Should().Be(source.Value.Month);
            destination.Year.Should().Be(source.Value.Year);
        }

        [Test]
        public void ShouldNotThrowExceptionIfSourceDateIsNull()
        {
            Assert.DoesNotThrow(() => mapper.Map<DateTime?, DateViewModel>(default(DateTime?)));
        }
    }
}
