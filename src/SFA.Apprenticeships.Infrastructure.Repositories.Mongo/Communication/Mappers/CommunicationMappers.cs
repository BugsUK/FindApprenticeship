namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Communication.Mappers
{
    using Domain.Entities.Communication;
    using Infrastructure.Common.Mappers;
    using Mongo.Communication.Entities;

    public class CommunicationMappers : MapperEngine
    {
        public override void Initialise()
        {
            InitialiseCommunicationMappers();
        }

        private void InitialiseCommunicationMappers()
        {
            Mapper.CreateMap<ExpiringApprenticeshipApplicationDraft, MongoApprenticeshipApplicationExpiringDraft>();
            Mapper.CreateMap<MongoApprenticeshipApplicationExpiringDraft, ExpiringApprenticeshipApplicationDraft>();

            Mapper.CreateMap<ApplicationStatusAlert, MongoApplicationStatusAlert>();
            Mapper.CreateMap<MongoApplicationStatusAlert, ApplicationStatusAlert>();

            Mapper.CreateMap<SavedSearchAlert, MongoSavedSearchAlert>();
            Mapper.CreateMap<MongoSavedSearchAlert, SavedSearchAlert>();

            Mapper.CreateMap<ContactMessage, MongoContactMessage>();
            Mapper.CreateMap<MongoContactMessage, ContactMessage>();
        }
    }
}