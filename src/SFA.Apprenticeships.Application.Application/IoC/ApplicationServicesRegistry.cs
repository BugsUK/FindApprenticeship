namespace SFA.Apprenticeships.Application.Application.IoC
{
    using Applications.Strategies;
    using Interfaces.Applications;
    using Strategies.Apprenticeships;
    using StructureMap.Configuration.DSL;

    public class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            // Strategies
            For<IGetApplicationForReviewStrategy>().Use<GetApplicationForReviewStrategy>();
            For<IUpdateApplicationNotesStrategy>().Use<UpdateApplicationNotesStrategy>();
            For<IApplicationStatusUpdateStrategy>().Use<ApplicationStatusUpdateStrategy>();
            For<IApplicationStatusAlertStrategy>().Use<ApplicationStatusAlertStrategy>();

            // Services
            For<IApprenticeshipApplicationService>().Use<ApprenticeshipApplicationService>();
        }
    }
}
