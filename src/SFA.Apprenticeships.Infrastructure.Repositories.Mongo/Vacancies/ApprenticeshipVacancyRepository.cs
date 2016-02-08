namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Domain.Interfaces.Queries;
    using Domain.Interfaces.Repositories;
    using Common;
    using Common.Configuration;
    using Entities;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using SFA.Infrastructure.Interfaces;

    public class ApprenticeshipVacancyRepository : GenericMongoClient<MongoApprenticeshipVacancy, Guid>, IApprenticeshipVacancyReadRepository, IApprenticeshipVacancyWriteRepository
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

        public ApprenticeshipVacancy Get(int vacancyReferenceNumber)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancy with Vacancy Reference Number={0}", vacancyReferenceNumber);

            var mongoEntity = Collection.FindOne(Query<ApprenticeshipVacancy>.EQ(v => v.VacancyReferenceNumber, vacancyReferenceNumber));

            return mongoEntity == null ? null : _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);
        }
        
        public List<ApprenticeshipVacancy> GetForProvider(string ukPrn, string providerSiteErn)
        {
            _logger.Debug("Called Mongodb to get apprenticeship vacancies with Vacancy UkPrn = {0}, providerSiteErn = {1}", ukPrn, providerSiteErn);

            var queryConditionList = new List<IMongoQuery>();

            queryConditionList.Add(Query<ApprenticeshipVacancy>.EQ(v => v.Ukprn, ukPrn));
            queryConditionList.Add(Query<ApprenticeshipVacancy>.NotIn(v => v.Status, new List<ProviderVacancyStatuses>() { ProviderVacancyStatuses.ParentVacancy }));
            queryConditionList.Add(Query<ApprenticeshipVacancy>.EQ(v => v.ProviderSiteEmployerLink.ProviderSiteErn, providerSiteErn));

            var mongoEntities = Collection.Find(Query.And(queryConditionList))
                .Select(e => _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(e))
                .OrderByDescending(v => v.DateCreated)
                .ToList();

            _logger.Debug(string.Format("Found {0} apprenticeship vacancies with ukprn = {1}, providerSiteErn = {2}", mongoEntities.Count, ukPrn, providerSiteErn));

            return mongoEntities;
        }

        public List<ApprenticeshipVacancy> GetWithStatus(params ProviderVacancyStatuses[] desiredStatuses)
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

            if (query.LatestClosingDate.HasValue)
            {
                mongoQueryConditions.Add(Query<ApprenticeshipVacancy>
                    .LTE(vacancy => vacancy.ClosingDate, query.LatestClosingDate));
            }

            if (query.DesiredStatuses.Any())
            {
                mongoQueryConditions.Add(Query<ApprenticeshipVacancy>
                    .In(vacancy => vacancy.Status, query.DesiredStatuses));
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

        public ApprenticeshipVacancy DeepSave(ApprenticeshipVacancy entity)
        {
            _logger.Debug("Called Mongodb to save apprenticeship vacancy with id={0}", entity.EntityId);

            var mongoEntity = SaveEntity(entity);

            _logger.Debug("Saved apprenticeship vacancy with to Mongodb with id={0}", entity.EntityId);

            return _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);
        }

        public ApprenticeshipVacancy ShallowSave(ApprenticeshipVacancy entity)
        {
            _logger.Debug("Called Mongodb to shallow save apprenticeship vacancy with id={0}", entity.EntityId);

            var mongoEntity = SaveEntity(entity);

            _logger.Debug("Shallow saved apprenticeship vacancy with to Mongodb with id={0}", entity.EntityId);

            return _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);
        }

        public ApprenticeshipVacancy ReplaceLocationInformation(Guid vacancyGuid, bool? isEmployerLocationMainApprenticeshipLocation,
            int? numberOfPositions, IEnumerable<VacancyLocationAddress> vacancyLocationAddresses, string locationAddressesComment,
            string additionalLocationInformation, string additionalLocationInformationComment)
        {
            _logger.Debug($"Calling Mongodb to replace location information of the vacancy with Id: {vacancyGuid}");

            var vacancy = Get(vacancyGuid);

            vacancy.IsEmployerLocationMainApprenticeshipLocation = isEmployerLocationMainApprenticeshipLocation;
            vacancy.NumberOfPositions = numberOfPositions;
            vacancy.LocationAddressesComment = locationAddressesComment;
            vacancy.AdditionalLocationInformation = additionalLocationInformation;
            vacancy.AdditionalLocationInformationComment = additionalLocationInformationComment;
            vacancy.LocationAddresses = vacancyLocationAddresses.ToList();

            var mongoEntity = SaveEntity(vacancy);

            _logger.Info($"Called Mongodb to replace location information of the vacancy with Id: {vacancyGuid}");

            return _mapper.Map<MongoApprenticeshipVacancy, ApprenticeshipVacancy>(mongoEntity);
        }

        private MongoApprenticeshipVacancy SaveEntity(ApprenticeshipVacancy entity)
        {
            UpdateEntityTimestamps(entity);

            var mongoEntity = _mapper.Map<ApprenticeshipVacancy, MongoApprenticeshipVacancy>(entity);

            Collection.Save(mongoEntity);
            return mongoEntity;
        }
        
        public ApprenticeshipVacancy ReserveVacancyForQA(int vacancyReferenceNumber)
        {
            _logger.Debug($"Calling Mongodb to get and reserve vacancy with reference number: {vacancyReferenceNumber} for QA");

            var username = Thread.CurrentPrincipal.Identity.Name;

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
