namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using Application.Vacancy;
    using Domain.Entities.Vacancies.Traineeships;

    public class TraineeshipVacancyDataProvider : IVacancyDataProvider<TraineeshipVacancyDetail>
    {
        public TraineeshipVacancyDetail GetVacancyDetails(int vacancyId, bool errorIfNotFound = false)
        {
            throw new System.NotImplementedException();
        }
    }
}