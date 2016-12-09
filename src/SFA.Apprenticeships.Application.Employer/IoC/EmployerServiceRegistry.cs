namespace SFA.Apprenticeships.Application.Employer.IoC
{
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class EmployerServiceRegistry : Registry
    {
        public EmployerServiceRegistry()
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