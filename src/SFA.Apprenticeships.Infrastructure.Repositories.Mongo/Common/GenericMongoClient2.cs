namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Common
{
    using System;
    using CuttingEdge.Conditions;
    using Domain.Entities.Raa;
    using MongoDB.Driver;

    public class GenericMongoClient2<T>
    {
        private MongoCollection<T> _collection;
        private MongoDatabase _database;

        protected string ConnectionString { get; set; }

        protected string CollectionName { get; set; }

        protected MongoCollection<T> Collection
        {
            get
            {
                return _collection ?? (_collection = Database.GetCollection<T>(CollectionName));
            }
        }

        private MongoDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    Initialise(ConnectionString, CollectionName);
                }
                return _database;
            }
        }

        protected void SetCreatedDateTime(ICreatableEntity entity)
        {
            if (entity.CreatedDateTime== DateTime.MinValue)
            {
                entity.CreatedDateTime = DateTime.UtcNow;
            }
        }

        protected void SetUpdatedDateTime(IUpdatableEntity entity)
        {
            entity.UpdatedDateTime = DateTime.UtcNow;
        }

        protected void Initialise(string connectionString, string collectionName)
        {
            Condition.Requires(connectionString);
            Condition.Requires(collectionName);

            CollectionName = collectionName;

            var mongoDbName = MongoUrl.Create(connectionString).DatabaseName;

            _database = new MongoClient(connectionString)
                .GetServer()
                .GetDatabase(mongoDbName);
        }
    }
}
