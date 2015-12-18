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
    [TestFixture]
    public class Class1
    {
        private IAvmsRepository _avms;

        [SetUp]
        public void SetUp()
        {
            var log = new ConsoleLogService();
            var configService = new AzureBlobConfigurationService(new MyConfigurationManager(), log);
            var config = configService.Get<MigrateFromAvmsConfiguration>();


            _avms = new AvmsDatabaseRespository(() =>
            {
                var conn = new SqlConnection(config.AvmsConnectionString);
                conn.Open();
                return conn;
            });
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


    }
}
