namespace SFA.Apprenticeships.Application.Communications.Housekeeping
{
    using System;

    public class CommunicationHousekeepingRequest
    {
        public Guid CommunicationId { get; set; }
 
        public CommunicationTypes CommunicationType { get; set; }

        protected bool Equals(CommunicationHousekeepingRequest other)
        {
            return CommunicationId.Equals(other.CommunicationId);
        }

        public override bool Equals(object obj)
        {
            var other = obj as CommunicationHousekeepingRequest;

            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            return CommunicationId.GetHashCode();
        }
    }
}