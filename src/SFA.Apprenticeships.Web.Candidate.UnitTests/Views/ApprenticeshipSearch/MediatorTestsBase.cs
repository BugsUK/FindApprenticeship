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
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

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
            Mediator = new ApprenticeshipSearchMediator(ConfigurationService.Object, SearchProvider.Object, CandidateServiceProvider.Object, UserDataProvider.Object, ReferenceDataService.Object, new ApprenticeshipSearchViewModelServerValidator(), new ApprenticeshipSearchViewModelLocationValidator(), ApprenticeshipVacancyProvider.Object);
        }

        private static IEnumerable<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category
                {
                    CodeName = "1",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "1_1"},
                        new Category {CodeName = "1_2"}
                    }
                },
                new Category
                {
                    CodeName = "2",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "2_1"},
                        new Category {CodeName = "2_2"},
                        new Category {CodeName = "2_3"}
                    }
                },
                new Category
                {
                    CodeName = "3",
                    SubCategories = new List<Category>
                    {
                        new Category {CodeName = "3_1"}
                    }
                }
            };
        }
    }
}