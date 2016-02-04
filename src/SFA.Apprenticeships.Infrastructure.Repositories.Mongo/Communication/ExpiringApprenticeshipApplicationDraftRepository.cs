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

    public class ExpiringApprenticeshipApplicationDraftRepository : CommunicationRepository<ExpiringApprenticeshipApplicationDraft, Guid>, IExpiringApprenticeshipApplicationDraftRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;

        public ExpiringApprenticeshipApplicationDraftRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
            : base(configurationService, "expiringdraftapplications")
        {
            _mapper = mapper;
            _logger = logger;
        }

        public ExpiringApprenticeshipApplicationDraft Get(Guid id)
        {
            _logger.Debug("Calling repository to get expiring apprenticeship application draft alert with Id={0}", id);

            var mongoEntity = Collection.FindOneByIdAs<MongoApprenticeshipApplicationExpiringDraft>(id);
            var message = mongoEntity == null ? "Found no expiring apprenticeship application draft alert with Id={0}" : "Found expiring apprenticeship application draft alert with Id={0}";

            _logger.Debug(message, id);

            return _mapper.Map<MongoApprenticeshipApplicationExpiringDraft, ExpiringApprenticeshipApplicationDraft>(mongoEntity);
        }

        public void Save(ExpiringApprenticeshipApplicationDraft expiringDraft)
        {
            _logger.Debug("Calling repository to save expiring draft with Id={0}, CandidateId={1}, VacancyId={2}", expiringDraft.EntityId, expiringDraft.CandidateId, expiringDraft.VacancyId);

            var mongoExpiringDraft = _mapper.Map<ExpiringApprenticeshipApplicationDraft, MongoApprenticeshipApplicationExpiringDraft>(expiringDraft);
            UpdateEntityTimestamps(mongoExpiringDraft);
            mongoExpiringDraft.SentDateTime = mongoExpiringDraft.BatchId.HasValue ? mongoExpiringDraft.DateUpdated : null;

            _logger.Debug("Saved expiring draft to repository with Id={0}, CandidateId={1}, VacancyId={2}", expiringDraft.EntityId, expiringDraft.CandidateId, expiringDraft.VacancyId);

            Collection.Save(mongoExpiringDraft);
        }

        public void Delete(ExpiringApprenticeshipApplicationDraft expiringDraft)
        {
            _logger.Debug("Calling repository to delete expiring draft with Id={0}", expiringDraft.EntityId);

            Collection.Remove(Query.EQ("_id", expiringDraft.EntityId));

            _logger.Debug("Deleted expiring draft with Id={0}", expiringDraft.EntityId);
        }

        public List<ExpiringApprenticeshipApplicationDraft> GetExpiringApplications(int vacancyId)
        {
            _logger.Debug("Calling repository to get all expiring drafts for VacancyId={0}", vacancyId);

            var mongoExpiringDrafts = Collection.FindAs<MongoApprenticeshipApplicationExpiringDraft>(Query.EQ("VacancyId", vacancyId));
            var expiringDrafts = _mapper.Map<IEnumerable<MongoApprenticeshipApplicationExpiringDraft>, IEnumerable<ExpiringApprenticeshipApplicationDraft>>(mongoExpiringDrafts).ToList();

            _logger.Debug("Found {0} expiring drafts for VacancyId={1}", expiringDrafts.Count, vacancyId);

            return expiringDrafts;
        }

        public Dictionary<Guid, List<ExpiringApprenticeshipApplicationDraft>> GetCandidatesDailyDigest()
        {
            _logger.Debug("Calling repository to get all candidates expiring drafts for their daily digest");

            var mongoExpiringDrafts = Collection.FindAs<MongoApprenticeshipApplicationExpiringDraft>(Query.EQ("BatchId", BsonNull.Value));
            var expiringDrafts = _mapper.Map<IEnumerable<MongoApprenticeshipApplicationExpiringDraft>, IEnumerable<ExpiringApprenticeshipApplicationDraft>>(mongoExpiringDrafts);
            var candidatesDailyDigest = expiringDrafts.GroupBy(x => x.CandidateId).ToDictionary(grp => grp.Key, grp => grp.ToList());

            _logger.Debug("Found expiring drafts for {0} candidates", candidatesDailyDigest.Count);

            return candidatesDailyDigest;
        }

        public IEnumerable<Guid> GetAlertsCreatedOnOrBefore(DateTime dateTime)
        {
            _logger.Debug("Calling repository to get all expiring apprenticeship application draft alerts created on or before={0}", dateTime);

            var alertIds = Collection
                .AsQueryable<MongoApprenticeshipApplicationExpiringDraft>()
                .Where(each => each.DateCreated <= dateTime)
                .Select(each => each.EntityId);

            _logger.Debug("Called repository to get all expiring apprenticeship application draft alerts created on or before={0}", dateTime);

            return alertIds;
        }
    }
}