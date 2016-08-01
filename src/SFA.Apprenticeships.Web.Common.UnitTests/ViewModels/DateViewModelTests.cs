namespace SFA.Apprenticeships.Web.Common.UnitTests.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Common.ViewModels;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class DateViewModelTests
    {
        [Test]
        public void GetLabelName()
        {
            var metadata = ModelMetadata.FromLambdaExpression(
                x => x.Date,
                new ViewDataDictionary<TestViewModel>()
            );

            metadata.DisplayName.Should().Be("Display Name");
        }
    }

    public class TestViewModel
    {
        [Display(Name = "Display Name")]
        public DateViewModel Date { get; set; }
    }
}