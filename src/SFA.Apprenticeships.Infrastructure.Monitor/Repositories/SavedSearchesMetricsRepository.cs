namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using SFA.Infrastructure.Interfaces;
    using Infrastructure.Repositories.Mongo.Candidates.Entities;
    using Infrastructure.Repositories.Mongo.Common;
    using Infrastructure.Repositories.Mongo.Common.Configuration;

    using SFA.Apprenticeships.Application.Interfaces;

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