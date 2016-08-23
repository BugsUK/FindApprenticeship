namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyProvider
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Application.Interfaces;
    using Application.Interfaces.Applications;
    using Application.Interfaces.Employers;
    using Application.Interfaces.Providers;
    using Application.Interfaces.VacancyPosting;
    using Domain.Entities.Applications;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.ProviderUser;
    using ViewModels.Vacancy;

    [TestFixture]
    public class DashboardTests
    {
        [Test]
        public void GetVacanciesSummaryForProviderShouldCallEmployerServiceGetMinimalEmployerDetails()
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

            var employerService = new Mock<IEmployerService>();
            var providerService = new Mock<IProviderService>();
            var vacancyPostingService = new Mock<IVacancyPostingService>();
            var apprenticeshipApplicationService = new Mock<IApprenticeshipApplicationService>();
            var mapper = new Mock<IMapper>();

            providerService.Setup(s => s.GetVacancyParties(providerSiteId)).Returns(new List<VacancyParty>
            {
                new VacancyParty
                {
                    VacancyPartyId = employerId,
                    EmployerId = employerId,
                    ProviderSiteId = providerSiteId
                }
            });
            vacancyPostingService.Setup(s => s.GetMinimalVacancyDetails(It.IsAny<IEnumerable<int>>()))
                .Returns(new ReadOnlyDictionary<int, IEnumerable<IMinimalVacancyDetails>>(
                    new Dictionary<int, IEnumerable<IMinimalVacancyDetails>> {
                        {1,
                        new List<IMinimalVacancyDetails>
                        {
                            new VacancySummary
                            {
                                OwnerPartyId = employerId
                            }
                        }
                    }}));

            vacancyPostingService.Setup(s => s.GetVacancySummariesByIds(It.IsAny<IEnumerable<int>>())).Returns(new List<VacancySummary>
            {
                new VacancySummary
                {
                    OwnerPartyId = employerId
                }
            });

            employerService.Setup(s => s.GetMinimalEmployerDetails(It.IsAny<IEnumerable<int>>(), It.IsAny<bool>())).Returns(new List<MinimalEmployerDetails>
            {
                new MinimalEmployerDetails
                {
                    EmployerId = employerId
                }
            });

            apprenticeshipApplicationService.Setup(s => s.GetCountsForVacancyIds(It.IsAny<IEnumerable<int>>())).Returns(
                new ReadOnlyDictionary<int, IApplicationCounts>(
                    new Dictionary<int, IApplicationCounts>
                    {
                        {0, new ZeroApplicationCounts()}
                    }
                    ));


            mapper.Setup(m => m.Map<VacancySummary, VacancySummaryViewModel>(It.IsAny<VacancySummary>()))
                .Returns(new VacancySummaryViewModel
                {
                    OwnerPartyId = employerId,
                    VacancyId = 0
                });

            vacancyPostingService.Setup(s => s.GetVacancyLocationsByVacancyIds(It.IsAny<IEnumerable<int>>())).Returns(
                new ReadOnlyDictionary<int, IEnumerable<VacancyLocation>>(
                    new Dictionary<int, IEnumerable<VacancyLocation>> {
                        {1,
                        new List<VacancyLocation>
                        {
                            new VacancyLocation()
                        }
                    }})
                );

            var provider = new VacancyProviderBuilder()
                .With(employerService)
                .With(providerService)
                .With(vacancyPostingService)
                .With(mapper)
                .With(apprenticeshipApplicationService)
                .BuildVacancyPostingProvider();

            provider.GetVacanciesSummaryForProvider(providerId, providerSiteId, search);

            employerService.Verify(s => s.GetMinimalEmployerDetails(It.IsAny<IEnumerable<int>>(), It.IsAny<bool>()), Times.Once);
        }
        
    }
}