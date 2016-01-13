namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication
{
    using Domain.Entities;
    using SFA.Infrastructure.Interfaces;
    using Mongo.Common;
    using Mongo.Common.Configuration;

    public abstract class CommunicationRepository<TBaseEntity> : GenericMongoClient<TBaseEntity> where TBaseEntity : BaseEntity
    {
        protected CommunicationRepository(IConfigurationService configurationService, string collectionName)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.CommunicationsDb, collectionName);
        }
    }
}