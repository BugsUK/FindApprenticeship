namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Common
{
    using MongoDB.Driver;

    public interface IMongoAdminClient
    {
        bool IsReplicaSet();

        CommandResult RunCommand(string command);
    }
}