namespace SFA.Apprenticeships.Data.Migrate.Faa.Entities.Mongo
{
    using System;
    using MongoDB.Bson.Serialization.Attributes;

    [BsonIgnoreExtraElements]
    public class VacancyApplication
    {
        [BsonId]
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public DateTime? DateApplied { get; set; }

        public int Status { get; set; }

        public Guid CandidateId { get; set; }

        public int LegacyApplicationId { get; set; }

        public string WithdrawnOrDeclinedReason { get; set; }

        public string UnsuccessfulReason { get; set; }

        public Vacancy Vacancy { get; set; }
    }
}