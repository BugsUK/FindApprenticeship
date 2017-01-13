namespace SFA.Apprenticeships.Application.Organisation.IoC
{
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class OrganisationServiceRegistry : Registry
    {
        public OrganisationServiceRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });
        }
    }
}