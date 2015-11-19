namespace SFA.Apprenticeships.Web.Recruit.EndToEndTests
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Controllers;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Infrastructure.Repositories.Vacancies.Entities;
    using MongoDB.Driver;
    using NUnit.Framework;
    using FluentAssertions;

    [TestFixture]
    public class VacancyPostingControllerEndToEndTests : RecruitWebEndToEndTestsBase
    {
        [Test, Ignore]
        public void CloneAVacancyShouldCreateANewOneWithSomeFieldsReset()
        {
            const int vacancyReferenceNumber = 1;
            const string title = "Vacancy title";

            var vacancy = GetCorrectVacancy(vacancyReferenceNumber, title);

            InitializeDatabaseWithVacancy(vacancy);

            var vacancyPostingController = Container.GetInstance<VacancyPostingController>();
            var result = vacancyPostingController.CloneVacancy(vacancyReferenceNumber);
            result.Should().BeOfType<RedirectToRouteResult>();
            var redirection = result as RedirectToRouteResult;
            redirection.RouteName.Should().Be("ConfirmEmployer");

            var newVacancyGuid = (Guid) redirection.RouteValues.Values.Last();

            newVacancyGuid.Should().NotBe(vacancy.Id);
            var clonedVacancy = Collection.FindOneById(newVacancyGuid);
            clonedVacancy.Title.Should().StartWith("(Copy of)");
            clonedVacancy.DateCreated.Should().BeCloseTo(DateTime.UtcNow, 1000); // inject fake date time service?
            clonedVacancy.DateUpdated.Should().NotHaveValue();
            clonedVacancy.VacancyReferenceNumber.Should().NotBe(vacancyReferenceNumber);
            clonedVacancy.DateSubmitted.Should().NotHaveValue();
            clonedVacancy.DateStartedToQA.Should().NotHaveValue();
            clonedVacancy.DateQAApproved.Should().NotHaveValue();
            clonedVacancy.Status.Should().Be(ProviderVacancyStatuses.PendingQA);
            clonedVacancy.ClosingDate.Should().NotHaveValue();
            clonedVacancy.PossibleStartDate.Should().NotHaveValue();
            CheckAllCommentsAreNull(clonedVacancy);
        }

        private static void CheckAllCommentsAreNull(MongoApprenticeshipVacancy clonedVacancy)
        {
            clonedVacancy.WorkingWeekComment.Should().BeNull();
            clonedVacancy.ApprenticeshipLevelComment.Should().BeNull();
            clonedVacancy.ClosingDateComment.Should().BeNull();
            clonedVacancy.DesiredQualificationsComment.Should().BeNull();
            clonedVacancy.DesiredSkillsComment.Should().BeNull();
            clonedVacancy.DurationComment.Should().BeNull();
            clonedVacancy.FirstQuestionComment.Should().BeNull();
            clonedVacancy.FrameworkCodeNameComment.Should().BeNull();
            clonedVacancy.FutureProspectsComment.Should().BeNull();
            clonedVacancy.LongDescriptionComment.Should().BeNull();
        }

        private static MongoApprenticeshipVacancy GetCorrectVacancy(int vacancyReferenceNumber, string title)
        {
            return new MongoApprenticeshipVacancy
            {
                Title = title,
                ApprenticeshipLevel = ApprenticeshipLevel.Advanced,
                VacancyReferenceNumber = vacancyReferenceNumber,
                ClosingDate = DateTime.UtcNow.AddDays(30),
                DateCreated = DateTime.UtcNow.AddDays(-1),
                DateSubmitted = DateTime.UtcNow.AddDays(-1),
                DesiredQualifications = "desired qualifications",
                DesiredSkills = "desired skills",
                Duration = 3,
                DurationType = DurationType.Years,
                EntityId = Guid.NewGuid(),
                FutureProspects = "future prospects",
                HoursPerWeek = 40,
                LongDescription = "long description",
                OfflineVacancy = false,
                PersonalQualities = "personal qualities",
                PossibleStartDate = DateTime.UtcNow.AddDays(100),
                ProviderSiteEmployerLink = new ProviderSiteEmployerLink
                {
                    DateCreated = DateTime.UtcNow,
                    Description = "employer link",
                    DateUpdated = DateTime.UtcNow,
                    ProviderSiteErn = "101282923",
                    WebsiteUrl = "www.google.com",
                    Employer = new Employer
                    {
                        Ern = "100608868",
                        Name = "Employer name",
                        Address = new Address
                        {
                            AddressLine1 = "address line 1",
                            AddressLine2 = "address line 2",
                            AddressLine3 = "address line 3",
                            AddressLine4 = "address line 4",
                            Postcode = "postcode",
                            Uprn = null,
                            GeoPoint = new GeoPoint
                            {
                                Latitude = 0.0,
                                Longitude = 0.0
                            }
                        }
                    }
                },
                ShortDescription = "short description",
                Status = ProviderVacancyStatuses.PendingQA,
                TrainingType = TrainingType.Standards,
                StandardId = 1,
                WorkingWeek = "Working week",
                WageType = WageType.ApprenticeshipMinimumWage,
                Ukprn = "10003816"
            };
        }

        private void InitializeDatabaseWithVacancy(MongoApprenticeshipVacancy vacancy)
        {
            var mongoConnectionString = MongoConfiguration.VacancyDb;
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            var database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);

            Collection = database.GetCollection<MongoApprenticeshipVacancy>("apprenticeshipVacancies");

            Collection.Save(vacancy);
        }
    }
}