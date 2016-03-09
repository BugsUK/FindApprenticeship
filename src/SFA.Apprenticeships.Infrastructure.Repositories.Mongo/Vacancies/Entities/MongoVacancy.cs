namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies.Entities
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoVacancy : Vacancy
    {
        [BsonId]
        public Guid Id
        {
            get { return VacancyGuid; }
            set { VacancyGuid = value; }
        }
    }
}
