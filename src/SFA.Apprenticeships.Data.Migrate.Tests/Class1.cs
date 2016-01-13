namespace SFA.Apprenticeships.Data.Migrate.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using FluentAssertions;
    using System.Data.SqlClient;
    using Infrastructure.Azure.Configuration;
    using Infrastructure.Console;
    using Infrastructure.Interfaces;

    using NewDB = SFA.Apprenticeships.NewDB.Domain.Entities;
    using Infrastructure.Sql;

    [TestFixture]
    public class Class1
    {
        private IAvmsRepository _avms;
        private INewDBRepository _newDB;

        const int VacancyReferenceNumber_VacancyA = 42;


        [SetUp]
        public void SetUp()
        {
            var log = new ConsoleLogService();
            var configService = new AzureBlobConfigurationService(new MyConfigurationManager(), log);
            var config = configService.Get<MigrateFromAvmsConfiguration>();


            _avms = new AvmsDatabaseRespository(new GetOpenConnectionFromConnectionString(config.AvmsConnectionString));

            {
                var getNewDbConnection = new GetOpenConnectionFromConnectionString(config.NewConnectionString);
                /*
                getNewDbConnection.Query<int>("DELETE Vacancy.Vacancy WHERE VacancyReferenceNumber = @VacancyReferenceNumber",
                    new { VacancyReferenceNumber = VacancyReferenceNumber_VacancyA } );
                */
                _newDB = new NewDBDatabaseRespository(getNewDbConnection);
            }

            /*
alter table Vacancy.Vacancy
alter column FrameworkId INT NULL

alter table Vacancy.Vacancy
alter column StandardId INT NULL

insert into vacancy.vacancyparty
values ('ES', 'Employer A', 'A', null, 'URL', 1, null)
insert into vacancy.vacancyparty
values ('PS', 'Provider A', 'A', null, 'URL', null, 1)

insert into reference.occupation
values (1, 1, 'O01', 'Occupation 1', 'Occupation 1', null)

insert into reference.framework
values (1, 'F01', 'Framework 1', 'Framework 1', 1, 1, null, null)

            */
        }

        private class MyConfigurationManager : IConfigurationManager
        {
            public string ConfigurationFilePath
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public string GetAppSetting(string key)
            {
                throw new NotImplementedException();
            }

            public T GetAppSetting<T>(string key)
            {
                if (key == "ConfigurationStorageConnectionString")
                    return (T)Convert.ChangeType("UseDevelopmentStorage=true", typeof(T));

                throw new NotImplementedException();
            }

            public string TryGetAppSetting(string key)
            {
                throw new NotImplementedException();
            }
        }


        [Test]
        public void FoundSomeVacancies()
        {
            // Arrange.
            // Act.
            var vacancies = _avms.GetAllVacancies();

            // Assert.
            vacancies.Take(1).Count().Should().Be(1);
        }

        [Test]
        public void InsertNewVacancy()
        {
            const int VacancyPartyId_EmployerA = 3;
            const int VacancyPartyId_ProviderA = 4;
            const int FrameworkId_Framework1 = 1;
            const string VacancyTypeCode_Apprenticeship = "A";
            const string VacancyStatusCode_Live = "LIV";
            const string VacancyLocationTypeCode_Specific = "S";
            const string TrainingTypeCode_Framework = "F";
            const string LevelCode_Intermediate = "2";

            // Arrange.
            var vacancy = new NewDB.Vacancy.Vacancy()
            {
                VacancyId = Guid.NewGuid(),
                DeliveryProviderVacancyPartyId = VacancyPartyId_ProviderA,
                OwnerVacancyPartyId = VacancyPartyId_ProviderA,
                ContractOwnerVacancyPartyId = VacancyPartyId_ProviderA,
                EmployerVacancyPartyId = VacancyPartyId_EmployerA,
                ManagerVacancyPartyId = VacancyPartyId_ProviderA,
                VacancyReferenceNumber = VacancyReferenceNumber_VacancyA,
                VacancyTypeCode = VacancyTypeCode_Apprenticeship,
                VacancyStatusCode = VacancyStatusCode_Live,
                VacancyLocationTypeCode = VacancyLocationTypeCode_Specific,
                Title = "X",
                TrainingTypeCode = TrainingTypeCode_Framework, // Framework
                FrameworkId = FrameworkId_Framework1,
                FrameworkIdComment = 42,
                StandardId = null,
                LevelCode = LevelCode_Intermediate,

            };

            // Act.
            _newDB.AddVacancy(vacancy);

            // Assert.
            // No crash!!
        }

        public void InsertAndFetchNewVacancy()
        {
            // Arrange.
            var vacancy = new NewDB.Vacancy.Vacancy()
            {
                DeliveryProviderVacancyPartyId = 42
            };

            // Act.
            _newDB.AddVacancy(vacancy);
            var vacancy2 = _newDB.GetVacancy(vacancy.VacancyId);

            // Assert.
            vacancy.DeliveryProviderVacancyPartyId.ShouldBeEquivalentTo(vacancy2.DeliveryProviderVacancyPartyId);
        }

    }
}
