namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Communication
{
    using Domain.Entities;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using SFA.Infrastructure.Interfaces;

    public abstract class CommunicationRepository<TBaseEntity> : GenericMongoClient<TBaseEntity> where TBaseEntity : BaseEntity
    {
        protected CommunicationRepository(IConfigurationService configurationService, string collectionName)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.CommunicationsDb, collectionName);
        }
    }
}