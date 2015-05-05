namespace SFA.Apprenticeships.Domain.Entities.Audit
{
    using System;

    public class AuditItem<T> : BaseEntity
    {
        public DateTime EventDate { get; set; }
        public string EventType { get; set; }
        public string Id1 { get; set; }
        public string Id2 { get; set; }
        public T Data { get; set; }
    }
}