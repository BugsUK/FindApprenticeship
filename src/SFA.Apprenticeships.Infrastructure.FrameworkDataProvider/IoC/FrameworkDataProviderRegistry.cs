namespace SFA.Apprenticeships.Infrastructure.FrameworkDataProvider.IoC
{
    using Application.ReferenceData;
    using StructureMap.Configuration.DSL;

    public class FrameworkDataProviderRegistry : Registry
    {
        public FrameworkDataProviderRegistry()
        {
            For<IReferenceDataProvider>().Use<FrameworkDataProvider>().Name = "FrameworkDataProvider";
        }
    }
}
