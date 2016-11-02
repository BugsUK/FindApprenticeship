// ReSharper disable HeuristicUnreachableCode
namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.IntegrationTests
{
    using System;
    using Common;
    using NUnit.Framework;

    [SetUpFixture]
    public class TestAssemblySetup
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTestsInThisAssembly()
        {
            var dbInitialiser = new DatabaseInitialiser();

            // ReSharper disable once ConvertToConstant.Local
            var shouldDropAndCreateDatabase = true;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (!shouldDropAndCreateDatabase)
            {
                return;
            }

            dbInitialiser.Publish(true);

            var scriptsBasePath = AppDomain.CurrentDomain.BaseDirectory + @"\Scripts\";

            var scriptFilePaths = new[]
            {
                scriptsBasePath + "InsertPerson.sql",
                scriptsBasePath + "InsertEmployerContact.sql",
                scriptsBasePath + "InsertEmployer.sql",
                scriptsBasePath + "InsertProvider.sql",
                scriptsBasePath + "InsertVacancyOwnerRelationship.sql",
                scriptsBasePath + "InsertAgencyUser.sql",
                scriptsBasePath + "InsertProviderUser.sql"
            };

            dbInitialiser.Seed(scriptFilePaths);
        }
    }
}
