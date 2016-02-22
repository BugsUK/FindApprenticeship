namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies.Entities
{
    using System;
    using Domain.Entities.Raa.Locations;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoVacancyLocationAddress : VacancyLocationAddress
    {
        [BsonId]
        public Guid Id
        {
            get { return VacancyLocationAddressGuid; }
            set { VacancyLocationAddressGuid = value; }
        }
    }
}