namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System.Collections.Generic;
    using Application.Interfaces.ReferenceData;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Configuration;
    using Common.Providers;
    using Domain.Entities.ReferenceData;
    using SFA.Infrastructure.Interfaces;
    using Moq;
    using NUnit.Framework;

    using SFA.Apprenticeships.Application.Interfaces;

    public abstract class MediatorTestsBase : ViewUnitTest
    {
        protected Mock<ICandidateServiceProvider> CandidateServiceProvider;
        protected Mock<IConfigurationService> ConfigurationService;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected Mock<ISearchProvider> SearchProvider;
        protected Mock<IReferenceDataService> ReferenceDataService;
        protected Mock<IApprenticeshipVacancyProvider> ApprenticeshipVacancyProvider;
        protected IApprenticeshipSearchMediator Mediator;

        [SetUp]
        public virtual void SetUpMethod()
        {
            //Use the mediator so that we get an accurate view model for testing
            CandidateServiceProvider = new Mock<ICandidateServiceProvider>();
            ConfigurationService = new Mock<IConfigurationService>();
            ConfigurationService.Setup(cm => cm.Get<CommonWebConfiguration>())
                .Returns(new CommonWebConfiguration() { VacancyResultsPerPage = 5, BlacklistedCategoryCodes = "00,99" });
            UserDataProvider = new Mock<IUserDataProvider>();
            SearchProvider = new Mock<ISearchProvider>();
            ReferenceDataService = new Mock<IReferenceDataService>();
            ReferenceDataService.Setup(rds => rds.GetCategories()).Returns(GetCategories);
            ApprenticeshipVacancyProvider = new Mock<IApprenticeshipVacancyProvider>();
            Mediator = new ApprenticeshipSearchMediator(ConfigurationService.Object, SearchProvider.Object, CandidateServiceProvider.Object, UserDataProvider.Object, ReferenceDataService.Object, new ApprenticeshipSearchViewModelServerValidator(), new ApprenticeshipSearchViewModelLocationValidator(), ApprenticeshipVacancyProvider.Object, new Mock<IGoogleMapsProvider>().Object);
        }

        private static IEnumerable<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category(1, "1", "1", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active, new List<Category>
                    {
                        new Category(1, "1_1", "1_1", CategoryType.Framework, CategoryStatus.Active),
                        new Category(2, "1_2", "1_2", CategoryType.Framework, CategoryStatus.Active)
                    }
                ),
                new Category(2, "2", "2", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active, new List<Category>
                    {
                        new Category(1, "2_1", "2_1", CategoryType.Framework, CategoryStatus.Active),
                        new Category(2, "2_2", "2_2", CategoryType.Framework, CategoryStatus.Active),
                        new Category(3, "2_3", "2_3", CategoryType.Framework, CategoryStatus.Active)
                    }
                ),
                new Category(3, "3", "3", CategoryType.SectorSubjectAreaTier1, CategoryStatus.Active, new List<Category>
                    {
                        new Category(1, "3_1", "3_1", CategoryType.Framework, CategoryStatus.Active)
                    }
                )
            };
        }
    }
}