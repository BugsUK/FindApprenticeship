namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Linq;
    using Domain.Interfaces.Configuration;
    using Infrastructure.Repositories.Candidates.Entities;
    using Mongo.Common;
    using MongoDB.Driver.Linq;

    public class CandidateMetricsRepository : GenericMongoClient<MongoCandidate>, ICandidateMetricsRepository
    {
        public CandidateMetricsRepository(IConfigurationManager configurationManager)
            : base(configurationManager, "Candidates.mongoDB", "candidates")
        {
        }

        public int GetVerfiedMobileNumbersCount()
        {
            return Collection
                .AsQueryable()
                .Count(each => each.CommunicationPreferences.AllowMobile);
        }
    }
}