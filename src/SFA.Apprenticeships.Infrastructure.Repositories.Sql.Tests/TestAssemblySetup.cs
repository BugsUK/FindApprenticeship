﻿using System;
using NUnit.Framework;
using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Common;

// ReSharper disable HeuristicUnreachableCode
namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests
{

    [SetUpFixture]
    public class TestAssemblySetup
    {
        [SetUp]
        public void RunBeforeAnyTestsInThisAssembly()
        {
            var dbInitialiser = new DatabaseInitialiser();

            // ReSharper disable once ConvertToConstant.Local
            var shouldDropAndCreateDatabase = false;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (!shouldDropAndCreateDatabase)
            {
                return;
            }

            dbInitialiser.Publish(true);

            var scriptsBasePath = AppDomain.CurrentDomain.BaseDirectory + @"\Scripts\";

            var scriptFilePaths = new[]
            {
                scriptsBasePath + "InsertContactPreferenceType.sql",
                scriptsBasePath + "InsertPersonType.sql",
                scriptsBasePath + "InsertPersonTitleType.sql",
                scriptsBasePath + "InsertPerson.sql",
                scriptsBasePath + "InsertEmployerContact.sql",
                scriptsBasePath + "InsertEmployer.sql",
                scriptsBasePath + "InsertVacancyProvisionRelationshipStatusType.sql",
                scriptsBasePath + "InsertVacancyOwnerRelationship.sql",
                scriptsBasePath + "InsertProviderUser.sql"
            };

            dbInitialiser.Seed(scriptFilePaths);

            // dbInitialiser.Seed(SeedData.Providers);
            // dbInitialiser.Seed(SeedData.ProviderUsers);
            // dbInitialiser.Seed(SeedData.AgencyUsers);
        }
    }
}
