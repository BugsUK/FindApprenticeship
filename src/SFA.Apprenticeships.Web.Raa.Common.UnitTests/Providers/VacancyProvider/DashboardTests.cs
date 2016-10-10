namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Application.Interfaces;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.VacancyPosting;
    using Application.Vacancy;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Repositories.Models;
    using Moq;
    using NUnit.Framework;
    using ViewModels.ProviderUser;
    using ViewModels.Vacancy;

    [TestFixture]
    public class DashboardTests
    {
        [Test]
        public void GetVacanciesSummaryForProviderShouldCallVacancySummaryService()
        {
            const int providerId = 1;
            const int providerSiteId = 2;
            const int employerId = 3;
            const VacancyType vacancyType = VacancyType.Apprenticeship;
            
            var search = new VacanciesSummarySearchViewModel
            {
                PageSize = 5,
                VacancyType = vacancyType
            };
            
            var vacancySummaryService = new Mock<IVacancySummaryService>();
            var mapper = new Mock<IMapper>();

            mapper.Setup(m => m.Map<VacancySummary, VacancySummaryViewModel>(It.IsAny<VacancySummary>()))
                .Returns(new VacancySummaryViewModel
                {
                    VacancyOwnerRelationshipId = employerId,
                    VacancyId = 0
                });
            

            int tmp;
            vacancySummaryService.Setup(s => s.GetSummariesForProvider(It.IsAny<VacancySummaryQuery>(), out tmp))
                .Returns(
                    new List<VacancySummary>()
                    {
                        new VacancySummary()
                        {
                            VacancyType = VacancyType.Apprenticeship,
                            Title = "Test",
                            Address = new PostalAddress() {Town = "Test"}
                        }
                    });

            vacancySummaryService.Setup(s => s.GetLotteryCounts(It.IsAny<VacancySummaryQuery>())).Returns(
                new VacancyCounts()
                {
                    RejectedCount = 1
                });

            var provider = new VacancyProviderBuilder()
                .With(mapper)
                .With(vacancySummaryService)
                .BuildVacancyPostingProvider();

            provider.GetVacanciesSummaryForProvider(providerId, providerSiteId, search);

            vacancySummaryService.Verify(s => s.GetSummariesForProvider(It.IsAny<VacancySummaryQuery>(), out tmp), Times.Once);
        }
    }
}