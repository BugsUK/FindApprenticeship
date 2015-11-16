namespace SFA.Apprenticeships.Web.Manage.IntegrationTests
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Web.Mvc;
    using Controllers;
    using Domain.Entities.Locations;
    using Domain.Entities.Organisations;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using FluentAssertions;
    using Infrastructure.Repositories.Vacancies.Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class VacancyControllerIntegrationTests : ManageWebIntegrationTestsBase
    {
        private MongoCollection<MongoApprenticeshipVacancy> Collection;

        [Test]
        public void GetBasicDetailsWithACorrectVacancyShouldReturnTheVacancyWithAnEmptyModelState()
        {
            // Arrange
            const int vacancyReferenceNumber = 1;
            const string title = "Vacancy title";
            var vacancy = GetCorrectVacancy(vacancyReferenceNumber, title);

            InitializeDatabaseWithVacancy(vacancy);

            var vacancyController = Container.GetInstance<VacancyController>();

            //Act
            var view = vacancyController.BasicDetails(vacancyReferenceNumber);

            // Assert
            view.Should().BeOfType<ViewResult>();

            var viewResult = view as ViewResult;

            viewResult.ViewData.ModelState.Should().HaveCount(0);
            viewResult.Model.Should().BeOfType<NewVacancyViewModel>();
            var vacancyViewModel = viewResult.Model as NewVacancyViewModel;
            vacancyViewModel.VacancyReferenceNumber.Should().Be(vacancyReferenceNumber);
            vacancyViewModel.Title.Should().Be(title);
        }

        [Test]
        public void GetBasicDetailsWithAVacancyWithErrorsShouldReturnTheVacancyWithAModelStateFilled()
        {
            // Arrange
            const int vacancyReferenceNumber = 1;
            const string title = "Vacancy title";
            var vacancy = GetVacancyWithErrorsInBasicDetails(vacancyReferenceNumber, title);

            InitializeDatabaseWithVacancy(vacancy);

            var vacancyController = Container.GetInstance<VacancyController>();

            //Act
            var view = vacancyController.BasicDetails(vacancyReferenceNumber);

            // Assert
            view.Should().BeOfType<ViewResult>();

            var viewResult = view as ViewResult;

            viewResult.ViewData.ModelState.Should().HaveCount(1);
            viewResult.ViewData.ModelState.Keys.First().Should().Be("OfflineApplicationUrl");
            viewResult.Model.Should().BeOfType<NewVacancyViewModel>();
            var vacancyViewModel = viewResult.Model as NewVacancyViewModel;
            vacancyViewModel.VacancyReferenceNumber.Should().Be(vacancyReferenceNumber);
            vacancyViewModel.Title.Should().Be(title);
        }

        [TearDown]
        public void TearDown()
        {
            Collection.RemoveAll();
        }

        private void RemoveVacancyFromDatabase(int vacancyReferenceNumber)
        {
            Collection.Remove(Query<MongoApprenticeshipVacancy>.EQ(o => o.VacancyReferenceNumber, vacancyReferenceNumber));
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
                FrameworkCodeName = "framework code name",
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
                    Employer = new Employer
                    {
                        Address = new Address
                        {
                            AddressLine1 = "address line 1",
                            AddressLine2 = "address line 2",
                            AddressLine3 = "address line 3",
                            AddressLine4 = "address line 4",
                            Postcode = "postcode",
                            Uprn = "ukprn",
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
                TrainingType = TrainingType.Frameworks
            };
        }

        private static MongoApprenticeshipVacancy GetVacancyWithErrorsInBasicDetails(int vacancyReferenceNumber, string title)
        {
            var vacancy = GetCorrectVacancy(vacancyReferenceNumber, title);
            vacancy.OfflineVacancy = true;
            vacancy.OfflineApplicationUrl = "invalid url"; // Error

            return vacancy;
        }
    }
}