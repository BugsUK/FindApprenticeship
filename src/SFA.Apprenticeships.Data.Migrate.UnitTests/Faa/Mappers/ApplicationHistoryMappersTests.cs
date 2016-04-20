﻿namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Mappers
{
    using System.Linq;
    using Builders;
    using FluentAssertions;
    using Migrate.Faa.Mappers;
    using NUnit.Framework;

    [TestFixture]
    public class ApplicationHistoryMappersTests
    {
        [Test]
        public void DraftVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(10).Build();

            //Act
            var applicationHistory = vacancyApplication.MapApplicationHistory(vacancyApplication.LegacyApplicationId);

            //Assert
            applicationHistory.Should().NotBeNullOrEmpty();
            applicationHistory.Count.Should().Be(1);
            var draftHistory = applicationHistory.First();
            draftHistory.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            draftHistory.UserName.Should().Be("");
            draftHistory.ApplicationHistoryEventDate.Should().Be(vacancyApplication.DateCreated);
            draftHistory.ApplicationHistoryEventTypeId.Should().Be(1);
            draftHistory.ApplicationHistoryEventSubTypeId.Should().Be(1);
            draftHistory.Comment.Should().Be("Status Change");
        }

        [Test]
        public void SubmittingVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(20).Build();

            //Act
            var applicationHistory = vacancyApplication.MapApplicationHistory(vacancyApplication.LegacyApplicationId);

            //Assert
            applicationHistory.Should().NotBeNullOrEmpty();
            applicationHistory.Count.Should().Be(1);
            var draftHistory = applicationHistory.First();
            draftHistory.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            draftHistory.UserName.Should().Be("");
            draftHistory.ApplicationHistoryEventDate.Should().Be(vacancyApplication.DateCreated);
            draftHistory.ApplicationHistoryEventTypeId.Should().Be(1);
            draftHistory.ApplicationHistoryEventSubTypeId.Should().Be(1);
            draftHistory.Comment.Should().Be("Status Change");
        }

        [Test]
        public void SubmittedVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(30).Build();

            //Act
            var applicationHistory = vacancyApplication.MapApplicationHistory(vacancyApplication.LegacyApplicationId);

            //Assert
            applicationHistory.Should().NotBeNullOrEmpty();
            applicationHistory.Count.Should().Be(2);
            var draftHistory = applicationHistory[0];
            draftHistory.ApplicationHistoryEventSubTypeId.Should().Be(1);
            var submittedHistory = applicationHistory[1];
            submittedHistory.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            submittedHistory.UserName.Should().Be("");
            // ReSharper disable once PossibleInvalidOperationException
            submittedHistory.ApplicationHistoryEventDate.Should().Be(vacancyApplication.DateUpdated.Value);
            submittedHistory.ApplicationHistoryEventTypeId.Should().Be(1);
            submittedHistory.ApplicationHistoryEventSubTypeId.Should().Be(2);
            submittedHistory.Comment.Should().Be("Status Change");
        }

        [Test]
        public void InProgressVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(40).Build();

            //Act
            var applicationHistory = vacancyApplication.MapApplicationHistory(vacancyApplication.LegacyApplicationId);

            //Assert
            applicationHistory.Should().NotBeNullOrEmpty();
            applicationHistory.Count.Should().Be(3);
            var draftHistory = applicationHistory[0];
            draftHistory.ApplicationHistoryEventSubTypeId.Should().Be(1);
            var submittedHistory = applicationHistory[1];
            submittedHistory.ApplicationHistoryEventSubTypeId.Should().Be(2);
            var inProgressHistory = applicationHistory[2];
            inProgressHistory.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            inProgressHistory.UserName.Should().Be("");
            // ReSharper disable once PossibleInvalidOperationException
            inProgressHistory.ApplicationHistoryEventDate.Should().Be(vacancyApplication.DateUpdated.Value);
            inProgressHistory.ApplicationHistoryEventTypeId.Should().Be(1);
            inProgressHistory.ApplicationHistoryEventSubTypeId.Should().Be(3);
            inProgressHistory.Comment.Should().Be("Status Change");
        }

        [Test]
        public void WithdrawnVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(15).Build();

            //Act
            var applicationHistory = vacancyApplication.MapApplicationHistory(vacancyApplication.LegacyApplicationId);

            //Assert
            applicationHistory.Should().NotBeNullOrEmpty();
            applicationHistory.Count.Should().Be(2);
            var draftHistory = applicationHistory[0];
            draftHistory.ApplicationHistoryEventSubTypeId.Should().Be(1);
            var withdrawnHistory = applicationHistory[1];
            withdrawnHistory.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            withdrawnHistory.UserName.Should().Be("");
            // ReSharper disable once PossibleInvalidOperationException
            withdrawnHistory.ApplicationHistoryEventDate.Should().Be(vacancyApplication.DateUpdated.Value);
            withdrawnHistory.ApplicationHistoryEventTypeId.Should().Be(1);
            withdrawnHistory.ApplicationHistoryEventSubTypeId.Should().Be(4);
            withdrawnHistory.Comment.Should().Be("Status Change");
        }

        [Test]
        public void UnsuccessfulVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(90).Build();

            //Act
            var applicationHistory = vacancyApplication.MapApplicationHistory(vacancyApplication.LegacyApplicationId);

            //Assert
            applicationHistory.Should().NotBeNullOrEmpty();
            applicationHistory.Count.Should().Be(4);
            var draftHistory = applicationHistory[0];
            draftHistory.ApplicationHistoryEventSubTypeId.Should().Be(1);
            var submittedHistory = applicationHistory[1];
            submittedHistory.ApplicationHistoryEventSubTypeId.Should().Be(2);
            var inProgressHistory = applicationHistory[2];
            inProgressHistory.ApplicationHistoryEventSubTypeId.Should().Be(3);
            var unsuccessfulHistory = applicationHistory[3];
            unsuccessfulHistory.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            unsuccessfulHistory.UserName.Should().Be("");
            // ReSharper disable once PossibleInvalidOperationException
            unsuccessfulHistory.ApplicationHistoryEventDate.Should().Be(vacancyApplication.DateUpdated.Value);
            unsuccessfulHistory.ApplicationHistoryEventTypeId.Should().Be(1);
            unsuccessfulHistory.ApplicationHistoryEventSubTypeId.Should().Be(5);
            unsuccessfulHistory.Comment.Should().Be("Status Change");
        }

        [Test]
        public void SuccessfulVacancyApplicationTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(80).Build();

            //Act
            var applicationHistory = vacancyApplication.MapApplicationHistory(vacancyApplication.LegacyApplicationId);

            //Assert
            applicationHistory.Should().NotBeNullOrEmpty();
            applicationHistory.Count.Should().Be(4);
            var draftHistory = applicationHistory[0];
            draftHistory.ApplicationHistoryEventSubTypeId.Should().Be(1);
            var submittedHistory = applicationHistory[1];
            submittedHistory.ApplicationHistoryEventSubTypeId.Should().Be(2);
            var inProgressHistory = applicationHistory[2];
            inProgressHistory.ApplicationHistoryEventSubTypeId.Should().Be(3);
            var successfulHistory = applicationHistory[3];
            successfulHistory.ApplicationId.Should().Be(vacancyApplication.LegacyApplicationId);
            successfulHistory.UserName.Should().Be("");
            // ReSharper disable once PossibleInvalidOperationException
            successfulHistory.ApplicationHistoryEventDate.Should().Be(vacancyApplication.DateUpdated.Value);
            successfulHistory.ApplicationHistoryEventTypeId.Should().Be(1);
            successfulHistory.ApplicationHistoryEventSubTypeId.Should().Be(6);
            successfulHistory.Comment.Should().Be("Status Change");
        }

        [Test]
        public void SuccessfulVacancyApplicationDictionaryTest()
        {
            //Arrange
            var vacancyApplication = new VacancyApplicationBuilder().WithStatus(80).Build();

            //Act
            var applicationHistory = vacancyApplication.MapApplicationHistoryDictionary(vacancyApplication.LegacyApplicationId);

            //Assert
            applicationHistory.Should().NotBeNullOrEmpty();
            applicationHistory.Count.Should().Be(4);
            var draftHistory = applicationHistory[0];
            draftHistory["ApplicationHistoryEventSubTypeId"].Should().Be(1);
            var submittedHistory = applicationHistory[1];
            submittedHistory["ApplicationHistoryEventSubTypeId"].Should().Be(2);
            var inProgressHistory = applicationHistory[2];
            inProgressHistory["ApplicationHistoryEventSubTypeId"].Should().Be(3);
            var successfulHistory = applicationHistory[3];
            successfulHistory["ApplicationId"].Should().Be(vacancyApplication.LegacyApplicationId);
            successfulHistory["UserName"].Should().Be("");
            // ReSharper disable once PossibleInvalidOperationException
            successfulHistory["ApplicationHistoryEventDate"].Should().Be(vacancyApplication.DateUpdated.Value);
            successfulHistory["ApplicationHistoryEventTypeId"].Should().Be(1);
            successfulHistory["ApplicationHistoryEventSubTypeId"].Should().Be(6);
            successfulHistory["Comment"].Should().Be("Status Change");
        }
    }
}