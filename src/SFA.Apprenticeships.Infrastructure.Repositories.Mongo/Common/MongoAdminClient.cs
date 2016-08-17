namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Common
{
    using System;
    using Configuration;
    using MongoDB.Driver;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    
    public class MongoAdminClient : IMongoAdminClient
    {
        private readonly ILogService _logger;
        private readonly MongoDatabase _database;

        public MongoAdminClient(IConfigurationService configurationService, ILogService logger)
        {
            _logger = logger;
            var mongoConfig = configurationService.Get<MongoConfiguration>();
            var mongoDbName = MongoUrl.Create(mongoConfig.AdminDb).DatabaseName;
            _database = new MongoClient(mongoConfig.AdminDb).GetServer().GetDatabase(mongoDbName);
        }

        public bool IsReplicaSet()
        {
            var result = RunCommand("getCmdLineOpts");
            if (result.Ok)
            {
                var parsed = result.Response["parsed"].ToString();
                var isReplicaSet = parsed.Contains("replSet");
                return isReplicaSet;
            }
            throw new Exception($"Result of the MongoDB command was not OK {result.Ok} {result.Response}");
        }

        public CommandResult RunCommand(string command)
        {
            _logger.Info("Running command {0}", command);
            var result = _database.RunCommand(command);
            _logger.Info("Command Result {0} {1}", result.Ok, result.Response);
            
            return result;
        }
    }
}