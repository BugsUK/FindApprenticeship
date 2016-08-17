namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Common;
    using Common.Configuration;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using ApplicationErrorCodes = Application.Interfaces.Applications.ErrorCodes;

    public class TraineeshipApplicationRepository : GenericMongoClient<MongoTraineeshipApplicationDetail>, ITraineeshipApplicationReadRepository, ITraineeshipApplicationWriteRepository
    {
        private readonly ILogService _logger;

        private readonly IMapper _mapper;

        private readonly CommonApplicationRepository _commonApplicationRepository;

        public TraineeshipApplicationRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.ApplicationsDb, "traineeships");
            _mapper = mapper;
            _logger = logger;
            _commonApplicationRepository = new CommonApplicationRepository(logger, Collection);
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete TraineeshipApplicationDetail with Id={0}", id);

            Collection.Remove(Query<MongoTraineeshipApplicationDetail>.EQ(o => o.Id, id));

            _logger.Debug("Deleted TraineeshipApplicationDetail with Id={0}", id);
        }

        public TraineeshipApplicationDetail Save(TraineeshipApplicationDetail entity)
        {
            _logger.Debug("Calling repository to save TraineeshipApplicationDetail Id={0}", entity.EntityId);

            var mongoEntity = _mapper.Map<TraineeshipApplicationDetail, MongoTraineeshipApplicationDetail>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved TraineeshipApplicationDetail to repository with Id={0}", entity.EntityId);

            return _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }

        public void UpdateApplicationNotes(Guid applicationId, string notes)
        {
            _logger.Debug("Calling repository to update traineeship application notes for application with Id={0}", applicationId);

            var result = Collection.Update(
                Query<MongoTraineeshipApplicationDetail>
                    .EQ(e => e.Id, applicationId),
                Update<MongoTraineeshipApplicationDetail>
                    .Set(e => e.Notes, notes)
                    .Set(e => e.DateUpdated, DateTime.UtcNow));

            if (result.Ok)
            {
                _logger.Debug("Called repository to update traineeship application notes for application with Id={0} successfully", applicationId);
            }
            else
            {
                var message = $"Call to repository to update traineeship application notes for application with Id={applicationId} failed! Exit code={result.Code}, error message={result.ErrorMessage}";
                _logger.Error(message);
                throw new Exception(message);
            }
        }

        public TraineeshipApplicationDetail Get(Guid id, bool errorIfNotFound)
        {
            _logger.Debug("Calling repository to get TraineeshipApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            string message;

            if (mongoEntity == null && errorIfNotFound)
            {
                message = string.Format("Unknown TraineeshipApplicationDetail with Id={0}", id);

                throw new CustomException(message, ApplicationErrorCodes.ApplicationNotFoundError);
            }

            message = mongoEntity == null ? "Found no TraineeshipApplicationDetail with Id={0}" : "Found TraineeshipApplicationDetail with Id={0}";

            _logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }

        public TraineeshipApplicationDetail Get(int legacyApplicationId)
        {
            _logger.Debug("Calling repository to get TraineeshipApplicationDetail with legacy Id={0}", legacyApplicationId);

            var mongoEntity = Collection.AsQueryable().FirstOrDefault(a => a.LegacyApplicationId == legacyApplicationId);

            var message = mongoEntity == null ? "Found no TraineeshipApplicationDetail with legacy Id={0}" : "Found TraineeshipApplicationDetail with legacy Id={0}";

            _logger.Debug(message, legacyApplicationId);

            return mongoEntity == null ? null : _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }

        public IList<TraineeshipApplicationSummary> GetForCandidate(Guid candidateId)
        {
            _logger.Debug("Calling repository to get TraineeshipApplicationSummary list for candidate with Id={0}", candidateId);

            // Get traineeship application summaries for the specified candidate, excluding any that are archived.
            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId)
                .ToArray();

            _logger.Debug("{0} MongoTraineeshipApplicationDetail items returned in collection for candidate with Id={1}", mongoApplicationDetailsList.Count(), candidateId);

            _logger.Debug("Mapping MongoTraineeshipApplicationDetail items to ApplicationSummary list for candidate with Id={0}", candidateId);

            var applicationDetailsList = _mapper
                .Map<MongoTraineeshipApplicationDetail[], IEnumerable<TraineeshipApplicationSummary>>(mongoApplicationDetailsList)
                .ToList();

            _logger.Debug("{0} ApplicationSummary items returned for candidate with Id={1}", applicationDetailsList.Count(), candidateId);

            return applicationDetailsList;
        }

        public IEnumerable<TraineeshipApplicationSummary> GetApplicationSummaries(int vacancyId)
        {
            _logger.Debug("Calling repository to get TraineeshipApplicationSummaries with VacancyId:{0}", vacancyId);

            var mongoEntities = Collection.Find(Query.EQ("Vacancy._id", vacancyId)).ToArray();

            _logger.Debug("Found {0} TraineeshipApplicationSummaries with VacancyId:{1}", mongoEntities.Count(), vacancyId);

            var applicationSummaries =
                _mapper.Map<IEnumerable<MongoTraineeshipApplicationDetail>, IEnumerable<TraineeshipApplicationSummary>>(
                    mongoEntities);

            return applicationSummaries;
        }

        public IEnumerable<Guid> GetApplicationsSubmittedOnOrBefore(DateTime dateApplied)
        {
            _logger.Debug("Calling repository to get traineeship applications submitted on or before: {0}", dateApplied);

            var applicationIds = Collection
                .AsQueryable()
                .Where(each => each.DateApplied <= dateApplied)
                .Select(each => each.EntityId);

            _logger.Debug("Called repository to get traineeship applications submitted on or before: {0}", dateApplied);

            return applicationIds;
        }

        public IReadOnlyDictionary<int, IApplicationCounts> GetCountsForVacancyIds(IEnumerable<int> vacancyIds)
        {
            return _commonApplicationRepository.GetCountsForVacancyIds(vacancyIds);
        }

        public TraineeshipApplicationDetail GetForCandidate(Guid candidateId, int vacancyId, bool errorIfNotFound = false)
        {
            _logger.Debug("Calling repository to get ApplicationSummary list for candidate with Id={0}", candidateId);

            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId && each.Vacancy.Id == vacancyId)
                .ToArray();

            if (mongoApplicationDetailsList.Length == 0 && errorIfNotFound)
            {
                var message = string.Format("No TraineeshipApplicationDetail found for candidateId={0} and vacancyId={1}", candidateId, vacancyId);

                throw new CustomException(message, ApplicationErrorCodes.ApplicationNotFoundError);
            }

            _logger.Debug("{0} MongoTraineeshipApplicationDetail items returned in collection for candidate with Id={1}", mongoApplicationDetailsList.Count(), candidateId);

            _logger.Debug("Mapping MongoTraineeshipApplicationDetail items to ApplicationSummary list for candidate with Id={0}", candidateId);

            var applicationDetailsList = _mapper.Map<MongoTraineeshipApplicationDetail[], IEnumerable<TraineeshipApplicationDetail>>(
                mongoApplicationDetailsList);

            var applicationDetails = applicationDetailsList as IList<TraineeshipApplicationDetail> ?? applicationDetailsList.ToList();

            _logger.Debug("{0} ApplicationSummary items returned for candidate with Id={1}", applicationDetails.Count(), candidateId);

            return applicationDetails
                .FirstOrDefault(); // we expect zero or 1
        }

        public TraineeshipApplicationDetail Get(Guid id)
        {
            _logger.Debug("Calling repository to get TraineeshipApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            var message = mongoEntity == null ? "Found no TraineeshipApplicationDetail with Id={0}" : "Found TraineeshipApplicationDetail with Id={0}";

            _logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoTraineeshipApplicationDetail, TraineeshipApplicationDetail>(mongoEntity);
        }
    }
}
