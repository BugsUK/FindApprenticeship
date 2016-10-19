namespace SFA.Apprenticeships.Application.Application.Entities
{
    using System;

    public class TraineeshipApplicationUpdate
    {
        public TraineeshipApplicationUpdate(Guid applicationGuid)
        {
            ApplicationGuid = applicationGuid;
        }

        public Guid ApplicationGuid { get; private set; }
    }
}