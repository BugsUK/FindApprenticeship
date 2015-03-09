namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication
{
    using System;
    using Application.Interfaces.Logging;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;

    public class ContactMessageRepository : CommunicationRepository<ContactMessage>, IContactMessageRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;

        public ContactMessageRepository(IConfigurationManager configurationManager, IMapper mapper, ILogService logger)
            : base(configurationManager, "contactmessages")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public ContactMessage Save(ContactMessage contactMessage)
        {
            var userId = contactMessage.UserId.HasValue
                ? contactMessage.UserId.ToString()
                : "<null>";

            var args = new object[] { contactMessage.EntityId, userId, contactMessage.Name, contactMessage.Email, contactMessage.Enquiry, contactMessage.Details };

            _logger.Debug("Calling repository to save contact message with Id='{0}', UserId='{1}', Name='{2}', Email='{3}', Enquiry='{4}', Details='{5}'", args);

            var mongoEntity = _mapper.Map<ContactMessage, MongoContactMessage>(contactMessage);

            UpdateEntityTimestamps(mongoEntity);

            _logger.Debug("Saved contact message to repository with Id='{0}', UserId='{1}', Name='{2}', Email='{3}', Enquiry='{4}', Details='{5}'", args);

            Collection.Save(mongoEntity);

            return _mapper.Map<MongoContactMessage, ContactMessage>(mongoEntity);
        }

        public void Delete(Guid id)
        {
            throw new NotSupportedException();
        }
    }
}
