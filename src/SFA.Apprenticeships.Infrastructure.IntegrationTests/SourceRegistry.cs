namespace SFA.Apprenticeships.Infrastructure.IntegrationTests
{
    using Application.ReferenceData;
    using Infrastructure.Raa;
    using StructureMap.Configuration.DSL;

    public class SourceRegistry : Registry
    {
        public SourceRegistry()
        {
            For<IReferenceDataProvider>()
                .Use<ReferenceDataProvider>();
        }
    }
}
