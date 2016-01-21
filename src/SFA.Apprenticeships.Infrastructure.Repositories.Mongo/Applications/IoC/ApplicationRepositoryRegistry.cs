namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Applications.IoC
{
    using Domain.Interfaces.Repositories;
    using Applications;
    using Mappers;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class ApplicationRepositoryRegistry : Registry
    {
        public ApplicationRepositoryRegistry()
        {
            // Apprenticeships.
            For<IMapper>().Use<ApprenticeshipApplicationMappers>().Name = "ApplicationDetailMapper";

            For<IApprenticeshipApplicationWriteRepository>()
                .Use<ApprenticeshipApplicationRepository>()
                .Ctor<IMapper>()
                .Named("ApplicationDetailMapper");

            For<IApprenticeshipApplicationReadRepository>()
                .Use<ApprenticeshipApplicationRepository>()
                .Ctor<IMapper>()
                .Named("ApplicationDetailMapper");

            // Traineeships.
            For<IMapper>().Use<TraineeshipApplicationMappers>().Name = "TraineeshipApplicationDetailMapper";

            For<ITraineeshipApplicationWriteRepository>()
                .Use<TraineeshipApplicationRepository>()
                .Ctor<IMapper>()
                .Named("TraineeshipApplicationDetailMapper");

            For<ITraineeshipApplicationReadRepository>()
                .Use<TraineeshipApplicationRepository>()
                .Ctor<IMapper>()
                .Named("TraineeshipApplicationDetailMapper");
        }
    }
}
