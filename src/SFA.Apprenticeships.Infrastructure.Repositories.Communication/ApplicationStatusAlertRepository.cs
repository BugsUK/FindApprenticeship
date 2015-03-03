namespace SFA.Apprenticeships.Infrastructure.Repositories.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using MongoDB.Driver.Builders;

    public class ApplicationStatusAlertRepository : CommunicationRepository<ApplicationStatusAlert>, IApplicationStatusAlertRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public ApplicationStatusAlertRepository(IConfigurationManager configurationManager, IMapper mapper, ILogService logger) : base(configurationManager, "applicationstatusalerts")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public void Save(ApplicationStatusAlert alert)
        {
            _logger.Debug("Calling repository to save application status alert with Id={0}, ApplicationId={1}, VacancyId={2}", alert.EntityId, alert.ApplicationId, alert.CandidateId);

            var mongoAlert = _mapper.Map<ApplicationStatusAlert, MongoApplicationStatusAlert>(alert);
            UpdateEntityTimestamps(mongoAlert);
            mongoAlert.SentDateTime = mongoAlert.BatchId.HasValue ? mongoAlert.DateUpdated : null;

            _logger.Debug("Saved application status alert to repository with Id={0}, ApplicationId={1}, VacancyId={2}", alert.EntityId, alert.ApplicationId, alert.CandidateId);

            Collection.Save(mongoAlert);
        }

        public List<ApplicationStatusAlert> Get(Guid applicationId)
        {
            _logger.Debug("Calling repository to get all application status alerts for ApplicationId={0}", applicationId);

            var mongoAlerts = Collection.FindAs<MongoApplicationStatusAlert>(Query.EQ("ApplicationId", applicationId));
            var alerts = _mapper.Map<IEnumerable<MongoApplicationStatusAlert>, IEnumerable<ApplicationStatusAlert>>(mongoAlerts).OrderByDescending(a => a.DateCreated).ToList();

            _logger.Debug("Found {0} application status alerts for ApplicationId={1}", alerts.Count, applicationId);

            return alerts;
        }
    }
}