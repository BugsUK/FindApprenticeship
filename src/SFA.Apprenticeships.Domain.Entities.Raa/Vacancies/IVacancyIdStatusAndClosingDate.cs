namespace SFA.Apprenticeships.Domain.Entities.Raa.Vacancies
{
    using System;

    public interface IVacancyIdStatusAndClosingDate
    {
        int VacancyId { get; }

        int VacancyPartyId { get; }

        VacancyStatus VacancyStatus { get; }

        /// <summary></summary>
        /// <exception cref="InvalidOperationException">If VacancyStatus != Live</exception>
        DateTime ClosingDate { get; }
    }
}
