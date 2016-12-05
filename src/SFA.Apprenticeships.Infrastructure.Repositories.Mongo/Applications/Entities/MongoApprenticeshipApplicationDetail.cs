namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Applications.Entities
{
    using System;
    using Domain.Entities.Applications;
    using Domain.Entities.Vacancies;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoApprenticeshipApplicationDetail : ApplicationDetail
    {
        public MongoApprenticeshipApplicationDetail()
        {
            Vacancy = new MongoApprenticeshipSummary();
        }

        public DateTime? SuccessfulDateTime { get; set; }

        public DateTime? UnsuccessfulDateTime { get; set; }

        public string WithdrawnOrDeclinedReason { get; set; }

        public string UnsuccessfulReason { get; set; }

        [BsonId]
        public Guid Id
        {
            get { return EntityId; }
            set { EntityId = value; }
        }

        public MongoApprenticeshipSummary Vacancy { get; set; }

    }
}