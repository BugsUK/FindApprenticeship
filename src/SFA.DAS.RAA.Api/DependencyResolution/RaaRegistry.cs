namespace SFA.DAS.RAA.Api.DependencyResolution
{
    using Services;
    using StructureMap;

    public class RaaRegistry : Registry
    {
        public RaaRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

            For<IAuthenticationService>().Use<ApiKeyAuthenticationService>();
        }
    }
}