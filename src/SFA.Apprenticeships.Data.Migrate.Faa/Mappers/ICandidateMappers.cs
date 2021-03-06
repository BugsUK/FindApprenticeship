﻿namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;
    using CandidateSummary = Entities.Sql.CandidateSummary;
    using Candidate = Entities.Sql.Candidate;

    public interface ICandidateMappers
    {
        CandidatePerson MapCandidatePerson(CandidateUser candidateUser, IDictionary<Guid, CandidateSummary> candidateSummaries, IDictionary<string, int> vacancyLocalAuthorities, IDictionary<int, int> localAuthorityCountyIds, IDictionary<int, int> schoolAttendedIds, bool anonymise);
        CandidateWithHistory MapCandidateWithHistory(CandidateUser candidateUser, IDictionary<Guid, CandidateSummary> candidateSummaries, IDictionary<string, int> vacancyLocalAuthorities, IDictionary<int, int> localAuthorityCountyIds, IDictionary<int, int> schoolAttendedIds, IDictionary<int, Dictionary<int, int>> candidateHistoryIds, bool anonymise);
        Dictionary<string, object> MapCandidateDictionary(Candidate candidate);
        Dictionary<string, object> MapPersonDictionary(Person person);
    }
}