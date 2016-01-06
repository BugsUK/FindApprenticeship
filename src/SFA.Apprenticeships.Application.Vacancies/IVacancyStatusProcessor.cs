namespace SFA.Apprenticeships.Application.Vacancies
{
    using System;
    using Entities;

    public interface IVacancyStatusProcessor
    {
        /// <summary>
        /// This should queue up the vacancies that expire by the deadline.
        /// </summary>
        /// <param name="deadline"></param>
        void QueueVacanciesForClosure(DateTime deadline);

        /// <summary>
        /// Close the vacancy
        /// </summary>
        /// <param name="vacancyEligibleForClosure"></param>
        void ProcessVacancyClosure(VacancyEligibleForClosure vacancyEligibleForClosure);
    }
}