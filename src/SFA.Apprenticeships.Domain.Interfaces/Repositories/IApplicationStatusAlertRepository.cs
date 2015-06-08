namespace SFA.Apprenticeships.Domain.Interfaces.Repositories
{
    using System;
    using System.Collections.Generic;
    using Entities.Communication;

    public interface IApplicationStatusAlertRepository
    {
        ApplicationStatusAlert Get(Guid id);

        void Save(ApplicationStatusAlert alert);

        void Delete(ApplicationStatusAlert alert);

        List<ApplicationStatusAlert> GetForApplication(Guid applicationId);

        Dictionary<Guid, List<ApplicationStatusAlert>> GetCandidatesDailyDigest();

        IEnumerable<Guid> GetAlertsCreatedOnOrBefore(DateTime dateTime);
    }
}