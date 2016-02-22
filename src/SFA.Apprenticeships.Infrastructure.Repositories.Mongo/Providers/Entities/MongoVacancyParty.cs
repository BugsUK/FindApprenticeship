namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Providers.Entities
{
    using System;
    using Domain.Entities.Raa.Parties;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoVacancyParty : VacancyParty
    {
        [BsonId]
        public Guid Id
        {
            get { return VacancyPartyGuid; }
            set { VacancyPartyGuid = value; }
        }
    }
}