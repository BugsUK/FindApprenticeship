namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Candidates
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Entities.Candidates;
    using Domain.Entities.Exceptions;
    using Domain.Interfaces.Repositories;
    using Entities;
    using Common;
    using Common.Configuration;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    using SFA.Apprenticeships.Application.Interfaces;
    using SFA.Infrastructure.Interfaces;
    using CandidateErrorCodes = Application.Interfaces.Candidates.ErrorCodes;

    public class CandidateRepository : GenericMongoClient<MongoCandidate>, ICandidateReadRepository,
        ICandidateWriteRepository
    {
        private readonly ILogService _logger;
        private readonly IMapper _mapper;

        public CandidateRepository(IConfigurationService configurationService, IMapper mapper, ILogService logger)
        {
            var config = configurationService.Get<MongoConfiguration>();
            Initialise(config.CandidatesDb, "candidates");
            _mapper = mapper;
            _logger = logger;
        }

        public Candidate Get(Guid id)
        {
            _logger.Debug("Calling repository to get candidate with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            LogOutcome("Id", id, mongoEntity);

            return CandidateOrNull(mongoEntity);
        }

        public Candidate Get(Guid id, bool errorIfNotFound)
        {
            _logger.Debug("Calling repository to get candidate with Id={0}", id);

            var mongoEntity = Collection.FindOneById(id);

            if (mongoEntity == null && errorIfNotFound)
            {
                var message = string.Format("Unknown candidate with Id={0}", id);
                _logger.Debug(message);

                throw new CustomException(message, CandidateErrorCodes.CandidateNotFoundError);
            }

            LogOutcome("Id", id, mongoEntity);

            return CandidateOrNull(mongoEntity);
        }

        public IEnumerable<Candidate> Get(string username, bool errorIfNotFound = true)
        {
            _logger.Debug("Calling repository to get candidates with username={0}", username);

            var candidates =
                Collection.Find(Query<MongoCandidate>.EQ(o => o.RegistrationDetails.EmailAddress, username.ToLower()))
                .Select(e => _mapper.Map<MongoCandidate, Candidate>(e))
                .ToList();

            if (candidates.Count == 0 && errorIfNotFound)
            {
                var message = string.Format("No candidates found with username={0}", username);
                _logger.Debug(message);

                throw new CustomException(message, CandidateErrorCodes.CandidateNotFoundError);
            }

            _logger.Debug(string.Format("Found {0} candidates with username={1}", candidates.Count, username));

            return candidates;
        }

        public Candidate Get(int legacyCandidateId, bool errorIfNotFound = true)
        {
            _logger.Debug("Calling repository to get candidate with legacyCandidateId={0}", legacyCandidateId);

            var mongoEntity = Collection.FindOne(Query<MongoCandidate>.EQ(o => o.LegacyCandidateId, legacyCandidateId));

            if (mongoEntity == null && errorIfNotFound)
            {
                var message = string.Format("Unknown candidate with legacyCandidateId={0}", legacyCandidateId);
                _logger.Debug(message);

                throw new CustomException(message, CandidateErrorCodes.CandidateNotFoundError);
            }

            LogOutcome(legacyCandidateId, mongoEntity);

            return CandidateOrNull(mongoEntity);
        }

        public IEnumerable<Candidate> GetAllCandidatesWithPhoneNumber(string phoneNumber, bool errorIfNotFound = true)
        {
            _logger.Debug("Calling repository to get candidates with PhoneNumber={0}", phoneNumber);

            var candidates = 
                Collection.Find(Query<MongoCandidate>.EQ(o => o.RegistrationDetails.PhoneNumber, phoneNumber))
                .Select(e => _mapper.Map<MongoCandidate, Candidate>(e))
                .ToList();

            if (candidates.Count == 0 && errorIfNotFound)
            {
                var message = string.Format("No candidates found with PhoneNumber={0}", phoneNumber);
                _logger.Debug(message);

                throw new CustomException(message, CandidateErrorCodes.CandidateNotFoundError);
            }

            _logger.Debug(string.Format("Found {0} candidates with PhoneNumber={1}", candidates.Count, phoneNumber));

            return candidates;
        }

        public IEnumerable<Guid> GetCandidatesWithPendingMobileVerification()
        {
            _logger.Debug("Calling repository to get candidates with pending mobile verification");

            var query = Query.And(
                Query<MongoCandidate>.EQ(o => o.CommunicationPreferences.VerifiedMobile, false),
                Query<MongoCandidate>.NE(o => o.CommunicationPreferences.MobileVerificationCode, null),
                Query<MongoCandidate>.NE(o => o.CommunicationPreferences.MobileVerificationCode, string.Empty));

            var candidateIds = Collection
                .Find(query)
                .Select(u => u.Id)
                .ToList();

            _logger.Debug(string.Format("Found {0} candidates with pending mobile verification", candidateIds.Count));

            return candidateIds;
        }

        public Candidate GetBySubscriberId(Guid subscriberId, bool errorIfNotFound = true)
        {
            _logger.Debug("Calling repository to get candidate with subscriberId='{0}'", subscriberId);

            var mongoEntity = Collection.FindOne(Query<MongoCandidate>.EQ(o => o.SubscriberId, subscriberId));

            if (mongoEntity == null && errorIfNotFound)
            {
                var message = string.Format("Unknown candidate with subscriberId='{0}'", subscriberId);
                _logger.Debug(message);

                throw new CustomException(message, CandidateErrorCodes.CandidateNotFoundError);
            }

            LogOutcome("SubscriberId", subscriberId, mongoEntity);

            return CandidateOrNull(mongoEntity);
        }

        public List<CandidateSummary> SearchCandidates(CandidateSearchRequest request)
        {
            _logger.Debug("Calling repository to find candidates matching search request {0}", request);

            if (string.IsNullOrEmpty(request.FirstName) && string.IsNullOrEmpty(request.LastName) && request.DateOfBirth == null && string.IsNullOrEmpty(request.Postcode))
            {
                throw new ArgumentException("You must specify at least one search parameter");
            }

            var query = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(request.FirstName))
            {
                query.Add(Query<MongoCandidate>.Matches(c => c.RegistrationDetails.FirstName, $@"/^{request.FirstName}$/i"));
            }
            if (!string.IsNullOrEmpty(request.LastName))
            {
                query.Add(Query<MongoCandidate>.Matches(c => c.RegistrationDetails.LastName, $@"/^{request.LastName}$/i"));
            }
            if (request.DateOfBirth.HasValue)
            {
                query.Add(Query<MongoCandidate>.EQ(c => c.RegistrationDetails.DateOfBirth, request.DateOfBirth));
            }
            if (!string.IsNullOrEmpty(request.Postcode))
            {
                query.Add(Query<MongoCandidate>.Matches(c => c.RegistrationDetails.Address.Postcode, $@"/^{request.Postcode}$/i"));
            }

            var candidates = Collection
                .Find(Query.And(query))
                .SetFields(Fields<MongoCandidate>.Include(c => c.RegistrationDetails))
                .Select(c => _mapper.Map<MongoCandidate, CandidateSummary>(c))
                .ToList();

            _logger.Debug("Found {1} candidates matching search request {0}", request, candidates.Count);

            return candidates;
        }

        public void Delete(Guid id)
        {
            _logger.Debug("Calling repository to delete candidate with Id={0}", id);

            Collection.Remove(Query<MongoCandidate>.EQ(o => o.Id, id));

            _logger.Debug("Deleted candidate with Id={0}",id);
        }

        public Candidate Save(Candidate entity)
        {
            _logger.Debug("Calling repository to save candidate with Id={0}, FirstName={1}, EmailAddress={2}", entity.EntityId, entity.RegistrationDetails.FirstName, entity.RegistrationDetails.EmailAddress);

            UpdateEntityTimestamps(entity);

            var mongoEntity = _mapper.Map<Candidate, MongoCandidate>(entity);

            Collection.Save(mongoEntity);

            _logger.Debug("Saved candidate to repository with Id={0}", entity.EntityId);

            return _mapper.Map<MongoCandidate, Candidate>(mongoEntity);
        }

        private void LogOutcome(string fieldName, Guid id, MongoCandidate mongoEntity)
        {
            var message = mongoEntity == null ? "Found no candidate with {0}={1}" : "Found candidate with Id={0}";

            _logger.Debug(message, fieldName, id);
        }

        private void LogOutcome(string username, MongoCandidate mongoEntity)
        {
            var message = mongoEntity == null ? "Found no candidate with username={0}" : "Found candidate with username={0}";

            _logger.Debug(message, username);
        }

        private void LogOutcome(int legacyCandidateId, MongoCandidate mongoEntity)
        {
            var message = mongoEntity == null ? "Found no candidate with legacyCandidateId={0}" : "Found candidate with legacyCandidateId={0}";

            _logger.Debug(message, legacyCandidateId);
        }

        private Candidate CandidateOrNull(MongoCandidate mongoEntity)
        {
            if (mongoEntity == null) return null;

            var entity = _mapper.Map<MongoCandidate, Candidate>(mongoEntity);

            return entity;
        }
    }
}