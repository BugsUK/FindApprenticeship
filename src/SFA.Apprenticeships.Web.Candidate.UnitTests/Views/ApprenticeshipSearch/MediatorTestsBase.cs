﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.ApprenticeshipSearch
{
    using System.Collections.Generic;
    using Application.Interfaces.ReferenceData;
    using Candidate.Mediators.Search;
    using Candidate.Providers;
    using Candidate.Validators;
    using Common.Providers;
    using Domain.Entities.ReferenceData;
    using Domain.Interfaces.Configuration;
    using Moq;
    using NUnit.Framework;

    public abstract class MediatorTestsBase : ViewUnitTest
    {
        protected Mock<IApprenticeshipVacancyDetailProvider> ApprenticeshipVacancyDetailProvider;
        protected Mock<ICandidateServiceProvider> CandidateServiceProvider;
        protected Mock<IConfigurationManager> ConfigurationManager;
        protected Mock<IUserDataProvider> UserDataProvider;
        protected Mock<ISearchProvider> SearchProvider;
        protected Mock<IReferenceDataService> ReferenceDataService;
        protected IApprenticeshipSearchMediator Mediator;

        [SetUp]
        public virtual void SetUpMethod()
        {
            //Use the mediator so that we get an accurate view model for testing
            ApprenticeshipVacancyDetailProvider = new Mock<IApprenticeshipVacancyDetailProvider>();
            CandidateServiceProvider = new Mock<ICandidateServiceProvider>();
            ConfigurationManager = new Mock<IConfigurationManager>();
            ConfigurationManager.Setup(cm => cm.GetAppSetting<int>("VacancyResultsPerPage")).Returns(5);
            ConfigurationManager.Setup(cm => cm.GetAppSetting("BlacklistedCategoryCodes")).Returns("00,99");
            UserDataProvider = new Mock<IUserDataProvider>();
            SearchProvider = new Mock<ISearchProvider>();
            ReferenceDataService = new Mock<IReferenceDataService>();
            ReferenceDataService.Setup(rds => rds.GetCategories()).Returns(GetCategories);
            Mediator = new ApprenticeshipSearchMediator(ConfigurationManager.Object, SearchProvider.Object, ApprenticeshipVacancyDetailProvider.Object, CandidateServiceProvider.Object, UserDataProvider.Object, ReferenceDataService.Object, new ApprenticeshipSearchViewModelServerValidator(), new ApprenticeshipSearchViewModelLocationValidator());
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