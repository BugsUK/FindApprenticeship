namespace SFA.Apprenticeships.Domain.Entities.Communication
{
    using Applications;
    using System;

    public class ApplicationStatusAlert : BaseEntity
    {
        public Guid CandidateId { get; set; }

        public Guid ApplicationId { get; set; }

        public int VacancyId { get; set; }

        public string Title { get; set; }

        public string EmployerName { get; set; }

        public ApplicationStatuses Status { get; set; }

        public string UnsuccessfulReason { get; set; }

        public DateTime DateApplied { get; set; }

        public Guid? BatchId { get; set; }

        public DateTime? SentDateTime { get; set; }
        public DateTime UnSuccessfulDateTime { get; set; }
    }
}
