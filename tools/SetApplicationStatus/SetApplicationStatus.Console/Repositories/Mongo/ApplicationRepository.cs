namespace SetApplicationStatus.Console.Repositories.Mongo
{
    using System;
    using System.Collections.Generic;
    using MongoDB.Driver;
    using MongoDB.Bson;
    using Entities.Mongo;

    public class ApplicationRepository
    {
        private const string CollectionName = "apprenticeships";

        private readonly IMongoDatabase _database;

        public ApplicationRepository(string connectionString)
        {
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;

            _database = new MongoClient(connectionString).GetDatabase(databaseName);
        }

        public IEnumerable<Application> GetApplicationByLegacyIds(IEnumerable<int> ids)
        {
            var options = new FindOptions<Application>
            {
                Projection = GetVacancyApplicationProjection()
            };

            var filter = Builders<Application>
                .Filter.In(each => each.LegacyApplicationId, ids);

            var cursor = _database
                .GetCollection<Application>(CollectionName)
                .FindSync(filter, options);

            return cursor.ToList();
        }


        private static ProjectionDefinition<Application> GetVacancyApplicationProjection()
        {
            return Builders<Application>.Projection
                .Include(application => application.Id)
                .Include(application => application.LegacyApplicationId)
                .Include(application => application.Status)
                .Include(application => application.DateUpdated);
        }

        public bool SetApplicationStatus(Guid applicationId, ApplicationStatus applicationStatus)
        {
            var collection = _database.GetCollection<Application>(CollectionName);

            var filter = new BsonDocument("_id", applicationId);

            var update = Builders<Application>
                .Update
                .Set("Status", applicationStatus)
                .Set("DateUpdated", DateTime.UtcNow);

            var options = new FindOneAndUpdateOptions<Application>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = false
            };

            var result = collection.FindOneAndUpdate(filter, update, options);

            return result != null;
        }
    }
}
