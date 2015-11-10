using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using SFA.Apprenticeships.Domain.Entities.Vacancies.ProviderVacancies;

namespace SFA.Apprenticeships.Infrastructure.Repositories.Vacancies
{
    using System;
    using Application.Interfaces.Logging;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Queries;
    using Domain.Interfaces.Repositories;
    using Mongo.Common;
    using Mongo.Common.Configuration;
    using MongoDB.Driver.Builders;
    using Entities;

    public class ApprenticeshipVacancyRepository : GenericMongoClient<MongoApprenticeshipVacancy>, IApprenticeshipVacancyReadRepository, IApprenticeshipVacancyWriteRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogService _logger;

        public ApprenticeshipVacancyRepository(
            IConfigurationService configurationService,
            IMapper mapper,
            ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();

            Initialise(config.VacancyDb, "apprenticeshipVacancies");

            _mapper = mapper;
            _logger = logger;
        }

        public ApprenticeshipVacancy Get(Guid id)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancy with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);
        }

        public ApprenticeshipVacancy Get(long vacancyReferenceNumber)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancy with Vacancy Reference Number={0}", vacancyReferenceNumber);

            var mongoEntity = Collection.FindOne(Query<ApprenticeshipVacancy>.EQ(v => v.VacancyReferenceNumber, vacancyReferenceNumber));

            return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);
        }

        public List<ApprenticeshipVacancy> GetForProvider(string ukPrn)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancies with Vacancy UkPrn = {0}", ukPrn);

            var mongoEntities = Collection.Find(Query<ApprenticeshipVacancy>.EQ(v => v.Ukprn, ukPrn))
                .Select(e => _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(e))
                .ToList();

            _logger.Debug(string.Format("Found {0} apprenticeship vacancies with ukprn ={1}", mongoEntities.Count, ukPrn));

            return mongoEntities;
        }

        public List<ApprenticeshipVacancy> GetForProvider(string ukPrn, List<ProviderVacancyStatuses> desiredStatuses)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancies with Vacancy UkPrn = {0} and in status {1}", ukPrn, string.Join(",", desiredStatuses));

            var queryConditionList = new List<IMongoQuery>();

            queryConditionList.Add(Query<ApprenticeshipVacancy>.EQ(v => v.Ukprn, ukPrn));
            queryConditionList.Add(Query<ApprenticeshipVacancy>.In(v => v.Status, desiredStatuses));

            var query = new QueryBuilder<ApprenticeshipVacancy>();

            var mongoEntities = Collection.Find(query.And(queryConditionList))
                .Select(e => _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(e))
                .ToList();

            _logger.Debug(string.Format("Found {0} apprenticeship vacancies with ukprn = {1} and statuses in {2}", mongoEntities.Count, ukPrn, string.Join(",", desiredStatuses)));

            return mongoEntities;
        }

        public List<ApprenticeshipVacancy> GetWithStatus(List<ProviderVacancyStatuses> desiredStatuses)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancies in status {0}", string.Join(",", desiredStatuses));

            var mongoEntities = Collection.Find(Query<ApprenticeshipVacancy>.In(v => v.Status, desiredStatuses))
                .Select(e => _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(e))
                .ToList();

            _logger.Debug(string.Format("Found {0} apprenticeship vacancies with statuses in {1}", mongoEntities.Count, string.Join(",", desiredStatuses)));

            return mongoEntities;
        }

        public List<ApprenticeshipVacancy>Find(ApprenticeshipVacancyQuery query, out int totalResultsCount)
        {
            _logger.Debug("Calling repository to find apprenticeship vacancies");

            var mongoQueryConditions = new List<IMongoQuery>()
            {
                Query<ApprenticeshipVacancy>.EQ(vacancy => vacancy.Status, ProviderVacancyStatuses.Live)
            };

            if (!string.IsNullOrWhiteSpace(query.FrameworkCodeName))
            {
                mongoQueryConditions.Add(Query<ApprenticeshipVacancy>
                    .EQ(vacancy => vacancy.FrameworkCodeName, query.FrameworkCodeName));
            }

            if (query.LiveDate.HasValue)
            {
                // TODO: DateSubmitted should be DateLive (or DatePublished).
                mongoQueryConditions.Add(Query<ApprenticeshipVacancy>
                    .GTE(vacancy => vacancy.DateSubmitted, query.LiveDate));
            }

            var queryBuilder = new QueryBuilder<ApprenticeshipVacancy>();

            var vacancies = Collection.Find(queryBuilder.And(mongoQueryConditions))
                .SetSortOrder(SortBy.Ascending("VacancyReferenceNumber"))
                .SetSkip(query.PageSize * (query.CurrentPage - 1))
                .SetLimit(query.PageSize)
                .Select(vacancy => _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(vacancy))
                .ToList();

            totalResultsCount = Convert.ToInt32(Collection.Count(queryBuilder.And(mongoQueryConditions)));

            _logger.Debug("Found {0} apprenticeship vacanc(ies)", vacancies.Count);

            return vacancies;
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete apprenticeship vacancy with Id={0}", id);

            Collection.Remove(Query<MongoApprenticeshipVacancy>.EQ(o => o.Id, id));

            _logger.Debug("Deleted apprenticeship vacancy with Id={0}", id);
        }

        public ApprenticeshipVacancy Save(ApprenticeshipVacancy entity)
        {
            _logger.Debug("Called Mongodb to save apprenticeship vacancy with id={0}", entity.EntityId);

            UpdateEntityTimestamps(entity);

            var mongoEntity = _mapper.Map<ApprenticeshipVacancy, MongoApprenticeshipVacancy>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved apprenticeship vacancy with to Mongodb with id={0}", entity.EntityId);

            return _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);
        }

        public ApprenticeshipVacancy ReserveVacancyForQA(long vacancyReferenceNumber, string username)
        {
            _logger.Debug($"Calling Mongodb to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA");

            //TODO: Need to check that this number is available for QA via status and/or timeout
            //TODO: Possibly further discussion about having code like this in the repo
            var args = new FindAndModifyArgs
            {
                Query = Query<ApprenticeshipVacancy>.EQ(d => d.VacancyReferenceNumber, vacancyReferenceNumber),
                Update =
                    Update.Set("Status", ProviderVacancyStatuses.ReservedForQA)
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

                var mongoEntity = result.GetModifiedDocumentAs<MongoApprenticeshipVacancy>();

                return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);
            }

            _logger.Warn($"Call to Mongodb to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA failed: {result.Code}, {result.ErrorMessage}");
            return null;
        }
    }
}
