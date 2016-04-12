namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Mongo
{
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class Vacancy
    {
        public int Id { get; set; }

        public string VacancyReference { get; set; }

        public string Title { get; set; }
    }
}