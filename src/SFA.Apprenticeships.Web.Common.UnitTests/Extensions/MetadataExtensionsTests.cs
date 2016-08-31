namespace SFA.Apprenticeships.Web.Common.UnitTests.Extensions
{
    using System.ComponentModel.DataAnnotations;
    using Common.Extensions;
    using Common.ViewModels;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class MetadataExtensionsTests
    {
        [Test]
        public void GetLabelName()
        {
            var metadata = typeof(TestViewModel).GetMetadata<TestViewModel, DateViewModel>(m => m.Date);

            metadata.DisplayName.Should().Be("Display Name");
        }

        [Test]
        public void GetLabelName2()
        {
            var metadata = new TestViewModel().GetMetadata(m => m.Date);

            metadata.DisplayName.Should().Be("Display Name");
        }
    }

    public class TestViewModel
    {
        [Display(Name = "Display Name")]
        public DateViewModel Date { get; set; }
    }
}