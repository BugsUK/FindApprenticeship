namespace SFA.Apprenticeships.Application.Application.IoC
{
    using Interfaces.Applications;
    using Strategies.Apprenticeships;
    using StructureMap.Configuration.DSL;

    public class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            For<IGetApplicationForReviewStrategy>().Use<GetApplicationForReviewStrategy>();
            For<IUpdateApplicationNotesStrategy>().Use<UpdateApplicationNotesStrategy>();

            For<IApprenticeshipApplicationService>().Use<ApprenticeshipApplicationService>();
        }
    }
}