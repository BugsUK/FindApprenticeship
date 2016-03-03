namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies.Entities
{
    using System;
    using Domain.Entities.Raa.Locations;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoVacancyLocation : VacancyLocation
    {
        //[BsonId]
        //public Guid Id
        //{
        //    get { return VacancyLocationGuid; }
        //    set { VacancyLocationGuid = value; }
        //}
    }
}