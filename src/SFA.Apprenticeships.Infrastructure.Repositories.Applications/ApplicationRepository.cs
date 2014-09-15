﻿namespace SFA.Apprenticeships.Infrastructure.Repositories.Applications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Domain.Entities.Applications;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Mongo.Common;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using NLog;

    public class ApplicationRepository : GenericMongoClient<MongoApplicationDetail>, IApplicationReadRepository,
        IApplicationWriteRepository
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IMapper _mapper;

        public ApplicationRepository(
            IConfigurationManager configurationManager,
            IMapper mapper)
            : base(configurationManager, "Applications.mongoDB", "applications")
        {
            _mapper = mapper;
        }

        public void Delete(Guid id)
        {
            Logger.Debug("Calling repository to delete ApplicationDetail with Id={0}", id);

            Collection.Remove(Query<MongoApplicationDetail>.EQ(o => o.Id, id));

            Logger.Debug("Deleted ApplicationDetail with Id={0}", id);
        }

        public ApplicationDetail Save(ApplicationDetail entity)
        {
            Logger.Debug("Calling repository to save ApplicationDetail Id={0}, Status={1}", entity.EntityId, entity.Status);

            var mongoEntity = _mapper.Map<ApplicationDetail, MongoApplicationDetail>(entity);

            UpdateEntityTimestamps(mongoEntity);

            Collection.Save(mongoEntity);

            Logger.Debug("Saved ApplicationDetail to repository with Id={0}", entity.EntityId);

            return _mapper.Map<MongoApplicationDetail, ApplicationDetail>(mongoEntity);
        }

        public ApplicationDetail Get(Guid id, bool errorIfNotFound)
        {
            Logger.Debug("Calling repository to get ApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            string message;

            if (mongoEntity == null && errorIfNotFound)
            {
                 message = string.Format("Unknown ApplicationDetail with Id={0}", id);

                throw new CustomException(message, ErrorCodes.ApplicationNotFoundError);
            }

            message = mongoEntity == null ? "Found no ApplicationDetail with Id={0}" : "Found ApplicationDetail with Id={0}";

            Logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoApplicationDetail, ApplicationDetail>(mongoEntity);
        }

        public ApplicationDetail Get(Expression<Func<ApplicationDetail, bool>> filter)
        {
            //var item = Collection.AsQueryable().Where(filter);
            //todo: return the first application that matches the filter
            // .FirstOrDefault()
            throw new NotImplementedException();
        }

        public IList<ApplicationSummary> GetForCandidate(Guid candidateId)
        {
            Logger.Debug("Calling repository to get ApplicationSummary list for candidate with Id={0}", candidateId);
            // Get application summaries for the specified candidate, excluding any that are archived.
            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId)
                .ToArray();

            Logger.Debug("{0} MongoApplicationDetail items returned in collection for candidate with Id={1}", mongoApplicationDetailsList.Count(), candidateId);

            Logger.Debug("Mapping MongoApplicationDetail items to ApplicationSummary list for candidate with Id={0}", candidateId);

            var applicationDetailsList = _mapper
                .Map<MongoApplicationDetail[], IEnumerable<ApplicationSummary>>(mongoApplicationDetailsList)
                .ToList();

            Logger.Debug("{0} ApplicationSummary items returned for candidate with Id={1}", applicationDetailsList.Count(), candidateId);

            return applicationDetailsList;
        }

        public ApplicationDetail GetForCandidate(Guid candidateId, Func<ApplicationDetail, bool> filter)
        {
            Logger.Debug("Calling repository to get ApplicationSummary list for candidate with Id={0}", candidateId);

            var mongoApplicationDetailsList = Collection
                .AsQueryable()
                .Where(each => each.CandidateId == candidateId)
                .ToArray();

            Logger.Debug("{0} MongoApplicationDetail items returned in collection for candidate with Id={1}", mongoApplicationDetailsList.Count(), candidateId);

            Logger.Debug("Mapping MongoApplicationDetail items to ApplicationSummary list for candidate with Id={0}", candidateId);

            var applicationDetailsList = _mapper.Map<MongoApplicationDetail[], IEnumerable<ApplicationDetail>>(
                mongoApplicationDetailsList);

            var applicationDetails = applicationDetailsList as IList<ApplicationDetail> ?? applicationDetailsList.ToList();

            Logger.Debug("{0} ApplicationSummary items returned for candidate with Id={1}", applicationDetails.Count(), candidateId);

            return applicationDetails
                .Where(filter)
                .FirstOrDefault(); // we expect zero or 1
        }

        public ApplicationDetail Get(Guid id)
        {
            Logger.Debug("Calling repository to get ApplicationDetail with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            var message = mongoEntity == null ? "Found no ApplicationDetail with Id={0}" : "Found ApplicationDetail with Id={0}";

            Logger.Debug(message, id);

            return mongoEntity == null ? null : _mapper.Map<MongoApplicationDetail, ApplicationDetail>(mongoEntity);
        }

        public void ExpireOrWithdrawForCandidate(Guid candidateId, int vacancyId)
        {
            var applicationDetail = GetForCandidate(
                candidateId, applicationdDetail => applicationdDetail.Vacancy.Id == vacancyId);

            applicationDetail.Status = ApplicationStatuses.ExpiredOrWithdrawn;
            Save(applicationDetail);
        }

        protected override void Initialise()
        {
            Collection.CreateIndex(new IndexKeysBuilder().Ascending("CandidateId"), IndexOptions.SetUnique(false));
        }
    }
}
