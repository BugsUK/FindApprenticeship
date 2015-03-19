namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Candidates.Entities;
    using Mongo.Common;

    public class SavedSearchesMetricsRepository : GenericMongoClient<MongoSavedSearch>, ISavedSearchesMetricsRepository
    {
        public SavedSearchesMetricsRepository(IConfigurationManager configurationManager)
            : base(configurationManager, "Candidates.mongoDB", "savedsearches")
        {
        }

        public int GetSavedSearchesCount()
        {
            return (int)Collection.Count();
        }
    }
}