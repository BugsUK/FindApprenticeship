namespace SFA.Apprenticeships.Web.Employer.AcceptanceTests.IoC
{
    using Ioc;
    using TechTalk.SpecFlow;
    using StructureMap;

    public class WebTestRegistry
    {

        public static Container Container;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Container = new Container(x =>
            {
                x.AddRegistry<EmployerWebRegistry>();
            });
        }
    }
}
