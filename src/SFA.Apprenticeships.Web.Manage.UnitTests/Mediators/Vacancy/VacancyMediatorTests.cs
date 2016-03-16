
namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Vacancy
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.UnitTests.Mediators;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using Manage.Mediators.Vacancy;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class VacancyMediatorTests
    {
        [Test]
        public void ShouldUpdateTheStatusOfTheVacancyToDraft()
        {
            //Arrange
            const int vacancyReferenceNumber = 1;
            var pendingQAVacancies = VacancyMediatorTestHelper.GetPendingVacancies(new[]
            {
                vacancyReferenceNumber
            });
            var provider = new Mock<IVacancyQAProvider>();
            provider.Setup(p => p.GetPendingQAVacancies()).Returns(pendingQAVacancies.ToList());

            var mediator = new VacancyMediatorBuilder().With(provider).Build();

            //Act
            mediator.RejectVacancy(vacancyReferenceNumber);

            //Assert
            provider.Verify(p => p.RejectVacancy(vacancyReferenceNumber), Times.Once);
        }
    }
}