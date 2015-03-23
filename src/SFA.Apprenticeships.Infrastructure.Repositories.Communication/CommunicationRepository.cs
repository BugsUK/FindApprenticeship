namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication
{
    using Domain.Entities;
    using Domain.Interfaces.Configuration;
    using Mongo.Common;
    using Mongo.Common.Configuration;

    public abstract class CommunicationRepository<TBaseEntity> : GenericMongoClient<TBaseEntity> where TBaseEntity : BaseEntity
    {
        protected CommunicationRepository(IConfigurationService configurationService, string collectionName)
        {
            var config = configurationService.Get<MongoConfiguration>(MongoConfiguration.MongoConfigurationName);
            Initialise(config.CommunicationsDb, collectionName);
        }
    }
}