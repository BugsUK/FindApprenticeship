namespace SFA.Apprenticeships.Infrastructure.Monitor.Tasks
{
    using System;
    using System.Linq;
    using Configuration;
    using Infrastructure.Repositories.Mongo.Common;
    using MongoDB.Bson;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;

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
            _expectedMongoReplicaSetCount = configurationService.Get<MonitorConfiguration>().ExpectedMongoReplicaSetCount;
        }

        public string TaskName
        {
            get { return "Check Mongo Replica Sets"; }
        }

        public void Run()
        {
            var verifyReplicaSets = false;
            var isReplicaSet = _mongoAdminClient.IsReplicaSet();
            if (isReplicaSet)
            {
                verifyReplicaSets = true;
                _logger.Debug("Replica set members will be verified");
            }

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