namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Tests.Applications
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using Domain.Interfaces.Repositories;
    using FluentAssertions;
    using Infrastructure.Repositories.Mongo.Applications.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using Moq;
    using NUnit.Framework;
    using SFA.Infrastructure.Interfaces;

    [TestFixture]
    public class ApprenticeshipApplicationWriteRepositoryTests : RepositoryIntegrationTest
    {
        private const int LegacyApplicationId = 12345;

        [SetUp]
        public void SetUp()
        {
            var mongoConnectionString = MongoConfiguration.ApplicationsDb;
            var mongoDbName = MongoUrl.Create(mongoConnectionString).DatabaseName;

            var database = new MongoClient(mongoConnectionString)
                .GetServer()
                .GetDatabase(mongoDbName);
            
            var collection = database.GetCollection<MongoApprenticeshipApplicationDetail>("apprenticeships");
            collection.Remove(Query.EQ("LegacyApplicationId", LegacyApplicationId));
        }

        [Test, Category("Integration")]
        public void ShouldCreateAndDeleteApplication()
        {
            // arrange
            var writer = Container.GetInstance<IApprenticeshipApplicationWriteRepository>();
            var reader = Container.GetInstance<IApprenticeshipApplicationReadRepository>();
            
            
            var application = CreateTestApplication();

            // act
            writer.Save(application);

            //assert
            var savedApplication = reader.Get(application.LegacyApplicationId, true);
            savedApplication.Should().NotBeNull();
            savedApplication.EntityId.Should().Be(application.EntityId);
            savedApplication.CandidateDetails.FirstName.Should().Be(application.CandidateDetails.FirstName);
            savedApplication.CandidateDetails.Address.AddressLine1.Should().Be(application.CandidateDetails.Address.AddressLine1);

            writer.Delete(savedApplication.EntityId);
            savedApplication = reader.Get(application.LegacyApplicationId, true);
            savedApplication.Should().BeNull();
        }

        //should update if not owned by RAA and not ignoring ownership
        //should update if ignoring ownership
        //should not update if owned by RAA and not ignoring ownership
        [TestCase(false, false ,true), Category("Integration")]
        [TestCase(false, true, true), Category("Integration")]
        [TestCase(true, true, true), Category("Integration")]
        [TestCase(true, false, false), Category("Integration")]
        public void TestUpdateApplicationStatus(bool ownedByRaa, bool ignoreOwnership, bool shouldUpdate)
        {
            // arrange
            Mock<IDateTimeService> dateTimeService = new Mock<IDateTimeService>();
            Container.Configure(c => c.For<IDateTimeService>().Use(dateTimeService.Object));
            var newDateTimeValue = DateTime.UtcNow;
            dateTimeService.Setup(m => m.UtcNow).Returns(newDateTimeValue);

            var newApplicationStatus = ApplicationStatuses.Submitted;
            var originalApplicationStatus = ApplicationStatuses.Successful;
            var originalDateTimeValue = DateTime.UtcNow.AddDays(-1);

            var writer = Container.GetInstance<IApprenticeshipApplicationWriteRepository>();
            var reader = Container.GetInstance<IApprenticeshipApplicationReadRepository>();

            var originalApplication = CreateTestApplication(originalDateTimeValue, originalApplicationStatus, ownedByRaa);
            writer.Save(originalApplication);

            var updatedApplication = originalApplication;
            updatedApplication.Status = newApplicationStatus;

           // act
            writer.UpdateApplicationStatus(updatedApplication, ignoreOwnership);

            //assert
            var savedApplication = reader.Get(originalApplication.LegacyApplicationId, true);
            if (shouldUpdate)
            {
                savedApplication.Status.Should().Be(newApplicationStatus);
                savedApplication.DateUpdated.Should().BeExactly(new TimeSpan(newDateTimeValue.Ticks));
            }
            else
            {
                savedApplication.Status.Should().Be(originalApplicationStatus);
                savedApplication.DateUpdated.Should().BeExactly(new TimeSpan(originalDateTimeValue.Ticks));
            }
            
        }

        #region Helpers
        private static ApprenticeshipApplicationDetail CreateTestApplication(DateTime dateUpdated = new DateTime(), ApplicationStatuses status = ApplicationStatuses.Unknown, bool ownedByRaa = true)
        {
            return new ApprenticeshipApplicationDetail
            {
                EntityId = Guid.NewGuid(),
                CandidateId = Guid.NewGuid(),
                LegacyApplicationId = LegacyApplicationId,
                VacancyStatus = VacancyStatuses.Live,
                Status = status,
                DateLastViewed = ownedByRaa ? DateTime.Today : new DateTime?(),
                DateUpdated = dateUpdated,
                DateCreated = DateTime.Now.AddDays(-3),
                CandidateDetails =
                {
                    FirstName = "Johnny",
                    LastName = "Candidate",
                    DateOfBirth = DateTime.UtcNow.AddYears(-30),
                    EmailAddress = "email@server.com",
                    PhoneNumber = "07777111222",
                    Address =
                    {
                        AddressLine1 = "Address line 1",
                        AddressLine2 = "Address line 2",
                        AddressLine3 = "Address line 3",
                        AddressLine4 = "Address line 4",
                        Postcode = "CV1 2WT"
                    }
                },
                CandidateInformation =
                {
                    AboutYou =
                    {
                        HobbiesAndInterests = "Hobbies and interests",
                        Improvements = "Improvements are not needed",
                        Strengths = "My strengths are many",
                        Support = "Third line"
                    }
                }
            };
        }

        #endregion
    }
}
