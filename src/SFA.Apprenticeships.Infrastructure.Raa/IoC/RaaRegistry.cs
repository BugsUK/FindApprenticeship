namespace SFA.Apprenticeships.Infrastructure.Raa.IoC
{
    using Application.ReferenceData;
    using Application.Reporting;

    using Application.Candidate.Configuration;

    using StructureMap.Configuration.DSL;

    public class RaaRegistry : Registry
    {
        public RaaRegistry(ServicesConfiguration servicesConfiguration)
        {
            if (servicesConfiguration.ServiceImplementation == ServicesConfiguration.Raa)
            {
                For<IReferenceDataProvider>()
                    .Use<ReferenceDataProvider>();

                For<IReportingProvider>()
                    .Use<ReportingProvider>();
            }
        }
    }
}