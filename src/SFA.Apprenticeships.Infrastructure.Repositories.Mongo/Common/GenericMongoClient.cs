namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Common
{
    using System;
    using CuttingEdge.Conditions;
    using Domain.Entities;
    using MongoDB.Driver;

    public class GenericMongoClient<T, TKey> where T : BaseEntity<TKey>
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

        protected void UpdateEntityTimestamps(BaseEntity<TKey> entity)
        {
            // determine whether this is a "new" entity being saved for the first time
            if (entity.DateCreated == DateTime.MinValue)
            {
                entity.DateCreated = DateTime.UtcNow;
                entity.DateUpdated = null;
            }
            else
            {
                entity.DateUpdated = DateTime.UtcNow;
            }
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
