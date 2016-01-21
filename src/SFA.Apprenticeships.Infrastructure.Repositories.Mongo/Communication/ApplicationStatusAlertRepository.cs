namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Communication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Communication;
    using Domain.Interfaces.Repositories;
    using Mongo.Communication.Entities;
    using MongoDB.Bson;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using SFA.Infrastructure.Interfaces;

    public class ApplicationStatusAlertRepository : CommunicationRepository<ApplicationStatusAlert>, IApplicationStatusAlertRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public ApplicationStatusAlertRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
            : base(configurationService, "applicationstatusalerts")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public ApplicationStatusAlert Get(Guid id)
        {
            _logger.Debug("Calling repository to get application status alert with Id={0}", id);

            var mongoEntity = Collection.FindOneByIdAs<MongoApplicationStatusAlert>(id);
            var message = mongoEntity == null ? "Found no application status alert with Id={0}" : "Found application status alert with Id={0}";

            _logger.Debug(message, id);

            return _mapper.Map<MongoApplicationStatusAlert, ApplicationStatusAlert>(mongoEntity);
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

        public void Delete(ApplicationStatusAlert alert)
        {
            _logger.Debug("Calling repository to delete application status alert with Id={0}", alert.EntityId);

            Collection.Remove(Query.EQ("_id", alert.EntityId));

            _logger.Debug("Deleted application status alert with Id={0}", alert.EntityId);
        }

        public List<ApplicationStatusAlert> GetForApplication(Guid applicationId)
        {
            _logger.Debug("Calling repository to get all application status alerts for ApplicationId={0}", applicationId);

            var mongoAlerts = Collection.FindAs<MongoApplicationStatusAlert>(Query.EQ("ApplicationId", applicationId));
            var alerts = _mapper.Map<IEnumerable<MongoApplicationStatusAlert>, IEnumerable<ApplicationStatusAlert>>(mongoAlerts).OrderByDescending(a => a.DateCreated).ToList();

            _logger.Debug("Found {0} application status alerts for ApplicationId={1}", alerts.Count, applicationId);

            return alerts;
        }

        public Dictionary<Guid, List<ApplicationStatusAlert>> GetCandidatesDailyDigest()
        {
            _logger.Debug("Calling repository to get all candidates application status alerts for their daily digest");

            var mongoAlerts = Collection.FindAs<MongoApplicationStatusAlert>(Query.EQ("BatchId", BsonNull.Value));
            var alerts = _mapper.Map<IEnumerable<MongoApplicationStatusAlert>, IEnumerable<ApplicationStatusAlert>>(mongoAlerts);
            var candidatesDailyDigest = alerts.GroupBy(x => x.CandidateId).ToDictionary(grp => grp.Key, grp => grp.ToList());

            _logger.Debug("Found application status alerts for {0} candidates", candidatesDailyDigest.Count);

            return candidatesDailyDigest;
        }

        public IEnumerable<Guid> GetAlertsCreatedOnOrBefore(DateTime dateTime)
        {
            _logger.Debug("Calling repository to get all application status alerts created on or before={0}", dateTime);

            var alertIds = Collection
                .AsQueryable<MongoApplicationStatusAlert>()
                .Where(each => each.DateCreated <= dateTime)
                .Select(each => each.EntityId);

            _logger.Debug("Called repository to get all application status alerts created on or before={0}", dateTime);

            return alertIds;
        }
    }
}