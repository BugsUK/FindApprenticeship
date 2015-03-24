namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Candidates.Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Driver.Linq;

    public class CandidateMetricsRepository : GenericMongoClient<MongoCandidate>, ICandidateMetricsRepository
    {
        public CandidateMetricsRepository(IConfigurationService configurationService)
        {
            var config = configurationService.Get<MongoConfiguration>(MongoConfiguration.MongoConfigurationName);
            Initialise(config.CandidatesDb, "candidates");
        }

        public int GetVerfiedMobileNumbersCount()
        {
            return Collection
                .AsQueryable()
                .Count(each => each.CommunicationPreferences.AllowMobile);
        }
    }
}