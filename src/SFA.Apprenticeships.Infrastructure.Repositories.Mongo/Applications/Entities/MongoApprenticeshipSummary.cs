namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Applications.Entities
{
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.Apprenticeships;
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class MongoApprenticeshipSummary : VacancySummary
    {
        public MongoApprenticeshipSummary()
        {
        }

        public ApprenticeshipLocationType VacancyLocationType { get; set; }

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }
        
        public string WorkingWeek { get; set; }
    }
}
