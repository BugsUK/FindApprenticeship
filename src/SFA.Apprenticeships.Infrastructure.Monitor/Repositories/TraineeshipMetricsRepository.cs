namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Candidates.Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;

    public class TraineeshipMetricsRepository : GenericMongoClient<MongoCandidate>, ITraineeshipMetricsRepository
    {
        public TraineeshipMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.ApplicationsDb, "traineeships");
        }

        public long GetApplicationCount()
        {
            return Collection.Count();
        }

        public long GetApplicationsPerCandidateCount()
        {
            return Collection.Distinct("CandidateId").Count();
        }
    }
}
