namespace SFA.Apprenticeships.Infrastructure.Raa.IoC
{
    using Application.ReferenceData;
    using Strategies;
    using StructureMap.Configuration.DSL;
    
    public class RaaRegistry : Registry
    {
        public RaaRegistry()
        {
            For<IReferenceDataProvider>().Use<ReferenceDataProvider>();

            For<IGetReleaseNotesStrategy>().Use<GetReleaseNotesStrategy>();
        }
    }
}