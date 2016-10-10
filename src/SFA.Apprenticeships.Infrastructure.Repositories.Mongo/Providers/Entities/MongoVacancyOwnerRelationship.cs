namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers.Entities
{
    using System;
    using Domain.Entities.Raa.Parties;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoVacancyOwnerRelationship : VacancyOwnerRelationship
    {
        [BsonId]
        public Guid Id
        {
            get { return VacancyOwnerRelationshipGuid; }
            set { VacancyOwnerRelationshipGuid = value; }
        }
    }
}