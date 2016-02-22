namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Employers.Entities
{
    using System;
    using Domain.Entities.Raa.Parties;
    using MongoDB.Bson.Serialization.Attributes;

    public class MongoEmployer : Employer
    {
        [BsonId]
        public Guid Id
        {
            get { return EmployerGuid; }
            set { EmployerGuid = value; }
        }
    }
}