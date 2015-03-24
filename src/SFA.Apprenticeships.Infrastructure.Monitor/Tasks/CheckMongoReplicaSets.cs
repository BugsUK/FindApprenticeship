namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Linq;
    using Configuration;
    using MongoDB.Bson;
    using Domain.Interfaces.Configuration;
    using Mongo.Common;
    using Application.Interfaces.Logging;

    public class CheckMongoReplicaSets : IMonitorTask
    {
        private readonly ILogService _logger;
        private readonly int _expectedMongoReplicaSetCount;
        private const string ReplicaSetGetStatusCommand = "replSetGetStatus";
        private readonly IMongoAdminClient _mongoAdminClient;

        public CheckMongoReplicaSets(IMongoAdminClient mongoAdminClient, IConfigurationService configurationService, ILogService logger)
        {
            _mongoAdminClient = mongoAdminClient;
            _logger = logger;
            _expectedMongoReplicaSetCount = configurationService.Get<MonitorConfiguration>(MonitorConfiguration.ConfigurationName).ExpectedMongoReplicaSetCount;
        }

        public string TaskName
        {
            get { return "Check Mongo Replica Sets"; }
        }

        public void Run()
        {
            var verifyReplicaSets = false;
            var isReplicaSet = _mongoAdminClient.IsReplicaSet();
            if (_expectedMongoReplicaSetCount > 1 && isReplicaSet)
            {
                verifyReplicaSets = true;
                _logger.Debug("Replica set members will be verified");
            }
            else if (_expectedMongoReplicaSetCount == 1 && !isReplicaSet)
            {
                _logger.Debug("Replica set members will not be verified");
            }
            else
                _logger.Error("{0} config is invalid. ExpectedReplicaSetCount: {1}, IsReplicaSet: {2}", TaskName, _expectedMongoReplicaSetCount, isReplicaSet);

            if (verifyReplicaSets)
            {
                var result = _mongoAdminClient.RunCommand(ReplicaSetGetStatusCommand);
                var members = (BsonArray)result.Response["members"];
                var membersCount = members.Values.Count();
                if (_expectedMongoReplicaSetCount != membersCount)
                {
                    throw new Exception(string.Format("Mongo DB replica set count {0} does not match expected {1}", membersCount, _expectedMongoReplicaSetCount));
                }
                const string memberHealth = "health";
                if (members.Values.Any(m => m[memberHealth] != 1))
                {
                    var unhealthyMembers = string.Join(", ", members.Values.Where(m => m[memberHealth] != 1).Select(m => string.Format("Id: {0}, Name: {1}", m["_id"], m["name"])));
                    throw new Exception(string.Format("The following Mongo DB replica set members appear to be down: {0}", unhealthyMembers));
                }
            }
        }
    }
}