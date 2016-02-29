namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Providers.VacancyPosting
{
    using System;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.ReferenceData;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SubmitVacancyTests : TestBase
    {
        [Test]
        public void ShouldSetStateToPendingQAWhenSumbittingTheVacancy()
        {
            var vacancyPostingProvider = GetVacancyPostingProvider();
            const int referenceNumber = 1;

            var apprenticeshipVacancy = new Vacancy
            {
                OwnerPartyId = 42,
                IsEmployerLocationMainApprenticeshipLocation = true
            };

            MockVacancyPostingService.Setup(ps => ps.GetVacancyByReferenceNumber(It.IsAny<int>())).Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(ps => ps.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockProviderService.Setup(ps => ps.GetProviderSite(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ProviderSite {Address = new PostalAddress()});
            MockReferenceDataService.Setup(ds => ds.GetSubCategoryByCode(It.IsAny<string>())).Returns(new Category());

            vacancyPostingProvider.SubmitVacancy(referenceNumber);

            MockVacancyPostingService.Verify(
                ps =>
                    ps.UpdateVacancy(
                        It.Is<Vacancy>(v => v.Status == VacancyStatus.PendingQA)));
        }

        [Test]
        public void ShouldSetDateSubmittedToUtcNowWhenSumbittingTheVacancy()
        {
            var vacancyPostingProvider = GetVacancyPostingProvider();
            const int referenceNumber = 1;

            var now = DateTime.Now;

            var apprenticeshipVacancy = new Vacancy
            {
                OwnerPartyId = 42,
                IsEmployerLocationMainApprenticeshipLocation = true
            };

            MockVacancyPostingService.Setup(ps => ps.GetVacancyByReferenceNumber(It.IsAny<int>())).Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(ps => ps.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockProviderService.Setup(ps => ps.GetProviderSite(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ProviderSite { Address = new PostalAddress() });
            MockReferenceDataService.Setup(ds => ds.GetSubCategoryByCode(It.IsAny<string>())).Returns(new Category());
            MockTimeService.Setup(ts => ts.UtcNow()).Returns(now);

            vacancyPostingProvider.SubmitVacancy(referenceNumber);

            MockVacancyPostingService.Verify(
                ps =>
                    ps.UpdateVacancy(
                        It.Is<Vacancy>(v => v.DateSubmitted == now)));
        }

        [Test]
        public void ShouldIncrementSubmissionCountWhenSumbittingTheVacancy()
        {
            var vacancyPostingProvider = GetVacancyPostingProvider();
            const int referenceNumber = 1;

            var apprenticeshipVacancy = new Vacancy
            {
                OwnerPartyId = 42,
                IsEmployerLocationMainApprenticeshipLocation = true,
                SubmissionCount = 2
            };

            MockVacancyPostingService.Setup(ps => ps.GetVacancyByReferenceNumber(It.IsAny<int>())).Returns(apprenticeshipVacancy);
            MockVacancyPostingService.Setup(ps => ps.UpdateVacancy(It.IsAny<Vacancy>()))
                .Returns(apprenticeshipVacancy);
            MockProviderService.Setup(ps => ps.GetProviderSite(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ProviderSite { Address = new PostalAddress() });
            MockReferenceDataService.Setup(ds => ds.GetSubCategoryByCode(It.IsAny<string>())).Returns(new Category());

            vacancyPostingProvider.SubmitVacancy(referenceNumber);

            MockVacancyPostingService.Verify(
                ps =>
                    ps.UpdateVacancy(
                        It.Is<Vacancy>(v => v.SubmissionCount == 3)));
        }
    }
}