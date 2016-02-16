//NB: This class is not placed within a namespace, intentionally.
//http://www.nunit.org/index.php?p=setupFixture&r=2.4

using NUnit.Framework;
using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests;
using SFA.Apprenticeships.Infrastructure.Repositories.Sql.Tests.Common;

[SetUpFixture]
public class TestAssemblySetsup
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

            var seedScripts = new string[]
            {
            };
            dbInitialiser.Seed(seedScripts);

            dbInitialiser.Seed(SeedData.Providers);
            dbInitialiser.Seed(SeedData.ProviderUsers);
            dbInitialiser.Seed(SeedData.AgencyUsers);
        }
    }
}
