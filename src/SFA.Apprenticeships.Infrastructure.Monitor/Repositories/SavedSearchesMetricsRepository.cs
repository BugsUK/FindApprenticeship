namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Repositories.Candidates.Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;

    public class SavedSearchesMetricsRepository : GenericMongoClient<MongoSavedSearch>, ISavedSearchesMetricsRepository
    {
        public SavedSearchesMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.MetricsCandidatesDb, "savedsearches");
        }

        public int GetSavedSearchesCount()
        {
            return (int)Collection.Count();
        }
    }
}