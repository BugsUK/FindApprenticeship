namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies.Entities
{
    using System;
    using Domain.Entities;

    public class MongoReferenceNumber : ReferenceNumber
    {
        public Guid Id { get; set; }
    }
}
