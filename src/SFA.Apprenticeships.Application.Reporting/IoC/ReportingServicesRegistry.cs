namespace SFA.Apprenticeships.Application.Reporting.IoC
{
    using Interfaces.Reporting;
    using Strategies;
    using StructureMap.Configuration.DSL;

    public class ReportingServicesRegistry : Registry
    {
        public ReportingServicesRegistry()
        {
            // Strategies
            For<IGetApplicationsReceivedResultItemsStrategy>().Use<GetApplicationsReceivedResultItemsStrategy>();
            For<IGetCandidatesWithApplicationsResultItemsStrategy>().Use<GetCandidatesWithApplicationsResultItemsStrategy>();

            // Services
            For<IReportingService>().Use<ReportingService>();
        }
    }
}
