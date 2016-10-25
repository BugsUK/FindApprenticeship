namespace SFA.Apprenticeships.Application.Application.Entities
{
    using System;

    public class TraineeshipApplicationUpdate : ApplicationUpdate
    {
        public TraineeshipApplicationUpdate(Guid applicationGuid, ApplicationUpdateType applicationUpdateType) : base(applicationGuid, applicationUpdateType)
        {
        }
    }
}