namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
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

    [TestFixture]
    public class SubmitVacancyTests : TestBase
    {
        [Test]
        public void ShouldSetStateToPendingQAWhenSumbittingTheVacancy()
        {
            var vacancyPostingProvider = GetVacancyPostingProvider();
            const long referenceNumber = 1;

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
                },
                IsEmployerLocationMainApprenticeshipLocation = true
            };

            MockVacancyPostingService.Setup(ps => ps.GetVacancy(It.IsAny<long>())).Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(ps => ps.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(apprenticeshipVacancy);
            MockProviderService.Setup(ps => ps.GetProviderSite(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ProviderSite {Address = new Address()});
            MockReferenceDataService.Setup(ds => ds.GetSubCategoryByCode(It.IsAny<string>())).Returns(new Category());

            vacancyPostingProvider.SubmitVacancy(referenceNumber);

            MockVacancyPostingService.Verify(
                ps =>
                    ps.ShallowSaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(v => v.Status == ProviderVacancyStatuses.PendingQA)));
        }

        [Test]
        public void ShouldSetDateSubmittedToUtcNowWhenSumbittingTheVacancy()
        {
            var vacancyPostingProvider = GetVacancyPostingProvider();
            const long referenceNumber = 1;

            var now = DateTime.Now;

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
                },
                IsEmployerLocationMainApprenticeshipLocation = true
            };

            MockVacancyPostingService.Setup(ps => ps.GetVacancy(It.IsAny<long>())).Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(ps => ps.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(apprenticeshipVacancy);
            MockProviderService.Setup(ps => ps.GetProviderSite(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ProviderSite { Address = new Address() });
            MockReferenceDataService.Setup(ds => ds.GetSubCategoryByCode(It.IsAny<string>())).Returns(new Category());
            MockTimeService.Setup(ts => ts.UtcNow()).Returns(now);

            vacancyPostingProvider.SubmitVacancy(referenceNumber);

            MockVacancyPostingService.Verify(
                ps =>
                    ps.ShallowSaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(v => v.DateSubmitted == now)));
        }

        [Test]
        public void ShouldIncrementSubmissionCountWhenSumbittingTheVacancy()
        {
            var vacancyPostingProvider = GetVacancyPostingProvider();
            const long referenceNumber = 1;

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
                    WebsiteUrl = "http://www.google.com",
                },
                IsEmployerLocationMainApprenticeshipLocation = true,
                SubmissionCount = 2
            };

            MockVacancyPostingService.Setup(ps => ps.GetVacancy(It.IsAny<long>())).Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(ps => ps.SaveApprenticeshipVacancy(It.IsAny<ApprenticeshipVacancy>()))
                .Returns(apprenticeshipVacancy);
            MockProviderService.Setup(ps => ps.GetProviderSite(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ProviderSite { Address = new Address() });
            MockReferenceDataService.Setup(ds => ds.GetSubCategoryByCode(It.IsAny<string>())).Returns(new Category());

            vacancyPostingProvider.SubmitVacancy(referenceNumber);

            MockVacancyPostingService.Verify(
                ps =>
                    ps.ShallowSaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(v => v.SubmissionCount == 3)));
        }
    }
}