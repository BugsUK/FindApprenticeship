namespace SFA.Apprenticeships.Infrastructure.UnitTests.Raa
{
    using System;
    using System.Linq;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using SFA.Apprenticeships.Domain.Entities.ReferenceData;
    using SFA.Apprenticeships.Domain.Raa.Interfaces.Repositories;
    using SFA.Apprenticeships.Infrastructure.Raa;

    public class ReferenceDataProviderTests
    {                

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            
        }

        [Test]
        public void GetCategories()
        {
            //Arrange            
            var referenceRepository = new Mock<IReferenceRepository>();
            var dataProvider = new ReferenceDataProvider(referenceRepository.Object);

            //Act
            var categories = dataProvider.GetCategories();

            //Assert
            categories.Should().NotBeNullOrEmpty();
            categories.Any(category => string.IsNullOrWhiteSpace(category.CodeName)                                
                                 || string.IsNullOrWhiteSpace(category.FullName)
                                 || Enum.IsDefined(typeof(CategoryType), category.CategoryType)
                                 || category.Count == 0
                                 || string.IsNullOrWhiteSpace(category.ParentCategoryCodeName)
                                 || !category.SubCategories.Any())                                 
                                 .Should().BeFalse();
        }       
    }
}