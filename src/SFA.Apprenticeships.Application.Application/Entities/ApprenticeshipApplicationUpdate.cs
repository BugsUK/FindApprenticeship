namespace SFA.Apprenticeships.Application.Application.Entities
{
    using System;

    public class ApprenticeshipApplicationUpdate
    {
        public ApprenticeshipApplicationUpdate(Guid applicationGuid)
        {
            ApplicationGuid = applicationGuid;
        }

        public Guid ApplicationGuid { get; private set; }
    }
}