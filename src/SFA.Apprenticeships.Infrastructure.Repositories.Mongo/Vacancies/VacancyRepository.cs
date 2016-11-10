namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies
{
    using Application.Interfaces;
    using Common;
    using Common.Configuration;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Raa.Interfaces.Queries;
    using Domain.Raa.Interfaces.Repositories;
    using Entities;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class VacancyRepository : GenericMongoClient2<MongoVacancy>, IVacancyReadRepository, IVacancyWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public VacancyRepository(
            IConfigurationService configurationService,
            IMapper mapper,
            ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();

            Initialise(config.VacancyDb, "vacancies");

            _mapper = mapper;
            _logger = logger;
        }

        public Vacancy Get(int vacancyId)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancy with Id={0}", vacancyId);

            var mongoEntity = Collection.FindOne(Query<Vacancy>.EQ(v => v.VacancyId, vacancyId));

            return mongoEntity == null ? null : _mapper.Map<MongoVacancy, Vacancy>(mongoEntity);
        }

        public Vacancy GetByReferenceNumber(int vacancyReferenceNumber)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancy with Vacancy Reference Number={0}", vacancyReferenceNumber);

            var mongoEntity = Collection.FindOne(Query<Vacancy>.EQ(v => v.VacancyReferenceNumber, vacancyReferenceNumber));

            return mongoEntity == null ? null : _mapper.Map<MongoVacancy, Vacancy>(mongoEntity);
        }

        public Vacancy GetByVacancyGuid(Guid vacancyGuid)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancy with Vacancy Guid={0}", vacancyGuid);

            var mongoEntity = Collection.FindOne(Query<Vacancy>.EQ(v => v.VacancyGuid, vacancyGuid));

            return mongoEntity == null ? null : _mapper.Map<MongoVacancy, Vacancy>(mongoEntity);
        }

        public VacancySummary GetById(int vacancyId)
        {
            throw new NotImplementedException();
        }

        public List<VacancySummary> GetByIds(IEnumerable<int> vacancyIds)
        {
            var mongoEntities = Collection.Find(Query.In("VacancyId", new BsonArray(vacancyIds)));

            return mongoEntities.Select(e => _mapper.Map<MongoVacancy, VacancySummary>(e)).ToList();
        }

        public List<VacancySummary> GetByOwnerPartyIds(IEnumerable<int> ownerPartyIds)
        {
            var mongoEntities = Collection.Find(Query.In("VacancyOwnerRelationshipId", new BsonArray(ownerPartyIds)));

            return mongoEntities.Select(e => _mapper.Map<MongoVacancy, VacancySummary>(e)).ToList();
        }

        public List<VacancySummary> GetByOwnerPartyId(int ownerPartyId)
        {
            var mongoEntity = Collection.Find(Query<VacancySummary>.EQ(v => v.VacancyOwnerRelationshipId, ownerPartyId));

            return mongoEntity.Select(e => _mapper.Map<MongoVacancy, VacancySummary>(e)).ToList();
        }

        public int CountWithStatus(params VacancyStatus[] desiredStatuses)
        {
            _logger.Debug("Called Mongodb to count apprenticeship vacancies in status {0}", string.Join(",", desiredStatuses));

            var count = (int)Collection.Count(Query<Vacancy>.In(v => v.Status, desiredStatuses));

            _logger.Debug(string.Format("Found {0} apprenticeship vacancies with statuses in {1}", count, string.Join(",", desiredStatuses)));

            return count;
        }

        public List<VacancySummary> GetWithStatus(int pageSize, int page, bool filterByProviderBeenMigrated, params VacancyStatus[] desiredStatuses)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancies in status {0}", string.Join(",", desiredStatuses));

            var mongoEntities = Collection.Find(Query<Vacancy>.In(v => v.Status, desiredStatuses))
                .Select(e => _mapper.Map<MongoVacancy, VacancySummary>(e))
                .ToList();

            _logger.Debug(string.Format("Found {0} apprenticeship vacancies with statuses in {1}", mongoEntities.Count, string.Join(",", desiredStatuses)));

            return mongoEntities;
        }

        public List<VacancySummary> Find(ApprenticeshipVacancyQuery query, out int totalResultsCount)
        {
            _logger.Debug("Calling repository to find apprenticeship vacancies");

            var mongoQueryConditions = new List<IMongoQuery>()
            {
                Query<Vacancy>.EQ(vacancy => vacancy.Status, VacancyStatus.Live)
            };

            if (!string.IsNullOrWhiteSpace(query.FrameworkCodeName))
            {
                mongoQueryConditions.Add(Query<Vacancy>
                    .EQ(vacancy => vacancy.FrameworkCodeName, query.FrameworkCodeName));
            }

            if (query.LiveDate.HasValue)
            {
                // TODO: DateSubmitted should be DateLive (or DatePublished).
                mongoQueryConditions.Add(Query<Vacancy>
                    .GTE(vacancy => vacancy.DateSubmitted, query.LiveDate));
            }

            if (query.LatestClosingDate.HasValue)
            {
                mongoQueryConditions.Add(Query<Vacancy>
                    .LTE(vacancy => vacancy.ClosingDate, query.LatestClosingDate));
            }

            if (query.DesiredStatuses.Any())
            {
                mongoQueryConditions.Add(Query<Vacancy>
                    .In(vacancy => vacancy.Status, query.DesiredStatuses));
            }

            var queryBuilder = new QueryBuilder<Vacancy>();

            var vacancies = Collection.Find(queryBuilder.And(mongoQueryConditions))
                .SetSortOrder(SortBy.Ascending("VacancyReferenceNumber"))
                .SetSkip(query.PageSize * (query.RequestedPage - 1))
                .SetLimit(query.PageSize)
                .Select(vacancy => _mapper.Map<MongoVacancy, VacancySummary>(vacancy))
                .ToList();

            totalResultsCount = Convert.ToInt32(Collection.Count(queryBuilder.And(mongoQueryConditions)));

            _logger.Debug("Found {0} apprenticeship vacanc(ies)", vacancies.Count);

            return vacancies;
        }

        public void UnReserveVacancyForQa(int vacancyReferenceNumber)
        {
            // TODO: Unreserve vacancy not implemented for mongo
            throw new NotImplementedException();
        }

        public void IncrementOfflineApplicationClickThrough(int vacancyReferenceNumber)
        {
            _logger.Debug("Calling Mongodb to increment the NoOfOfflineApplicants property by one for vacancy with reference number: {0}", vacancyReferenceNumber);

            var args = new FindAndModifyArgs
            {
                Query = new QueryBuilder<Vacancy>().And(Query<Vacancy>.EQ(d => d.VacancyReferenceNumber, vacancyReferenceNumber), Query<Vacancy>.EQ(d => d.OfflineVacancy, true)),
                Update = MongoDB.Driver.Builders.Update.Inc("NoOfOfflineApplicants", 1),
                VersionReturned = FindAndModifyDocumentVersion.Modified
            };

            var result = Collection.FindAndModify(args);

            if (result.Ok)
            {
                _logger.Debug(
                    "Call to Mongodb to increment the NoOfOfflineApplicants property by one for vacancy with reference number: {0} successfully",
                    vacancyReferenceNumber);
            }
            else
            {
                _logger.Warn(
                    "Call to Mongodb to increment the NoOfOfflineApplicants property by one for vacancy with reference number: {0} failed: {1}, {2}",
                    vacancyReferenceNumber, result.Code, result.ErrorMessage);
            }
        }

        public void Delete(int vacancyId)
        {
            _logger.Debug("Calling repository to delete apprenticeship vacancy with Id={0}", vacancyId);

            Collection.Remove(Query<MongoVacancy>.EQ(o => o.VacancyId, vacancyId));

            _logger.Debug("Deleted apprenticeship vacancy with Id={0}", vacancyId);
        }

        public Vacancy Create(Vacancy entity)
        {
            _logger.Debug("Called Mongodb to save apprenticeship vacancy with id={0}", entity.VacancyId);

            var mongoEntity = SaveEntity(entity);

            _logger.Debug("Saved apprenticeship vacancy with to Mongodb with id={0}", entity.VacancyId);

            return _mapper.Map<MongoVacancy, Vacancy>(mongoEntity);
        }

        public Vacancy Update(Vacancy entity)
        {
            _logger.Debug("Called Mongodb to save apprenticeship vacancy with id={0}", entity.VacancyId);

            var mongoEntity = SaveEntity(entity);

            _logger.Debug("Saved apprenticeship vacancy with to Mongodb with id={0}", entity.VacancyId);

            return _mapper.Map<MongoVacancy, Vacancy>(mongoEntity);
        }

        private MongoVacancy SaveEntity(Vacancy entity)
        {
            if (entity.VacancyId == 0 && entity.VacancyReferenceNumber != 0)
            {
                entity.VacancyId = (int)entity.VacancyReferenceNumber;
            }

            SetCreatedDateTime(entity);
            SetUpdatedDateTime(entity);

            var mongoEntity = _mapper.Map<Vacancy, MongoVacancy>(entity);

            Collection.Save(mongoEntity);
            return mongoEntity;
        }

        public Vacancy ReserveVacancyForQA(int vacancyReferenceNumber)
        {
            _logger.Debug($"Calling Mongodb to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA");

            var username = Thread.CurrentPrincipal.Identity.Name;

            //TODO: Need to check that this number is available for QA via status and/or timeout
            //TODO: Possibly further discussion about having code like this in the repo
            var args = new FindAndModifyArgs
            {
                Query = Query<Vacancy>.EQ(d => d.VacancyReferenceNumber, vacancyReferenceNumber),
                Update =
                    MongoDB.Driver.Builders.Update.Set("Status", VacancyStatus.ReservedForQA)
                        .Set("QAUserName", username)
                        .Set("DateStartedToQA", DateTime.UtcNow),
                SortBy = SortBy.Null,
                Upsert = true,
                VersionReturned = FindAndModifyDocumentVersion.Modified
            };

            var result = Collection.FindAndModify(args);

            if (result.Ok)
            {
                _logger.Info($"Called Mongodb to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA successfully");

                var mongoEntity = result.GetModifiedDocumentAs<MongoVacancy>();

                return mongoEntity == null ? null : _mapper.Map<MongoVacancy, Vacancy>(mongoEntity);
            }

            _logger.Warn($"Call to Mongodb to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA failed: {result.Code}, {result.ErrorMessage}");
            return null;
        }

        public int GetVacancyIdByReferenceNumber(int vacancyReferenceNumber)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<int, IEnumerable<VacancyLocation>> GetVacancyLocationsByVacancyIds(IEnumerable<int> vacancyIds)
        {
            throw new NotImplementedException();
        }
    }
}
