//NB: This class is not placed within a namespace, intentionally.
//http://www.nunit.org/index.php?p=setupFixture&r=2.4

using System;
using NUnit.Framework;
using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests;
using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Common;

[SetUpFixture]
public class TestAssemblySetup
{
    [SetUp]
    public void RunBeforeAnyTestsInThisAssembly()
    {
        var dbInitialiser = new DatabaseInitialiser();

        //control db initialisation and seeding through these two bools
        var shouldDropCreateDB = true;

        if (shouldDropCreateDB)
        {
            dbInitialiser.Publish(true);

            var seedScripts = new []
            {
                AppDomain.CurrentDomain.BaseDirectory + "Scripts\\InsertContactPreferenceType.sql",
                AppDomain.CurrentDomain.BaseDirectory + "Scripts\\InsertPersonType.sql",
                AppDomain.CurrentDomain.BaseDirectory + "Scripts\\InsertPersonTitleType.sql",
                AppDomain.CurrentDomain.BaseDirectory + "Scripts\\InsertPerson.sql",
                AppDomain.CurrentDomain.BaseDirectory + "Scripts\\InsertEmployerContact.sql",
                AppDomain.CurrentDomain.BaseDirectory + "Scripts\\InsertEmployer.sql",
                AppDomain.CurrentDomain.BaseDirectory + "Scripts\\InsertVacancyProvisionRelationshipStatusType.sql",
                AppDomain.CurrentDomain.BaseDirectory + "Scripts\\InsertVacancyOwnerRelationship.sql"
            };
            dbInitialiser.Seed(seedScripts);

            // dbInitialiser.Seed(SeedData.Providers);
            // dbInitialiser.Seed(SeedData.ProviderUsers);
            // dbInitialiser.Seed(SeedData.AgencyUsers);
        }
    }
}
