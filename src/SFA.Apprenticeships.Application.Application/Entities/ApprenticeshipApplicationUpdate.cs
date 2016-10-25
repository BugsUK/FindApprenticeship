namespace SFA.Apprenticeships.Application.Application.Entities
{
    using System;

    public class ApprenticeshipApplicationUpdate : ApplicationUpdate
    {
        public ApprenticeshipApplicationUpdate(Guid applicationGuid, ApplicationUpdateType applicationUpdateType) : base(applicationGuid, applicationUpdateType)
        {
        }
    }
}