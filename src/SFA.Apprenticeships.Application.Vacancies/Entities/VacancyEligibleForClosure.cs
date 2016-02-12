namespace SFA.Apprenticeships.Application.Vacancies.Entities
{
    public class VacancyEligibleForClosure
    {
        public int EntityId { get; private set; }

        private VacancyEligibleForClosure()
        {
        }

        public VacancyEligibleForClosure(int entityId)
        {
            EntityId = entityId;
        }
    }
}
