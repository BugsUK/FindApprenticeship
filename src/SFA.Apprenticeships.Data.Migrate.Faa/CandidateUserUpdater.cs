namespace SFA.Apprenticeships.Data.Migrate.Faa
{
    using System;
    using System.Collections.Generic;
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

        private readonly Lazy<IDictionary<string, int>> _vacancyLocalAuthorities;
        private readonly Lazy<IDictionary<int, int>> _localAuthorityCountyIds;

        private readonly bool _anonymiseData;

        public CandidateUserUpdater(IConfigurationService configurationService, ILogService logService)
        {
            _logService = logService;

            var configuration = configurationService.Get<MigrateFromFaaToAvmsPlusConfiguration>();
            _targetDatabase = new GetOpenConnectionFromConnectionString(configuration.TargetConnectionString);

            _candidateMappers = new CandidateMappers(_logService);

            _candidateUserRepository = new CandidateUserRepository(configurationService, _logService);
            _userRepository = new UserRepository(configurationService, logService);

            _vacancyLocalAuthorities = new Lazy<IDictionary<string, int>>(() => new VacancyRepository(_targetDatabase).GetAllVacancyLocalAuthorities());
            _localAuthorityCountyIds = new Lazy<IDictionary<int, int>>(() => new LocalAuthorityRepository(_targetDatabase).GetLocalAuthorityCountyIds());

            _anonymiseData = configuration.AnonymiseData;
        }

        public void Create(Guid candidateGuid)
        {
            var candidate = _candidateUserRepository.GetCandidate(candidateGuid);
            var user = _userRepository.GetUser(candidateGuid);
            var candidateUser = new CandidateUser
            {
                Candidate = candidate,
                User = user
            };

            var candidateWithHistory = _candidateMappers.MapCandidateWithHistory(candidateUser, new Dictionary<Guid, CandidateSummary>(), _vacancyLocalAuthorities.Value, _localAuthorityCountyIds.Value, new Dictionary<int, int>(), new Dictionary<int, Dictionary<int, int>>(), _anonymiseData);

            Create(candidateWithHistory);
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
            throw new NotImplementedException();
        }

        public void Delete(Guid candidateGuid)
        {
            throw new NotImplementedException();
        }
    }
}