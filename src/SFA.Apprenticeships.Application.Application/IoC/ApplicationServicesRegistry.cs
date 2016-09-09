namespace SFA.Apprenticeships.Application.Application.IoC
{
    using Interfaces.Applications;
    using Strategies;
    using StructureMap.Configuration.DSL;

    public class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            // Strategies
            For<Strategies.Apprenticeships.IGetApplicationForReviewStrategy>().Use<Strategies.Apprenticeships.GetApplicationForReviewStrategy>();
            For<Strategies.Apprenticeships.IUpdateApplicationNotesStrategy>().Use<Strategies.Apprenticeships.UpdateApplicationNotesStrategy>();
            For<Strategies.Traineeships.IGetApplicationForReviewStrategy>().Use<Strategies.Traineeships.GetApplicationForReviewStrategy>();
            For<Strategies.Traineeships.IUpdateApplicationNotesStrategy>().Use<Strategies.Traineeships.UpdateApplicationNotesStrategy>();
            For<IApplicationStatusUpdateStrategy>().Use<ApplicationStatusUpdateStrategy>();
            For<IApplicationStatusAlertStrategy>().Use<ApplicationStatusAlertStrategy>();
            For<ISetApplicationStatusStrategy>().Use<SetApplicationStatusStrategy>();

            // Services
            For<IApprenticeshipApplicationService>().Use<ApprenticeshipApplicationService>();
            For<ITraineeshipApplicationService>().Use<TraineeshipApplicationService>();
        }
    }
}
