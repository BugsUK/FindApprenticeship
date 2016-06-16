namespace SFA.Apprenticeships.Infrastructure.UnitTests.Raa.Mappers
{
    using System;
    using System.Linq;
    using FluentAssertions;
    using Infrastructure.Presentation;
    using Infrastructure.Raa.Extensions;
    using Infrastructure.Raa.Mappers;
    using Moq;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class ApprenticeshipVacancyDetailMapperTests
    {
        private Mock<ILogService> _mockLogService;

        [SetUp]
        public void SetUp()
        {
            _mockLogService = new Mock<ILogService>();
        }

        [TestCase(10, 5)]
        public void ShouldGetApprenticeshipDetail(int vacancyCount, int categoryCount)
        {
            for (var i = 0; i < vacancyCount; i++)
            {
                // Arrange.
                var fixture = new Fixture();

                var vacancy = fixture
                    .Build<Domain.Entities.Raa.Vacancies.Vacancy>()
                    .With(each => each.WageType, Domain.Entities.Raa.Vacancies.WageType.NationalMinimum)
                    .Create();

                var vacancyParty = fixture.Create<Domain.Entities.Raa.Parties.VacancyParty>();
                var employer = fixture.Create<Domain.Entities.Raa.Parties.Employer>();
                var provider = fixture.Create<Domain.Entities.Raa.Parties.Provider>();
                var providerSite = fixture.Create<Domain.Entities.Raa.Parties.ProviderSite>();

                var categories = fixture
                    .Build<Domain.Entities.ReferenceData.Category>()
                    .CreateMany(categoryCount)
                    .ToList();

                // Act.
                var detail = ApprenticeshipVacancyDetailMapper.GetApprenticeshipVacancyDetail(
                    vacancy, vacancyParty, employer, provider, providerSite, categories, _mockLogService.Object);

                // Assert.
                detail.Should().NotBeNull();

                detail.Id.Should().Be(vacancy.VacancyId);
                detail.ApprenticeshipLevel.Should().Be(vacancy.ApprenticeshipLevel.GetApprenticeshipLevel());

                detail.VacancyLocationType.Should()
                    .Be(vacancy.VacancyLocationType == Domain.Entities.Raa.Vacancies.VacancyLocationType.Nationwide
                        ? Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLocationType.National
                        : Domain.Entities.Vacancies.Apprenticeships.ApprenticeshipLocationType.NonNational);

                detail.VacancyReference.Should().Be(vacancy.VacancyReferenceNumber.GetVacancyReference());
                detail.Title.Should().Be(vacancy.Title);
                detail.Description.Should().Be(vacancy.ShortDescription);
                detail.FullDescription.Should().Be(vacancy.LongDescription);

                // NOTE: hard to unit test.
                detail.SubCategory.Should().NotBeNull();

                detail.StartDate.Should().Be(vacancy.PossibleStartDate ?? DateTime.MinValue);
                detail.ClosingDate.Should().Be(vacancy.ClosingDate ?? DateTime.MinValue);
                detail.PostedDate.Should().Be(vacancy.DateQAApproved ?? DateTime.MinValue);
                detail.InterviewFromDate.Should().Be(DateTime.MinValue);

                // NOTE: hard to unit test.
                detail.Wage.Should().Be(vacancy.Wage ?? 0);
                detail.WageUnit.Should().Be(Domain.Entities.Vacancies.WageUnit.Weekly);
                detail.WageDescription.Should().NotBeNull();
                detail.WageType.Should().Be((Domain.Entities.Vacancies.LegacyWageType)vacancy.WageType);

                detail.WorkingWeek.Should().Be(vacancy.WorkingWeek);

                detail.OtherInformation.Should().Be(vacancy.OtherInformation);
                detail.FutureProspects.Should().Be(vacancy.FutureProspects);
                detail.VacancyOwner.Should().BeNull();
                detail.VacancyManager.Should().BeNull();

                detail.LocalAuthority.Should().BeNull();
                detail.NumberOfPositions.Should().Be(vacancy.NumberOfPositions ?? 0);
                detail.RealityCheck.Should().Be(vacancy.ThingsToConsider);
                detail.Created.Should().Be(vacancy.CreatedDateTime);
                detail.VacancyStatus.Should().Be(vacancy.Status.GetVacancyStatuses());
                detail.TrainingType.Should().Be(vacancy.TrainingType.GetTrainingType());
                detail.EmployerName.Should().Be(employer.Name);
                detail.AnonymousEmployerName.Should().Be(vacancy.EmployerAnonymousName);
                detail.EmployerDescription.Should().Be(vacancyParty.EmployerDescription);
                detail.EmployerWebsite.Should().Be(vacancy.EmployerWebsiteUrl);
                detail.ApplyViaEmployerWebsite.Should().Be(vacancy.OfflineVacancy ?? false);
                detail.VacancyUrl.Should().Be(vacancy.OfflineApplicationUrl);
                detail.ApplicationInstructions.Should().Be(vacancy.OfflineApplicationInstructions);
                detail.IsEmployerAnonymous.Should().Be(!string.IsNullOrWhiteSpace(vacancy.EmployerAnonymousName));
                detail.IsPositiveAboutDisability.Should().Be(employer.IsPositiveAboutDisability);

                detail.ExpectedDuration.Should().Be(vacancy.ExpectedDuration);

                detail.VacancyAddress.Should().NotBeNull();

                detail.VacancyAddress.AddressLine1.Should().Be(vacancy.Address.AddressLine1);
                detail.VacancyAddress.AddressLine2.Should().Be(vacancy.Address.AddressLine2);
                detail.VacancyAddress.AddressLine3.Should().Be(vacancy.Address.AddressLine3);
                detail.VacancyAddress.AddressLine4.Should().Be(vacancy.Address.AddressLine4);
                detail.VacancyAddress.Postcode.Should().Be(vacancy.Address.Postcode);

                // NOTE: hard to unit test.
                detail.VacancyAddress.GeoPoint.Should().NotBeNull();
                detail.VacancyAddress.GeoPoint.Latitude.Should().NotBe(0.0);
                detail.VacancyAddress.GeoPoint.Longitude.Should().NotBe(0.0);

                detail.IsRecruitmentAgencyAnonymous.Should().Be(false);
                detail.IsSmallEmployerWageIncentive.Should().Be(false);

                detail.SupplementaryQuestion1.Should().Be(vacancy.FirstQuestion);
                detail.SupplementaryQuestion2.Should().Be(vacancy.SecondQuestion);

                detail.RecruitmentAgency.Should().Be(providerSite.TradingName);
                detail.ProviderName.Should().Be(provider.Name);
                detail.TradingName.Should().Be(employer.TradingName);
                detail.ProviderDescription.Should().BeNull();

                // NOTE: hard to unit test.
                detail.Contact.Should().NotBeNull();

                detail.ProviderSectorPassRate.Should().Be(default(int?));
                detail.TrainingToBeProvided.Should().Be(vacancy.TrainingProvided);
                detail.ContractOwner.Should().BeNull();
                detail.DeliveryOrganisation.Should().BeNull();
                detail.IsNasProvider.Should().Be(false);
                detail.PersonalQualities.Should().Be(vacancy.PersonalQualities);
                detail.QualificationRequired.Should().Be(vacancy.DesiredQualifications);
                detail.SkillsRequired.Should().Be(vacancy.DesiredSkills);
            }
        }

        [TestCase("Anonymous Employer Name", true)]
        [TestCase(null, false)]
        [TestCase("", false)]
        public void ShouldHandleEmployerNameAnonymisation(
            string anonymousEmployerName, bool anonymised)
        {
            // Arrange.
            var fixture = new Fixture();

            var vacancy = fixture
                .Build<Domain.Entities.Raa.Vacancies.Vacancy>()
                .With(each => each.EmployerAnonymousName, anonymousEmployerName)
                .Create();

            var employer = fixture.Create<Domain.Entities.Raa.Parties.Employer>();
            var vacancyParty = fixture.Create<Domain.Entities.Raa.Parties.VacancyParty>();
            var provider = fixture.Create<Domain.Entities.Raa.Parties.Provider>();
            var providerSite = fixture.Create<Domain.Entities.Raa.Parties.ProviderSite>();

            var categories = fixture
                .Build<Domain.Entities.ReferenceData.Category>()
                .CreateMany(1)
                .ToList();

            // Act.
            var detail = ApprenticeshipVacancyDetailMapper.GetApprenticeshipVacancyDetail(
                vacancy, vacancyParty, employer, provider, providerSite, categories, _mockLogService.Object);

            // Assert.
            detail.Should().NotBeNull();
            detail.AnonymousEmployerName.Should().Be(anonymousEmployerName);
            detail.IsEmployerAnonymous.Should().Be(anonymised);
        }
    }
}
