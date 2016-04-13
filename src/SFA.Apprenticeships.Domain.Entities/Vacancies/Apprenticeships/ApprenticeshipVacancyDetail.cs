namespace SFA.Apprenticeships.Domain.Entities.Vacancies.Apprenticeships
{
    using System;

    public class ApprenticeshipVacancyDetail : VacancyDetail
    {
        #region Vacancy

        public ApprenticeshipLevel ApprenticeshipLevel { get; set; }

        public ApprenticeshipLocationType VacancyLocationType { get; set; }
    }
}
