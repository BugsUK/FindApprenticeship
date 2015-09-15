namespace SFA.Apprenticeships.Infrastructure.Monitor.Repositories
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Applications;
    using MongoDB.Bson;

    public interface IApprenticeshipMetricsRepository
    {
        int GetApplicationCount();
        int GetSubmittedApplicationCount(DateTime submittedDateStart, DateTime submittedDateEnd);
        int GetApplicationStateCount(ApplicationStatuses applicationStatus);
        int GetApplicationCountPerCandidate();
        int GetApplicationStateCountPerCandidate(ApplicationStatuses applicationStatus);
        int GetCandidatesWithApplicationsInStatusCount(ApplicationStatuses applicationStatus, int minimumCount);
        IEnumerable<BsonDocument> GetApplicationStatusCounts();
        IEnumerable<Guid> GetCandidatesThatWouldHaveSeenTraineeshipPrompt();
        IEnumerable<BsonDocument> GetApplicationCountPerApprenticeship();
        IEnumerable<BsonDocument> GetUnsubmittedApplicationsCountPerCandidate(DateTime dateCreatedStart, DateTime dateCreatedEnd);
        IEnumerable<BsonDocument> GetSubmittedApplicationsCountPerCandidate(DateTime dateCreatedStart, DateTime dateCreatedEnd);
        BsonDocument GetAverageApplicationCountPerApprenticeship();
    }
}