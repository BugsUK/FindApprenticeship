namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Communication
{
    using Domain.Entities;
    using Common;
    using Common.Configuration;
    using SFA.Infrastructure.Interfaces;

    public abstract class CommunicationRepository<TBaseEntity, TEntityKey> : GenericMongoClient<TBaseEntity, TEntityKey> where TBaseEntity : BaseEntity<TEntityKey>
    {
        protected CommunicationRepository(IConfigurationService configurationService, string collectionName)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.CommunicationsDb, collectionName);
        }
    }
}