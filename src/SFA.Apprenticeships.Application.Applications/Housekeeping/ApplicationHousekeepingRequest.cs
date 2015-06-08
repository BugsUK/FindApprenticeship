namespace SFA.Apprenticeships.Application.Applications.Housekeeping
{
    using System;
    using Domain.Entities.Vacancies;

    public class ApplicationHousekeepingRequest
    {
        public Guid ApplicationId { get; set; }
        public VacancyType VacancyType { get; set; }

        protected bool Equals(ApplicationHousekeepingRequest other)
        {
            return ApplicationId.Equals(other.ApplicationId);
        }

        public override bool Equals(object obj)
        {
            var other = obj as ApplicationHousekeepingRequest;

            return other != null && Equals(other);
        }

        public override int GetHashCode()
        {
            return ApplicationId.GetHashCode();
        }
    }
}