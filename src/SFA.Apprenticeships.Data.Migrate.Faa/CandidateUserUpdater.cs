namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces;
    using Configuration;
    using Entities;
    using Entities.Mongo;
    using Infrastructure.Repositories.Sql.Common;
    using Mappers;
    using Repository.Mongo;
    using Repository.Sql;
    using CandidateSummary = Entities.Sql.CandidateSummary;

    public class CandidateUserUpdater : ICandidateUserUpdater
    {
        private readonly ILogService _logService;
        private readonly ICandidateMappers _candidateMappers;
        private readonly IGetOpenConnection _targetDatabase;

        private readonly CandidateUserRepository _candidateUserRepository;
        private readonly UserRepository _userRepository;
        private readonly CandidateRepository _candidateRepository;
        private readonly SchoolAttendedRepository _schoolAttendedRepository;
        private readonly CandidateHistoryRepository _candidateHistoryRepository;
        private readonly ApplicationRepository _applicationRepository;

        private readonly Lazy<IDictionary<string, int>> _vacancyLocalAuthorities;
        private readonly Lazy<IDictionary<int, int>> _localAuthorityCountyIds;

        private readonly bool _anonymiseData;

        public CandidateUserUpdater(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;

            var configuration = configurationService.Get<MigrateFromFaaToAvmsPlusConfiguration>();
            _targetDatabase = new GetOpenConnectionFromConnectionString(configuration.TargetConnectionString);

            _candidateMappers = new CandidateMappers(logService);

            _candidateUserRepository = new CandidateUserRepository(configurationService, logService);
            _userRepository = new UserRepository(configurationService, logService);
            _candidateRepository = new CandidateRepository(_targetDatabase);
            _schoolAttendedRepository = new SchoolAttendedRepository(_targetDatabase);
            _candidateHistoryRepository = new CandidateHistoryRepository(_targetDatabase);
            _applicationRepository = new ApplicationRepository(_targetDatabase);

            _vacancyLocalAuthorities = new Lazy<IDictionary<string, int>>(() => new VacancyRepository(_targetDatabase).GetAllVacancyLocalAuthorities());
            _localAuthorityCountyIds = new Lazy<IDictionary<int, int>>(() => new LocalAuthorityRepository(_targetDatabase).GetLocalAuthorityCountyIds());

            _anonymiseData = configuration.AnonymiseData;
        }

        public void Create(Guid candidateGuid)
        {
            var candidateUser = GetCandidateUser(candidateGuid);

            var candidateWithHistory = _candidateMappers.MapCandidateWithHistory(candidateUser, new Dictionary<Guid, CandidateSummary>(), _vacancyLocalAuthorities.Value, _localAuthorityCountyIds.Value, new Dictionary<int, int>(), new Dictionary<int, Dictionary<int, int>>(), _anonymiseData);

            Create(candidateWithHistory);
        }

        private CandidateUser GetCandidateUser(Guid candidateGuid)
        {
            var candidate = _candidateUserRepository.GetCandidate(candidateGuid);
            var user = _userRepository.GetUser(candidateGuid);
            var candidateUser = new CandidateUser
            {
                Candidate = candidate,
                User = user
            };
            return candidateUser;
        }

        private void Create(CandidateWithHistory candidateWithHistory)
        {
            //Insert new person record to match with candidate record
            var personId = (int)_targetDatabase.Insert(candidateWithHistory.CandidatePerson.Person);
            candidateWithHistory.CandidatePerson.Candidate.PersonId = personId;

            //Insert candidate records
            var candidateId = (int)_targetDatabase.Insert(candidateWithHistory.CandidatePerson.Candidate);

            var schoolAttended = candidateWithHistory.CandidatePerson.SchoolAttended;
            if (schoolAttended != null)
            {
                //Insert school attended
                schoolAttended.CandidateId = candidateId;
                _targetDatabase.Insert(schoolAttended);
            }

            //Insert new candidate history records
            foreach (var candidateHistory in candidateWithHistory.CandidateHistory)
            {
                candidateHistory.CandidateId = candidateId;
                _targetDatabase.Insert(candidateHistory);
            }
        }

        public void Update(Guid candidateGuid)
        {
            var candidateUser = GetCandidateUser(candidateGuid);

            var candidateSummaries = _candidateRepository.GetCandidateSummariesByGuid(new[] {candidateUser.Candidate.Id});
            var schoolAttendedIds = _schoolAttendedRepository.GetSchoolAttendedIdsByCandidateIds(candidateSummaries.Values.Select(cs => cs.CandidateId));
            var candidateHistoryIds = _candidateHistoryRepository.GetCandidateHistoryIdsByCandidateIds(candidateSummaries.Values.Select(cs => cs.CandidateId).Distinct());
            var candidateWithHistory = _candidateMappers.MapCandidateWithHistory(candidateUser, candidateSummaries, _vacancyLocalAuthorities.Value, _localAuthorityCountyIds.Value, schoolAttendedIds, candidateHistoryIds, _anonymiseData);

            Update(candidateWithHistory);
        }

        private void Update(CandidateWithHistory candidateWithHistory)
        {
            //update existing person
            _targetDatabase.UpdateSingle(candidateWithHistory.CandidatePerson.Person);

            //update existing candidate
            _targetDatabase.UpdateSingle(candidateWithHistory.CandidatePerson.Candidate);

            //Insert new candidate history records
            foreach (var candidateHistory in candidateWithHistory.CandidateHistory.Where(a => a.CandidateHistoryId == 0))
            {
                _targetDatabase.Insert(candidateHistory);
            }

            var schoolAttended = candidateWithHistory.CandidatePerson.SchoolAttended;
            if (schoolAttended != null)
            {
                if (schoolAttended.SchoolAttendedId == 0)
                {
                    //Insert school attended if not already present
                    _targetDatabase.Insert(schoolAttended);
                }
                else
                {
                    //Otherwise update
                    _targetDatabase.UpdateSingle(schoolAttended);
                }
            }

            //Update existing candidate history records
            foreach (var candidateHistory in candidateWithHistory.CandidateHistory.Where(a => a.CandidateHistoryId != 0))
            {
                _targetDatabase.UpdateSingle(candidateHistory);
            }
        }

        public void Delete(Guid candidateGuid)
        {
            var candidateGuids = _candidateRepository.GetCandidateIdsByGuid(new[] { candidateGuid });

            _logService.Warn($"Deleting candidate and related application records for candidates with ids: {string.Join(",", candidateGuids.Values)}");

            _applicationRepository.DeleteByCandidateId(candidateGuids.Values);
            _candidateRepository.DeleteByCandidateGuid(candidateGuids.Keys);
        }
    }
}