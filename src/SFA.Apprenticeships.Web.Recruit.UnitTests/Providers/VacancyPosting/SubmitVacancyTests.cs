namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Providers.VacancyPosting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
                    ps.SaveApprenticeshipVacancy(
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
                    ps.SaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(v => v.DateSubmitted == now)));
        }

        [Test]
        public void AVacancyWithMultipleLocationsShouldCreateMultipleVacanciesOnSubmitting()
        {
            var vacancyPostingProvider = GetVacancyPostingProvider();
            const long referenceNumber = 1;

            var now = DateTime.Now;

            var apprenticeshipVacancy = new ApprenticeshipVacancy
            {
                VacancyReferenceNumber = 1,
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
                IsEmployerLocationMainApprenticeshipLocation = false,
                LocationAddresses = new List<VacancyLocationAddress> { 
                    new VacancyLocationAddress
                    {
                        AddressLine1 = "address line 1 1",
                        AddressLine2 = "address line 2 1",
                        AddressLine3 = "address line 3 1",
                        AddressLine4 = "address line 4 1",
                        Postcode = "Postcode 1"
                    },
                    new VacancyLocationAddress
                    {
                        AddressLine1 = "address line 1 2",
                        AddressLine2 = "address line 2 2",
                        AddressLine3 = "address line 3 2",
                        AddressLine4 = "address line 4 2",
                        Postcode = "Postcode 2"
                    }
                }
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
                    ps.SaveApprenticeshipVacancy(
                        It.IsAny<ApprenticeshipVacancy>()), Times.Exactly(2));

            MockVacancyPostingService.Verify(
                ps =>
                    ps.SaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>( v => v.LocationAddresses.First().AddressLine1 == "address line 1 1" &&
                                                            v.LocationAddresses.First().AddressLine2 == "address line 2 1" &&
                                                            v.LocationAddresses.First().AddressLine3 == "address line 3 1" &&
                                                            v.LocationAddresses.First().AddressLine4 == "address line 4 1" &&
                                                            v.LocationAddresses.First().Postcode == "Postcode 1")));

            MockVacancyPostingService.Verify(
                ps =>
                    ps.SaveApprenticeshipVacancy(
                        It.Is<ApprenticeshipVacancy>(v => v.LocationAddresses.First().AddressLine1 == "address line 1 2" &&
                                                           v.LocationAddresses.First().AddressLine2 == "address line 2 2" &&
                                                           v.LocationAddresses.First().AddressLine3 == "address line 3 2" &&
                                                           v.LocationAddresses.First().AddressLine4 == "address line 4 2" &&
                                                           v.LocationAddresses.First().Postcode == "Postcode 2")));

        }
    }
}