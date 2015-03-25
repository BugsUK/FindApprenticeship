﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication
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

        public ContactMessageRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
            : base(configurationService, "contactmessages")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public ContactMessage Save(ContactMessage contactMessage)
        {
            var userId = contactMessage.UserId.HasValue
                ? contactMessage.UserId.ToString()
                : "<null>";

            _logger.Debug("Calling repository to save contact message with UserId='{0}', Name='{1}', Email='{2}', Enquiry='{3}', Details='{4}'",
                userId, contactMessage.Name, contactMessage.Email, contactMessage.Enquiry, contactMessage.Details);

            var mongoEntity = _mapper.Map<ContactMessage, MongoContactMessage>(contactMessage);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved contact message to repository with Id='{0}', UserId='{1}', Name='{2}', Email='{3}', Enquiry='{4}', Details='{5}'", 
                mongoEntity.EntityId, userId, contactMessage.Name, contactMessage.Email, contactMessage.Enquiry, contactMessage.Details);

            return _mapper.Map<MongoContactMessage, ContactMessage>(mongoEntity);
        }

        public void Delete(Guid id)
        {
            throw new NotSupportedException();
        }
    }
}
