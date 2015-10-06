namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices.IoC
{
    using Application.ReferenceData;
    using StructureMap.Configuration.DSL;
    using TacticalDataServices;

    public class TacticalDataServicesRegistry : Registry
    {
        public TacticalDataServicesRegistry()
        {
            For<IReferenceDataProvider>().Use<FrameworkDataProvider>().Name = "FrameworkDataProvider";
        }
    }
}
