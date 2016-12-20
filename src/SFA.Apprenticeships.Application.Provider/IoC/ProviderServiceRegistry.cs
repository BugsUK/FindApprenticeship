namespace SFA.Apprenticeships.Application.Provider.IoC
{
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class ProviderServiceRegistry : Registry
    {
        public ProviderServiceRegistry()
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