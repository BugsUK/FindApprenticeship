namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Entities.Mongo;
    using Entities.Sql;

    public static class CandidateHistoryMappers
    {
        private const int CandidateHistoryEventIdStatusChange = 1;
        private const int CandidateHistoryEventIdReferral = 2;
        private const int CandidateHistoryEventIdNote = 3;

        private const int CandidateStatusTypeIdPreRegistered = 1;
        private const int CandidateStatusTypeIdActivated = 2;
        private const int CandidateStatusTypeIdRegistered = 3;
        private const int CandidateStatusTypeIdSuspended = 4;
        private const int CandidateStatusTypeIdPendingDelete = 5;
        private const int CandidateStatusTypeIdDeleted = 6;

        public static IList<CandidateHistory> MapCandidateHistory(this CandidateUser candidateUser, int candidateId, IDictionary<int, Dictionary<int, int>> candidateHistoryIds)
        {
            var user = candidateUser.User;

            var candidateHistory = new List<CandidateHistory>
            {
                //Pre Registration
                GetCandidateHistory(candidateId, user.DateCreated, CandidateHistoryEventIdStatusChange, CandidateStatusTypeIdPreRegistered, candidateHistoryIds)
            };

            if (user.Status >= 20 && (user.Status < 999 || candidateUser.User.ActivationDate.HasValue || candidateUser.Candidate.LegacyCandidateId != 0))
            {
                //Activation
                candidateHistory.Add(GetCandidateHistory(candidateId, user.ActivationDate ?? user.DateUpdated ?? user.DateCreated, CandidateHistoryEventIdStatusChange, CandidateStatusTypeIdActivated, candidateHistoryIds));
            }

            if (user.Status >= 20)
            {
                //Note
                candidateHistory.Add(GetCandidateHistory(candidateId, user.ActivationDate ?? user.DateUpdated ?? user.DateCreated, CandidateHistoryEventIdNote, 0, candidateHistoryIds));
            }

            return candidateHistory;
        }

        public static IList<IDictionary<string, object>> MapCandidateHistoryDictionary(this IList<CandidateHistory> candidateHistories)
        {
            return candidateHistories.Select(ah => ah.MapCandidateHistoryDictionary()).ToList();
        }

        public static IDictionary<string, object> MapCandidateHistoryDictionary(this CandidateHistory candidateHistory)
        {
            return new Dictionary<string, object>
            {
                {"CandidateHistoryId", candidateHistory.CandidateHistoryId},
                {"CandidateId", candidateHistory.CandidateId},
                {"CandidateHistoryEventTypeId", candidateHistory.CandidateHistoryEventTypeId},
                {"CandidateHistorySubEventTypeId", candidateHistory.CandidateHistorySubEventTypeId},
                {"EventDate", candidateHistory.EventDate},
                {"Comment", candidateHistory.Comment},
                {"UserName", candidateHistory.UserName},
            };
        }

        private static CandidateHistory GetCandidateHistory(int candidateId, DateTime candidateHistoryEventDate, int candidateHistoryEventTypeId, int candidateHistorySubEventTypeId, IDictionary<int, Dictionary<int, int>> candidateHistoryIds)
        {
            var candidateHistoryId = GetCandidateHistoryId(candidateId, candidateHistoryEventTypeId, candidateHistorySubEventTypeId, candidateHistoryIds);

            var comment = candidateHistoryEventTypeId == CandidateHistoryEventIdNote ? "NAS Exemplar registered Candidate." : null;
            var username = candidateHistorySubEventTypeId == CandidateStatusTypeIdPreRegistered ? "dummy" : "NAS Gateway";

            return new CandidateHistory
            {
                CandidateHistoryId = candidateHistoryId,
                CandidateId = candidateId,
                CandidateHistoryEventTypeId = candidateHistoryEventTypeId,
                CandidateHistorySubEventTypeId = candidateHistorySubEventTypeId,
                EventDate = candidateHistoryEventDate,
                Comment = comment,
                UserName = username
            };
        }

        private static int GetCandidateHistoryId(int candidateId, int candidateHistoryEventTypeId, int candidateHistoryEventSubTypeId, IDictionary<int, Dictionary<int, int>> candidateHistoryIds)
        {
            var candidateHistoryId = 0;

            if (candidateHistoryIds.ContainsKey(candidateId))
            {
                var ids = candidateHistoryIds[candidateId];
                if (ids.ContainsKey(candidateHistoryEventSubTypeId))
                {
                    candidateHistoryId = ids[candidateHistoryEventSubTypeId];
                }
            }

            return candidateHistoryId;
        }
    }
}