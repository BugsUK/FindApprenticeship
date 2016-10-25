namespace SFA.Apprenticeships.Application.Application.Entities
{
    using System;

    public abstract class ApplicationUpdate
    {
        protected ApplicationUpdate(Guid applicationGuid, ApplicationUpdateType applicationUpdateType)
        {
            ApplicationGuid = applicationGuid;
            ApplicationUpdateType = applicationUpdateType;
        }

        public Guid ApplicationGuid { get; private set; }

        public ApplicationUpdateType ApplicationUpdateType { get; private set; }
    }
}