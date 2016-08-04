namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System;

    public interface IMinimalVacancyDetails
    {
        int VacancyId { get; }

        int OwnerPartyId { get; }

        VacancyStatus Status { get; }

        /// <summary></summary>
        /// <exception cref="InvalidOperationException">If VacancyStatus isn't Live, Closed or Completed</exception>
        DateTime LiveClosingDate { get; }

        DateTime SyntheticUpdatedDateTime { get; }

        VacancyType VacancyType { get; }

        string EmployerName { get; set; }

        string Title { get; set; }
    }
}
