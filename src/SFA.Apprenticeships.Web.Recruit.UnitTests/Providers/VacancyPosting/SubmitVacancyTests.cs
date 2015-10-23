namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using System;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Moq;
    using NUnit.Framework;
    using ViewModels.Vacancy;

    [TestFixture]
    public class SubmitVacancyTests : TestBase
    {
        [Test]
        public void ShouldSetStateToForReviewWhenSumbittingTheVacancy()
        {
            var vacancyPostingProvider = GetProvider();
            var apprenticeshipVacancy = new ApprenticeshipVacancy
            {
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    Description = "Description",
                    Employer = new Employer
                    {
                        Address = new Address()
                    },
                    EntityId = Guid.NewGuid(),
                    ProviderSiteErn = string.Empty,
                    WebsiteUrl = "http://www.google.com"
                }
            };

            MockVacancyPostingService.Setup(ps => ps.GetVacancy(It.IsAny<long>())).Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(ps => ps.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(apprenticeshipVacancy);
            MockProviderService.Setup(ps => ps.GetProviderSite(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ProviderSite {Address = new Address()});
            MockReferenceDataService.Setup(ds => ds.GetSubCategoryByCode(It.IsAny<string>())).Returns(new Category());

            vacancyPostingProvider.SubmitVacancy(new VacancyViewModel());

            MockVacancyPostingService.Verify(
                ps =>
                    ps.SaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(v => v.Status == ProviderVacancyStatuses.ToReview)));
        }
    }
}