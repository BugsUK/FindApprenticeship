namespace SFA.Apprenticeships.Application.Vacancies.Entities
{
    public class VacancyEligibleForClosure
    {
        public VacancyEligibleForClosure(int vacancyId)
        {
            VacancyId = vacancyId;
        }

        public int VacancyId { get; private set; }
    }
}
