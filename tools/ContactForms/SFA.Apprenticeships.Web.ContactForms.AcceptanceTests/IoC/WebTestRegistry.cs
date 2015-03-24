namespace SFA.Apprenticeships.Web.ContactForms.AcceptanceTests.IoC
{
    using ContactForms.Ioc;
    using StructureMap;
    using TechTalk.SpecFlow;

    public class WebTestRegistry
    {
        public static Container Container;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Container = new Container(x =>
            {
                x.AddRegistry<ContactFormsWebRegistry>();
            });
        }
    }
}
