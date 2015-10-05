namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies.Entities
{
    using System;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoApprenticeshipVacancy : ApprenticeshipVacancy
    {
        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }
    }
}
